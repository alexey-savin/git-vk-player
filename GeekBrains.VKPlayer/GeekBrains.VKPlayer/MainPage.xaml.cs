using System;
using System.Collections.Generic;
using VK.WindowsPhone.SDK;
using VK.WindowsPhone.SDK.API;
using VK.WindowsPhone.SDK.API.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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
        private bool anotherTrack = false;

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

        private void trackRequest_TextChanged(object sender, TextChangedEventArgs e)
        {
            var requestPar = new VKRequestParameters("audio.search", "q", trackRequest.Text);
            Action<VKBackendResult<VKList<VKAudio>>> callback = (result) =>
            {
                trackView.ItemsSource = result?.Data?.items; // оператор безопасной навигации
            };

            VKRequest.Dispatch<VKList<VKAudio>>(requestPar, callback);
        }

        private void trackView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            anotherTrack = true;            
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PlaySelectedTrack();
        }

        private void player_MediaOpened(object sender, RoutedEventArgs e)
        {
            playerProgress.Maximum = player.NaturalDuration.TimeSpan.TotalSeconds;
        }

        private void player_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (trackView.SelectedIndex < trackView.Items.Count - 1)
            {
                trackView.SelectedIndex++;

                PlaySelectedTrack();
            }
        }

        private void PlaySelectedTrack()
        {
            VKAudio track = trackView.SelectedItem as VKAudio;
            if (track != null && !string.IsNullOrEmpty(track.url))
            {
                if (anotherTrack)
                {
                    player.Source = new Uri(GetTrackUrl(track.url));
                    player.Play();
                }
                else
                {
                    if (player.CurrentState == MediaElementState.Playing)
                    {
                        player.Pause();
                    }
                    else
                    {
                        player.Play();
                    }
                }
            }

            anotherTrack = false;
        }
    }
}
