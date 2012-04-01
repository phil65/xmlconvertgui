<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Filechooser
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.OpenFileDialog = New System.Windows.Forms.OpenFileDialog()
        Me.ConvertButton = New System.Windows.Forms.Button()
        Me.xmlname = New System.Windows.Forms.Label()
        Me.OutputButton = New System.Windows.Forms.Button()
        Me.OutputFolderDialog = New System.Windows.Forms.FolderBrowserDialog()
        Me.OutputLabel = New System.Windows.Forms.Label()
        Me.OutputLog = New System.Windows.Forms.TextBox()
        Me.ConversionDropDown = New System.Windows.Forms.ComboBox()
        Me.HeaderOption = New System.Windows.Forms.CheckBox()
        Me.EncodingDropDown = New System.Windows.Forms.ComboBox()
        Me.DebugOutput = New System.Windows.Forms.CheckBox()
        Me.ConvertBorders = New System.Windows.Forms.CheckBox()
        Me.TextureCheckButton = New System.Windows.Forms.Button()
        Me.SkinFolderButton = New System.Windows.Forms.Button()
        Me.SkinFolderDialog = New System.Windows.Forms.FolderBrowserDialog()
        Me.ClearLogButton = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Splitter1 = New System.Windows.Forms.Splitter()
        Me.CheckIncludesButton = New System.Windows.Forms.Button()
        Me.IndentingDropDown = New System.Windows.Forms.ComboBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.BuildFolderDialog = New System.Windows.Forms.FolderBrowserDialog()
        Me.EOLComboBox = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'OpenFileDialog
        '
        Me.OpenFileDialog.FileName = "TexturePackerDialog"
        '
        'ConvertButton
        '
        Me.ConvertButton.Enabled = False
        Me.ConvertButton.Location = New System.Drawing.Point(202, 322)
        Me.ConvertButton.Name = "ConvertButton"
        Me.ConvertButton.Size = New System.Drawing.Size(75, 23)
        Me.ConvertButton.TabIndex = 3
        Me.ConvertButton.Text = "Convert"
        Me.ConvertButton.UseVisualStyleBackColor = True
        '
        'xmlname
        '
        Me.xmlname.AutoSize = True
        Me.xmlname.Location = New System.Drawing.Point(163, 25)
        Me.xmlname.Name = "xmlname"
        Me.xmlname.Size = New System.Drawing.Size(99, 13)
        Me.xmlname.TabIndex = 4
        Me.xmlname.Text = "Choose Skin Folder"
        '
        'OutputButton
        '
        Me.OutputButton.Location = New System.Drawing.Point(202, 229)
        Me.OutputButton.Name = "OutputButton"
        Me.OutputButton.Size = New System.Drawing.Size(75, 24)
        Me.OutputButton.TabIndex = 5
        Me.OutputButton.Text = "Choose"
        Me.OutputButton.UseVisualStyleBackColor = True
        Me.OutputButton.Visible = False
        '
        'OutputLabel
        '
        Me.OutputLabel.Location = New System.Drawing.Point(193, 194)
        Me.OutputLabel.Name = "OutputLabel"
        Me.OutputLabel.Size = New System.Drawing.Size(152, 24)
        Me.OutputLabel.TabIndex = 6
        Me.OutputLabel.Text = "Choose Output Folder"
        Me.OutputLabel.Visible = False
        '
        'OutputLog
        '
        Me.OutputLog.Location = New System.Drawing.Point(351, 22)
        Me.OutputLog.Multiline = True
        Me.OutputLog.Name = "OutputLog"
        Me.OutputLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.OutputLog.Size = New System.Drawing.Size(559, 317)
        Me.OutputLog.TabIndex = 7
        '
        'ConversionDropDown
        '
        Me.ConversionDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ConversionDropDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ConversionDropDown.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.ConversionDropDown.Location = New System.Drawing.Point(9, 194)
        Me.ConversionDropDown.Name = "ConversionDropDown"
        Me.ConversionDropDown.Size = New System.Drawing.Size(121, 21)
        Me.ConversionDropDown.TabIndex = 8
        '
        'HeaderOption
        '
        Me.HeaderOption.AutoSize = True
        Me.HeaderOption.Location = New System.Drawing.Point(9, 322)
        Me.HeaderOption.Name = "HeaderOption"
        Me.HeaderOption.Size = New System.Drawing.Size(108, 17)
        Me.HeaderOption.TabIndex = 9
        Me.HeaderOption.Text = "Add XML Header"
        Me.HeaderOption.UseVisualStyleBackColor = True
        '
        'EncodingDropDown
        '
        Me.EncodingDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.EncodingDropDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.EncodingDropDown.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.EncodingDropDown.Location = New System.Drawing.Point(9, 229)
        Me.EncodingDropDown.Name = "EncodingDropDown"
        Me.EncodingDropDown.Size = New System.Drawing.Size(121, 21)
        Me.EncodingDropDown.TabIndex = 10
        '
        'DebugOutput
        '
        Me.DebugOutput.AutoSize = True
        Me.DebugOutput.Location = New System.Drawing.Point(9, 345)
        Me.DebugOutput.Name = "DebugOutput"
        Me.DebugOutput.Size = New System.Drawing.Size(115, 17)
        Me.DebugOutput.TabIndex = 11
        Me.DebugOutput.Text = "Enable Debug Log"
        Me.DebugOutput.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.DebugOutput.UseVisualStyleBackColor = True
        '
        'ConvertBorders
        '
        Me.ConvertBorders.AutoSize = True
        Me.ConvertBorders.Location = New System.Drawing.Point(9, 368)
        Me.ConvertBorders.Name = "ConvertBorders"
        Me.ConvertBorders.Size = New System.Drawing.Size(102, 17)
        Me.ConvertBorders.TabIndex = 12
        Me.ConvertBorders.Text = "Convert Borders"
        Me.ConvertBorders.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ConvertBorders.UseVisualStyleBackColor = True
        '
        'TextureCheckButton
        '
        Me.TextureCheckButton.Location = New System.Drawing.Point(49, 59)
        Me.TextureCheckButton.Name = "TextureCheckButton"
        Me.TextureCheckButton.Size = New System.Drawing.Size(95, 23)
        Me.TextureCheckButton.TabIndex = 13
        Me.TextureCheckButton.Text = "Check Textures"
        Me.TextureCheckButton.UseVisualStyleBackColor = True
        '
        'SkinFolderButton
        '
        Me.SkinFolderButton.Location = New System.Drawing.Point(58, 20)
        Me.SkinFolderButton.Name = "SkinFolderButton"
        Me.SkinFolderButton.Size = New System.Drawing.Size(75, 23)
        Me.SkinFolderButton.TabIndex = 14
        Me.SkinFolderButton.Text = "Choose"
        Me.SkinFolderButton.UseVisualStyleBackColor = True
        '
        'ClearLogButton
        '
        Me.ClearLogButton.Location = New System.Drawing.Point(835, 345)
        Me.ClearLogButton.Name = "ClearLogButton"
        Me.ClearLogButton.Size = New System.Drawing.Size(75, 23)
        Me.ClearLogButton.TabIndex = 15
        Me.ClearLogButton.Text = "Clear Log"
        Me.ClearLogButton.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(49, 88)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(95, 23)
        Me.Button1.TabIndex = 16
        Me.Button1.Text = "Check Fonts"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(75, 155)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(143, 16)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Conversion Options"
        '
        'Splitter1
        '
        Me.Splitter1.Location = New System.Drawing.Point(0, 0)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(3, 619)
        Me.Splitter1.TabIndex = 18
        Me.Splitter1.TabStop = False
        '
        'CheckIncludesButton
        '
        Me.CheckIncludesButton.Location = New System.Drawing.Point(49, 117)
        Me.CheckIncludesButton.Name = "CheckIncludesButton"
        Me.CheckIncludesButton.Size = New System.Drawing.Size(95, 23)
        Me.CheckIncludesButton.TabIndex = 19
        Me.CheckIncludesButton.Text = "Check Includes"
        Me.CheckIncludesButton.UseVisualStyleBackColor = True
        '
        'IndentingDropDown
        '
        Me.IndentingDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.IndentingDropDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.IndentingDropDown.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.IndentingDropDown.Location = New System.Drawing.Point(9, 262)
        Me.IndentingDropDown.Name = "IndentingDropDown"
        Me.IndentingDropDown.Size = New System.Drawing.Size(121, 21)
        Me.IndentingDropDown.TabIndex = 20
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(78, 419)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(154, 23)
        Me.Button2.TabIndex = 21
        Me.Button2.Text = "Choose TexutePacker EXE"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(78, 465)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(154, 23)
        Me.Button3.TabIndex = 22
        Me.Button3.Text = "Choose BUILD Path"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(78, 508)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(154, 23)
        Me.Button4.TabIndex = 23
        Me.Button4.Text = "Start bat file"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'EOLComboBox
        '
        Me.EOLComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.EOLComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.EOLComboBox.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.EOLComboBox.Location = New System.Drawing.Point(9, 295)
        Me.EOLComboBox.Name = "EOLComboBox"
        Me.EOLComboBox.Size = New System.Drawing.Size(121, 21)
        Me.EOLComboBox.TabIndex = 24
        '
        'Filechooser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(913, 619)
        Me.Controls.Add(Me.EOLComboBox)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.IndentingDropDown)
        Me.Controls.Add(Me.CheckIncludesButton)
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ClearLogButton)
        Me.Controls.Add(Me.SkinFolderButton)
        Me.Controls.Add(Me.TextureCheckButton)
        Me.Controls.Add(Me.ConvertBorders)
        Me.Controls.Add(Me.DebugOutput)
        Me.Controls.Add(Me.EncodingDropDown)
        Me.Controls.Add(Me.HeaderOption)
        Me.Controls.Add(Me.ConversionDropDown)
        Me.Controls.Add(Me.OutputLog)
        Me.Controls.Add(Me.OutputLabel)
        Me.Controls.Add(Me.OutputButton)
        Me.Controls.Add(Me.xmlname)
        Me.Controls.Add(Me.ConvertButton)
        Me.Name = "Filechooser"
        Me.Text = "Skin XML Converter 0.9 BETA - by phil65"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents OpenFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ConvertButton As System.Windows.Forms.Button
    Friend WithEvents xmlname As System.Windows.Forms.Label
    Friend WithEvents OutputButton As System.Windows.Forms.Button
    Friend WithEvents OutputFolderDialog As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents OutputLabel As System.Windows.Forms.Label
    Friend WithEvents OutputLog As System.Windows.Forms.TextBox
    Friend WithEvents ConversionDropDown As System.Windows.Forms.ComboBox
    Friend WithEvents HeaderOption As System.Windows.Forms.CheckBox
    Friend WithEvents EncodingDropDown As System.Windows.Forms.ComboBox
    Friend WithEvents DebugOutput As System.Windows.Forms.CheckBox
    Friend WithEvents ConvertBorders As System.Windows.Forms.CheckBox
    Friend WithEvents TextureCheckButton As System.Windows.Forms.Button
    Friend WithEvents SkinFolderButton As System.Windows.Forms.Button
    Friend WithEvents SkinFolderDialog As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents ClearLogButton As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents CheckIncludesButton As System.Windows.Forms.Button
    Friend WithEvents IndentingDropDown As System.Windows.Forms.ComboBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents BuildFolderDialog As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents EOLComboBox As System.Windows.Forms.ComboBox

End Class
