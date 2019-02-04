using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using Tweetinvi;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Interfaces;

namespace Twitter
{
    public static class Twitter
    {
        private static ILoggedUser _loggedUser;
        private static ITwitterCredentials _applicationCredentials;
        private static ITwitterCredentials _userCredentials;      
        private static readonly MetroWindow MetroWindow = Application.Current.MainWindow as MetroWindow;
        private static readonly Configuration Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        private static void SaveConfiguration()
        {
            Configuration.AppSettings.Settings["twitterAccessToken"].Value = _userCredentials.AccessToken;
            Configuration.AppSettings.Settings["twitterAccessTokenSecret"].Value = _userCredentials.AccessTokenSecret;
            Configuration.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }

        public static void GetCredentialsFromConfig()
        {
            _userCredentials = new TwitterCredentials(ConfigurationManager.AppSettings["twitterConsumerKey"],
                                          ConfigurationManager.AppSettings["twitterConsumerSecret"],
                                          ConfigurationManager.AppSettings["twitterAccessToken"],
                                          ConfigurationManager.AppSettings["twitterAccessTokenSecret"]);
        }

        public static void Authorize()
        {
            try {
                Auth.SetCredentials(_userCredentials);
                _loggedUser = User.GetLoggedUser(_userCredentials);
            }
            catch(ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void GetTwitterAuthorizationUrl()
        {
            _applicationCredentials = new TwitterCredentials(ConfigurationManager.AppSettings["twitterConsumerKey"],
                                                             ConfigurationManager.AppSettings["twitterConsumerSecret"]);
            var url = CredentialsCreator.GetAuthorizationURL(_applicationCredentials);
            System.Diagnostics.Process.Start(url);
        }

        public static async void GetTwitterCaptcha()
        {
            if (_userCredentials == null)
            {
                GetTwitterAuthorizationUrl();
            }
            else
            {
                Authorize();
                await MetroWindow.ShowMessageAsync("TWITTER CAPTCHA", $"Jesteś już zalogowany jako {_loggedUser.ScreenName}!");
            }
        }

        public static async void GenerateCredentials(string captcha)
        {
            if (captcha != null)
            {
                _userCredentials = CredentialsCreator.GetCredentialsFromVerifierCode(captcha, _applicationCredentials);
                if (_userCredentials == null)
                {
                    await MetroWindow.ShowMessageAsync("TWITTER CAPTCHA", "Wpisałeś błędny kod!");
                }
                else
                {
                    Authorize();
                    SaveConfiguration();
                    await MetroWindow.ShowMessageAsync(_loggedUser.ScreenName, "Zalogowano!");
                }
            }
            else
            {
                await MetroWindow.ShowMessageAsync("TWITTER CAPTCHA", "Nie wpisałeś kodu captcha!");
            }
        }

        public static IEnumerable<ITweet> GetLoggedUserTimeLine()
        {
            GetCredentialsFromConfig();
            Authorize();
            return _userCredentials != null ? _loggedUser.GetHomeTimeline(10) : Tweet.GetTweets();
        }

        public static async void Publish(string text, MediaFile.MediaFile mediaFile)
        {
            if (text != null && mediaFile == null)
            {
                _loggedUser.PublishTweet(text);
                await MetroWindow.ShowMessageAsync("TWITTER TIMELINE", "Opublikowano tweeta!");
            }

            else {

                if (text != null && mediaFile._openFileDialog.FilterIndex == 5)
                {                    
                    Tweet.TweetController.PublishTweetWithVideo(text, mediaFile._fileBytes);                                       
                    await MetroWindow.ShowMessageAsync("TWITTER TIMELINE", "Opublikowano tweeta z plikiem video!");
                }

                else if (text != null)
                {                  
                    Tweet.PublishTweetWithImage(text, mediaFile._fileBytes);
                    await MetroWindow.ShowMessageAsync("TWITTER TIMELINE", "Opublikowano tweeta z obrazkiem!");
                }
            }
        }
        
        public static async void LikeAndUnlikeTweet(ITweet selectedTweet)
        {
            if (selectedTweet == null) return;
            if (selectedTweet.Favourited == false)
            {
                selectedTweet.Favourite();
                await MetroWindow.ShowMessageAsync("TWITTER TIMELINE", "Dodano Like'a!");
            }

            else
            {
                selectedTweet.UnFavourite();
                await MetroWindow.ShowMessageAsync("TWITTER TIMELINE", "Cofnięto Like'a!");
            }
        }

        public static async void CommentTweet(ITweet selectedTweet, string text)
        {
            if (selectedTweet == null) return;
            var textToPublish = $"@{selectedTweet.CreatedBy.ScreenName} {text}";
            Tweet.PublishTweetInReplyTo(textToPublish, selectedTweet);

            await MetroWindow.ShowMessageAsync("TWITTER TIMELINE", "Opublikowano komentarz!");
        }

        public static async void Retweet(ITweet selectedTweet)
        {
            if (selectedTweet == null) return;
            Tweet.PublishRetweet(selectedTweet);
            await MetroWindow.ShowMessageAsync("TWITTER TIMELINE", "Udostępniono tweeta!");
        }
      
    }
}