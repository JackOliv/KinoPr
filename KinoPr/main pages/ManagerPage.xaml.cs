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
    /// Логика взаимодействия для ManagerPage.xaml
    /// </summary>
    public partial class ManagerPage : Page
    {
        public MainWindow mainWindow;
        public ManagerPage(MainWindow main)
        {
            InitializeComponent();
            mainWindow = main;
            profSurname.Content = Data.currentUser.Surname;
            profName.Content = Data.currentUser.Name;
            profPatronymic.Content = Data.currentUser.Patronymic;
            profTelephone.Content = Data.currentUser.PhoneNumber;
            profEmail.Content = Data.currentUser.Email;
            profLogin.Content = Data.currentUser.Login;
            profPassword.Content = Data.currentUser.Password;
            profBiht.Content = Data.currentUser.Birth;
        }




        //Добавить на сеанс
        private void FindUser_Click(object sender, RoutedEventArgs e)
        {

        }
        private void FindFilm_Click(object sender, RoutedEventArgs e)
        {

        }
        private void AddOnSessionButton_Click(object sender, RoutedEventArgs e)
        {

        }




        //Продажа еды
        private void FindUserFood_Click(object sender, RoutedEventArgs e)
        {

        }
        private void FindFood_Click(object sender, RoutedEventArgs e)
        {

        }
        private void MinusCount_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(Count.Content) > 1)
            {
                Count.Content = Convert.ToInt32(Count.Content) - 1;
            }
            else MessageBox.Show("Значение не может быть меньше 1");
        }
        private void AddCount_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(Count.Content) < 5)
            {
                Count.Content = Convert.ToInt32(Count.Content) + 1;
            }
            else MessageBox.Show("Значение не может быть больше 5");
        }
        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {

        }




        //Профиль
        private void SeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (profPassword.Visibility == Visibility.Visible)
            {
                profPassword.Visibility = Visibility.Collapsed;
                placeholder.Visibility = Visibility.Visible;
            }
            else
            {
                profPassword.Visibility = Visibility.Visible;
                placeholder.Visibility = Visibility.Collapsed;
            }
        }
        private void Change_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Data.currentUser = null;
            FrameManager.MainFrame.Navigate(new AutorizationPage(mainWindow));
        }
    }
}
