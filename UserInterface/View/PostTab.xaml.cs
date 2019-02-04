using System.Windows;
using System.Windows.Controls;

namespace UserInterface.View
{
    /// <summary>
    /// Interaction logic for PostTab.xaml
    /// </summary>
    public partial class PostTab : UserControl
    {
        public PostTab()
        {
            InitializeComponent();
        }

        

        private void YoutubeIconClick(object sender, RoutedEventArgs e)
        {
            YoutubeTitle.Visibility = YoutubeTitle.Visibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
            YoutubeTitleText.Visibility = YoutubeTitleText.Visibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
        }
    }
}