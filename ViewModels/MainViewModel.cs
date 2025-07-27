using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Dragablz;

namespace WPFLearning.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged, IInterTabClient
    {
        private TabViewModel _selectedTab;

        public ObservableCollection<TabViewModel> Tabs { get; } = new();

        public TabViewModel SelectedTab
        {
            get => _selectedTab;
            set
            {
                if (_selectedTab != value)
                {
                    _selectedTab = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand NewTabCommand { get; }
        public ICommand CloseTabCommand { get; }

        public MainViewModel()
        {

            NewTabCommand = new RelayCommand(_ => AddTab());
            AddTab(); // Open initial tab
            CloseTabCommand = new RelayCommand(tab => RemoveTab(tab as TabViewModel), tab => true);


        }

        public void AddTab()
        {
            var tab = new TabViewModel();
            Tabs.Add(tab);
            SelectedTab = tab;
        }

        public void RemoveTab(TabViewModel tab)
        {
            System.Diagnostics.Debug.WriteLine($"RemoveTab called with: {tab?.Title}");
            System.Diagnostics.Debug.WriteLine($"Current tab count: {Tabs.Count}");
            
            if (tab != null && Tabs.Contains(tab))
            {
                int index = Tabs.IndexOf(tab);
                System.Diagnostics.Debug.WriteLine($"Removing tab at index: {index}");
                Tabs.Remove(tab);
                System.Diagnostics.Debug.WriteLine($"Tab count after removal: {Tabs.Count}");
                
                if (Tabs.Count > 0)
                {
                    int newIndex = index > 0 ? index - 1 : 0;
                    SelectedTab = Tabs[newIndex];
                    System.Diagnostics.Debug.WriteLine($"Selected new tab: {SelectedTab.Title}");
                }
                else
                {
                    SelectedTab = null;
                    System.Diagnostics.Debug.WriteLine("No tabs remaining");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Tab not found or null");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // IInterTabClient implementation  
        public INewTabHost<System.Windows.Window> GetNewHost(IInterTabClient interTabClient, object partition, TabablzControl source)
        {
            var newWindow = new MainWindow();
            newWindow.Show();
            var tabControl = newWindow.FindName("TabControl") as TabablzControl;
            return new NewTabHost<System.Windows.Window>(newWindow, tabControl);
        }

        public TabEmptiedResponse TabEmptiedHandler(TabablzControl tabControl, System.Windows.Window window)
        {
            return TabEmptiedResponse.DoNothing;
        }

        public ItemActionCallback ClosingTabItemHandler => ClosingTabItemHandlerImpl;

        private void ClosingTabItemHandlerImpl(ItemActionCallbackArgs<TabablzControl> args)
        {
            var tabViewModel = args.DragablzItem.DataContext as TabViewModel;
            System.Diagnostics.Debug.WriteLine($"ClosingTabItemHandler called for: {tabViewModel?.Title}");
            
            if (tabViewModel != null)
            {
                RemoveTab(tabViewModel);
            }
        }

        public Func<object> NewItemFactory => () => new TabViewModel();
    }
}