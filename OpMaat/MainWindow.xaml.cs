using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Win32;

namespace OpMaat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public VideoPlayer playerWindow;
        public List<Uri> videoList;

        public MainWindow()
        {
            InitializeComponent();
            playerWindow = new VideoPlayer();
            videoList = new List<Uri>();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listBox.Items.Add("Nog geen bestand(en) gekozen");
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void uploadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                DefaultExt = ".mp4",
                Filter = "MP4 files (*.mp4)|*.mp4",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            if (openFileDialog.ShowDialog() != true) return;

            foreach (var fileName in openFileDialog.FileNames)
            {
                if(videoList.Count == 0)
                    listBox.Items.Clear();
                videoList.Add(new Uri(fileName));
                listBox.Items.Add(fileName);
            }

            BestandLabel.Visibility = Visibility.Visible;
            StartButton.IsEnabled = true;
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            playerWindow.VideoList = videoList;
            playerWindow.mediaPlayer.Source = videoList[0];
            playerWindow.Show();
            var seconden = 10;
            try
            {
                seconden = int.Parse(PauzeTextBox.Text);
            }
            catch (Exception ex)
            {
                // ignored
            }
            playerWindow.PauzeTimer.Interval = seconden * 1000;
            playerWindow.PauzeTimer.Start();
            playerWindow.mediaPlayer.Play();

            this.Close();
        }

        private new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            var regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }
    }
}
