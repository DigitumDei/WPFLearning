namespace WPFLearning.ViewModels
{
    public class TabViewModel : BrowserViewModel
    {
        private string _title = "New Tab";
        public TabViewModel()
        {
        }
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        public override void UpdateAddressFromWebView(Uri uri)
        {
            base.UpdateAddressFromWebView(uri);
            // Update tab title based on the page title or URL
            Title = uri?.Host ?? "New Tab";
        }

        public void UpdateTitle(string pageTitle)
        {
            if (!string.IsNullOrEmpty(pageTitle))
            {
                Title = pageTitle;
            }
        }
    }
}