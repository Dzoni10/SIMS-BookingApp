using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BookingApp.WPF.View
{
    /// <summary>
    /// Interaction logic for TutorialReserve.xaml
    /// </summary>
    public partial class TutorialReserve : Page
    {
        private bool IsPlaying = false;
        private bool IsUserDraggingSlider = false;

        private readonly DispatcherTimer Timer = new() { Interval = TimeSpan.FromSeconds(0.1) };
        public TutorialReserve()
        {
            InitializeComponent();
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        public TutorialReserve(string videoPath) : this()
        {
            if (!string.IsNullOrEmpty(videoPath))
            {
                Player.Source = new Uri(videoPath, UriKind.RelativeOrAbsolute);
            }
        }
        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (Player.Source != null && Player.NaturalDuration.HasTimeSpan && !IsUserDraggingSlider)
            {
                ProgressSlider.Maximum = Player.NaturalDuration.TimeSpan.TotalSeconds;
                ProgressSlider.Value = Player.Position.TotalSeconds;
            }
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Player?.Source != null)
            {
                Player.Play();
                IsPlaying = true;
            }
        }

        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsPlaying)
                Player.Pause();
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsPlaying)
            {
                Player.Stop();
                IsPlaying = false;
            }
        }

        private void ProgressSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            IsUserDraggingSlider = true;
        }

        private void ProgressSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            IsUserDraggingSlider = false;
            Player.Position = TimeSpan.FromSeconds(ProgressSlider.Value);
        }

        private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            StatusLbl.Text = TimeSpan.FromSeconds(ProgressSlider.Value).ToString(@"hh\:mm\:ss");
        }
    }
}
