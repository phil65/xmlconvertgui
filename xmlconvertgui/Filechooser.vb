Imports System.Xml
Imports System.Text
Imports System
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Xml.Schema
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
    Public actualFile As String
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
        strOutputFolder = My.Settings.OutPutFolder
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
        Dim myXmlSettings As New XmlWriterSettings
        If Not HeaderOption.Checked Then myXmlSettings.OmitXmlDeclaration = True
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
        For j = 0 To Filepaths.Count - 1
            Try
                doc.Load(Filepaths(j))
                actualFile = SafeFilepaths(j)
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
                    For i = 0 To elementlist.Count - 1
                        ScaleXMLNode(elementlist(i), "time", AnimationMultiplier.Text)
                        ScaleXMLNode(elementlist(i), "delay", AnimationMultiplier.Text)
                    Next
                End If
                CheckValues()
                Dim wrtr As XmlWriter = XmlWriter.Create(strOutputFolder + "\" + SafeFilepaths(j).ToString, myXmlSettings)
                doc.WriteTo(wrtr)
                wrtr.WriteEndDocument()
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
        OutputFolderDialog.Description = "Choose Output Folder"
        Dim DidWork As Integer = OutputFolderDialog.ShowDialog()
        If DidWork = DialogResult.Cancel Then
        Else
            strOutputFolder = OutputFolderDialog.SelectedPath
            If Filepaths(0) <> "" Then ConvertButton.Enabled = True
            OutputLabel.Text = strOutputFolder + "\"
            OutputLog.AppendText("Output Folder chosen:" & vbCrLf & strOutputFolder & vbCrLf)
        End If
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
                    If elementlist.Count > 0 Then
                        If Not elementlist(0).Attributes("folder") Is Nothing Then
                            XMLFolder = SkinFolder + "\" + elementlist(0).Attributes("folder").InnerText.ToString
                            OutputLog.AppendText("XML Folder:" & XMLFolder & vbCrLf)
                            Const ATTR_DIRECTORY = 16
                            If Dir$(XMLFolder, ATTR_DIRECTORY) <> "" Then
                                Dim DirInfo As New DirectoryInfo(XMLFolder)
                                Dim FileObj As IO.FileSystemInfo
                                For Each FileObj In DirInfo.GetFileSystemInfos
                                    If FileObj.Name.Contains(".xml") Then

                                        Filepaths.Add(FileObj.FullName)
                                        SafeFilepaths.Add(FileObj.Name)
                                    End If
                                Next
                                OutputButton.Visible = True
                                OutputLabel.Visible = True
                                If strOutputFolder <> "" Then ConvertButton.Enabled = True
                            Else
                                MsgBox("Path from addon.xml does not exist.")
                            End If
                        Else
                            MsgBox("Folder Tag in Addon.xml empty")
                        End If
                    Else
                        MsgBox("no res tag in addon.xml")
                    End If
                Else
                    MsgBox("no skin folder chosen")
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
            AddNodesToArray(FontList, "name")
            AddNodesToArray(FontList2, "filename")
        Catch xmlex As XmlException                  ' Handle the Xml Exceptions here.
            OutputLog.AppendText(xmlex.Message)
        Catch ex As Exception                        ' Handle the generic Exceptions here.
            OutputLog.AppendText(ex.Message)
        End Try
        OutputLog.AppendText("Scanning XMLs. This may take a while..." & vbCrLf & "Please check the fonts of the upcoming list for usage." & vbCrLf)
        For j = 0 To Filepaths.Count - 1
            Try
                doc.Load(Filepaths(j))
                actualFile = SafeFilepaths(j)
                If Not Filepaths(j).ToString.ToLower.Contains("font.xml") Then RemoveNodesFromArray(FontList, "font")
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
                actualFile = SafeFilepaths(j)
                AddAttributesToArray(IncludeList, "//include[(@name)]", {"name"})
                elementlist = doc.SelectNodes("//include[not(@name)]")
                For i = 0 To elementlist.Count - 1
                    AddStringToArray(IncludeList2, elementlist(i).InnerXml)
                    AddStringToArray(IncludeListBackup, elementlist(i).InnerXml)
                Next
            Catch xmlex As XmlException                  ' Handle the Xml Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + xmlex.Message & vbCrLf)
            Catch ex As Exception                        ' Handle the generic Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + ex.Message & vbCrLf)
            End Try
        Next
        Dim Include As String
        For Each Include In IncludeList
            RemoveStringFromArray(IncludeList2, Include)
        Next
        For Each Include In IncludeListBackup
            RemoveStringFromArray(IncludeList, Include)
        Next
        OutputLog.AppendText("Scanning XMLs. This may take a while..." & vbCrLf & "Please check the upcoming List of Includes." & vbCrLf)
        OutputLog.AppendText("Unused Includes:" & vbCrLf)
        PrintArray(IncludeList)
        OutputLog.AppendText("Undefined Includes:" & vbCrLf)
        PrintArray(IncludeList2)
    End Sub

    Private Sub StartSkinBuildButton(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Process.Start("build.bat", SkinFolder)
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

    Private Sub CheckBracketsButton_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBracketsButton.Click
        OutputLog.AppendText("Checking the brackets..." & vbCrLf)
        For j = 0 To Filepaths.Count - 1
            Try
                doc.Load(Filepaths(j))
                actualFile = SafeFilepaths(j)
                For k = 0 To xmlelementsBrackets.Length - 1
                    elementlist = doc.GetElementsByTagName(xmlelementsBrackets(k))
                    For i = 0 To elementlist.Count - 1
                        SameCharNumber(elementlist(i).InnerXml, "[", "]")
                        SameCharNumber(elementlist(i).InnerXml, "(", ")")
                    Next i
                Next k
                elementlist = doc.SelectNodes("//include | //onup | //ondown | //onleft | //onright | //animation | //onload | //onunload | //onclick | //onback | //focusedlayout | //itemlayout | //onfocus | //value")
                For i = 0 To elementlist.Count - 1
                    If Not elementlist(i).Attributes("condition") Is Nothing Then
                        Dim CompareString As String = elementlist(i).Attributes("condition").InnerText.ToString
                        SameCharNumber(CompareString, "[", "]")
                        SameCharNumber(CompareString, "(", ")")
                    End If
                Next i
            Catch xmlex As XmlException                  ' Handle the Xml Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + xmlex.Message & vbCrLf)
            Catch ex As Exception                        ' Handle the generic Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + ex.Message & vbCrLf)
            End Try
        Next
    End Sub

    Private Sub TextureCheckButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextureCheckButton.Click
        OutputLog.AppendText("Building Texture List" & vbCrLf)
        TextureFinder(SkinFolder + "\media")
        OutputLog.AppendText("Scanning XMLs. This may take a while..." & vbCrLf & "Please check the textures of the upcoming list for usage." & vbCrLf)
        For j = 0 To Filepaths.Count - 1
            Try
                doc.Load(Filepaths(j))
                actualFile = SafeFilepaths(j)
                RemoveAttributesFromArray(ShortenedTexturePaths, "//texture", {"diffuse", "fallback"}, True)
                For k = 0 To xmlelementsTexture.Length - 1
                    RemoveNodesFromArray(ShortenedTexturePaths, xmlelementsTexture(k), True)
                Next k
            Catch xmlex As XmlException                  ' Handle the Xml Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + xmlex.Message & vbCrLf)
            Catch ex As Exception                        ' Handle the generic Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + ex.Message & vbCrLf)
            End Try
        Next j
        OutputLog.AppendText("Unused Textures:" & vbCrLf)
        PrintArray(ShortenedTexturePaths)
    End Sub

    Sub changeElements(ByVal tag As String)
        elementlist = doc.GetElementsByTagName(tag)
        For i = 0 To elementlist.Count - 1
            elementlist(i).InnerXml = ConvertValue(elementlist(i).InnerXml, multiplyFactor)
        Next i
    End Sub

    Private Sub CheckIDsButton_Click(sender As System.Object, e As System.EventArgs) Handles CheckIDsButton.Click
        Dim IDListRefs As New ArrayList()
        Dim IDListDefines As New ArrayList()
        Dim IDListDefinesBackup As New ArrayList()
        Dim r As Regex = New Regex("(?<=\()[0-9]+", RegexOptions.IgnoreCase)
        IDListRefs.Clear()
        IDListDefines.Clear()
        IDListDefinesBackup.Clear()
        OutputLog.AppendText("Checking the IDs..." & vbCrLf)
        For j = 0 To Filepaths.Count - 1
            Try
                doc.Load(Filepaths(j))
                actualFile = SafeFilepaths(j)
                For k = 0 To xmlelementsBrackets.Length - 1
                    elementlist = doc.GetElementsByTagName(xmlelementsBrackets(k))
                    For i = 0 To elementlist.Count - 1
                        If Not elementlist(i).InnerXml Is Nothing Then
                            Dim m As Match = r.Match(elementlist(i).InnerXml.ToString)
                            While (m.Success)
                                AddStringToArray(IDListRefs, m.Value.ToString())
                                m = m.NextMatch()
                            End While
                        End If
                    Next i
                Next k
                elementlist = doc.SelectNodes("//include | //onup | //ondown | //onleft | //onright | //animation | //onload | //onunload | //onclick | //onback | //focusedlayout | //itemlayout | //onfocus | //value")
                For i = 0 To elementlist.Count - 1
                    If Not elementlist(i).Attributes("condition") Is Nothing Then
                        Dim m As Match = r.Match(elementlist(i).Attributes("condition").InnerText.ToString)
                        While (m.Success)
                            AddStringToArray(IDListRefs, m.Value.ToString())
                            m = m.NextMatch()
                        End While
                    End If
                Next i
                AddAttributesToArray(IDListDefines, "//control[(@id)] | //window[(@id)] | //item[(@id)]", {"id"})
                AddAttributesToArray(IDListDefinesBackup, "//control[(@id)] | //window[(@id)] | //item[(@id)]", {"id"})
            Catch xmlex As XmlException                  ' Handle the Xml Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + xmlex.Message & vbCrLf)
            Catch ex As Exception                        ' Handle the generic Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + ex.Message & vbCrLf)
            End Try
        Next
        For i = 0 To IDListRefs.Count - 1
            RemoveStringFromArray(IDListDefines, IDListRefs(i))
        Next
        For i = 0 To IDListDefinesBackup.Count - 1
            RemoveStringFromArray(IDListRefs, IDListDefinesBackup(i))
        Next
        OutputLog.AppendText("Undefined IDs:" & vbCrLf)
        PrintArray(IDListRefs)
        '       OutputLog.AppendText("Undefined IDs:" & vbCrLf)
        '      For Each str In IDListDefines
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
        My.Settings.OutPutFolder = strOutputFolder
        MsgBox("Settings saved")
    End Sub

    Private Sub CheckValuesButton_Click(sender As System.Object, e As System.EventArgs) Handles CheckValuesButton.Click
        OutputLog.AppendText("Scanning XMLs..." & vbCrLf)
        For j = 0 To Filepaths.Count - 1
            Try
                doc.Load(Filepaths(j))
                actualFile = SafeFilepaths(j)
                CheckValues()
            Catch xmlex As XmlException                  ' Handle the Xml Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + xmlex.Message & vbCrLf)
            Catch ex As Exception                        ' Handle the generic Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + ex.Message & vbCrLf)
            End Try
        Next j
        OutputLog.AppendText("Scan complete" & vbCrLf)
    End Sub

    Sub CheckValues()
        CheckChildren("//control[@type='button']/*", {"description", "posx", "posy", "width", "height", "visible", "colordiffuse", "texturefocus", "include", "animation", "texturenofocus", "label", "label2", "font", "textcolor", "disabledcolor", "selectedcolor", "shadowcolor", "align", "aligny", "textoffsetx", "textoffsety", "pulseonselect", "onclick", "onfocus", "onunfocus", "onup", "onleft", "onright", "ondown", "onback", "textwidth", "focusedcolor", "angle", "hitrect", "enable"})
        CheckChildren("//control[@type='radiobutton']/*", {"description", "posx", "posy", "width", "height", "visible", "colordiffuse", "texturefocus", "include", "animation", "texturenofocus", "label", "selected", "font", "textcolor", "disabledcolor", "selectedcolor", "shadowcolor", "align", "aligny", "textoffsetx", "textoffsety", "pulseonselect", "onclick", "onfocus", "onunfocus", "onup", "onleft", "onright", "ondown", "onback", "textwidth", "focusedcolor", "angle", "hitrect", "enable", "textureradioon", "textureradiooff", "radioposx", "radioposy", "radiowidth", "radioheight"})
        CheckChildren("//control[@type='togglebutton']/*", {"description", "posx", "posy", "width", "height", "visible", "colordiffuse", "texturefocus", "alttexturefocus", "alttexturenofocus", "altclick", "include", "animation", "texturenofocus", "label", "altlabel", "usealttexture", "font", "textcolor", "disabledcolor", "shadowcolor", "align", "aligny", "textoffsetx", "textoffsety", "pulseonselect", "onclick", "onfocus", "onunfocus", "onup", "onleft", "onright", "ondown", "onback", "ondown", "ondown", "ondown", "textwidth", "focusedcolor", "subtype", "hitrect", "enable"})
        CheckChildren("//control[@type='label']/*", {"description", "posx", "posy", "width", "height", "visible", "align", "aligny", "include", "animation", "scroll", "scrollout", "info", "number", "angle", "haspath", "label", "textcolor", "selectedcolor", "font", "shadowcolor", "disabledcolor", "pauseatend", "wrapmultiline", "scrollspeed", "scrollsuffix", "textoffsetx", "textoffsety"})
        CheckChildren("//control[@type='textbox']/*", {"description", "posx", "posy", "width", "height", "visible", "align", "aligny", "include", "animation", "autoscroll", "label", "info", "font", "textcolor", "selectedcolor", "shadowcolor", "pagecontrol"})
        CheckChildren("//control[@type='image']/*", {"description", "posx", "posy", "width", "height", "visible", "align", "aligny", "include", "animation", "aspectratio", "fadetime", "colordiffuse", "texture", "bordertexture", "bordersize", "info"})
        CheckChildren("//control[@type='multiimage']/*", {"description", "posx", "posy", "width", "height", "visible", "align", "aligny", "include", "animation", "aspectratio", "fadetime", "colordiffuse", "imagepath", "timeperimage", "loop", "info", "randomize", "pauseatend"})
        CheckChildren("//control[@type='scrollbar']/*", {"description", "posx", "posy", "width", "height", "visible", "texturesliderbackground", "texturesliderbar", "include", "animation", "texturesliderbarfocus", "textureslidernib", "textureslidernibfocus", "pulseonselect", "orientation", "showonepage", "pagecontrol", "onclick", "onfocus", "onunfocus", "onup", "onleft", "onright", "ondown", "onback"})
        CheckChildren("//control[@type='progress']/*", {"description", "posx", "posy", "width", "height", "visible", "texturebg", "lefttexture", "include", "animation", "colordiffuse", "righttexture", "overlaytexture", "midtexture", "info", "reveal"})
        CheckChildren("//content/*", {"item", "include"})
        '     MoveNodeToBottom("animation")
        '    MoveNodeToBottom("visible")
        '      MoveNodeToBottom("include")
        If ReorderButton.Checked Then
            MoveNodeToTop("disabledcolor")
            MoveNodeToTop("selectedcolor")
            MoveNodeToTop("textcolor")
            MoveNodeToTop("shadowcolor")
            MoveNodeToTop("font")
            MoveNodeToTop("property")
            MoveNodeToTop("label2")
            MoveNodeToTop("altlabel")
            MoveNodeToTop("label")
            MoveNodeToTop("alttexturenofocus")
            MoveNodeToTop("alttexturefocus")
            MoveNodeToTop("texturenofocus")
            MoveNodeToTop("texturefocus")
            MoveNodeToTop("fadetime")
            MoveNodeToTop("texture")
            MoveNodeToTop("info")
            MoveNodeToTop("imagepath")
            MoveNodeToTop("aspectratio")
            MoveNodeToTop("aligny")
            MoveNodeToTop("align")
            MoveNodeToTop("textoffsety")
            MoveNodeToTop("textoffsetx")
            MoveNodeToTop("height")
            MoveNodeToTop("width")
            MoveNodeToTop("posy")
            MoveNodeToTop("posx")
            MoveNodeToTop("description")
        End If

        CheckAttributeValue("//*[(@align)]", {"align"}, {"left", "center", "right", "justify"})
        CheckAttributeValue("//*[(@aligny)]", {"aligny"}, {"top", "center", "bottom"})
        CheckAttributeValue("//*[(@flipx)]", {"flipx"}, {"true", "false"})
        CheckAttributeValue("//*[(@flipy)]", {"flipy"}, {"true", "false"})

        CheckNodeValue("align", {"left", "center", "right", "justify"})
        CheckNodeValue("aspectratio", {"keep", "scale", "stretch", "center"})
        CheckNodeValue("aligny", {"top", "center", "bottom"})
        CheckNodeValue("orientation", {"horizontal", "vertical"})
        CheckNodeValue("subtype", {"page", "int", "float", "text"})
        CheckNodeValue("action", {"volume", "seek"})
        CheckNodeValue("scroll", {"false", "true", "yes", "no"})
        CheckNodeValue("randomize", {"false", "true", "yes", "no"})
        CheckNodeValue("scrollout", {"false", "true", "yes", "no"})
        CheckNodeValue("pulseonselect", {"false", "true", "yes", "no"})
        CheckNodeValue("reverse", {"false", "true", "yes", "no"})
        CheckNodeValue("usecontrolcoords", {"false", "true", "yes", "no"})

        CheckAttributes("aspectratio", {"align", "aligny", "scalediffuse"})
        CheckAttributes("texture", {"background", "flipx", "flipy", "fallback", "border", "diffuse"})
        CheckAttributes("label", {"fallback"})
        CheckAttributes("align", {})
        CheckAttributes("aligny", {})
        CheckAttributes("posx", {})
        CheckAttributes("posy", {})
        CheckAttributes("height", {})
        CheckAttributes("width", {"min", "max"})
        CheckAttributes("camera", {"x", "y"})
        CheckAttributes("hitrect", {"x", "y", "w", "h"})
        CheckAttributes("textoffsetx", {})
        CheckAttributes("textoffsety", {})
        CheckAttributes("onload", {"condition"})
        CheckAttributes("onunload", {"condition"})
        CheckAttributes("onclick", {"condition"})
        CheckAttributes("onleft", {"condition"})
        CheckAttributes("onright", {"condition"})
        CheckAttributes("onup", {"condition"})
        CheckAttributes("ondown", {"condition"})
        CheckAttributes("onback", {"condition"})
        CheckAttributes("onfocus", {"condition"})
        CheckAttributes("onunfocus", {"condition"})
        CheckAttributes("property", {"name", "fallback"})
        CheckAttributes("focusedlayout", {"height", "width", "condition"})
        CheckAttributes("itemlayout", {"height", "width", "condition"})
        CheckAttributes("item", {"id"})
        CheckAttributes("control", {"id", "type"})
        CheckAttributes("animation", {"start", "end", "effect", "tween", "easing", "time", "condition", "reversible", "type", "center", "delay", "pulse", "loop", "acceleration"})
        CheckAttributes("effect", {"start", "end", "tween", "easing", "time", "condition", "type", "center", "delay", "pulse", "loop", "acceleration"})
        CheckDoubleValues()
        Dim Actions As String() = {"Help", "Reboot", "Restart", "ShutDown", "Powerdown", "Quit", "Hibernate", "Suspend", "InhibitIdleShutdown", "AllowIdleShutdown", "RestartApp", "Minimize"}
    End Sub

    Sub ScaleXMLNode(ByRef Element As XmlNode, ByVal tag As String, ByVal ScaleFactor As String)
        Dim Number As Double
        If (Not Element.Attributes(tag) Is Nothing) Then
            If (Double.TryParse(Element.Attributes(tag).InnerText.ToString, Number)) Then
                Element.Attributes(tag).InnerText = Math.Round(XmlConvert.ToDouble(Number) / RoundFactor * XmlConvert.ToDouble(ScaleFactor)) * RoundFactor
            End If
        End If
    End Sub

    Function ConvertValue(ByVal InputString As String, ByVal ScaleFactor As Double) As String
        Dim number As Integer
        Dim TempLetter As String
        ConvertValue = InputString
        If Double.TryParse(InputString, number) And (InputString <> "1") Then
            InputString = XmlConvert.ToDouble(InputString)
            ConvertValue = Math.Round(InputString * ScaleFactor).ToString
        Else
            If InputString.Length > 1 Then
                TempLetter = InputString.Substring(InputString.Length - 1, 1)
                InputString = InputString.Substring(0, InputString.Length - 1)
                If Int32.TryParse(InputString, number) Then
                    ConvertValue = Math.Round(number * multiplyFactor).ToString + TempLetter
                Else
                    If InputString.Length > 1 Then
                        TempLetter = InputString.Substring(InputString.Length - 1, 1)
                        InputString = InputString.Substring(0, InputString.Length - 1)
                        If Int32.TryParse(InputString, number) Then ConvertValue = Math.Round(number * multiplyFactor).ToString
                    End If
                End If
            End If
        End If
    End Function

    Sub convertString(ByRef Node As XmlNode, ByVal Tags As String(), Optional ByVal ConvertAll As Boolean = True)
        Dim Tag As String
        For Each Tag In Tags
            Dim IndexStart = 0
            Dim CompleteString As String
            Dim NewString As String = ""
            If Not Node.Attributes(Tag) Is Nothing Then
                CompleteString = Node.Attributes(Tag).InnerText.ToString
                If CompleteString.Contains(",") Or (ConvertAll = True) Then
                    For i = 0 To CompleteString.Length - 1
                        If CompleteString(i) = "," Then
                            NewString = NewString + ConvertValue(CompleteString.Substring(IndexStart, i - IndexStart), multiplyFactor) + ","
                            IndexStart = i + 1
                        End If
                    Next
                    CompleteString = NewString + ConvertValue((CompleteString.Substring(IndexStart, CompleteString.Length - IndexStart)), multiplyFactor)
                End If
                Node.Attributes(Tag).InnerText = CompleteString
            End If
        Next
    End Sub

    Sub changeAttributes()
        elementlist = doc.SelectNodes("//animation[@effect='zoom'] | //effect[@type='zoom']")
        For i = 0 To elementlist.Count - 1
            convertString(elementlist(i), {"start", "end"}, False)
        Next
        elementlist = doc.SelectNodes("//animation[@effect='slide'] | //effect[@type='slide']")
        For i = 0 To elementlist.Count - 1
            convertString(elementlist(i), {"start", "end"})
        Next
        elementlist = doc.SelectNodes("//animation[@effect='rotatex'] | //animation[@effect='rotatey'] | //animation[@effect='rotate'] | //effect[@type='rotate'] | //effect[@type='rotatex'] | //effect[@type='rotatey'] | //animation[@effect='slide'] | //effect[@type='slide']")
        For i = 0 To elementlist.Count - 1
            convertString(elementlist(i), {"end"})
        Next
        elementlist = doc.SelectNodes("//focusedlayout | //itemlayout | //channellayout | //focusedchannellayout | //rulerlayout")
        For i = 0 To elementlist.Count - 1
            convertString(elementlist(i), {"width", "height"})
        Next
        elementlist = doc.SelectNodes("//hitrect | //camera")
        For i = 0 To elementlist.Count - 1
            convertString(elementlist(i), {"x", "y", "w", "h"})
        Next
        elementlist = doc.SelectNodes("//width | //height")
        For i = 0 To elementlist.Count - 1
            convertString(elementlist(i), {"min", "max"})
        Next
        If ConvertBorders.Checked Then
            elementlist = doc.SelectNodes("//texturefocus | //texture | //texturenofocus | //bordertexture | //texturesliderbackground | //texturesliderbar | //texturesliderbarfocus | //alttexturenofocus | //alttexturefocus | //midtexture")
            For i = 0 To elementlist.Count - 1
                convertString(elementlist(i), {"border"})
            Next
        End If
    End Sub

    Sub TextureFinder(ByVal dir As String, Optional ByVal Lowercase As Boolean = True)
        Try
            For Each fname As String In Directory.GetFiles(dir)
                Dim number As Integer = 0
                Dim ShortPath As String = fname.Substring(SkinFolder.Length + 7, fname.Length - (SkinFolder.Length + 7))
                Dim blacklist As String() = {"flags\", "cerberus", "default", "stars", "rating", "\480p.png", "\540p.png", "\720p.png", "\576p.png", "\1080p.png", "overlay", ".xbt"}
                Dim blacklisted As Boolean = False
                For Each Item In blacklist
                    If ShortPath.Contains(Item) Then blacklisted = True
                Next
                If blacklisted = False Then
                    ShortPath = ShortPath.Replace("\", "/")
                    If Lowercase = True Then
                        ShortPath = ShortPath.ToLower
                    End If
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

    Sub PrintArray(ByVal PrintArray As ArrayList)
        Dim Line As String
        For Each Line In PrintArray
            OutputLog.AppendText(Line & vbCrLf)
        Next
    End Sub

    Sub RemoveAttributesFromArray(ByRef EditArray As ArrayList, ByVal NodeSelection As String, ByVal Attributes As String(), Optional ByVal Lowercase As Boolean = False)
        elementlist = doc.SelectNodes(NodeSelection)
        For i = 0 To elementlist.Count - 1
            For Each Attribute In Attributes
                If Not Attribute Is Nothing Then
                    If Not elementlist(i).Attributes(Attribute) Is Nothing Then
                        RemoveStringFromArray(EditArray, elementlist(i).Attributes(Attribute).InnerText.ToString, Lowercase)
                    End If
                End If
            Next Attribute
        Next
    End Sub

    Sub RemoveNodesFromArray(ByRef EditArray As ArrayList, ByVal NodeSelection As String, Optional ByVal Lowercase As Boolean = False)
        elementlist = doc.GetElementsByTagName(NodeSelection)
        For i = 0 To elementlist.Count - 1
            If Not elementlist(i).InnerXml Is Nothing Then RemoveStringFromArray(EditArray, elementlist(i).InnerXml, Lowercase)
        Next
    End Sub

    Sub RemoveStringFromArray(ByRef EditArray As ArrayList, ByVal StringToRemove As String, Optional ByVal Lowercase As Boolean = False)
        If Lowercase = True Then StringToRemove = StringToRemove.ToLower
        If EditArray.Contains(StringToRemove) Then EditArray.Remove(StringToRemove)
    End Sub

    Sub AddNodesToArray(ByRef EditArray As ArrayList, ByVal NodeSelection As String, Optional ByVal Lowercase As Boolean = False)
        elementlist = doc.GetElementsByTagName(NodeSelection)
        For i = 0 To elementlist.Count - 1
            If Not elementlist(i).InnerXml Is Nothing Then AddStringToArray(EditArray, elementlist(i).InnerXml, Lowercase)
        Next
    End Sub

    Sub AddAttributesToArray(ByRef EditArray As ArrayList, ByVal NodeSelection As String, ByVal AttributeList As String(), Optional ByVal Lowercase As Boolean = False)
        elementlist = doc.SelectNodes(NodeSelection)
        For i = 0 To elementlist.Count - 1
            For Each Attribute In AttributeList
                If Not elementlist(i).Attributes(Attribute).InnerText Is Nothing Then AddStringToArray(EditArray, elementlist(i).Attributes(Attribute).InnerText.ToString, Lowercase)
            Next Attribute
        Next
    End Sub

    Sub AddArrayListToArray(ByRef EditArray As ArrayList, ByVal ArrayToAdd As ArrayList, Optional ByVal Lowercase As Boolean = False)
        For Each StringToAdd In ArrayToAdd
            AddStringToArray(EditArray, StringToAdd, Lowercase)
        Next StringToAdd
    End Sub

    Sub AddStringToArray(ByRef EditArray As ArrayList, ByVal StringToAdd As String, Optional ByVal Lowercase As Boolean = False)
        If Lowercase = True Then StringToAdd = StringToAdd.ToLower
        '      StringToAdd.Replace("$LOCALIZE[", "")
        '       StringToAdd.Replace("]", "")
        If Not EditArray.Contains(StringToAdd) Then EditArray.Add(StringToAdd)
    End Sub

    Private Function CheckPath(ByVal strPath As String) As Boolean
        If Dir$(strPath) <> "" Then CheckPath = True Else CheckPath = False
    End Function
    Private Sub MoveNodeToTop(ByVal XMLTag As String)
        elementlist = doc.GetElementsByTagName(XMLTag)
        For i = 0 To elementlist.Count - 1
            If Not elementlist(i) Is Nothing Then
                If Not elementlist(i).ParentNode.FirstChild.Equals(elementlist(i)) And Not elementlist(i).HasChildNodes Then
                    elementlist(i).ParentNode.InsertBefore(elementlist(i), elementlist(i).ParentNode.FirstChild)
                End If
            End If
        Next
    End Sub
    Private Sub MoveNodeToBottom(ByVal XMLTag As String)
        elementlist = doc.GetElementsByTagName(XMLTag)
        For i = 0 To elementlist.Count - 1
            If Not elementlist(i) Is Nothing Then
                If Not elementlist(i).ParentNode.LastChild.Equals(elementlist(i)) And Not elementlist(i).HasChildNodes Then
                    '              elementlist(i).ParentNode.ChildNodes()
                    elementlist(i).ParentNode.InsertBefore(elementlist(i), elementlist(i).ParentNode.LastChild)
                End If
            End If
        Next
    End Sub
    Private Sub CheckNodeValue(ByVal XMLTag As String, ByVal ValidValues As String())
        elementlist = doc.GetElementsByTagName(XMLTag)
        For i = 0 To elementlist.Count - 1
            If Not elementlist(i) Is Nothing Then
                If Not ValidValues.Contains(elementlist(i).InnerXml.ToString.ToLower) Then
                    OutputLog.AppendText(actualFile + ": Invalid Value for " & XMLTag & ": " & elementlist(i).InnerXml & vbCrLf)
                    If AutoFixCheckBox.Checked Then
                        elementlist(i).ParentNode.RemoveChild(elementlist(i))
                    End If
                End If
            End If
        Next
    End Sub
    Private Sub CheckAttributes(ByVal XMLTag As String, ByVal ValidValues As String())
        elementlist = doc.GetElementsByTagName(XMLTag)
        For i = 0 To elementlist.Count - 1
            For Each Attribute As XmlAttribute In elementlist(i).Attributes
                If Not Attribute Is Nothing Then
                    Dim Test As String = Attribute.Name
                    If Not ValidValues.Contains(Test) Then
                        OutputLog.AppendText(actualFile + ": Invalid Attribute for " & XMLTag & ": " & Test & vbCrLf)
                        If AutoFixCheckBox.Checked Then
                            elementlist(i).Attributes.RemoveNamedItem(Test)
                        End If
                    End If
                End If
            Next
        Next
    End Sub
    Private Sub CheckDoubleValues()
        elementlist = doc.SelectNodes("//*")
        Dim IgnoreList As String() = {"control", "item", "onclick", "onleft", "onright", "onup", "ondown", "property", "include", "onback", "group", "animation", "effect", "onload", "onunload", "visible", "enable", "itemlayout", "focusedlayout", "variable", "value", "altclick", "onfocus", "#comment", "default", "font", "constant", "style", "label"}
        For i = 0 To elementlist.Count - 1
            If elementlist(i).HasChildNodes Then
                Dim DoubleValuesList As New ArrayList()
                DoubleValuesList.Clear()
                For Each element In elementlist(i).ChildNodes
                    Dim blacklisted As Boolean = False
                    For Each Item In IgnoreList
                        If element.Name.Contains(Item) Then blacklisted = True
                    Next
                    If blacklisted = False Then
                        If Not DoubleValuesList.Contains(element.Name) Then
                            AddStringToArray(DoubleValuesList, element.Name)
                        Else
                            OutputLog.AppendText("Double XML Tag: " & element.Name & " in " & actualFile & vbCrLf)
                            If AutoFixCheckBox.Checked Then
                                element.ParentNode.RemoveChild(element)
                            End If
                        End If
                    End If
                Next
            End If
        Next
    End Sub
    Private Sub CheckChildren(ByVal XMLTag As String, ByVal ValidValues As String())
        elementlist = doc.SelectNodes(XMLTag)
        For i = 0 To elementlist.Count - 1
            If Not elementlist(i) Is Nothing Then
                If Not ValidValues.Contains(elementlist(i).Name) Then
                    OutputLog.AppendText(actualFile + ": Invalid Value for " & XMLTag & ": " & elementlist(i).Name & vbCrLf)
                    If AutoFixCheckBox.Checked Then
                        elementlist(i).ParentNode.RemoveChild(elementlist(i))
                    End If
                End If
            End If
        Next
    End Sub
    Private Sub CheckAttributeValue(ByVal XMLTag As String, ByVal Attributes As String(), ByVal ValidValues As String())
        elementlist = doc.SelectNodes(XMLTag)
        For i = 0 To elementlist.Count - 1
            For Each Attribute In Attributes
                If Not Attribute Is Nothing Then
                    If Not elementlist(i).Attributes(Attribute) Is Nothing Then
                        If Not ValidValues.Contains(elementlist(i).Attributes(Attribute).InnerText.ToString) Then
                            OutputLog.AppendText(actualFile + ": Invalid Value for " & XMLTag & ": " & elementlist(i).Attributes(Attribute).InnerText.ToString & vbCrLf)
                        End If
                    End If
                End If
            Next Attribute
        Next
    End Sub
    Private Sub CheckAttributesValue(ByVal XMLTag As String, ByVal Attribute As String, ByVal ValidValues As String())
        elementlist = doc.GetElementsByTagName(XMLTag)
        For i = 0 To elementlist.Count - 1
            If Not ValidValues.Contains(elementlist(i).InnerXml.ToString.ToLower) Then
                OutputLog.AppendText(actualFile + ": Invalid Value for " & XMLTag & ": " & elementlist(i).InnerXml & vbCrLf)
            End If
        Next
    End Sub

    Public Sub SameCharNumber(ByVal value As String, ByVal CompareChar1 As Char, ByVal CompareChar2 As Char)
        Dim cnt As Integer = 0
        Dim Unmatched As Boolean = False
        For Each c As Char In value
            If c = CompareChar1 Then cnt += 1
            If c = CompareChar2 Then cnt -= 1
            If cnt < 0 Then Unmatched = True
        Next
        If (cnt <> 0) Or (Unmatched = True) Then OutputLog.AppendText("Unmatched parenthesis: " + value & vbCrLf)
    End Sub

    Private Sub CheckVarsButton_Click(sender As System.Object, e As System.EventArgs) Handles CheckVarsButton.Click
        Dim VarsListRefs As New ArrayList()
        Dim VarsListDefines As New ArrayList()
        Dim VarsListDefinesBackup As New ArrayList()
        Dim r As Regex = New Regex("(?<=\$VAR\[)[0-9A-Za-z-]+", RegexOptions.IgnoreCase)
        VarsListRefs.Clear()
        VarsListDefines.Clear()
        VarsListDefinesBackup.Clear()
        OutputLog.AppendText("Checking the Vars..." & vbCrLf)
        For j = 0 To Filepaths.Count - 1
            Try
                doc.Load(Filepaths(j))
                actualFile = SafeFilepaths(j)
                elementlist = doc.SelectNodes("//*")
                For i = 0 To elementlist.Count - 1
                    If Not elementlist(i).InnerXml Is Nothing Then
                        Dim m As Match = r.Match(elementlist(i).InnerXml.ToString)
                        While (m.Success)
                            AddStringToArray(VarsListRefs, m.Value.ToString())
                            m = m.NextMatch()
                        End While
                    End If
                Next i
                AddAttributesToArray(VarsListDefines, "//variable", {"name"})
                AddAttributesToArray(VarsListDefinesBackup, "//variable", {"name"})
            Catch xmlex As XmlException                  ' Handle the Xml Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + xmlex.Message & vbCrLf)
            Catch ex As Exception                        ' Handle the generic Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + ex.Message & vbCrLf)
            End Try
        Next
        For i = 0 To VarsListRefs.Count - 1
            RemoveStringFromArray(VarsListDefines, VarsListRefs(i))
        Next
        For i = 0 To VarsListDefinesBackup.Count - 1
            RemoveStringFromArray(VarsListRefs, VarsListDefinesBackup(i))
        Next
        OutputLog.AppendText("Undefined Vars:" & vbCrLf)
        PrintArray(VarsListRefs)
        OutputLog.AppendText("Unused Vars:" & vbCrLf)
        PrintArray(VarsListDefines)
    End Sub

    Private Sub CheckLabelsButton_Click(sender As System.Object, e As System.EventArgs) Handles CheckLabelsButton.Click
        Dim LabelsListRefs As New ArrayList()
        Dim LabelsListDefines As New ArrayList()
        Dim LabelsListDefinesBackup As New ArrayList()
        '   Dim t As Regex = New Regex("(?<=\#)[0-9]+", RegexOptions.IgnoreCase)
        Dim r As Regex = New Regex("((?<=\$LOCALIZE\[)[0-9]+)|([0-9]+)", RegexOptions.IgnoreCase)
        Dim RegExNumber As Regex = New Regex("\d+")
        LabelsListRefs.Clear()
        LabelsListDefines.Clear()
        LabelsListDefinesBackup.Clear()
        Dim text = File.ReadAllLines(SkinFolder + "\language\english\strings.po")
        For Each TextLine In text
            If TextLine.StartsWith("msgctxt") Then
                Dim match As Match = RegExNumber.Match(TextLine)
                AddStringToArray(LabelsListDefines, match.Value.ToString)
                AddStringToArray(LabelsListDefinesBackup, match.Value.ToString)
            End If
        Next
        OutputLog.AppendText("Checking the Labels..." & vbCrLf)
        For j = 0 To Filepaths.Count - 1
            Try
                doc.Load(Filepaths(j))
                actualFile = SafeFilepaths(j)
                elementlist = doc.SelectNodes("//label | //altlabel | //label2 | //value | //onclick | //property")
                For i = 0 To elementlist.Count - 1
                    If Not elementlist(i).InnerXml Is Nothing Then
                        Dim m As Match = r.Match(elementlist(i).InnerXml.ToString)
                        While (m.Success)
                            Dim Result As String = m.Value.ToString()
                            AddStringToArray(LabelsListRefs, Result)
                            m = m.NextMatch()
                        End While
                    End If
                Next i
                elementlist = doc.SelectNodes("//label | //altlabel | //label2")
                For i = 0 To elementlist.Count - 1
                    If Not elementlist(i).InnerXml Is Nothing Then
                        Dim m As Match = r.Match(elementlist(i).InnerXml.ToString)
                        If (m.Success = False) And (Not elementlist(i).InnerXml.ToString.Contains("$INFO")) And (Not elementlist(i).InnerXml.ToString.Contains("$VAR")) And (Not elementlist(i).InnerXml.ToString = "-") Then
                            OutputLog.AppendText(SafeFilepaths(j) + ": Untranslated Label:" + elementlist(i).InnerXml.ToString & vbCrLf)
                        End If
                    End If
                Next i
                AddAttributesToArray(LabelsListRefs, "//viewtype[(@label)]", {"label"})
                AddAttributesToArray(LabelsListRefs, "//fontset[(@idloc)]", {"idloc"})
                AddAttributesToArray(LabelsListRefs, "//*[(@fallback)]", {"fallback"})
            Catch xmlex As XmlException                  ' Handle the Xml Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + xmlex.Message & vbCrLf)
            Catch ex As Exception                        ' Handle the generic Exceptions here.
                OutputLog.AppendText(SafeFilepaths(j) + ": " + ex.Message & vbCrLf)
            End Try
        Next
        For i = 0 To LabelsListRefs.Count - 1
            Dim match As Match = RegExNumber.Match(LabelsListRefs(i))
            RemoveStringFromArray(LabelsListDefines, match.Value.ToString)
        Next
        '  OutputLog.AppendText(vbCrLf & "LabelsListRefs:" & vbCrLf & vbCrLf)
        '   PrintArray(LabelsListRefs)
        '    OutputLog.AppendText(vbCrLf & "LabelsListDefinesBackup:" & vbCrLf & vbCrLf)
        '     PrintArray(LabelsListDefinesBackup)
        OutputLog.AppendText(vbCrLf & "Unused Strings:" & vbCrLf & vbCrLf)
        PrintArray(LabelsListDefines)
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs)
        Dim reader As XmlReader = XmlReader.Create(XMLFolder + "\MyVideoNav.xml")
        Dim schemaSet As XmlSchemaSet = New XmlSchemaSet()
        Dim schema As XmlSchemaInference = New XmlSchemaInference()
        schemaSet = schema.InferSchema(reader)
        For Each s As XmlSchema In schemaSet.Schemas()
            s.Write(Console.Out)
        Next
    End Sub
End Class


