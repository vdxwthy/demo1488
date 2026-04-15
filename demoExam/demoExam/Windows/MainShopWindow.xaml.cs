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
using System.Windows.Shapes;

namespace demoExam.Windows
{
    public partial class MainShopWindow : Window
    {
        public Model.User? CurrentUser { get; private set; }

        public MainShopWindow()
        {
            InitializeComponent();
            NavigateToLogin();
        }

        public void SetUser(Model.User? user)
        {
            CurrentUser = user;
            UserFullNameText.Text = user == null
                ? "Гость"
                : $"{user.Lastname} {user.Name} {user.Midname}".Trim();
        }

        private void NavigateToLogin()
        {
            SetUser(null);
            ShowHeader(false);
            MainFrame.Navigate(new Pages.LoginPage(this));
        }

        public void NavigateToProducts()
        {
            ShowHeader(true);
            MainFrame.Navigate(new Pages.ProductsPage(CurrentUser));
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToLogin();
        }

        private void ShowHeader(bool visible)
        {
            var v = visible ? Visibility.Visible : Visibility.Collapsed;
            UserFullNameText.Visibility = v;
            LogoutButton.Visibility = v;
        }
    }
}
