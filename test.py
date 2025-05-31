import neoui

# initialize window
window = neoui.Window("neoui test", 1000, 700)
# window.applyStyle("background-color: #2c2c2c; color: white; font-family: Segoe UI;")

# label
label = window.addElement("label", 40, 30, 300, 40)
label.setText("testing all neoui components")
# label.applyStyle("font-size: 18px; color: #ffffff;")

# button
button = window.addElement("button", 40, 90, 150, 50)
button.setText("click me")
# button.applyStyle("font-size: 14px; background-color: #ffc371; shadow: 10px; color: white;")

# image (placeholder path)
# image = window.addElement("image", 220, 90, 100, 100)
# image.setImage("assets/test_image.png")  # ensure this path is valid in your app
# image.applyStyle("shadow: 6px;")

# textbox
textbox = window.addElement("textbox", 40, 170, 300, 100)
# extbox.applyStyle("background-color: #444; font-size: 12px; color: #f0f0f0; shadow: 4px;")

# checkbox
checkbox = window.addElement("checkbox", 420, 90, 200, 40)
checkbox.setText("enable feature")
# checkbox.applyStyle("font-size: 13px;")

# radiobuttons
radio1 = window.addElement("radiobutton", 420, 140, 200, 40)
radio1.setText("Option A")
# radio1.applyStyle("font-size: 13px;")

radio2 = window.addElement("radiobutton", 420, 190, 200, 40)
radio2.setText("Option B")
# radio2.applyStyle("font-size: 13px;")

radio3 = window.addElement("radiobutton", 420, 240, 200, 40)
radio3.setText("Option C")
# radio3.applyStyle("font-size: 13px;")

# footer label
footer = window.addElement("label", 40, 620, 300, 30)
footer.setText("Wassup")
# footer.applyStyle("font-size: 12px; color: gray;")

# frame (container look)
frame = window.addElement("frame", 400, 60, 550, 450)
# frame.applyStyle("background-color: #303030; shadow: 10px;")

# window.addEffect("mica") # for visual shit

# Run the app
window.run()
