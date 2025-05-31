using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;


namespace GuiBackend {
    public class Window
    {
        private Form form;
        private List<GuiElement> elements = new List<GuiElement>();

        // yo gurt
        // why did i make this shit?
        //                  - neco

        public Window(string title, int width, int height)
        {
            form = new Form
            {
                Text = title,
                Width = width,
                Height = height,
                StartPosition = FormStartPosition.CenterScreen
            };
        }

        public Form InternalForm => form;

        private static List<GuiElement> _elements = new List<GuiElement>();

        public GuiElement AddElement(string type, int x, int y, int w, int h)
        {
            GuiElement element = null;

            switch (type.ToLower())
                {
                    case "label":
                        element = new LabelElement(x, y, w, h);
                        break;
                    case "button":
                        element = new ButtonElement(x, y, w, h);
                        break;
                    case "image":
                        element = new ImageElement(x, y, w, h);
                        break;
                    case "textbox":
                        element = new TextBoxElement(x, y, w, h);
                        break;
                    case "frame":
                        element = new FrameElement(x, y, w, h);
                        break;
                    //case "webview":           // I FUCKING HATE THIS FUCKING SHIT and well, ts ended support until i found a fucking fix
                    //    element = new WebViewElement(x, y, w, h);
                    //    break;
                    case "checkbox":
                        element = new CheckboxElement(x, y, w, h);
                        break;
                    case "radiobutton":
                    element = new RadioButtonElement(x, y, w, h);
                        break;
                    default:
                        throw new ArgumentException("Unsupported element type: " + type);
                }

                if (element != null)
                {
                    form.Controls.Add(element.Control);
                    elements.Add(element);
                }

            return element;
        }

        public void RemoveElement(GuiElement element)
        {
            if (elements.Contains(element))
            {
                form.Controls.Remove(element.Control);
                elements.Remove(element);
            }
        }

        public void SetTitle(string title)
        {
            form.Text = title;
        }

        public string GetTitle()
        {
            return form.Text;
        }

        public void SetPosition(int x, int y)
        {
            form.Location = new Point(x, y);
        }

        public void SetSize(int width, int height)
        {
            form.Width = width;
            form.Height = height;
        }

        public Form GetForm()
        {
            return form;
        }

        private class GradientData
        {
            public float Angle { get; set; }
            public Color[] Colors { get; set; }
        }

        private void Control_Paint_Gradient(object sender, PaintEventArgs e)
        {
            var ctl = sender as Control;
            if (ctl?.Tag is GradientData data && data.Colors.Length >= 2)
            {
                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    ctl.ClientRectangle,
                    data.Colors[0],
                    data.Colors[data.Colors.Length - 1],
                    data.Angle))
                {
                    e.Graphics.FillRectangle(brush, ctl.ClientRectangle);
                }
            }
        }

        // apply style

        public void ApplyStyle(string styleString)
        {
            var styles = styleString.Split(';');
            foreach (var s in styles)
            {
                var kv = s.Split(':');
                if (kv.Length != 2) continue;

                var key = kv[0].Trim().ToLower();
                var value = kv[1].Trim();

                switch (key)
                {
                    case "background":
                    case "background-color":
                        if (value.StartsWith("rgba"))
                        {
                            // extract the rgba values
                            var parts = value.Replace("rgba(", "").Replace(")", "").Split(',');
                            int r = int.Parse(parts[0]);
                            int g = int.Parse(parts[1]);
                            int b = int.Parse(parts[2]);
                            float alpha = float.Parse(parts[3], CultureInfo.InvariantCulture); // 0.0 to 1.0
                            int a = (int)(alpha * 255); // convert to 0-255
                            form.BackColor = Color.FromArgb(a, r, g, b);
                        }
                        else if (value.StartsWith("rgb"))
                        {
                            var parts = value.Replace("rgb(", "").Replace(")", "").Split(',');
                            int r = int.Parse(parts[0]);
                            int g = int.Parse(parts[1]);
                            int b = int.Parse(parts[2]);
                            form.BackColor = Color.FromArgb(r, g, b);
                        }
                        else if (value == "red")
                        {
                            form.BackColor = Color.FromArgb(255, 255, 0, 0);
                        }
                        else if (value == "green")
                        {
                            form.BackColor = Color.FromArgb(255, 0, 255, 0);
                        }
                        else if (value == "blur")
                        {
                            form.BackColor = Color.FromArgb(255, 0, 0, 255);
                        }
                        else if (value == "gray")
                        {
                            form.BackColor = Color.FromArgb(255, 150, 150, 150);
                        }
                        else if (value == "yellow")
                        {
                            form.BackColor = Color.FromArgb(255, 0, 255, 255);
                        }
                        else
                        {
                            form.BackColor = ColorTranslator.FromHtml(value);
                        }
                        break;
                    case "color":
                        if (value.StartsWith("rgba")) // copy O:
                        {
                            // extract the rgba values
                            var parts = value.Replace("rgba(", "").Replace(")", "").Split(',');
                            int r = int.Parse(parts[0]);
                            int g = int.Parse(parts[1]);
                            int b = int.Parse(parts[2]);
                            float alpha = float.Parse(parts[3], CultureInfo.InvariantCulture); // 0.0 to 1.0
                            int a = (int)(alpha * 255); // convert to 0-255
                            form.BackColor = Color.FromArgb(a, r, g, b);
                        }
                        else if (value.StartsWith("rgb"))
                        {
                            var parts = value.Replace("rgb(", "").Replace(")", "").Split(',');
                            int r = int.Parse(parts[0]);
                            int g = int.Parse(parts[1]);
                            int b = int.Parse(parts[2]);
                            form.ForeColor = Color.FromArgb(255, r, g, b);
                        }
                        else if (value == "red")
                        {
                            form.ForeColor = Color.FromArgb(255, 255, 0, 0);
                        }
                        else if (value == "green")
                        {
                            form.ForeColor = Color.FromArgb(255, 0, 255, 0);
                        }
                        else if (value == "blur")
                        {
                            form.ForeColor = Color.FromArgb(255, 0, 0, 255);
                        }
                        else if (value == "gray")
                        {
                            form.ForeColor = Color.FromArgb(255, 150, 150, 150);
                        }
                        else if (value == "yellow")
                        {
                            form.ForeColor = Color.FromArgb(255, 0, 255, 255);
                        }
                        else
                        {
                            form.ForeColor = ColorTranslator.FromHtml(value);
                        }
                        break;
                    case "opacity":
                        if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double opacity))
                            form.Opacity = opacity;
                        break;
                }
            }
        }

        private bool isFullscreen = false;
        private FormWindowState prevWindowState;
        private FormBorderStyle prevBorderStyle;
        private Rectangle prevBounds;

        public List<GuiElement> GetAllElements()
{
    return _elements;
}

        public void Fullscreen()
        {
            if (!isFullscreen)
            {
                prevWindowState = form.WindowState;
                prevBorderStyle = form.FormBorderStyle;
                prevBounds = form.Bounds;

                form.FormBorderStyle = FormBorderStyle.None;
                form.WindowState = FormWindowState.Normal;
                form.Bounds = Screen.FromControl(form).Bounds;
                isFullscreen = true;
            }
            else
            {
                form.FormBorderStyle = prevBorderStyle;
                form.WindowState = prevWindowState;
                form.Bounds = prevBounds;
                isFullscreen = false;
            }
        }
        public bool IsFullscreen()
        {
            return isFullscreen;
        }
        public void Minimize()
        {
            form.WindowState = FormWindowState.Minimized;
        }
        public void Maximize()
        {
            form.WindowState = FormWindowState.Maximized;
        }
        public void Restore()
        {
            form.WindowState = FormWindowState.Normal;
        }

        public void ShowDialog()
        {
            form.ShowDialog();
        }

        public void Run()
        {
            Application.Run(form);
        }

        public void Show()
        {
            form.Show();
        }
        
        public void Close()
        {
            form.Close();
        }

        public void Hide()
        {
            form.Hide();
        }

        public Color BackColor
        {
            get => form.BackColor;
            set => form.BackColor = value;
        }

        // expose shit
        public IntPtr Handle => form.Handle;
    }
}
