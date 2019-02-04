using Commander;
using Google.Apis.YouTube.v3.Data;
using PropertyChanged;
using System.Collections.ObjectModel;

namespace UserInterface.ViewModel
{
    [ImplementPropertyChanged]
    public class YouTubeTimelineViewModel
    {
        public ObservableCollection<YouTube.Repository> Repository { get; set; }
        public string Query { get; set; }
        public YouTubeTimelineViewModel()
        {
            RefreshYouTube();
        }

        [OnCommand("RefreshYouTube")]
        public void RefreshYouTube()
        {  
            var popularVid= YouTube.YouTubeAP.GetPopularVideos();
            Repository = new ObservableCollection<YouTube.Repository>();
            foreach (var item in popularVid)
            {
                Repository.Add(new YouTube.Repository
                {
                    Title = item.Snippet.Title,
                    Description = item.Snippet.Description,
                    ImageUrl = item.Snippet.Thumbnails.Medium.Url,
                    PublishedDate = item.Snippet.PublishedAt.ToString(),
                    VideoId = item.Id
                });
            }
        }

        [OnCommand("HyperlinkWatch")]
        public void HyperlinkCommand(string id)
        {
           
            System.Diagnostics.Process.Start("https://www.youtube.com/watch?v="+id);
        }

        [OnCommand("SearchVideo")]
        public void SearchYouTube()
        {
            var searchResult = YouTube.YouTubeAP.SearchVideo(Query);
            Repository = new ObservableCollection<YouTube.Repository>();
            foreach (var item in searchResult)
            {
                Repository.Add(new YouTube.Repository
                {
                    Title = item.Snippet.Title,
                    Description = item.Snippet.Description,
                    ImageUrl = item.Snippet.Thumbnails.Medium.Url,
                    PublishedDate = item.Snippet.PublishedAt.ToString(),
                    VideoId = item.Id.VideoId
                });
            }
         
        }
    }
}

