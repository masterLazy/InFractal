using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPF {
    /* 渲染视图 */
    public class RendererView : Control {
        // CppCLI.CLDevice r = new CppCLI.CLDevice();

        private const int chunkSize = 512; // 缓存区快的尺寸

        public double CenterR = 0.0, CenterI = 0.0; // 渲染视图中心复数坐标
        public bool RAPD = true, IAPD = true; // true 表示向上、向右
        public double Zoom = 0; // 缩放，越大看到的区间越小

        public int MaxIteration = 100; // 最大迭代次数
        public bool AntiAliasing = true; // 抗锯齿

        public int ColorTheme = 1; // 1 = 经典

        // 屏幕空间 <-> 复平面 映射
        public Point GetPoint(double r, double i) {
            Point center = new Point(ActualWidth / 2, ActualHeight / 2);
            double x, y;
            if (RAPD) {
                x = r - CenterR;
            } else {
                x = CenterR - r;
            }
            if (IAPD) {
                y = CenterI - i;
            } else {
                y = i - CenterI;
            }
            x = x * Math.Pow(2, Zoom) * chunkSize + center.X;
            y = y * Math.Pow(2, Zoom) * chunkSize + center.Y;
            return new Point(Math.Round(x), Math.Round(y));
        }
        public (double, double) GetZ(Point p) {
            Point center = new Point(ActualWidth / 2, ActualHeight / 2);
            double r = (p.X - center.X) / chunkSize * Math.Pow(2, -Zoom);
            double i = (p.Y - center.Y) / chunkSize * Math.Pow(2, -Zoom);
            if (RAPD) {
                r = CenterR + r;
            } else {
                r = CenterR - r;
            }
            if (IAPD) {
                i = CenterI - i;
            } else {
                i = CenterI + i;
            }
            return (r, i);
        }

        // 鼠标移动事件
        public (double, double) GetCursorPos(MouseEventArgs e) {
            return GetZ(e.GetPosition(this));
        }
        // 鼠标拖拽
        private bool _isDragging = false;
        private Point _lastPosition;
        public void Drag_MouseDown(MouseEventArgs e) {
            _isDragging = true;
            _lastPosition = e.GetPosition(this);
            this.CaptureMouse();
        }
        public void Drag_MouseMove(MouseEventArgs e) {
            if (_isDragging) {
                // 计算鼠标移动的距离
                Point currentPosition = e.GetPosition(this);
                Vector offset = currentPosition - _lastPosition;
                // 更新中心坐标
                CenterR -= offset.X / chunkSize * Math.Pow(2, -Zoom);
                CenterI += offset.Y / chunkSize * Math.Pow(2, -Zoom);
                // 更新最后位置
                _lastPosition = currentPosition;
                this.InvalidateVisual();
            }
        }
        public void Drag_MouseUp(MouseButtonEventArgs e) {
            if (_isDragging) {
                _isDragging = false;
                this.ReleaseMouseCapture();
            }
        }
        // 重写渲染事件
        protected override void OnRender(DrawingContext drawingContext) {
            base.OnRender(drawingContext);
            // 确定使用的缩放
            double usingZoom = Zoom;
            if (AntiAliasing) {
                usingZoom++;
            }
            double zoomStride = Math.Pow(2, -usingZoom); // 当前缩放在复平面中的步长
            double chunkActualSize = 512 * Math.Pow(2, Zoom - usingZoom); // 区块的实际尺寸
            // 确定边界区块
            (double zL, double zB) = GetZ(new Point(0, ActualHeight));
            (double zR, double zT) = GetZ(new Point(ActualWidth, 0));
            (int l, int b) = ((int)Math.Floor(zL / zoomStride), (int)Math.Floor(zB / zoomStride)); // Floor 不可省略 因为有负数
            (int r, int t) = ((int)Math.Ceiling(zR / zoomStride), (int)Math.Ceiling(zT / zoomStride));
            Rect rect;
            Point posLB;
            for (int x = l; x <= r; x++) {
                for (int y = b; y <= t; y++) {
                    posLB = GetPoint(zoomStride * x, zoomStride * (y + 1));
                    rect = new Rect(posLB, new Point(posLB.X + chunkActualSize, posLB.Y + chunkActualSize));
                    DrawChunk(drawingContext, (int)usingZoom, x, y, rect);
                }
            }
        }

        private void DrawChunk(DrawingContext drawingContext, int zoom, int idxX, int idxY, Rect rect) {
            // 绘制区块
            var img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri($"D:\\Projects\\无尽分形\\cache\\M\\{zoom}_{idxX}_{idxY}.png", UriKind.Relative);
            img.EndInit();
            bool imgLoaded = false;
            try {
                if (img.PixelWidth > 0 && img.PixelHeight > 0) {
                    imgLoaded = true;
                }
            }
            catch { }
            if (imgLoaded) {
                drawingContext.DrawImage(img, rect);
            } else {
                drawingContext.DrawRectangle(Brushes.Transparent, new Pen(Brushes.White, 1), rect);
                drawingContext.DrawLine(new Pen(Brushes.White, 1), new Point(rect.Left, rect.Top), new Point(rect.Right, rect.Bottom));
                drawingContext.DrawLine(new Pen(Brushes.White, 1), new Point(rect.Right, rect.Top), new Point(rect.Left, rect.Bottom));
                drawingContext.DrawText(new FormattedText(
                    $"{zoom}_{idxX}_{idxY}",
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(new FontFamily("Arial"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
                    18, Brushes.White
                    ), new Point((rect.Left + rect.Right) / 2, (rect.Top + rect.Bottom) / 2));
            }
        }
    }
}
