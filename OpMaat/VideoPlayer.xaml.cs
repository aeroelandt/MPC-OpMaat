using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace OpMaat
{
    /// <summary>
    /// Interaction logic for VideoPlayer.xaml
    /// </summary>
    public partial class VideoPlayer : Window
    {
        public int CurrentItem = 1;
        public readonly Timer PauzeTimer = new Timer();
        public List<Uri> VideoList;

        public VideoPlayer()
        {
            KeyDown += mediaPlayer_KeyDown;
            InitializeComponent();
            PauzeTimer.Elapsed += pauzeTimer_Elapsed;
            mediaPlayer.MediaEnded += mediaPlayer_ended;
        }

        private void MediaPlayer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!PauzeTimer.Enabled)
            {
                mediaPlayer.Play();
                PauzeTimer.Start();
            }
        }

        private void mediaPlayer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                MainWindow window = new MainWindow();
                mediaPlayer.Pause();
                mediaPlayer.Source = null;
                window.Show();
                Close();
            }
        }

        private void pauzeTimer_Elapsed(object source, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                mediaPlayer.Pause();
            });
            PauzeTimer.Stop();
        }

        private void mediaPlayer_ended(object source, RoutedEventArgs e)
        {
            if (CurrentItem < VideoList.Count)
            {
                mediaPlayer.Close();
                mediaPlayer.Source = VideoList[CurrentItem];
                mediaPlayer.Play();

                CurrentItem++;
            }
            else
            {
                var window = new MainWindow();

                mediaPlayer.Source = null;
                window.Show();
                Close();
            }
        }

    }
}
