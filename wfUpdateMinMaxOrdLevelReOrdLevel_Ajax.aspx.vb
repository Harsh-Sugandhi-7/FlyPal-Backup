Public Class wfUpdateMinMaxOrdLevelReOrdLevel_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mMinMaxOrdLevelReOrdLevelList As MinMaxOrdLevelReOrdLevelList
    Public mCategoryLists As CategoryList
    Public mCurrentpage As Integer = 1
    Public mpageSize As Integer = 100
    Dim mpageindex As Integer = 0
    Dim pagecount As Integer = 0
    Dim totalCount As Integer = 0
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMinMaxOrdLevelReOrdLevelList = Session("mMinMaxOrdLevelReOrdLevelList")
        mCurrentpage = Session("mCurrentpage")
        mpageSize = Session("mpageSize")
        mpageindex = Session("mpageindex")
        pagecount = Session("pagecount")
        totalCount = Session("totalCount")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMinMaxOrdLevelReOrdLevelList")
        Session.Remove("MiddleFrame")
        Session.Remove("mCurrentpage")
        Session.Remove("mpageSize")
        Session.Remove("mpageindex")
        Session.Remove("pagecount")
        Session.Remove("totalCount")
    End Sub
    Private Sub DataFieldBind()
        UpdateItemGridView()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        'Added By Vikrant On 21-Nov-2016 For BA21112016
        If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
            If custValidator.ControlToValidate = "txtSearch" Then
                Dim txtMinStockLevel, txtMaxStockLevel As TextBox
                Dim chkIsOneTimePurchase As CheckBox
                For i As Integer = 0 To gdvItem.Rows.Count - 1
                    txtMinStockLevel = CType(gdvItem.Rows(i).FindControl("txtMinStockLevel"), TextBox)
                    txtMaxStockLevel = CType(gdvItem.Rows(i).FindControl("txtMaxStockLevel"), TextBox)
                    chkIsOneTimePurchase = CType(gdvItem.Rows(i).FindControl("chkIsOneTimePurchase"), CheckBox)

                    If Not chkIsOneTimePurchase.Checked Then
                        If CDec(Val(txtMaxStockLevel.Text)) <= 0 Then
                            custValidator.ErrorMessage = "Either mark Item " & gdvItem.Rows(i).Cells(1).Text & " as One Time Purchase or enter Max Stock Level quantity."
                            e.IsValid = False
                            Exit Sub
                        ElseIf (CDec(Val(txtMaxStockLevel.Text)) > 0) Then
                            If CDec(Val(txtMaxStockLevel.Text)) - CDec(Val(txtMinStockLevel.Text)) < 0 Then
                                custValidator.ErrorMessage = "Max Stock Level quantity of " & gdvItem.Rows(i).Cells(1).Text & " should be greater than Min Stock Level quantity."
                                e.IsValid = False
                                Exit Sub
                            End If
                        End If
                    End If
                Next
            End If
            'End
        End If
    End Sub
    Private Sub FindNow(ByVal Index As Int32)
        mMinMaxOrdLevelReOrdLevelList = MinMaxOrdLevelReOrdLevelList.GetMinMaxOrdLevelReOrdLevelList(txtSearch.Text.Trim, "", cmbCategory.SelectedValue.ToString, IsCustomPaging:=True, CurrentPage:=mpageindex, PageSize:=mpageSize)

        totalCount = mMinMaxOrdLevelReOrdLevelList.TotalCount
        pagecount = Math.Ceiling(totalCount / mpageSize)

        Session("totalCount") = totalCount
        Session("pagecount") = pagecount

        Session("mMinMaxOrdLevelReOrdLevelList") = mMinMaxOrdLevelReOrdLevelList
        gdvItem.DataSource = mMinMaxOrdLevelReOrdLevelList
        gdvItem.DataBind()
        UpdateItemGridView()
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfUpdateMinMaxOrdLevelReOrdLevel_Ajax.aspx?" Then
            RemoveSession()
        End If
    End Sub
    Private Sub SetControl()
        mCategoryLists = CategoryList.GetCategoryList("(All)")
        cmbCategory.DataSource = mCategoryLists
        cmbCategory.DataBind()


        mpageSize = IIf(CInt(Session("mpageSize")) = 0, gdvItem.PageSize, CInt(Session("mpageSize")))
        mCurrentpage = CInt(Session("mCurrentpage"))
        mpageindex = CInt(Session("mpageindex"))
        pagecount = CInt(Session("pagecount"))

        mpageindex = gdvItem.PageIndex
        mCurrentpage = mpageindex + 1

        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        Session("mpageSize") = mpageSize

        FindNow(0)
    End Sub
    Private Sub UpdateItemGridView()
        Dim currentrow As Integer = mpageSize * (mpageindex)
        If totalCount = 0 Then
            lblResult.Text = "List of Part as per criteria : " & totalCount & " Record(s) found."
        Else
            lblResult.Text = "List of Part as per criteria : " & currentrow + 1 & " to " & currentrow + mMinMaxOrdLevelReOrdLevelList.Count & " of " & totalCount & " Record(s) found."
        End If

        gdvItem.DataBind()
        upnlgrid.Update()
    End Sub
    Private Sub Save()
        Dim SaveEventLog As Boolean = False
        Dim RecordCount As Integer
        RecordCount = gdvItem.Rows.Count
        For i As Integer = 0 To gdvItem.Rows.Count - 1
            Dim txtMinStockLevel, txtMaxStockLevel, txtMinReOrderLevel As TextBox
            Dim ChkIsOneTimePurchase As CheckBox 'Added By Vikrant On 21-Nov-2016 For BA21112016
            txtMinStockLevel = CType(Me.gdvItem.Rows(i).FindControl("txtMinStockLevel"), TextBox)
            txtMaxStockLevel = CType(Me.gdvItem.Rows(i).FindControl("txtMaxStockLevel"), TextBox)
            txtMinReOrderLevel = CType(Me.gdvItem.Rows(i).FindControl("txtMinReOrderLevel"), TextBox)
            ChkIsOneTimePurchase = CType(Me.gdvItem.Rows(i).FindControl("ChkIsOneTimePurchase"), CheckBox)

            mMinMaxOrdLevelReOrdLevelList(i).NewMinStockLevel = Val(txtMinStockLevel.Text)
            mMinMaxOrdLevelReOrdLevelList.Item(i).NewMaxStockLevel = Val(txtMaxStockLevel.Text)

            'Added By Vikrant On 21-Nov-2016 For BA21112016
            mMinMaxOrdLevelReOrdLevelList.Item(i).IsOneTimePurchase = ChkIsOneTimePurchase.Checked
            If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
                mMinMaxOrdLevelReOrdLevelList.Item(i).IsConsiderForReOrder = IIf(ChkIsOneTimePurchase.Checked, False, True)
                Dim MaxMinQtyDiffForReOrder As Integer = Val(txtMaxStockLevel.Text) - Val(txtMinStockLevel.Text)
                If MaxMinQtyDiffForReOrder >= 0 Then
                    txtMinReOrderLevel.Text = MaxMinQtyDiffForReOrder.ToString
                End If
            End If
            'End
            mMinMaxOrdLevelReOrdLevelList.Item(i).NewMinReOrderLevel = Val(txtMinReOrderLevel.Text)

            If mMinMaxOrdLevelReOrdLevelList.Item(i).IsDirty Then
                Try
                    MinMaxOrdLevelReOrdLevelList.UpdateMinMaxOrdLevelReOrdLevel(mMinMaxOrdLevelReOrdLevelList(i).ItemID, Val(txtMinStockLevel.Text), Val(txtMinReOrderLevel.Text), Val(txtMaxStockLevel.Text), ChkIsOneTimePurchase.Checked)
                    SaveEventLog = True
                Catch ex As Exception
                    MSGBoxCtrl.Show("Alert", "Error In Updating Min Max Ord. Level Re-Ord. Level.", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End Try
            End If
        Next
        If SaveEventLog Then
            If RecordCount = 1 Then
                MarkLog(Util.Action.Save, "UpdateMin.Stoc LevelandRe-OrderLevel", "User Name: " + HttpContext.Current.User.Identity.Name + ", Part No.: " + mMinMaxOrdLevelReOrdLevelList(0).ItemName + ", Description: " + mMinMaxOrdLevelReOrdLevelList(0).ItemDescription + ", Update Remark: " + txtUpdateRemark.Text.Trim + ", Updated Page No.: " + mCurrentpage.ToString, ErrorType.NoError, Guid.Empty, EventLogID)
            Else
                MarkLog(Util.Action.Save, "UpdateMin.Stoc LevelandRe-OrderLevel", "User Name: " + HttpContext.Current.User.Identity.Name + ", Update Remark: " + txtUpdateRemark.Text.Trim + ", Updated Page No.: " + mCurrentpage.ToString, ErrorType.NoError, Guid.Empty, EventLogID)
            End If

            MSGBoxCtrl.Show("Success", "Modified records updated successfully.", "", MsgBoxStyle.OkOnly, "SuccessMsg")
            txtUpdateRemark.Text = ""
            txtUpdateRemark.DataBind()
            upnlUpdateHistory.Update()
        Else
            MSGBoxCtrl.Show("Alert!", "Please change details of at least One Item and then click on Update button", "", MsgBoxStyle.OkOnly, "SuccessMsg")
        End If

    End Sub

    Private Sub EnableDisable()
        Dim mConsiderForReOrder As String
        For i As Integer = 0 To gdvItem.Rows.Count - 1
            Dim txtMinReOrderLevel, txtMaxStockLevel, txtMinStockLevel As TextBox

            txtMinReOrderLevel = CType(Me.gdvItem.Rows(i).FindControl("txtMinReOrderLevel"), TextBox)
            mConsiderForReOrder = Me.gdvItem.Rows.Item(i).Cells(13).Text

            txtMaxStockLevel = CType(Me.gdvItem.Rows(i).FindControl("txtMaxStockLevel"), TextBox)
            txtMinStockLevel = CType(Me.gdvItem.Rows(i).FindControl("txtMinStockLevel"), TextBox)
            'Added By Vikrant On 21-Nov-2016 For BA21112016
            If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
                Dim chkIsOneTimePurchase As CheckBox
                chkIsOneTimePurchase = CType(Me.gdvItem.Rows(i).FindControl("chkIsOneTimePurchase"), CheckBox)
                If chkIsOneTimePurchase.Checked Then
                    txtMaxStockLevel.Enabled = False
                    txtMinStockLevel.Enabled = False
                    txtMaxStockLevel.Text = "0"
                    txtMinStockLevel.Text = "0"
                    txtMinReOrderLevel.Text = "0"
                Else
                    txtMaxStockLevel.Enabled = True
                    txtMinStockLevel.Enabled = True
                End If
            End If
            'End

            txtMinReOrderLevel.Enabled = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo", False, IIf(mConsiderForReOrder = "True", True, False))
        Next
        gdvItem.Columns(11).Visible = IIf(AppSettings("ClientCode") = "STR", True, False)
    End Sub
#End Region
#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 19-Jul-2011 For All19072011
        If Not IsPostBack And Session("sender") = "" Then
            Session("MiddleFrame") = "wfUpdateMinMaxOrdLevelReOrdLevel_Ajax.aspx?"
            SetControl()
        End If
        EnableDisable()
        'TextChanged(sender, e)
    End Sub
    Private Sub gdvItem_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gdvItem.PageIndexChanging
        gdvItem.PageIndex = e.NewPageIndex
        mCurrentpage = e.NewPageIndex
        Session("mCurrentpage") = mCurrentpage
        FindNow(0)
        EnableDisable()
        'TextChanged(sender, e)
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        gdvItem.PageIndex = 0
        mpageindex = 0
        mCurrentpage = mpageindex + 1
        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        FindNow(0)
        EnableDisable()
        'TextChanged(sender, e)
    End Sub
    Private Sub btnUpdate_Click(sender As Object, e As System.EventArgs) Handles btnUpdate.Click
        If IsValid Then
            Save()
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub

    'Added By Vikrant On 21-Nov-2016 For BA21112016
    Protected Sub txtMaxStockLevel_TextChanged(sender As Object, e As System.EventArgs)
        If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
            Dim CurrentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
            Dim txtMaxStockLevel, txtMinStockLevel, txtMinReOrderLevel As TextBox

            txtMaxStockLevel = CType(CurrentRow.FindControl("txtMaxStockLevel"), TextBox)
            txtMinStockLevel = CType(CurrentRow.FindControl("txtMinStockLevel"), TextBox)
            txtMinReOrderLevel = CType(CurrentRow.FindControl("txtMinReOrderLevel"), TextBox)

            Dim MaxMinQtyDiffForReOrder As Integer = Val(txtMaxStockLevel.Text) - Val(txtMinStockLevel.Text)
            If MaxMinQtyDiffForReOrder >= 0 Then
                txtMinReOrderLevel.Text = MaxMinQtyDiffForReOrder.ToString
            End If
            'End If
        End If
    End Sub
    Protected Sub txtMinStockLevel_TextChanged(sender As Object, e As System.EventArgs)
        If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
            Dim CurrentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
            Dim txtMaxStockLevel, txtMinStockLevel, txtMinReOrderLevel As TextBox

            txtMaxStockLevel = CType(CurrentRow.FindControl("txtMaxStockLevel"), TextBox)
            txtMinStockLevel = CType(CurrentRow.FindControl("txtMinStockLevel"), TextBox)
            txtMinReOrderLevel = CType(CurrentRow.FindControl("txtMinReOrderLevel"), TextBox)

            Dim MaxMinQtyDiffForReOrder As Integer = Val(txtMaxStockLevel.Text) - Val(txtMinStockLevel.Text)
            If MaxMinQtyDiffForReOrder >= 0 Then
                txtMinReOrderLevel.Text = MaxMinQtyDiffForReOrder.ToString
            End If
            'End If
        End If
    End Sub
    'End
#End Region

    Private Sub lnkUpdationHistory_Click(sender As Object, e As System.EventArgs) Handles lnkUpdationHistory.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenUpdationHistoryWindow", "OpenUpdationHistoryWindow();", True)
    End Sub
End Class