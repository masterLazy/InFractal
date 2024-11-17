using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPF {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        /* 分隔栏 */
        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e) {
            Sidebar.BeginAnimation(WidthProperty, null); // 停止动画
            double newWidth = Sidebar.ActualWidth - e.HorizontalChange;
            if (newWidth >= SidebarMinWidth) {
                Sidebar.Width = Math.Min(newWidth, 600);
            }
        }
        private void Thumb_MouseEnter(object sender, MouseEventArgs e) {
            this.Cursor = Cursors.SizeWE;
        }
        private void Thumb_MouseLeave(object sender, MouseEventArgs e) {
            this.Cursor = Cursors.Arrow;
        }

        /* 侧栏 */
        const double SidebarMinWidth = 300;
        double SidebarWidth = 300;
        const double AnimateSeconds = 0.33;
        private void OpenSidebar(object sender, RoutedEventArgs e) {
            DoubleAnimation animation = new DoubleAnimation {
                From = -SidebarWidth,
                To = 0,
                Duration = TimeSpan.FromSeconds(AnimateSeconds),
                EasingFunction = new CircleEase { EasingMode = EasingMode.EaseOut }
            };
            Sidebar.BeginAnimation(Canvas.RightProperty, animation);
            OptionButton.Visibility = Visibility.Hidden;
            Thumb.Width = 5;
        }
        private void CloseSidebar(object sender, RoutedEventArgs e) {
            SidebarWidth = Sidebar.ActualWidth;
            DoubleAnimation animation = new DoubleAnimation {
                From = 0,
                To = -SidebarWidth,
                Duration = TimeSpan.FromSeconds(AnimateSeconds),
                EasingFunction = new CircleEase { EasingMode = EasingMode.EaseOut }
            };
            Sidebar.BeginAnimation(Canvas.RightProperty, animation);
            OptionButton.Visibility = Visibility.Visible;
            Thumb.Width = 0;
        }

        /* 细节选项 */
        private void MaxIteration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            RendererV.MaxIteration = (int)MaxIteration.Value;
        }
        private void RAPDCb_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (((ComboBox)sender).SelectedIndex == 0) {
                RendererV.RAPD = true;
            } else {
                RendererV.RAPD = false;
            }
            RendererV.InvalidateVisual();
        }
        private void IAPDCb_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (((ComboBox)sender).SelectedIndex == 0) {
                RendererV.IAPD = true;
            } else {
                RendererV.IAPD = false;
            }
            RendererV.InvalidateVisual();
        }
        private void AACb_Changed(object sender, RoutedEventArgs e) {
            if (AACb.IsChecked == true) {
                RendererV.AntiAliasing = true;
            } else {
                RendererV.AntiAliasing = false;
            }
            RendererV.InvalidateVisual();
        }
        private void ColorTheme_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            RendererV.ColorTheme = ((ComboBox)sender).SelectedIndex + 1;
        }

        /* 渲染视图 */
        private void RendererV_MouseDown(object sender, MouseButtonEventArgs e) {
            RendererV.Drag_MouseDown(e);
        }
        private void RendererV_MouseMove(object sender, MouseEventArgs e) {
            double r, i;
            (r, i) = ((RendererView)sender).GetCursorPos(e);
            StringBuilder sb = new StringBuilder();
            if (r >= 0) {
                sb.Append("+ ");
            } else {
                sb.Append("- ");
            }
            sb.Append(Math.Abs(r).ToString($"F{9}"));
            sb.Append("\n");
            if (i >= 0) {
                sb.Append("+ ");
            } else {
                sb.Append("- ");
            }
            sb.Append(Math.Abs(i).ToString($"F{9}"));
            sb.Append(" i");
            CurPos.Text = sb.ToString();
            RendererV.Drag_MouseMove(e);
        }
        private void RendererV_MouseUp(object sender, MouseButtonEventArgs e) {
            RendererV.Drag_MouseUp(e);
        }
        private void RendererV_MouseWheel(object sender, MouseWheelEventArgs e) {
            if (e.Delta > 0) {
                RendererV.Zoom++;
            } else {
                RendererV.Zoom--;
            }
            RendererV.InvalidateVisual();
        }
    }
}