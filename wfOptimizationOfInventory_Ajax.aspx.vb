Public Class wfOptimizationOfInventory_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mOptimizationOfInventoryList As OptimizationOfInventoryList
    Public mCategoryLists As CategoryList
    Public mCurrentpage As Integer = 1
    Public mpageSize As Integer = 100
    Dim mpageindex As Integer = 0
    Dim pagecount As Integer = 0
    Dim totalCount As Integer = 0
    Dim EventLogID As Guid
    Public mModelList As ModelList
    Dim SearchCriteria As String = String.Empty
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mOptimizationOfInventoryList = Session("mOptimizationOfInventoryList")
        mCurrentpage = Session("mCurrentpage")
        mpageSize = Session("mpageSize")
        mpageindex = Session("mpageindex")
        pagecount = Session("pagecount")
        totalCount = Session("totalCount")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mOptimizationOfInventoryList")
        Session.Remove("MiddleFrame")
        Session.Remove("mCurrentpage")
        Session.Remove("mpageSize")
        Session.Remove("mpageindex")
        Session.Remove("pagecount")
        Session.Remove("totalCount")
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
                        'If CDec(Val(txtMaxStockLevel.Text)) <= 0 Then
                        '    custValidator.ErrorMessage = "Either mark Item " & gdvItem.Rows(i).Cells(1).Text & " as One Time Purchase or enter Max Stock Level quantity."
                        '    e.IsValid = False
                        '    Exit Sub
                        'ElseIf (CDec(Val(txtMaxStockLevel.Text)) > 0) Then
                        '    If CDec(Val(txtMaxStockLevel.Text)) - CDec(Val(txtMinStockLevel.Text)) < 0 Then
                        '        custValidator.ErrorMessage = "Max Stock Level quantity of " & gdvItem.Rows(i).Cells(1).Text & " should be greater than Min Stock Level quantity."
                        '        e.IsValid = False
                        '        Exit Sub
                        '    End If
                        'End If
                         If (CDec(Val(txtMaxStockLevel.Text)) > 0) Then
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
    Public Sub CustomValidator1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtMaxMonth" Then
            If CDec(Val(txtMaxMonth.Text)) < CDec(Val(txtMinMonth.Text)) Then
                custValidator.ErrorMessage = "Max level month should be greater than Min level month."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtAvgMonth" Then
            If CDec(Val(txtAvgMonth.Text)) <= 0 Then
                custValidator.ErrorMessage = "Avg. Monthly Comsumption of Last __ Month Should be Grater than 0"
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txt4Year" Then
            If CDec(Val(txt4Year.Text)) <= 0 Then
                custValidator.ErrorMessage = "Yearly Comsumption of Last __ Year Should be Grater than 0"
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txt6Month" Then
            If CDec(Val(txt6Month.Text)) <= 0 Then
                custValidator.ErrorMessage = "Avg. Monthly Comsumption__ Month Should be Grater than 0"
                e.IsValid = False
            End If
        End If
    End Sub
    Private Sub FindNow(ByVal Index As Int32)
        mOptimizationOfInventoryList = OptimizationOfInventoryList.GetOptimizationOfInventoryList(txtSearch.Text.Trim, "", _
                                                                                                  cmbCategory.SelectedValue.ToString, IsCustomPaging:=True, _
                                                                                                  CurrentPage:=mpageindex, PageSize:=mpageSize, _
                                                                                                  MonthForAvgCal:=Val(txtAvgMonth.Text), _
                                                                                                  MaxMonth:=Val(txtMaxMonth.Text), _
                                                                                                  MinMonth:=Val(txtMinMonth.Text), _
                                                                                                  ModelID:=cmbModel.SelectedValue.ToString, _
                                                                                                  ManuallyUpdated:=chkManuallyUpdated.Checked, _
                                                                                                  Year4:=Val(txt4Year.Text.Trim), _
                                                                                                  Month6:=Val(txt6Month.Text.Trim))

        totalCount = mOptimizationOfInventoryList.TotalCount
        pagecount = Math.Ceiling(totalCount / mpageSize)

        Session("totalCount") = totalCount
        Session("pagecount") = pagecount

        Session("mOptimizationOfInventoryList") = mOptimizationOfInventoryList
        gdvItem.DataSource = mOptimizationOfInventoryList
        gdvItem.DataBind()
        UpdateItemGridView()
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfOptimizationOfInventory_Ajax.aspx?" Then
            RemoveSession()
        End If
    End Sub
    Private Sub SetControl()
        mCategoryLists = CategoryList.GetCategoryList()
        cmbCategory.DataSource = mCategoryLists
        cmbCategory.DataBind()

        mModelList = ModelList.GetAirframeModelList("(All)")
        cmbModel.DataSource = mModelList
        cmbModel.DataBind()


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
        'If totalCount = 0 Then
        lblResult.Text = "List of Part as per criteria : " & mOptimizationOfInventoryList.Count.ToString & " Record(s) found."
        'Else
        'lblResult.Text = "List of Part as per criteria : " & currentrow + 1 & " to " & currentrow + mOptimizationOfInventoryList.Count & " of " & totalCount & " Record(s) found."
        'End If

        'SliderExtender1.Minimum = 1
        'SliderExtender1.Maximum = pagecount
        'Slidercontrol.Text = mCurrentpage
        'txtPageDisplay.Text = mCurrentpage
        'lblpagecount.Text = pagecount
        'If pagecount > 1 Then
        '    PnlPaging.Visible = True
        'Else
        '    PnlPaging.Visible = False
        'End If

        gdvItem.DataBind()
        upnlgrid.Update()
    End Sub
    Private Sub Save()
        For i As Integer = 0 To gdvItem.Rows.Count - 1
            Dim txtMinStockLevel, txtMaxStockLevel, txtMinReOrderLevel As TextBox
            Dim ChkIsOneTimePurchase As CheckBox 'Added By Vikrant On 21-Nov-2016 For BA21112016
            txtMinStockLevel = CType(Me.gdvItem.Rows(i).FindControl("txtMinStockLevel"), TextBox)
            txtMaxStockLevel = CType(Me.gdvItem.Rows(i).FindControl("txtMaxStockLevel"), TextBox)
            txtMinReOrderLevel = CType(Me.gdvItem.Rows(i).FindControl("txtMinReOrderLevel"), TextBox)
            ChkIsOneTimePurchase = CType(Me.gdvItem.Rows(i).FindControl("ChkIsOneTimePurchase"), CheckBox)

            '---Added by Prashant On 22-Oct-2019 For BA22102019--------------------
            If ((mOptimizationOfInventoryList(i).NewMinStockLevel <> Val(txtMinStockLevel.Text)) Or _
                (mOptimizationOfInventoryList(i).NewMaxStockLevel <> Val(txtMaxStockLevel.Text))) Then
                mOptimizationOfInventoryList.Item(i).ManuallyUpdated = True
            End If
            '----------------------------------------------------------------------

            mOptimizationOfInventoryList(i).NewMinStockLevel = Val(txtMinStockLevel.Text)
            mOptimizationOfInventoryList.Item(i).NewMaxStockLevel = Val(txtMaxStockLevel.Text)
            mOptimizationOfInventoryList.Item(i).IsOneTimePurchase = ChkIsOneTimePurchase.Checked

            If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
                mOptimizationOfInventoryList.Item(i).IsConsiderForReOrder = IIf(ChkIsOneTimePurchase.Checked, False, True)
                Dim MaxMinQtyDiffForReOrder As Integer = Val(txtMaxStockLevel.Text) - Val(txtMinStockLevel.Text)
                If MaxMinQtyDiffForReOrder >= 0 Then
                    txtMinReOrderLevel.Text = MaxMinQtyDiffForReOrder.ToString
                End If
            End If
            'End
            mOptimizationOfInventoryList.Item(i).NewMinReOrderLevel = Val(txtMinReOrderLevel.Text)

            'If mOptimizationOfInventoryList.Item(i).IsDirty Then
            Try
                OptimizationOfInventoryList.UpdateOptimizationOfInventory(mOptimizationOfInventoryList(i).ItemID, _
                                                                          Val(txtMinStockLevel.Text), Val(txtMinReOrderLevel.Text), _
                                                                          Val(txtMaxStockLevel.Text), ChkIsOneTimePurchase.Checked, _
                                                                          mOptimizationOfInventoryList.Item(i).ManuallyUpdated)
            Catch ex As Exception
                MSGBoxCtrl.show("Alert", "Error In Updating", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End Try
            'End If
        Next
        SearchCriteria = " Part No. " + txtSearch.Text.Trim + " Category " + cmbCategory.SelectedItem.ToString + " Model " + cmbModel.SelectedItem.ToString + " MaxMonth " + txtMaxMonth.Text + " MinMonth " + txtMinMonth.Text + " Year " + txt4Year.Text + " Month " + txt6Month.Text
        MarkLog(Util.Action.Save, "OptimizationOfInventory", "User Name : " + HttpContext.Current.User.Identity.Name + " Date Time : " + Environment.NewLine + Now.ToString + SearchCriteria, ErrorType.NoError, Guid.Empty, EventLogID)
        MSGBoxCtrl.show("Updated Successfully", "Updated Successfully", "", MsgBoxStyle.OkOnly, "")
    End Sub
    'Public Sub TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim txtValue As TextBox
    '    For i As Integer = 0 To gdvItem.Rows.Count - 1
    '        Try
    '            txtValue = CType(Me.gdvItem.Rows(i).FindControl("txtMinStockLevel"), TextBox)
    '            txtValue.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('" + txtValue.ClientID + "').value)")

    '            txtValue = CType(Me.gdvItem.Rows(i).FindControl("txtMaxStockLevel"), TextBox)
    '            txtValue.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('" + txtValue.ClientID + "').value)")

    '            txtValue = CType(Me.gdvItem.Rows(i).FindControl("txtMinReOrderLevel"), TextBox)
    '            txtValue.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('" + txtValue.ClientID + "').value)")

    '        Catch ex As Exception
    '        End Try
    '        i = i + 1
    '    Next
    '    upnlgrid.Update()
    'End Sub
    Private Sub EnableDisable()
        Dim mConsiderForReOrder As String
        For i As Integer = 0 To gdvItem.Rows.Count - 1
            Dim txtMinReOrderLevel, txtMaxStockLevel, txtMinStockLevel As TextBox

            txtMinReOrderLevel = CType(Me.gdvItem.Rows(i).FindControl("txtMinReOrderLevel"), TextBox)
            mConsiderForReOrder = Me.gdvItem.Rows.Item(i).Cells(15).Text  'IsConsiderForReOrder

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
    End Sub
    Private Sub addattributes()
        txtAvgMonth.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('" + txtAvgMonth.ClientID + "').value,event)")
        txtMaxMonth.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('" + txtMaxMonth.ClientID + "').value,event)")
        txtMinMonth.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtMinMonth.ClientID + "').value,event)")
        txt4Year.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('" + txt4Year.ClientID + "').value,event)")
        txt6Month.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('" + txt6Month.ClientID + "').value,event)")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Save" Then
                        Try
                            Save()
                        Catch ex As Exception
                            Throw ex
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Save" Then

                    End If
            End Select
        End If
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        addattributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 19-Jul-2011 For All19072011
        If Not IsPostBack And Session("sender") = "" Then
            Session("MiddleFrame") = "wfOptimizationOfInventory_Ajax.aspx?"
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
        If IsValid Then
            gdvItem.PageIndex = 0
            mpageindex = 0
            mCurrentpage = mpageindex + 1
            Session("mpageindex") = mpageindex
            Session("mCurrentpage") = mCurrentpage
            FindNow(0)
            EnableDisable()
            'TextChanged(sender, e)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnCalculate_Click(sender As Object, e As System.EventArgs) Handles btnCalculate.Click
        If IsValid Then
            gdvItem.PageIndex = 0
            mpageindex = 0
            mCurrentpage = mpageindex + 1
            Session("mpageindex") = mpageindex
            Session("mCurrentpage") = mCurrentpage
            FindNow(0)
            '-----------------------------------------------------------------
            totalCount = mOptimizationOfInventoryList.TotalCount
            pagecount = Math.Ceiling(totalCount / mpageSize)

            Session("totalCount") = totalCount
            Session("pagecount") = pagecount

            Session("mOptimizationOfInventoryList") = mOptimizationOfInventoryList
            gdvItem.DataSource = mOptimizationOfInventoryList
            gdvItem.DataBind()
            UpdateItemGridView()
            '-----------------------------------------------------------------
            EnableDisable()
            'TextChanged(sender, e)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub tabUpdateTop_Click(sender As Object, e As System.EventArgs) Handles tabUpdateTop.Click, tabUpdate.Click
        If IsValid Then
            Try
                MSGBoxCtrl.show("Update Alert", "This will update Min/Max/One time purchase of " + mOptimizationOfInventoryList.Count.ToString + " Part(s). Do you want to continue? ", "", MsgBoxStyle.YesNo, "Save")
                Exit Sub
            Catch ex As Exception
            Finally

            End Try
            'Save()
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    'Private Sub btnGridPaging_Click(sender As Object, e As System.EventArgs) Handles btnGridPaging.Click
    '    mCurrentpage = CInt(Slidercontrol.Text.Trim)
    '    mpageindex = mCurrentpage - 1
    '    gdvItem.PageIndex = mpageindex
    '    Session("mpageindex") = mpageindex
    '    Session("mCurrentpage") = mCurrentpage
    '    FindNow(0)
    '    EnableDisable()
    '    'TextChanged(sender, e)
    'End Sub
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
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region


End Class