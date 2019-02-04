using InstaSharp;
using InstaSharp.Endpoints;
using System.Configuration;
using System.Collections.Generic;
using MahApps.Metro.Controls;
using System.Windows;
using System;
using System.Linq;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;

namespace Instagram
{
    public class Instagram
    {
        public static string Code { get; set; }  
        public static string Access { get; set; }     
        public static List<InstaSharp.Models.Media> Images = new List<InstaSharp.Models.Media>();
        public static InstagramConfig _config = new InstagramConfig(ConfigurationManager.AppSettings["instagramClientID"],
             ConfigurationManager.AppSettings["instagramClientSecret"], 
             ConfigurationManager.AppSettings["instagramRedirectUri"]);
        protected static Users _user;
        private static readonly MetroWindow _metroWindow = Application.Current.MainWindow as MetroWindow;


        public static string OAuth()
        {

            var scopes = new List<OAuth.Scope>();
            scopes.Add(InstaSharp.OAuth.Scope.Likes);
            scopes.Add(InstaSharp.OAuth.Scope.Comments);
            scopes.Add(InstaSharp.OAuth.Scope.Public_Content);
            scopes.Add(InstaSharp.OAuth.Scope.Basic);
            scopes.Add(InstaSharp.OAuth.Scope.Follower_List);
            scopes.Add(InstaSharp.OAuth.Scope.Relationships);
            var link = InstaSharp.OAuth.AuthLink(_config.OAuthUri + "authorize", _config.ClientId, _config.RedirectUri, scopes, InstaSharp.OAuth.ResponseType.Code);
            return link;
        }

        public static async void LikeInsta(InstaSharp.Models.Media selectedPost)
        {
            if (selectedPost != null)
            {
                var authInfo = new OAuth(_config);
                var authResponse = new InstaSharp.Models.Responses.OAuthResponse();
                var response = await _user.GetSelf();
                authResponse.AccessToken = Access;
                authResponse.User = response.Data;
                var likes = new Likes(_config, authResponse);
                if (selectedPost.UserHasLiked == true)
                {
                    await likes.Delete(selectedPost.Id);
                    selectedPost.UserHasLiked = false;
                    await _metroWindow.ShowMessageAsync("Instagram", "Cofnięto Like'a!");
                }
                else
                {
                    await likes.Post(selectedPost.Id);
                    selectedPost.UserHasLiked = true;
                    await _metroWindow.ShowMessageAsync("Instagram", "Dodano Like'a!");
                }
            }
            else await _metroWindow.ShowMessageAsync("", "Nie wybrano posta");
        }

        public static async void MyFeed()
        {
            try {
                if (_user == null)
                {
                    var authInfo = new OAuth(_config);
                    var response = await authInfo.RequestToken(Code);
                    Access = response.AccessToken;
                    _user = new Users(_config, response);
                }
                var media = await _user.RecentSelf();
                Images.Clear();
                Parallel.ForEach(media.Data, (item) =>
                {
                    Images.Add(item);
                });
            }
            catch
            {
                await _metroWindow.ShowMessageAsync("Instagram", "Nie zalogowano");
            }


        }

        public static async void Comment(InstaSharp.Models.Media selectedPost, string comentText)
        {
            if (selectedPost != null)
            {

                var authInfo = new OAuth(_config);
                var thing = new InstaSharp.Models.Responses.OAuthResponse();
                var response = await _user.GetSelf();
                thing.AccessToken = Access;
                thing.User = response.Data;
                var comments = new Comments(_config,thing);
                await comments.Post(selectedPost.Id, comentText);
                await _metroWindow.ShowMessageAsync("Instagram", "Opublikowano komentarz!");
            }
            else await _metroWindow.ShowMessageAsync("", "Nie wybrano posta");
        }
        #region auth0
        //public static void Login()
        //{
        //    Window w = System.Windows.Application.Current.MainWindow;
        //    var windowParent = new WindowWrapper(new WindowInteropHelper(w).Handle);
        //    var auth0 = new Auth0Client(
        //        "ath.eu.auth0.com/", _config.ClientId
        //        /*"BPlkYggrnmdat00OPkthgiAVrVcwxpiW"*/);
        //    auth0.LoginAsync(windowParent).ContinueWith(t =>
        //    {
        //        var profile = t.Result.Profile;
        //    },
        //    TaskScheduler.FromCurrentSynchronizationContext());
        //}
        #endregion

    }
}
