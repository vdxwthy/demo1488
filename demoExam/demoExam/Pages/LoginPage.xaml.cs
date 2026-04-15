using System.Linq;
using System.Windows;
using System.Windows.Controls;
using demoExam.Model;
using demoExam.Windows;

namespace demoExam.Pages;

public partial class LoginPage : Page
{
    private readonly MainShopWindow _mainWindow;

    public LoginPage(MainShopWindow mainWindow)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        ErrorText.Text = "";
        var email = EmailTextBox.Text?.Trim();
        var password = PasswordBox.Password?.Trim();
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ErrorText.Text = "Введите email и пароль";
            return;
        }

        try
        {
            using var db = new DbshopbootsContext();
            var user = db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user == null)
            {
                ErrorText.Text = "Неверные учетные данные";
                return;
            }

            _mainWindow.SetUser(user);
            _mainWindow.NavigateToProducts();
        }
        catch
        {
            ErrorText.Text = "Ошибка подключения к базе данных";
        }
    }

    private void GuestButton_Click(object sender, RoutedEventArgs e)
    {
        _mainWindow.SetUser(null);
        _mainWindow.NavigateToProducts();
    }
}
