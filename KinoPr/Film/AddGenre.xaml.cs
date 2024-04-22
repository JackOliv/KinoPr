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
    /// Логика взаимодействия для AddGenre.xaml
    /// </summary>
    public partial class AddGenre : Page
    {
        public MainWindow mainWindow;
        public AddGenre(MainWindow main)
        {
            InitializeComponent();
            mainWindow = main;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AdminPage(mainWindow));
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
