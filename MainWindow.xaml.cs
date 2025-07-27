using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System.Windows;
using System.Windows.Input;
using WPFLearning.ViewModels;
using Dragablz;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace WPFLearning
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
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
        private async void WebView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (sender is WebView2 webView && DataContext is MainViewModel mainVm)
            {
                mainVm.SelectedTab?.UpdateAddressFromWebView(webView.Source);
                
                // Update tab title with actual page title
                try
                {
                    var pageTitle = await webView.CoreWebView2.ExecuteScriptAsync("document.title");
                    if (!string.IsNullOrEmpty(pageTitle))
                    {
                        // Remove quotes from JSON string result
                        pageTitle = pageTitle.Trim('"');
                        mainVm.SelectedTab?.UpdateTitle(pageTitle);
                    }
                }
                catch
                {
                    // Fallback to host name if title retrieval fails
                }
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

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void CloseTab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("CloseTab_MouseDown called!");
            
            if (sender is Button button)
            {
                System.Diagnostics.Debug.WriteLine($"Button DataContext: {button.DataContext}");
                
                var tabToClose = button.DataContext as TabViewModel;
                var mainVm = DataContext as MainViewModel;
                
                System.Diagnostics.Debug.WriteLine($"TabToClose: {tabToClose?.Title}");
                System.Diagnostics.Debug.WriteLine($"MainVm: {mainVm}");
                
                if (tabToClose != null && mainVm != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Removing tab: {tabToClose.Title}");
                    mainVm.RemoveTab(tabToClose);
                    e.Handled = true; // Prevent drag operation
                }
            }
        }

    }
}