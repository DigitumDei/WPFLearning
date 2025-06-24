using System.ComponentModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace WPFLearning.ViewModels
{
    public class BrowserViewModel : INotifyPropertyChanged
    {
        private readonly Subject<string> _addressInput = new();
        private string _address = "https://cv.darkervision.com";
        public string Address
        {
            get => _address;
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged();
                    _addressInput.OnNext(value);
                }
            }
        }

        private Uri? _source;
        public Uri? Source
        {
            get => _source;
            set { _source = value; OnPropertyChanged(); }
        }

        private bool _showSuggestion;
        public bool ShowSuggestion
        {
            get => _showSuggestion;
            set { _showSuggestion = value; OnPropertyChanged(); }
        }

        private string _duckDuckGoSuggestion = "";
        public string DuckDuckGoSuggestion
        {
            get => _duckDuckGoSuggestion;
            set { _duckDuckGoSuggestion = value; OnPropertyChanged(); }
        }

        public ICommand GoCommand { get; }
        public ICommand DuckDuckGoSearchCommand { get; }

        public BrowserViewModel()
        {
            GoCommand = new RelayCommand(_ => GoToAddress());
            DuckDuckGoSearchCommand = new RelayCommand(_ => DuckDuckGoSearch());

            // Rx pipeline for suggestions
            _addressInput
                .Throttle(TimeSpan.FromMilliseconds(150))
                .DistinctUntilChanged()
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(UpdateSuggestion);

            GoToAddress();
        }

        private void GoToAddress()
        {
            var url = Address?.Trim();
            if (string.IsNullOrWhiteSpace(url)) return;
            if (!url.StartsWith("http"))
                url = "https://" + url;
            Source = new Uri(url);
            ShowSuggestion = false;
        }

        private void DuckDuckGoSearch()
        {
            if (Address.Length >= 3)
            {
                var query = Uri.EscapeDataString(Address);
                Source = new Uri($"https://duckduckgo.com/?q={query}");
                ShowSuggestion = false;
            }
        }

        private void UpdateSuggestion(string input)
        {
            if (!string.IsNullOrWhiteSpace(input) && input.Length >= 3)
            {
                DuckDuckGoSuggestion = $"Search DuckDuckGo for \"{input}\"";
                ShowSuggestion = true;
            }
            else
            {
                ShowSuggestion = false;
            }
        }

        public virtual void UpdateAddressFromWebView(Uri uri)
        {
            Address = uri?.ToString() ?? "";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}