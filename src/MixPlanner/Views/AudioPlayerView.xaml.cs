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
using Microsoft.Practices.ServiceLocation;
using MixPlanner.Player;

namespace MixPlanner.Views
{
    /// <summary>
    /// Interaction logic for AudioPlayerView.xaml
    /// </summary>
    public partial class AudioPlayerView : UserControl
    {
        public AudioPlayerView()
        {
            
            InitializeComponent();

            ResourceDictionary themeResources = Application.LoadComponent(new Uri(@"Themes\MixPlanner.xaml", UriKind.Relative)) as ResourceDictionary;
            Resources.MergedDictionaries.Add(themeResources);

            waveFormTimeline.RegisterSoundPlayer(NAudioEngine.Instance);
        }
    }
}
