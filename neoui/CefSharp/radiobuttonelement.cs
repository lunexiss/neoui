using System.Drawing;
using System.Windows.Forms;

namespace GuiBackend {
    public class RadioButtonElement : GuiElement {
        public RadioButtonElement(int x, int y, int width, int height) {
            Control = new RadioButton {
                Left = x,
                Top = y,
                Width = width,
                Height = height
            };
        }
    }
}
