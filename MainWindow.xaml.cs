using Microsoft.Win32;
using Shazzam.Shaders;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VideoShaderEffects
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                player.Source = new Uri(dialog.FileName, UriKind.Absolute);
                player.Play();
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var t = (sender as RadioButton)?.Content?.ToString();
            switch (t)
            {
                case "Alpha":
                    Panel_Alpha.Visibility = Visibility.Visible;
                    Panel_Tile.Visibility = Visibility.Collapsed;
                    Panel_VR.Visibility = Visibility.Collapsed;
                    StopMouse();
                    player.RenderTransformOrigin = new Point(0, 0.5);
                    player.RenderTransform = new ScaleTransform(1.0, 2.0);
                    player.Effect = new AlphaEffect();
                    break;
                case "VR":
                    Panel_Alpha.Visibility = Visibility.Collapsed;
                    Panel_Tile.Visibility = Visibility.Collapsed;
                    Panel_VR.Visibility = Visibility.Visible;
                    player.RenderTransform = null;
                    player.Effect = new VrEffect();
                    StartMouse();
                    break;
                case "Tile":
                    Panel_Alpha.Visibility = Visibility.Collapsed;
                    Panel_Tile.Visibility = Visibility.Visible;
                    Panel_VR.Visibility = Visibility.Collapsed;
                    StopMouse();
                    player.RenderTransform = null;
                    player.Effect = new TileEffect() { TargetSize = player.RenderSize };
                    break;

                default:
                    //处理透明通道视频，透明信息所在位置（左上右下）
                    //左右时，需放大2倍高度，上下时，放大2倍宽度
                    if (player.Effect is AlphaEffect alpha && !string.IsNullOrEmpty(t))
                    {
                        alpha.Position = Enum.Parse<Dock>(t);
                        switch (alpha.Position)
                        {
                            case Dock.Left:
                            case Dock.Right:
                                player.RenderTransformOrigin = new Point(0, 0.5);
                                player.RenderTransform = new ScaleTransform(1.0, 2.0);
                                break;
                            case Dock.Top:
                            case Dock.Bottom:
                                player.RenderTransformOrigin = new Point(0.5, 0);
                                player.RenderTransform = new ScaleTransform(2.0, 1.0);
                                break;
                        }
                    }
                    else
                    {
                        player.Effect = null;
                        player.RenderTransform = null;
                    }
                    break;
            }
        }

        #region VR视频使用鼠标进行旋转
        private void StartMouse()
        {
            player.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
            player.MouseMove += MainWindow_MouseMove;
        }

        Point _lastPoint;

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var point = e.GetPosition(this);
                var move = point - _lastPoint;
                _lastPoint = point;
                if (player.Effect is VrEffect effect)
                {
                    effect.RotationX += move.X / 1000.0;
                    var y = effect.RotationY - move.Y / 1000.0;
                    if (y >= -1 && y <= 0.0)
                        effect.RotationY = y;
                }
            }
        }

        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _lastPoint = e.GetPosition(this);
        }

        private void StopMouse()
        {
            player.MouseLeftButtonDown -= MainWindow_MouseLeftButtonDown;
            player.MouseMove -= MainWindow_MouseMove;
        } 
        #endregion

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var cbb = sender as ComboBox;
            var colors = typeof(Colors).GetProperties()
                .Where(d => d.PropertyType == typeof(Color))
                .Select(d => (Color)d.GetValue(null))
                .ToList();
            cbb.ItemsSource = colors;
        }

        private void player_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (player.Effect is TileEffect effect)
            {
                //设置以计算间隙的比值
                effect.TargetSize = e.NewSize;
            }
        }

        private void player_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (player.Effect is VrEffect effect)
            {
                //vr视频需传入视频的宽高，以计算视角
                effect.Size = new Size(player.NaturalVideoWidth, player.NaturalVideoHeight);
                Debug.WriteLine("VrEffect.Size:{0}", effect.Size);
            }
        }
    }
}