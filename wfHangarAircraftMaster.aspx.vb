Public Class wfHangarAircraftMaster
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAirCraftMaster As AirCraftMaster
    Public mAirCraftMasterList As AirCraftMasterList
    Public mVendorList As VendorList
    Public mHangarList As HangarList
    Public mModelList As ModelList
    ' Dim EventLogID As Guid 'Added By Utkarsh On 19-Jul-2011 For All19072011
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mModelList = CType(Session("mModelList"), ModelList)
        mAirCraftMasterList = CType(Session("mAirCraftMasterList"), AirCraftMasterList)
        mAirCraftMaster = CType(Session("mAirCraftMaster"), AirCraftMaster)
        mVendorList = Session("mVendorList")
        Session("NewPage") = "False"
    End Sub
    Private Sub SetSession()
        Session("mModelList") = mModelList
        Session("mAirCraftMasterList") = mAirCraftMasterList
        Session("mAirCraftMaster") = mAirCraftMaster
        Session("mVendorList") = mVendorList
    End Sub
    Private Sub NewRecord()
        mAirCraftMaster = AirCraftMaster.NewHangarAircraft()
        lbltitle.Text = "Aircraft [New]"
        Session("mAirCraftMaster") = mAirCraftMaster
        ' upnlTitle.Update()
    End Sub
  
    Private Sub setObject()
        mAirCraftMaster.Haircraft = Trim(txtAircraft.Text)
        mAirCraftMaster.VendorID = New Guid(cmbCustomer.SelectedValue)
        mAirCraftMaster.ModelID = New Guid(CmbModel.SelectedValue)
        mAirCraftMaster.SerialNo = Trim(TxtSerialNo.Text)
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mAirCraftMaster = AirCraftMaster.GetHangarAircraft(mId)
        Session("mAirCraftMaster") = mAirCraftMaster
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
                            mAirCraftMaster = CType(Session("mAirCraftMaster"), AirCraftMaster)
                            AirCraftMaster.DeleteHangarAircraft(mAirCraftMaster.HID)                    
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


                                MarkLog(Util.Action.Delete, "Aircraft", "Can't delete : " & mAirCraftMaster.Haircraft & " is Currently in use", Util.ErrorType.NoError, mAirCraftMaster.HID, EventLogID)
                      
                            End If
                            NewRecord()
                            DataFieldBind()
                            SetTitle()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Changed By Utkarsh On 19-Jul-2011 For All19072011

                                MarkLog(Util.Action.Delete, "Aircraft", mAirCraftMaster.Haircraft, Util.ErrorType.NoError, mAirCraftMaster.HID, EventLogID)
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
        upnlAircraftDetails.Update()
    End Sub
    Private Sub SetTitle()
        If mAirCraftMaster.IsNew Then
            lbltitle.Text = "Aircraft [New]"
        Else
            If Len(mAirCraftMaster.Haircraft) > 15 Then
                lbltitle.Text = "Aircraft [" & mAirCraftMaster.Haircraft.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Aircraft [" & mAirCraftMaster.Haircraft & "]"
            End If
        End If
        'Added by Amrita on 10-Dec-07 for displaying no of records in data grid.
        'lblResult.Text = "Aircraft List: " & mAirCraftMasterList.Count & " Record(s) Found."
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mModelList = ModelList.GetModelList(1, "", , , "(SELECT)")
        CmbModel.DataSource = mModelList
        Session("mModelList") = mModelList
        CmbModel.DataBind()
        mVendorList = VendorList.GetVendorstList(0, "", "", "", "", "", "<SELECT>", True, False, False)
        '' mVendorList = VendorList.GetVendortList(0, "", "", "", "", "", , True, False, False)
        ' mVendorList = VendorList.GetVendortList(0, , , , , , True, True,False)
        cmbCustomer.DataSource = mVendorList
        cmbCustomer.DataBind()
        mAirCraftMasterList = AirCraftMasterList.GetHangarList()
        dgAirCraft.DataSource = mAirCraftMasterList
        Session("mAirCraftMasterList") = mAirCraftMasterList
        dgAirCraft.DataBind()
        '''upnlGrid.Update()
        upnlAircraftDetails.Update()
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
            Session.Remove("mAirCraftMaster")
            Session.Remove("mAirCraftMasterList")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub


    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            setObject()
            mAirCraftMaster.Save()
            ' MarkLog(Util.Action.Save, "Manufacturer", mAirCraftMaster.Name, Util.ErrorType.NoError, mManufacturer.ID, EventLogID)
            mAirCraftMaster = AirCraftMaster.NewHangarAircraft()
            NewRecord()
            txtAircraft.Text = ""
            TxtSerialNo.Text = ""
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

    Private Sub imgbtnModel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnModel.Click
        setObject()
        Session("mAirCraftMaster") = mAirCraftMaster
        Session("Type") = True
        Session("AircraftModels") = True
        Session("AssemblyTypeId") = 1
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelWindow", "OpenModelWindow()", True)
    End Sub
    Private Sub hdnBtnModel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnModel.Click
        mModelList = ModelList.GetModelList(1, "", , , "(SELECT)")
        cmbModel.DataSource = mModelList
        Session("mModelList") = mModelList
        cmbModel.DataBind()
        CmbModel.SelectedValue = mAirCraftMaster.ModelID.ToString()
        upnlAircraftDetails.Update()
    End Sub


    Protected Sub dgAirCraft_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgAirCraft.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                mAirCraftMaster = AirCraftMaster.GetHangarAircraft(mID)
                If mAirCraftMaster.AircraftCount = 0 Then
                    'do nothing 
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Record Already Exists in Hangar Planning so it cannot be Edited", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Session("mAirCraftMaster") = mAirCraftMaster
                txtAircraft.Text = mAirCraftMaster.Haircraft
                CmbModel.SelectedValue = mAirCraftMaster.ModelID.ToString()
                CmbModel.DataBind()
                TxtSerialNo.Text = mAirCraftMaster.SerialNo              
                cmbCustomer.SelectedValue = mAirCraftMaster.VendorID.ToString()
                cmbCustomer.DataBind()
                SetFocus(txtAircraft)
                lbltitle.Text = "Aircraft " + "[" + mAirCraftMaster.Haircraft + "]"

                'upnlAircraftDetails.DataBind()
                mAirCraftMasterList = AirCraftMasterList.GetHangarList()
                dgAirCraft.DataSource = mAirCraftMasterList
                Session("mAirCraftMasterList") = mAirCraftMasterList
                dgAirCraft.DataBind()

                upnlAircraftDetails.Update()
            Case "DeleteRec"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                DeleteRecord(mID)
                DataFieldBind()
                txtAircraft.Text = ""
                TxtSerialNo.Text = ""
                upnlAircraftDetails.Update()
                Session("mAirCraftMaster") = mAirCraftMaster
        End Select
    End Sub

    Protected Sub dgAirCraft_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgAirCraft.PageIndexChanging
        dgAirCraft.PageIndex = e.NewPageIndex
        dgAirCraft.DataSource = mAirCraftMasterList
        Session("mAirCraftMasterList") = mAirCraftMasterList
        dgAirCraft.DataBind()
        upnlAircraftDetails.Update()
    End Sub

    Private Sub dgAirCraft_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgAirCraft.Sorting
        mAirCraftMasterList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mAirCraftMasterList") = mAirCraftMasterList
        dgAirCraft.DataSource = mAirCraftMasterList
        dgAirCraft.DataBind()
        upnlAircraftDetails.Update()
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        NewRecord()
        MarkLog(Util.Action.[New], "Aircraft", "", Util.ErrorType.NoError, mAirCraftMaster.HID, EventLogID)
        DataFieldBind()
        SetTitle()
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region


End Class