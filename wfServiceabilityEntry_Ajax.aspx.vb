Public Class wfServiceabilityEntry_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Decln"
    Public mServiciability As Serviciability
    Dim mYear As Integer
    Dim mMonth As Integer
    Dim mDateNo As Integer
    Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
    Protected mServiceabilityPriorityList As ServiceabilityPriorityList  'Added By Vikrant 20-Jun-2018 for ALL20062018
    Dim mFAScsReportList As FAScsReportList
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    Public munscheduleCatagoryList As UnScheduleCatagory   'Added by Shital on 16-Dec-2021 for TSL15122021 
#End Region

#Region " Function"
    Private Sub GetSession()
        mServiciability = Session("mServiciability")
        mYear = Session("mYear")
        mMonth = Session("mMonth")
        mDateNo = Session("mDateNo")
        mServiceabilityPriorityList = Session("mServiceabilityPriorityList") 'Added By Vikrant 20-Jun-2018 for ALL20062018
        mFAScsReportList = Session("mFAScsReportList")
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        munscheduleCatagoryList = Session("munscheduleCatagoryList")      'Added by Shital on 16-Dec-2021 for TSL15122021
    End Sub

    Private Sub SetSession()
        Session("mServiciability") = mServiciability
        Session("mYear") = mYear
        Session("mMonth") = mMonth
        Session("mDateNo") = mDateNo
    End Sub

    Private Sub SetObject()
        Dim chkValue1 As CheckBox
        Dim chkValue As RadioButton
        Dim txtValue As TextBox
        Dim txtDayPercent As TextBox
        Dim cmbPriority, cmbUnscheduleCatagory As DropDownList 'Added By Vikrant 20-Jun-2018 for ALL20062018

        Dim mServiciabilityDetail As ServiciabilityDetail

        Dim i As Integer = 0
        For Each mServiciabilityDetail In mServiciability.ServiciabilityDetails
            With mServiciabilityDetail
                chkValue = CType(Me.dgServiciabilityDetailList.Items(i).FindControl("chkServiceability"), RadioButton)
                .S_Status = chkValue.Checked

                chkValue = CType(Me.dgServiciabilityDetailList.Items(i).FindControl("chkSchedule"), RadioButton)
                .SM_Status = chkValue.Checked

                chkValue = CType(Me.dgServiciabilityDetailList.Items(i).FindControl("chkUnSchedule"), RadioButton)
                .USM_Status = chkValue.Checked

                txtValue = CType(Me.dgServiciabilityDetailList.Items(i).FindControl("txtRemark"), TextBox)
                .Remark = txtValue.Text

                txtValue = CType(Me.dgServiciabilityDetailList.Items(i).FindControl("txtErrorMark"), TextBox)
                txtValue.Text = .ErrorMark

                txtDayPercent = CType(Me.dgServiciabilityDetailList.Items(i).FindControl("txtDayPercent"), TextBox)
                .DayPercent = txtDayPercent.Text

                chkValue1 = CType(Me.dgServiciabilityDetailList.Items(i).FindControl("chkIsSelect"), CheckBox)
                .IsSelect = chkValue.Checked

                'Added By Vikrant 20-Jun-2018 for ALL20062018
                cmbPriority = CType(Me.dgServiciabilityDetailList.Items(i).FindControl("cmbPriority"), DropDownList)
                .PriorityID = CInt(cmbPriority.SelectedValue)
                'End

                'Added by Shital on 16-Dec-2021 for TSL
                cmbUnscheduleCatagory = CType(Me.dgServiciabilityDetailList.Items(i).FindControl("cmbUnscheduleCatagory"), DropDownList)
                .UnscheduleCatagoryID = CInt(cmbUnscheduleCatagory.SelectedValue)
                'End

            End With

            i = i + 1
        Next
    End Sub

    Private Function CustomValidate() As Boolean
        Dim strMSG As String = ""
        If Not mServiciability.IsValid Then
            Dim mShowBrokenRules As BrokenRules.RulesCollection = mServiciability.ShowBrokenRules
            For i As Integer = 0 To mShowBrokenRules.Count - 1
                strMSG = strMSG + mShowBrokenRules(i).Description + "<Br>"
            Next
        End If
        For i As Integer = 0 To dgServiciabilityDetailList.Items.Count - 1
            For j As Integer = 0 To dgServiciabilityDetailList.Items.Count - 1
                Dim IRowPriorityValue As Integer = CInt(CType(Me.dgServiciabilityDetailList.Items(i).FindControl("cmbPriority"), DropDownList).SelectedValue)
                Dim JRowPriorityValue As Integer = CInt(CType(Me.dgServiciabilityDetailList.Items(j).FindControl("cmbPriority"), DropDownList).SelectedValue)
                Dim IRowModelValue As String = CType(Me.dgServiciabilityDetailList.Items(i).FindControl("lblModel"), Label).Text
                Dim JRowModelValue As String = CType(Me.dgServiciabilityDetailList.Items(j).FindControl("lblModel"), Label).Text

                If i <> j And IRowPriorityValue <> 10 And JRowPriorityValue <> 10 And IRowPriorityValue = JRowPriorityValue And IRowModelValue = JRowModelValue Then
                    strMSG = strMSG + "Same Priority can not be assigned to two Aircraft with same model." + "<Br>"
                    Exit For
                    Exit For
                End If
            Next
        Next
        If strMSG.Trim <> "" Then
            cvBrokenRules.ErrorMessage = strMSG
            cvBrokenRules.IsValid = False
            Return False
        End If

        Return True
    End Function


    Private Sub enableddisabledLinkButtons()
        If AppSettings("ClientCode") = "APFT" Or
           AppSettings("ClientCode") = "AAP" Then
            lnkbtn1.Text = "01-08"
            lnkbtn2.Text = "09-16"
            lnkbtn3.Text = "17-24"
            lnkbtn4.Text = "25-32"
            lnkbtn5.Text = "33-40"
            lnkbtn6.Text = "41-48"
            lnkbtn7.Text = "49-56"
            lnkbtn8.Text = "57-64"

            Dim a As Integer = (mServiciability.ServiciabilityDetails.Count / 8)
            Dim Md As Integer = (mServiciability.ServiciabilityDetails.Count Mod 8)
            Dim IsChecked As Boolean = False

            lnkbtn1.Visible = (mServiciability.ServiciabilityDetails.Count >= 1)
            lnkbtn2.Visible = (mServiciability.ServiciabilityDetails.Count > 8)
            lnkbtn3.Visible = (mServiciability.ServiciabilityDetails.Count > 16)
            lnkbtn4.Visible = (mServiciability.ServiciabilityDetails.Count > 24)
            lnkbtn5.Visible = (mServiciability.ServiciabilityDetails.Count > 32)
            lnkbtn6.Visible = (mServiciability.ServiciabilityDetails.Count > 40)
            lnkbtn7.Visible = (mServiciability.ServiciabilityDetails.Count > 48)

            If a = 0 And CDec(mServiciability.ServiciabilityDetails.Count / 8) > 0 Then lnkbtn1.Text = "01-" + Format(Md, "00")
            If a = 1 Then lnkbtn2.Text = "9-" + ((a * 8) + Format(Md, "00")).ToString
            If a = 2 Then lnkbtn3.Text = "17-" + ((a * 8) + Format(Md, "00")).ToString
            If a = 3 Then lnkbtn4.Text = "25-" + ((a * 8) + Format(Md, "00")).ToString
            If a = 4 Then lnkbtn5.Text = "33-" + ((a * 8) + Format(Md, "00")).ToString
            If a = 5 Then lnkbtn6.Text = "41-" + ((a * 8) + Format(Md, "00")).ToString
            If a = 6 Then lnkbtn7.Text = "49-" + ((a * 8) + Format(Md, "00")).ToString
            If a = 7 Then lnkbtn8.Text = "57-" + ((a * 8) + Format(Md, "00")).ToString
        Else
            Dim a As Integer = (mServiciability.ServiciabilityDetails.Count / 10)
            Dim Md As Integer = (mServiciability.ServiciabilityDetails.Count Mod 10)
            Dim IsChecked As Boolean = False

            lnkbtn1.Visible = (mServiciability.ServiciabilityDetails.Count >= 1)
            lnkbtn2.Visible = (mServiciability.ServiciabilityDetails.Count > 10)
            lnkbtn3.Visible = (mServiciability.ServiciabilityDetails.Count > 20)
            lnkbtn4.Visible = (mServiciability.ServiciabilityDetails.Count > 30)
            lnkbtn5.Visible = (mServiciability.ServiciabilityDetails.Count > 40)
            lnkbtn6.Visible = (mServiciability.ServiciabilityDetails.Count > 50)
            lnkbtn7.Visible = (mServiciability.ServiciabilityDetails.Count > 60)

            If a = 0 And CDec(mServiciability.ServiciabilityDetails.Count / 10) > 0 Then lnkbtn1.Text = "01-" + Format(Md, "00")
            If a = 1 Then lnkbtn2.Text = "11-" + ((a * 10) + Format(Md, "00")).ToString
            If a = 2 Then lnkbtn3.Text = "21-" + ((a * 10) + Format(Md, "00")).ToString
            If a = 3 Then lnkbtn4.Text = "31-" + ((a * 10) + Format(Md, "00")).ToString
            If a = 4 Then lnkbtn5.Text = "41-" + ((a * 10) + Format(Md, "00")).ToString
            If a = 5 Then lnkbtn6.Text = "51-" + ((a * 10) + Format(Md, "00")).ToString
            If a = 6 Then lnkbtn7.Text = "61-" + ((a * 10) + Format(Md, "00")).ToString
            If a = 7 Then lnkbtn7.Text = "71-" + ((a * 10) + Format(Md, "00")).ToString
        End If

    End Sub


    Private Sub Save(Optional ByVal IsNaviagte As Boolean = False)
        Dim ServiciabilityClone As Serviciability
        ServiciabilityClone = mServiciability.Clone
        Try
            'check whether min. one item is present while saving
            If Not mServiciability.ServiciabilityDetails.Count = 0 Then
                'SetObject()
                mServiciability.IsHoliday = chkIsHoliday.Checked 'Added By Vikrant 20-Jun-2018 for ALL20062018
                mServiciability.Save()
                'MarkLog(Util.Action.Save, "Serviceability", mServiciability.CurrentDate, Util.ErrorType.NoError, mServiciability.ID)
                MarkLog(Util.Action.Save, "Serviceability", mServiciability.CurrentDate, Util.ErrorType.NoError, mServiciability.ID, EventLogID)
                mServiciability.MarkClean()
                Session("mServiciability") = mServiciability
                'If Not IsNaviagte Then Response.Redirect("wfServiceabilityEntry_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Record can not be saved without Item.", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        Catch ex As SqlException
            Session("ServiciabilityClone") = ServiciabilityClone

        Catch ex1 As Exception

        Finally
            ServiciabilityClone = Nothing
        End Try
    End Sub


    Private Sub print(ByVal MachineList As Guid(), ByVal RegNoList As String())

        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As New CompanyDetail

        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objServiciabilityReportList As ServiciabilityReportList
        If (AppSettings("ClientCode") IsNot Nothing) AndAlso 
           (AppSettings("ClientCode") = "APFT" Or
            AppSettings("ClientCode") = "AAP") Then
            myReport = New ServicieabilityTestAPFT

            objServiciabilityReportList = ServiciabilityReportList.GETServiciabilityReportList(mYear, mMonth, User.Identity.Name, MachineList, RegNoList, True)

        Else
            myReport = New ServicieabilityTest 'crptServiciability

            objServiciabilityReportList = ServiciabilityReportList.GETServiciabilityReportList(mYear, mMonth, User.Identity.Name, MachineList, RegNoList)

        End If




        Dim ds As New dsServiciability
        If objServiciabilityReportList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
                mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
                mCompanyDetail.WebSite, IIf(AppSettings("ClientCode") = "APFT" Or
                                                                      AppSettings("ClientCode") = "AAP",
                                                             "Aircraft Serviceability for the month of " +
                                                                     MonthName(Month(txtDate.Text), True).ToString +
                                                                     "-" + Year(txtDate.Text).ToString,
                                                             "Flying Details for the month of (" +
                                                                      MonthName(Month(txtDate.Text), True).ToString +
                                                                      "-" + Year(txtDate.Text).ToString + ")"), "", "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
        da.Fill(ds, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, objServiciabilityReportList)

        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub

    Private Sub Print(ByVal startFrom As Integer, ByVal endWith As Integer)
        If Not (User.IsInRole("ServiceabilityView")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If

        Dim MachineList As Guid()
        Dim RegNoList As String()

        Dim i As Integer = startFrom
        Dim j As Integer = 0
        ReDim MachineList(endWith - startFrom)
        ReDim RegNoList(endWith - startFrom)

        For i = 0 To mServiciability.ServiciabilityDetails.Count - 1
            If (i + 1) >= startFrom And (i + 1) <= endWith Then
                MachineList(j) = mServiciability.ServiciabilityDetails(i).MachineID
                RegNoList(j) = mServiciability.ServiciabilityDetails(i).RegNo
                j = j + 1
            End If
        Next
        print(MachineList, RegNoList)
    End Sub
#End Region

#Region " Helper Methods "
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Close" Then
                        Page.Validate()
                        If IsValid Then
                            SetObject()
                            Save()
                        End If
                    End If
                    
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.Ok
                    Session("Sender") = ""
                    SetSession()
                    'Response.Redirect("wfServiceabilityEntry_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))

                    'DataFieldBind()
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
        Session("Sender") = ""
        ''Response.Redirect("wfServiceabilityEntry_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
        End If
    End Sub
#End Region

#Region " Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
        If Not IsPostBack Then 'And Session("sender") = "" Then

            Dim i As Integer
            For i = 0 To 5
                cmbYearList.Items.Add(Year(Now.Date) - (5 - i))
            Next
            For i = 1 To 5
                cmbYearList.Items.Add(Year(Now.Date) + i)
            Next

            mYear = Year(Now.Date)
            mMonth = Month(Now.Date)
            mDateNo = Day(Now.Date)

            cmbYearList.SelectedValue = mYear
            cmbMonthList.SelectedValue = mMonth

            'Added by Shital on 1-Sep-2016
            Dim days As Integer = System.DateTime.DaysInMonth(mYear, mMonth)
            cmbDateList.Items.Clear()
            For j As Integer = 1 To days
                cmbDateList.Items.Add(j)
            Next
            cmbDateList.SelectedValue = mDateNo
            '--------------
            Session("mYear") = mYear
            Session("mMonth") = mMonth
            Session("mDateNo") = mDateNo

            mServiciability = Serviciability.GetServiciability(User.Identity.Name, cmbYearList.SelectedValue, cmbMonthList.SelectedValue, mDateNo)
            dgServiciabilityDetailList.DataSource = mServiciability.ServiciabilityDetails
            Session("mServiciability") = mServiciability

            'Added By Vikrant 20-Jun-2018 for ALL20062018
            mServiceabilityPriorityList = ServiceabilityPriorityList.GetList()
            Session("mServiceabilityPriorityList") = mServiceabilityPriorityList
            mFAScsReportList = FAScsReportList.GetFAScsReportList()
            Session("mFAScsReportList") = mFAScsReportList
            'Énd
            'Added by Shital on 15-Dec-2021 for TSL15122021 
            munscheduleCatagoryList = UnScheduleCatagory.GetList()
            Session("munscheduleCatagoryList") = munscheduleCatagoryList

            If AppSettings("ClientCode") = "TSL" Then
                dgServiciabilityDetailList.Columns(10).Visible = True
            Else
                dgServiciabilityDetailList.Columns(10).Visible = False
            End If
            '----
            DataBind()
            Save()
            enableddisabledLinkButtons()
        Else
            'If Not mServiciability Is Nothing Then
            '    dgServiciabilityDetailList.DataSource = mServiciability.ServiciabilityDetails
            '    DataBind()
            'End If
        End If
    End Sub

    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        mYear = cmbYearList.SelectedValue
        mMonth = cmbMonthList.SelectedValue

        'Added by Shital on 1-Sep-2016
        If cmbDateList.SelectedValue <> 0 Then
            mDateNo = cmbDateList.SelectedValue
        Else
            mDateNo = 1
        End If

        'mDateNo = Day(Now.Date)
        Session("mYear") = mYear
        Session("mMonth") = mMonth
        Session("mDateNo") = mDateNo

        mServiciability = Serviciability.GetServiciability(User.Identity.Name, cmbYearList.SelectedValue, cmbMonthList.SelectedValue, mDateNo)
        dgServiciabilityDetailList.DataSource = mServiciability.ServiciabilityDetails
        Session("mServiciability") = mServiciability
        DataBind()

        btnPrevious.Enabled = (mServiciability.DateNo - 1) >= 1
        btnNext.Enabled = (mServiciability.DateNo + 1) <= Date.DaysInMonth(mServiciability.Year, mServiciability.Month)

        Save()

        enableddisabledLinkButtons()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'MarkLog(Util.Action.Close, "Serviceability", "", Util.ErrorType.NoError, Guid.Empty)
        Session("IsValid") = IsValid
        If mServiciability.IsDirty Then
            If Not (User.IsInRole("ServiceabilityView")) Then
                MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
                
                
            Else
                MarkLog(Util.Action.Close, "Serviceability", mServiciability.CurrentDate, Util.ErrorType.NoError, mServiciability.ID, EventLogID)
                Session("MiddleFrame") = ""
                Response.Redirect("Dashboard.aspx")
            End If
        Else
            MarkLog(Util.Action.Close, "Serviceability", mServiciability.CurrentDate, Util.ErrorType.NoError, mServiciability.ID, EventLogID)
            Session("MiddleFrame") = ""
            Response.Redirect("Dashboard.aspx")
        End If

    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not (User.IsInRole("ServiceabilityView")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        SetSession()
        SetObject()
        If Not CustomValidate() Then Exit Sub
        If IsValid Then    'Added Code
            Save()
        End If
    End Sub
    Private Sub btnPrevious_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevious.Click
        If Not mServiciability Is Nothing And (mServiciability.DateNo - 1) >= 1 Then
            'Before moving to Previous, Saving Current Entry...
            If Not (User.IsInRole("ServiceabilityNew")) Then
                MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                Exit Sub
            End If

            SetObject()
            If Not CustomValidate() Then Exit Sub
            If IsValid Then   'Added Code
                Save(True)
            End If

            mYear = mServiciability.Year
            mMonth = mServiciability.Month
            mDateNo = mServiciability.DateNo - 1

            cmbYearList.SelectedValue = mYear
            cmbMonthList.SelectedValue = mMonth
            'Added on 1-Sep-2016 By Shital
            cmbDateList.SelectedValue = mDateNo

            Session("mYear") = mYear
            Session("mMonth") = mMonth
            Session("mDateNo") = mDateNo

            mServiciability = Serviciability.GetServiciability(User.Identity.Name, cmbYearList.SelectedValue, cmbMonthList.SelectedValue, mDateNo)
            dgServiciabilityDetailList.DataSource = mServiciability.ServiciabilityDetails
            Session("mServiciability") = mServiciability
            DataBind()

            btnPrevious.Enabled = (mServiciability.DateNo - 1) >= 1
            btnNext.Enabled = (mServiciability.DateNo + 1) <= Date.DaysInMonth(mServiciability.Year, mServiciability.Month)
        End If
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        If Not mServiciability Is Nothing And (mServiciability.DateNo + 1) <= Date.DaysInMonth(mServiciability.Year, mServiciability.Month) Then
            'Before moving to Next, Saving Current Entry...
            If Not (User.IsInRole("ServiceabilityView")) Then
                MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                Exit Sub
            End If

            SetObject()
            If Not CustomValidate() Then Exit Sub
            If IsValid Then   'Added Code
                Save(True)
            End If

            mYear = mServiciability.Year
            mMonth = mServiciability.Month
            mDateNo = mServiciability.DateNo + 1

            cmbYearList.SelectedValue = mYear
            cmbMonthList.SelectedValue = mMonth
            'Added on 1-Sep-2016 By Shital 
            cmbDateList.SelectedValue = mDateNo

            Session("mYear") = mYear
            Session("mMonth") = mMonth
            Session("mDateNo") = mDateNo

            mServiciability = Serviciability.GetServiciability(User.Identity.Name, cmbYearList.SelectedValue, cmbMonthList.SelectedValue, mDateNo)
            dgServiciabilityDetailList.DataSource = mServiciability.ServiciabilityDetails
            Session("mServiciability") = mServiciability
            DataBind()

            btnPrevious.Enabled = (mServiciability.DateNo - 1) >= 1
            btnNext.Enabled = (mServiciability.DateNo + 1) <= Date.DaysInMonth(mServiciability.Year, mServiciability.Month)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not (User.IsInRole("ServiceabilityView")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If

        Dim MachineList As Guid()
        Dim RegNoList As String()
        Dim i As Integer = 0
        Dim j As Integer = 1
        Dim chkValue As CheckBox

        ReDim MachineList(CInt(IIf(AppSettings("ClientCode") = "APFT" Or
                                   AppSettings("ClientCode") = "AAP", 7, 9)))
        ReDim RegNoList(CInt(IIf(AppSettings("ClientCode") = "APFT" Or
                                 AppSettings("ClientCode") = "AAP", 7, 9)))
        Dim IsCountGreater As Boolean = False
        Dim IsChecked As Boolean = False

        For i = 0 To mServiciability.ServiciabilityDetails.Count - 1
            chkValue = CType(Me.dgServiciabilityDetailList.Items(i).FindControl("chkIsSelect"), CheckBox)
            mServiciability.ServiciabilityDetails(i).IsSelect = chkValue.Checked
            If (mServiciability.ServiciabilityDetails(i).IsSelect = True) Then
                IsChecked = True
                Exit For
            Else
                IsChecked = False
            End If
        Next

        If IsChecked = False Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoOfAircrafts, MSGBox.Message_text.NoneAircraftsChecked, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If


        For i = 0 To mServiciability.ServiciabilityDetails.Count - 1
            chkValue = CType(Me.dgServiciabilityDetailList.Items(i).FindControl("chkIsSelect"), CheckBox)
            mServiciability.ServiciabilityDetails(i).IsSelect = chkValue.Checked
            If (mServiciability.ServiciabilityDetails(i).IsSelect = True) Then
                If (j > CInt(IIf(AppSettings("ClientCode") = "APFT" Or
                                 AppSettings("ClientCode") = "AAP", 8, 10))) Then
                    IsCountGreater = True
                    Exit For
                Else
                    MachineList(j) = mServiciability.ServiciabilityDetails(i).MachineID
                    RegNoList(j) = mServiciability.ServiciabilityDetails(i).RegNo
                    j = j + 1
                    IsCountGreater = False
                End If
            End If
        Next

        If IsCountGreater = True Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoOfAircrafts, MSGBox.Message_text.NoOfAircrafts, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        Else
            print(MachineList, RegNoList)
        End If

    End Sub
    Private Sub lnkbtn1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkbtn1.Click
        print(lnkbtn1.Text.Split("-")(0), lnkbtn1.Text.Split("-")(1))
    End Sub
    Private Sub lnkbtn2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkbtn2.Click
        print(lnkbtn2.Text.Split("-")(0), lnkbtn2.Text.Split("-")(1))
    End Sub
    Private Sub lnkbtn3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkbtn3.Click
        print(lnkbtn3.Text.Split("-")(0), lnkbtn3.Text.Split("-")(1))
    End Sub
    Private Sub lnkbtn4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkbtn4.Click
        print(lnkbtn4.Text.Split("-")(0), lnkbtn4.Text.Split("-")(1))
    End Sub
    Private Sub lnkbtn5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkbtn5.Click
        print(lnkbtn5.Text.Split("-")(0), lnkbtn5.Text.Split("-")(1))
    End Sub
    Private Sub lnkbtn6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkbtn6.Click
        print(lnkbtn6.Text.Split("-")(0), lnkbtn6.Text.Split("-")(1))
    End Sub
    Private Sub lnkbtn7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkbtn7.Click
        print(lnkbtn7.Text.Split("-")(0), lnkbtn7.Text.Split("-")(1))
    End Sub
    Private Sub lnkbtn8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkbtn8.Click
        print(lnkbtn8.Text.Split("-")(0), lnkbtn8.Text.Split("-")(1))
    End Sub

    Private Sub cmbMonthList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbMonthList.SelectedIndexChanged
        Dim month As Integer = cmbMonthList.SelectedValue
        Dim year As Integer = cmbYearList.SelectedValue
        Dim days As Integer = System.DateTime.DaysInMonth(year, month)
        cmbDateList.Items.Clear()
        For i As Integer = 1 To days
            cmbDateList.Items.Add(i)
        Next
    End Sub

    Private Sub cmbYearList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbYearList.SelectedIndexChanged
        Dim month As Integer = cmbMonthList.SelectedValue
        Dim year As Integer = cmbYearList.SelectedValue
        Dim days As Integer = System.DateTime.DaysInMonth(year, month)
        cmbDateList.Items.Clear()
        For i As Integer = 1 To days
            cmbDateList.Items.Add(i)
        Next
    End Sub
    'Added By Vikrant 20-Jun-2018 for ALL20062018
    Private Sub btnSendMail_Click(sender As Object, e As System.EventArgs) Handles btnSendMail.Click
        Session("ServDate") = mServiciability.CurrentDate
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        Session("UserEmailID") = mModuleList.Item("Serviceability").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("Serviceability").SendCCMailID
        '--------------------------
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", "OpenByMaiWindow();", True)
    End Sub
    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click

    End Sub
    'End
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

    
End Class