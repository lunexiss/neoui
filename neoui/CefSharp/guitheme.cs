using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GuiBackend
{
    public class GuiTheme
    {
        public Color BackgroundColor { get; set; }
        public Color ForegroundColor { get; set; }
        public Font Font { get; set; }
        // public string ImageScaling { get; set; } = "stretch";
        public string BorderColor { get; set; }
        public int BorderThickness { get; set; } = 0;
        public int BorderRadius { get; set; } = 0;

        public void ApplyTheme(GuiElement element)
        {
            if (element == null || element.Control == null) return;

            var control = element.Control;

            control.BackColor = BackgroundColor;
            control.ForeColor = ForegroundColor;
            control.Font = Font;

            // element.SetImageScaling(ImageScaling);

            if (BorderThickness > 0 && !string.IsNullOrEmpty(BorderColor))
            {
                control.Paint += (sender, e) =>
                {
                    using (Pen p = new Pen(ColorTranslator.FromHtml(BorderColor), BorderThickness))
                    {
                        p.Alignment = PenAlignment.Inset;
                        e.Graphics.DrawRectangle(p, 0, 0, control.Width - 1, control.Height - 1);
                    }
                };
            }

            if (BorderRadius > 0)
            {
                control.Resize += (sender, e) =>
                {
                    var path = new GraphicsPath();
                    int w = control.Width;
                    int h = control.Height;
                    int r = BorderRadius;

                    path.AddArc(0, 0, r, r, 180, 90);
                    path.AddArc(w - r, 0, r, r, 270, 90);
                    path.AddArc(w - r, h - r, r, r, 0, 90);
                    path.AddArc(0, h - r, r, r, 90, 90);
                    path.CloseAllFigures();

                    control.Region = new Region(path);
                };
            }
        }

        public void ApplyThemeToAll(IEnumerable<GuiElement> elements)
        {
            if (elements == null) return;

            foreach (var element in elements)
            {
                ApplyTheme(element);
            }
        }
    }
}