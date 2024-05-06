using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
using static KinoPr.Genre;

namespace KinoPr
{
    /// <summary>
    /// Логика взаимодействия для AddFilm.xaml
    /// </summary>
    public partial class AddFilm : Page
    {
        public MainWindow mainWindow;
        public AddFilm(MainWindow main)
        {
            InitializeComponent();
            mainWindow = main;
            LoadGenre();
        }
        private async Task LoadGenre()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/genre");

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        List<Genre> genres = JsonConvert.DeserializeObject<List<Genre>>(responseBody);
                        GenreResponse genreResponse = new GenreResponse { Data = genres };
                        genre.ItemsSource = genreResponse.Data;
                        genre.DisplayMemberPath = "Name";
                        genre.SelectedValuePath = "Id";
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при загрузке Жанров: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке Жанров: " + ex.Message);
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AdminPage(mainWindow));
        }
        private void ChangePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;
                // Отобразить выбранное изображение в элементе Image
                filmImage.Source = new BitmapImage(new Uri(imagePath));

            }
        }
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создаем объект Movie для добавления данных
                Movie newMovie = new Movie
                {
                    Name = title.Text,
                    GenreId = (int)genre.SelectedValue,
                    Duration = duration.Text,
                    Year = int.Parse(year.Text),
                    Description = description.Text,
                    Director = director.Text,
                    Country = country.Text
                };

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.currentUser.api_token);
                    MultipartFormDataContent multiContent = new MultipartFormDataContent();

                    // Добавляем данные формы в мультипарт контент
                    multiContent.Add(new StringContent(newMovie.Name), "name");
                    multiContent.Add(new StringContent(newMovie.Duration), "duration");
                    multiContent.Add(new StringContent(newMovie.Description), "description");
                    multiContent.Add(new StringContent(newMovie.Year.ToString()), "year");
                    multiContent.Add(new StringContent(newMovie.Country), "country");
                    multiContent.Add(new StringContent(newMovie.Director), "director");
                    multiContent.Add(new StringContent(newMovie.GenreId.ToString()), "genre_id");

                    // Если есть новое изображение, добавляем его
                    if (filmImage.Source != null)
                    {
                        BitmapSource bitmapSource = (BitmapSource)filmImage.Source;
                        byte[] imageData;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            BitmapEncoder encoder = new PngBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                            encoder.Save(ms);
                            imageData = ms.ToArray();
                        }
                        ByteArrayContent imageContent = new ByteArrayContent(imageData);
                        multiContent.Add(imageContent, "photo", "film_image.png");
                    }

                    HttpResponseMessage response = await client.PostAsync("http://motov-ae.tepk-it.ru/api/film", multiContent);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Фильм успешно добавлен!");
                        // Переходим на страницу администратора после успешного добавления
                        FrameManager.MainFrame.Navigate(new AdminPage(mainWindow));
                    }
                    else
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Ошибка при добавлении фильма: " + response.StatusCode + ", " + responseBody);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении фильма: " + ex.Message);
            }
        }

    }
}
