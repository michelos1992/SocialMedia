using System.Windows.Controls;
using System;
using System.Configuration;
using System.Reflection;
using Instagram;
using System.Windows.Interop;
using System.Windows;
using System.Collections.ObjectModel;

namespace UserInterface.View
{
    /// <summary>
    /// Interaction logic for InstagramAuthorization.xaml
    /// </summary>
    public partial class InstagramAuthorization : UserControl
    {
        public string Code { get; set; }
        public InstagramAuthorization()
        {
            InitializeComponent();
            string OAuthURL = Instagram.Instagram.OAuth();
            WBrowserI.Navigate(new Uri(OAuthURL, UriKind.Absolute));
        }

        private void WBrowserI_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            HideScriptErrors(WBrowserI,true);
            if (e.Uri.ToString().StartsWith("http://localhost/?code"))
            {
                Code = e.Uri.Query.Replace("?code=", "");
                Instagram.Instagram.Code = Code;
                WBrowserI.Navigate("about:blank");
                MessageBox.Show("Zalogowano!", "Instagram");
            }
            
        }
        public void HideScriptErrors(WebBrowser wb, bool hide)
        {
            var fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;
            var objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null)
            {
                wb.Loaded += (o, s) => HideScriptErrors(wb, hide); //In case we are to early
                return;
            }
            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { hide });
        }
    }
}
