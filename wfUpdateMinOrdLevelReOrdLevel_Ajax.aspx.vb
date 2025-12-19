Public Class wfUpdateMinOrdLevelReOrdLevel_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declarations "
    Dim mMinOrdLevelReOrdLevel As MinOrdLevelReOrdLevel
    'Added by Vikrant on 4-AUG-2011
    Dim EventLogID As Guid

#End Region

#Region " Business Properties and Methods "
    Private Sub GetSession()
        mMinOrdLevelReOrdLevel = Session("mMinOrdLevelReOrdLevel")
    End Sub
    Private Sub SetSession()
        Session("mMinOrdLevelReOrdLevel") = mMinOrdLevelReOrdLevel
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMinOrdLevelReOrdLevel")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1               
                Case MsgBoxResult.Ok
                    cmbFromYear.SelectedIndex = 9
                    cmbToYear.SelectedIndex = 10
                    DataFieldBind()
                    upnlSearchCriteria.Update()
            End Select
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
        str = "document.getElementById('" + cntrl.ClientID + "').focus();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
    End Sub
#End Region


#Region " Data Binding "
    Private Sub DataFieldBind()
        mMinOrdLevelReOrdLevel = MinOrdLevelReOrdLevel.GetMinOrdLevelReOrdLevel(cmbFromYear.SelectedItem.ToString, cmbToYear.SelectedItem.ToString)
        dgItems.DataSource = mMinOrdLevelReOrdLevel
        Session("mMinOrdLevelReOrdLevel") = mMinOrdLevelReOrdLevel
        DataBind()
        lblResult.Text = "List of Parts : " & mMinOrdLevelReOrdLevel.Count & " Record(s) Found."
    End Sub
    Private Sub SetComboForcmbYear1()
        Dim i As Integer
        If cmbToYear.Items.Count = 0 Or cmbToYear.SelectedValue = "" Then
            For i = -10 To 10
                cmbToYear.Items.Add(DateAdd(DateInterval.Year, i, Today).Year)
            Next
            cmbToYear.SelectedIndex = 10
        End If
    End Sub
    Private Sub SetComboForcmbYear()
        Dim j As Integer
        If cmbFromYear.Items.Count = 0 Or cmbFromYear.SelectedValue = "" Then
            For j = -10 To 10
                cmbFromYear.Items.Add(DateAdd(DateInterval.Year, j, Today).Year)
            Next
            cmbFromYear.SelectedIndex = 9
        End If
    End Sub
#End Region

#Region " Events "

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Vikrant on 4-AUG-2011
        If Not IsPostBack Then
            RemoveSession()
            SetComboForcmbYear()
            SetComboForcmbYear1()
            DataFieldBind()
            If cmbFromYear.Enabled = True Then
                setFocus(cmbFromYear)
            End If
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        'If cmbFromYear.SelectedIndex >= cmbToYear.SelectedIndex Then
        If (cmbToYear.SelectedIndex - cmbFromYear.SelectedIndex = 1) Then
            mMinOrdLevelReOrdLevel = MinOrdLevelReOrdLevel.GetMinOrdLevelReOrdLevel(cmbFromYear.SelectedItem.ToString, cmbToYear.SelectedItem.ToString)
            dgItems.DataSource = mMinOrdLevelReOrdLevel
            Session("mMinOrdLevelReOrdLevel") = mMinOrdLevelReOrdLevel
            dgItems.DataBind()
            lblResult.Text = "List of Parts : " & mMinOrdLevelReOrdLevel.Count & " Record(s) Found."
        Else
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.FinancialYearSelection, SIMsgBox.Message_text.FinancialYearSelection, "Please change the Year.", MsgBoxStyle.OKOnly)
            'msg1.ReplacePage = "wfUpdateMinOrdLevelReOrdLevel.aspx?"
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.FinancialYearSelection, MSGBox.Message_text.FinancialYearSelection, "Please change the Year.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click, btnUpdateTop.Click
        If (cmbToYear.SelectedIndex - cmbFromYear.SelectedIndex = 1) Then
            Try
                mMinOrdLevelReOrdLevel.Update(CType(cmbFromYear.SelectedItem.ToString, Integer), CType(cmbToYear.SelectedItem.ToString, Integer))
            Catch ex As Exception
                'Dim msg1 As New SIMsgBox(Page, "Update Status", ex.Message, "Update Not Completed !", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfUpdateMinOrdLevelReOrdLevel.aspx?"
                'msg1.Show()
                MSGBoxCtrl.show("Update Status", ex.Message, "Update Not Completed !", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Finally
                'Added by Vikrant on 4-AUG-2011
                MarkLog(Util.Action.Save, "UpdateMin.Stoc LevelandRe-OrderLevel", "From : " + cmbFromYear.SelectedItem.Text + " To : " + cmbToYear.SelectedItem.Text, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                'Dim msg1 As New SIMsgBox(Page, "Update Status", "Update Completed Sucessfully !", "", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfUpdateMinOrdLevelReOrdLevel.aspx?"
                'msg1.Show()
                MSGBoxCtrl.show("Update Status", "Update Completed Sucessfully !", "", MsgBoxStyle.OkOnly, "")
            End Try
        Else
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.FinancialYearSelection, SIMsgBox.Message_text.FinancialYearSelection, "Please change the Year.", MsgBoxStyle.OKOnly)
            'msg1.ReplacePage = "wfUpdateMinOrdLevelReOrdLevel.aspx?"
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.FinancialYearSelection, MSGBox.Message_text.FinancialYearSelection, "Please change the Year.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        'Added by Vikrant on 4-AUG-2011
        MarkLog(Util.Action.Close, "UpdateMin.Stoc LevelandRe-OrderLevel", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub

    '''''Private Sub dgItems_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgItems.PageIndexChanged
    '''''    dgItems.CurrentPageIndex = e.NewPageIndex
    '''''    dgItems.DataSource = mMinOrdLevelReOrdLevel
    '''''    Session("mMinOrdLevelReOrdLevel") = mMinOrdLevelReOrdLevel
    '''''    dgItems.DataBind()
    '''''End Sub
    Private Sub dgItems_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgItems.PageIndexChanging
        dgItems.PageIndex = e.NewPageIndex
        dgItems.DataSource = mMinOrdLevelReOrdLevel
        Session("mMinOrdLevelReOrdLevel") = mMinOrdLevelReOrdLevel
        dgItems.DataBind()

    End Sub

    Private Sub dgItems_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgItems.Sorting
        mMinOrdLevelReOrdLevel.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mMinOrdLevelReOrdLevel") = mMinOrdLevelReOrdLevel
        dgItems.DataSource = mMinOrdLevelReOrdLevel
        dgItems.DataBind()
    End Sub
    '''''Private Sub dgItems_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgItems.SortCommand
    '''''    mMinOrdLevelReOrdLevel.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
    '''''    Session("mMinOrdLevelReOrdLevel") = mMinOrdLevelReOrdLevel
    '''''    dgItems.DataSource = mMinOrdLevelReOrdLevel
    '''''    dgItems.DataBind()
    '''''End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

    
End Class