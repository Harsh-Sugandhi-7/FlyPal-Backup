Partial Class wfKit_Ajax
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents valError As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents txt As System.Web.UI.WebControls.TextBox
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents reset As System.Web.UI.WebControls.Button

    Protected WithEvents CustomValidator1 As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents print As System.Web.UI.WebControls.ImageButton
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
    Public mKit As Kit
    'Added by Vikrant on 27-July-2011
    Dim EventLogID As Guid
    Dim mItemList As ItemList
    Dim mItem As Item
    Dim ItemIDToSkipForKitItem As String = Guid.Empty.ToString
#End Region

#Region " Business Methods"
    Private Sub GetSession()
        mKit = Session("mKit")
        mItemList = Session("mItemList")
    End Sub
    Private Sub SetSession()
        Session("mKit") = mKit
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValidator As CustomValidator
        CustValidator = CType(s, CustomValidator)
        If CustValidator.ControlToValidate = "txtInspectionKit" Then
            If Trim(txtInspectionKit.Text) = "" Then
                CustValidator.ErrorMessage = "Kit Name Required."
                e.IsValid = False
            ElseIf Len(Trim(txtInspectionKit.Text)) > 50 Then
                CustValidator.ErrorMessage = "Kit Name too long."
                e.IsValid = False
            End If

            If mKit.KitItems.Count = 0 Then
                CustValidator.ErrorMessage = "Atleast one Kit Item required."
                e.IsValid = False
            End If
        End If
    End Sub
    Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtQuantity" Then
            If Val(txtQuantity.Text) <= 0 Then
                custValidator.ErrorMessage = "Quantity must be greater than zero."
                e.IsValid = False
            End If


        End If
    End Sub
    Private Sub NewRecord()
        mKit.KitItems.Add(mKit.ID)
        mKit.KitItems.CurrentItem.SrNo = mKit.KitItems.CurrentIndex + 1
        Session("mKit") = mKit
    End Sub
    Private Function setObject() As Boolean
        mKit.ApplyEdit()
        mKit.KitName = txtInspectionKit.Text
        'If mKit.KitItems.Count <= 0 Then
        '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, "Inspection Kit can not be saved without Kit Item.", MsgBoxStyle.OKOnly)
        '    msg1.ReplacePage = "wfKit_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
        '    msg1.Show()
        '    mKit.CancelEdit()
        '    Exit Function
        'Else
        mKit.ApplyEdit()
        'End If
        Return True
    End Function
    Private Function setObjectForKitItem() As Boolean
        mKit.BeginEdit()
        If ItemIDValue.Value <> "" Then
            mKit.KitItems.CurrentItem.ItemID = New Guid(ItemIDValue.Value)
            mKit.KitItems.CurrentItem.ItemName = mItemList(New Guid(ItemIDValue.Value)).Name 'ItemNameValue.Value
        End If
        mKit.KitItems.CurrentItem.Qty = Val(txtQuantity.Text)
        If Session("EditItem") = False Then
            If mKit.KitItems.Contains(mKit.KitItems.CurrentItem) Then
                mKit.CancelEdit()
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "Kit Item", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfKitItem_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&SearchText=" & mKit.KitItems.CurrentItem.ItemName
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Kit Item", MsgBoxStyle.OkOnly, "KitItem")
                Exit Function
            Else
                mKit.ApplyEdit()
            End If
        Else
            mKit.ApplyEdit()
        End If

        Return True
    End Function
    Private Sub DeleteRecord(ByVal Idx As Integer)
        Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        msg1.ReplacePage = "wfKit_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
        Session("sender") = "Delete"
        msg1.Show()
        mKit.KitItems.CurrentIndex = Idx
        Session("mKit") = mKit
    End Sub
    Private Sub SetPage()
        If mKit.Type = 2 Then
            If mKit.KitName <> "" Then
                If Len(mKit.KitName) > 15 Then
                    lblInspection.Text = "Kit [" & mKit.KitName.Substring(0, 15) & "...]"
                Else
                    lblInspection.Text = "Kit [" & mKit.KitName & "]"
                End If
            Else
                lblInspection.Text = "Kit [New]"
            End If
            lblInspectionKit.Text = "Part Kit"
            txtInspectionKit.ToolTip = "Enter Kit Name"
            btnSave.ToolTip = "Click to Save Kit"
            btnAdd.ToolTip = "Click to Add Kit Item"
        Else
            If mKit.KitName <> "" Then
                If Len(mKit.KitName) > 15 Then
                    lblInspection.Text = "Inspection Kit [" & mKit.KitName.Substring(0, 15) & "...]"
                Else
                    lblInspection.Text = "Inspection Kit [" & mKit.KitName & "]"
                End If
            Else
                lblInspection.Text = "Inspection Kit [New]"
            End If
            txtInspectionKit.ToolTip = "Enter Inspection Kit Name"
            btnSave.ToolTip = "Click to Save Inspection Kit "
            btnAdd.ToolTip = "Click to Add Inspection Kit Item."
        End If

        upnlTitle.Update()
    End Sub
    Private Sub SetPageForKitItem()
        If mKit.Type = 2 Then
            If mKit.KitItems.CurrentItem.ItemName <> "" Then
                lblTitle.Text = "Kit Item [" & mKit.KitItems.CurrentItem.ItemName & "]"
            Else
                lblTitle.Text = "Kit Item [New]"
            End If
            btnSave.ToolTip = "Click to Save Kit"
            btnSearchNew.ToolTip = "Click to find Kit Item"
        Else
            If mKit.KitItems.CurrentItem.ItemName <> "" Then
                lblTitle.Text = "Inspection Item [" & mKit.KitItems.CurrentItem.ItemName & "]"
            Else
                lblTitle.Text = "Inspection Item [New]"
            End If
            btnSave.ToolTip = "Click to Save Inspection Kit "
            btnSearchNew.ToolTip = "Click to find Inspection Item"
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Try
            Dim str As String
            str = "try{document.getElementById('" + cntrl.ClientID + "').focus();}catch (Error) {}"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
        Catch ex As Exception
            '
        End Try
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mKit = Session("mKit")
                            'mKit.KitItems(mKit.KitItems.CurrentIndex).Delete()
                            If mKit.KitItems.Count = 1 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DeleteAlert, MSGBox.Message_text.DeleteAlert, "Can not delete, atleast one Kit Item required.", MsgBoxStyle.OkOnly, "")
                            Else
                                mKit.KitItems.Remove(mKit.KitItems.CurrentItem)
                            End If

                            DataFieldBind()
                            SetPage()
                            ControlVisibility()
                            upnlKit.Update()
                            upnlSaveClose.Update()
                            Try
                                MarkLog(Util.Action.Delete, "Kit", mKit.KitName, Util.ErrorType.NoError, mKit.ID, EventLogID)
                            Catch ex As Exception
                                '
                            End Try

                            'Response.Redirect("wfKit_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                                'msg1.ReplacePage = "wfKit_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                                'msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                                'msg1.ReplacePage = "wfKit_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                                'msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                                'msg1.ReplacePage = "wfKit_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                                'Added by Vikrant
                                MarkLog(Util.Action.Delete, "Kit", "Cant Delete:" & mKit.KitName & " is currently in use", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                                'msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            'DataFieldBind()
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "Close" Then  '' Close confirmation
                        Session("sender") = ""
                        DataFieldBind()
                        Page.Validate("valGroupParent")
                        Try
                            If IsValid Then
                                save()
                            Else
                                upnlValidationSummary.Update()
                                Exit Sub
                            End If
                        Catch ex As Exception
                            Throw ex
                        End Try

                        MarkLog(Util.Action.Save, "Kit", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                        '
                        'Commented For AJAX Implementation
                        'If mKit.Type = 2 Then
                        '    Response.Redirect(Request.QueryString("BackPage"))
                        'End If
                        'Added by utkarsh on 7-nov-2013 for Kit popup
                        Dim mopenas As String = Request.QueryString("Type")
                        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                            Session.Remove("mKit")
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                            Exit Sub
                        End If
                        'End
                        Response.Redirect(Request.QueryString("BackPage"))
                    Else
                        Session("sender") = ""
                        'Response.Redirect("wfKit_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                    End If
                Case MsgBoxResult.No
                    Session("Sender") = ""
                    If MSGBoxCtrl.Sender <> "Delete" Then
                        'Added by utkarsh on 7-nov-2013 for Kit popup
                        Dim mopenas As String = Request.QueryString("Type")
                        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                            Session.Remove("mKit")
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                            Exit Sub
                        End If
                        'End
                        Response.Redirect(Request.QueryString("BackPage"))
                    End If
                    '' Response.Redirect("wfKit_Ajax.aspx?MsgResult=0&&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    If MSGBoxCtrl.Sender = "KitItem" Then
                        'txtSearch.Text = ItemNameValue.Value
                        cmbPartNo.DataSource = mItemList
                        cmbPartNo.DataBind()
                        upnlKitItem.Update()
                    Else
                        DataFieldBind()
                    End If
                    Session("sender") = ""
                    'Response.Redirect("wfKit_Ajax.aspx?MsgResult=0&&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    'Response.Redirect("wfKit_Ajax.aspx?MsgResult=0&&BackPage=" & Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
            'Response.Redirect("wfKit_Ajax.aspx?MsgResult=0&&BackPage=" & Request.QueryString("BackPage"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            ' DataFieldBind()
        End If
    End Sub
    Private Sub save()
        Try
            If setObject() Then
                mKit.Save()
                'Added by Vikrant on 27-July-2011
                MarkLog(Util.Action.Save, "Kit", mKit.KitName, Util.ErrorType.NoError, mKit.ID, EventLogID)
                SetPage() 'AJAX
                If mKit.KitItems.Count = 0 Then
                    Dim clnKit As Kit
                    clnKit = mKit.Clone
                    Kit.DeleteKit(mKit.ID)
                    mKit = Kit.NewKit
                    mKit.ItemID = clnKit.ItemID
                    mKit.KitName = clnKit.KitName
                    mKit.Type = clnKit.Type
                    Session("mKit") = mKit
                    'Added by Shweta on 9-May-2012 for 09052012-1
                    btnPrint.Enabled = False
                    '********************************************
                Else
                    btnPrint.Enabled = True
                End If
                upnlSaveClose.Update()

                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully,
                                MSGBox.Message_text.SavedSuccessFully,
                                "",
                                MsgBoxStyle.OkOnly,
                                "")
            End If
        Catch ex As SqlException
            If ex.Number = 8145 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfKit_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
                'Session("sender") = "Delete"
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
            ElseIf ex.Number = 2627 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfKit_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
                'Session("sender") = "Delete"
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
            ElseIf ex.Number = 547 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfKit_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
                'Session("sender") = "Delete"
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
            End If
        End Try
        'End If
    End Sub
    Private Sub ControlVisibility() 'Added by Shweta on 9-May-2012 for 09052012-1
        'btnPrint.Enabled = Not mKit.IsNew
        If (mKit.KitItems.Count = 0) Then
            btnPrint.Enabled = False
        ElseIf Not mKit.IsNew Then
            btnPrint.Enabled = True
        End If
        upnlSaveClose.Update()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        dgKit.DataSource = mKit.KitItems
        Session("mKit") = mKit
        dgKit.DataBind()
        txtInspectionKit.DataBind()
        lblResult.Text = "Kit Item List :" + CType(mKit.KitItems.Count, String) + " Record(s)."
        upnlGridView.Update()
    End Sub
    Private Sub DataFieldBindForKitItem(Optional ByVal SearchText As String = "")
        If mKit.Type = 2 Then
            mItem = Session("mItem")
            ItemIDToSkipForKitItem = mItem.ID.ToString
        Else
            ItemIDToSkipForKitItem = Guid.Empty.ToString
        End If
        mItemList = ItemList.GetItemList(7, SearchText, SearchText, , , , , True, ItemIDToSkipForKitItem:=ItemIDToSkipForKitItem)
        Session("mItemList") = mItemList
        cmbPartNo.DataSource = mItemList
        cmbPartNo.SelectedValue = mKit.KitItems.CurrentItem.ItemID.ToString
        txtQuantity.Text = mKit.KitItems.CurrentItem.Qty.ToString
        txtSrNo.Text = mKit.KitItems.CurrentItem.SrNo.ToString
        txtSearch.Text = SearchText
        cmbPartNo.DataBind()
        txtQuantity.DataBind()
        txtSearch.DataBind()
    End Sub
    Private Sub ReloadPageAfterModalPopUpClose()
        DataFieldBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        'Added by Vikrant on 27-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            If mKit.Type = 1 Then
                If txtInspectionKit.Enabled = True Then
                    setFocus(txtInspectionKit)
                End If
            Else
                If btnAdd.Enabled = True Then
                    setFocus(btnAdd)
                End If
            End If
            DataFieldBind()
            SetPage()
            ControlVisibility()
        End If
        'MessageBoxResult()
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        mKit.KitName = txtInspectionKit.Text.Trim
        NewRecord()
        If (Not User.IsInRole("KitNew") And mKit.IsNew) Or (Not User.IsInRole("KitEdit") And Not mKit.IsNew) Then
            'Added by Vikrant on 27-July-2011
            MarkLog(Util.Action.[New], "Kit", User.Identity.Name & " is not Authorized User to add " & mKit.KitName, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        'Added by Vikrant on 27-July-2011
        MarkLog(Util.Action.[New], "Kit", "", Util.ErrorType.NoError, mKit.ID, EventLogID)
        'AJAX
        txtQuantity.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtQuantity').value,event)")
        DataFieldBindForKitItem()
        SetPageForKitItem()
        mdlPopUpKitItem.Show()
        upnlKitItem.Update()
        'End
        'Response.Redirect("wfKitItem_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfKit_Ajax.aspx")
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("KitNew") And mKit.IsNew) Or (Not User.IsInRole("KitEdit") And Not mKit.IsNew) Then
            setObject()
            SetSession()
            'Added by Vikrant on 27-July-2011
            MarkLog(Util.Action.Save, "Kit", User.Identity.Name & " is not Authorized User to save " & mKit.KitName, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If IsValid Then
            save()
        Else
            upnlValidationSummary.Update()
        End If

    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        mKit.KitName = Trim(txtInspectionKit.Text)
        If mKit.IsDirty Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.CloseConfirm, SIMsgBox.Message_text.Save, "", MsgBoxStyle.YesNo)
            'msg1.ReplacePage = "wfKit_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
            'Session("sender") = "Close"
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
        Else
            If Not Session("Edit") Then mKit.Delete()
            MarkLog(Util.Action.Close, "Kit", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
            'Added by utkarsh on 7-nov-2013 for Kit popup
            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                Session.Remove("mKit")
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If
            'End
            Response.Redirect(Request.QueryString("BackPage"))
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        'Added by Shweta on 9-May-2012 for 09052012-1
        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As New CompanyDetail
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsKitDetail
        Dim mkititems As KitItems

        mKit = Session("mKit")
        mkititems = mKit.KitItems

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
             mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
             mCompanyDetail.WebSite, "Inspection Kit Detail Report", "", "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        myReport = New crKit
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mKit)
        da.Fill(ds, mkititems)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)

        Session("CrystalReport") = myReport
        Dim str As String = "openTranDetail();"
        'str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", str, True)
    End Sub

    Private Sub dgKit_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgKit.PageIndexChanging
        dgKit.PageIndex = e.NewPageIndex
        dgKit.DataSource = mKit.KitItems
        Session("mKit") = mKit
        dgKit.DataBind()
    End Sub
    Private Sub dgKit_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgKit.RowCommand
        Dim idx As Int16
        Select Case e.CommandName
            Case "EditRec"
                If (Not User.IsInRole("KitView") And Not User.IsInRole("KitEdit")) Then
                    setObject()
                    SetSession()
                    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
                    'msg.ReplacePage = "wfKit_Ajax.aspx?MsgResult=0&&BackPage=" & Request.QueryString("BackPage")
                    'Session("sender") = "Authorization"
                    'msg.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                idx = CInt(e.CommandArgument) + dgKit.PageIndex * dgKit.PageSize
                Session("EditItem") = True
                mKit.KitItems.CurrentIndex = idx
                Session("mKit") = mKit
                'AJAX
                txtQuantity.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtQuantity').value,event)")
                DataFieldBindForKitItem(mKit.KitItems(idx).ItemName)
                ItemIDValue.Value = mKit.KitItems(idx).ItemID.ToString
                SetPageForKitItem()
                mdlPopUpKitItem.Show()
                upnlKitItem.Update()
                'End
                'Response.Redirect("wfKitItem_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfKit_Ajax.aspx&SearchText=" & mKit.KitItems.CurrentItem.ItemName)
            Case "DeleteRec"
                If (Not User.IsInRole("KitDelete")) Then
                    setObject()
                    SetSession()
                    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
                    'msg.ReplacePage = "wfKit_Ajax.aspx?MsgResult=0&&BackPage=" & Request.QueryString("BackPage")
                    'Session("sender") = "Authorization"
                    'msg.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
                'msg1.ReplacePage = "wfKit_Ajax.aspx?MsgResult=0&&BackPage=" & Request.QueryString("BackPage")
                'Session("sender") = "Delete"
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
                idx = CInt(e.CommandArgument) + dgKit.PageIndex * dgKit.PageSize
                mKit.KitItems.CurrentIndex = idx
                Session("mKit") = mKit
        End Select
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub btnSearchNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchNew.Click
        If mKit.Type = 2 Then
            mItem = Session("mItem")
            ItemIDToSkipForKitItem = mItem.ID.ToString
        Else
            ItemIDToSkipForKitItem = Guid.Empty.ToString
        End If
        mItemList = ItemList.GetItemList(7, txtSearch.Text, txtSearch.Text, , , , , True, ItemIDToSkipForKitItem:=ItemIDToSkipForKitItem)

        cmbPartNo.DataSource = mItemList
        Session("mItemList") = mItemList
        cmbPartNo.DataBind()
    End Sub
    Private Sub btnCloseKitItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseKitItem.Click
        Session.Remove("mItemList")
        If Not Session("EditItem") Then mKit.KitItems.Remove(mKit.KitItems.CurrentItem)
        Session("EditItem") = False
        MarkLog(Util.Action.Close, "Kit Item", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        mdlPopUpKitItem.Hide()
    End Sub
    Private Sub btnAddKitItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddKitItem.Click
        cmbPartNo.DataSource = mItemList
        cmbPartNo.DataBind()
        cmbPartNo.SelectedValue = ItemIDValue.Value
        If IsValid Then
            If setObjectForKitItem() Then
                Session("EditItem") = False
                'Added by Vikrant
                MarkLog(Util.Action.Save, "Kit Item", "Part No : " + cmbPartNo.SelectedItem.Text + " Qty. : " + txtQuantity.Text, Util.ErrorType.NoError, mItemList(mItemList.CurrentIndex).ID, EventLogID)
                mdlPopUpKitItem.Hide()
                ReloadPageAfterModalPopUpClose()
            End If
        End If
    End Sub
    Private Sub dgKit_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgKit.Sorting
        mKit.KitItems.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mKit") = mKit
        dgKit.DataSource = mKit.KitItems
        dgKit.DataBind()
    End Sub
#End Region


End Class
