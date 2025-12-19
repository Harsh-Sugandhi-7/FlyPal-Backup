
'AJAX Conversion By: Saylee on 17-Mar-2015 : ModuleID:302

Public Class wfUpdateRemovedAssemblyHistory_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Private mMachineList As MachineList
    Private mUpdateHistoryAssemblyStausList As UpdateHistoryAssemblyStatusList
    Private AircraftId As String
    Private RemoveDate As String
    Private mMachine As Machine
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mUpdateHistoryAssemblyStausList = CType(Session("mUpdateHistoryAssemblyStausList"), UpdateHistoryAssemblyStatusList)
        mMachineList = CType(Session("mMachineList"), MachineList)
        AircraftId = CType(Session("AircraftId"), String)
        RemoveDate = CType(Session("RemoveDate"), String)
        mMachine = CType(Session("mMachine"), Machine)
    End Sub
    Private Sub SetSession()
        Session("mUpdateHistoryAssemblyStausList") = mUpdateHistoryAssemblyStausList
        Session("mMachineList") = mMachineList
        Session("AircraftId") = AircraftId
        Session("RemoveDate") = RemoveDate
        Session("mMachine") = mMachine
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mUpdateHistoryAssemblyStausList")
        Session.Remove("mMachineList")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfUpdateRemovedAssemblyHistory_AJAX.aspx?" Then
            Session.Remove("mUpdateHistoryAssemblyStausList")
            Session.Remove("mMachineList")
            Session.Remove("AircraftId")
            Session.Remove("RemoveDate")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub SetCaption()
        lblRemovedAssemblyList.Text = "History for Removed Assembly as of " & New SmartDate(calDate.Text).FormattedText & "  : " & mUpdateHistoryAssemblyStausList.Count & " Record(s) found."
    End Sub
    Private Sub ControlVisibility()
        calDate.Enabled = False
    End Sub
    Private Sub SetGrid()
        Dim B As Boolean
        For j As Integer = 0 To dgRemovedAssemblyList.Rows.Count - 1
            B = CType(Me.dgRemovedAssemblyList.Rows.Item(j).Cells(9).Text, Boolean)
            If B = False Then
                dgRemovedAssemblyList.Rows.Item(j).Cells(8).Enabled = False
            End If
        Next
    End Sub
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        Dim TodayDate As String = Today.Date.ToString(AppSettings("DateFormat").ToString)
        If IsNothing(Session("RemoveDate")) Then
            calDate.Text = TodayDate
            RemoveDate = TodayDate 'Added By Rahul on 29-Apr-2009
        Else
            calDate.Text = RemoveDate
        End If
        Session("RemoveDate") = calDate.Text

        'mMachineList = tmpMachineList.GetMachineList(, , , , , "<SELECT>")

        mMachineList = MachineList.GetMachineListMonitoringStatus(Today.Date.ToShortDateString, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , True, "<SELECT>", SkipIsForInventoryAircarft:=True)

        ''cmbMachine.DataSource = mMachineList
        ''Session("mMachineList") = mMachineList
        ''cmbMachine.DataBind()

        dgRemovedAssemblyList.DataSource = mUpdateHistoryAssemblyStausList
        Session("mUpdateHistoryAssemblyStausList") = mUpdateHistoryAssemblyStausList
        dgRemovedAssemblyList.DataBind()

        txtModel.Text = Session("ModelName")
        txtSerialNo.Text = Session("SerialNo") 'mUpdateHistoryAssemblyStausList(0).SerialNo

        DataBind()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custvalid As CustomValidator = CType(s, CustomValidator)
        If custvalid.ControlToValidate = "cmbMachine" Then

        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        REM:put here the code to initialize the page
        GetSession()
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            DataFieldBind()
            ControlVisibility()
            SetCaption()
            SetGrid()
        End If
      
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Session.Remove("mUpdateHistoryAssemblyStausList")
        ' Response.Redirect(Request.QueryString("BackPage")) '' & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub

    Private Sub dgRemovedAssemblyList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRemovedAssemblyList.RowCommand
        Select Case e.CommandName
            Case "ViewRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgRemovedAssemblyList.PageSize * dgRemovedAssemblyList.PageIndex
                Dim mID As Guid = mUpdateHistoryAssemblyStausList(Index).AssemblyStatusID
                Dim mIsAttachemntAdded As Boolean = mUpdateHistoryAssemblyStausList(mID).IsAttachmentAdded
                Dim No As New Random
                'Added By Saylee On 1-Dec-2014
                Dim mFileAttach As FileAttach
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(mUpdateHistoryAssemblyStausList(Index).ID)
                Session("mFileAttach") = mFileAttach
                If mFileAttach.Size > 0 Then
                    Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        'Dim Str As String
                        'Str = "openFile();"
                        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
                    End If
                End If
        End Select
    End Sub
    Private Sub dgRemovedAssemblyList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgRemovedAssemblyList.Sorting
        mUpdateHistoryAssemblyStausList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mUpdateHistoryAssemblyStausList") = mUpdateHistoryAssemblyStausList
        dgRemovedAssemblyList.DataSource = mUpdateHistoryAssemblyStausList
        dgRemovedAssemblyList.DataBind()
    End Sub
#End Region

  
End Class