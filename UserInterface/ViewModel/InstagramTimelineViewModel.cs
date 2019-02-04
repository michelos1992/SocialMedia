using Commander;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Configuration;
using UserInterface.View;

namespace UserInterface.ViewModel
{
    [ImplementPropertyChanged]
    public class InstagramTimelineViewModel
    {
        //private ObservableCollection<Image> _images;
        private ObservableCollection<object> _myFeed;
        public ObservableCollection<object> MyFeed
        {
            get { return _myFeed; }
            set
            {
                _myFeed = value;
            }
        }
        public string CText { get; set; }

        public InstaSharp.Models.Media SelectedPost { get; set; }

        [OnCommand("RefreshImages")]
        public void RefreshImages()
        {
            Instagram.Instagram.MyFeed();
            MyFeed = new ObservableCollection<object>(Instagram.Instagram.Images);
        }

        [OnCommand("LinkImage")]
        public void LinkImage(string url)
        {
            System.Diagnostics.Process.Start(url);
        }

        [OnCommand("LikeInsta")]
        public void LikeInsta()
        {
            Instagram.Instagram.LikeInsta(SelectedPost);
            RefreshImages();
        }

        [OnCommand("Comments")]
        public void Comments()
        {
            Instagram.Instagram.Comment(SelectedPost, CText);
        }
    }
}
