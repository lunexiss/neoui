import neoui

window = neoui.Window("title", 800, 600)
label = window.addElement("label", 10, 10, 200, 30)
label.setText("hi")

button = window.addElement("button", 60, 100, 200, 30)
button.setText("Click Me")

button.applyStyle("backdrop-filter: 1px")
button.applyStyle("background: rgba(0, 0, 0, 0)")

def test():
    neoui.MessageBox.new("Test", "Button clicked!", "info")

    # button.applyStyle("border-radius: 100px")

    label.hide()
    window.applyTheme()

button.setOnClick(test)

image = window.addElement("image", 300, 100, 200, 200)
image.setImage("c:/Users/Админ/Downloads/9db6e301-9bcb-4fba-8d8b-6b8298f45591.jpeg")
image.applyStyle("border-radius: 5px; image-scaling: stretch; blur: 1px")

textedit = window.addElement("textbox", 500, 300, 50, 50)

neoui.XmlExport.export(r"C:\Users\Админ\OneDrive\Документы\Projects\neoui\test.xml")

# window.applyTheme()

# window.addEffect("blur")

window.show()
