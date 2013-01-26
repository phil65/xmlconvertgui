Imports System.Xml
Imports System.Text
Imports System
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Security
Imports System.Security.Principal.WindowsIdentity
Public Class Filechooser
    Public strOutputFolder As String = ""
    Public TexturePackerPath As String = ""
    Public BuildFolder As String = ""
    Public XMLFolder As String = ""
    Public SkinFolder As String = ""
    Public xmlelements As String() = {"posx", "posy", "width", "height", "textoffsetx", "textoffsety", "radiowidth", "radioheight", "radioposx", "radioposy", "textwidth", "size", "itemgap", "spinwidth", "spinheight"}
    Public xmlelementsBorder As String() = {"border", "bordersize"}
    Public xmlelementsTexture As String() = {"texture", "texturefocus", "texturenofocus", "texturebg", "bordertexture", "value", "icon", "thumb", "alttexturefocus", "alttexturenofocus", "texturesliderbackground", "texturesliderbar", "texturesliderbarfocus", "textureslidernib", "textureslidernibfocus", "midtexture", "righttexture", "lefttexture"}
    Public xmlelementsBrackets As String() = {"visible", "enable", "usealttexture", "selected"}
    Public xmlattributes As String(,)
    Public doc As New XmlDocument()
    Public multiplyFactor As Double = 1.5
    Public ShortenedTexturePaths As New ArrayList()
    Public Filepaths As New ArrayList()
    Public SafeFilepaths As New ArrayList()
    Public elementlist As XmlNodeList
    Public XMLCounter As String
    Public RoundFactor As Integer
    Private Sub Filechooser_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ConversionDropDown.Items.Add("720p --> 1080p")
        ConversionDropDown.Items.Add("1080p --> 720p")
        ConversionDropDown.Items.Add("No Change")
        IndentingDropDown.Items.Add("Indenting: 2")
        IndentingDropDown.Items.Add("Indenting: 4")
        IndentingDropDown.Items.Add("Indenting: Tab")
        EOLComboBox.Items.Add("No Change")
        EOLComboBox.Items.Add("Windows Line Endings")
        EOLComboBox.Items.Add("Linux Line Endings")
        ConversionDropDown.SelectedIndex = 2
        IndentingDropDown.SelectedIndex = 1
        HeaderOption.Checked = Not HeaderOption.Checked
        EOLComboBox.SelectedIndex = 0
        RoundFactor = 1
        TexturePackerPath = My.Settings.TexturePackerPath
        XMLFolder = My.Settings.XMLFolder
        SkinFolder = My.Settings.SkinFolder
        IndentingDropDown.SelectedIndex = My.Settings.Indenting
        EOLComboBox.SelectedIndex = My.Settings.EndOfLine
        ConversionDropDown.SelectedIndex = My.Settings.ConversionType
        HeaderOption.Checked = My.Settings.XMLHeader
        ConvertBorders.Checked = My.Settings.ConvertBorders
        OutputLog.AppendText("Program started" & vbCrLf)
        OutputLog.AppendText("TexturePacker Path:" & TexturePackerPath & vbCrLf)
        OutputLog.AppendText("XML Folder Path:" & XMLFolder & vbCrLf)
        OutputLog.AppendText("Skin Path:" & SkinFolder & vbCrLf)
    End Sub

    Public Sub ConvertButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConvertButton.Click
        OutputLog.AppendText("Amount Of Files Chosen: " + Filepaths.Count.ToString & vbCrLf)
        xmlname.Text = Filepaths.Count.ToString + " Files chosen"
        Select Case ConversionDropDown.SelectedIndex
            Case 0
                multiplyFactor = 1.5
                OutputLog.AppendText("converting 720 ==> 1080: Factor 1.5" & vbCrLf)
            Case 1
                multiplyFactor = 0.66666666666666663
                OutputLog.AppendText("converting 1080 ==> 720: Factor 0.6666666" & vbCrLf)
            Case 2
                multiplyFactor = 1
                OutputLog.AppendText("only converting Encoding / adding Headers" & vbCrLf)
        End Select
        XMLCounter = 0
        Dim errorcounter = 0
        For j = 0 To Filepaths.Count - 1
            Try
                doc.Load(Filepaths(j))
                If multiplyFactor <> 1 Then
                    For i = 0 To xmlelements.Length - 1
                        changeElements(xmlelements(i))
                    Next i
                    If ConvertBorders.Checked Then
                        For i = 0 To xmlelementsBorder.Length - 1
                            changeElements(xmlelementsBorder(i))
                        Next i
                    End If
                    changeAttributes()
                End If
                Dim Number As Double
                If (AnimationMultiplier.Text <> 1) And (Double.TryParse(AnimationMultiplier.Text, Number)) Then
                    elementlist = doc.SelectNodes("//animation | //effect")
                    For Each element In elementlist
                        ScaleXMLNode(element, "time", AnimationMultiplier.Text)
                        ScaleXMLNode(element, "delay", AnimationMultiplier.Text)
                    Next
                End If
                Dim myXmlSettings As New XmlWriterSettings
                If Not HeaderOption.Checked Then
                    myXmlSettings.OmitXmlDeclaration = True
                End If
                Dim UTF8NoBom As Encoding = New UTF8Encoding(False)
                myXmlSettings.Encoding = UTF8NoBom
                Select Case EOLComboBox.SelectedIndex
                    Case 0
                        myXmlSettings.NewLineHandling = NewLineHandling.None
                    Case 1
                        myXmlSettings.NewLineHandling = NewLineHandling.Replace
                        myXmlSettings.NewLineChars = ControlChars.CrLf
                        OutputLog.AppendText("Windows Line Endings selected" & vbCrLf)
                    Case 2
                        myXmlSettings.NewLineHandling = NewLineHandling.Replace
                        myXmlSettings.NewLineChars = ControlChars.NewLine
                        OutputLog.AppendText("Linux Line Endings selected" & vbCrLf)
                End Select
                myXmlSettings.Indent = True
                Select Case IndentingDropDown.SelectedIndex
                    Case 0
                        myXmlSettings.IndentChars = "  "
                        OutputLog.AppendText("Indenting: 2" & vbCrLf)
                    Case 1
                        myXmlSettings.IndentChars = "    "
                        OutputLog.AppendText("Indenting: 4" & vbCrLf)
                    Case 2
                        myXmlSettings.IndentChars = (ControlChars.Tab)
                        OutputLog.AppendText("Indenting: Tab" & vbCrLf)
                End Select
                Dim wrtr As XmlWriter = XmlWriter.Create(strOutputFolder + "\" + SafeFilepaths(j).ToString, myXmlSettings)
                doc.WriteTo(wrtr)
                wrtr.Close()
                OutputLog.AppendText(SafeFilepaths(j) + " created successfully" & vbCrLf)
                XMLCounter = XMLCounter + 1
            Catch xmlex As XmlException                  ' Handle the Xml Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + xmlex.Message & vbCrLf)
                errorcounter = errorcounter + 1
            Catch ex As Exception                        ' Handle the generic Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + ex.Message & vbCrLf)
                errorcounter = errorcounter + 1
            End Try
        Next j
        OutputLog.AppendText("All Files converted" & vbCrLf)
        MsgBox(XMLCounter + " XML Files converted." & vbCrLf & "Errors: " + errorcounter.ToString)
        errorcounter = 0
        ConvertButton.Enabled = False

    End Sub

    Private Sub OutputButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OutputButton.Click
        Do
            OutputFolderDialog.Description = "Choose Output Folder"

            Dim DidWork As Integer = OutputFolderDialog.ShowDialog()
            If DidWork = DialogResult.Cancel Then

            Else
                strOutputFolder = OutputFolderDialog.SelectedPath
                If Filepaths(0) <> "" Then
                    ConvertButton.Enabled = True
                End If
                OutputLabel.Text = strOutputFolder + "\"
                OutputLog.AppendText("Output Folder chosen:" & vbCrLf & strOutputFolder & vbCrLf)
            End If
            If (strOutputFolder + "\" + SafeFilepaths(0) = Filepaths(0)) Then
                MsgBox("You´ve chosen the soure directory. please change the output path.")
            End If
        Loop While (strOutputFolder + "\" + SafeFilepaths(0) = Filepaths(0))


    End Sub

    Sub changeElements(ByVal tag As String)
        elementlist = doc.GetElementsByTagName(tag)
        For Each element In elementlist
            element.InnerXml = ConvertValue(element.InnerXml)
        Next element
    End Sub

    Sub ScaleXMLNode(ByRef Element As XmlNode, ByVal tag As String, ByVal ScaleFactor As String)
        Dim Number As Double
        If (Not Element.Attributes(tag) Is Nothing) Then
            If (Double.TryParse(Element.Attributes(tag).InnerText, Number)) Then
                Element.Attributes(tag).InnerText = Math.Round(XmlConvert.ToDouble(Number) / RoundFactor * XmlConvert.ToDouble(ScaleFactor)) * RoundFactor
            End If
        End If
    End Sub

    Function ConvertValue(ByVal InputString As String) As String
        Dim number As Integer
        Dim TempLetter As String
        ConvertValue = InputString
        If Double.TryParse(InputString, number) And (InputString <> "1") Then
            InputString = XmlConvert.ToDouble(InputString)
            ConvertValue = Math.Round(InputString * multiplyFactor)
        Else
            If InputString.Length > 1 Then
                TempLetter = InputString.ToString.Substring(InputString.Length - 1, 1)
                InputString = InputString.ToString.Substring(0, InputString.Length - 1)
                If Int32.TryParse(InputString, number) Then
                    ConvertValue = Math.Round(number * multiplyFactor).ToString + TempLetter
                    OutputLog.AppendText(InputString & vbCrLf)
                Else
                    If InputString.Length > 1 Then
                        TempLetter = InputString.ToString.Substring(InputString.Length - 1, 1)
                        InputString = InputString.ToString.Substring(0, InputString.Length - 1)
                        If Int32.TryParse(InputString, number) Then
                            ConvertValue = Math.Round(number * multiplyFactor).ToString
                        Else
                        End If
                    Else
                    End If
                End If
            End If
        End If
    End Function

    Sub convertString(ByRef CompleteString As String, Optional ByVal ConvertAll As Boolean = True)
        Dim NewString As String = ""
        Dim IndexStart = 0
        If CompleteString.Contains(",") Or (ConvertAll = True) Then
            For i = 0 To CompleteString.Length - 1
                If CompleteString(i) = "," Then
                    NewString = NewString + ConvertValue(CompleteString.Substring(IndexStart, i - IndexStart)) + ","
                    IndexStart = i + 1
                End If
            Next
            CompleteString = NewString + ConvertValue((CompleteString.Substring(IndexStart, CompleteString.Length - IndexStart)))
        End If
    End Sub

    Sub changeAttributes()
        elementlist = doc.SelectNodes("//animation[@effect='zoom'] | //effect[@type='zoom']")
        For Each element In elementlist
            If Not element.Attributes("start") Is Nothing Then
                convertString(element.Attributes("start").InnerText, False)
            End If
            If Not element.Attributes("end") Is Nothing Then
                convertString(element.Attributes("end").InnerText, False)
            End If
        Next
        elementlist = doc.SelectNodes("//animation[@effect='slide'] | //effect[@type='slide']")
        For Each element In elementlist
            If Not element.Attributes("start") Is Nothing Then
                convertString(element.Attributes("start").InnerText)
            End If
            If Not element.Attributes("end") Is Nothing Then
                convertString(element.Attributes("end").InnerText)
            End If
        Next
        elementlist = doc.SelectNodes("//animation[@effect='rotatex'] | //animation[@effect='rotatey'] | //animation[@effect='rotate'] | //effect[@type='rotate'] | //effect[@type='rotatex'] | //effect[@type='rotatey'] | //animation[@effect='slide'] | //effect[@type='slide']")
        For Each element In elementlist
            If Not element.Attributes("center") Is Nothing Then
                convertString(element.Attributes("center").InnerText)
            End If
        Next
        If ConvertBorders.Checked Then
            elementlist = doc.SelectNodes("//texturefocus | //texture | //texturenofocus | //bordertexture | //texturesliderbackground | //texturesliderbar | //texturesliderbarfocus | //alttexturenofocus | //alttexturefocus | //midtexture")
            For Each element In elementlist
                If Not element.Attributes("border") Is Nothing Then
                    convertString(element.Attributes("border").InnerText)
                End If
            Next
        End If
        elementlist = doc.SelectNodes("//focusedlayout | //itemlayout | //channellayout | //focusedchannellayout | //rulerlayout")
        For Each element In elementlist
            If Not element.Attributes("width") Is Nothing Then
                convertString(element.Attributes("width").InnerText)
            End If
            If Not element.Attributes("height") Is Nothing Then
                convertString(element.Attributes("height").InnerText)
            End If
        Next
        elementlist = doc.SelectNodes("//hitrect | //camera")
        For Each element In elementlist
            If Not element.Attributes("x") Is Nothing Then
                convertString(element.Attributes("x").InnerText)
            End If
            If Not element.Attributes("y") Is Nothing Then
                convertString(element.Attributes("y").InnerText)
            End If
            If Not element.Attributes("h") Is Nothing Then
                convertString(element.Attributes("h").InnerText)
            End If
            If Not element.Attributes("w") Is Nothing Then
                convertString(element.Attributes("w").InnerText)
            End If
        Next
        elementlist = doc.SelectNodes("//width | //height")
        For Each element In elementlist
            If Not element.Attributes("min") Is Nothing Then
                convertString(element.Attributes("min").InnerText)
            End If
            If Not element.Attributes("max") Is Nothing Then
                convertString(element.Attributes("max").InnerText)
            End If
        Next
    End Sub

    Sub TextureFinder(ByVal dir As String)
        Dim ShortPath As String = ""
        Try
            For Each fname As String In Directory.GetFiles(dir)
                Dim number As Integer = 0
                ShortPath = fname.Substring(SkinFolder.Length + 7, fname.Length - (SkinFolder.Length + 7))
                If ((Not ShortPath.Contains("flags\")) And (Not ShortPath.Contains("cerberus")) And (Not ShortPath.ToLower.Contains("default")) And
                    (Not ShortPath.ToLower.Contains("stars\")) And (Not ShortPath.ToLower.Contains("rating1.png")) And (Not ShortPath.ToLower.Contains("rating2.png")) And
                    (Not ShortPath.ToLower.Contains("rating3.png")) And (Not ShortPath.ToLower.Contains("rating4.png")) And (Not ShortPath.ToLower.Contains("rating5.png")) And
                    (Not ShortPath.ToLower.Contains("\480p.png")) And (Not ShortPath.ToLower.Contains("\540p.png")) And (Not ShortPath.ToLower.Contains("\720p.png")) And
                    (Not ShortPath.ToLower.Contains("\576p.png")) And (Not ShortPath.ToLower.Contains("\1080p.png")) And (Not ShortPath.ToLower.Contains("overlaywatched.png"))) Then
                    ShortPath = ShortPath.Replace("\", "/")
                    '   ShortPath = ShortPath.ToLower
                    ShortenedTexturePaths.Add(ShortPath)
                End If
            Next
            For Each subdir As String In Directory.GetDirectories(dir)
                TextureFinder(subdir)
            Next
        Catch xmlex As XmlException                  ' Handle the Xml Exceptions here.
            OutputLog.AppendText(xmlex.Message)
        Catch ex As Exception                        ' Handle the generic Exceptions here.
            OutputLog.AppendText(ex.Message)
        End Try
    End Sub

    Private Sub TextureCheckButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextureCheckButton.Click
        OutputLog.AppendText("Building Texture List" & vbCrLf)
        TextureFinder(SkinFolder + "\media")
        OutputLog.AppendText("Scanning XMLs. This may take a while..." & vbCrLf & "Please check the textures of the upcoming list for usage." & vbCrLf)
        For j = 0 To Filepaths.Count - 1
            Try
                doc.Load(Filepaths(j))
                RemoveTexturesFromArray()
            Catch xmlex As XmlException                  ' Handle the Xml Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + xmlex.Message & vbCrLf)
            Catch ex As Exception                        ' Handle the generic Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + ex.Message & vbCrLf)
            End Try
        Next j
        OutputLog.AppendText("Unused Textures:" & vbCrLf)
        Dim str As String
        For Each str In ShortenedTexturePaths
            OutputLog.AppendText(str & vbCrLf)
        Next
    End Sub

    Sub RemoveTexturesFromArray()
        Try
            elementlist = doc.SelectNodes("//texture")
            For Each element In elementlist
                If Not element.Attributes("diffuse") Is Nothing Then
                    If ShortenedTexturePaths.Contains(element.Attributes("diffuse").InnerText.ToString.ToLower) Then
                        ShortenedTexturePaths.Remove(element.Attributes("diffuse").InnerText.ToString.ToLower)
                    End If
                End If
                If Not element.Attributes("fallback") Is Nothing Then
                    If ShortenedTexturePaths.Contains(element.Attributes("fallback").InnerText.ToString.ToLower) Then
                        ShortenedTexturePaths.Remove(element.Attributes("fallback").InnerText.ToString.ToLower)
                    End If
                End If
            Next
            For j = 0 To xmlelementsTexture.Length
                elementlist = doc.GetElementsByTagName(xmlelementsTexture(j))
                For Each element In elementlist
                    If ShortenedTexturePaths.Contains(element.InnerXml.ToLower) Then
                        ShortenedTexturePaths.Remove(element.InnerXml.ToLower)
                    End If
                Next element
            Next j
        Catch
        End Try
    End Sub

    Private Function CheckPath(ByVal strPath As String) As Boolean
        If Dir$(strPath) <> "" Then
            CheckPath = True
        Else
            CheckPath = False
        End If
    End Function

    Private Sub CheckNodeValue(ByVal XMLTag As String, ByVal ValidValues As String(), Optional ByRef FileName As String = "")
        elementlist = doc.GetElementsByTagName(XMLTag)
        For Each element In elementlist
            If Not ValidValues.Contains(element.InnerXml.ToString.ToLower) Then
                OutputLog.AppendText(FileName + ": Invalid Value for " & XMLTag & ": " & element.InnerXml & vbCrLf)
            End If
        Next
    End Sub

    Private Sub SkinFolderButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SkinFolderButton.Click
        SafeFilepaths.Clear()
        Filepaths.Clear()
        xmlname.Text = ""
        SkinFolderDialog.Description = "Choose Skin Folder"
        Dim DidWork As Integer = SkinFolderDialog.ShowDialog()
        If DidWork = DialogResult.Cancel Then
        Else
            SkinFolder = SkinFolderDialog.SelectedPath
            OutputLog.AppendText("Skin Folder chosen:" & vbCrLf & SkinFolder & vbCrLf)
        End If
        If Not CheckPath(SkinFolder + "\addon.xml") Then
            MsgBox("Please choose a skin folder.")
        Else
            Try
                If SkinFolder <> "" Then
                    doc.Load(SkinFolder + "\addon.xml")
                    elementlist = doc.SelectNodes("//res")
                    If Not elementlist(0).Attributes("folder") Is Nothing Then
                        XMLFolder = SkinFolder + "\" + elementlist(0).Attributes("folder").InnerText
                        OutputLog.AppendText("XML Folder:" & XMLFolder & vbCrLf)
                        Const ATTR_DIRECTORY = 16
                        If Dir$(XMLFolder, ATTR_DIRECTORY) <> "" Then
                            Dim DirInfo As New DirectoryInfo(XMLFolder)
                            Dim FileObj As IO.FileSystemInfo
                            For Each FileObj In DirInfo.GetFileSystemInfos
                                Filepaths.Add(FileObj.FullName)
                                SafeFilepaths.Add(FileObj.Name)
                            Next
                            OutputButton.Visible = True
                            OutputLabel.Visible = True
                            If strOutputFolder <> "" Then
                                ConvertButton.Enabled = True
                            End If
                        Else
                            MsgBox("Path from addon.xml does not exist.")
                        End If
                    End If
                End If
            Catch xmlex As XmlException                  ' Handle the Xml Exceptions here.
                OutputLog.AppendText(xmlex.Message & vbCrLf)
            Catch ex As Exception                        ' Handle the generic Exceptions here.
                OutputLog.AppendText(ex.Message & vbCrLf)
            End Try
        End If
    End Sub

    Private Sub ClearLogButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearLogButton.Click
        OutputLog.Clear()
    End Sub

    Private Sub CheckFontsButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckFontsButton.Click
        OutputLog.AppendText("Building Font List" & vbCrLf)
        Dim ShortPath As String = ""
        Dim FontList As New ArrayList()
        Dim FontList2 As New ArrayList()
        Try
            doc.Load(XMLFolder + "\font.xml")
            elementlist = doc.GetElementsByTagName("name")
            For Each element In elementlist
                If Not FontList.Contains(element.InnerXml) Then
                    FontList.Add(element.InnerXml)
                End If
            Next element
            elementlist = doc.GetElementsByTagName("filename")
            For Each element In elementlist
                If Not FontList2.Contains(element.InnerXml) Then
                    FontList2.Add(element.InnerXml)
                    OutputLog.AppendText("Added" + element.InnerXml & vbCrLf)
                End If
            Next element
        Catch xmlex As XmlException                  ' Handle the Xml Exceptions here.
            OutputLog.AppendText(xmlex.Message)
        Catch ex As Exception                        ' Handle the generic Exceptions here.
            OutputLog.AppendText(ex.Message)
        End Try
        OutputLog.AppendText("Scanning XMLs. This may take a while..." & vbCrLf & "Please check the fonts of the upcoming list for usage." & vbCrLf)
        For j = 0 To Filepaths.Count - 1
            Try
                doc.Load(Filepaths(j))
                If Not Filepaths(j).ToString.ToLower.Contains("font.xml") Then
                    elementlist = doc.GetElementsByTagName("font")
                    For Each element In elementlist
                        If FontList.Contains(element.InnerXml) Then
                            FontList.Remove(element.InnerXml)
                        End If
                    Next element
                End If
            Catch xmlex As XmlException                  ' Handle the Xml Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + xmlex.Message & vbCrLf)
            Catch ex As Exception                        ' Handle the generic Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + ex.Message & vbCrLf)
            End Try
        Next j
        OutputLog.AppendText("Unused Fonts:" & vbCrLf)
        Dim str As String
        For Each str In FontList
            OutputLog.AppendText(str & vbCrLf)
        Next
    End Sub

    Private Sub CheckIncludesButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckIncludesButton.Click
        OutputLog.AppendText("Building Include List" & vbCrLf)
        Dim IncludeList As New ArrayList()
        Dim IncludeListBackup As New ArrayList()
        Dim IncludeList2 As New ArrayList()
        Dim ShortPath As String = ""
        IncludeList2.Clear()
        IncludeListBackup.Clear()
        IncludeList.Clear()
        For j = 0 To Filepaths.Count - 1
            Try
                doc.Load(Filepaths(j))
                elementlist = doc.SelectNodes("//include[(@name)]")
                For Each element In elementlist
                    If Not IncludeList.Contains(element.Attributes("name").InnerText) Then
                        IncludeList.Add(element.Attributes("name").InnerText)
                    End If
                Next
                elementlist = doc.SelectNodes("//include[not(@name)]")
                For Each element In elementlist
                    If Not IncludeList2.Contains(element.InnerXml) Then
                        IncludeList2.Add(element.InnerXml)
                        IncludeListBackup.Add(element.InnerXml)
                    End If
                Next
            Catch xmlex As XmlException                  ' Handle the Xml Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + xmlex.Message & vbCrLf)
            Catch ex As Exception                        ' Handle the generic Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + ex.Message & vbCrLf)
            End Try
        Next
        Dim Include As String
        For Each Include In IncludeList
            If IncludeList2.Contains(Include) Then
                IncludeList2.Remove(Include)
            End If
        Next
        For Each Include In IncludeListBackup
            If IncludeList.Contains(Include) Then
                IncludeList.Remove(Include)
            End If
        Next
        OutputLog.AppendText("Scanning XMLs. This may take a while..." & vbCrLf & "Please check the upcoming List of Includes." & vbCrLf)
        OutputLog.AppendText("Unused Includes:" & vbCrLf)
        For Each Include In IncludeList
            OutputLog.AppendText(Include & vbCrLf)
        Next
        OutputLog.AppendText("Undefined Includes:" & vbCrLf)
        For Each Include In IncludeList2
            OutputLog.AppendText(Include & vbCrLf)
        Next
    End Sub

    Private Sub StartSkinBuildButton(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Process.Start("build.bat", SkinFolder + " " + TexturePackerPath + " " + BuildFolder)
    End Sub

    Private Sub ChooseTexturePackerButton(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        OpenFileDialog.Title = "Choose EXE file"
        OpenFileDialog.Filter = "EXE Files|*.exe"
        Dim DidWork As Integer = OpenFileDialog.ShowDialog()
        If DidWork = DialogResult.Cancel Then
        Else
            OutputButton.Visible = True
            OutputLabel.Visible = True
            TexturePackerPath = OpenFileDialog.FileName
            OutputLog.AppendText("TexturePacker Path: " + TexturePackerPath & vbCrLf)
        End If
    End Sub

    Private Sub BuildFolderButton(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        BuildFolderDialog.Description = "Choose Build Folder"
        Dim DidWork As Integer = BuildFolderDialog.ShowDialog()
        If DidWork = DialogResult.Cancel Then
        Else
            BuildFolder = BuildFolderDialog.SelectedPath
            OutputLog.AppendText("Build Folder chosen:" & vbCrLf)
            OutputLog.AppendText(BuildFolder & vbCrLf)
        End If
    End Sub

    Public Function CountCharacter(ByVal value As String, ByVal ch As Char) As Integer
        Dim cnt As Integer = 0
        For Each c As Char In value
            If c = ch Then cnt += 1
        Next
        Return cnt
    End Function

    Private Sub CheckBracketsButton_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBracketsButton.Click
        OutputLog.AppendText("Checking the brackets..." & vbCrLf)
        For j = 0 To Filepaths.Count - 1
            Try
                doc.Load(Filepaths(j))
                For k = 0 To xmlelementsBrackets.Length - 1
                    elementlist = doc.GetElementsByTagName(xmlelementsBrackets(k))
                    For Each element In elementlist
                        If Not (CountCharacter(element.InnerXml, "[") = CountCharacter(element.InnerXml, "]")) Then
                            OutputLog.AppendText("Unmatched parenthesis: " + element.InnerXml.ToString & vbCrLf)
                        End If
                        If Not (CountCharacter(element.InnerXml, "(") = CountCharacter(element.InnerXml, ")")) Then
                            OutputLog.AppendText("Unmatched parenthesis: " + element.InnerXml.ToString & vbCrLf)
                        End If
                    Next element
                Next k
                elementlist = doc.SelectNodes("//include | //onup | //ondown | //onleft | //onright | //animation | //onload | //onunload | //onclick | //onback | //focusedlayout | //itemlayout | //onfocus | //value")
                For Each element In elementlist
                    If Not element.Attributes("condition") Is Nothing Then
                        If Not (CountCharacter(element.Attributes("condition").InnerText, "[") = CountCharacter(element.Attributes("condition").InnerText, "]")) Then
                            OutputLog.AppendText("Unmatched parenthesis: " + element.Attributes("condition").InnerText & vbCrLf)
                        End If
                        If Not (CountCharacter(element.Attributes("condition").InnerText, "(") = CountCharacter(element.Attributes("condition").InnerText, ")")) Then
                            OutputLog.AppendText("Unmatched parenthesis: " + element.Attributes("condition").InnerText & vbCrLf)
                        End If
                    End If
                Next element
            Catch xmlex As XmlException                  ' Handle the Xml Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + xmlex.Message & vbCrLf)
            Catch ex As Exception                        ' Handle the generic Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + ex.Message & vbCrLf)
            End Try
        Next
    End Sub

    Private Sub CheckIDsButton_Click(sender As System.Object, e As System.EventArgs) Handles CheckIDsButton.Click
        Dim charsToTrim() As Char = {"("c, ")"c}
        Dim pattern As String = "\([0-9]+\)"
        Dim IDList As New ArrayList()
        Dim IDList2 As New ArrayList()
        Dim IDListBackup As New ArrayList()
        IDList.Clear()
        IDList2.Clear()
        IDListBackup.Clear()
        '   Dim pattern as String = "\([a-zA-Z0-9]*\)"
        OutputLog.AppendText("Checking the IDs..." & vbCrLf)
        For j = 0 To Filepaths.Count - 1
            Try
                doc.Load(Filepaths(j))
                For k = 0 To xmlelementsBrackets.Length - 1
                    elementlist = doc.GetElementsByTagName(xmlelementsBrackets(k))
                    For Each element In elementlist
                        If Not element.InnerXml Is Nothing Then
                            Dim r As Regex = New Regex(pattern, RegexOptions.IgnoreCase)
                            Dim m As Match = r.Match(element.InnerXml.ToString)
                            While (m.Success)
                                Dim tempText As String = m.Value.ToString()
                                tempText = Replace(tempText, "(", "")
                                tempText = Replace(tempText, ")", "")
                                If Not IDList.Contains(tempText) Then
                                    IDList.Add(tempText)
                                    IDListBackup.Add(tempText)
                                    '      OutputLog.AppendText(tempText & vbCrLf)
                                End If
                                m = m.NextMatch()
                            End While
                        End If
                    Next element
                Next k
                elementlist = doc.SelectNodes("//include | //onup | //ondown | //onleft | //onright | //animation | //onload | //onunload | //onclick | //onback | //focusedlayout | //itemlayout | //onfocus | //value")
                For Each element In elementlist

                    If Not element.Attributes("condition") Is Nothing Then
                        Dim r As Regex = New Regex(pattern, RegexOptions.IgnoreCase)
                        Dim m As Match = r.Match(element.Attributes("condition").InnerText)
                        While (m.Success)
                            Dim tempText As String = m.Value.ToString()
                            tempText = Replace(tempText, "(", "")
                            tempText = Replace(tempText, ")", "")
                            If Not IDList.Contains(tempText) Then
                                IDList.Add(tempText)
                                ' OutputLog.AppendText(tempText & vbCrLf)
                            End If
                            m = m.NextMatch()
                        End While
                    End If
                Next element
                elementlist = doc.SelectNodes("//control[(@id)] | //window[(@id)]")
                For Each element In elementlist
                    If Not IDList2.Contains(element.Attributes("id").InnerText) Then
                        IDList2.Add(element.Attributes("id").InnerText)
                        IDListBackup.Add(element.Attributes("id").InnerText)
                    End If
                Next
            Catch xmlex As XmlException                  ' Handle the Xml Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + xmlex.Message & vbCrLf)
            Catch ex As Exception                        ' Handle the generic Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + ex.Message & vbCrLf)
            End Try
        Next
        For i = 0 To IDList.Count - 1
            If IDList2.Contains(IDList(i)) Then
                IDList2.Remove(IDList(i))
            End If
        Next
        For i = 0 To IDListBackup.Count - 1
            If IDList.Contains(IDListBackup(i)) Then
                IDList.Remove(IDListBackup(i))
            End If
        Next

        OutputLog.AppendText("Undefined IDs:" & vbCrLf)
        Dim str As String
        For Each str In IDList
            OutputLog.AppendText(str & vbCrLf)
        Next
        '       OutputLog.AppendText("Undefined IDs:" & vbCrLf)
        '      For Each str In IDList2
        ' OutputLog.AppendText(str & vbCrLf)
        '  Next
    End Sub

    Private Sub SaveButton_Click(sender As System.Object, e As System.EventArgs) Handles SaveButton.Click
        My.Settings.TexturePackerPath = TexturePackerPath
        My.Settings.XMLFolder = XMLFolder
        My.Settings.SkinFolder = SkinFolder
        My.Settings.XMLHeader = HeaderOption.Checked
        My.Settings.ConvertBorders = ConvertBorders.Checked
        My.Settings.EndOfLine = EOLComboBox.SelectedIndex
        My.Settings.Indenting = IndentingDropDown.SelectedIndex
        MsgBox("Settings saved")
    End Sub

    Private Sub CheckValuesButton_Click(sender As System.Object, e As System.EventArgs) Handles CheckValuesButton.Click
        OutputLog.AppendText("Scanning XMLs..." & vbCrLf)
        For j = 0 To Filepaths.Count - 1
            Try
                doc.Load(Filepaths(j))
                CheckNodeValue("align", {"left", "center", "right", "justify"}, SafeFilepaths(j))
                CheckNodeValue("aspectratio", {"keep", "scale", "stretch", "center"}, SafeFilepaths(j))
                CheckNodeValue("aligny", {"top", "center", "bottom"}, SafeFilepaths(j))
                CheckNodeValue("orientation", {"horizontal", "vertical"}, SafeFilepaths(j))
                CheckNodeValue("subtype", {"page", "int", "float", "text"}, SafeFilepaths(j))
                CheckNodeValue("action", {"volume", "seek"}, SafeFilepaths(j))
                CheckNodeValue("scroll", {"false", "true", "yes", "no"}, SafeFilepaths(j))
                CheckNodeValue("randomize", {"false", "true", "yes", "no"}, SafeFilepaths(j))
                CheckNodeValue("scrollout", {"false", "true", "yes", "no"}, SafeFilepaths(j))
                CheckNodeValue("pulseonselect", {"false", "true", "yes", "no"}, SafeFilepaths(j))
                CheckNodeValue("reverse", {"false", "true", "yes", "no"}, SafeFilepaths(j))
                CheckNodeValue("usecontrolcoords", {"false", "true", "yes", "no"}, SafeFilepaths(j))
            Catch xmlex As XmlException                  ' Handle the Xml Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + xmlex.Message & vbCrLf)
            Catch ex As Exception                        ' Handle the generic Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + ex.Message & vbCrLf)
            End Try
        Next j
        OutputLog.AppendText("Scan complete" & vbCrLf)
    End Sub
End Class


