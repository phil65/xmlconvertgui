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
        Me.ChooseButton = New System.Windows.Forms.Button()
        Me.ConvertButton = New System.Windows.Forms.Button()
        Me.xmlname = New System.Windows.Forms.Label()
        Me.OutputButton = New System.Windows.Forms.Button()
        Me.FolderBrowserDialog = New System.Windows.Forms.FolderBrowserDialog()
        Me.OutputLabel = New System.Windows.Forms.Label()
        Me.OutputLog = New System.Windows.Forms.TextBox()
        Me.ConversionDropDown = New System.Windows.Forms.ComboBox()
        Me.HeaderOption = New System.Windows.Forms.CheckBox()
        Me.EncodingDropDown = New System.Windows.Forms.ComboBox()
        Me.DebugOutput = New System.Windows.Forms.CheckBox()
        Me.ConvertBorders = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'OpenFileDialog
        '
        Me.OpenFileDialog.FileName = "OpenFileDialog1"
        Me.OpenFileDialog.Multiselect = True
        '
        'ChooseButton
        '
        Me.ChooseButton.Location = New System.Drawing.Point(49, 22)
        Me.ChooseButton.Name = "ChooseButton"
        Me.ChooseButton.Size = New System.Drawing.Size(75, 23)
        Me.ChooseButton.TabIndex = 1
        Me.ChooseButton.Text = "Choose"
        Me.ChooseButton.UseVisualStyleBackColor = True
        '
        'ConvertButton
        '
        Me.ConvertButton.Enabled = False
        Me.ConvertButton.Location = New System.Drawing.Point(202, 214)
        Me.ConvertButton.Name = "ConvertButton"
        Me.ConvertButton.Size = New System.Drawing.Size(75, 23)
        Me.ConvertButton.TabIndex = 3
        Me.ConvertButton.Text = "Convert"
        Me.ConvertButton.UseVisualStyleBackColor = True
        '
        'xmlname
        '
        Me.xmlname.AutoSize = True
        Me.xmlname.Location = New System.Drawing.Point(163, 27)
        Me.xmlname.Name = "xmlname"
        Me.xmlname.Size = New System.Drawing.Size(98, 13)
        Me.xmlname.TabIndex = 4
        Me.xmlname.Text = "Choose XML File(s)"
        '
        'OutputButton
        '
        Me.OutputButton.Location = New System.Drawing.Point(49, 84)
        Me.OutputButton.Name = "OutputButton"
        Me.OutputButton.Size = New System.Drawing.Size(75, 24)
        Me.OutputButton.TabIndex = 5
        Me.OutputButton.Text = "Choose"
        Me.OutputButton.UseVisualStyleBackColor = True
        Me.OutputButton.Visible = False
        '
        'OutputLabel
        '
        Me.OutputLabel.Location = New System.Drawing.Point(163, 90)
        Me.OutputLabel.Name = "OutputLabel"
        Me.OutputLabel.Size = New System.Drawing.Size(203, 61)
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
        Me.OutputLog.Size = New System.Drawing.Size(362, 284)
        Me.OutputLog.TabIndex = 7
        '
        'ConversionDropDown
        '
        Me.ConversionDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ConversionDropDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ConversionDropDown.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.ConversionDropDown.Location = New System.Drawing.Point(12, 130)
        Me.ConversionDropDown.Name = "ConversionDropDown"
        Me.ConversionDropDown.Size = New System.Drawing.Size(121, 21)
        Me.ConversionDropDown.TabIndex = 8
        '
        'HeaderOption
        '
        Me.HeaderOption.AutoSize = True
        Me.HeaderOption.Location = New System.Drawing.Point(12, 203)
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
        Me.EncodingDropDown.Location = New System.Drawing.Point(12, 167)
        Me.EncodingDropDown.Name = "EncodingDropDown"
        Me.EncodingDropDown.Size = New System.Drawing.Size(121, 21)
        Me.EncodingDropDown.TabIndex = 10
        '
        'DebugOutput
        '
        Me.DebugOutput.AutoSize = True
        Me.DebugOutput.Location = New System.Drawing.Point(12, 226)
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
        Me.ConvertBorders.Location = New System.Drawing.Point(12, 249)
        Me.ConvertBorders.Name = "ConvertBorders"
        Me.ConvertBorders.Size = New System.Drawing.Size(102, 17)
        Me.ConvertBorders.TabIndex = 12
        Me.ConvertBorders.Text = "Convert Borders"
        Me.ConvertBorders.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ConvertBorders.UseVisualStyleBackColor = True
        '
        'Filechooser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(725, 318)
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
        Me.Controls.Add(Me.ChooseButton)
        Me.Name = "Filechooser"
        Me.Text = "Skin XML Converter - by phil65"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents OpenFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ChooseButton As System.Windows.Forms.Button
    Friend WithEvents ConvertButton As System.Windows.Forms.Button
    Friend WithEvents xmlname As System.Windows.Forms.Label
    Friend WithEvents OutputButton As System.Windows.Forms.Button
    Friend WithEvents FolderBrowserDialog As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents OutputLabel As System.Windows.Forms.Label
    Friend WithEvents OutputLog As System.Windows.Forms.TextBox
    Friend WithEvents ConversionDropDown As System.Windows.Forms.ComboBox
    Friend WithEvents HeaderOption As System.Windows.Forms.CheckBox
    Friend WithEvents EncodingDropDown As System.Windows.Forms.ComboBox
    Friend WithEvents DebugOutput As System.Windows.Forms.CheckBox
    Friend WithEvents ConvertBorders As System.Windows.Forms.CheckBox

End Class
