using Microsoft.IdentityModel.Tokens;
using SINGLE_STAGE.Entities;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SINGLE_STAGE
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        readonly SingleStageContext _context;

        public string enteredUsername { get; set; }

        public Employee tempEmployee { get; set; }

        public LoginWindow()
        {
            InitializeComponent();
            DataContext = this;
            _context = new();
        }

        private void GridLoaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(UI02);
            LoginFailedMessage.Visibility = Visibility.Hidden;
            FillOutAllFieldsMessage.Visibility = Visibility.Hidden;
        }

        private void UI02GotFocus(object sender, RoutedEventArgs e)
        {
            LoginFailedMessage.Visibility = Visibility.Hidden;
            FillOutAllFieldsMessage.Visibility = Visibility.Hidden;
        }

        private void UI02KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
                Keyboard.Focus(PB01);
            if (e.Key == Key.Enter)
                LoginButtonClicked(sender, e);
        }

        private void PB01GotFocus(object sender, RoutedEventArgs e)
        {
            LoginFailedMessage.Visibility = Visibility.Hidden;
            FillOutAllFieldsMessage.Visibility = Visibility.Hidden;
        }

        private void PB01KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
                Keyboard.Focus(LoginButton);
            if (e.Key == Key.Enter)
                LoginButtonClicked(sender, e);
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void LoginButtonClicked(object sender, RoutedEventArgs e)
        {
            // check that all fields are filled out
            if (enteredUsername.IsNullOrEmpty() ||
                PB01.Password.IsNullOrEmpty()   )
            {
                FillOutAllFieldsMessage.Visibility = Visibility.Visible;
                return;
            }

            // check that the username exists
            tempEmployee = _context.Employees.FirstOrDefault(employee => employee.Username == enteredUsername);

            if (tempEmployee == null)
            {
                LoginFailedMessage.Visibility = Visibility.Visible;
                return;
            }

            // check that entered password matches password in the database
            bool passwordOK = BCrypt.Net.BCrypt.Verify(PB01.Password, tempEmployee.Password);

            if (!passwordOK)
            {
                LoginFailedMessage.Visibility = Visibility.Visible;
                return;
            }

            // if all checks pass, open main dashboard
            MainWindow main = new();
            main.Show();
            this.Close();
        }
    }
}
