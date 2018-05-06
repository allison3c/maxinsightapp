namespace MaxInsight.Mobile.Pages
{
    public partial class IndexPage
    {
        public IndexPage()
        {
            InitializeComponent();
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            Title = CurrentPage?.Title;
        }
    }
}
