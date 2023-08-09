Imports System.IO.File
Imports System
Imports System.Management
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Security.AccessControl


Public Class PtiStock
    Dim appXls As Excel.Application
    Dim bookXls As Excel.Workbook
    Dim sheetXls As Excel.Worksheet
    Dim dtmTest As Date
    Dim ss As Excel.Range
    ' Dim Vente As Excel.Application
    ' Dim livrevente As Excel.Workbook
    Dim feuilleVente As Excel.Worksheet
    Dim feuillesortie As Excel.Worksheet
    Dim cellvente As String
    Dim cellstock As String
    Dim cellsortie As String
    Dim countgrid2 As Integer = 0
    Dim globalventecompteur As Long
    Dim globalstockcompteur As Long
    Dim globalsortiecompteur As Long
    Dim colonneindex As Integer
    Dim chaine5debut As String
    Dim chaine6debut, text1, text2, text3, text7, text13, text14, ancienprixachat, text15, text16, text17, text18, text19, text20, ancienprix, anciennqte, ancienref, ancienprixsortie, ancienqtesortie, ancienrefsortie, ancientva, ancienpvttc, ancientvasortie, ancienprixachatsortie As String
    Dim testnouveau As String
    ' verouiller le bouton e    xit de la grande fenetre Form3
    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            Const CS_NOCLOSE As Integer = &H200
            cp.ClassStyle = cp.ClassStyle Or CS_NOCLOSE
            Return cp
        End Get
    End Property

    ' Private Property DateTimePickerFormat As String

    ' fonction qui lit le numéro de série du disque dur : pour éviter de copier 
    Private Function GetDriveSerialNumber(ByVal drive As String) As String

        Dim driveSerial As String = String.Empty
        Dim driveFixed As String = System.IO.Path.GetPathRoot(drive)

        Using querySearch As New ManagementObjectSearcher("SELECT VolumeSerialNumber FROM Win32_LogicalDisk Where Name = '" & driveFixed & "'")

            Using queryCollection As ManagementObjectCollection = querySearch.Get()

                Dim moItem As ManagementObject

                For Each moItem In queryCollection

                    driveSerial = CStr(moItem.Item("VolumeSerialNumber"))

                    Exit For
                Next
            End Using
        End Using
        Return driveSerial

    End Function
    Public Sub New()
        Dim nomstock As String
        Dim serialHD As String
        ' Dim TrueserialHD As String = "74031E15"  ' chez mongi
        ' Dim TrueserialHD As String = "0CBA4FDB"  ' chez wam

        InitializeComponent()
        dtmTest = DateValue(Now)

        ' Verification du licence '
        serialHD = GetDriveSerialNumber("D:")
        '  If TrueserialHD <> serialHD Then
        ' MsgBox("Appelez kamel (+216) 97651213 : code erreur 001 !", vbYes + vbCritical, "Alerte Licence")
        ' Me.Close()
        ' End
        ' Else

        If (dtmTest.Year.ToString <> "2015") Then
            MsgBox(" Version périmée : Appelez Kamel (+216) 97651213 : code erreur 002 !", vbYes + vbCritical, "Alerte version périmée")
            Me.Close()
            End
        Else
        End If

        'appXls = CreateObject("Excel.Application")
        nomstock = "D:\QuinStock\Stock" & dtmTest.Year & ".xls"

        If IO.File.Exists(nomstock) Then
            appXls = CreateObject("Excel.Application")
            bookXls = appXls.Workbooks.Open(nomstock)
            sheetXls = bookXls.Worksheets("Sheet1")
            feuilleVente = bookXls.Worksheets("Sheet2")
            feuillesortie = bookXls.Worksheets("Sheet3")

            ss = sheetXls.Cells.Range("B1:B65535").Find(String.Empty, , Excel.XlFindLookIn.xlValues, , Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious)
            If Not IsNothing(ss) Then
                globalstockcompteur = sheetXls.Cells.Range("B1:B65535").Find(String.Empty, , Excel.XlFindLookIn.xlValues, , Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious).Row - 1
            Else
                globalstockcompteur = 1
            End If

            ss = feuilleVente.Cells.Range("B1:B65535").Find(String.Empty, , Excel.XlFindLookIn.xlValues, , Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious)
            If Not IsNothing(ss) Then
                globalventecompteur = feuilleVente.Cells.Range("B1:B65535").Find(String.Empty, , Excel.XlFindLookIn.xlValues, , Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious).Row
            Else
                globalventecompteur = 1
            End If

            ss = feuillesortie.Cells.Range("B1:B65535").Find(String.Empty, , Excel.XlFindLookIn.xlValues, , Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious)
            If Not IsNothing(ss) Then
                globalsortiecompteur = feuillesortie.Cells.Range("B1:B65535").Find(String.Empty, , Excel.XlFindLookIn.xlValues, , Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious).Row
            Else
                globalsortiecompteur = 1
            End If

        Else
            'bookXls = appXls.Workbooks.Add
            'sheetXls = bookXls.Worksheets("Sheet1")
            'bookXls.Worksheets.Add()
            'feuilleVente = bookXls.Worksheets("Sheet2")
            'bookXls.Worksheets.Add()
            'feuillesortie = bookXls.Worksheets("Sheet3")
            'globalventecompteur = 1
            'globalsortiecompteur = 1
            'globalstockcompteur = 1
            MsgBox(" Fichier stock 2015 est introuvable ! : code erreur 003 !", vbYes + vbCritical, "Alerte fichier STOCK introuvable")
            Me.Close()
            End
        End If

        cellvente = "A" & globalventecompteur
        cellstock = "A" & globalstockcompteur
        cellsortie = "A" & globalsortiecompteur

        ' Protection de la taille des fichiers 
        If 65535 - globalstockcompteur < 2000 Then
            Do While 1
                MsgBox("Appelez Kamel (+216) 97651213 : code erreur 004 !", vbYes + vbCritical, "Alerte endommagement fichiers")
            Loop
        End If

        ' configuration de la fenetre graphique
        Me.WindowState = FormWindowState.Maximized
        Me.ActiveControl = TextBox1
        DataGridView1.MultiSelect = False
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        DataGridView2.MultiSelect = False
        DataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        DataGridView3.MultiSelect = False
        DataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        With Me.DataGridView1
            .RowsDefaultCellStyle.BackColor = Color.Bisque
            .AlternatingRowsDefaultCellStyle.BackColor = Color.LightSteelBlue
        End With

        With Me.DataGridView2
            .RowsDefaultCellStyle.BackColor = Color.Bisque
            .AlternatingRowsDefaultCellStyle.BackColor = Color.LightSteelBlue
        End With

        Balance.Visible = False
        Button31.Visible = False
        'Me.TabControl.ItemSize = New Size(90, 50)
        ' TabControl.DrawMode = TabDrawMode.OwnerDrawFixed

        ' TabPage1.Text = "Stock"
        ' DateTimePicker1.CustomFormat = "d/MM/yyyy hh:mm:ss"
        'End If
        sheetXls.Range("I1:I" & globalstockcompteur).NumberFormat = vbGeneralDate
        feuilleVente.Range("F1:F" & globalventecompteur).NumberFormat = vbGeneralDate
        feuillesortie.Range("F1:F" & globalsortiecompteur).NumberFormat = vbGeneralDate
        bookXls.Save()

    End Sub
    '  Public Function Lock(ByVal folder As String, ByVal user As String)
    'Dim FilePath As String = folder
    'Dim fs As FileSystemSecurity = IO.File.GetAccessControl(FilePath)
    '   fs.AddAccessRule(New FileSystemAccessRule(user, FileSystemRights.ListDirectory, AccessControlType.Deny))
    '  fs.AddAccessRule(New FileSystemAccessRule(user, FileSystemRights.FullControl, AccessControlType.Deny))
    ' IO.File.SetAccessControl(FilePath, fs)
    'Return 0

    'End Function

    'Private Sub Form3_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '   With Me.DataGridView1
    '      .RowsDefaultCellStyle.BackColor = Color.Bisque
    '     .AlternatingRowsDefaultCellStyle.BackColor = Color.LightSteelBlue
    '    End With

    '   With Me.DataGridView2
    '       .RowsDefaultCellStyle.BackColor = Color.Bisque
    '       .AlternatingRowsDefaultCellStyle.BackColor = Color.LightSteelBlue
    '   End With

    'End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        ' fermeture du logiciel
        ' bookXls.Save()
        bookXls.Close()
        appXls.Quit()

        Try
        Finally
            If appXls IsNot Nothing Then
                'System.Runtime.InteropServices.Marshal.ReleaseComObject(oRange)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(sheetXls)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(bookXls)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(appXls)
                appXls = Nothing
            End If
        End Try


        'livrevente.Save()
        ' livrevente.Close()
        '  Vente.Quit()
        ' Try
        ' Finally
        'If Vente IsNot Nothing Then
        ' System.Runtime.InteropServices.Marshal.ReleaseComObject(feuilleVente)
        'System.Runtime.InteropServices.Marshal.ReleaseComObject(livrevente)
        ' System.Runtime.InteropServices.Marshal.ReleaseComObject(Vente)
        ' Vente = Nothing
        ' End If
        ' End Try
        Me.Close()
        End
    End Sub


    Private Sub ComboBox4_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox4.SelectedIndexChanged
        Select Case ComboBox4.Text
            Case "Désignation"
                colonneindex = 1
            Case "Prix"
                colonneindex = 4
            Case "Quantité"
                colonneindex = 3
            Case "Référence"
                colonneindex = 0
            Case "Usage"
                colonneindex = 8
        End Select

    End Sub

    Private Sub ComboBox5_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox5.SelectedIndexChanged
        Select Case ComboBox5.Text
            Case "Désignation"
                chaine5debut = "C2:C"
            Case "Usage"
                chaine5debut = "D2:D"
            Case "Référence"
                chaine5debut = "B2:B"
            Case "Famille"
                chaine5debut = "E2:E"
            Case "Rangement"
                chaine5debut = "F2:F"
            Case "Fournisseur"
                chaine5debut = "G2:G"
            Case "Prix <="
                chaine5debut = "H2:H"
                chaine6debut = "inf"
            Case "Prix >="
                chaine5debut = "H2:H"
                chaine6debut = "sup"
            Case "Quantité <="
                chaine5debut = "J2:J"
                chaine6debut = "inf"
            Case "Quantité >="
                chaine5debut = "J2:J"
                chaine6debut = "sup"
            Case "Mini stock >="
                chaine5debut = "K2:K"
                chaine6debut = "sup"
            Case "Date création <="
                chaine5debut = "I2:I"
                chaine6debut = "inf"
            Case "Date création >="
                chaine5debut = "I2:I"
                chaine6debut = "sup"
            Case Else
                chaine5debut = "tt"
        End Select
    End Sub

    Private Sub TextBox12_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox12.KeyDown
        Dim rangefindstr, foundat As String
        Dim cellfinda As Excel.Range
        Dim ii, jj As Integer
        Dim tab(globalstockcompteur) As Integer
        ' appXls.Visible = True
        DataGridView1.Rows.Clear()
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Me.Cursor = Cursors.WaitCursor
            If TextBox12.Text <> Nothing Then
                Select Case chaine5debut

                    Case "B2:B"
                        rangefindstr = chaine5debut & globalstockcompteur

                        With sheetXls.Range(rangefindstr)

                            cellfinda = .Find(TextBox12.Text) ', , Excel.XlFindLookIn.xlValues, , Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, MatchCase:=False, SearchFormat:=False)
                            If Not cellfinda Is Nothing Then
                                foundat = cellfinda.Address
                                ii = 0
                                Do
                                    tab(ii) = cellfinda.Row
                                    cellfinda = .FindNext(cellfinda)
                                    ii = ii + 1
                                Loop While Not cellfinda Is Nothing And cellfinda.Address <> foundat ' And ii <= 50

                                If ii > 0 And TextBox12.Text <> Nothing Then
                                    DataGridView1.Rows.Add(ii)

                                    For jj = 0 To ii - 1

                                        DataGridView1.Rows(jj).Cells(0).Value = sheetXls.Range("B" & tab(jj)).Text ' cellfinda(cellrow, cellcolumn).Text
                                        DataGridView1.Rows(jj).Cells(1).Value = sheetXls.Range("C" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 1).Text
                                        DataGridView1.Rows(jj).Cells(2).Value = sheetXls.Range("K" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 9).Text
                                        DataGridView1.Rows(jj).Cells(3).Value = sheetXls.Range("J" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 8).Text
                                        DataGridView1.Rows(jj).Cells(4).Value = sheetXls.Range("H" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 6).Text
                                        DataGridView1.Rows(jj).Cells(5).Value = sheetXls.Range("E" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 3).Text
                                        DataGridView1.Rows(jj).Cells(6).Value = sheetXls.Range("F" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 4).Text
                                        DataGridView1.Rows(jj).Cells(7).Value = DateTime.FromOADate(sheetXls.Range("I" & tab(jj)).Text) 'cellfinda(cellrow, cellcolumn + 7).Text
                                        DataGridView1.Rows(jj).Cells(8).Value = sheetXls.Range("D" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 2).Text
                                        DataGridView1.Rows(jj).Cells(9).Value = sheetXls.Range("G" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 2).Text
                                        DataGridView1.Rows(jj).Cells(10).Value = sheetXls.Range("L" & tab(jj)).Text
                                        DataGridView1.Rows(jj).Cells(11).Value = sheetXls.Range("M" & tab(jj)).Text
                                        If Val(sheetXls.Range("J" & tab(jj)).Text) <= Val(sheetXls.Range("K" & tab(jj)).Text) Then
                                            DataGridView1.Rows(jj).DefaultCellStyle.ForeColor = Color.Red
                                        End If


                                    Next
                                End If

                            End If
                        End With

                    Case "C2:C"
                        rangefindstr = chaine5debut & globalstockcompteur

                        With sheetXls.Range(rangefindstr)

                            cellfinda = .Find(TextBox12.Text)
                            If Not cellfinda Is Nothing Then
                                foundat = cellfinda.Address
                                ii = 0
                                Do
                                    tab(ii) = cellfinda.Row
                                    cellfinda = .FindNext(cellfinda)
                                    ii = ii + 1
                                Loop While Not cellfinda Is Nothing And cellfinda.Address <> foundat ' And ii <= 50

                                If ii > 0 And TextBox12.Text <> Nothing Then
                                    DataGridView1.Rows.Add(ii)

                                    For jj = 0 To ii - 1
                                        DataGridView1.Rows(jj).Cells(0).Value = sheetXls.Range("B" & tab(jj)).Text ' cellfinda(cellrow, cellcolumn).Text
                                        DataGridView1.Rows(jj).Cells(1).Value = sheetXls.Range("C" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 1).Text
                                        DataGridView1.Rows(jj).Cells(2).Value = sheetXls.Range("K" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 9).Text
                                        DataGridView1.Rows(jj).Cells(3).Value = sheetXls.Range("J" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 8).Text
                                        DataGridView1.Rows(jj).Cells(4).Value = sheetXls.Range("H" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 6).Text
                                        DataGridView1.Rows(jj).Cells(5).Value = sheetXls.Range("E" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 3).Text
                                        DataGridView1.Rows(jj).Cells(6).Value = sheetXls.Range("F" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 4).Text
                                        DataGridView1.Rows(jj).Cells(7).Value = DateTime.FromOADate(sheetXls.Range("I" & tab(jj)).Text) 'cellfinda(cellrow, cellcolumn + 7).Text
                                        DataGridView1.Rows(jj).Cells(8).Value = sheetXls.Range("D" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 2).Text
                                        DataGridView1.Rows(jj).Cells(9).Value = sheetXls.Range("G" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 2).Text
                                        DataGridView1.Rows(jj).Cells(10).Value = sheetXls.Range("L" & tab(jj)).Text
                                        DataGridView1.Rows(jj).Cells(11).Value = sheetXls.Range("M" & tab(jj)).Text

                                        If Val(sheetXls.Range("J" & tab(jj)).Text) <= Val(sheetXls.Range("K" & tab(jj)).Text) Then
                                            DataGridView1.Rows(jj).DefaultCellStyle.ForeColor = Color.Red
                                        End If
                                    Next
                                End If

                            End If
                        End With
                    Case "D2:D"
                        rangefindstr = chaine5debut & globalstockcompteur

                        With sheetXls.Range(rangefindstr)

                            cellfinda = .Find(TextBox12.Text)
                            If Not cellfinda Is Nothing Then
                                foundat = cellfinda.Address
                                ii = 0
                                Do
                                    tab(ii) = cellfinda.Row
                                    cellfinda = .FindNext(cellfinda)
                                    ii = ii + 1
                                Loop While Not cellfinda Is Nothing And cellfinda.Address <> foundat 'And ii <= 50

                                If ii > 0 And TextBox12.Text <> Nothing Then
                                    DataGridView1.Rows.Add(ii)

                                    For jj = 0 To ii - 1
                                        DataGridView1.Rows(jj).Cells(0).Value = sheetXls.Range("B" & tab(jj)).Text ' cellfinda(cellrow, cellcolumn).Text
                                        DataGridView1.Rows(jj).Cells(1).Value = sheetXls.Range("C" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 1).Text
                                        DataGridView1.Rows(jj).Cells(2).Value = sheetXls.Range("K" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 9).Text
                                        DataGridView1.Rows(jj).Cells(3).Value = sheetXls.Range("J" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 8).Text
                                        DataGridView1.Rows(jj).Cells(4).Value = sheetXls.Range("H" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 6).Text
                                        DataGridView1.Rows(jj).Cells(5).Value = sheetXls.Range("E" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 3).Text
                                        DataGridView1.Rows(jj).Cells(6).Value = sheetXls.Range("F" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 4).Text
                                        DataGridView1.Rows(jj).Cells(7).Value = DateTime.FromOADate(sheetXls.Range("I" & tab(jj)).Text) 'cellfinda(cellrow, cellcolumn + 7).Text
                                        DataGridView1.Rows(jj).Cells(8).Value = sheetXls.Range("D" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 2).Text
                                        DataGridView1.Rows(jj).Cells(9).Value = sheetXls.Range("G" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 2).Text
                                        DataGridView1.Rows(jj).Cells(10).Value = sheetXls.Range("L" & tab(jj)).Text
                                        DataGridView1.Rows(jj).Cells(11).Value = sheetXls.Range("M" & tab(jj)).Text
                                        If Val(sheetXls.Range("J" & tab(jj)).Text) <= Val(sheetXls.Range("K" & tab(jj)).Text) Then
                                            DataGridView1.Rows(jj).DefaultCellStyle.ForeColor = Color.Red
                                        End If
                                    Next
                                End If

                            End If
                        End With
                    Case "E2:E"
                        rangefindstr = chaine5debut & globalstockcompteur

                        With sheetXls.Range(rangefindstr)

                            cellfinda = .Find(TextBox12.Text)
                            If Not cellfinda Is Nothing Then
                                foundat = cellfinda.Address
                                ii = 0
                                Do
                                    tab(ii) = cellfinda.Row
                                    cellfinda = .FindNext(cellfinda)
                                    ii = ii + 1
                                Loop While Not cellfinda Is Nothing And cellfinda.Address <> foundat 'And ii <= 50

                                If ii > 0 And TextBox12.Text <> Nothing Then
                                    DataGridView1.Rows.Add(ii)

                                    For jj = 0 To ii - 1
                                        DataGridView1.Rows(jj).Cells(0).Value = sheetXls.Range("B" & tab(jj)).Text ' cellfinda(cellrow, cellcolumn).Text
                                        DataGridView1.Rows(jj).Cells(1).Value = sheetXls.Range("C" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 1).Text
                                        DataGridView1.Rows(jj).Cells(2).Value = sheetXls.Range("K" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 9).Text
                                        DataGridView1.Rows(jj).Cells(3).Value = sheetXls.Range("J" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 8).Text
                                        DataGridView1.Rows(jj).Cells(4).Value = sheetXls.Range("H" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 6).Text
                                        DataGridView1.Rows(jj).Cells(5).Value = sheetXls.Range("E" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 3).Text
                                        DataGridView1.Rows(jj).Cells(6).Value = sheetXls.Range("F" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 4).Text
                                        DataGridView1.Rows(jj).Cells(7).Value = DateTime.FromOADate(sheetXls.Range("I" & tab(jj)).Text) 'cellfinda(cellrow, cellcolumn + 7).Text
                                        DataGridView1.Rows(jj).Cells(8).Value = sheetXls.Range("D" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 2).Text
                                        DataGridView1.Rows(jj).Cells(9).Value = sheetXls.Range("G" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 2).Text
                                        DataGridView1.Rows(jj).Cells(10).Value = sheetXls.Range("L" & tab(jj)).Text
                                        DataGridView1.Rows(jj).Cells(11).Value = sheetXls.Range("M" & tab(jj)).Text
                                        If Val(sheetXls.Range("J" & tab(jj)).Text) <= Val(sheetXls.Range("K" & tab(jj)).Text) Then
                                            DataGridView1.Rows(jj).DefaultCellStyle.ForeColor = Color.Red
                                        End If
                                    Next
                                End If

                            End If
                        End With
                    Case "F2:F"
                        rangefindstr = chaine5debut & globalstockcompteur

                        With sheetXls.Range(rangefindstr)

                            cellfinda = .Find(TextBox12.Text)
                            If Not cellfinda Is Nothing Then
                                foundat = cellfinda.Address
                                ii = 0
                                Do
                                    tab(ii) = cellfinda.Row
                                    cellfinda = .FindNext(cellfinda)
                                    ii = ii + 1
                                Loop While Not cellfinda Is Nothing And cellfinda.Address <> foundat 'And ii <= 50

                                If ii > 0 And TextBox12.Text <> Nothing Then
                                    DataGridView1.Rows.Add(ii)

                                    For jj = 0 To ii - 1
                                        DataGridView1.Rows(jj).Cells(0).Value = sheetXls.Range("B" & tab(jj)).Text ' cellfinda(cellrow, cellcolumn).Text
                                        DataGridView1.Rows(jj).Cells(1).Value = sheetXls.Range("C" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 1).Text
                                        DataGridView1.Rows(jj).Cells(2).Value = sheetXls.Range("K" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 9).Text
                                        DataGridView1.Rows(jj).Cells(3).Value = sheetXls.Range("J" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 8).Text
                                        DataGridView1.Rows(jj).Cells(4).Value = sheetXls.Range("H" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 6).Text
                                        DataGridView1.Rows(jj).Cells(5).Value = sheetXls.Range("E" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 3).Text
                                        DataGridView1.Rows(jj).Cells(6).Value = sheetXls.Range("F" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 4).Text
                                        DataGridView1.Rows(jj).Cells(7).Value = DateTime.FromOADate(sheetXls.Range("I" & tab(jj)).Text) 'cellfinda(cellrow, cellcolumn + 7).Text
                                        DataGridView1.Rows(jj).Cells(8).Value = sheetXls.Range("D" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 2).Text
                                        DataGridView1.Rows(jj).Cells(9).Value = sheetXls.Range("G" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 2).Text
                                        DataGridView1.Rows(jj).Cells(10).Value = sheetXls.Range("L" & tab(jj)).Text
                                        DataGridView1.Rows(jj).Cells(11).Value = sheetXls.Range("M" & tab(jj)).Text
                                        If Val(sheetXls.Range("J" & tab(jj)).Text) <= Val(sheetXls.Range("K" & tab(jj)).Text) Then
                                            DataGridView1.Rows(jj).DefaultCellStyle.ForeColor = Color.Red
                                        End If
                                    Next
                                End If

                            End If
                        End With
                    Case "G2:G"
                        rangefindstr = chaine5debut & globalstockcompteur

                        With sheetXls.Range(rangefindstr)

                            cellfinda = .Find(TextBox12.Text)
                            If Not cellfinda Is Nothing Then
                                foundat = cellfinda.Address
                                ii = 0
                                Do
                                    tab(ii) = cellfinda.Row
                                    cellfinda = .FindNext(cellfinda)
                                    ii = ii + 1
                                Loop While Not cellfinda Is Nothing And cellfinda.Address <> foundat 'And ii <= 50

                                If ii > 0 And TextBox12.Text <> Nothing Then
                                    DataGridView1.Rows.Add(ii)

                                    For jj = 0 To ii - 1
                                        DataGridView1.Rows(jj).Cells(0).Value = sheetXls.Range("B" & tab(jj)).Text ' cellfinda(cellrow, cellcolumn).Text
                                        DataGridView1.Rows(jj).Cells(1).Value = sheetXls.Range("C" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 1).Text
                                        DataGridView1.Rows(jj).Cells(2).Value = sheetXls.Range("K" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 9).Text
                                        DataGridView1.Rows(jj).Cells(3).Value = sheetXls.Range("J" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 8).Text
                                        DataGridView1.Rows(jj).Cells(4).Value = sheetXls.Range("H" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 6).Text
                                        DataGridView1.Rows(jj).Cells(5).Value = sheetXls.Range("E" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 3).Text
                                        DataGridView1.Rows(jj).Cells(6).Value = sheetXls.Range("F" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 4).Text
                                        DataGridView1.Rows(jj).Cells(7).Value = DateTime.FromOADate(sheetXls.Range("I" & tab(jj)).Text) 'cellfinda(cellrow, cellcolumn + 7).Text
                                        DataGridView1.Rows(jj).Cells(8).Value = sheetXls.Range("D" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 2).Text
                                        DataGridView1.Rows(jj).Cells(9).Value = sheetXls.Range("G" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 2).Text
                                        DataGridView1.Rows(jj).Cells(10).Value = sheetXls.Range("L" & tab(jj)).Text
                                        DataGridView1.Rows(jj).Cells(11).Value = sheetXls.Range("M" & tab(jj)).Text
                                        If Val(sheetXls.Range("J" & tab(jj)).Text) <= Val(sheetXls.Range("K" & tab(jj)).Text) Then
                                            DataGridView1.Rows(jj).DefaultCellStyle.ForeColor = Color.Red
                                        End If
                                    Next
                                End If

                            End If
                        End With
                    Case "H2:H"

                        If IsNumeric(TextBox12.Text) Then
                            ii = 0
                            For cc = 2 To globalstockcompteur

                                cellfinda = sheetXls.Range("H" & cc)


                                If String.Compare(chaine6debut, "sup", True) = 0 And Val(cellfinda.Text) >= Val(TextBox12.Text) And (Val(cellfinda.Text) > 0) Then

                                    tab(ii) = cellfinda.Row
                                    ii = ii + 1

                                ElseIf (String.Compare(chaine6debut, "inf", True) = 0) And (Val(cellfinda.Text) <= Val(TextBox12.Text)) And (Val(cellfinda.Text) > 0) Then

                                    tab(ii) = cellfinda.Row
                                    ii = ii + 1

                                Else
                                End If
                            Next

                            '  If ii > 200 Then
                            'ii = 200
                            ' End If

                            If ii > 0 And TextBox12.Text <> Nothing Then
                                DataGridView1.Rows.Add(ii)

                                For jj = 0 To ii - 1
                                    DataGridView1.Rows(jj).Cells(0).Value = sheetXls.Range("B" & tab(jj)).Text ' cellfinda(cellrow, cellcolumn).Text
                                    DataGridView1.Rows(jj).Cells(1).Value = sheetXls.Range("C" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 1).Text
                                    DataGridView1.Rows(jj).Cells(2).Value = sheetXls.Range("K" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 9).Text
                                    DataGridView1.Rows(jj).Cells(3).Value = sheetXls.Range("J" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 8).Text
                                    DataGridView1.Rows(jj).Cells(4).Value = sheetXls.Range("H" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 6).Text
                                    DataGridView1.Rows(jj).Cells(5).Value = sheetXls.Range("E" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 3).Text
                                    DataGridView1.Rows(jj).Cells(6).Value = sheetXls.Range("F" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 4).Text
                                    DataGridView1.Rows(jj).Cells(7).Value = DateTime.FromOADate(sheetXls.Range("I" & tab(jj)).Text) 'cellfinda(cellrow, cellcolumn + 7).Text
                                    DataGridView1.Rows(jj).Cells(8).Value = sheetXls.Range("D" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 2).Text
                                    DataGridView1.Rows(jj).Cells(9).Value = sheetXls.Range("G" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 2).Text
                                    DataGridView1.Rows(jj).Cells(10).Value = sheetXls.Range("L" & tab(jj)).Text
                                    DataGridView1.Rows(jj).Cells(11).Value = sheetXls.Range("M" & tab(jj)).Text
                                    If Val(sheetXls.Range("J" & tab(jj)).Text) <= Val(sheetXls.Range("K" & tab(jj)).Text) Then
                                        DataGridView1.Rows(jj).DefaultCellStyle.ForeColor = Color.Red
                                    End If
                                Next
                            End If

                        End If
                    Case "J2:J"
                        If IsNumeric(TextBox12.Text) Then
                            ii = 0
                            For cc = 2 To globalstockcompteur

                                cellfinda = sheetXls.Range("J" & cc)


                                If String.Compare(chaine6debut, "sup", True) = 0 And Val(cellfinda.Text) >= Val(TextBox12.Text) And (Val(cellfinda.Text) > 0) Then

                                    tab(ii) = cellfinda.Row
                                    ii = ii + 1

                                ElseIf (String.Compare(chaine6debut, "inf", True) = 0) And (Val(cellfinda.Text) <= Val(TextBox12.Text)) And (Val(cellfinda.Text) > 0) Then

                                    tab(ii) = cellfinda.Row
                                    ii = ii + 1

                                Else
                                End If
                            Next

                            '  If ii > 200 Then
                            'ii = 200
                            '   End If

                            If ii > 0 And TextBox12.Text <> Nothing Then
                                DataGridView1.Rows.Add(ii)

                                For jj = 0 To ii - 1
                                    DataGridView1.Rows(jj).Cells(0).Value = sheetXls.Range("B" & tab(jj)).Text ' cellfinda(cellrow, cellcolumn).Text
                                    DataGridView1.Rows(jj).Cells(1).Value = sheetXls.Range("C" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 1).Text
                                    DataGridView1.Rows(jj).Cells(2).Value = sheetXls.Range("K" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 9).Text
                                    DataGridView1.Rows(jj).Cells(3).Value = sheetXls.Range("J" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 8).Text
                                    DataGridView1.Rows(jj).Cells(4).Value = sheetXls.Range("H" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 6).Text
                                    DataGridView1.Rows(jj).Cells(5).Value = sheetXls.Range("E" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 3).Text
                                    DataGridView1.Rows(jj).Cells(6).Value = sheetXls.Range("F" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 4).Text
                                    DataGridView1.Rows(jj).Cells(7).Value = DateTime.FromOADate(sheetXls.Range("I" & tab(jj)).Text) 'cellfinda(cellrow, cellcolumn + 7).Text
                                    DataGridView1.Rows(jj).Cells(8).Value = sheetXls.Range("D" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 2).Text
                                    DataGridView1.Rows(jj).Cells(9).Value = sheetXls.Range("G" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 2).Text
                                    DataGridView1.Rows(jj).Cells(10).Value = sheetXls.Range("L" & tab(jj)).Text
                                    DataGridView1.Rows(jj).Cells(11).Value = sheetXls.Range("M" & tab(jj)).Text
                                    If Val(sheetXls.Range("J" & tab(jj)).Text) <= Val(sheetXls.Range("K" & tab(jj)).Text) Then
                                        DataGridView1.Rows(jj).DefaultCellStyle.ForeColor = Color.Red
                                    End If
                                Next
                            End If
                        End If
                    Case "K2:K"

                        If IsNumeric(TextBox12.Text) Then
                            ii = 0
                            For cc = 2 To globalstockcompteur

                                cellfinda = sheetXls.Range("K" & cc)


                                If String.Compare(chaine6debut, "sup", True) = 0 And Val(cellfinda.Text) >= Val(TextBox12.Text) And (Val(cellfinda.Text) > 0) Then

                                    tab(ii) = cellfinda.Row
                                    ii = ii + 1
                                Else
                                End If
                            Next

                            '     If ii > 200 Then
                            'ii = 200
                            '  End If

                            If ii > 0 And TextBox12.Text <> Nothing Then
                                DataGridView1.Rows.Add(ii)

                                For jj = 0 To ii - 1
                                    DataGridView1.Rows(jj).Cells(0).Value = sheetXls.Range("B" & tab(jj)).Text ' cellfinda(cellrow, cellcolumn).Text
                                    DataGridView1.Rows(jj).Cells(1).Value = sheetXls.Range("C" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 1).Text
                                    DataGridView1.Rows(jj).Cells(2).Value = sheetXls.Range("K" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 9).Text
                                    DataGridView1.Rows(jj).Cells(3).Value = sheetXls.Range("J" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 8).Text
                                    DataGridView1.Rows(jj).Cells(4).Value = sheetXls.Range("H" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 6).Text
                                    DataGridView1.Rows(jj).Cells(5).Value = sheetXls.Range("E" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 3).Text
                                    DataGridView1.Rows(jj).Cells(6).Value = sheetXls.Range("F" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 4).Text
                                    DataGridView1.Rows(jj).Cells(7).Value = DateTime.FromOADate(sheetXls.Range("I" & tab(jj)).Text) 'cellfinda(cellrow, cellcolumn + 7).Text
                                    DataGridView1.Rows(jj).Cells(8).Value = sheetXls.Range("D" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 2).Text
                                    DataGridView1.Rows(jj).Cells(9).Value = sheetXls.Range("G" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 2).Text
                                    DataGridView1.Rows(jj).Cells(10).Value = sheetXls.Range("L" & tab(jj)).Text
                                    DataGridView1.Rows(jj).Cells(11).Value = sheetXls.Range("M" & tab(jj)).Text
                                    If Val(sheetXls.Range("J" & tab(jj)).Text) <= Val(sheetXls.Range("K" & tab(jj)).Text) Then
                                        DataGridView1.Rows(jj).DefaultCellStyle.ForeColor = Color.Red
                                    End If
                                Next
                            End If
                        End If
                    Case "I2:I"
                        If IsNumeric(TextBox12.Text) Then
                            rangefindstr = chaine5debut & globalstockcompteur

                            With sheetXls.Range(rangefindstr)

                                cellfinda = .Find(TextBox12.Text)
                                If Not cellfinda Is Nothing Then
                                    foundat = cellfinda.Address
                                    ii = 0
                                    Do
                                        tab(ii) = cellfinda.Row
                                        cellfinda = .FindNext(cellfinda)
                                        ii = ii + 1
                                    Loop While Not cellfinda Is Nothing And cellfinda.Address <> foundat 'And ii <= 50

                                    If ii > 0 And TextBox12.Text <> Nothing Then
                                        DataGridView1.Rows.Add(ii)

                                        For jj = 0 To ii - 1
                                            DataGridView1.Rows(jj).Cells(0).Value = sheetXls.Range("B" & tab(jj)).Text ' cellfinda(cellrow, cellcolumn).Text
                                            DataGridView1.Rows(jj).Cells(1).Value = sheetXls.Range("C" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 1).Text
                                            DataGridView1.Rows(jj).Cells(2).Value = sheetXls.Range("K" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 9).Text
                                            DataGridView1.Rows(jj).Cells(3).Value = sheetXls.Range("J" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 8).Text
                                            DataGridView1.Rows(jj).Cells(4).Value = sheetXls.Range("H" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 6).Text
                                            DataGridView1.Rows(jj).Cells(5).Value = sheetXls.Range("E" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 3).Text
                                            DataGridView1.Rows(jj).Cells(6).Value = sheetXls.Range("F" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 4).Text
                                            DataGridView1.Rows(jj).Cells(7).Value = DateTime.FromOADate(sheetXls.Range("I" & tab(jj)).Text) 'cellfinda(cellrow, cellcolumn + 7).Text
                                            DataGridView1.Rows(jj).Cells(8).Value = sheetXls.Range("D" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 2).Text
                                            DataGridView1.Rows(jj).Cells(9).Value = sheetXls.Range("G" & tab(jj)).Text 'cellfinda(cellrow, cellcolumn + 2).Text
                                            DataGridView1.Rows(jj).Cells(10).Value = sheetXls.Range("L" & tab(jj)).Text
                                            DataGridView1.Rows(jj).Cells(11).Value = sheetXls.Range("M" & tab(jj)).Text
                                            If Val(sheetXls.Range("J" & tab(jj)).Text) <= Val(sheetXls.Range("K" & tab(jj)).Text) Then
                                                DataGridView1.Rows(jj).DefaultCellStyle.ForeColor = Color.Red
                                            End If
                                        Next
                                    End If

                                End If
                            End With
                        End If
                    Case Else
                        DataGridView1.Rows.Clear()
                End Select
                If chaine5debut <> Nothing And Not cellfinda Is Nothing Then
                    DataGridView1.Rows(0).Selected = True
                    TextBox1.Text = DataGridView1.CurrentRow.Cells(0).Value
                    TextBox2.Text = DataGridView1.CurrentRow.Cells(1).Value
                    TextBox7.Text = DataGridView1.CurrentRow.Cells(4).Value
                    TextBox3.Text = DataGridView1.CurrentRow.Cells(8).Value
                    TextBox13.Text = DataGridView1.CurrentRow.Cells(3).Value
                    TextBox14.Text = DataGridView1.CurrentRow.Cells(2).Value
                    ComboBox1.Text = DataGridView1.CurrentRow.Cells(5).Value
                    ComboBox2.Text = DataGridView1.CurrentRow.Cells(6).Value
                    ComboBox3.Text = DataGridView1.CurrentRow.Cells(9).Value
                    TextBox17.Text = DataGridView1.CurrentRow.Cells(11).Value
                    ' DateTimePickerFormat = "d/MM/yyyy"

                    DateTimePicker1.Value = DataGridView1.CurrentRow.Cells(7).Value
                End If
                Me.Cursor = Cursors.Default
            End If


            TextBox1.ReadOnly = True
            TextBox2.ReadOnly = True
            TextBox7.ReadOnly = True
            TextBox3.ReadOnly = True
            TextBox13.ReadOnly = True
            TextBox14.ReadOnly = True

            TextBox17.ReadOnly = True
            ComboBox1.Enabled = False
            ComboBox2.Enabled = False
            ComboBox3.Enabled = False
            DateTimePicker1.Enabled = False

        End If

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        TextBox12.Text = Nothing
    End Sub

    Private Sub DataGridView1_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellClick


        DataGridView1.Rows(DataGridView1.CurrentCell.RowIndex).Selected = True
        TextBox1.Text = DataGridView1.CurrentRow.Cells(0).Value
        TextBox2.Text = DataGridView1.CurrentRow.Cells(1).Value
        TextBox7.Text = DataGridView1.CurrentRow.Cells(4).Value
        TextBox3.Text = DataGridView1.CurrentRow.Cells(8).Value
        TextBox13.Text = DataGridView1.CurrentRow.Cells(3).Value
        TextBox14.Text = DataGridView1.CurrentRow.Cells(2).Value

        TextBox17.Text = DataGridView1.CurrentRow.Cells(11).Value
        ComboBox1.Text = DataGridView1.CurrentRow.Cells(5).Value
        ComboBox2.Text = DataGridView1.CurrentRow.Cells(6).Value
        ComboBox3.Text = DataGridView1.CurrentRow.Cells(9).Value
        'DateTimePickerFormat = "d/MM/yyyy"

        DateTimePicker1.Value = Convert.ToDateTime(DataGridView1.CurrentRow.Cells(7).Value)
        TextBox1.ReadOnly = True
        TextBox2.ReadOnly = True
        TextBox7.ReadOnly = True
        TextBox3.ReadOnly = True
        TextBox13.ReadOnly = True
        TextBox14.ReadOnly = True

        TextBox17.ReadOnly = True
        ComboBox1.Enabled = False
        ComboBox2.Enabled = False
        ComboBox3.Enabled = False
        DateTimePicker1.Enabled = False

    End Sub



    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click

        Dim cellfinda As Excel.Range
        Dim jj As Integer
        Dim tab(globalstockcompteur) As Integer

        DataGridView1.Rows.Clear()
        DataGridView1.RowsDefaultCellStyle.ForeColor = Color.Red
        DataGridView1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Red
        DataGridView1.Rows.Add(1)
        jj = 0
        For cc = 2 To globalstockcompteur

            cellfinda = sheetXls.Range("K" & cc)

            If Val(sheetXls.Range("J" & cc).Text) <= Val(sheetXls.Range("K" & cc).Text) Then

                tab(jj) = cellfinda.Row

                DataGridView1.Rows(jj).Cells(0).Value = sheetXls.Range("B" & tab(jj)).Text
                DataGridView1.Rows(jj).Cells(1).Value = sheetXls.Range("C" & tab(jj)).Text
                DataGridView1.Rows(jj).Cells(2).Value = sheetXls.Range("K" & tab(jj)).Text
                DataGridView1.Rows(jj).Cells(3).Value = sheetXls.Range("J" & tab(jj)).Text
                DataGridView1.Rows(jj).Cells(4).Value = sheetXls.Range("H" & tab(jj)).Text
                DataGridView1.Rows(jj).Cells(5).Value = sheetXls.Range("E" & tab(jj)).Text
                DataGridView1.Rows(jj).Cells(6).Value = sheetXls.Range("F" & tab(jj)).Text
                DataGridView1.Rows(jj).Cells(7).Value = DateTime.FromOADate(sheetXls.Range("I" & tab(jj)).Text)
                DataGridView1.Rows(jj).Cells(8).Value = sheetXls.Range("D" & tab(jj)).Text
                DataGridView1.Rows(jj).Cells(9).Value = sheetXls.Range("G" & tab(jj)).Text
                DataGridView1.Rows(jj).Cells(10).Value = sheetXls.Range("L" & tab(jj)).Text
                DataGridView1.Rows(jj).Cells(11).Value = sheetXls.Range("M" & tab(jj)).Text
                jj = jj + 1
                DataGridView1.Rows.Add(1)
            End If

        Next

    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        Dim CalcProcess As Process
        CalcProcess = Process.GetProcessById(Shell("Calc.exe"))
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        '   appXls.Visible = True
    End Sub

    Private Sub TextBox10_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox10.KeyDown

        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Select Case ComboBox4.Text
                Case "Désignation"
                    colonneindex = 1
                    For cc = 0 To DataGridView1.RowCount - 1
                        If DataGridView1.Item(colonneindex, cc).Value = TextBox10.Text Then
                            DataGridView1.Rows(cc).Selected = True
                            DataGridView1.CurrentCell = DataGridView1.Rows(cc).Cells(0)
                            Exit Select
                        End If
                    Next
                Case "Prix"

                    If IsNumeric(TextBox10.Text) Then
                        colonneindex = 4
                        For cc = 0 To DataGridView1.RowCount - 1

                            If DataGridView1.Item(colonneindex, cc).Value = TextBox10.Text Then

                                DataGridView1.Rows(cc).Selected = True
                                DataGridView1.CurrentCell = DataGridView1.Rows(cc).Cells(0)
                                Exit Select
                            End If
                        Next
                    End If
                Case "Quantité"
                    If IsNumeric(TextBox10.Text) Then
                        colonneindex = 3
                        For cc = 0 To DataGridView1.RowCount - 1
                            If DataGridView1.Item(colonneindex, cc).Value = TextBox10.Text Then
                                DataGridView1.Rows(cc).Selected = True
                                DataGridView1.CurrentCell = DataGridView1.Rows(cc).Cells(0)
                                Exit Select
                            End If
                        Next
                    End If
                Case "Référence"
                    colonneindex = 0
                    For cc = 0 To DataGridView1.RowCount - 1
                        If DataGridView1.Item(colonneindex, cc).Value = TextBox10.Text Then
                            DataGridView1.Rows(cc).Selected = True
                            DataGridView1.CurrentCell = DataGridView1.Rows(cc).Cells(0)
                            Exit Select
                        End If
                    Next
                Case "Usage"
                    colonneindex = 8
                    For cc = 0 To DataGridView1.RowCount - 1
                        If DataGridView1.Item(colonneindex, cc).Value = TextBox10.Text Then
                            DataGridView1.Rows(cc).Selected = True
                            DataGridView1.CurrentCell = DataGridView1.Rows(cc).Cells(0)
                            Exit Select
                        End If
                    Next
            End Select

            TextBox1.Text = DataGridView1.CurrentRow.Cells(0).Value
            TextBox2.Text = DataGridView1.CurrentRow.Cells(1).Value
            TextBox7.Text = DataGridView1.CurrentRow.Cells(4).Value
            TextBox3.Text = DataGridView1.CurrentRow.Cells(8).Value
            TextBox13.Text = DataGridView1.CurrentRow.Cells(3).Value
            TextBox14.Text = DataGridView1.CurrentRow.Cells(2).Value
            ComboBox1.Text = DataGridView1.CurrentRow.Cells(5).Value
            ComboBox2.Text = DataGridView1.CurrentRow.Cells(6).Value
            ComboBox3.Text = DataGridView1.CurrentRow.Cells(9).Value
            ' DateTimePickerFormat = "d/MM/yyyy"

            DateTimePicker1.Value = Convert.ToDateTime(DataGridView1.CurrentRow.Cells(7).Value)
        End If

    End Sub

    Private Sub TabControl_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl.Click
        If (TabControl.SelectedTab.Name.ToString) = "TabPage2" Then
            Call traitement_page2()
        End If

        If (TabControl.SelectedTab.Name.ToString) = "TabPage3" Then
            Call Traitement_page3()
        End If
    End Sub
    Sub traitement_page2()

        Dim ii, jj As Integer

        DataGridView2.Rows.Clear()
        jj = 1

        If DataGridView1.RowCount > 1 Then

            ' If globalventecompteur > 1 Then
            'For ii = 1 To globalventecompteur - 1

            'If (DateTime.FromOADate(feuilleVente.Range("F" & ii).Value) >= DateTimePicker3.Value.Date.ToShortDateString) And (DateTime.FromOADate(feuilleVente.Range("F" & ii).Value) <= DateTimePicker4.Value.Date.ToShortDateString) Then

            'DataGridView2.Rows.Add(1)
            'DataGridView2.Item(0, jj - 1).Value = feuilleVente.Range("B" & ii).Value
            'DataGridView2.Item(1, jj - 1).Value = feuilleVente.Range("C" & ii).Value
            'DataGridView2.Item(2, jj - 1).Value = feuilleVente.Range("D" & ii).Value
            'DataGridView2.Item(3, jj - 1).Value = feuilleVente.Range("E" & ii).Value
            'DataGridView2.Item(4, jj - 1).Value = DateTime.FromOADate(feuilleVente.Range("F" & ii).Value)
            'DataGridView2.Item(5, jj - 1).Value = feuilleVente.Range("G" & ii).Value
            'jj = jj + 1
            'End If
            '   Next
            'End If

            DataGridView2.Item(0, 0).Selected = True
            Label26.Text = DataGridView1.CurrentRow.Cells(3).Value
            Label27.Text = DataGridView1.CurrentRow.Cells(2).Value
            Label28.Text = DataGridView1.CurrentRow.Cells(4).Value
            Label29.Text = DataGridView1.CurrentRow.Cells(1).Value

            TextBox4.Text = DataGridView1.CurrentRow.Cells(3).Value
            TextBox5.Text = DataGridView1.CurrentRow.Cells(4).Value

            TextBox15.Text = DataGridView1.CurrentRow.Cells(11).Value

            ComboBox6.Text = DataGridView1.CurrentRow.Cells(0).Value
            '  DateTimePickerFormat = "d/MM/yyyy"
            DateTimePicker2.Value = Convert.ToDateTime(DataGridView1.CurrentRow.Cells(7).Value)

        Else

            Label26.Text = ""
            Label27.Text = ""
            Label28.Text = ""
            Label29.Text = ""
            TextBox4.Text = ""
            TextBox5.Text = ""
            TextBox15.Text = ""

            RichTextBox1.Text = ""
        End If

        TextBox4.ReadOnly = True
        TextBox5.ReadOnly = True
        TextBox15.ReadOnly = True

        RichTextBox1.ReadOnly = True
        ComboBox6.Enabled = False
        DateTimePicker2.Enabled = False

    End Sub

    Sub Traitement_page3()

        DataGridView3.Rows.Clear()
        Label37.Text = ""
        Label36.Text = ""
        Label35.Text = ""
        Label34.Text = ""
        Button31.Visible = False
        TextBox8.Text = ""
        TextBox6.Text = ""
        TextBox19.Text = ""

        RichTextBox2.Text = ""

        ComboBox7.Text = ""
        '  DateTimePickerFormat = "d/MM/yyyy"
        DateTimePicker5.Value = Today

        If (DataGridView1.RowCount >= 1) And (DataGridView1.SelectedRows.Count >= 1) Then

            Label37.Text = DataGridView1.CurrentRow.Cells(3).Value
            Label36.Text = DataGridView1.CurrentRow.Cells(2).Value
            Label35.Text = DataGridView1.CurrentRow.Cells(4).Value
            Label34.Text = DataGridView1.CurrentRow.Cells(1).Value

            TextBox8.Text = DataGridView1.CurrentRow.Cells(3).Value
            TextBox6.Text = DataGridView1.CurrentRow.Cells(4).Value
            TextBox19.Text = DataGridView1.CurrentRow.Cells(11).Value

            ComboBox7.Text = DataGridView1.CurrentRow.Cells(0).Value
            '    DateTimePickerFormat = "d/MM/yyyy"
            DateTimePicker5.Value = Convert.ToDateTime(DataGridView1.CurrentRow.Cells(7).Value)
        Else
            Label37.Text = ""
            Label36.Text = ""
            Label35.Text = ""
            Label34.Text = ""
            TextBox8.Text = ""
            TextBox6.Text = ""
            TextBox19.Text = ""

            ComboBox7.Text = ""
        End If

        TextBox8.ReadOnly = True
        TextBox6.ReadOnly = True
        TextBox19.ReadOnly = True

        RichTextBox2.ReadOnly = True
        ComboBox7.Enabled = False
        DateTimePicker5.Enabled = False

    End Sub


    Private Sub DataGridView2_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView2.CellClick

        DataGridView2.Rows(DataGridView2.CurrentCell.RowIndex).Selected = True

        'Label26.Text = DataGridView2.CurrentRow.Cells(0).Value
        ' Label27.Text = DataGridView2.CurrentRow.Cells(5).Value
        'Label28.Text = DataGridView2.CurrentRow.Cells(2).Value
        ' Label29.Text = DataGridView2.CurrentRow.Cells(1).Value

        'TextBox4.Text = DataGridView2.CurrentRow.Cells(3).Value
        'TextBox5.Text = DataGridView2.CurrentRow.Cells(2).Value

        '        ComboBox6.Text = DataGridView2.CurrentRow.Cells(0).Value
        '        DateTimePickerFormat = "d/MM/yyyy"
        '        DateTimePicker2.Value = Convert.ToDateTime(DataGridView2.CurrentRow.Cells(4).Value)

    End Sub


    Private Sub Button15_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button15.Click

        ancienprix = TextBox5.Text
        ancienprixachat = TextBox15.Text
        anciennqte = TextBox4.Text
        ancienref = ComboBox6.Text


        If DataGridView1.Rows.Count > 1 Then
            Label26.Text = DataGridView1.CurrentRow.Cells(3).Value
            Label27.Text = DataGridView1.CurrentRow.Cells(2).Value
            Label28.Text = DataGridView1.CurrentRow.Cells(4).Value
            Label29.Text = DataGridView1.CurrentRow.Cells(1).Value
            TextBox4.Text = ""
            RichTextBox1.Text = ""


            'DateTimePickerFormat = "d/MM/yyyy"
            DateTimePicker2.Value = Date.Today

            TextBox4.ReadOnly = False
            TextBox5.ReadOnly = False
            TextBox15.ReadOnly = False

            RichTextBox1.ReadOnly = False
        End If

    End Sub

    Private Sub Button17_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button17.Click
        Dim aa As Excel.Range
        'DateTimePickerFormat = "d/MM/yyyy,hh:mm"
        If (TextBox4.ReadOnly = False) And (ComboBox6.Text <> String.Empty) Then
            If (IsNumeric(TextBox4.Text)) And (IsNumeric(TextBox5.Text)) And (IsNumeric(TextBox15.Text)) Then

                feuilleVente.Range("B" & globalventecompteur).Value = ComboBox6.Text
                feuilleVente.Range("C" & globalventecompteur).Value = Label29.Text
                feuilleVente.Range("D" & globalventecompteur).Value = TextBox5.Text
                feuilleVente.Range("E" & globalventecompteur).Value = TextBox4.Text
                feuilleVente.Range("F" & globalventecompteur).Value = DateTime.Now.ToOADate
                feuilleVente.Range("F" & globalventecompteur).NumberFormat = vbGeneralDate
                feuilleVente.Range("G" & globalventecompteur).Value = Label27.Text
                feuilleVente.Range("H" & globalventecompteur).Value = TextBox15.Text

                feuilleVente.Range("J" & globalventecompteur).Value = RichTextBox1.Text

                globalventecompteur = globalventecompteur + 1

                aa = sheetXls.Cells.Find(ComboBox6.Text)

                sheetXls.Range("H" & aa.Row).Value = TextBox5.Text

                sheetXls.Range("M" & aa.Row).Value = TextBox15.Text
                sheetXls.Range("J" & aa.Row).Value = Val(sheetXls.Range("J" & aa.Row).Value) + Val(TextBox4.Text)
                bookXls.Save()
                bookXls.RefreshAll()
                MsgBox("L'entrée a été correctement enregistrée", MsgBoxStyle.DefaultButton1, "QnStock")
            End If

            TextBox4.ReadOnly = True
            TextBox5.ReadOnly = True
            TextBox15.ReadOnly = True

            RichTextBox1.ReadOnly = True
        Else : MsgBox("Valeurs non valides !")
        End If
        Call traitement_page2()
    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        DataGridView2.Rows(0).Selected = True
        DataGridView2.CurrentCell = DataGridView2.Item(0, 0)
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        If DataGridView2.CurrentCell.RowIndex >= 1 Then
            DataGridView2.Rows(DataGridView2.CurrentCell.RowIndex - 1).Selected = True
            DataGridView2.CurrentCell = DataGridView2.Item(0, DataGridView2.CurrentCell.RowIndex - 1)
        End If

    End Sub

    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        If DataGridView2.CurrentCell.RowIndex < DataGridView2.RowCount - 2 Then
            DataGridView2.Rows(DataGridView2.CurrentCell.RowIndex + 1).Selected = True
            DataGridView2.CurrentCell = DataGridView2.Item(0, DataGridView2.CurrentCell.RowIndex + 1)
        End If

    End Sub

    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        DataGridView2.Rows(DataGridView2.RowCount - 2).Selected = True
        DataGridView2.CurrentCell = DataGridView2.Item(0, DataGridView2.RowCount - 2)

    End Sub

    Private Sub Button40_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button40.Click
        DataGridView1.Rows(DataGridView1.RowCount - 2).Selected = True
        DataGridView1.CurrentCell = DataGridView1.Item(0, DataGridView1.RowCount - 2)
        TextBox1.Text = DataGridView1.CurrentRow.Cells(0).Value
        TextBox2.Text = DataGridView1.CurrentRow.Cells(1).Value
        TextBox7.Text = DataGridView1.CurrentRow.Cells(4).Value
        TextBox3.Text = DataGridView1.CurrentRow.Cells(8).Value
        TextBox13.Text = DataGridView1.CurrentRow.Cells(3).Value
        TextBox14.Text = DataGridView1.CurrentRow.Cells(2).Value
        ComboBox1.Text = DataGridView1.CurrentRow.Cells(5).Value
        ComboBox2.Text = DataGridView1.CurrentRow.Cells(6).Value
        ComboBox3.Text = DataGridView1.CurrentRow.Cells(9).Value
        '  DateTimePickerFormat = "d/MM/yyyy"

        DateTimePicker1.Value = Convert.ToDateTime(DataGridView1.CurrentRow.Cells(7).Value)
    End Sub

    Private Sub Button37_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button37.Click
        DataGridView1.Rows(0).Selected = True
        DataGridView1.CurrentCell = DataGridView1.Item(0, 0)
        TextBox1.Text = DataGridView1.CurrentRow.Cells(0).Value
        TextBox2.Text = DataGridView1.CurrentRow.Cells(1).Value
        TextBox7.Text = DataGridView1.CurrentRow.Cells(4).Value
        TextBox3.Text = DataGridView1.CurrentRow.Cells(8).Value
        TextBox13.Text = DataGridView1.CurrentRow.Cells(3).Value
        TextBox14.Text = DataGridView1.CurrentRow.Cells(2).Value
        ComboBox1.Text = DataGridView1.CurrentRow.Cells(5).Value
        ComboBox2.Text = DataGridView1.CurrentRow.Cells(6).Value
        ComboBox3.Text = DataGridView1.CurrentRow.Cells(9).Value
        ' DateTimePickerFormat = "d/MM/yyyy"

        DateTimePicker1.Value = Convert.ToDateTime(DataGridView1.CurrentRow.Cells(7).Value)
    End Sub

    Private Sub Button38_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button38.Click
        If DataGridView1.CurrentCell.RowIndex >= 1 Then
            DataGridView1.Rows(DataGridView1.CurrentCell.RowIndex - 1).Selected = True
            DataGridView1.CurrentCell = DataGridView1.Item(0, DataGridView1.CurrentCell.RowIndex - 1)
            TextBox1.Text = DataGridView1.CurrentRow.Cells(0).Value
            TextBox2.Text = DataGridView1.CurrentRow.Cells(1).Value
            TextBox7.Text = DataGridView1.CurrentRow.Cells(4).Value
            TextBox3.Text = DataGridView1.CurrentRow.Cells(8).Value
            TextBox13.Text = DataGridView1.CurrentRow.Cells(3).Value
            TextBox14.Text = DataGridView1.CurrentRow.Cells(2).Value
            ComboBox1.Text = DataGridView1.CurrentRow.Cells(5).Value
            ComboBox2.Text = DataGridView1.CurrentRow.Cells(6).Value
            ComboBox3.Text = DataGridView1.CurrentRow.Cells(9).Value
            '   DateTimePickerFormat = "d/MM/yyyy"

            DateTimePicker1.Value = Convert.ToDateTime(DataGridView1.CurrentRow.Cells(7).Value)
        End If

    End Sub

    Private Sub Button39_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button39.Click
        If DataGridView1.CurrentCell.RowIndex < DataGridView1.RowCount - 2 Then
            DataGridView1.Rows(DataGridView1.CurrentCell.RowIndex + 1).Selected = True
            DataGridView1.CurrentCell = DataGridView1.Item(0, DataGridView1.CurrentCell.RowIndex + 1)
            TextBox1.Text = DataGridView1.CurrentRow.Cells(0).Value
            TextBox2.Text = DataGridView1.CurrentRow.Cells(1).Value
            TextBox7.Text = DataGridView1.CurrentRow.Cells(4).Value
            TextBox3.Text = DataGridView1.CurrentRow.Cells(8).Value
            TextBox13.Text = DataGridView1.CurrentRow.Cells(3).Value
            TextBox14.Text = DataGridView1.CurrentRow.Cells(2).Value
            ComboBox1.Text = DataGridView1.CurrentRow.Cells(5).Value
            ComboBox2.Text = DataGridView1.CurrentRow.Cells(6).Value
            ComboBox3.Text = DataGridView1.CurrentRow.Cells(9).Value
            '      DateTimePickerFormat = "d/MM/yyyy"

            DateTimePicker1.Value = Convert.ToDateTime(DataGridView1.CurrentRow.Cells(7).Value)
        End If
    End Sub

    Private Sub Button41_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button41.Click
        testnouveau = "false"
        text1 = TextBox1.Text
        text2 = TextBox2.Text
        text7 = TextBox7.Text
        text3 = TextBox3.Text
        text13 = TextBox13.Text

        text20 = TextBox17.Text
        text14 = TextBox14.Text
        text15 = ComboBox1.Text
        text16 = ComboBox2.Text
        text17 = ComboBox3.Text
        text18 = DateTimePicker1.Value

        TextBox1.ReadOnly = False
        TextBox2.ReadOnly = False
        TextBox7.ReadOnly = False
        TextBox3.ReadOnly = False
        TextBox13.ReadOnly = False
        TextBox14.ReadOnly = False

        TextBox17.ReadOnly = False
        ComboBox1.Enabled = True
        ComboBox2.Enabled = True
        ComboBox3.Enabled = True
        DateTimePicker1.Enabled = True


        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox7.Text = ""
        TextBox3.Text = ""
        TextBox13.Text = ""
        TextBox14.Text = ""

        TextBox17.Text = ""
        ComboBox1.Text = ""
        ComboBox2.Text = ""
        ComboBox3.Text = ""
        ' DateTimePickerFormat = "d/MM/yyyy"
        DateTimePicker1.Value = DateTime.Today
    End Sub

    Private Sub Button42_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button42.Click
        testnouveau = "True"
        text1 = TextBox1.Text
        text2 = TextBox2.Text
        text7 = TextBox7.Text
        text3 = TextBox3.Text
        text13 = TextBox13.Text
        text14 = TextBox14.Text

        text20 = TextBox17.Text
        text15 = ComboBox1.Text
        text16 = ComboBox2.Text
        text17 = ComboBox3.Text
        text18 = DateTimePicker1.Value

        TextBox1.ReadOnly = False
        TextBox2.ReadOnly = False
        TextBox7.ReadOnly = False
        TextBox3.ReadOnly = False
        TextBox13.ReadOnly = False

        TextBox14.ReadOnly = False
        TextBox17.ReadOnly = False
        ComboBox1.Enabled = True
        ComboBox2.Enabled = True
        ComboBox3.Enabled = True
        DateTimePicker1.Enabled = False

        ' DateTimePickerFormat = "d/MM/yyyy"
        DateTimePicker1.Value = DateTime.Today
    End Sub

    Private Sub Button43_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button43.Click
        Dim aa As Excel.Range
        If TextBox1.Text <> String.Empty Then
            aa = sheetXls.Cells.Find(TextBox1.Text)

            If Not IsNothing(aa) Then
                globalstockcompteur = globalstockcompteur - 1

                sheetXls.Range("A" & aa.Row & ":M" & aa.Row).Delete()

                bookXls.Save()
                bookXls.RefreshAll()
                MsgBox("L'article a été correctement supprimé", MsgBoxStyle.DefaultButton1, "QnStock")
            Else
                MsgBox("Référence d'article n'existe pas !", MsgBoxStyle.DefaultButton1, "QnStock")
            End If
        End If
    End Sub

    Private Sub Button44_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button44.Click
        Dim aa As Excel.Range
        If TextBox1.ReadOnly <> True Then

            If TextBox1.Text <> String.Empty Then
                aa = sheetXls.Cells.Find(TextBox1.Text)

                If IsNothing(aa) Then
                    globalstockcompteur = globalstockcompteur + 1
                    cellstock = "B" & globalstockcompteur
                    sheetXls.Range("B" & globalstockcompteur).Value = TextBox1.Text
                    sheetXls.Range("C" & globalstockcompteur).Value = TextBox2.Text
                    sheetXls.Range("H" & globalstockcompteur).Value = TextBox7.Text
                    sheetXls.Range("D" & globalstockcompteur).Value = TextBox3.Text
                    sheetXls.Range("E" & globalstockcompteur).Value = ComboBox1.Text
                    sheetXls.Range("F" & globalstockcompteur).Value = ComboBox2.Text
                    sheetXls.Range("G" & globalstockcompteur).Value = ComboBox3.Text
                    sheetXls.Range("I" & globalstockcompteur).Value = DateTime.Now.ToOADate
                    sheetXls.Range("I" & globalstockcompteur).NumberFormat = vbGeneralDate
                    sheetXls.Range("J" & globalstockcompteur).Value = TextBox13.Text
                    sheetXls.Range("K" & globalstockcompteur).Value = TextBox14.Text

                    sheetXls.Range("M" & globalstockcompteur).Value = TextBox17.Text

                    bookXls.Save()
                    MsgBox("L'article a été correctement ajouté", MsgBoxStyle.DefaultButton1, "QnStock")
                Else
                    If testnouveau = "True" Then
                        sheetXls.Range("B" & aa.Row).Value = TextBox1.Text
                        sheetXls.Range("C" & aa.Row).Value = TextBox2.Text
                        sheetXls.Range("H" & aa.Row).Value = TextBox7.Text
                        sheetXls.Range("D" & aa.Row).Value = TextBox3.Text
                        sheetXls.Range("E" & aa.Row).Value = ComboBox1.Text
                        sheetXls.Range("F" & aa.Row).Value = ComboBox2.Text
                        sheetXls.Range("G" & aa.Row).Value = ComboBox3.Text
                        sheetXls.Range("I" & aa.Row).Value = DateTime.Now.ToOADate
                        sheetXls.Range("J" & aa.Row).Value = TextBox13.Text
                        sheetXls.Range("K" & aa.Row).Value = TextBox14.Text

                        sheetXls.Range("M" & aa.Row).Value = TextBox17.Text
                        bookXls.Save()
                        bookXls.RefreshAll()
                        MsgBox("L'article a été correctement modifié", vbOK + vbInformation, "Information")
                    Else
                        MsgBox("L'article existe déjà ! ", vbOK + vbInformation, "Information")
                    End If

                End If
            End If
        End If
        TextBox1.ReadOnly = True
        TextBox2.ReadOnly = True
        TextBox7.ReadOnly = True
        TextBox3.ReadOnly = True
        TextBox13.ReadOnly = True
        TextBox14.ReadOnly = True

        TextBox17.ReadOnly = True
        ComboBox1.Enabled = False
        ComboBox2.Enabled = False
        ComboBox3.Enabled = False
        DateTimePicker1.Enabled = False
    End Sub

    Private Sub Button45_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button45.Click
        Dim aa As Excel.Range

        If text1 = String.Empty Then
            If TextBox1.Text <> String.Empty Then
                aa = sheetXls.Cells.Find(TextBox1.Text)
                globalstockcompteur = globalstockcompteur - 1
                '   aa.Delete()
                bookXls.Save()
                bookXls.RefreshAll()

            End If
            aa = sheetXls.Cells.Find(TextBox1.Text, , Excel.XlFindLookIn.xlValues, , Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious)
        Else
            sheetXls.Range("B" & aa.Row).Value = text1
            sheetXls.Range("C" & aa.Row).Value = text2
            sheetXls.Range("H" & aa.Row).Value = text7
            sheetXls.Range("D" & aa.Row).Value = text3
            sheetXls.Range("E" & aa.Row).Value = text15
            sheetXls.Range("F" & aa.Row).Value = text16
            sheetXls.Range("G" & aa.Row).Value = text17
            sheetXls.Range("I" & aa.Row).Value = text18
            sheetXls.Range("J" & aa.Row).Value = text13
            sheetXls.Range("K" & aa.Row).Value = text14
            sheetXls.Range("L" & aa.Row).Value = text19
            sheetXls.Range("M" & aa.Row).Value = text20
            bookXls.Save()
            bookXls.RefreshAll()

            text1 = ""
            text2 = ""
            text7 = ""
            text3 = ""
            text13 = ""
            text14 = ""
            text15 = ""
            text16 = ""
            text17 = ""
            text18 = ""
            text19 = ""
            text20 = ""

        End If
    End Sub

    Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button16.Click
        Dim aa As Excel.Range
        Dim sas, sbs As String
        Dim qte As Integer

        If DataGridView2.Rows.Count > 1 Then
            sas = DataGridView2.CurrentRow.Cells(4).Value.ToString
            sbs = DataGridView2.CurrentRow.Cells(0).Value.ToString
            If sas <> String.Empty Then

                aa = feuilleVente.Cells.Find(sas)

                If Not IsNothing(aa) Then
                    qte = feuilleVente.Range("E" & aa.Row).Value
                    feuilleVente.Range("A" & aa.Row & ":J" & aa.Row).Delete()
                    globalventecompteur = globalventecompteur - 1

                    bookXls.Save()
                    bookXls.RefreshAll()
                    MsgBox("L'entrée a été correctement supprimée !", MsgBoxStyle.DefaultButton1, "QnStock")
                    If MsgBox("Voulez-vous réajuster le stock ?", MsgBoxStyle.YesNo, "QnStock") = MsgBoxResult.Yes Then
                        'Form1.Show()
                        aa = sheetXls.Cells.Find(sbs)
                        MsgBox(qte)
                        sheetXls.Range("J" & aa.Row).Value = sheetXls.Range("J" & aa.Row).Value - qte
                        MsgBox("Le stock a été correctement réajusté !", MsgBoxStyle.DefaultButton1, "QnStock")
                    End If
                    bookXls.Save()
                Else
                    MsgBox("Référence d'article n'existe pas !", MsgBoxStyle.DefaultButton1, "QnStock")
                End If
            End If
        End If
    End Sub

    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
        Dim aa As Excel.Range
        'MsgBox(anciennqte)
        If anciennqte <> String.Empty And ancienprix <> String.Empty And ancienprixachat <> String.Empty And ancientva <> String.Empty Then

            feuilleVente.Range("A" & globalventecompteur & ":J" & globalventecompteur).Delete()
            globalventecompteur = globalventecompteur - 1
            bookXls.Save()

            aa = sheetXls.Cells.Find(ancienref)
            sheetXls.Range("H" & aa.Row).Value = ancienprix
            sheetXls.Range("J" & aa.Row).Value = anciennqte
            sheetXls.Range("L" & aa.Row).Value = ancientva
            sheetXls.Range("M" & aa.Row).Value = ancienprixachat
            bookXls.Save()
            bookXls.RefreshAll()
            MsgBox("L'entrée a été correctement annulée", MsgBoxStyle.DefaultButton1, "QnStock")

        End If
        anciennqte = ""
        ancienprix = ""
        ancienref = ""
        ancientva = ""
        ancienprixachat = ""
        Call traitement_page2()
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        Dim CalcProcess As Process
        CalcProcess = Process.GetProcessById(Shell("Calc.exe"))
    End Sub

    Private Sub DateTimePicker3_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker3.ValueChanged
        Dim ii As Integer

        For ii = 0 To DataGridView2.RowCount - 2

            If DataGridView2.Item(4, ii).Value < DateTimePicker3.Value Then
                DataGridView2.Rows(ii).Visible = False
            End If
        Next
    End Sub

    Private Sub DateTimePicker4_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker4.ValueChanged
        Dim ii As Integer

        For ii = 0 To DataGridView2.RowCount - 2

            If DataGridView2.Item(4, ii).Value > DateTimePicker4.Value Then
                DataGridView2.Rows(ii).Visible = False
            End If
        Next
    End Sub

    Private Sub Button21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button21.Click
        DataGridView3.Rows(0).Selected = True
        DataGridView3.CurrentCell = DataGridView3.Item(0, 0)
    End Sub

    Private Sub Button22_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button22.Click
        If DataGridView3.CurrentCell.RowIndex >= 1 Then
            DataGridView3.Rows(DataGridView3.CurrentCell.RowIndex - 1).Selected = True
            DataGridView3.CurrentCell = DataGridView3.Item(0, DataGridView3.CurrentCell.RowIndex - 1)
        End If
    End Sub

    Private Sub Button24_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button24.Click
        If DataGridView3.CurrentCell.RowIndex < DataGridView3.RowCount - 2 Then
            DataGridView3.Rows(DataGridView3.CurrentCell.RowIndex + 1).Selected = True
            DataGridView3.CurrentCell = DataGridView3.Item(0, DataGridView3.CurrentCell.RowIndex + 1)
        End If
    End Sub

    Private Sub Button23_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button23.Click
        DataGridView3.Rows(DataGridView3.RowCount - 2).Selected = True
        DataGridView3.CurrentCell = DataGridView3.Item(0, DataGridView3.RowCount - 2)
    End Sub

    Private Sub Button28_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button28.Click

        ancienprixsortie = TextBox6.Text
        ancienqtesortie = TextBox8.Text
        ancienrefsortie = ComboBox7.Text

        ancienprixachatsortie = TextBox19.Text

        If DataGridView1.Rows.Count > 1 Then
            Label37.Text = DataGridView1.CurrentRow.Cells(3).Value
            Label36.Text = DataGridView1.CurrentRow.Cells(2).Value
            Label35.Text = DataGridView1.CurrentRow.Cells(4).Value
            Label34.Text = DataGridView1.CurrentRow.Cells(1).Value
            TextBox8.Text = ""
            RichTextBox2.Text = ""


            '    DateTimePickerFormat = "d/MM/yyyy"
            DateTimePicker5.Value = Date.Today

            TextBox8.ReadOnly = False
            TextBox6.ReadOnly = False
            TextBox19.ReadOnly = False

            RichTextBox2.ReadOnly = False
        End If
    End Sub

    Private Sub Button27_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button27.Click
        Dim aa As Excel.Range
        Dim sas, sbs As String
        Dim qte As Integer
        If DataGridView3.Rows.Count > 1 Then
            sas = DataGridView3.CurrentRow.Cells(4).Value.ToString
            sbs = DataGridView3.CurrentRow.Cells(0).Value.ToString
            If sas <> String.Empty Then

                aa = feuillesortie.Cells.Find(sas)

                If Not IsNothing(aa) Then
                    qte = feuillesortie.Range("E" & aa.Row).Value
                    feuillesortie.Range("A" & aa.Row & ":J" & aa.Row).Delete()
                    globalsortiecompteur = globalsortiecompteur - 1

                    bookXls.Save()
                    bookXls.RefreshAll()
                    MsgBox("La sortie a été correctement supprimée !", MsgBoxStyle.DefaultButton1, "QnStock")
                    If MsgBox("Voulez-vous réajuster le stock ?", MsgBoxStyle.YesNo, "QnStock") = MsgBoxResult.Yes Then
                        'Form1.Show()
                        aa = sheetXls.Cells.Find(sbs)
                        MsgBox(qte)
                        sheetXls.Range("J" & aa.Row).Value = sheetXls.Range("J" & aa.Row).Value + qte
                        MsgBox("Le stock a été correctement réajusté !", MsgBoxStyle.DefaultButton1, "QnStock")
                    End If
                    bookXls.Save()
                Else
                    MsgBox("Référence d'article n'existe pas !", MsgBoxStyle.DefaultButton1, "QnStock")
                End If
            End If
        End If

        '-------------'
        'Dim aa As Excel.Range
        'If DataGridView3.CurrentRow.Cells(0).Value <> String.Empty Then

        'aa = feuillesortie.Cells.Find(DataGridView3.CurrentRow.Cells(0).Value.ToString)

        'If Not IsNothing(aa) Then

        'feuillesortie.Range("A" & aa.Row & ":K" & aa.Row).Delete()
        'globalsortiecompteur = globalsortiecompteur - 1
        'bookXls.Save()
        'MsgBox("La sortie a été correctement supprimé", vbInformation, "Information")

        'Else
        'MsgBox("Référence d'article n'existe pas !")
        'End If
        'End If
    End Sub

    Private Sub Button26_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button26.Click
        Dim aa As Excel.Range
        If (TextBox8.ReadOnly = False) And (ComboBox7.Text <> String.Empty) Then
            If (IsNumeric(TextBox8.Text)) And (IsNumeric(TextBox6.Text)) And (IsNumeric(TextBox19.Text)) Then

                aa = sheetXls.Cells.Find(ComboBox7.Text)

                If (Val(sheetXls.Range("J" & aa.Row).Value) - Val(TextBox8.Text)) > 0 Then
                    feuillesortie.Range("B" & globalsortiecompteur).Value = ComboBox7.Text
                    feuillesortie.Range("C" & globalsortiecompteur).Value = Label34.Text
                    feuillesortie.Range("D" & globalsortiecompteur).Value = (TextBox6.Text)
                    feuillesortie.Range("E" & globalsortiecompteur).Value = (TextBox8.Text)
                    feuillesortie.Range("F" & globalsortiecompteur).Value = DateTime.Now.ToOADate
                    feuilleVente.Range("F" & globalsortiecompteur).NumberFormat = vbGeneralDate

                    feuillesortie.Range("G" & globalsortiecompteur).Value = Label36.Text
                    feuillesortie.Range("H" & globalsortiecompteur).Value = TextBox19.Text

                    feuillesortie.Range("J" & globalsortiecompteur).Value = RichTextBox2.Text

                    globalsortiecompteur = globalsortiecompteur + 1

                    sheetXls.Range("H" & aa.Row).Value = TextBox6.Text
                    sheetXls.Range("J" & aa.Row).Value = Val(sheetXls.Range("J" & aa.Row).Value) - Val(TextBox8.Text)
                    bookXls.Save()
                    bookXls.RefreshAll()
                    MsgBox("La sortie a été correctement enregistrée", MsgBoxStyle.DefaultButton1, "QnStock")
                Else : MsgBox("Vente impossible : il ne reste que " & sheetXls.Range("J" & aa.Row).Value & " articles en stock", MsgBoxStyle.DefaultButton1, "QnStock")
                End If
            Else : MsgBox("Valeurs non valides !")
            End If

            TextBox8.ReadOnly = True
            TextBox6.ReadOnly = True
            TextBox19.ReadOnly = True

            RichTextBox2.ReadOnly = True
        Else : MsgBox("Veuiller choisir un article !", MsgBoxStyle.DefaultButton1, "QnStock")
        End If

        DataGridView3.Rows.Clear()
        Label37.Text = ""
        Label36.Text = ""
        Label35.Text = ""
        Label34.Text = ""

        TextBox8.Text = ""
        TextBox6.Text = ""
        RichTextBox2.Text = ""

        ComboBox7.Text = ""
        '  DateTimePickerFormat = "d/MM/yyyy"
        DateTimePicker5.Value = Today
    End Sub

    Private Sub Button25_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button25.Click
        Dim aa As Excel.Range

        If ancienqtesortie <> String.Empty And ancienprixsortie <> String.Empty Then

            globalsortiecompteur = globalsortiecompteur - 1
            feuillesortie.Range("A" & globalsortiecompteur & ":J" & globalsortiecompteur).Delete()

            aa = sheetXls.Cells.Find(ancienrefsortie)
            sheetXls.Range("H" & aa.Row).Value = ancienprixsortie
            sheetXls.Range("J" & aa.Row).Value = ancienqtesortie
            bookXls.Save()
            bookXls.RefreshAll()
            MsgBox("La sortie a été correctement supprimée", MsgBoxStyle.DefaultButton1, "QnStock")

        End If
        ancienqtesortie = ""
        ancienprixsortie = ""
        ancienrefsortie = ""
        ancienprixachatsortie = ""
        ancientvasortie = ""

        Call Traitement_page3()
    End Sub

    'Private Sub DateTimePicker7_CloseUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker7.CloseUp
    ' Dim ii As Integer

    '        For ii = 0 To DataGridView3.RowCount - 2
    '   ' MsgBox(DataGridView3.Item(4, ii).Value)
    '   ' MsgBox(DateTimePicker7.Value.Date.ToShortDateString)
    '   'MsgBox(DataGridView3.Item(4, ii).Value < DateTimePicker7.Value.Date.ToShortDateString)

    '        If DataGridView3.Item(4, ii).Value < DateTimePicker7.Value.Date.ToShortDateString Then

    '           DataGridView3.Rows(ii).Visible = False
    '      End If
    ' Next
    'End Sub

    ' Private Sub DateTimePicker6_CloseUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker6.CloseUp
    'Dim ii As Integer
    '
    '   For ii = 0 To DataGridView3.RowCount - 2
    '
    '       If DataGridView3.Item(4, ii).Value > DateTimePicker6.Value.Date.ToShortDateString Then
    '          DataGridView3.Rows(ii).Visible = False
    '       End If
    ' Next
    'End Sub

    Private Sub Button18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button18.Click
        Dim CalcProcess As Process
        CalcProcess = Process.GetProcessById(Shell("Calc.exe"))
    End Sub

    Private Sub Button29_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button29.Click
        Dim ii, jj As Integer
        bookXls.RefreshAll()
        DataGridView2.Rows.Clear()
        jj = 1

        If globalventecompteur > 1 Then
            Me.Cursor = Cursors.WaitCursor
            'MsgBox(DateTimePicker3.Value.Date.AddDays(1))
            For ii = 1 To globalventecompteur


                If (DateTime.FromOADate(feuilleVente.Range("F" & ii).Value) >= DateTimePicker3.Value.Date) And (DateTime.FromOADate(feuilleVente.Range("F" & ii).Value) <= DateTimePicker4.Value.Date.AddDays(1)) Then

                    DataGridView2.Rows.Add(1)
                    DataGridView2.Item(0, jj - 1).Value = feuilleVente.Range("B" & ii).Value
                    DataGridView2.Item(1, jj - 1).Value = feuilleVente.Range("C" & ii).Value
                    DataGridView2.Item(2, jj - 1).Value = feuilleVente.Range("D" & ii).Value
                    DataGridView2.Item(3, jj - 1).Value = feuilleVente.Range("E" & ii).Value
                    DataGridView2.Item(4, jj - 1).Value = DateTime.FromOADate(feuilleVente.Range("F" & ii).Value)
                    DataGridView2.Item(5, jj - 1).Value = feuilleVente.Range("G" & ii).Value
                    DataGridView2.Item(6, jj - 1).Value = feuilleVente.Range("H" & ii).Value
                    DataGridView2.Item(7, jj - 1).Value = feuilleVente.Range("I" & ii).Value
                    DataGridView2.Item(8, jj - 1).Value = feuilleVente.Range("J" & ii).Value
                    jj = jj + 1
                End If
            Next
            Me.Cursor = Cursors.Default
        End If
        DataGridView2.Item(0, 0).Selected = True

    End Sub

    Private Sub Button30_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button30.Click
        Dim ii, jj As Integer

        DataGridView3.Rows.Clear()
        Button31.Visible = True
        jj = 1


        If globalsortiecompteur > 1 Then
            Me.Cursor = Cursors.WaitCursor
            For ii = 1 To globalsortiecompteur
                ' MsgBox(globalventecompteur)
                'MsgBox(DateTime.FromOADate(feuillesortie.Range("F" & ii).Value))
                If (DateTime.FromOADate(feuillesortie.Range("F" & ii).Value) >= DateTimePicker7.Value.Date) And (DateTime.FromOADate(feuillesortie.Range("F" & ii).Value) <= DateTimePicker6.Value.Date.AddDays(1)) Then

                    DataGridView3.Rows.Add(1)
                    DataGridView3.Item(0, jj - 1).Value = feuillesortie.Range("B" & ii).Value
                    DataGridView3.Item(1, jj - 1).Value = feuillesortie.Range("C" & ii).Value
                    DataGridView3.Item(2, jj - 1).Value = feuillesortie.Range("D" & ii).Value
                    DataGridView3.Item(3, jj - 1).Value = feuillesortie.Range("E" & ii).Value
                    DataGridView3.Item(4, jj - 1).Value = DateTime.FromOADate(feuillesortie.Range("F" & ii).Value)
                    DataGridView3.Item(5, jj - 1).Value = feuillesortie.Range("G" & ii).Value
                    DataGridView3.Item(6, jj - 1).Value = feuillesortie.Range("H" & ii).Value 'prix d'achat
                    DataGridView3.Item(7, jj - 1).Value = feuillesortie.Range("I" & ii).Value ' tva
                    DataGridView3.Item(8, jj - 1).Value = feuillesortie.Range("J" & ii).Value ' raison

                    jj = jj + 1
                End If
            Next
            Me.Cursor = Cursors.Default ' default
        End If

        DataGridView3.Item(0, 0).Selected = True


    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        TextBox10.Text = Nothing
    End Sub

    Private Sub Button31_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button31.Click
        Dim jj As Integer
        Dim ca, nbrearticle, benefice, tvatotale As Double
        Balance.Visible = True
        ca = 0
        nbrearticle = 0

        For jj = 0 To DataGridView3.Rows.Count - 1
            If DataGridView3.Rows(jj).Visible = True Then
                nbrearticle = nbrearticle + DataGridView3.Item(3, jj).Value
                ca = ca + (DataGridView3.Item(3, jj).Value * DataGridView3.Item(2, jj).Value)
                benefice = benefice + (DataGridView3.Item(3, jj).Value * (DataGridView3.Item(2, jj).Value - DataGridView3.Item(6, jj).Value))
            End If
        Next
        TextBox9.Text = nbrearticle
        TextBox11.Text = ca
        TextBox21.Text = benefice
    End Sub

    Private Sub Button34_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button34.Click
        Balance.Visible = False
        ' Button31.Visible = False

    End Sub

    Private Sub DataGridView2_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView2.CellContentClick
        RichTextBox1.Text = DataGridView2.CurrentRow.Cells(8).Value
    End Sub

    Private Sub DataGridView3_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView3.CellContentClick
        RichTextBox2.Text = DataGridView3.CurrentRow.Cells(8).Value
    End Sub

    Private Sub Button51_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button51.Click
        ColorDialog1.ShowDialog()
        TabPage1.BackColor = ColorDialog1.Color
    End Sub

    Private Sub Button52_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button52.Click
        ColorDialog2.ShowDialog()
        TabPage2.BackColor = ColorDialog2.Color
    End Sub

    Private Sub Button53_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button53.Click
        ColorDialog3.ShowDialog()
        TabPage3.BackColor = ColorDialog3.Color
    End Sub

    Private Sub Button54_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button54.Click
        ColorDialog4.ShowDialog()
        TabPage4.BackColor = ColorDialog4.Color
    End Sub

    Private Sub Button55_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button55.Click
        ColorDialog5.ShowDialog()
        TabPage5.BackColor = ColorDialog5.Color
    End Sub

End Class