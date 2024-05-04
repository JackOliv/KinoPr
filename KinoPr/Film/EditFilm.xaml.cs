using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
using static KinoPr.Genre;
using System.IO;
using System.Net.Http.Headers;

namespace KinoPr
{
    /// <summary>
    /// Логика взаимодействия для EditFilm.xaml
    /// </summary>
    public partial class EditFilm : Page
    {
        public MainWindow mainWindow;
        private Movie selectedMovie;
        public EditFilm(Movie selectedMovie, MainWindow main)
        {
            InitializeComponent();
            this.selectedMovie = selectedMovie;
            mainWindow = main;
            LoadGenre();
            genre.DisplayMemberPath = "Name";
            genre.SelectedValuePath = "Id";
            title.Text = selectedMovie.Name;
            duration.Text = selectedMovie.Duration;
            year.Text = selectedMovie.Year.ToString();
            description.Text = selectedMovie.Description;
            director.Text = selectedMovie.Director;
            country.Text = selectedMovie.Country;
            LoadImage();
        }
        private async void LoadImage()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.currentUser.api_token);
                    HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/storage/" + selectedMovie.Photo);
                    if (response.IsSuccessStatusCode)
                    {
                        Stream stream = await response.Content.ReadAsStreamAsync();
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = stream;
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();

                        filmImage.Source = bitmapImage;
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
        private async Task LoadGenre()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.currentUser.api_token);
                    HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/genre");

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        List<Genre> genres = JsonConvert.DeserializeObject<List<Genre>>(responseBody);
                        Genre selectedGenre = genres.FirstOrDefault(g => g.Name == selectedMovie.GenreName);
                        if (selectedGenre != null)
                        {
                            selectedMovie.GenreId = selectedGenre.Id;
                            genre.ItemsSource = genres;
                            genre.SelectedValue = selectedMovie.GenreId; // Устанавливаем GenreId в качестве выбранного значения
                        }
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

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверяем, что все необходимые поля заполнены
                if (string.IsNullOrWhiteSpace(title.Text) || genre.SelectedValue == null || string.IsNullOrWhiteSpace(duration.Text) ||
                    string.IsNullOrWhiteSpace(year.Text) || string.IsNullOrWhiteSpace(description.Text) ||
                    string.IsNullOrWhiteSpace(director.Text) || string.IsNullOrWhiteSpace(country.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля");
                    return;
                }

                // Создаем объект фильма для обновления данных
                Movie updatedMovie = new Movie
                {
                    Id = selectedMovie.Id,
                    Name = title.Text,
                    GenreId = (int)genre.SelectedValue,
                    Duration = duration.Text,
                    Year = int.Parse(year.Text),
                    Description = description.Text,
                    Director = director.Text,
                    Country = country.Text,
                    Photo = selectedMovie.Photo // Пока не обновляем изображение фильма
                };

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.currentUser.api_token);
                    MultipartFormDataContent multiContent = new MultipartFormDataContent();

                    // Добавляем данные формы в мультипарт контент
                    multiContent.Add(new StringContent(updatedMovie.Name), "name");
                    multiContent.Add(new StringContent(updatedMovie.Duration), "duration");
                    multiContent.Add(new StringContent(updatedMovie.Description), "description");
                    multiContent.Add(new StringContent(updatedMovie.Year.ToString()), "year");
                    multiContent.Add(new StringContent(updatedMovie.Country), "country");
                    multiContent.Add(new StringContent(updatedMovie.Director), "director");
                    multiContent.Add(new StringContent(updatedMovie.GenreId.ToString()), "genre_id");

                    // Если есть новое изображение, добавляем его
                    if (filmImage.Source != null)
                    {
                        BitmapSource bitmapSource = (BitmapSource)filmImage.Source;
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
                        multiContent.Add(imageContent, "photo", "film_image.png");
                    }

                    HttpResponseMessage response = await client.PostAsync($"http://motov-ae.tepk-it.ru/api/film/{selectedMovie.Id}", multiContent);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Фильм успешно обновлен!");
                        // Переходим на страницу администратора после успешного обновления
                        FrameManager.MainFrame.Navigate(new AdminPage(mainWindow));
                    }
                    else
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Ошибка при обновлении фильма: " + response.StatusCode + ", " + responseBody);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении фильма: " + ex.Message);
            }
        }




    }
}
