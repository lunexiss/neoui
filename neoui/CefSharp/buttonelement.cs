using System.Windows.Forms;

namespace GuiBackend {
    public class ButtonElement : GuiElement {
        public ButtonElement(int x, int y, int width, int height) {
            Control = new Button {
                Left = x,
                Top = y,
                Width = width,
                Height = height
            };
        }
    }
}
