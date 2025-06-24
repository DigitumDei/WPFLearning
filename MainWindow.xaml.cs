using MahApps.Metro.Controls;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System.Windows.Input;
using WPFLearning.ViewModels;

namespace WPFLearning
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Handle Enter key for navigation
        private void AddressBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && DataContext is MainViewModel mainVm)
            {
                if (mainVm.SelectedTab?.GoCommand?.CanExecute(null) ?? false)
                    mainVm.SelectedTab.GoCommand.Execute(null);
            }
        }

        // Update address bar when navigation completes
        private void WebView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (sender is WebView2 webView && DataContext is MainViewModel mainVm)
            {
                mainVm.SelectedTab?.UpdateAddressFromWebView(webView.Source);
            }
        }

        private void SuggestionList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is MainViewModel mainVm)
            {
                if (mainVm.SelectedTab?.DuckDuckGoSearchCommand?.CanExecute(null) ?? false)
                    mainVm.SelectedTab.DuckDuckGoSearchCommand.Execute(null);
            }
        }
    }
}