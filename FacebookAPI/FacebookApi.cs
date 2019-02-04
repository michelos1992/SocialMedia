using System.Collections.Generic;
using Facebook;
using System.Windows.Media.Animation;
using System.Dynamic;
using System;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace FacebookAPI
{
    public class FacebookApi
    {
        private static FacebookClient facebookClient;
        public static string Accessik = "CAAGFgcwM4t4BAEkhazyLYL4inWvC9o9RN0vmieGZAmoGZAvybu5tnvzZADrsUi6ISTqPDwGqXsXfUohVuxscvAEUYMtEZAnsh0eK594EMKqHLZAciwAYMjjCkn96LjwNfJTfKuvqXxEHAVBuZB7rAWgymNzbC0boptUpZAScGgXcLAhHmRZC06mCaPmyXAdG4D0ZD";
        public static List<Post> posts = new List<Post>();

        public static void GetMyFeed()
        {
            if (Accessik != null)
            {
                facebookClient = new FacebookClient(Accessik);
                dynamic result = facebookClient.Get("me/feed?fields=from{picture{url}, name},message,created_time,link,picture,actions");
                posts.Clear();
                string link1 = "", link2 = "", IdField = "";
                string date1 = "", date2 = "", date = "";
                
                foreach (var item in result.data)
                {
                    if (item.message != null)
                    {
                        if (item.picture != null)
                        {
                            IdField = item.id;
                            string[] Link = IdField.Split('_');
                            link1 = Link[0];
                            link2 = Link[1];
                            string link3 = "https://www.facebook.com/" + link1 + "/posts/" + link2;
                            date = item.created_time;
                            string[] date_table = date.Split('+');
                            date = date_table[0];
                            string[] date_table2 = date.Split('T');
                            date1 = date_table2[0];
                            date2 = date_table2[1];
                            date = date1 + " " + date2;
                            posts.Add(new Post
                            {
                                Id = item.id,
                                NamePicture = item.from.picture.data.url,
                                Name = item.from.name,
                                LinkPost = link3,
                                Description = item.message,
                                Data = date,
                                LinkPhotoPost = item.link,
                                Picture = item.picture,
                            });
                        }
                        else
                        {
                            date = item.created_time;
                            string[] date_table = date.Split('+');
                            date = date_table[0];
                            string[] date_table2 = date.Split('T');
                            date1 = date_table2[0];
                            date2 = date_table2[1];
                            date = date1 + " " + date2;
                            IdField = item.id;
                            string[] Link = IdField.Split('_');
                            link1 = Link[0];
                            link2 = Link[1];
                            string link3 = "https://www.facebook.com/" + link1 + "/posts/" + link2;
                            posts.Add(new Post
                            {
                                Id = item.id,
                                NamePicture = item.from.picture.data.url,
                                Name = item.from.name,
                                LinkPost = link3,
                                Description = item.message,
                                Data = date,
                            });
                            //https://www.facebook.com/1016964581691183/posts/1047831468604494
                        }
                    }
                    else
                    {
                        if (item.picture != null)
                        {
                            date = item.created_time;
                            string[] date_table = date.Split('+');
                            date = date_table[0];
                            string[] date_table2 = date.Split('T');
                            date1 = date_table2[0];
                            date2 = date_table2[1];
                            date = date1 + " " + date2;
                            IdField = item.id;
                            string[] Link = IdField.Split('_');
                            link1 = Link[0];
                            link2 = Link[1];
                            string link3 = "https://www.facebook.com/" + link1 + "/posts/" + link2;
                            posts.Add(new Post
                            {
                                Id = item.id,
                                NamePicture = item.from.picture.data.url,
                                Name = item.from.name,
                                LinkPost = link3,
                                Data = date,
                                LinkPhotoPost = item.link,
                                Picture = item.picture,
                            });
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Zaloguj się");
            }
        }

        public static void Publish(string text, MediaFile.MediaFile mediaFile)
        {
            if (Accessik != null)
            {
                facebookClient = new FacebookClient(Accessik);
                if (text != null && mediaFile == null)
                {
                    facebookClient.Post("/me/feed", new { message = text });
                    MessageBox.Show("Opublikowano status!");
                }

                else {

                    if (text != null && mediaFile._openFileDialog.FilterIndex == 5)
                    {
                        FacebookMediaObject facebookMediaObject = new FacebookMediaObject
                        {
                            FileName = mediaFile._openFileDialog.FileName,
                            ContentType = "video/mp4"
                        };
                        facebookMediaObject.SetValue(mediaFile._fileBytes);
                        var postInfo = new Dictionary<string, object>();
                        postInfo.Add("message", text);
                        postInfo.Add("image", facebookMediaObject);
                        facebookClient.Post("/" + "117306115306848" + "/videos", postInfo);
                        MessageBox.Show("Opublikowano status z plikiem video");
                    }

                    else if (text != null)
                    {
                        FacebookMediaObject facebookMediaObject = new FacebookMediaObject
                        {
                            FileName = mediaFile._openFileDialog.FileName,
                            ContentType = "image/jpg"
                        };
                        facebookMediaObject.SetValue(mediaFile._fileBytes);
                        var postInfo = new Dictionary<string, object>();
                        postInfo.Add("message", text);
                        postInfo.Add("image", facebookMediaObject);
                        facebookClient.Post("/" + "117306115306848" + "/photos", postInfo);
                        MessageBox.Show("Opublikowano status z obrazkiem");
                    }
                }
            }
            else
            {
                MessageBox.Show("Zaloguj się");
            }
        }

        public static void LikeUnlike(object selectedPost)
        {
            FacebookClient fb = new FacebookClient(Accessik);
            if (selectedPost != null)
            {
                dynamic parametr = new ExpandoObject();
                parametr = selectedPost;
                string HiddenMyPostID = parametr.Id.ToString();

                bool like = true;

                dynamic parameters = new ExpandoObject();
                parameters.likes = like;

                dynamic liken = fb.Post(HiddenMyPostID + "/likes", parameters);
                MessageBox.Show("Like");

            }
            else
            {
                MessageBox.Show("Nie zaznaczono postu");
            }
        }

        public static void Coment(object selectedPost, string ComentText)
        {
            FacebookClient fb = new FacebookClient(Accessik);
            string txtNewComment = ComentText;
            if (selectedPost != null)
            {
                dynamic parametr = new ExpandoObject();
                parametr = selectedPost;
                string HiddenMyPostID = parametr.Id.ToString();

                dynamic parameters = new ExpandoObject();
                parameters.message = txtNewComment.Trim();

                dynamic result = fb.Post(HiddenMyPostID + "/comments", parameters);
                MessageBox.Show("Dodano post");
            }
            else
            {
                MessageBox.Show("Nie zaznaczono postu");
            }
        }
    }
}
