using System;
using System.Windows;

namespace B2B_Utils
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OperationsButton_Click(object sender, RoutedEventArgs e)
        {
            this._NavigationFrame.NavigationService.Navigate(new Uri("/View/OperationsLogsPage.xaml",UriKind.Relative));
            //this._NavigationFrame.NavigationService.Navigate(new Uri("http://google.com"));
        }

        private void SearchStatButton_Click(object sender, RoutedEventArgs e)
        {
            this._NavigationFrame.NavigationService.Navigate(new Uri("/View/SearchStatPage.xaml", UriKind.Relative));
        }

        private void CityPairsButton_Click(object sender, RoutedEventArgs e)
        {
            this._NavigationFrame.NavigationService.Navigate(new Uri("/View/CityPairsPage.xaml", UriKind.Relative));
        }
    }
}
