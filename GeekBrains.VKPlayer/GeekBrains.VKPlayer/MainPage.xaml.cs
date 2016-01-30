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

        private string GetTrackUrl(string vkTrackUrl)
        {
            return vkTrackUrl.Substring(0, vkTrackUrl.IndexOf('?'));
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string tag = (sender as TextBlock).Tag.ToString();
            player.Source = new Uri(GetTrackUrl(tag));
            player.Play();
        }

        private void trackRequest_TextChanged(object sender, TextChangedEventArgs e)
        {
            var requestPar = new VKRequestParameters("audio.search", "q", trackRequest.Text);
            Action<VKBackendResult<VKList<VKAudio>>> callback = (result) =>
            {
                trackView.ItemsSource = result?.Data?.items; // оператор безопасной навигации
            };

            VKRequest.Dispatch<VKList<VKAudio>>(requestPar, callback);
        }
    }
}
