using System.Windows.Forms;

namespace GuiBackend {
    public class TextBoxElement : GuiElement {
        public TextBoxElement(int x, int y, int width, int height) {
            Control = new TextBox {
                Left = x,
                Top = y,
                Width = width,
                Height = height
            };
        }
    }
}
