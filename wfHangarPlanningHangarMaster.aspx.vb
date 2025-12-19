Public Class wfHangarPlanningHangarMaster
    Inherits System.Web.UI.Page
#Region " Variable Declaration "
    Public mHangerMaster As HangerMaster
    Public mHangerMasterList As HangerMasterList
    Public mTransTypeID As Trans
    Public mCityList As CityInvList
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mHangerMaster = CType(Session("mHangerMaster"), HangerMaster)
        mHangerMasterList = CType(Session("mHangerMasterList"), HangerMasterList)
        mTransTypeID = Session("mTransTypeID")
        mCityList = CType(Session("mCityList"), CityInvList)
    End Sub
    Private Sub SetSession()
        Session("mHangerMaster") = mHangerMaster
        Session("mHangerMasterList") = mHangerMasterList
        Session("mCityList") = mCityList
    End Sub
    Private Sub NewRecord()
        mHangerMaster = HangerMaster.NewHanger()
        Session("mHangerMaster") = mHangerMaster
        lbltitle.Text = "Hangar [New]"
        'upnlTitle.Update()
    End Sub

    Private Sub setObject()
        mHangerMaster.HHanger = Trim(txtHanger.Text)
        mHangerMaster.CityID = New Guid(cmbCity.SelectedValue)
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mHangerMaster = HangerMaster.GetHanger(mId)
        Session("mHangerMaster") = mHangerMaster
    End Sub


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
                            mHangerMaster = CType(Session("mHangerMaster"), HangerMaster)
                            HangerMaster.DeleteHanger(mHangerMaster.HID)
                            NewRecord()
                            DataFieldBind()
                            SetTitle()

                        Catch ex As SqlException
                            If ex.Number = 8145 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, "Hangar Planning", MsgBoxStyle.OkOnly, "")


                                MarkLog(Util.Action.Delete, "Hangar", "Can't delete : " & mHangerMaster.HHanger & " is Currently in use", Util.ErrorType.NoError, mHangerMaster.HID, EventLogID)

                            End If
                            NewRecord()
                            DataFieldBind()
                            SetTitle()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Changed By Utkarsh On 19-Jul-2011 For All19072011

                                MarkLog(Util.Action.Delete, "Hangar", mHangerMaster.HHanger, Util.ErrorType.NoError, mHangerMaster.HID, EventLogID)
                                'End
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
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added

                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    DataFieldBind()
            End Select
        ElseIf Result1 = -1 Then
            'Session("sender") = ""
            DataFieldBind()
            'Changed By Utkarsh On 31-Jan-2013 For ALL30122013
            'Response.Redirect("wfManufacturer.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId"))
            'End
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            'Session("sender") = ""
            DataFieldBind()
        End If
        upnlHangarDetails.Update()
    End Sub
    Private Sub SetTitle()
        If mHangerMaster.IsNew Then
            lbltitle.Text = "Hangar [New]"
        Else
            If Len(mHangerMaster.HHanger) > 15 Then
                lbltitle.Text = "Hangar [" & mHangerMaster.HHanger.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Hangar [" & mHangerMaster.HHanger & "]"
            End If
        End If
        'Added by Amrita on 10-Dec-07 for displaying no of records in data grid.
        'lblResult.Text = "Aircraft List: " & mAirCraftMasterList.Count & " Record(s) Found."
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCityList = CityInvList.GetCityList(0, "", "", True)
        Session("mCityList") = mCityList
        cmbCity.DataSource = mCityList
        cmbCity.DataBind()

        mHangerMasterList = HangerMasterList.GetHangarList(, , , , )
        dgHangerList.DataSource = mHangerMasterList
        Session("mHangerMasterList") = mHangerMasterList
        dgHangerList.DataBind()

        ''' upnlGrid.Update()
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 19-Jul-2011 For All19072011
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            NewRecord()
            DataFieldBind()
        End If
        SetTitle()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            Session.Remove("mHangerMaster")
            Session.Remove("mHangerMasterList")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub


    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            setObject()
            mHangerMaster.Save()
            ' MarkLog(Util.Action.Save, "Manufacturer", mAirCraftMaster.Name, Util.ErrorType.NoError, mManufacturer.ID, EventLogID)

            mHangerMaster = HangerMaster.NewHanger()
            NewRecord()
            txtHanger.Text = ""
            LblState.Text = ""
            LblCountry.Text = ""
            DataFieldBind()
            SetSession()
            SetTitle()

        Catch ex As SqlException
            If ex.Number = 8145 Then

                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
            ElseIf ex.Number = 2627 Then

                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then

                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
            End If
            DataFieldBind()
        End Try
    End Sub




    Protected Sub dgHangerList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgHangerList.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                mHangerMaster = HangerMaster.GetHanger(mID)
                If mHangerMaster.HangarCount = 0 Then
                    'do nothing 
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Record Already Exists in Hangar Planning so it cannot be Edited", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Session("mHangerMaster") = mHangerMaster
                txtHanger.Text = mHangerMaster.HHanger
                cmbCity.SelectedValue = mHangerMaster.CityID.ToString()
                cmbCity.DataBind()
                LblState.Text = mHangerMaster.HState
                LblCountry.Text = mHangerMaster.HCountry
                SetFocus(txtHanger)
                lbltitle.Text = "Hangar " + "[" + mHangerMaster.HHanger + "]"


                mHangerMasterList = HangerMasterList.GetHangarList()
                dgHangerList.DataSource = mHangerMasterList
                Session("mHangerMasterList") = mHangerMasterList
                dgHangerList.DataBind()
                upnlHangarDetails.Update()
            Case "DeleteRec"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                DeleteRecord(mID)
                DataFieldBind()
                txtHanger.Text = ""
                upnlHangarDetails.Update()
                Session("mHangerMaster") = mHangerMaster
        End Select
    End Sub

    Protected Sub dgHangerList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgHangerList.PageIndexChanging
        dgHangerList.PageIndex = e.NewPageIndex
        dgHangerList.DataSource = mHangerMasterList
        Session("mHangerMasterList") = mHangerMasterList
        dgHangerList.DataBind()
        upnlHangarDetails.Update()
    End Sub

    Private Sub dgHangerList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgHangerList.Sorting
        mHangerMasterList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mHangerMasterList") = mHangerMasterList
        dgHangerList.DataSource = mHangerMasterList
        dgHangerList.DataBind()
        upnlHangarDetails.Update()
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        NewRecord()
        MarkLog(Util.Action.[New], "Hangar", "", Util.ErrorType.NoError, mHangerMaster.HID, EventLogID)
        DataFieldBind()
        SetTitle()
        txtHanger.Text = ""
        LblState.Text = ""
        LblCountry.Text = ""
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Protected Sub cmbCity_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbCity.SelectedIndexChanged
        Dim indx As Integer
        indx = cmbCity.SelectedIndex
        LblState.Text = mCityList.Item(indx).State.ToString
        LblState.DataBind()
        LblCountry.Text = mCityList.Item(indx).Country.ToString
        LblCountry.DataBind()
        'setObject()
        'LblState.Text = mHangerMaster.HState
        'LblState.DataBind()
        'LblCountry.Text = mHangerMaster.HCountry
        'LblCountry.DataBind()
    End Sub
#End Region
End Class