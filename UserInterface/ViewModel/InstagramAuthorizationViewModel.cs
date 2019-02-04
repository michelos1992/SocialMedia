using Commander;
using MahApps.Metro.Controls;
using PropertyChanged;
using System.Windows;

namespace UserInterface.ViewModel
{
    [ImplementPropertyChanged]
    public class InstagramAuthorizationViewModel
    { 

        public string InstagramCaptcha { get; set; }

        [OnCommand("LogowanieI")]
        public void LogowanieI()
        {

        }
    }
}
