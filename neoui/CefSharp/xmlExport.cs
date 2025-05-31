using System;
using System.IO;
using System.Text;
using System.Xml;

namespace xmlExport
{   
    public static class xmlExport
{
    public static string Export(string xmlPath)
    {
        if (!File.Exists(xmlPath))
            throw new FileNotFoundException("NeoUI XML file not found.", xmlPath);

        XmlDocument doc = new XmlDocument();
        doc.Load(xmlPath);

        var sb = new StringBuilder();
        sb.AppendLine("import neoui");
        sb.AppendLine();

        XmlNode windowNode = doc.SelectSingleNode("/window");
        if (windowNode == null)
            throw new Exception("No <window> node found.");

        string title = windowNode.Attributes["title"]?.Value ?? "Window";
        int width = int.Parse(windowNode.Attributes["width"]?.Value ?? "800");
        int height = int.Parse(windowNode.Attributes["height"]?.Value ?? "600");

        sb.AppendLine($"window = Window(\"{Escape(title)}\", {width}, {height})");
        sb.AppendLine();

        int counter = 1;
        foreach (XmlNode elemNode in windowNode.SelectNodes("element"))
        {
            string type = elemNode.Attributes["type"]?.Value ?? "label";
            int x = int.Parse(elemNode.Attributes["x"]?.Value ?? "0");
            int y = int.Parse(elemNode.Attributes["y"]?.Value ?? "0");
            int w = int.Parse(elemNode.Attributes["width"]?.Value ?? "100");
            int h = int.Parse(elemNode.Attributes["height"]?.Value ?? "30");
            string text = elemNode.Attributes["text"]?.Value ?? "";

            string varName = $"{type}{counter}";
            counter++;

            sb.AppendLine($"{varName} = window.addElement(\"{type}\", {x}, {y}, {w}, {h})");

            if (!string.IsNullOrWhiteSpace(text))
                sb.AppendLine($"{varName}.setText(\"{Escape(text)}\")");

            sb.AppendLine();
        }

        sb.AppendLine("window.show()");
        return sb.ToString();
    }

    private static string Escape(string value)
    {
        return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }
}
}