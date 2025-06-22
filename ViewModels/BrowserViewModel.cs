using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace WPFLearning.ViewModels
{
    public class BrowserViewModel : INotifyPropertyChanged
    {
        private string _address = "https://cv.darkervision.com";
        public string Address
        {
            get => _address;
            set { _address = value; OnPropertyChanged(); }
        }

        private Uri _source;
        public Uri Source
        {
            get => _source;
            set { _source = value; OnPropertyChanged(); }
        }

        public ICommand GoCommand { get; }

        public BrowserViewModel()
        {
            GoCommand = new RelayCommand(_ => GoToAddress());
            GoToAddress();
        }

        private void GoToAddress()
        {
            var url = Address?.Trim();
            if (string.IsNullOrWhiteSpace(url)) return;
            if (!url.StartsWith("http"))
                url = "https://" + url;
            Source = new Uri(url);
        }

        public void UpdateAddressFromWebView(Uri uri)
        {
            Address = uri?.ToString() ?? "";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    // Simple RelayCommand implementation
    public class RelayCommand(Action<object> execute) : ICommand
    {
        private readonly Action<object> _execute = execute;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter) => _execute(parameter);
        public event EventHandler CanExecuteChanged;
    }
}