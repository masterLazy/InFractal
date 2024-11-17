using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPF {
    /* 渲染视图 */
    public class RendererView : Control {
        // CppCLI.CLDevice r = new CppCLI.CLDevice();

        public double CenterR = 0.0, CenterI = 0.0; // 渲染视图中心复数坐标
        public bool RAPD = true, IAPD = true; // true 表示向上、向右
        public double Zoom = 0;
        public int MaxIteration = 100;
        public bool AntiAliasing = true;
        public int ColorTheme = 1; // 1=经典

        // 鼠标移动事件
        public (double, double) GetCursorPos(MouseEventArgs e) {
            Point center = new Point(ActualWidth / 2, ActualHeight / 2); // 渲染窗口中心坐标
            double CurR = (e.GetPosition(this).X - center.X) / 512.0 / Math.Pow(2, Zoom) + CenterR;
            double CurI = (center.Y - e.GetPosition(this).Y) / 512.0 / Math.Pow(2, Zoom) + CenterI;
            return (CurR, CurI);
        }
        // 重写渲染事件
        protected override void OnRender(DrawingContext drawingContext) {
            base.OnRender(drawingContext);

            // 当前缩放下的区块步长
            double zoomStride = Math.Pow(2, Zoom);

            // 复平面中离中心点最近的区块
            int centerChunkX = (int)(CenterR / zoomStride);
            int centerChunkY = (int)(CenterI / zoomStride);

            Point center = new Point(ActualWidth / 2, ActualHeight / 2); // 渲染窗口中心坐标
            int chunkCntX = (int)Math.Ceiling((ActualWidth - center.X) / 512) * 2; // 横向区块数
            int chunkCntY = (int)Math.Ceiling((ActualHeight - center.Y) / 512) * 2; // 纵向区块数

            DrawChunk(drawingContext, (int)Zoom, 0, 0, center);
        }

        private void DrawChunk(DrawingContext drawingContext, int zoom, int idxX, int idxY, Point pos) {
            // 确定区块(左下角)在复平面中的坐标
            double startR = Math.Pow(2, zoom) * idxX;
            double startI = Math.Pow(2, zoom) * idxY;

            // 绘制区块
            var img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri($"cache\\M\\{zoom}_{idxX}_{idxY}.png", UriKind.Relative);
            img.EndInit();
            bool imgLoaded = false;
            try {
                if (img.PixelWidth > 0 && img.PixelHeight > 0) {
                    imgLoaded = true;
                }
            }
            catch { }
            var rect = new Rect(pos, new Point(pos.X + 512, pos.Y + 512));
            if (imgLoaded) {
                drawingContext.DrawImage(img, rect);
            } else {
                drawingContext.DrawRectangle(Brushes.Transparent, new Pen(Brushes.White, 1), rect);
            }
        }
    }
}
