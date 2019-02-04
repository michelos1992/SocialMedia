using Commander;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using PropertyChanged;
using System.Windows;

namespace UserInterface.ViewModel
{
    [ImplementPropertyChanged]
    public class PostTabViewModel
    {
        private MediaFile.MediaFile _mediaFile;
        public string Text { get; set; }
        public string Tittle { get; set; }
        public bool TwitterIconOpacity { get; set; }
        public bool FacebookIconOpacity { get; set; }
        public bool InstagramIconOpacity { get; set; }
        public bool YoutubeIconOpacity { get; set; }

        [OnCommand("TwitterIconCommand")]
        public void TwitterIconCommand()
        {
            TwitterIconOpacity = TwitterIconOpacity == false;         
        }

        [OnCommand("FacebookIconCommand")]
        public void FacebookIconCommand()
        {
            FacebookIconOpacity = FacebookIconOpacity == false;          
        }

        [OnCommand("InstagramIconCommand")]
        public void InstagramIconCommand()
        {
            InstagramIconOpacity = InstagramIconOpacity == false;          
        }
        [OnCommand("YoutubeIconCommand")]
        public void YoutubeIconCommand()
        {
            YoutubeIconOpacity = YoutubeIconOpacity == false;         
        }

        [OnCommand("AddFile")]
        public void AddFile()
        {
            _mediaFile = new MediaFile.MediaFile();
            _mediaFile.AddFile();
        }

        [OnCommand("Publish")]
        public async void Publish()
        {
            if (YoutubeIconOpacity) YouTube.YouTubeAP.Publish(_mediaFile, Tittle, Text);
            if (TwitterIconOpacity) Twitter.Twitter.Publish(Text, _mediaFile);
            if (FacebookIconOpacity) FacebookAPI.FacebookApi.Publish(Text, _mediaFile);
            if (TwitterIconOpacity == false && FacebookIconOpacity == false && InstagramIconOpacity == false && YoutubeIconOpacity == false)
            {
                var metroWindow = (Application.Current.MainWindow as MetroWindow);
                await metroWindow.ShowMessageAsync("POST TAB", "Nie wybrałeś żadnego z portali!");
            }
        }
    }
}