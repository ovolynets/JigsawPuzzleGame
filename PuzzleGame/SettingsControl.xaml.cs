using Windows.UI.Xaml;

namespace PuzzleGame
{
    public partial class SettingsControl
    {
        public SettingsControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SettingsTextProperty =
            DependencyProperty.Register("SettingsText", typeof(string), typeof(SettingsControl), new PropertyMetadata(""));

        public string SettingsText { get; set; }
    }
}