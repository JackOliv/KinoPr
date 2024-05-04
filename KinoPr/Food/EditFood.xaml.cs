using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Globalization;

namespace KinoPr
{
    /// <summary>
    /// Логика взаимодействия для EditFood.xaml
    /// </summary>
    public partial class EditFood : Page
    {
        public MainWindow mainWindow;
        private Product selectedProduct;

        public EditFood(Product selectedProduct, MainWindow main)
        {
            InitializeComponent();
            this.selectedProduct = selectedProduct;
            mainWindow = main;
            name.Text = selectedProduct.Name;
            price.Text = selectedProduct.Price.ToString();
            mass.Text = selectedProduct.Mass.ToString();
            LoadImage();
        }

        private async void LoadImage()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/storage/" + selectedProduct.Photo);
                    if (response.IsSuccessStatusCode)
                    {
                        Stream stream = await response.Content.ReadAsStreamAsync();
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = stream;
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();

                        foodImage.Source = bitmapImage;
                    }
                    else
                    {
                        MessageBox.Show("Ошибка загрузки изображения: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки изображения: " + ex.Message);
            }
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new ManagerPage(mainWindow));
        }

        private void ChangePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;
                // Отобразить выбранное изображение в элементе Image
                foodImage.Source = new BitmapImage(new Uri(imagePath));
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                // Создаем объект продукта для обновления данных
                Product updatedProduct = new Product
                {
                    Id = selectedProduct.Id,
                    Name = name.Text,
                    Price = Convert.ToDecimal(price.Text),
                    Mass = Convert.ToInt32(mass.Text),
                    Photo = selectedProduct.Photo // Пока не обновляем изображение продукта
                };

                using (HttpClient client = new HttpClient())
                {
                    MultipartFormDataContent multiContent = new MultipartFormDataContent();

                    // Добавляем данные формы в мультипарт контент
                    multiContent.Add(new StringContent(updatedProduct.Name), "name");
                    multiContent.Add(new StringContent(updatedProduct.Price.ToString(CultureInfo.InvariantCulture)), "price");
                    multiContent.Add(new StringContent(updatedProduct.Mass.ToString()), "mass");

                    // Если есть новое изображение, добавляем его
                    if (foodImage.Source != null)
                    {
                        BitmapSource bitmapSource = (BitmapSource)foodImage.Source;
                        byte[] imageData;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            BitmapEncoder encoder = new PngBitmapEncoder(); // Используйте нужный тип кодирования в зависимости от типа изображения
                            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                            encoder.Save(ms);
                            imageData = ms.ToArray();
                        }
                        ByteArrayContent imageContent = new ByteArrayContent(imageData);
                        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/png"); // Укажите правильный Content-Type
                        multiContent.Add(imageContent, "photo", "product_image.png");
                    }

                    HttpResponseMessage response = await client.PostAsync($"http://motov-ae.tepk-it.ru/api/product/{selectedProduct.Id}", multiContent);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Продукт успешно обновлен!");
                        // Переходим на страницу администратора после успешного обновления
                        FrameManager.MainFrame.Navigate(new ManagerPage(mainWindow));
                    }
                    else
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Ошибка при обновлении продукта: " + response.StatusCode + ", " + responseBody);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении продукта: " + ex.Message);
            }
        }
    }
}
