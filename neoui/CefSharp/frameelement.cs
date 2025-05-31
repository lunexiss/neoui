using System.Drawing;
using System.Windows.Forms;

namespace GuiBackend {
    public class FrameElement : GuiElement {
        public FrameElement(int x, int y, int width, int height) {
            Control = new Panel {
                Left = x,
                Top = y,
                Width = width,
                Height = height
            };
        }
    }
}
