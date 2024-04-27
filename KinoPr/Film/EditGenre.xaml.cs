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

namespace KinoPr
{
    /// <summary>
    /// Логика взаимодействия для AddGenre.xaml
    /// </summary>
    public partial class EditGenre : Page
    {
        public MainWindow mainWindow;

        private Genre selectedGenre;
        
        public EditGenre(Genre selectedGenre, MainWindow main)
        {
            InitializeComponent();
            mainWindow = main;
            this.selectedGenre = selectedGenre;
            genreName.Text = selectedGenre.Name;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AdminPage(mainWindow));
        }
        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создаем объект Genre для обновления данных
                Genre updatedGenre = new Genre
                {
                    Id = selectedGenre.Id,
                    Name = genreName.Text
                };

                using (HttpClient client = new HttpClient())
                {
                    MultipartFormDataContent multiContent = new MultipartFormDataContent();

                    // Добавляем данные формы в мультипарт контент
                    multiContent.Add(new StringContent(updatedGenre.Name), "name");

                    HttpResponseMessage response = await client.PostAsync($"http://motov-ae.tepk-it.ru/api/genre/{selectedGenre.Id}", multiContent);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Жанр успешно обновлен!");
                        // Переходим на страницу администратора после успешного обновления
                        FrameManager.MainFrame.Navigate(new AdminPage(mainWindow));
                    }
                    else
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Ошибка при обновлении жанра: " + response.StatusCode + ", " + responseBody);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении жанра: " + ex.Message);
            }
        }



    }
}
