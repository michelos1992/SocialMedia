using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YouTube
{
    public class YouTubeAP
    {
        Repository rep = new Repository();
        public static IEnumerable<Video> GetPopularVideos()//Video
        {
            
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyDntWkrZCMiOFMef733L2aAxtzv4bImrPs",
                ApplicationName = "YouTubeTest"
            });
            var getVid = youtubeService.Videos.List("snippet");
            getVid.Chart = 0;
            getVid.MaxResults = 50;
            var mostPopularVid = getVid.Execute();

         
            return mostPopularVid.Items;
        }

        public static IEnumerable<SearchResult> SearchVideo(string query)//SearchResult
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyDntWkrZCMiOFMef733L2aAxtzv4bImrPs",
                ApplicationName = "YouTubeTest"
            });
            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = query; 
            searchListRequest.MaxResults = 50;

            var result = searchListRequest.Execute();
            return result.Items;
        }
        public static void Publish(MediaFile.MediaFile mediaFile, string tittle, string description)
        {
            try
            {
                new YouTubeAP().Run(mediaFile, tittle, description).Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    MessageBox.Show("Error: " + e.Message);
                }
            }
        }

        public async Task Run(MediaFile.MediaFile mediaFile, string tittle, string description)
        {
            UserCredential credential;
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
              GoogleClientSecrets.Load(stream).Secrets,
              //This OAuth 2.0 access scope allows for full read/ write access to the
              //authenticated user's account.
              new[] { YouTubeService.Scope.Youtube },
              "user",
              CancellationToken.None,
              new FileDataStore(this.GetType().ToString())

                );
            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                //  ApiKey = "AIzaSyDntWkrZCMiOFMef733L2aAxtzv4bImrPs",
                HttpClientInitializer = credential,
                ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
            });

            var video = new Video();
            video.Snippet = new VideoSnippet();
            video.Snippet.Title = tittle;
            video.Snippet.Description = description;
            video.Snippet.Tags = new string[] { "tag1", "tag2" };
            video.Snippet.CategoryId = "22"; // See https://developers.google.com/youtube/v3/docs/videoCategories/list
            video.Status = new VideoStatus();
            video.Status.PrivacyStatus = "public"; // or "private" or "public"
            var filePath = mediaFile._filePath;
            if (mediaFile._openFileDialog.FilterIndex == 5)
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open))
                {

                    var videosInsertRequest = youtubeService.Videos.Insert(video, "snippet,status", fileStream, "video/*");
                    videosInsertRequest.ProgressChanged += videosInsertRequest_ProgressChanged;

                    const int KB = 0x400;
                    var minimumChunkSize = 256 * KB;
                    videosInsertRequest.ChunkSize = minimumChunkSize * 1;

                    videosInsertRequest.Upload();

                   
                }
            }
            else
                MessageBox.Show("błędny format pliku");
        }

        void videosInsertRequest_ProgressChanged(Google.Apis.Upload.IUploadProgress progress)
        {
            switch (progress.Status)
            {
                case UploadStatus.Uploading:
                    break;

                case UploadStatus.Failed:
                    MessageBox.Show("An error prevented the upload from completing");
                    break;
                case UploadStatus.Completed:
                    MessageBox.Show("Film został wysłany");
                    break;
            }
        }

    }
}
