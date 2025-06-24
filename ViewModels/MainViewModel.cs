using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace WPFLearning.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
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
            CloseTabCommand = new RelayCommand(tab => RemoveTab(tab as TabViewModel),
                                            tab => Tabs.Count > 1);

            AddTab(); // Open initial tab
        }

        public void AddTab()
        {
            var tab = new TabViewModel();
            Tabs.Add(tab);
            SelectedTab = tab;
        }

        public void RemoveTab(TabViewModel tab)
        {
            if (tab != null && Tabs.Contains(tab))
            {
                int index = Tabs.IndexOf(tab);
                Tabs.Remove(tab);
                if (Tabs.Count > 0)
                    SelectedTab = Tabs[index > 0 ? index - 1 : 0];
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}