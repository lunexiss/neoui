import clr
import os

# pythonnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn

# why are you reading this
#               - neco

# load this shit ass dll
clr.AddReference(os.path.join(os.path.dirname(__file__), "DLLs", "neoui.dll"))

from GuiBackend import GuiBackend as _GuiBackend
from GuiBackend import WindowEffect
from GuiBackend import Gui
from GuiBackend import GuiTheme
from xmlExport import xmlExport

clr.AddReference("System.Drawing")
from System.Drawing import Image, Color, Font

class Element:
    def __init__(self, csharp_element):
        self._elem = csharp_element

    def setText(self, text):
        self._elem.SetText(text)

    def getText(self):
        return self._elem.GetText()
    
    def applyStyle(self, style):
        self._elem.ApplyStyle(style)

    def setSize(self, w, h):
        self._elem.SetSize(w, h)
    
    def setPosition(self, x, y):
        self._elem.SetPosition(x, y)

    def setOnClick(self, callback):
        import System
        from System import Action
        delegate = Action(callback)
        self._elem.SetOnClick(delegate)

    def show(self):
        self._elem.Show()

    def hide(self):
        self._elem.Hide()

    def setImage(self, path: str):
        img = Image.FromFile(path)
        self._elem.setImage(img)
    
    def clearImage(self):
        self._elem.clearImage()

class Window:
    def __init__(self, title, width, height):
        self._win = _GuiBackend.NewWindow(title, width, height)
        self._elements = []

    def addElement(self, element_type, x, y, w, h):
        elem = self._win.AddElement(element_type, x, y, w, h)
        wrapped_elem = Element(elem)
        self._elements.append(wrapped_elem)  # keep reference alive
        return wrapped_elem
    
    def addEffect(self, effect_type):
        WindowEffect.ApplyEffect(self._win.Handle, effect_type)

    def setTitle(self, title):
        self._win.SetTitle(title)

    def setSize(self, w, h):
        self._win.SetSize(w, h)

    def fullscreen(self):
        self._win.Fullscreen()
        
    def setPosition(self, x, y):
        self._win.SetPosition(x, y)

    def show(self):
        self._win.Show()

    def applyTheme(self):
        theme = GuiTheme()
        theme.BackgroundColor = Color.FromArgb(30, 30, 30)
        theme.ForegroundColor = Color.White
        theme.Font = Font("Segoe UI", 10)
        theme.BorderColor = "#666666"
        theme.BorderThickness = 2
        theme.BorderRadius = 8

        #theme.ApplyThemeToAll(self._win.GetAllElements())

class MessageBox:
    def new(title, message, icon=""):
        Gui.ShowMessage(title, message, icon)

class XmlExport:
    def export(path):
        print(xmlExport.Export(path))