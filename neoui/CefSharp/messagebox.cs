using System.Windows.Forms;

namespace GuiBackend
{
    public class Gui
    {
        public static void ShowMessage(string title, string message, string icon = "info")
        {
            MessageBoxIcon messageBoxIcon;
            switch (icon)
            {
                case "info":
                    messageBoxIcon = MessageBoxIcon.Information;
                    break;
                case "warning":
                    messageBoxIcon = MessageBoxIcon.Warning;
                    break;
                case "error":
                    messageBoxIcon = MessageBoxIcon.Error;
                    break;
                case "question":
                    messageBoxIcon = MessageBoxIcon.Question;
                    break;
                default:
                    messageBoxIcon = MessageBoxIcon.None;
                    break;
            }
            MessageBox.Show(message, title, MessageBoxButtons.OK, messageBoxIcon);
        }
    }
}
