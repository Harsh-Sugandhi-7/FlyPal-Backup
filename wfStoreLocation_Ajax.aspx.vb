'Added by Prashant

Public Class wfStoreLocation_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mStoreLocation As StoreLocation
    Public mLocationList As LocationList
    Public mCityInvList As CityInvList
    Public mCityInv As CityInv
    Public mState As State
    Dim EventLogID As Guid


#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mStoreLocation = Session("mStoreLocation")
        mLocationList = Session("mLocationList")
        mCityInvList = Session("mCityInvList")
    End Sub
    Private Sub SetSession()
        Session("mStoreLocation") = mStoreLocation
        Session("mLocationList") = mLocationList
        Session("mCityInvList") = mCityInvList
    End Sub
    Private Sub NewRecord()
        mStoreLocation = StoreLocation.NewLocation
        Session("mStoreLocation") = mStoreLocation
        LocationTitle()
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mStoreLocation = StoreLocation.GetLocation(mId)
        Session("mStoreLocation") = mStoreLocation
        setFocus(txtStationName)
        LocationTitle()
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        mStoreLocation = StoreLocation.GetLocation(mId)
        Session("mStoreLocation") = mStoreLocation
        GridBind()
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
    End Sub
    Private Sub setObject()
        mStoreLocation.Name = txtStationName.Text
        mStoreLocation.Address = txtAddress.Text
        mStoreLocation.CityID = New Guid(cmbCity.SelectedValue)
        mStoreLocation.Phone1 = txtPhone1.Text
        mStoreLocation.Phone2 = txtPhone2.Text
        mStoreLocation.Phone3 = txtPhone3.Text
        mStoreLocation.Fax = txtFax.Text
        mStoreLocation.Email = txtEmail.Text
        mStoreLocation.ContactPerson = txtContactPerson.Text
        Session("mStoreLocation") = mStoreLocation
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            mStoreLocation = Session("mStoreLocation")
                            StoreLocation.DeleteLocation(mStoreLocation.ID)
                            NewRecord()
                            DataFieldBind()
                            upnlSotreLocationInformation.Update()
                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                Dim stringInfo As String = ""
                                If ex.Message.Contains("tabLocationCapability") Then
                                    stringInfo = "Location Capability."
                                ElseIf ex.Message.Contains("tabCustomerContractTask") Then
                                    stringInfo = "Customer Contract Task."
                                ElseIf ex.Message.Contains("tabReq") Then
                                    stringInfo = "Requisition."
                                ElseIf ex.Message.Contains("tabCalloutLocation") Then
                                    stringInfo = "Callout Location."
                                ElseIf ex.Message.Contains("tabLineMaintInvoice") Then
                                    stringInfo = "Line Maint Invoice."
                                ElseIf ex.Message.Contains("tabLineMaintOrder") Then
                                    stringInfo = "Line Maint Order."
                                ElseIf ex.Message.Contains("tabRequisition") Then
                                    stringInfo = "Requisition."
                                ElseIf ex.Message.Contains("tabStore") Then
                                    stringInfo = "Store."
                                ElseIf ex.Message.Contains("tabWOLocation") Then
                                    stringInfo = "WO Location."
                                ElseIf ex.Message.Contains("tabEmployee") Then
                                    stringInfo = "Employee"

                                End If
                                ''MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly, "")
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, stringInfo, MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                        Finally
                            MarkLog(Util.Action.Delete, "Station", mStoreLocation.Name, Util.ErrorType.NoError, mStoreLocation.ID, EventLogID)
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        DataFieldBind()
                    End If
                    If MSGBoxCtrl.Sender = "Delete" Then
                        NewRecord()
                        DataFieldBind()
                        upnlSotreLocationInformation.Update()
                    End If
                Case MsgBoxResult.Ok
                    DataFieldBind()
            End Select
        End If
    End Sub
    Private Sub LocationTitle()
        If mStoreLocation.IsNew Then
            lblStoreLocationTitle.Text = "Location [New]"
        Else
            If Len(mStoreLocation.Name) > 15 Then
                lblStoreLocationTitle.Text = "Location [" & mStoreLocation.Name.Substring(0, 15) & "...]"
            Else
                lblStoreLocationTitle.Text = "Location [" & mStoreLocation.Name & "]"
            End If
        End If
        upnlStoreLocationValidation.Update()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub SetCity()
        If mStoreLocation.IsNew Then
            txtState.Text = mCityInvList.Item(cmbCity.SelectedIndex).State.ToString
            txtCountry.Text = mCityInvList.Item(cmbCity.SelectedIndex).Country.ToString
        Else
            If mStoreLocation.CityID.Equals(Guid.Empty) Then
                txtState.Text = ""
                txtCountry.Text = ""
            Else
                mCityInv = CityInv.GetCity(mStoreLocation.CityID)
                cmbCity.SelectedValue = mStoreLocation.CityID.ToString
                mState = State.GetState(mCityInv.stateID)
                txtState.Text = mState.Name
                txtCountry.Text = mState.CountryName
            End If

        End If
    End Sub
    'Private Sub DataFieldBind()
    '    mLocationList = LocationList.GetLocationList(0)
    '    dgLocation.DataSource = mLocationList
    '    Session("mLocationList") = mLocationList
    '    mCityInvList = CityInvList.GetCityList(0, , , True)
    '    cmbCity.DataSource = mCityInvList
    '    Session("mCityInvList") = mCityInvList
    '    '=========
    '    '=========
    '    If Not mCityInvList.Contains(mStoreLocation.CityID) Then
    '        mStoreLocation.CityID = Guid.Empty
    '    End If
    '    DataBind()
    '    SetCity()
    'End Sub
    Private Sub GridBind()
        dgLocation.DataSource = mLocationList
        dgLocation.DataBind()
        upnlSotoreLocationGridView.Update()
    End Sub
    Private Sub DataFieldBind()
        mLocationList = LocationList.GetLocationList(0)
        dgLocation.DataSource = mLocationList
        Session("mLocationList") = mLocationList
        mCityInvList = CityInvList.GetCityList(0, , , True)
        cmbCity.DataSource = mCityInvList
        Session("mCityInvList") = mCityInvList
        cmbCity.DataBind()
        If Not mCityInvList.Contains(mStoreLocation.CityID) Then
            mStoreLocation.CityID = Guid.Empty
        End If
        txtStationName.DataBind()
        txtAddress.DataBind()
        txtState.DataBind()
        txtCountry.DataBind()
        txtPhone1.DataBind()
        txtPhone2.DataBind()
        txtPhone3.DataBind()
        txtEmail.DataBind()
        txtFax.DataBind()
        txtContactPerson.DataBind()
        GridBind()
        lblStoreLocationResult.Text = "Location List: " & mLocationList.Count & " Record(s) Found."
        SetCity()
    End Sub
    Public Sub Customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator
        CustValid = CType(s, CustomValidator)
        If CustValid.ControlToValidate = "txtStationName" Then
            If Len(Trim(txtStationName.Text)) > 100 Then
                CustValid.ErrorMessage = " Name is too long "
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
        If CustValid.ControlToValidate = "txtAddress" Then
            If Len(txtAddress.Text) > 500 Then
                CustValid.ErrorMessage = " Address is too long "
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
        If CustValid.ControlToValidate = "cmbCity" Then
            If cmbCity.SelectedIndex <= 0 Then
                CustValid.ErrorMessage = "Please select the City "
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack And Session("sender") = "" Then
            If txtStationName.Enabled = True Then
                setFocus(txtStationName)
            End If
            NewRecord()
            DataFieldBind()
        ElseIf CType(Session("sender"), String) = "CityList" Then
            DataFieldBind()
            Session("sender") = ""
        End If
        lblStoreLocationResult.Text = "Location List: " & mLocationList.Count & " Record(s) Found."
    End Sub
    Private Sub btnStoreLocationSaveTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStoreLocationSaveBottom.Click
        Try
            Page.Validate("a")
            If IsValid Then
                setObject()
                mStoreLocation.Save()
                If txtStationName.Enabled = True Then
                    setFocus(txtStationName)
                End If
                MarkLog(Util.Action.Save, "Station", mStoreLocation.Name, Util.ErrorType.HandledError, mStoreLocation.ID, EventLogID)
                mStoreLocation = StoreLocation.NewLocation
                DataFieldBind()
                SetSession()
                LocationTitle()
                lblStoreLocationResult.Text = "Location List: " & mLocationList.Count & " Record(s) Found."
                upnlSotreLocationInformation.Update()
            Else
                GridBind()
                upnlStoreLocationValidation.Update()
                Exit Sub
            End If
        Catch ex As SqlException
            GridBind()
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        End Try
    End Sub
    Private Sub dgLocation_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLocation.RowCommand
        Select Case e.CommandName
            Case "EditView"
                'Dim index As Integer = CInt(e.CommandArgument) + dgLocation.PageIndex * dgLocation.PageSize
                'Dim mId As Guid = mLocationList(index).ID 'New Guid(dgLocation.Rows(index).Cells(0).Text)
                Dim mId As Guid = New Guid(e.CommandArgument.ToString)
                EditRecord(mId)
                txtStationName.DataBind()
                txtAddress.DataBind()
                cmbCity.SelectedValue = mStoreLocation.CityID.ToString
                Dim indx As Integer
                indx = cmbCity.SelectedIndex
                txtState.Text = mCityInvList.Item(indx).State.ToString
                txtCountry.Text = mCityInvList.Item(indx).Country.ToString
                txtPhone1.DataBind()
                txtPhone2.DataBind()
                txtPhone3.DataBind()
                txtEmail.DataBind()
                txtFax.DataBind()
                txtContactPerson.DataBind()
                MarkLog(Util.Action.Edit, "Station", mStoreLocation.Name, Util.ErrorType.NoError, mStoreLocation.ID, EventLogID)
                lblStoreLocationResult.Text = "Location List: " & mLocationList.Count & " Record(s) Found."
                LocationTitle()
                '  GridBind()
                upnlStoreLocationValidation.Update()
                upnlSotreLocationInformation.Update()
                upnlSotoreLocationGridView.Update()
                upnlCity.Update()
            Case "Remove"
                ' Dim index As Integer = CInt(e.CommandArgument) + dgLocation.PageIndex * dgLocation.PageSize
                'Dim mId As Guid = mLocationList(index).ID
                Dim mId As Guid = New Guid(e.CommandArgument.ToString)
                EditRecord(mId)
                upnlStoreLocationValidation.Update()
                DeleteRecord(mId)
        End Select
    End Sub
    Private Sub btnStoreLocationCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStoreLocationCloseBottom.Click
        Session("sender") = ""
        MarkLog(Util.Action.Close, "Station", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
    End Sub
    Private Sub btnStoreLocationNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStoreLocationNew.Click
        If txtStationName.Enabled = True Then
            setFocus(txtStationName)
        End If
        MarkLog(Util.Action.[New], "Station", "", Util.ErrorType.NoError, mStoreLocation.ID, EventLogID)
        NewRecord()
        txtStationName.Text = ""
        DataFieldBind()
        lblStoreLocationResult.Text = "Location List: " & mLocationList.Count & " Record(s) Found."
        upnlStoreLocationValidation.Update()
        upnlSotreLocationInformation.Update()
        upnlSotoreLocationGridView.Update()
    End Sub
    'Private Sub ImgbtnCity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImgbtnCity.Click
    '    setObject()
    '    SetSession()
    '    Response.Redirect("wfCityInv_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&Type=" & Request.QueryString("Type") & "&BackPage3=wfStoreLocation_Ajax.aspx")
    'End Sub
    Private Sub imgbtnCity_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnCity.Click
        setObject() 'Added Code
        SetSession()
    End Sub
    Private Sub cmbCity_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCity.SelectedIndexChanged
        Dim indx As Integer
        indx = cmbCity.SelectedIndex
        txtState.Text = mCityInvList.Item(indx).State.ToString
        txtCountry.Text = mCityInvList.Item(indx).Country.ToString
        setFocus(cmbCity)
    End Sub
    Private Sub dgLocation_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLocation.PageIndexChanging
        dgLocation.PageIndex = e.NewPageIndex
        dgLocation.DataSource = mLocationList
        Session("mLocationList") = mLocationList
        GridBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub hdnimgBtnCity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnCity.Click
        mCityInvList = CityInvList.GetCityList(0, , , True)
        cmbCity.DataSource = mCityInvList
        cmbCity.DataBind()
        Session("mCityInvList") = mCityInvList
        SetCity()
        upnlCity.Update()
    End Sub
#End Region

End Class