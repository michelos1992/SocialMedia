using Commander;
using PropertyChanged;

namespace UserInterface.ViewModel
{
    [ImplementPropertyChanged]
    public class TwitterAuthorizationViewModel
    {
        public string TwitterCaptcha { get; set; }       

        [OnCommand("AuthorizeTwitter")]
        public void AuthorizeTwitter()
        {
            Twitter.Twitter.GenerateCredentials(TwitterCaptcha);
        }

        [OnCommand("GetTwitterCaptcha")]
        public void GetTwitterCaptcha()
        {
            Twitter.Twitter.GetTwitterCaptcha();
        }
    }
}