using System;
using Commander;
using PropertyChanged;
using System.Collections.ObjectModel;
using UserInterface.View;

namespace UserInterface.ViewModel
{
    [ImplementPropertyChanged]
    public class FacebookTimelineViewModel
    {

        private ObservableCollection<object> _myFeed;
        public string ComentText { get; set; }

        public object SelectedPost { get; set; }

        public ObservableCollection<object> MyFeed
        {
            get { return _myFeed; }
            set
            {
                _myFeed = value;
            }
        }

        public FacebookTimelineViewModel()
        {
            FacebookAPI.FacebookApi.GetMyFeed();
            MyFeed = new ObservableCollection<object>(FacebookAPI.FacebookApi.posts);
        }

        [OnCommand("RefreshMyFeed")]
        public void RefreshMyFeed()
        {
            FacebookAPI.FacebookApi.GetMyFeed();
            MyFeed = new ObservableCollection<object>(FacebookAPI.FacebookApi.posts);
        }

        [OnCommand("LinkCommandPost")]
        public void LinkCommandPost(string url)
        {
            System.Diagnostics.Process.Start(url);
        }

        [OnCommand("LinkCommandPhoto")]
        public void LinkCommandPhoto(string _url)
        {
            System.Diagnostics.Process.Start(_url);
        }

        [OnCommand("LikeUnlike")]
        public void LikeUnlike()
        {
            FacebookAPI.FacebookApi.LikeUnlike(SelectedPost);
        }

        [OnCommand("Coments")]
        public void Coments()
        {
            FacebookAPI.FacebookApi.Coment(SelectedPost, ComentText);
        }
    }
}
