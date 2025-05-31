using System;
using System.Windows.Forms;

namespace GuiBackend
{
    public static class GuiBackend
    {
        public static Window NewWindow(string title, int width, int height)
        {
            return new Window(title, width, height);
        }
    }
}