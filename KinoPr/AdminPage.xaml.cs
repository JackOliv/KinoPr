using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public MainWindow mainWindow;
        public AdminPage(MainWindow main)
        {
            InitializeComponent();
            mainWindow = main;
        }


        //Список фильмов
        private void AddMoviesButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AddFilm(mainWindow));
        }
        private void EditMoviesButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new EditFilm(mainWindow));
        }
        private void DeleteMoviesButton_Click(object sender, RoutedEventArgs e)
        {
        }
        private void AddGenreButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AddGenre(mainWindow));
        }



        //Список сеансов
        private void AddSessionButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AddSession(mainWindow));
        }
        private void EditSessionButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new EditSession(mainWindow));
        }
        private void DeleteSessionButton_Click(object sender, RoutedEventArgs e)
        {

        }


        //Список пользователей
        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AddUser(mainWindow));
        }
        private void EditUserButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new EditUser(mainWindow));
        }
        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {

        }

        
        //Список пользователей
        private void AddFoodButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AddFood(mainWindow));
        }
        private void EditFoodButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AddFood(mainWindow));
        }
        private void DeleteFoodButton_Click(object sender, RoutedEventArgs e)
        {

        }




        //Профиль
        private void Change_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
