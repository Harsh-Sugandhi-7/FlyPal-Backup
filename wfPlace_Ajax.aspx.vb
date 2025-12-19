'Added by Prashant

Public Class wfPlace_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mPlace As Place
    Public mPlaceList As PlaceList
    Public mCityList As CityList
    Dim EventLogID As Guid
    Dim CityID As Guid = Guid.Empty
    Dim mCompanyDetail As New CompanyDetail
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mPlace = CType(Session("mPlace"), Place)
        mPlaceList = CType(Session("mPlaceList"), PlaceList)
        mCityList = CType(Session("mCityList"), CityList)
        mCompanyDetail = Session("mCompanyDetail")
    End Sub
    Private Sub SetSession()
        Session("mPlace") = mPlace
        Session("mPlaceList") = mPlaceList
        Session("mCityList") = mCityList
        Session("mCompanyDetail") = mCompanyDetail
    End Sub
    Private Sub MakeControlsBlank()
        txtCode.Text = ""
        txtName.Text = ""
        cmbCity.DataSource = mCityList
        cmbCity.DataBind()
        cmbCity.SelectedIndex = 0
        txtICAO.Text = ""  'Ajay 27-Des-2022
    End Sub
    Private Sub NewRecord()
        mPlace = Place.NewPlace(Guid.NewGuid)
        Session("mPlace") = mPlace
        SetTitle()
        txtName.Enabled = True
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mPlace = Place.GetPlace(mId)
        Session("mPlace") = mPlace
        SetTitle()
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        EditRecord(mId)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        GridBind()
    End Sub
    Private Sub setObject()
        mPlace.Code = Trim(txtCode.Text)
        mPlace.Name = Trim(txtName.Text)
        If CityValue.Value = "" Then

        Else
            mPlace.CityID = New Guid(CityValue.Value.ToString)
        End If

        mPlace.ICAO = Trim(txtICAO.Text)
        Session("mPlace") = mPlace
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
                            'mPlace = Session("mPlace")
                            Place.DeletePlace(mPlace.ID)
                            MarkLog(Util.Action.Delete, "Place", mPlace.Name, Util.ErrorType.NoError, mPlace.ID, EventLogID)
                            'DataFieldBind()
                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                NewRecord()
                                DataFieldBind()
                                SetGrid()
                                MakeControlsBlank()
                                upnlPlaceDetails.Update()
                                Exit Sub
                            End If
                        Finally
                            NewRecord()
                            DataFieldBind()
                            SetGrid()
                            MakeControlsBlank()
                            upnlPlaceDetails.Update()
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        DataFieldBind()
                        SetGrid()
                    End If
                    If MSGBoxCtrl.Sender = "Delete" Then
                        MakeControlsBlank()
                        NewRecord()
                        DataFieldBind()
                        SetGrid()
                        upnlPlaceDetails.Update()
                        upnlGrid.Update()
                    End If
                Case MsgBoxResult.Ok
                    DataFieldBind()
                    SetGrid()
                    upnlPlaceDetails.Update()
            End Select
        End If
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfPlace_Ajax.aspx?" Then
            Session.Remove("mPlace")
            Session.Remove("mPlaceList")
            Session.Remove("mCity")
        End If
    End Sub
    Private Sub SetTitle()
        If mPlace.IsNew = True Then
            lbltitle.Text = "Place [New]"
        Else
            If Len(mPlace.Name) > 15 Then
                lbltitle.Text = "Place [" & mPlace.Name.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Place [" & mPlace.Name & "]"
            End If
        End If
        'lblResult.Text = "Place List: " & mPlaceList.Count & " Record(s) Found."
        upnlTitle.Update()
    End Sub
    Private Sub DisableName(ByVal mId As Guid) 'Added by : Shital 19-Jun-2020, ALL16062020
        Dim mTransCountAsPerMasters As TransCountAsPerMasters = TransCountAsPerMasters.GetTransCountAsPerPlace(mId)
        If Not mTransCountAsPerMasters Is Nothing Then
            txtName.Enabled = mTransCountAsPerMasters.Count = 0
        End If
    End Sub
    Private Sub SetGrid()
        Dim IsSyncFromCRS As Boolean
        For j As Integer = 0 To dgGridView.Rows.Count - 1
            IsSyncFromCRS = CType(Me.dgGridView.Rows(j).Cells(7).Text, Boolean)

            If IsSyncFromCRS = True Then
                dgGridView.Rows(j).Cells(6).Enabled = False
                'dgGridView.Rows(j).Cells(7).Enabled = False

            End If
        Next

        If mCompanyDetail Is Nothing Then
            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        End If
        btnAdd.Enabled = Not mCompanyDetail.IsSyncApplication
        btnSave.Enabled = Not mCompanyDetail.IsSyncApplication
    End Sub
#End Region

#Region " Data Binding "
    Private Sub GridBind()
        dgGridView.DataSource = mPlaceList
        dgGridView.DataBind()
        SetGrid()
        upnlGrid.Update()
    End Sub
    Private Sub DataFieldBind()
        mCityList = CityList.GetCityList("", "(SELECT)")
        Session("mCityList") = mCityList
        cmbCity.DataSource = mCityList
        cmbCity.DataBind()
        cmbSearchCity.DataSource = mCityList
        cmbSearchCity.DataBind()

        If AppSettings("ClientCode") = "Deccan" Then
            mPlaceList = PlaceList.GetPlaceList("", "", Show100Records:=True)
        Else
            mPlaceList = PlaceList.GetPlaceList("", "")
        End If

        Session("mPlaceList") = mPlaceList
        dgGridView.DataSource = mPlaceList
        cmbCity.SelectedValue = mPlace.CityID.ToString
        cmbCity.DataBind()
        txtCode.DataBind()
        txtName.DataBind()
        txtICAO.DataBind() 'Ajay 27-Des-2022
        GridBind()
        lblResult.Text = "As per criteria " & mPlaceList.Count & " Record(s) Found."
    End Sub
    'Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
    '    Dim custValidator As CustomValidator
    '    custValidator = CType(s, CustomValidator)
    '    If custValidator.ControlToValidate = "cmbCity" Then
    '        If cmbCity.SelectedIndex <= 0 Then
    '            custValidator.ErrorMessage = "Select city from the list."
    '            e.IsValid = False
    '        End If
    '    End If
    'End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            If txtCode.Enabled = True Then
                setFocus(txtCode)
            End If
            If IsNothing(Request.QueryString("BackPage1")) Or Request.QueryString("BackPage1") = "" Then
                Session("MiddleFrame") = "wfPlace_Ajax.aspx?"
            End If
            NewRecord()
            DataFieldBind()
            SetGrid()
        End If
    End Sub
    'Private Sub imgbtnCity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgbtnCity.Click
    '    setObject()
    '    Dim str As String
    '    If IsNothing(Request.QueryString("BackPage1")) Or Request.QueryString("BackPage1") = "" Then
    '        str = "<script language='javascript'>openledgersame('wfCityMain_Ajax.aspx?ChildPage1=Index.aspx&Type=1');</script>"
    '    Else
    '        str = "<script language='javascript'>openledgersame('wfCityMain_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage1=wfPlace-Ajax.aspx&Type=1');</script>"
    '    End If
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, False)
    'End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("PlaceNew") And mPlace.IsNew) Or (Not User.IsInRole("PlaceEdit") And Not mPlace.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If Not IsValid Then
            upnlTitle.Update()
            GridBind()
            Exit Sub
        End If
        Try
            setObject()
            mPlace.Save()
            If txtCode.Enabled = True Then
                setFocus(txtCode)
            End If
            MarkLog(Util.Action.Save, "Place", mPlace.Name, Util.ErrorType.HandledError, mPlace.ID, EventLogID)
            cmbCity.DataSource = mCityList
            cmbSearchCity.DataSource = mCityList
            cmbCity.SelectedValue = mPlace.CityID.ToString
            'GridBind()
            NewRecord()
            DataFieldBind()
            SetGrid()
            SetSession()
            CityValue.Dispose()
        Catch ex As SqlException
            GridBind()
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            DataFieldBind()
            SetGrid()
        End Try
    End Sub
    Private Sub dgGridView_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgGridView.PageIndexChanging
        dgGridView.PageIndex = e.NewPageIndex
        dgGridView.DataSource = mPlaceList
        Session("mPlaceList") = mPlaceList
        GridBind()
    End Sub
    Private Sub dgGridView_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgGridView.RowCommand
        Select Case e.CommandName
            Case "EditView"
                If (Not User.IsInRole("PlaceView") And Not User.IsInRole("PlaceEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                Dim index As Integer = CInt(e.CommandArgument) 'CInt(e.CommandArgument) + dgGridView.PageIndex * dgGridView.PageSize
                Dim mID As Guid = mPlaceList(index).ID
                Dim mCityID As Guid = mPlaceList(index).CityID
                Dim mName As String = mPlaceList(index).Name
                EditRecord(mID)
                setFocus(txtCode)
                txtCode.DataBind()
                txtName.DataBind()
                txtICAO.DataBind() 'Ajay 27-Des-2022

                cmbCity.DataSource = mCityList
                cmbCity.DataBind()
                cmbCity.SelectedValue = mCityID.ToString
                Session("mCityID") = mCityID
                upnlPlaceDetails.Update()
                GridBind()
                DisableName(mID) 'Added by : Shital 19-Jun-2020, ALL16062020
                MarkLog(Util.Action.Edit, "Place", mPlace.Name, Util.ErrorType.NoError, mPlace.ID, EventLogID)
            Case "Remove"
                If (Not User.IsInRole("PlaceDelete")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                Dim index As Integer = CInt(e.CommandArgument) 'CInt(e.CommandArgument) + dgGridView.PageIndex * dgGridView.PageSize
                Dim mID As Guid = mPlaceList(index).ID
                DeleteRecord(mID)
        End Select
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImgFindNow.Click  'btnFindNow.Click
        upnlGrid.Update()
        Dim Search As String
        If SearchCityValue.Value.ToString = "" Then
            Search = ""
        Else
            'Search = cmbSearchCity.SelectedItem.Text
            Search = SearchCityValue.Value.ToString
            cmbSearchCity.SelectedIndex = 0
        End If
        mPlaceList = PlaceList.GetPlaceList(txtPlace.Text, Search)
        Session("mPlaceList") = mPlaceList
        GridBind()
        cmbSearchCity.DataSource = mCityList
        cmbSearchCity.DataBind()
        If SearchCityValue.Value.ToString = "" Then
            'Do nothing
        Else
            cmbSearchCity.SelectedValue = mCityList.Item(New Guid(SearchCityID.Value)).ID.ToString
        End If
        SearchCityValue.Dispose()
        lblResult.Text = "As per criteria " & mPlaceList.Count & " Record(s) Found."
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If txtCode.Enabled = True Then
            setFocus(txtCode)
        End If
        NewRecord()
        MakeControlsBlank()
        upnlPlaceDetails.Update()
        GridBind()
        MarkLog(Util.Action.[New], "Place", "", Util.ErrorType.NoError, mPlace.ID, EventLogID)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("sender") = ""
        MarkLog(Util.Action.Close, "Place", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)

        Dim mopenas As String = Request.QueryString("Typepup")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

        If IsNothing(Request.QueryString("BackPage1")) Or Request.QueryString("BackPage1") = "" Then
            Session("MiddleFrame") = ""
            Response.Redirect("Dashboard.aspx")
        Else
            Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub hdnimgBtnCityMain_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnCityMain.Click
        mCityList = CityList.GetCityList("", "(SELECT)")
        Session("mCityList") = mCityList
        cmbCity.DataSource = mCityList
        cmbCity.DataBind()
        cmbSearchCity.DataSource = mCityList
        cmbSearchCity.DataBind()
        GridBind()
        upnlPlaceDetails.Update()
        upnlFindNow.Update()
    End Sub
#End Region

End Class