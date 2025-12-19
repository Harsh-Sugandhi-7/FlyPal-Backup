Partial Class wfSearchCriteriaForModel
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

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
    Public mAssemblyStatus As AssemblyStatus
    Public mAssemblyList As AssemblyList
    Public mModel As String
    Public mMacList As AssemblyList
    Public mModelName As String
    Public mSerialNo As String
    Public mSelectPeriods As SelectPeriods
    Public mAssemblyStatusID As Guid
    Dim mAssemblyTypeIndex As Integer
#End Region

#Region " Business Methods "
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub GetSession()
        mModel = CType(Session("mModel"), String)
        mSerialNo = CType(Session("mSerialNo"), String)
        mAssemblyList = CType(Session("mAssemblyList"), AssemblyList)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mMacList = CType(Session("mMacList"), AssemblyList)
        mModelName = CType(Session("mModelName"), String)
        mAssemblyTypeIndex = Session("AssemblyTypeIndex")
    End Sub
    Private Sub SetSession()
        Session("mModel") = mModel
        Session("mSerialNo") = mSerialNo
        Session("mAssemblyList") = mAssemblyList
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mMacList") = mMacList
        Session("mModelName") = mModelName
    End Sub
    Private Sub RemoveSessions()
        Session.Remove("mAssemblyList")
    End Sub
    Private Sub DataFieldBind()
        mAssemblyList = AssemblyList.GetAssemblyList("", "", mAssemblyTypeIndex, "{00000000-0000-0000-0000-000000000000}", Now.Date.ToString)  ''mAssemblyList.Item(mAssemblyList.CurrentIndex).ModelName, mAssemblyList.Item(mAssemblyList.CurrentIndex).SerialNo, , mAssemblyList.Item(mAssemblyList.CurrentIndex).ID.ToString)
        dgPartList.DataSource = mAssemblyList
        Session("mAssemblyList") = mAssemblyList
        DataBind()
        lblResult.Text = "List of Searching Criteria :" & mAssemblyList.Count & " Record(s) found."
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            setFocus(dgPartList)
            DataFieldBind()
        End If
    End Sub
    Private Sub dgPartList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgPartList.ItemCommand
        If e.Item.Cells(0).Text = "ID" Or e.Item.Cells(0).Text = "" Then Exit Sub
        If e.Item.Cells(1).Text = "Model" Or e.Item.Cells(1).Text = "" Then Exit Sub
        If e.Item.Cells(2).Text = "Serial No." Or e.Item.Cells(2).Text = "" Then Exit Sub
        Dim MID As Guid = New Guid(e.Item.Cells(0).Text)
        Dim mModelName As String = e.Item.Cells(1).Text
        Dim mSerialNo As String = e.Item.Cells(2).Text

        Select Case e.CommandName
            Case "Select"
                Session("mModelName") = mModelName
                Session("mSerialNo") = mSerialNo
                Session("MID") = MID

                If Not MID.Equals(Guid.Empty) Then
                    mSelectPeriods = SelectPeriods.NewSelectPeriods
                    mAssemblyStatusID = ReportFetchAssemblyStatusInfo.GetReportFetchAssemblyStatusInfo(Today.ToShortDateString, MID).AssemblyStatusID
                    mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyStatusID)
                    Dim i As Integer = 0
                    While i <= mAssemblyStatus.AssemblyStatusPeriods.Count - 1
                        If mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 2 Then
                            mSelectPeriods.Add(mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID, mAssemblyStatus.AssemblyStatusPeriods(i).PeriodName)
                        End If
                        i = i + 1
                    End While
                    Session("mSelectPeriods") = mSelectPeriods
                    Session("sender") = "SelectPeriods"
                End If
                Response.Redirect(Request.QueryString("BackPage"))
        End Select
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session.Remove("mItemList")
        'mItemList = Nothing
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    'Added by Prashant 22-June-2009 for grid sorting
    Private Sub dgPartList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgPartList.SortCommand
        mAssemblyList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgPartList.DataSource = mAssemblyList
        Session("mAssemblyList") = mAssemblyList
        dgPartList.DataBind()
    End Sub
    '----------------------------------------------
#End Region

End Class
