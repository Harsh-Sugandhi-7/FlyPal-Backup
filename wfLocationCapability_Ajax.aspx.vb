Public Class wfLocationCapability_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mLocationCapability As LocationCapability
    Public mLocationCapabilityList As LocationCapabilityList
    Public mCapabilityList As CapabilityList
    Public mLocationList As LocationList
    Public mModelList As ModelList
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mLocationCapability = CType(Session("mLocationCapability"), LocationCapability)
        mLocationCapabilityList = CType(Session("mLocationCapabilityList"), LocationCapabilityList)
    End Sub
    Private Sub SetSession()
        Session("mLocationCapability") = mLocationCapability
        Session("mLocationCapabilityList") = mLocationCapabilityList
    End Sub
    Private Sub NewRecord()
        mLocationCapability = LocationCapability.NewLocationCapability()
        Session("mLocationCapability") = mLocationCapability
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mLocationCapability = LocationCapability.GetLocationCapability(mId)
        Session("mLocationCapability") = mLocationCapability
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")

        mLocationCapability = LocationCapability.GetLocationCapability(mId)
        Session("mLocationCapability") = mLocationCapability
    End Sub
    Private Sub setObject()
        mLocationCapability.LocationID = New Guid(cmbLocation.SelectedValue)
        mLocationCapability.LocationName = cmbLocation.SelectedItem.Text
        mLocationCapability.CapabilityID = cmbCapability.SelectedValue
        mLocationCapability.CapabilityName = cmbCapability.SelectedItem.Text
        mLocationCapability.ModelID = New Guid(cmbModel.SelectedValue)
        mLocationCapability.ModelName = cmbModel.SelectedItem.Text
        mLocationCapability.Remark = txtRemark.Text.Trim
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim LocationCapabilityDet As String = String.Empty
                        Try
                            Session("sender") = ""
                            mLocationCapability = CType(Session("mLocationCapability"), LocationCapability)

                            LocationCapabilityDet = mLocationCapability.LocationName + " Capability : " + mLocationCapability.CapabilityName
                            LocationCapability.DeleteLocationCapability(mLocationCapability.ID)
                            NewRecord()
                            DataFieldBind()
                            SetTitle()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, "", MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "LocationCapability", "Can't delete : " & mLocationCapability.LocationName & " is Currently in use", Util.ErrorType.NoError, mLocationCapability.ID, EventLogID)
                            End If
                            NewRecord()
                            DataFieldBind()
                            SetTitle()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "LocationCapability", LocationCapabilityDet, Util.ErrorType.NoError, mLocationCapability.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        NewRecord()
                        DataFieldBind()
                        SetTitle()
                    End If
                    Session("sender") = ""
                    SetTitle()
                Case MsgBoxResult.Ok
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    DataFieldBind()

            End Select
        ElseIf Result1 = -1 Then
            DataFieldBind()

        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            'Session("sender") = ""
            DataFieldBind()
        End If
        upnlLocationCapability.Update()
    End Sub
    Private Sub SetTitle()
        If mLocationCapability.IsNew Then
            lbltitle.Text = "Location Capability [New]"
        Else
            If Len(mLocationCapability.LocationName) > 15 Then
                lbltitle.Text = "Location Capability [" & mLocationCapability.LocationName.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Location Capability [" & mLocationCapability.LocationName & "]"
            End If
        End If
        lblResult.Text = "Location Capability List: " & mLocationCapabilityList.Count & " Record(s) Found."
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mLocationCapabilityList = LocationCapabilityList.GetLocationCapabilityList()
        Session("mLocationCapabilityList") = mLocationCapabilityList
        dgLocationCapability.DataSource = mLocationCapabilityList
        dgLocationCapability.DataBind() '''''DataBind()

        txtRemark.Text = mLocationCapability.Remark
        mCapabilityList = CapabilityList.GetCapabilityList("(SELECT)")
        cmbCapability.DataSource = mCapabilityList
       
        mLocationList = LocationList.GetLocationList(0, , , , , , True)
        cmbLocation.DataSource = mLocationList

        mModelList = ModelList.GetModelList(0, "", , , "(SELECT)")
        cmbModel.DataSource = mModelList
        DataBind()
        upnlLocationCapability.Update()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValidator As CustomValidator
        CustValidator = CType(s, CustomValidator)
        Dim strMsg As String = ""
        setObject()
        If Not mLocationCapability.IsValid Then
            For i As Integer = 0 To mLocationCapability.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mLocationCapability.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        If strMsg.Trim <> "" Then
            CustValidator.ErrorMessage = strMsg
            e.IsValid = False
        End If
    End Sub
    Public Function CustomValidate1() As Boolean
        Dim strMsg As String = ""
        setObject()
        If Not mLocationCapability.IsValid Then
            For i As Integer = 0 To mLocationCapability.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mLocationCapability.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If

        If strMsg.Trim <> "" Then
            cvDesc.ErrorMessage = strMsg
            cvDesc.IsValid = False
            Return False
        End If
        Return True
    End Function
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack Then
            NewRecord()
            DataFieldBind()
            SetTitle()
        End If

    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "LocationCapability", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("sender") = ""
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            Session.Remove("mLocationCapability")
            Session.Remove("mLocationCapabilityList")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        ''If (Not User.IsInRole("LocationCapabilityNew") And mLocationCapability.IsNew) Or (Not User.IsInRole("LocationCapabilityEdit") And Not mLocationCapability.IsNew) Then
        ''    setObject()
        ''    SetSession()
        ''    MarkLog(Util.Action.Save, "LocationCapability", User.Identity.Name & " is not Authorized User to save " & mLocationCapability.Description, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
        ''    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
        ''    Exit Sub
        ''End If
        If Not IsValid Then Exit Sub

        If CustomValidate1() Then
            Try
                setObject()
                mLocationCapability.Save()
                MarkLog(Util.Action.Save, "LocationCapability", mLocationCapability.LocationName, Util.ErrorType.NoError, mLocationCapability.ID, EventLogID)
                NewRecord()
                DataFieldBind()
                SetSession()
                SetTitle()
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Or ex.Number = 2601 Then
                    If InStr(ex.Message, "UK_tabLocationCapability", CompareMethod.Text) Then
                        MSGBoxCtrl.show("Save Error!", "Duplicate Record", "You are trying to add duplicate.", MsgBoxStyle.OkOnly, "")
                    End If
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.DatabaseException, MSGBox.Message_text.DatabaseException, ex.Message, MsgBoxStyle.OkOnly, "")
                End If
            End Try
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub dgLocationCapability_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLocationCapability.RowCommand
        Dim mId As Guid
        Select Case e.CommandName
            Case "ViewRec"
                mId = New Guid(e.CommandArgument.ToString)
                'Dim mName As String = mLocationCapabilityList(Idx).Description
                'If (Not User.IsInRole("LocationCapabilityView") And Not User.IsInRole("LocationCapabilityEdit")) Then
                '    setObject()
                '    SetSession()
                '    MarkLog(Util.Action.Edit, "LocationCapability", User.Identity.Name & " is not Authorized User to Edit " & mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                '    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                '    Exit Sub
                'End If
                EditRecord(mId)
                cmbLocation.SelectedValue = mLocationCapability.LocationID.ToString
                cmbCapability.SelectedValue = mLocationCapability.CapabilityID
                cmbModel.SelectedValue = mLocationCapability.ModelID.ToString
                txtRemark.Text = mLocationCapability.Remark

                SetTitle()

                MarkLog(Util.Action.Edit, "LocationCapability", mLocationCapability.LocationName, Util.ErrorType.NoError, mLocationCapability.ID, EventLogID)
                upnlLocationCapability.Update()
            Case "DeleteRec"
                'Idx = CInt(e.CommandArgument) + dgLocationCapability.PageIndex * dgLocationCapability.PageSize
                mId = New Guid(e.CommandArgument.ToString)
                'Dim mName As String = mLocationCapabilityList(Idx).Description
                'If (Not User.IsInRole("LocationCapabilityDelete")) Then
                '    setObject()
                '    SetSession()
                '    MarkLog(Util.Action.Delete, "LocationCapability", User.Identity.Name & " is not Authorized User to Delete " & mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                '    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                '    Exit Sub
                'End If
                DeleteRecord(mId)
        End Select
    End Sub
    Private Sub dgLocationCapability_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLocationCapability.PageIndexChanging
        dgLocationCapability.PageIndex = e.NewPageIndex
        dgLocationCapability.DataSource = mLocationCapabilityList
        Session("mLocationCapabilityList") = mLocationCapabilityList
        dgLocationCapability.DataBind()
    End Sub
    Private Sub dgLocationCapability_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgLocationCapability.Sorting
        mLocationCapabilityList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mLocationCapabilityList") = mLocationCapabilityList
        dgLocationCapability.DataSource = mLocationCapabilityList
        dgLocationCapability.DataBind()
    End Sub
    Private Sub btnPrintTop_Click(sender As Object, e As System.EventArgs) Handles btnPrintTop.Click, btnPrintBottom.Click
        Dim mrptLocationwiseCapabilities As rptLocationwiseCapabilities
        mrptLocationwiseCapabilities = rptLocationwiseCapabilities.GetLocationwiseCapabilities
        If mrptLocationwiseCapabilities.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim mCompanyDetail As New CompanyDetail
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                                     mCompanyDetail.WebSite, "Locationwise Capabilities", SearchStr1:="", SearchStr2:="", SearchStr3:="", SearchStr4:="", SearchStr5:="", _
                                     ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), SearchStr6:="", SearchStr7:="", SearchStr8:="", SearchStr9:="")

        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsrptLocationwiseCapabilities
        Dim rptReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        rptReport = New crptLocationwiseCapabilities
        da.Fill(ds, mrptLocationwiseCapabilities)
        da.Fill(ds, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        rptReport.SetDataSource(ds)
        Session("CrystalReport") = rptReport

        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        NewRecord()
        MarkLog(Util.Action.[New], "AccountHead", "", Util.ErrorType.NoError, mLocationCapability.ID, EventLogID)
        DataFieldBind()
        SetTitle()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

    
End Class