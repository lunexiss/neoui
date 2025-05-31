using System.Windows.Forms;

namespace GuiBackend {
    public class LabelElement : GuiElement {
        public LabelElement(int x, int y, int width, int height) {
            Control = new Label {
                Left = x,
                Top = y,
                Width = width,
                Height = height
            };
        }
    }
}
