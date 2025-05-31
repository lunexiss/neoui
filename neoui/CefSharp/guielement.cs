using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Collections.Generic;

namespace GuiBackend {
    public enum ImageScalingMode
    {
        Stretch, // fills entire bounds, may distort
        Fill,    // fills, preserving aspect ratio, may crop
        Fit,     // fits inside bounds, preserving aspect ratio
        Center   // draws at natural size, centered
    }

    // anyone knows how to make the stupid rounded borders fucking smooth?
    // if you know how to fix it, please dm '@lunexis.' in discord. thank you
    //                                              - neco

    // i fucking hate python and c#

    public class GuiElement
    {
        public Control Control { get; protected set; }

        private Image _image;
        private ImageScalingMode _scalingMode = ImageScalingMode.Stretch;

        public void SetText(string text)
        {
            if (Control != null) Control.Text = text;
        }

        public string GetText()
        {
            return Control?.Text;
        }

        public void SetOnClick(Action callback)
        {
            Control.Click += (s, e) => callback();
        }
        
        public void SetOnHover(Action callback)
        {
            Control.MouseHover += (s, e) => callback();
        }

        public void SetOnHoverLeave(Action callback)
        {
            Control.MouseLeave += (s, e) => callback();
        }

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
                        if (value.StartsWith("linear-gradient("))
                        {
                            var gradientValue = value.Substring("linear-gradient(".Length);
                            gradientValue = gradientValue.TrimEnd(')');

                            var parts = gradientValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            float angle = 0f;
                            int colorStartIndex = 0;

                            if (parts[0].Trim().EndsWith("deg"))
                            {
                                var angleStr = parts[0].Trim().Replace("deg", "");
                                float.TryParse(angleStr, out angle);
                                colorStartIndex = 1;
                            }

                            List<Color> gradientColors = new List<Color>();
                            for (int i = colorStartIndex; i < parts.Length; i++)
                            {
                                string c = parts[i].Trim();
                                try
                                {
                                    gradientColors.Add(ColorTranslator.FromHtml(c));
                                }
                                catch
                                {
                                    if (c == "red") gradientColors.Add(Color.Red);
                                    else if (c == "green") gradientColors.Add(Color.Green);
                                    else if (c == "blue") gradientColors.Add(Color.Blue);
                                    else if (c == "yellow") gradientColors.Add(Color.Yellow);
                                    else gradientColors.Add(Color.Black);
                                }
                            }

                            Control.Paint -= Control_Paint_Gradient;
                            Control.Paint += Control_Paint_Gradient;

                            Control.Tag = new GradientData
                            {
                                Angle = angle,
                                Colors = gradientColors.ToArray()
                            };

                            Control.Invalidate();
                        }
                        else if (value.StartsWith("rgba"))
                        {
                            Control.Paint -= Control_Paint_Gradient;
                            Control.Tag = null;
                            var parts = value.Replace("rgba(", "").Replace(")", "").Split(',');
                            int r = int.Parse(parts[0]);
                            int g = int.Parse(parts[1]);
                            int b = int.Parse(parts[2]);
                            float alpha = float.Parse(parts[3], CultureInfo.InvariantCulture); // 0.0 to 1.0
                            int a = (int)(alpha * 255); // convert to 0-255
                            Control.BackColor = Color.FromArgb(a, r, g, b);
                        }
                        else if (value.StartsWith("rgb"))
                        {
                            Control.Paint -= Control_Paint_Gradient;
                            Control.Tag = null;
                            var parts = value.Replace("rgb(", "").Replace(")", "").Split(',');
                            int r = int.Parse(parts[0]);
                            int g = int.Parse(parts[1]);
                            int b = int.Parse(parts[2]);
                            Control.BackColor = Color.FromArgb(r, g, b);
                        }
                        else if (value == "red")
                        {
                            Control.Paint -= Control_Paint_Gradient;
                            Control.Tag = null;
                            Control.BackColor = Color.FromArgb(255, 255, 0, 0);
                        }
                        else if (value == "green")
                        {
                            Control.Paint -= Control_Paint_Gradient;
                            Control.Tag = null;
                            Control.BackColor = Color.FromArgb(255, 0, 255, 0);
                        }
                        else if (value == "blur")
                        {
                            Control.Paint -= Control_Paint_Gradient;
                            Control.Tag = null;
                            Control.BackColor = Color.FromArgb(255, 0, 0, 255);
                        }
                        else if (value == "gray")
                        {
                            Control.Paint -= Control_Paint_Gradient;
                            Control.Tag = null;
                            Control.BackColor = Color.FromArgb(255, 150, 150, 150);
                        }
                        else if (value == "yellow")
                        {
                            Control.Paint -= Control_Paint_Gradient;
                            Control.Tag = null;
                            Control.BackColor = Color.FromArgb(255, 0, 255, 255);
                        }
                        else
                        {
                            Control.Paint -= Control_Paint_Gradient;
                            Control.Tag = null;
                            Control.BackColor = ColorTranslator.FromHtml(value);
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
                            Control.BackColor = Color.FromArgb(a, r, g, b);
                        }
                        else if (value.StartsWith("rgb"))
                        {
                            var parts = value.Replace("rgb(", "").Replace(")", "").Split(',');
                            int r = int.Parse(parts[0]);
                            int g = int.Parse(parts[1]);
                            int b = int.Parse(parts[2]);
                            Control.ForeColor = Color.FromArgb(255, r, g, b);
                        }
                        else if (value == "red")
                        {
                            Control.ForeColor = Color.FromArgb(255, 255, 0, 0);
                        }
                        else if (value == "green")
                        {
                            Control.ForeColor = Color.FromArgb(255, 0, 255, 0);
                        }
                        else if (value == "blur")
                        {
                            Control.ForeColor = Color.FromArgb(255, 0, 0, 255);
                        }
                        else if (value == "gray")
                        {
                            Control.ForeColor = Color.FromArgb(255, 150, 150, 150);
                        }
                        else if (value == "yellow")
                        {
                            Control.ForeColor = Color.FromArgb(255, 0, 255, 255);
                        }
                        else
                        {
                            Control.ForeColor = ColorTranslator.FromHtml(value);
                        }
                        break;
                    case "opacity":
                        // not supported for control, only for forms and shit
                        break;
                    case "shadow":
                        if (int.TryParse(value.Replace("px", ""), out int shadowSize))
                        {
                            shadowSize = shadowSize + 5;
                            if (Control.Parent == null)
                                break;

                            Control.Parent.Paint += (sender, e) =>
                            {
                                var g = e.Graphics;
                                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                                Rectangle rect = new Rectangle(
                                    Control.Left - shadowSize,
                                    Control.Top - shadowSize,
                                    Control.Width + shadowSize * 2,
                                    Control.Height + shadowSize * 2
                                );

                                using (GraphicsPath path = new GraphicsPath())
                                {
                                    path.AddRectangle(rect);

                                    using (PathGradientBrush brush = new PathGradientBrush(path))
                                    {
                                        brush.CenterColor = Color.FromArgb(70, 0, 0, 0);
                                        brush.SurroundColors = new Color[] { Color.FromArgb(0, 0, 0, 0) };
                                        g.FillRectangle(brush, rect);
                                    }
                                }
                            };

                            Control.Parent.Invalidate();
                        }
                        break;
                    case "font-size":
                        var currentFont = Control.Font;
                        float size = float.Parse(value.Replace("px", ""));
                        Control.Font = new Font(currentFont.FontFamily, size);
                        break;
                    case "font-family":
                        currentFont = Control.Font;
                        Control.Font = new Font(value, currentFont.Size);
                        break;
                    case "image-scaling":
                        SetImageScaling(value);
                        break;
                    case "font-weight":
                        if (value.ToLower() == "bold")
                        {
                            Control.Font = new Font(Control.Font, FontStyle.Bold);
                        }
                        else if (value.ToLower() == "normal")
                        {
                            Control.Font = new Font(Control.Font, FontStyle.Regular);
                        }
                        else if (value.ToLower() == "italic")
                        {
                            Control.Font = new Font(Control.Font, FontStyle.Italic);
                        }
                        break;
                    case "font-style":
                        if (value.ToLower() == "bold")
                        {
                            Control.Font = new Font(Control.Font, FontStyle.Bold);
                        }
                        else if (value.ToLower() == "normal")
                        {
                            Control.Font = new Font(Control.Font, FontStyle.Regular);
                        }
                        else if (value.ToLower() == "italic")
                        {
                            Control.Font = new Font(Control.Font, FontStyle.Italic);
                        }
                        break;
                    case "text-align":
                        if (Control is Label label)
                        {
                            switch (value.ToLower())
                            {
                                case "left":
                                    label.TextAlign = ContentAlignment.MiddleLeft;
                                    break;
                                case "center":
                                    label.TextAlign = ContentAlignment.MiddleCenter;
                                    break;
                                case "right":
                                    label.TextAlign = ContentAlignment.MiddleRight;
                                    break;
                                default:
                                    throw new ArgumentException("Unsupported text-align value: " + value);
                            }
                        }
                        break;
                    case "border":
                        Control.Paint += (sender, e) =>
                        {
                            try
                            {
                                var borderProp = Control.GetType().GetProperty("BorderStyle");
                                if (borderProp != null)
                                    borderProp.SetValue(Control, BorderStyle.None);
                                var parts = value.Split(' ');
                                if (parts.Length != 3) return;
                                int thickness = int.Parse(parts[0].Replace("px", ""));
                                Color borderColor = ColorTranslator.FromHtml(parts[2]);
                                using (Pen p = new Pen(borderColor, thickness))
                                {
                                    p.Alignment = PenAlignment.Inset;
                                    e.Graphics.DrawRectangle(p, 0, 0, Control.Width - 1, Control.Height - 1);
                                }
                            }
                            catch { }
                        };
                        break;
                    case "scale":
                        if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out float scale))
                        {
                            Control.Scale(new SizeF(scale, scale));
                        }
                        else
                        {
                            throw new ArgumentException("Invalid scale value: " + value);
                        }
                        break;
                    case "border-radius":
                        if (int.TryParse(value.Replace("px", ""), out int radius))
                        {
                            var borderProp = Control.GetType().GetProperty("BorderStyle");
                            if (borderProp != null)
                                borderProp.SetValue(Control, BorderStyle.None);

                            int diameter = radius * 2;
                            int w = Control.Width;
                            int h = Control.Height;

                            var path = new GraphicsPath();

                            path.AddArc(0, 0, diameter, diameter, 180, 90);

                            path.AddArc(w - diameter, 0, diameter, diameter, 270, 90);

                            path.AddArc(w - diameter, h - diameter, diameter, diameter, 0, 90);

                            path.AddArc(0, h - diameter, diameter, diameter, 90, 90);

                            path.CloseFigure();

                            Control.Region = new Region(path);
                        }
                        break;
                    case "blur":
                        if (int.TryParse(value.Replace("px", ""), out int blurRadius))
                        {
                            Control.Paint += (sender, e) =>
                            {
                                try
                                {
                                    // capture the control into a bitmap
                                    Bitmap bmp = new Bitmap(Control.Width, Control.Height);
                                    Control.DrawToBitmap(bmp, new Rectangle(0, 0, Control.Width, Control.Height));

                                    // apply the gaussian blur
                                    Bitmap blurred = GaussianBlur(bmp, blurRadius);

                                    // draw the blurred bitmap as control background
                                    e.Graphics.DrawImage(blurred, 0, 0);

                                    // dispose bitmaps
                                    bmp.Dispose();
                                    blurred.Dispose();
                                }
                                catch { }
                            };

                            // Control.Invalidate(); // trigger repaint but fucks the pc performance
                        }
                        break;
                    case "backdrop-filter":
                        if (int.TryParse(value.Replace("px", ""), out int val))
                        {
                            ApplyBackdropBlur(Control, val);
                        }
                        break;
                }
            }
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

        public void SetEnabled(bool enabled)
        {
            Control.Enabled = enabled;
        }

        public bool IsEnabled()
        {
            return Control.Enabled;
        }

        public void SetPosition(int x, int y)
        {
            if (Control != null)
            {
                Control.Left = x;
                Control.Top = y;
            }
        }

        public void SetOnTextChanged(Action callback)
        {
            Control.TextChanged += (s, e) => callback();
        }

        public bool IsVisible()
        {
            return Control?.Visible ?? false;
        }

        public void SetSize(int width, int height)
        {
            if (Control != null)
            {
                Control.Width = width;
                Control.Height = height;
                Control.Invalidate(); // redraw on resize
            }
        }

        public void Show()
        {
            if (Control != null && !Control.Visible)
            {
                Control.Visible = true;
            }
        }

        public void setImage(Image image)
        {
            _image = image;

            if (Control is PictureBox pictureBox)
            {
                // disable picturebox sizemode to handle custom painting
                pictureBox.SizeMode = PictureBoxSizeMode.Normal;
                pictureBox.Paint -= Control_Paint;
                pictureBox.Paint += Control_Paint;
                pictureBox.Invalidate();
            }
            else if (Control is Button button)
            {
                // for button, just set the fucking image normally (no custom scaling or shit )
                button.Image = image;
            }
        }

        public void SetImageScaling(string mode)
        {
            if (Control is PictureBox pictureBox)
            {
                switch (mode.ToLower())
                {
                    case "normal":
                        pictureBox.SizeMode = PictureBoxSizeMode.Normal;
                        break;
                    case "stretch":
                        pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        break;
                    case "zoom":
                        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                        break;
                    case "center":
                        pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                        break;
                    case "autosize":
                        pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                        break;
                    default:
                        throw new ArgumentException("Unsupported image scaling mode: " + mode);
                }
            }
            else if (Control is Button button)
            {
                switch (mode.ToLower())
                {
                    case "center":
                        button.ImageAlign = ContentAlignment.MiddleCenter;
                        break;
                    case "left":
                        button.ImageAlign = ContentAlignment.MiddleLeft;
                        break;
                    case "right":
                        button.ImageAlign = ContentAlignment.MiddleRight;
                        break;
                    case "top":
                        button.ImageAlign = ContentAlignment.TopCenter;
                        break;
                    case "bottom":
                        button.ImageAlign = ContentAlignment.BottomCenter;
                        break;
                    default:
                        throw new ArgumentException("Unsupported image alignment for button: " + mode);
                }
            }
        }

        public void SetPlaceholder(string text)
        {
            if (Control is TextBox textBox)
            {
                if (textBox.Tag as string == "placeholder_set") return;

                textBox.Tag = "placeholder_set"; // prevent multiple hookups

                Color defaultColor = textBox.ForeColor;
                textBox.Text = text;
                textBox.ForeColor = Color.Gray;

                textBox.GotFocus += (s, e) =>
                {
                    if (textBox.Text == text)
                    {
                        textBox.Text = "";
                        textBox.ForeColor = defaultColor;
                    }
                };

                textBox.LostFocus += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        textBox.Text = text;
                        textBox.ForeColor = Color.Gray;
                    }
                };
            }
        }

        public void ApplyBackdropBlur(Control control, int blurRadius)
        {
            // capture the background
            Bitmap bmp = new Bitmap(control.Width, control.Height);
            control.Parent.DrawToBitmap(bmp, new Rectangle(control.Location, control.Size));

            // apply this stupid shit blur
            Bitmap blurredBmp = GaussianBlur(bmp, blurRadius);

            // set this blurred shit and idk
            control.BackgroundImage = blurredBmp;
            control.BackgroundImageLayout = ImageLayout.Stretch;
        }

        public static Bitmap GaussianBlur(Bitmap image, int radius)
        {
            if (radius < 1) return image;

            Bitmap blurred = new Bitmap(image.Width, image.Height);

            // simble box blur for performance shit

            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

            System.Drawing.Imaging.BitmapData srcData = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, image.PixelFormat);
            System.Drawing.Imaging.BitmapData dstData = blurred.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly, image.PixelFormat);

            int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(image.PixelFormat) / 8;
            int stride = srcData.Stride;
            IntPtr srcScan0 = srcData.Scan0;
            IntPtr dstScan0 = dstData.Scan0;
            int width = image.Width;
            int height = image.Height;

            // i'm fucked at math

            unsafe
            {
                byte* srcPtr = (byte*)srcScan0.ToPointer();
                byte* dstPtr = (byte*)dstScan0.ToPointer();

                int kernelSize = radius * 2 + 1;
                int kernelArea = kernelSize * kernelSize;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int bSum = 0, gSum = 0, rSum = 0, aSum = 0;
                        int count = 0;

                        for (int ky = -radius; ky <= radius; ky++)
                        {
                            int py = y + ky;
                            if (py < 0 || py >= height) continue;

                            for (int kx = -radius; kx <= radius; kx++)
                            {
                                int px = x + kx;
                                if (px < 0 || px >= width) continue;

                                byte* p = srcPtr + py * stride + px * bytesPerPixel;

                                bSum += p[0];
                                gSum += p[1];
                                rSum += p[2];
                                if (bytesPerPixel == 4)
                                    aSum += p[3];
                                count++;
                            }
                        }

                        byte* dstPixel = dstPtr + y * stride + x * bytesPerPixel;
                        dstPixel[0] = (byte)(bSum / count);
                        dstPixel[1] = (byte)(gSum / count);
                        dstPixel[2] = (byte)(rSum / count);
                        if (bytesPerPixel == 4)
                            dstPixel[3] = (byte)(aSum / count);
                    }
                }
            }

            image.UnlockBits(srcData);
            blurred.UnlockBits(dstData);

            return blurred;
        }

        public void clearImage()
        {
            _image = null;

            if (Control is PictureBox pictureBox)
            {
                pictureBox.Image = null;
                pictureBox.Paint -= Control_Paint;
            }
            else if (Control is Button button)
            {
                button.Image = null;
            }
        }

        public bool IsChecked()
        {
            if (Control is CheckBox checkBox)
                return checkBox.Checked;
            else if (Control is RadioButton radioButton)
                return radioButton.Checked;
            return false;
        }

        public void SetCheck(bool check)
        {
            if (Control is CheckBox checkBox)
                checkBox.Checked = check;
            else if (Control is RadioButton radioButton)
                radioButton.Checked = check;
        }

        public void SetOnChecked(Action callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            if (Control is CheckBox checkBox)
                checkBox.CheckedChanged += (s, e) => callback();
            else if (Control is RadioButton radioButton)
                radioButton.CheckedChanged += (s, e) => callback();
        }

        public void Hide()
        {
            if (Control != null && Control.Visible)
            {
                Control.Visible = false;
            }
        }

        // custom paint handler to draw image with scaling mode on picturebox
        private void Control_Paint(object sender, PaintEventArgs e)
        {
            if (_image == null) return;

            Rectangle destRect = GetImageRect(_image, Control.ClientRectangle, _scalingMode);

            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImage(_image, destRect);
        }

        private Rectangle GetImageRect(Image image, Rectangle bounds, ImageScalingMode mode)
        {
            switch (mode)
            {
                case ImageScalingMode.Stretch:
                    return bounds;

                case ImageScalingMode.Center:
                    return new Rectangle(
                        bounds.X + (bounds.Width - image.Width) / 2,
                        bounds.Y + (bounds.Height - image.Height) / 2,
                        image.Width,
                        image.Height
                    );

                case ImageScalingMode.Fill:
                case ImageScalingMode.Fit:
                    float imageRatio = (float)image.Width / image.Height;
                    float boxRatio = (float)bounds.Width / bounds.Height;

                    int drawWidth, drawHeight;

                    if ((mode == ImageScalingMode.Fit && imageRatio > boxRatio) ||
                        (mode == ImageScalingMode.Fill && imageRatio < boxRatio))
                    {
                        drawWidth = bounds.Width;
                        drawHeight = (int)(bounds.Width / imageRatio);
                    }
                    else
                    {
                        drawHeight = bounds.Height;
                        drawWidth = (int)(bounds.Height * imageRatio);
                    }

                    return new Rectangle(
                        bounds.X + (bounds.Width - drawWidth) / 2,
                        bounds.Y + (bounds.Height - drawHeight) / 2,
                        drawWidth,
                        drawHeight
                    );

                default:
                    return bounds;
            }
        }
    }
}
