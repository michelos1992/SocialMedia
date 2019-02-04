using Facebook;
using FacebookAPI;
using System;
using System.Reflection;
using System.Windows.Controls;

namespace UserInterface.View
{
    public partial class FacebookAuthorization : UserControl
    {
        public string AccessToken { get; set; }
        private FacebookClient fbC;

        public FacebookAuthorization()
        {
            InitializeComponent();
            string OAuthURL = @"https://graph.facebook.com/oauth/authorize?client_id=428267497382622&redirect_uri=http://www.facebook.com/connect/login_success.html&scopes=publish_pages,user_videos,pages_manage_cta,manage_pages,user_posts,user_status,publish_actions,user_photos,friends_photos,public_profile,read_stream&type=user_agent&display=popup";
            WBrowser.Navigate(new Uri(OAuthURL, UriKind.Absolute));
        }

        private void WBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.Uri.ToString().StartsWith("http://www.facebook.com/connect/login_success.html"))
            {
                AccessToken = e.Uri.Fragment.Split('&')[0].Replace("#access_token=", "");
                fbC = new FacebookClient(AccessToken);
                FacebookApi.Accessik = AccessToken;
            }
        }
        
    }
}
