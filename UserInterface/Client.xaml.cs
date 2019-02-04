using System;
using System.Windows;
using MahApps.Metro.Controls;

namespace UserInterface
{
    public partial class Client : MetroWindow
    {
        public Client()
        {
            InitializeComponent();
        }

        private void OpenPostTab(object sender, RoutedEventArgs e)
        {
            PostTab.Visibility = PostTab.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;       
            TwitterTab.Margin = TwitterTab.Margin == new Thickness(250, 0, 0, 0) ? new Thickness(10, 0, 0, 0) : new Thickness(250, 0, 0, 0);
            FacebookTab.Margin = FacebookTab.Margin == new Thickness(470, 0, 0, 0) ? new Thickness(280, 0, 0, 0) : new Thickness(470, 0, 0, 0);
            InstagramTab.Margin = InstagramTab.Margin == new Thickness(690, 0, 0, 0) ? new Thickness(550, 0, 0, 0) : new Thickness(690, 0, 0, 0);
            YoutubeTab.Margin = YoutubeTab.Margin == new Thickness(910, 0, 0, 0) ? new Thickness(820, 0, 0, 0) : new Thickness(910, 0, 0, 0);
            
            TwitterTimelineUserControl.Width = TwitterTimelineUserControl.Width == 210 ? 260 : 210;
            TwitterAuthorizationUserControl.Width = TwitterAuthorizationUserControl.Width == 210 ? 260 : 210;
            FacebookTimelineUserControl.Width = FacebookTimelineUserControl.Width == 210 ? 260 : 210;
            FacebookAuthorizationUserControl.Width = FacebookAuthorizationUserControl.Width == 210 ? 260 : 210;
            InstagramTimelineUserControl.Width = InstagramTimelineUserControl.Width == 210 ? 260 : 210;
            InstagramAuthorizationUserControl.Width = InstagramAuthorizationUserControl.Width == 210 ? 260 : 210;
            YouTubeTimelineUserControl.Width = YouTubeTimelineUserControl.Width == 210 ? 300 : 210;
        }
    }
}