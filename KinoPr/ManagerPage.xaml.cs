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
        private void AddCount_Click(object sender, RoutedEventArgs e)
        {

        }
        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
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
