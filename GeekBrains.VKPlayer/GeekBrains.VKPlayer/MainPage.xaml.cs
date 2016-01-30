using System;
using System.Collections.Generic;
using VK.WindowsPhone.SDK;
using VK.WindowsPhone.SDK.API;
using VK.WindowsPhone.SDK.API.Model;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу http://go.microsoft.com/fwlink/?LinkId=391641

namespace GeekBrains.VKPlayer
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private List<string> _scopeList = new List<string>() { VKScope.AUDIO };

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            VKSDK.Initialize("5116069");
            VKSDK.Authorize(_scopeList, false, false);
        }

        private void button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            trackView.Items.Clear();
            var requestPar = new VKRequestParameters("audio.search", "q", trackRequest.Text);
            Action<VKBackendResult<VKList<VKAudio>>> callback = (result) => 
            {
                foreach (var track in result.Data.items)
                {
                    trackView.Items.Add($"{track.artist} - {track.title}");
                }
            };

            VKRequest.Dispatch<VKList<VKAudio>>(requestPar, callback);
        }
    }
}
