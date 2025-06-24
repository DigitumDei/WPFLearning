using Microsoft.Web.WebView2.Core;
using System.Windows;
using System.Windows.Input;
using WPFLearning.ViewModels;

namespace WPFLearning
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Handle Enter key for navigation
        private void AddressBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (DataContext is BrowserViewModel vm && vm.GoCommand.CanExecute(null))
                    vm.GoCommand.Execute(null);
            }
        }

        // Update address bar when navigation completes
        private void WebView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (DataContext is BrowserViewModel vm)
                vm.UpdateAddressFromWebView(WebView.Source);
        }

        private void SuggestionList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is BrowserViewModel vm && vm.DuckDuckGoSearchCommand.CanExecute(null))
                vm.DuckDuckGoSearchCommand.Execute(null);
        }
    }
}