using System.Drawing;
using System.Windows.Forms;

namespace GuiBackend {
    public class CheckboxElement : GuiElement {
        public CheckboxElement(int x, int y, int width, int height) {
            Control = new CheckBox {
                Left = x,
                Top = y,
                Width = width,
                Height = height
            };
        }
    }
}
