'Added by Vikrant

Partial Class wfStore_Ajax
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
    Public mStore As Store
    Public mStoreList As StoreList
    Public mLocationList As LocationList
    Public mVendorList As VendorList
    Public Type As Int16 = 0

    'Added by Vikrant on 20-July-2011
    Dim EventLogID As Guid
    Public mItemTagList As ItemTagList
#End Region

#Region " Business Methods "
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
    Private Sub GetSession()
        'Code Added
        Type = Session("Type")
        'Type = Val(Request.QueryString("Type"))
        'Code Added
        mStore = Session("mStore")
        mStoreList = Session("mStoreList")
        mLocationList = Session("mLocationList")
        mVendorList = Session("mVendorList")
    End Sub
    Private Sub SetSession()
        Session("mStore") = mStore
        Session("mStoreList") = mStoreList
        Session("mLocationList") = mLocationList
        Session("mVendorList") = mVendorList
        Session("Type") = Type    'Added Code
    End Sub
    Private Sub NewRecord()
        mStore = Store.NewStore
        Session("mStore") = mStore
        EnableDisableButtons()  'Code Added
        txtName.Enabled = True
    End Sub
    Private Sub EditRecord(ByVal ID As Guid)
        mStore = Store.GetStore(ID)
        Session("mStore") = mStore
        EnableDisableButtons() 'Code Added
        'New Addition By Yogita on 10-Dec-2007 to solve Bug No:-ST5 given by Pramod
        setFocus(txtName)
        txtName.Enabled = True
    End Sub
    Private Sub DeleteRecord(ByVal ID As Guid)
        If Not ID.Equals(Session("StoreID")) Then
            EditRecord(ID)
            MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        Else
            MSGBoxCtrl.show(MSGBox.Message_title.CurrentlySelected, MSGBox.Message_text.CurrentlySelected, "", MsgBoxStyle.YesNo, "Delete")
        End If
    End Sub
    Public Function SaveAfterNotInUse() As Boolean
        Try
            setObject()
            If mStore.IsValid Then
                mStore.Save()
                If txtName.Enabled = True Then
                    setFocus(txtName)
                End If
                MarkLog(Util.Action.Save, "Store", mStore.Name, Util.ErrorType.HandledError, mStore.ID, EventLogID)
                NewRecord()
                ' GetList()
                DataFieldBind()
                lblTitle.Text = "Store Information [New]"
                'New Addition By Yogita on 10-Dec-2007
                lblResult.Text = "Store List: " & mStoreList.Count & " Record(s) Found."
                upnlTitle.Update()
                upnlStoreDetails.Update()
                upnlGridView.Update()

                Return True
            Else
                cvDate.IsValid = False
                Dim str As String = ""
                For i As Integer = 0 To mStore.GetBrokenRulesCollection.Count - 1
                    str = str + mStore.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
                cvDate.ErrorMessage = str
                cvDate.IsValid = False
                upnlValidationSummary.Update()
                Return False
            End If
        Catch ex As SqlException
            If ex.Number = 8145 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfStore.aspx?BackPage1=&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfStore.aspx?BackPage1=&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfStore.aspx?BackPage1=&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 50000 Then
                MSGBoxCtrl.show("Alert !", ex.Message, "", MsgBoxStyle.OkOnly, "")
            End If
            Return False
        End Try
    End Function
    Public Function Save() As Boolean
        Try
            setObject()
            If mStore.IsValid Then

                If chkNotInUse.Checked = True And txtNotInUseDate.Text <> "" Then
                    MSGBoxCtrl.show("Alert !", "You are about to mark store as Not In Use on " + New SmartDate(txtNotInUseDate.Text).FormattedText + " date.", "Once you mark Not In Use, store will not be available for further transactions after " + New SmartDate(txtNotInUseDate.Text).FormattedText + ". Do you want to continue?", MsgBoxStyle.YesNo, "NotInUse")
                    DataFieldBind()
                    Exit Function
                Else
                    If mStore.IsNew Then Session("PoupupUserMappingwithStore") = True
                    mStore.Save()
                End If
                If txtName.Enabled = True Then
                    setFocus(txtName)
                End If
                MarkLog(Util.Action.Save, "Store", mStore.Name, Util.ErrorType.HandledError, mStore.ID, EventLogID)

                '''--------------------------------------------
                'NewRecord()
                '' GetList()
                'DataFieldBind()
                'lblTitle.Text = "Store Information [New]"
                ''New Addition By Yogita on 10-Dec-2007
                'lblResult.Text = "Store List: " & mStoreList.Count & " Record(s) Found."
                'upnlTitle.Update()
                'upnlStoreDetails.Update()
                'upnlGridView.Update()
                '''----------------------------------------------
                Return True
            Else
                cvDate.IsValid = False
                Dim str As String = ""
                For i As Integer = 0 To mStore.GetBrokenRulesCollection.Count - 1
                    str = str + mStore.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
                cvDate.ErrorMessage = str
                cvDate.IsValid = False
                upnlValidationSummary.Update()
                Return False
            End If
        Catch ex As SqlException
            If ex.Number = 8145 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfStore.aspx?BackPage1=&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfStore.aspx?BackPage1=&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfStore.aspx?BackPage1=&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 50000 Then
                MSGBoxCtrl.show("Alert !", ex.Message, "", MsgBoxStyle.OkOnly, "")
            End If
            Return False
        End Try
    End Function
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0

        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mStore = Session("mStore")
                            Store.DeleteStore(mStore.ID)
                            NewRecord()
                            DataFieldBind()
                            lblResult.Text = "Store List: " & mStoreList.Count & " Record(s) Found."
                            lblTitle.Text = "Store Information [New]"
                            upnlTitle.Update()
                            upnlStoreDetails.Update()
                            upnlGridView.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                                'msg1.ReplacePage = "wfStore.aspx?BackPage1=&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type")
                                'msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                                'msg1.ReplacePage = "wfStore.aspx?BackPage1=&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type")
                                'msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                Dim stringInfo As String = ""
                                If ex.Message.Contains("tabIssue") Then
                                    stringInfo = "Issue"
                                ElseIf ex.Message.Contains("tabReceiptItem") Then
                                    stringInfo = "Receipt Item."
                                ElseIf ex.Message.Contains("tabReceipt") Then
                                    stringInfo = "Receipt."
                                End If
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, stringInfo, MsgBoxStyle.OkOnly, "")
                            End If
                            NewRecord()
                            DataFieldBind()
                            upnlStoreDetails.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "Store", mStore.Name, Util.ErrorType.NoError, mStore.ID, EventLogID)
                            End If
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "NotInUse" Then
                        SaveAfterNotInUse()
                    ElseIf MSGBoxCtrl.Sender = "DeleteStoreTag" Then
                        Try
                            Session("sender") = ""
                            mStore.StoreTags.Remove(mStore.StoreTags(mStore.StoreTags.CurrentIndex))
                            mStore.Save()
                            Session("mStore") = mStore
                            Session("StoreTagEdit") = False
                            DataFieldBindForStoreTag()
                            SetTitleForStoreTag()
                            ClearControls()
                            lnkStoreTagCount_ModalPopupExtender.Show()
                            upnlStoreTag.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "RefDeleteStoreTag")
                            End If
                            Dim mTempStore As Store = Store.GetStore(mStore.ID)
                            dgStoreTagList.DataSource = mTempStore.StoreTags
                            upnlStoreTag.DataBind()
                            Session("mStore") = mTempStore
                            msgCount = ex.Errors.Count
                            lnkStoreTagCount_ModalPopupExtender.Show()
                            upnlStoreTag.Update()
                        Finally
                            If msgCount = 0 Then
                            End If
                        End Try
                    End If

                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Session("sender") = ""
                        NewRecord()
                        DataFieldBind()
                        upnlStoreDetails.Update()
                    End If
                    If MSGBoxCtrl.Sender = "DeleteStoreTag" Or MSGBoxCtrl.Sender = "RefDeleteStoreTag" Then
                        lnkStoreTagCount_ModalPopupExtender.Show()
                        upnlStoreTag.Update()
                    End If
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    'DataFieldBind()
                    'Response.Redirect("wfStore.aspx?MsgResult=0&BackPage1=&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    'DataFieldBind()
                    'Response.Redirect("wfStore.aspx?MsgResult=0&BackPage1=&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
            'Response.Redirect("wfStore.aspx?MsgResult=0&BackPage1=&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfStore_Ajax.aspx?") <= 0 And Val(Request.QueryString("Type")) = 2 Then
            Session.Remove("mStore")
            Session.Remove("mStoreList")
            Session.Remove("mLocationList")
            Session.Remove("mVendorList")
            Session.Remove("Type")
            Session.Remove("mVendor")
            Session.Remove("mCitylist")
            Session.Remove("mCity")
            Session.Remove("New")
        End If
    End Sub
    Private Sub EnableDisableButtons() 'Code Added
        'Store location
        ' imgbtnLocation.Enabled = User.IsInRole("StoreNew") Or User.IsInRole("StoreNew") Or User.IsInRole("StoreNew")
    End Sub
    Private Sub ControlVisibility() 'Added by Saylee on 20-July-2016
        txtNotInUseDate.Enabled = (chkNotInUse.Checked And mStore.IsNew)
    End Sub
    Private Sub ShowStoreTags()
        lnkStoreTagCount_ModalPopupExtender.Show()
        DataFieldBindForStoreTag()
        SetStoreTagTitle()
        upnlStoreTag.Update()
    End Sub

    Private Sub DisableName(ByVal mId As Guid) 'Added by : Shital 19-Jun-2020, ALL16062020
        Dim mTransCountAsPerMasters As TransCountAsPerMasters = TransCountAsPerMasters.GetTransCountAsPerStore(mId)
        If Not mTransCountAsPerMasters Is Nothing Then
            txtName.Enabled = mTransCountAsPerMasters.Count = 0
        End If
    End Sub
#End Region

#Region " DataBinding "
    Private Sub setObject()
        Dim Id As New Guid(cmbLocation.SelectedValue)
        mStore.Name = Trim(txtName.Text)
        mStore.LocationID = Id
        mStore.IsValued = chkIsValued.Checked
        mStore.IsOwnedByCustomer = chkIsOwnedByCustomer.Checked
        mStore.VendorID = IIf(chkIsOwnedByCustomer.Checked, New Guid(cmbVendorList.SelectedValue), Guid.Empty)

        If txtNotInUseDate.Text = "" Then
            mStore.NotInUseDate = System.DBNull.Value
        Else
            mStore.NotInUseDate = txtNotInUseDate.Text.ToString
        End If
        mStore.NotInUse = chkNotInUse.Checked
        Session("mStore") = mStore
    End Sub
    Private Sub DataFieldBind()
        mVendorList = VendorList.GetVendortList(0, , , , , , True, True, False)
        cmbVendorList.DataSource = mVendorList
        Session("mVendorList") = mVendorList
        mLocationList = LocationList.GetLocationList(0, , , , , , True)
        Session("mLocationList") = mLocationList
        cmbLocation.DataSource = mLocationList
        mStoreList = StoreList.GetStoreList(0, "")
        Session("mStoreList") = mStoreList
        dgStore.DataSource = mStoreList

        'cmbLocation.ClearSelection()
        'cmbVendorList.ClearSelection()
        'cmbLocation.SelectedIndex = 0
        'cmbVendorList.SelectedIndex = 0

        'Code Added
        ''cmbLocation.SelectedValue = mStore.LocationID.ToString
        ''cmbVendorList.SelectedValue = mStore.VendorID.ToString
        'Code Added
        If mStore.NotInUseDate.ToString = "" Then
            txtNotInUseDate.Text = ""
        Else
            txtNotInUseDate.Text = Format(CDate(mStore.NotInUseDate), AppSettings("DateFormat"))
        End If
        DataBind()

        If txtNotInUseDate.Text = "" Then
            chkNotInUse.Enabled = True
        Else
            chkNotInUse.Enabled = False
        End If
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValidator As CustomValidator
        CustValidator = CType(s, CustomValidator)
        If CustValidator.ControlToValidate = "txtName" Then
            If txtName.Text = "" Then
                CustValidator.ErrorMessage = "Store Name Required."
                e.IsValid = False
            End If
        ElseIf CustValidator.ControlToValidate = "cmbLocation" Then
            If cmbLocation.SelectedIndex <= 0 Then
                CustValidator.ErrorMessage = "Select Location from the List."
                e.IsValid = False
            End If
        ElseIf CustValidator.ControlToValidate = "cmbVendorList" Then
            If cmbVendorList.SelectedIndex <= 0 And chkIsOwnedByCustomer.Checked Then
                CustValidator.ErrorMessage = "Select Customer from the List."
                e.IsValid = False

            End If

        End If
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        Type = Val(Request.QueryString("Type"))
        'Added by Vikrant on 20-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack Then
            If txtName.Enabled = True Then
                setFocus(txtName)
            End If
            If Session("sender") = "" And Session("New") <> "True" Then
                If Type = 0 Then
                    Dim a As Integer = 0
                End If
                Session("Type") = Type
                'Code Added
                If Type = 2 Then
                    Session("MiddleFrame") = "wfStore_Ajax.aspx?Type=" & Request.QueryString("Type")
                End If
                NewRecord()
            Else
                'Session("New") = ""
            End If
            DataFieldBind()

            'Added by Harsh on 15th July 2024 for FLYPAL 1745
            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "Store") Then

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "Mark As Favourite",
                                                    "MarkAsFavourite();",
                                                    True)

            Else

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "Remove From Favourite",
                                                    "RemoveFromFavourite();",
                                                    True)

            End If

        End If
        'New Addition By Yogita on 10-Dec-2007
        lblResult.Text = "Store List: " & mStoreList.Count & " Record(s) Found."
        'Commented by Yogita
        'btnBackTop.Visible = False
        Session("mStore") = mStore
        'MessageBoxResult()
        ControlVisibility()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("StoreNew") And mStore.IsNew) Or (Not User.IsInRole("StoreEdit") And Not mStore.IsNew) Then
            setObject()
            SetSession()
            MarkLog(Util.Action.Save, "Store", User.Identity.Name & " is not Authorized User to save " & mStore.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            'msg.ReplacePage = "wfStore.aspx?MsgResult=0&BackPage1=&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type")
            'Session("sender") = "Authorization"
            'msg.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If IsValid Then
            If Save() Then
                If Not Session("PoupupUserMappingwithStore") Is Nothing AndAlso CBool(Session("PoupupUserMappingwithStore")) Then
                    Session.Remove("PoupupUserMappingwithStore")
                    Session("StoreIDUserMappingwithStore") = mStore.ID
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToAddUserMappingwithStore", "OpenToAddUserMappingwithStore();", True)
                    Exit Sub
                End If
                NewRecord()
                ' GetList()
                DataFieldBind()
                lblTitle.Text = "Store Information [New]"
                'New Addition By Yogita on 10-Dec-2007
                lblResult.Text = "Store List: " & mStoreList.Count & " Record(s) Found."
                upnlTitle.Update()
                upnlStoreDetails.Update()
                upnlGridView.Update()
            End If

        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub dgStore_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgStore.RowCommand
        Dim Idx As Int32
        Dim ID As New Guid
        Dim mName As String
        Select Case e.CommandName
            Case "EditRec"
                Idx = CInt(e.CommandArgument) + dgStore.PageIndex * dgStore.PageSize
                ID = mStoreList(Idx).ID
                'ID = New Guid(e.CommandArgument.ToString)
                mName = mStoreList(ID).Name

                If (Not User.IsInRole("StoreView") And Not User.IsInRole("StoreEdit")) Then
                    setObject()
                    SetSession()
                    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                    MarkLog(Util.Action.Edit, "Store", User.Identity.Name & " is not Authorized User to edit " & mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'msg.ReplacePage = "wfStore.aspx?MsgResult=0&BackPage1=&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type")
                    'Session("sender") = "Authorization"
                    'msg.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If

                EditRecord(ID)
                DataFieldBind()
                txtName.DataBind()
                cmbLocation.SelectedValue = mStore.LocationID.ToString
                cmbVendorList.SelectedValue = mStore.VendorID.ToString
                DisableName(ID) 'Added by : Shital 19-Jun-2020, ALL16062020
                MarkLog(Util.Action.Edit, "Store", mStore.Name, Util.ErrorType.NoError, mStore.ID, EventLogID)
                If Len(mStore.Name) > 15 Then
                    lblTitle.Text = "Store Information [" & mStore.Name.Substring(0, 15) & "...]"
                Else
                    lblTitle.Text = "Store Information [" & mStore.Name & "]"
                End If
                ControlVisibility()
                'New Addition By Yogita on 10-Dec-2007
                lblResult.Text = "Store List: " & mStoreList.Count & " Record(s) Found."
                upnlStoreDetails.Update()
                upnlTitle.Update()
                upnlGridView.Update()
            Case "DeleteRec"
                Idx = CInt(e.CommandArgument) + dgStore.PageIndex * dgStore.PageSize
                ID = mStoreList(Idx).ID
                mName = mStoreList(ID).Name

                If (Not User.IsInRole("StoreDelete")) Then
                    setObject()
                    SetSession()
                    MarkLog(Util.Action.Delete, "Store", User.Identity.Name & " is not Authorized User to delete " & mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                    'msg.ReplacePage = "wfStore.aspx?MsgResult=0&BackPage1=&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type")
                    'Session("sender") = "Authorization"
                    'msg.Show()
                    'Exit Sub
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                End If
                DeleteRecord(ID)
            Case "StoreTag"
                Idx = CInt(e.CommandArgument) + dgStore.PageIndex * dgStore.PageSize
                ID = mStoreList(Idx).ID
                mStore = Store.GetStore(ID)
                Session("mStore") = mStore
                DataFieldBind()

                ShowStoreTags()
                upnlStoreTag.Update()
        End Select
    End Sub

    Private Sub imgLocation_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgLocation.Click
        Dim str As String

        'Commented by Amrita on 11-Dec-07 for Solving Bug No.LC2 given by Pramod
        'setObject()
        '------------

        Session("mStore") = mStore
        Session("New") = "True"
        If Type = 2 Then
            str = "OpenLocation('wfStoreLocation_Ajax.aspx?BackPage1=Index.aspx&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type") & "');"
        Else
            str = "OpenLocation('wfStoreLocation_Ajax.aspx?BackPage1=wfStore_Ajax.aspx&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type") & "');"
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenLocation", str, True)
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If txtName.Enabled = True Then
            setFocus(txtName)
        End If
        MarkLog(Util.Action.[New], "Store", "", Util.ErrorType.NoError, mStore.ID, EventLogID)
        NewRecord()
        DataFieldBind()
        lblTitle.Text = "Store Information [New]"
        'New Addition By Yogita on 10-Dec-2007
        lblResult.Text = "Store List: " & mStoreList.Count & " Record(s) Found."
        upnlStoreDetails.Update()
        upnlTitle.Update()
        upnlValidationSummary.Update()
        upnlGridView.Update()
    End Sub

    Private Sub ImgVendor_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImgVendor.Click
        If Not (User.IsInRole("StoreNew") And User.IsInRole("StoreEdit") And User.IsInRole("StoreDelete")) Then
            SetSession()
            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            'msg.ReplacePage = "wfStore.aspx?MsgResult=0&BackPage1=&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type")
            'Session("sender") = "Authorization"
            'msg.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        Dim str As String

        'Code Added
        setObject()
        Session("mStore") = mStore
        Session("New") = "True"
        'Code Added

        If Type = 2 Then
            str = "OpenLocation('wfVendorList_Ajax.aspx?BackPage1=Index.aspx&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type") & "');"
        Else
            str = "OpenLocation('wfVendorList_Ajax.aspx?BackPage1=wfStore_Ajax.aspx&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type") & "');"
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenLocation", str, True)
    End Sub
    Private Sub chkIsOwnedByCustomer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsOwnedByCustomer.CheckedChanged
        cmbVendorList.Enabled = (chkIsOwnedByCustomer.Checked)
        'If Not chkIsOwnedByCustomer.Checked Then cmbVendorList.SelectedIndex = 0
        If Not chkIsOwnedByCustomer.Checked Then cmbVendorList.SelectedValue = Guid.Empty.ToString
        If chkIsOwnedByCustomer.Enabled = True Then
            setFocus(chkIsOwnedByCustomer)
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        MarkLog(Util.Action.Close, "Store", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session.Remove("New")
        Session.Remove("mStore")
        If Type = 2 Then
            Session("MiddleFrame") = ""
            Response.Redirect("Dashboard.aspx")
        Else
            Session.Remove("index")
            Response.Redirect(Request.QueryString("GChildPage1") & "?BackPage1=&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&Type=" & Request.QueryString("Type"))
        End If
    End Sub
    Private Sub dgStore_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgStore.Sorting
        mStoreList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mStoreList") = mStoreList
        dgStore.DataSource = mStoreList
        dgStore.DataBind()
    End Sub
    Private Sub dgStore_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgStore.PageIndexChanging
        dgStore.PageIndex = e.NewPageIndex
        dgStore.DataSource = mStoreList
        Session("mStoreList") = mStoreList
        dgStore.DataBind()
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub chkNotInUse_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNotInUse.CheckedChanged
        txtNotInUseDate.Enabled = chkNotInUse.Checked
        If chkNotInUse.Checked = False Then
            txtNotInUseDate.Text = ""
        End If
    End Sub
    Private Sub hdnBtnAddUserMappingwithStore_Click(sender As Object, e As System.EventArgs) Handles hdnBtnAddUserMappingwithStore.Click
        NewRecord()
        DataFieldBind()
        lblTitle.Text = "Store Information [New]"
        lblResult.Text = "Store List: " & mStoreList.Count & " Record(s) Found."
        upnlTitle.Update()
        upnlStoreDetails.Update()
        upnlGridView.Update()
    End Sub

    'Added by Harsh on 15th July 2024 for FLYPAL 1745
    Private Sub MarkFav(sender As Object, e As EventArgs) Handles hdnBtnMarkFavourite.Click

        Try
            MarkFavourite(HttpContext.Current.User.Identity.Name, "Store")
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub RemoveFav(sender As Object, e As EventArgs) Handles hdnBtnRemoveFavourite.Click

        Try
            RemoveFavourite(HttpContext.Current.User.Identity.Name, "Store")
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub
    'End

#End Region

#Region "Tag"
    Private Sub EditRecordStoreTag(ByVal mId As Guid)
        cmbItemTag.SelectedValue = mStore.StoreTags.Item(mId).ItemTagID
    End Sub
    Private Sub DataFieldBindForStoreTag()
        mItemTagList = ItemTagList.GetItemTagList(True)
        cmbItemTag.DataSource = mItemTagList
        cmbItemTag.DataBind()
        Dim mTempStore As Store = Store.GetStore(mStore.ID)
        dgStoreTagList.DataSource = mTempStore.StoreTags
        dgStoreTagList.PageIndex = 0
        Session("mStore") = mTempStore
        upnlStoreTag.DataBind()
    End Sub
    Private Sub SetStoreTagTitle()
        lblResultStoreTag.Text = "Store Tag List : " & mStore.StoreTags.Count & " Record(s) Found."
    End Sub
    Private Sub btnSaveStoreTag_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveStoreTag.Click
        If (Not User.IsInRole("StoreNew") And mStore.IsNew) Or (Not User.IsInRole("StoreEdit") And Not mStore.IsNew) Then
            SetSessionForStoreTag()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Try
            'If CustomValidate1() Then
            If IsValid Then
                If Session("StoreTagEdit") = False Then
                    mStore.StoreTags.Add(ID:=Guid.NewGuid, StoreID:=mStore.ID, ItemTagID:=cmbItemTag.SelectedValue)
                    Session("mStore") = mStore
                    setFocus(cmbItemTag)
                Else
                    setObjectForStoreTag()
                    If Not IsValid Then
                        Exit Sub
                    End If
                    Session("mStore") = mStore
                    setFocus(cmbItemTag)
                    Session("StoreTagEdit") = False
                End If
                Try
                    mStore.Save()
                    DataFieldBindForStoreTag()
                    SetTitleForStoreTag()
                    ClearControls()
                    lnkStoreTagCount_ModalPopupExtender.Show()
                Catch ex As SqlException
                    If ex.Number = 8145 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2627 Then
                        mStore.StoreTags.Remove(mStore.StoreTags.CurrentItem)
                        DataFieldBindForStoreTag()
                        MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "RefDeleteStoreTag")
                    ElseIf ex.Number = 2601 Then
                        mStore.StoreTags.Remove(mStore.StoreTags.CurrentItem)
                        DataFieldBindForStoreTag()
                        MSGBoxCtrl.show(MSGBox.Message_title.Restriction, MSGBox.Message_text.Duplicate, "Store Tag Should Not Be Same For Same ATA.", MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 547 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    End If
                    lnkStoreTagCount_ModalPopupExtender.Show()
                End Try
            Else
                lnkStoreTagCount_ModalPopupExtender.Show()
                upnlStoreTag.Update()
            End If
        Catch ex As Exception
            Throw ex
        Finally

        End Try
    End Sub
    Private Sub SetSessionForStoreTag()
        Session("mStore") = mStore
    End Sub
    Public Function CustomValidate1() As Boolean
        'Dim strMSG As String = ""

        'If IsNumeric(txtSubATACode.Text) Then
        '    If Val(txtSubATACode.Text) < 0 Then
        '        strMSG = "Sub ATA Code should be Numeric." + "<BR>"
        '    ElseIf txtSubATACode.Text = "" Then 'AJAX
        '        strMSG += "Sub ATA Code Required." + "<BR>"
        '    End If
        'Else
        '    If txtSubATACode.Text = "" Then 'AJAX
        '        strMSG += "Sub ATA Code Required." + "<BR>"
        '    Else
        '        strMSG = "Sub ATA Code should be Numeric." + "<BR>"
        '    End If

        'End If

        'If txtSubATAChapter.Text = "" Then 'AJAX
        '    strMSG += "Sub ATA Chapter Required." + "<BR>"
        'ElseIf txtSubATAChapter.Text.Trim.Length > 50 Then
        '    strMSG += "Sub ATA Chapter Should Not Greater than 50 Characters." + "<BR>"
        'End If

        'If Len(txtDescription.Text.Trim) > 1000 Then
        '    strMSG += "Sub ATA Description Should Not Greater than 1000 Characters." + "<BR>"
        'End If
        'upnlStoreTag.Update()
        'If strMSG.Trim <> "" Then
        '    cvDescription.ErrorMessage = strMSG
        '    cvDescription.IsValid = False
        '    Return False
        'End If
        Return True
    End Function
    Private Sub setObjectForStoreTag()
        mStore.StoreTags.Item(mStore.StoreTags.CurrentIndex).ItemTagID = cmbItemTag.SelectedValue
    End Sub
    Private Sub SetTitleForStoreTag()
        lblResultStoreTag.Text = "Store Tag List : " & mStore.StoreTags.Count & " Record(s) Found."
    End Sub
    Private Sub dgStoreTagList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgStoreTagList.RowCommand
        Dim Idx As Int32
        Select Case e.CommandName
            Case "DeleteRec"
                lnkStoreTagCount_ModalPopupExtender.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteStoreTag")
                Idx = CInt(e.CommandArgument) + dgStoreTagList.PageIndex * dgStoreTagList.PageSize
                mStore.StoreTags.CurrentIndex = Idx
                Session("mStore") = mStore
            Case "EditRec"
                Idx = CInt(e.CommandArgument) + dgStoreTagList.PageIndex * dgStoreTagList.PageSize
                mStore.StoreTags.CurrentIndex = Idx
                If Len(mStore.StoreTags.Item(Idx).ItemTagName) > 15 Then
                    lblTitleStoreTag.Text = "Store Tag [" & mStore.StoreTags.Item(Idx).ItemTagName.Substring(0, 15) & "...]"
                Else
                    lblTitleStoreTag.Text = "Store Tag [" & mStore.StoreTags.Item(Idx).ItemTagName & "]"
                End If
                Dim mID As Guid = mStore.StoreTags(Idx).ID
                EditRecordStoreTag(mID)
                setFocus(cmbItemTag)
                dgStoreTagList.DataSource = mStore.StoreTags
                upnlStoreTag.DataBind()
                Session("StoreTagEdit") = True
                Session("mStore") = mStore
                lnkStoreTagCount_ModalPopupExtender.Show()
        End Select
    End Sub
    Private Sub dgStoreTagList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgStoreTagList.PageIndexChanging
        dgStoreTagList.PageIndex = e.NewPageIndex
        dgStoreTagList.DataSource = mStore.StoreTags
        Session("mStore") = mStore
        dgStoreTagList.DataBind()
    End Sub
    Private Sub btnCloseStoreTag_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseStoreTag.Click
        lnkStoreTagCount_ModalPopupExtender.Hide()
        ClearControls()
        DataFieldBind()
        'If mStore.StoreTags.Count > 0 Then
        '    lnkSubATACount.Visible = True
        '    lblSubATA.Visible = True
        '    lnkSubATACount.Text = "( " + mStore.StoreTags.Count.ToString + " ) Records"
        'Else
        '    lnkSubATACount.Visible = False
        '    lblSubATA.Visible = False
        'End If
        upnlGridView.Update()
        upnlStoreTag.Update()
        cmbItemTag.SelectedValue = 0
        'NewRecord()
    End Sub
    Private Sub ClearControls()
        cmbItemTag.SelectedValue = 0
        lblTitleStoreTag.Text = "Store Tag [New]"
    End Sub
#End Region


End Class
