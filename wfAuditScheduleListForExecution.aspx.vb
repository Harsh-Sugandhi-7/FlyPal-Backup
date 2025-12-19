
'Created By     :   Saylee
'Dated          :   5-Feb-2010
'Modified By    :   6-Apr-2010


Partial Class wfAuditScheduleListForExecution
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtAuditDate As SIControls.SICalendar
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Variable Declaration "
    Public mAuditScheduleListForExecution As AuditScheduleListForExecution
    Protected mAuditExecution As AuditExecution
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAuditExecution = Session("mAuditExecution")
        mAuditScheduleListForExecution = Session("mAuditScheduleListForExecution")
    End Sub
    Private Sub SetSession()
        Session("mAuditExecution") = mAuditExecution
        Session("mAuditScheduleListForExecution") = mAuditScheduleListForExecution
    End Sub
    Private Sub SetTitle()
        lblResult.Text = "Audit Schedule List : " & mAuditScheduleListForExecution.Count & " Record(s) found."
        btnBackTop.Visible = mAuditScheduleListForExecution.Count > 25
    End Sub

    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub SetObject(ByVal Index As Int32)
        mAuditExecution.AuditScheduleID = mAuditScheduleListForExecution(Index).ID
        mAuditExecution.StartDate = txtAuditDate.Value
        mAuditExecution.AuditNo = mAuditScheduleListForExecution(Index).AuditNo
        mAuditExecution.Reference = mAuditScheduleListForExecution(Index).Reference
        mAuditExecution.Description = mAuditScheduleListForExecution(Index).Description
        mAuditExecution.OtherInformation = mAuditScheduleListForExecution(Index).OtherInformation

        For i As Integer = 0 To mAuditScheduleListForExecution(Index).AuditScheduleTasks.Count - 1
            mAuditExecution.AuditExecutionTasks.Add(mAuditExecution.ID)
            mAuditExecution.AuditExecutionTasks.CurrentItem.AuditTaskID = mAuditScheduleListForExecution(Index).AuditScheduleTasks(i).AuditTaskID
        Next

        mAuditExecution.ImageFile = mAuditScheduleListForExecution(Index).ImageFile
        mAuditExecution.ImageSize = mAuditScheduleListForExecution(Index).ImageSize
        mAuditExecution.FileExtension = mAuditScheduleListForExecution(Index).FileExtension

        Session("mAuditExecution") = mAuditExecution
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mAuditScheduleListForExecution = AuditScheduleListForExecution.GetAuditScheduleListForExecution(txtAuditDate.Value.ToString)
        Session("mAuditScheduleListForExecution") = mAuditScheduleListForExecution
        dgAuditScheduleList.DataSource = mAuditScheduleListForExecution
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            txtAuditDate.Value = Today.Date.ToString
            DataFieldBind()
        End If
        SetTitle()
    End Sub
    Private Sub dgAuditScheduleList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgAuditScheduleList.ItemCommand
        Dim Index As Int32 = e.Item.ItemIndex + dgAuditScheduleList.CurrentPageIndex * dgAuditScheduleList.PageSize
        Select Case e.CommandName
            Case "Select"
                SetObject(Index)
                Response.Redirect("wfAuditExecution.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfAuditScheduleListForExecution.aspx" & "&AuditNo=" & mAuditScheduleListForExecution(Index).AuditNo)
        End Select
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgAuditScheduleList.CurrentPageIndex = 0
        mAuditScheduleListForExecution = AuditScheduleListForExecution.GetAuditScheduleListForExecution(txtAuditDate.Value.ToString)
        Session("mAuditScheduleListForExecution") = mAuditScheduleListForExecution
        lblResult.Text = "Audit Schedule List : " & mAuditScheduleListForExecution.Count & " Record(s) found."
        dgAuditScheduleList.DataSource = mAuditScheduleListForExecution
        dgAuditScheduleList.DataBind()
    End Sub
    Private Sub dgAuditScheduleList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgAuditScheduleList.SortCommand
        mAuditScheduleListForExecution.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgAuditScheduleList.DataSource = mAuditScheduleListForExecution
        Session("mAuditScheduleListForExecution") = mAuditScheduleListForExecution
        dgAuditScheduleList.DataBind()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
#End Region
End Class
