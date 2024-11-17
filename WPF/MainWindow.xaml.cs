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
            RendererInstance.MaxIteration = (int)MaxIteration.Value;
        }
        private void RAPDCb_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (((ComboBox)sender).SelectedIndex == 0) {
                RendererInstance.RAPD = true;
            } else {
                RendererInstance.RAPD = false;
            }
        }
        private void IAPDCb_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (((ComboBox)sender).SelectedIndex == 0) {
                RendererInstance.IAPD = true;
            } else {
                RendererInstance.IAPD = false;
            }
        }
        private void ColorTheme_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            RendererInstance.ColorTheme = ((ComboBox)sender).SelectedIndex + 1;
        }

        /* 渲染视图 */
        private void RendererView_MouseMove(object sender, MouseEventArgs e) {
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
        }


        public RendererView RendererInstance = new RendererView();
    }
}