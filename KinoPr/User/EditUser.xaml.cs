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
    /// Логика взаимодействия для EditUser.xaml
    /// </summary>
    public partial class EditUser : Page
    {
        public MainWindow mainWindow;
        private User selectedUser;
        public EditUser(User selectedUser, MainWindow main)
        {
            InitializeComponent();
            this.selectedUser = selectedUser;
            mainWindow = main;
            surname.Text = selectedUser.surname;
            name.Text = selectedUser.name;
            patronymic.Text = selectedUser.patronymic;
            login.Text = selectedUser.login;
            password.Text = selectedUser.password;
            phone_number.Text = selectedUser.phone_number;
            email.Text = selectedUser.email;
            birht.Text = selectedUser.birth.ToString();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AdminPage(mainWindow));
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
