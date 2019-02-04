using System;
using Commander;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using Tweetinvi.Core.Interfaces;

namespace UserInterface.ViewModel
{
    [ImplementPropertyChanged]
    public class TwitterTimelineViewModel
    {
        public ObservableCollection<ITweet> Tweets { get; set; }
        public ITweet SelectedTweet { get; set; }
        public string TweetCommentText { get; set; }
        private DispatcherTimer _timer;

        public TwitterTimelineViewModel()
        {
            Twitter.Twitter.GetCredentialsFromConfig();
            Twitter.Twitter.Authorize();
            RefreshTweets();
            GetLoggedUserTimeLine();
        }

        [OnCommand("RefreshTweets")]
        public void RefreshTweets()
        {
            Tweets = new ObservableCollection<ITweet>(Twitter.Twitter.GetLoggedUserTimeLine());
        }

        [OnCommand("GetTwitterAuthorizationUrl")]
        public void GetTwitterAuthorizationUrl()
        {
            Twitter.Twitter.GetTwitterAuthorizationUrl();
        }

        [OnCommand("TwitterHyperlinkCommand")]
        public void TwitterHyperlinkCommand(string url)
        {
            System.Diagnostics.Process.Start(url);
        }

        [OnCommand("LikeAndUnlikeTweet")]
        public void LikeAndUnlikeTweet()
        {
            Twitter.Twitter.LikeAndUnlikeTweet(SelectedTweet);
        }

        [OnCommand("CommentTweet")]
        public void CommentTweet()
        {
            Twitter.Twitter.CommentTweet(SelectedTweet, TweetCommentText);
        }

        [OnCommand("Retweet")]
        public void Retweet()
        {
            Twitter.Twitter.Retweet(SelectedTweet);
        }

        private void GetLoggedUserTimeLine()
        {
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(30000) };
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            RefreshTweets();
        }
    }
}