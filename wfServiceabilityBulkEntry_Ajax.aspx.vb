

'Created by Saylee on 11-Nov-2019


Public Class wfServiceabilityBulkEntry_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Decln"
    Public mServiciability As Serviciability
    Dim mYear As Integer
    Dim mMonth As Integer
    Dim mDateNo As Integer
    Dim EventLogID As Guid
    Protected mServiceabilityPriorityList As ServiceabilityPriorityList
    Dim mFAScsReportList As FAScsReportList
    Dim mMachineNameValueList As MachineNameValueList

    Dim MachineName As Guid
    Dim AircraftName As String
    Dim AircraftIds() As Guid
    Public munscheduleCatagoryList As UnScheduleCatagory   'Added by Shital on 16-Dec-2021 for TSL15122021 
#End Region

#Region " Function"
    Private Sub GetSession()
        mServiciability = Session("mServiciability")
        mYear = Session("mYear")
        mMonth = Session("mMonth")
        mDateNo = Session("mDateNo")
        mServiceabilityPriorityList = Session("mServiceabilityPriorityList")
        mFAScsReportList = Session("mFAScsReportList")
        mMachineNameValueList = Session("mMachineNameValueList")
        munscheduleCatagoryList = Session("munscheduleCatagoryList")      'Added by Shital on 16-Dec-2021 for TSL15122021
    End Sub
    Private Sub SetSession()
        Session("mServiciability") = mServiciability
        Session("mYear") = mYear
        Session("mMonth") = mMonth
        Session("mDateNo") = mDateNo
        Session("mMachineNameValueList") = mMachineNameValueList
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfServiceabilityBulkEntry_Ajax.aspx" Then
            Session.Remove("mServiciability")
            Session.Remove("mMachineNameValueList")
            Session.Remove("mFAScsReportList")
        End If
    End Sub
    Private Sub Save(AircraftIds() As Guid)
        Dim i As Integer = 0

        Dim mYear As Integer = Year(txtFromDate.Text)
        Dim mMonth As Integer = Month(txtFromDate.Text)
        Dim mDatNo As Integer = Day(txtFromDate.Text)


        mServiciability = Serviciability.GetServiciability(User.Identity.Name, mYear, mMonth, mDatNo)
        While CDate(txtFromDate.Text).AddDays(i) <= CDate(txtToDate.Text)
            mServiciability = Serviciability.GetServiciability(User.Identity.Name, Year(CDate(txtFromDate.Text).AddDays(i)), Month(CDate(txtFromDate.Text).AddDays(i)), Day(CDate(txtFromDate.Text).AddDays(i)))


            Session("mServiciability") = mServiciability
            Dim mServiciabilityDetail As ServiciabilityDetail

            For Each mServiciabilityDetail In mServiciability.ServiciabilityDetails
                For j As Integer = 0 To AircraftIds.Length - 1
                    With mServiciabilityDetail
                        If AircraftIds(j).Equals(mServiciabilityDetail.MachineID) Then
                            .S_Status = chkServiceability.Checked
                            .SM_Status = chkSchedule.Checked
                            .USM_Status = chkUnSchedule.Checked
                            .Remark = txtRemark.Text
                            .DayPercent = txtDayPercent.Text
                            '   .PriorityID = CInt(cmbPriority.SelectedValue)
                            .UnscheduleCatagoryID = cmbUnscheduleCatagory.SelectedValue
                        End If
                    End With
                Next

            Next
            Dim ServiciabilityClone As Serviciability
            ServiciabilityClone = mServiciability.Clone
            Try
                'check whether min. one item is present while saving
                If Not mServiciability.ServiciabilityDetails.Count = 0 Then
                    mServiciability.Save()
                    'MarkLog(Util.Action.Save, "Serviceability", mServiciability.CurrentDate, Util.ErrorType.NoError, mServiciability.ID)
                    MarkLog(Util.Action.Save, "Serviceability", mServiciability.CurrentDate, Util.ErrorType.NoError, mServiciability.ID, EventLogID)
                    mServiciability.MarkClean()
                    Session("mServiciability") = mServiciability
                    ' MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                End If
            Catch ex As Exception
                Session("ServiciabilityClone") = ServiciabilityClone
            Finally
                ServiciabilityClone = Nothing
            End Try


            i += 1
        End While


    End Sub
#End Region

#Region " Data Binding "
    Public Sub DataFieldBinding()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, SkipIsForInventoryAircarft:=True)
        Session("mMachineNameValueList") = mMachineNameValueList
        ChklistAircraft.DataSource = mMachineNameValueList

        mFAScsReportList = FAScsReportList.GetFAScsReportList()
        Session("mFAScsReportList") = mFAScsReportList

        mServiceabilityPriorityList = ServiceabilityPriorityList.GetList()
        Session("mServiceabilityPriorityList") = mServiceabilityPriorityList

        'Added by Shital on 15-Dec-2021 for TSL15122021 
        munscheduleCatagoryList = UnScheduleCatagory.GetList()
        cmbUnscheduleCatagory.DataSource = munscheduleCatagoryList
        Session("munscheduleCatagoryList") = munscheduleCatagoryList
        '--
        DataBind()
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfServiceabilityBulkEntry_Ajax.aspx"
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            txtDayPercent.Text = "100.00"
            DataFieldBinding()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not (User.IsInRole("ServiceabilityView")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If IsValid Then
            Dim MachineList As Guid()
            Dim RegNoList As String()
            Dim j As Integer = 1
            ReDim MachineList(10)
            ReDim RegNoList(10)
            Dim IsCountGreater As Boolean = False
            Dim IsChecked As Boolean = False
            For i As Integer = 0 To ChklistAircraft.Items.Count - 1
                If ChklistAircraft.Items(i).Selected Then
                    IsChecked = True
                    Exit For
                Else
                    IsChecked = False
                End If
            Next
            If IsChecked = False Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoOfAircrafts, SIMsgBox.Message_text.NoneAircraftsChecked, "", MsgBoxStyle.OkOnly)
                'msg1.ReplacePage = "wfServiceabilityEntry.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.NoOfAircrafts, MSGBox.Message_text.NoneAircraftsChecked, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            AircraftName = String.Empty
            ReDim AircraftIds(10)
            For i As Integer = 0 To ChklistAircraft.Items.Count - 1
                If ChklistAircraft.Items(i).Selected Then
                    If (j > 10) Then
                        IsCountGreater = True
                        Exit For
                    Else
                        AircraftIds(j) = New Guid(ChklistAircraft.Items(i).Value)
                        j = j + 1
                        IsCountGreater = False
                    End If
                End If
            Next
            If IsCountGreater = True Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoOfAircrafts, SIMsgBox.Message_text.NoOfAircrafts, "", MsgBoxStyle.OkOnly)
                'msg1.ReplacePage = "wfServiceabilityEntry.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.NoOfAircrafts, MSGBox.Message_text.NoOfAircrafts, "", MsgBoxStyle.OkOnly, "")
            Else
                Save(AircraftIds)
            End If

        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
         Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("DashBoard.aspx")

    End Sub
#End Region
End Class