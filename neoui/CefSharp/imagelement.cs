using System.Windows.Forms;

namespace GuiBackend {
    public class ImageElement : GuiElement {
        public ImageElement(int x, int y, int width, int height) {
            Control = new PictureBox {
                Left = x,
                Top = y,
                Width = width,
                Height = height
            };
        }
    }
}
