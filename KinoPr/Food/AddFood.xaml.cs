using Microsoft.Win32;
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
using System.Globalization;

namespace KinoPr
{
    /// <summary>
    /// Логика взаимодействия для AddFood.xaml
    /// </summary>
    public partial class AddFood : Page
    {
        public MainWindow mainWindow;

        public AddFood(MainWindow main)
        {
            InitializeComponent();
            mainWindow = main;
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создаем объект продукта для добавления
                Product newProduct = new Product
                {
                    Name = name.Text,
                    Price = Convert.ToDecimal(price.Text),
                    Mass = Convert.ToInt32(mass.Text)
                };

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.currentUser.api_token);
                    MultipartFormDataContent multiContent = new MultipartFormDataContent();

                    // Добавляем данные формы в мультипарт контент
                    multiContent.Add(new StringContent(newProduct.Name), "name");
                    multiContent.Add(new StringContent(newProduct.Price.ToString(CultureInfo.InvariantCulture)), "price");
                    multiContent.Add(new StringContent(newProduct.Mass.ToString()), "mass");

                    // Если есть выбранное изображение, добавляем его
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

                    HttpResponseMessage response = await client.PostAsync("http://motov-ae.tepk-it.ru/api/product", multiContent);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Продукт успешно добавлен!");
                        // Переходим на страницу администратора после успешного добавления
                        if (Data.currentUser.role_id == 3)
                        {
                            FrameManager.MainFrame.Navigate(new AdminPage(mainWindow));

                        }
                        else
                        {
                            FrameManager.MainFrame.Navigate(new ManagerPage(mainWindow));
                        }
                    }
                    else
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Ошибка при добавлении продукта: " + response.StatusCode + ", " + responseBody);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении продукта: " + ex.Message);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Data.currentUser.role_id == 3)
            {
                FrameManager.MainFrame.Navigate(new AdminPage(mainWindow));

            }
            else
            {
                FrameManager.MainFrame.Navigate(new ManagerPage(mainWindow));
            }
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
    }

}
