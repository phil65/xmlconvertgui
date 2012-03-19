Imports System.Xml
Imports System.Text
Imports System
Imports System.IO
Imports System.Security
Imports System.Security.Principal.WindowsIdentity
Public Class Filechooser
    Public strOutputFolder As String = ""
    Public xmlelements As String() = {"posx", "posy", "width", "height", "textoffsetx", "textoffsety", "radiowidth", "radioheight", "radioposx", "radioposy", "textwidth", "border", "size", "bordersize", "itemgap"}
    Public xmlattributes As String(,)
    Public doc As New XmlDocument()
    Public multiplyFactor As Double = 1.5
    Public Filenames As String() = {}
    Public SafeFilenames As String() = {}
    Public elementlist As XmlNodeList
    Public TempLetter As String
    Public ElementCounter As String
    Private Sub Filechooser_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ConversionDropDown.Items.Add("720p --> 1080p")
        ConversionDropDown.Items.Add("No Change")
        ConversionDropDown.SelectedIndex = 0
        EncodingDropDown.Items.Add("UTF-8")
        EncodingDropDown.Items.Add("ANSI")
        EncodingDropDown.SelectedIndex = 0
        OutputLog.AppendText("Program started" & vbCrLf)
    End Sub

    Private Sub ChooseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChooseButton.Click

        OpenFileDialog.Title = "Choose XML file"
        OpenFileDialog.Filter = "XML Files|*.xml"
        Dim DidWork As Integer = OpenFileDialog.ShowDialog()
        If DidWork = DialogResult.Cancel Then

        Else
            OutputButton.Visible = True
            OutputLabel.Visible = True
            Filenames = OpenFileDialog.FileNames
            SafeFilenames = OpenFileDialog.SafeFileNames
            If Filenames.Length = 1 Then
                OutputLog.AppendText("XML File chosen: " + SafeFilenames(0) & vbCrLf)
                xmlname.Text = SafeFilenames(0)
            Else
                OutputLog.AppendText("Amount Of Files Chosen: " + Filenames.Length.ToString & vbCrLf)
                For i = 0 To Filenames.Length - 1
                    OutputLog.AppendText(SafeFilenames(i) & vbCrLf)
                Next i
                xmlname.Text = Filenames.Length.ToString + " Files chosen"
            End If
            If strOutputFolder <> "" Then
                ConvertButton.Enabled = True
            End If
        End If
    End Sub

    Public Sub ConvertButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConvertButton.Click
        ' Create an XML declaration. 
        Dim xmldecl As XmlDeclaration
        xmldecl = doc.CreateXmlDeclaration("1.0", Nothing, Nothing)
        Select Case EncodingDropDown.SelectedIndex
            Case 0
                xmldecl.Encoding = "UTF-8"
            Case 1
                xmldecl.Encoding = "ISO-8859-1"
        End Select
        xmldecl.Standalone = "yes"
        Select Case ConversionDropDown.SelectedIndex
            Case 0
                multiplyFactor = 1.5
                OutputLog.AppendText("converting 720 ==> 1080: Factor 1.5" & vbCrLf)
            Case 1
                multiplyFactor = 1
                OutputLog.AppendText("only converting Encoding / adding Headers" & vbCrLf)
        End Select
        ElementCounter = 0
        Dim errorcounter = 0
        For j = 0 To Filenames.Length - 1
            Try
                OutputLog.AppendText("Processing " + SafeFilenames(j) & vbCrLf)
                doc.Load(Filenames(j))
                If HeaderOption.Checked Then
                    Dim root As XmlElement = doc.DocumentElement
                    doc.InsertBefore(xmldecl, root)
                End If
                If multiplyFactor <> 1 Then
                    For i = 0 To xmlelements.Length - 1
                        changeElements(xmlelements(i))
                    Next i
                    changeAttributes()
                End If
                Dim wrtr As XmlTextWriter = Nothing
                Select Case EncodingDropDown.SelectedIndex
                    Case 0
                        wrtr = New XmlTextWriter(strOutputFolder + "\" + SafeFilenames(j), Encoding.UTF8)
                    Case 1
                        wrtr = New XmlTextWriter(strOutputFolder + "\" + SafeFilenames(j), Encoding.ASCII)
                End Select
                wrtr.Formatting = Formatting.Indented
                doc.WriteTo(wrtr)
                wrtr.Close()
                OutputLog.AppendText(SafeFilenames(j) + " created successfully" & vbCrLf)
                ElementCounter = ElementCounter + 1
            Catch xmlex As XmlException                  ' Handle the Xml Exceptions here.
                OutputLog.AppendText(xmlex.Message)
                errorcounter = errorcounter + 1
            Catch ex As Exception                        ' Handle the generic Exceptions here.
                OutputLog.AppendText(ex.Message)
                errorcounter = errorcounter + 1
            End Try
        Next j
        OutputLog.AppendText("All Files converted" & vbCrLf)
        MsgBox(ElementCounter + " XML Files converted." & vbCrLf & "Errors: " + errorcounter.ToString)
        errorcounter = 0
        ReDim Filenames(0)
        ReDim SafeFilenames(0)
        xmlname.Text = ""
        ConvertButton.Enabled = False

    End Sub

    Private Sub OutputButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OutputButton.Click
        Do
            FolderBrowserDialog.Description = "Choose Output Folder"

            Dim DidWork As Integer = FolderBrowserDialog.ShowDialog()
            If DidWork = DialogResult.Cancel Then

            Else
                strOutputFolder = FolderBrowserDialog.SelectedPath
                If Filenames(0) <> "" Then
                    ConvertButton.Enabled = True
                End If
                OutputLabel.Text = strOutputFolder + "\"
                OutputLog.AppendText("Output Folder chosen:" & vbCrLf)
                OutputLog.AppendText(strOutputFolder & vbCrLf)
            End If
            If (strOutputFolder + "\" + SafeFilenames(0) = Filenames(0)) Then
                MsgBox("You´ve chosen the soure directory. please change the output path.")
            End If
        Loop While (strOutputFolder + "\" + SafeFilenames(0) = Filenames(0))


    End Sub

    Sub changeElements(ByVal tag As String)
        If DebugOutput.Checked Then
            OutputLog.AppendText("Changing " + tag + " values..." & vbCrLf)
        End If
        elementlist = doc.GetElementsByTagName(tag)
        For i = 0 To elementlist.Count - 1
            If DebugOutput.Checked Then
                OutputLog.AppendText(elementlist(i).InnerXml.ToString & " ==> " & ConvertValue(elementlist(i).InnerXml) & vbCrLf)
            End If
            elementlist(i).InnerXml = ConvertValue(elementlist(i).InnerXml)
        Next i
    End Sub

    Function ConvertValue(ByVal InputString As String) As String
        Dim number As Integer
        ConvertValue = InputString
        If Int32.TryParse(InputString, number) Then
            ConvertValue = Math.Round(InputString * multiplyFactor)
        Else
            If DebugOutput.Checked Then
                OutputLog.AppendText("Attempted conversion of " + InputString + " failed." & vbCrLf)
            End If
            If InputString.Length > 1 Then
                TempLetter = InputString.ToString.Substring(InputString.Length - 1, 1)
                InputString = InputString.ToString.Substring(0, InputString.Length - 1)
                If DebugOutput.Checked Then
                    OutputLog.AppendText("Removed last letter... " & vbCrLf)
                    OutputLog.AppendText(InputString & vbCrLf)
                End If
                If Int32.TryParse(InputString, number) Then
                    ConvertValue = Math.Round(number * multiplyFactor).ToString + TempLetter
                    '   ConvertValue = Math.Round(number * multiplyFactor).ToString
                    If DebugOutput.Checked Then
                        OutputLog.AppendText("Conversion successful" & vbCrLf)
                    End If
                    OutputLog.AppendText(InputString & vbCrLf)
                Else
                    If InputString.Length > 1 Then
                        TempLetter = InputString.ToString.Substring(InputString.Length - 1, 1)
                        InputString = InputString.ToString.Substring(0, InputString.Length - 1)
                        If DebugOutput.Checked Then
                            OutputLog.AppendText("Removed last letter... " & vbCrLf)
                            OutputLog.AppendText(InputString & vbCrLf)
                        End If
                        If Int32.TryParse(InputString, number) Then
                            '      ConvertValue = Math.Round(number * multiplyFactor).ToString + TempLetter
                            ConvertValue = Math.Round(number * multiplyFactor).ToString
                            If DebugOutput.Checked Then
                                OutputLog.AppendText("Conversion successful" & vbCrLf)
                            End If
                            OutputLog.AppendText(InputString & vbCrLf)
                        Else
                            If DebugOutput.Checked Then
                                OutputLog.AppendText("not converted... " & vbCrLf)
                            End If
                        End If
                    Else
                        If DebugOutput.Checked Then
                            OutputLog.AppendText("not converted... " & vbCrLf)
                        End If
                    End If
                End If
            End If
        End If
    End Function
    Function ConvertValue(ByVal InputString As String, ByVal orientation As String) As String
        Dim number As Integer
        ConvertValue = InputString
        If Int32.TryParse(InputString, number) Then
            ConvertValue = Math.Round(InputString * multiplyFactor)
        Else
            If DebugOutput.Checked Then
                OutputLog.AppendText("Attempted conversion of " + InputString + " failed." & vbCrLf)
            End If
            If InputString.Length > 1 Then
                TempLetter = InputString.ToString.Substring(InputString.Length - 1, 1)
                InputString = InputString.ToString.Substring(0, InputString.Length - 1)
                If DebugOutput.Checked Then
                    OutputLog.AppendText("Removed last letter... " & vbCrLf)
                    OutputLog.AppendText(InputString & vbCrLf)
                End If
                If Int32.TryParse(InputString, number) Then
                    ConvertValue = Math.Round(number * multiplyFactor).ToString + TempLetter
                    '   ConvertValue = Math.Round(number * multiplyFactor).ToString
                    If DebugOutput.Checked Then
                        OutputLog.AppendText("Conversion successful" & vbCrLf)
                    End If
                    OutputLog.AppendText(InputString & vbCrLf)
                Else
                    If InputString.Length > 1 Then
                        TempLetter = InputString.ToString.Substring(InputString.Length - 1, 1)
                        InputString = InputString.ToString.Substring(0, InputString.Length - 1)
                        If DebugOutput.Checked Then
                            OutputLog.AppendText("Removed last letter... " & vbCrLf)
                            OutputLog.AppendText(InputString & vbCrLf)
                        End If
                        If Int32.TryParse(InputString, number) Then
                            '      ConvertValue = Math.Round(number * multiplyFactor).ToString + TempLetter
                            ConvertValue = Math.Round(number * multiplyFactor).ToString
                            If DebugOutput.Checked Then
                                OutputLog.AppendText("Conversion successful" & vbCrLf)
                            End If
                            OutputLog.AppendText(InputString & vbCrLf)
                        Else
                            If DebugOutput.Checked Then
                                OutputLog.AppendText("not converted... " & vbCrLf)
                            End If
                        End If
                    Else
                        If DebugOutput.Checked Then
                            OutputLog.AppendText("not converted... " & vbCrLf)
                        End If
                    End If
                End If
            End If
        End If
    End Function

    Sub convertString(ByRef CompleteString As String)
        Dim NewString As String = ""
        Dim TempString As String = CompleteString
        Dim IndexStart = 0
        For i = 0 To CompleteString.Length - 1
            If CompleteString(i) = "," Then
                If DebugOutput.Checked Then
                    OutputLog.AppendText("comma detected. Pos i:" + i.ToString & vbCrLf)
                End If
                NewString = NewString + ConvertValue(CompleteString.Substring(IndexStart, i - IndexStart)) + ","
                IndexStart = i + 1
            End If
        Next
        CompleteString = NewString + ConvertValue((CompleteString.Substring(IndexStart, CompleteString.Length - IndexStart)))
        If DebugOutput.Checked Then
            OutputLog.AppendText(TempString & " ==> " & CompleteString & vbCrLf)
        End If
    End Sub

    Sub convertStringifseveral(ByRef CompleteString As String)
        Dim NewString As String = ""
        Dim IndexStart = 0
        Dim TempString As String = CompleteString
        If CompleteString.Contains(",") Then
            For i = 0 To CompleteString.Length - 1
                If CompleteString(i) = "," Then
                    If DebugOutput.Checked Then
                        OutputLog.AppendText("comma detected. Pos i:" + i.ToString & vbCrLf)
                    End If
                    NewString = NewString + ConvertValue(CompleteString.Substring(IndexStart, i - IndexStart)) + ","
                    IndexStart = i + 1
                End If
            Next
            CompleteString = NewString + ConvertValue((CompleteString.Substring(IndexStart, CompleteString.Length - IndexStart)))
            If DebugOutput.Checked Then
                OutputLog.AppendText(TempString & " ==> " & CompleteString & vbCrLf)
            End If
        End If
    End Sub
    Sub convertStringifseveralstretch(ByRef CompleteString As String)
        Dim NewString As String = ""
        Dim IndexStart = 0
        Dim TempString As String = CompleteString
        Dim isX As Boolean = True
        If CompleteString.Contains(",") Then
            For i = 0 To CompleteString.Length - 1
                If CompleteString(i) = "," Then
                    If DebugOutput.Checked Then
                        OutputLog.AppendText("comma detected. Pos i:" + i.ToString & vbCrLf)
                    End If
                    NewString = NewString + ConvertValue(CompleteString.Substring(IndexStart, i - IndexStart)) + ","
                    If isX = True Then
                        isX = False
                    Else
                        isX = True
                    End If
                    IndexStart = i + 1
                End If
            Next
            CompleteString = NewString + ConvertValue((CompleteString.Substring(IndexStart, CompleteString.Length - IndexStart)))
            If DebugOutput.Checked Then
                OutputLog.AppendText(TempString & " ==> " & CompleteString & vbCrLf)
            End If
        End If
    End Sub

    Sub changeAttributes()
        If DebugOutput.Checked Then
            OutputLog.AppendText("Changing Attributes..." & vbCrLf)
        End If
        elementlist = doc.SelectNodes("//animation[@effect='zoom'] | //effect[@type='zoom']")
        For i = 0 To elementlist.Count - 1
            If Not elementlist(i).Attributes("start") Is Nothing Then
                convertStringifseveral(elementlist(i).Attributes("start").InnerText)
            End If
            If Not elementlist(i).Attributes("end") Is Nothing Then
                convertStringifseveral(elementlist(i).Attributes("end").InnerText)
            End If
            If Not elementlist(i).Attributes("zoom") Is Nothing Then
                convertStringifseveral(elementlist(i).Attributes("zoom").InnerText)
            End If
        Next
        elementlist = doc.SelectNodes("//animation[@effect='slide'] | //effect[@type='slide']")
        For i = 0 To elementlist.Count - 1
            If Not elementlist(i).Attributes("start") Is Nothing Then
                convertString(elementlist(i).Attributes("start").InnerText)
            End If
            If Not elementlist(i).Attributes("end") Is Nothing Then
                convertString(elementlist(i).Attributes("end").InnerText)
            End If
        Next
        elementlist = doc.SelectNodes("//animation[@effect='rotatex'] | //animation[@effect='rotatey'] | //animation[@effect='rotate'] | //effect[@type='rotate'] | //effect[@type='rotatex'] | //effect[@type='rotatey'] | //animation[@effect='slide'] | //effect[@type='slide']")
        For i = 0 To elementlist.Count - 1
            If Not elementlist(i).Attributes("center") Is Nothing Then
                convertString(elementlist(i).Attributes("center").InnerText)
            End If
        Next
        elementlist = doc.SelectNodes("//texturefocus | //texture | //texturenofocus | //bordertexture | //texturesliderbackground | //texturesliderbar | //texturesliderbarfocus | //alttexturenofocus | //alttexturefocus | //midtexture")
        For i = 0 To elementlist.Count - 1
            If Not elementlist(i).Attributes("border") Is Nothing Then
                convertString(elementlist(i).Attributes("border").InnerText)
            End If
        Next
        elementlist = doc.SelectNodes("//focusedlayout | //itemlayout")
        For i = 0 To elementlist.Count - 1
            If Not elementlist(i).Attributes("width") Is Nothing Then
                convertString(elementlist(i).Attributes("width").InnerText)
            End If
            If Not elementlist(i).Attributes("height") Is Nothing Then
                convertString(elementlist(i).Attributes("height").InnerText)
            End If
        Next
        elementlist = doc.SelectNodes("//hitrect | //camera")
        For i = 0 To elementlist.Count - 1
            If Not elementlist(i).Attributes("x") Is Nothing Then
                convertString(elementlist(i).Attributes("x").InnerText)
            End If
            If Not elementlist(i).Attributes("y") Is Nothing Then
                convertString(elementlist(i).Attributes("y").InnerText)
            End If
            If Not elementlist(i).Attributes("h") Is Nothing Then
                convertString(elementlist(i).Attributes("h").InnerText)
            End If
            If Not elementlist(i).Attributes("w") Is Nothing Then
                convertString(elementlist(i).Attributes("w").InnerText)
            End If
        Next
        elementlist = doc.SelectNodes("//width | //height")
        For i = 0 To elementlist.Count - 1
            If Not elementlist(i).Attributes("min") Is Nothing Then
                convertString(elementlist(i).Attributes("min").InnerText)
            End If
            If Not elementlist(i).Attributes("max") Is Nothing Then
                convertString(elementlist(i).Attributes("max").InnerText)
            End If
        Next
    End Sub

End Class
