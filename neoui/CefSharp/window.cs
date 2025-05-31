using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace GuiBackend {
    public class Window
    {
        private Form form;
        private List<GuiElement> elements = new List<GuiElement>();

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
                default:
                    throw new ArgumentException("Unsupported element type: " + type);
            }

            if (element != null)
            {
                form.Controls.Add(element.Control);
                _elements.Add(element);
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

        private bool isFullscreen = false;
        private FormWindowState prevWindowState;
        private FormBorderStyle prevBorderStyle;
        private Rectangle prevBounds;

        public List<GuiElement> GetAllElements()
{
    return _elements; // Assuming you keep elements here.
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

        public void Show()
        {
            Application.Run(form);
        }

        public Color BackColor {
            get => form.BackColor;
            set => form.BackColor = value;
        }

        // expose shit
        public IntPtr Handle => form.Handle;
    }
}
