'AJAX Conversion By Vikrant On 21-Aug-2014

Imports System.Web.Mail
Imports System.Text
Imports System.Linq
Imports System.Linq.Enumerable
Imports System.Collections.Generic
Imports System.Web.Script.Serialization
Public Class wfRequisition_Ajax
    Inherits System.Web.UI.Page

#Region " Enumaration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
        FindNow = 7
        Authorized = 8
    End Enum
#End Region

#Region " Variable Declaration "
    Public mRequisitionNew As RequisitionNew
    Private mLocationList As LocationList
    Private mEmployeeList As EmployeeList
    Public Flag As Integer
    Dim EventLogID As Guid
    Dim mRequisitionDetail As String
    Public mEmployeeStatus As EmployeeStatus 'Added By Shweta On 07-Aug-2013 For ALL01082013
    Public mRequisitionEngineeringBranchesList As RequisitionEngineeringBranchesList
    Dim mTransTypeID As Integer 'All13082014
    Dim mTransactionList As TransactionList
    Public mWorkShopList As WorkShopList
    Dim mRequisitionCustomer As RequisitionCustomer
    Dim CustName As String = String.Empty
    Dim CustAddress As String = String.Empty
    Dim AircraftType As String = String.Empty
    Dim RequiredByDate As String
    Dim mUser As User
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mRequisitionNew = Session("mRequisitionNew")
        mLocationList = Session("mLocationList")
        mEmployeeList = Session("mEmployeeList")
        mTransTypeID = Session("TransTypeID")
        mTransactionList = Session("mTransactionList")
        mWorkShopList = Session("mWorkShopList")
    End Sub
    Private Sub setSession()
        Session("mRequisitionNew") = mRequisitionNew
        Session("mLocationList") = mLocationList
        Session("mEmployeeList") = mEmployeeList
    End Sub
    Private Sub setObject()
        If txtRequisitionDate.Text = "" Then
            txtRequisitionDate.Text = Today.Date
        Else
            mRequisitionNew.ReqDate = New SmartDate(txtRequisitionDate.Text).Text
        End If
        mRequisitionNew.Text = txtText.Text.Trim
        mRequisitionNew.No = Val(txtNo.Text)
        mRequisitionNew.UserName = User.Identity.Name
        If mRequisitionNew.TransTypeID = Util.Trans.StoresRequisition Then
            mRequisitionNew.ReqTypeID = 0
            'ElseIf mRequisitionNew.TransTypeID = Util.Trans.PlanningRequisition Then  'Commented by Prashant 20-Oct-2020 STR20102020.Add Requisition Type as “Part Purchase or Part Request” in Planning Requisition module
            '    mRequisitionNew.ReqTypeID = 1
        Else
            mRequisitionNew.ReqTypeID = IIf(rdoPartRequest.Checked, 1, 2)
        End If
        mRequisitionNew.RecommendedBy = Trim(txtRecommendedBy.Text)
        mRequisitionNew.Supervisor = Trim(txtSupervisor.Text)
        mRequisitionNew.WorkShopID = New Guid(cmbWorkShop.SelectedValue)
        mRequisitionNew.IndentTypeID = CInt(cmbIndentType.SelectedValue)
        mRequisitionNew.Remark = Trim(txtRemark.Text)
        Dim txtValue As TextBox
        Dim mRequisitionItemNew As RequisitionItemNew
        Dim i As Integer = 0
        For Each mRequisitionItemNew In mRequisitionNew.RequisitionItemsNew
            With mRequisitionItemNew
                txtValue = CType(Me.dgRequisitionItems.Rows(i).FindControl("txtQty"), TextBox)
                .RequestedQty = CDec(Val(txtValue.Text))

            End With
            i = i + 1
        Next
    End Sub
    Private Sub setComboDetails()
        mRequisitionNew.LocationID = New Guid(cmbLocationList.SelectedValue)
        mRequisitionNew.EmployeeName = txtEmployee.Text
        If mRequisitionNew.TransTypeID = Util.Trans.EngineeringRequisition Or mRequisitionNew.TransTypeID = Util.Trans.WorkShopRequisition Then
            mRequisitionNew.RequisitionEngineeringBrancheID = cmbRequisitionEngineeringBranches.SelectedValue
        ElseIf mRequisitionNew.TransTypeID = Util.Trans.PlanningRequisition Then
            mRequisitionNew.RequisitionEngineeringBrancheID = 4
        Else
            mRequisitionNew.RequisitionEngineeringBrancheID = 0
        End If
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "Delete")
        mRequisitionNew.RequisitionItemsNew.CurrentIndex = Index
        Session("mRequisitionNew") = mRequisitionNew
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("Sender") = ""
                            Dim mRequisitionNew As RequisitionNew
                            mRequisitionNew = CType(Session("mRequisitionNew"), RequisitionNew)
                            mRequisitionNew.RequisitionItemsNew.Remove(mRequisitionNew.RequisitionItemsNew.CurrentItem)
                            Session("mRequisitionNew") = mRequisitionNew
                            dgRequisitionItems.DataSource = mRequisitionNew.RequisitionItemsNew
                            dgRequisitionItems.DataBind()
                            ControlVisibility()
                            upnlReqDetails.Update()
                            upnlGridView.Update()
                            upnlActionBtn.Update()
                            upnlReqItemAdd.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "Close" Then  '' Close confirmation
                        Session("sender") = ""
                        Page.Validate("1")
                        If IsValid Then
                            Session.Remove("IsValid")
                            If mRequisitionNew.RequisitionItemsNew.Count = 0 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Requisition can not be saved without Item.", MsgBoxStyle.OkOnly, "")
                                If mRequisitionNew.StatusID = 2 Then
                                    mRequisitionNew.StatusID = 1
                                    Session("mRequisitionNew") = mRequisitionNew
                                End If
                                Exit Sub
                            End If
                            DataFieldBind()
                            If (Not IsInRole(Rights.New)) And (Not IsInRole(Rights.Edit)) Then
                                MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                            Save()
                            Dim URL As Stack = CType(Session("ReqURLFromWO"), Stack)
                            If Not URL Is Nothing Then
                                If URL.Count > 0 Then
                                    'Session("MiddleFrame") = "wfnWOList_AJAX.aspx?TransTypeID=" & Session("WOTransTypeID")
                                    Session.Remove("WOTransTypeID")
                                    Session("MiddleFrame") = Session("MiddleFrameForWO") '12-Jun-2019
                                    Response.Redirect(URL.Peek.ToString)
                                    Exit Sub
                                End If
                            End If
                            Response.Redirect("Index.aspx")
                        Else
                            Session.Remove("IsValid")
                            upnlValidationSummary.Update()
                        End If
                    ElseIf MSGBoxCtrl.Sender = "Status" Then
                        Session("sender") = ""
                        If Session("IsValid") Then
                            Session.Remove("IsValid")
                            DataFieldBind()
                            Save()
                            If (AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND" Or AppSettings("ClientCode") = "BA") Then Print(True) 'BA Added by Prashant on 13-Jan-2023
                        Else
                            Session.Remove("IsValid")
                            upnlValidationSummary.Update()
                        End If
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session.Remove("IsValid")
                        Session("Sender") = ""
                        Dim URL As Stack = CType(Session("ReqURLFromWO"), Stack)
                        If Not URL Is Nothing Then
                            If URL.Count > 0 Then
                                'Session("MiddleFrame") = "wfnWOList_AJAX.aspx?TransTypeID=" & Session("WOTransTypeID")
                                Session("MiddleFrame") = Session("MiddleFrameForWO") '12-Jun-2019
                                Session.Remove("WOTransTypeID")
                                Response.Redirect(URL.Peek.ToString)
                                Exit Sub
                            End If
                        End If
                        Response.Redirect("Index.aspx")
                    ElseIf MSGBoxCtrl.Sender = "Status" Then
                        Session("Sender") = ""
                        Session.Remove("IsValid")
                        If mRequisitionNew.StatusID = 2 Then
                            mRequisitionNew.StatusID = 1
                        ElseIf mRequisitionNew.StatusID = 4 Then 'Added By Vikrant On 28-Sep-2015 For All28092015
                            mRequisitionNew.StatusID = 2
                            mRequisitionNew.MarkClean()
                            'End
                        End If
                        Session("mRequisitionNew") = mRequisitionNew
                        upnlStatus.Update()
                    Else
                        Session("Sender") = ""
                    End If
                Case MsgBoxResult.Ok
                    'Added by Utkarsh On 22-Nov-2013 For TransTextSeries
                    If MSGBoxCtrl.Sender = "RequisitionTransTextSeriesAlert" Then
                        Session("sender") = ""
                        Session("AddTransTextSeries") = "True"
                        Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                        'ENd
                        'Added By Vikrant On 28-Sep-2015 For All28092015
                    ElseIf MSGBoxCtrl.Sender = "Status" Then
                        ''==========================================WO - 2006-2007-1-17.doc
                        'If mRequisitionNew.StatusID = 2 And Session("sender") <> "Close" Then
                        '    mRequisitionNew.StatusID = 1
                        'ElseIf mRequisitionNew.StatusID = 4 Then
                        '    mRequisitionNew.StatusID = 2
                        'End If
                        Session("sender") = ""
                        Session("mRequisitionNew") = mRequisitionNew
                        ''========================================
                        'DataFieldBind()
                        upnlStatus.Update()
                        'End
                    ElseIf MSGBoxCtrl.Sender = "ResetEmployee" Then
                        'cmbEmployeeList.DataSource = mEmployeeList
                        'cmbEmployeeList.DataBind()
                        'If Not mRequisitionNew.EmployeeID.Equals(Guid.Empty) Then
                        '    cmbEmployeeList.SelectedValue = mRequisitionNew.EmployeeID.ToString
                        'End If
                        'cmbEmployeeList.Enabled = IIf(mRequisitionNew.StatusID > 1, False, True)
                        'ClearEmpID()
                        txtEmployee.Text = ""
                        If Not mRequisitionNew.EmployeeID.Equals(Guid.Empty) Then
                            txtEmployee.Text = mRequisitionNew.EmployeeName
                            'hdnEmpId.Value = mRequisitionNew.EmployeeID.ToString
                            'SetEmpID()
                        End If

                        'Commented and Added By Vikrant On 26-Feb-2021 For Heligo01032021
                        'txtEmployee.Enabled = IIf(mRequisitionNew.StatusID > 1, False, True)
                        mUser = SI.UTILITY.User.GetUser(User.Identity.Name)
                        If (mRequisitionNew.StatusID > 1 Or (AppSettings("ClientCode") = "Heligo" And mUser.EmployeeName <> "")) Then
                            txtEmployee.Enabled = False
                        Else
                            txtEmployee.Enabled = True
                        End If
                        'End
                        upnlReqDetails.Update()
                    Else
                        Session("sender") = ""
                    End If
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            ''Added New on 9 April RAJNISH
            If mRequisitionNew.StatusID = 2 Then
                mRequisitionNew.StatusID = 1
                'ElseIf mRequisition.StatusID = 4 Then
                '    mRequisition.StatusID = 2
            End If
            Session("mRequisitionNew") = mRequisitionNew
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
        End If
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value)")
    End Sub
    Private Sub SetPage()
        Dim mModuleName As String = String.Empty
        mModuleName = mTransactionList(CType(mTransTypeID, Util.Trans)).Name
        If mRequisitionNew.No > 0 Then
            lblTitle.Text = mModuleName & " Details [" & mRequisitionNew.Text + "-" + CType(mRequisitionNew.No, String) + "]"
        Else
            lblTitle.Text = mModuleName & " Details "
        End If
    End Sub
    Private Sub Print(ByVal IsSendMail As Boolean)
        Dim AircraftList As New StringBuilder
        Dim WorkOrderNumberList As New StringBuilder
        Dim AircraftTypeList As New StringBuilder
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As DataSet
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass 'Added By Vikrant On 12-Mar-2014 For All13032014

        If AppSettings("ClientCode") = "Novo" Then 'Added By Prashant On 07-Dec-2018 For NovoAir07122018
            myReport = New crptRequisitionDetailNovoAir
            ds = New dsRequisitionNew
            da.Fill(ds, mRequisitionNew)
            da.Fill(ds, mRequisitionNew.RequisitionItemsNew)
        Else
            If mRequisitionNew.TransTypeID = Util.Trans.EngineeringRequisition Or mRequisitionNew.TransTypeID = Util.Trans.WorkShopRequisition Then
                If mRequisitionNew.ReqTypeID = 1 Then 'Or mRequisitionNew.TransTypeID = Util.Trans.WorkShopRequisition Then 'Part Request


                    ds = New dsIssueAgainstRequisitionItem
                    If AppSettings("ClientCode") = "STR" Then
                        myReport = New crptIssueAgainstRequisitionItemStarAir
                    Else
                        myReport = New crptIssueAgainstRequisitionItem
                    End If

                    Dim mIssueAgainstRequisitionItem As IssueAgainstRequisitionItem = IssueAgainstRequisitionItem.GetIssueAgainstRequisitionItem(mRequisitionNew.ID, ClientCode:=AppSettings("ClientCode"))
                    Dim mTemp As New Hashtable
                    For i As Integer = 0 To mIssueAgainstRequisitionItem.Count - 1
                        If Not mTemp.ContainsValue(mIssueAgainstRequisitionItem(i).RegNo) Then
                            mTemp.Add(i, mIssueAgainstRequisitionItem(i).RegNo)
                            AircraftList.Append(mTemp(i) + ",")
                        End If
                    Next

                    If AircraftList.Length > 0 Then
                        AircraftList.Replace(",", "", AircraftList.Length - 1, 1)
                    End If
                    da.Fill(ds, mIssueAgainstRequisitionItem)
                Else 'Part Purchase
                    If (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022 
                        myReport = New crptPurchaseOrderAgainstRequisitionItemDeccan
                    Else
                        myReport = New crptPurchaseOrderAgainstRequisitionItem
                    End If

                    ds = New dsPurchaseOrderAgainstRequisitionItem
                    Dim mPurchaseOrderAgainstRequisitionItem As PurchaseOrderAgainstRequisitionItem = PurchaseOrderAgainstRequisitionItem.GetPurchaseOrderAgainstRequisitionItem(mRequisitionNew.ID)
                    da.Fill(ds, mPurchaseOrderAgainstRequisitionItem)
                End If
            ElseIf mRequisitionNew.TransTypeID = Util.Trans.StoresRequisition Then
                myReport = New crptMaterialReplanishmentNote
                ds = New dsIssueAgainstRequisitionItem
                Dim mMaterialReplanishmentNote As MaterialReplanishmentNote = MaterialReplanishmentNote.GetMaterialReplanishmentNote(mRequisitionNew.ID.ToString)
                da.Fill(ds, mMaterialReplanishmentNote)
            ElseIf mRequisitionNew.TransTypeID = Util.Trans.PlanningRequisition Then
                Dim mTemp As New Hashtable
                Dim mTempWoNoList As New Hashtable
                Dim mTempAircraftTypeList As New Hashtable
                mRequisitionNew = RequisitionNew.GetRequisition(mRequisitionNew.ID) ' Need to fetch again to get saved user name. As uer name changed on this page again on authrize if other user authorized it.
                For i As Integer = 0 To mRequisitionNew.RequisitionItemsNew.Count - 1
                    If Not mTemp.ContainsValue(mRequisitionNew.RequisitionItemsNew(i).RegNo) And mRequisitionNew.RequisitionItemsNew(i).RegNo <> "" Then
                        mTemp.Add(i, mRequisitionNew.RequisitionItemsNew(i).RegNo)
                        AircraftList.Append(mTemp(i) + ",")
                    End If

                    If Not mTempWoNoList.ContainsValue(mRequisitionNew.RequisitionItemsNew(i).WONoNRCNo) And mRequisitionNew.RequisitionItemsNew(i).WONoNRCNo <> "" Then
                        mTempWoNoList.Add(i, mRequisitionNew.RequisitionItemsNew(i).WONoNRCNo)
                        WorkOrderNumberList.Append(mTempWoNoList(i) + ",")
                    End If

                    If AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IRM" Then
                        mRequisitionCustomer = RequisitionCustomer.GetCustomer(mRequisitionNew.RequisitionItemsNew(i).MachineID)
                        If Not mTempAircraftTypeList.ContainsValue(mRequisitionCustomer.AircraftType) And mRequisitionCustomer.AircraftType <> "" Then
                            mTempAircraftTypeList.Add(i, mRequisitionCustomer.AircraftType)
                            AircraftTypeList.Append(mTempAircraftTypeList(i) + ",")
                        End If
                        RequiredByDate = CDate(mRequisitionNew.ReqDate).AddDays(mRequisitionNew.RequisitionItemsNew(i).Days).ToString(AppSettings("DateFormat"))
                    Else
                        If Not mRequisitionNew.RequisitionItemsNew(i).MachineID.Equals(Guid.Empty) Then
                            mRequisitionCustomer = RequisitionCustomer.GetCustomer(mRequisitionNew.RequisitionItemsNew(i).MachineID)
                            CustName = mRequisitionCustomer.CustomerName
                            CustAddress = mRequisitionCustomer.CustomerAddress
                            AircraftType = mRequisitionCustomer.AircraftType
                            Exit For
                        End If
                    End If
                Next

                If AircraftList.Length > 0 Then
                    AircraftList.Replace(",", "", AircraftList.Length - 1, 1)
                End If
                If WorkOrderNumberList.Length > 0 Then
                    WorkOrderNumberList.Replace(",", "", WorkOrderNumberList.Length - 1, 1)
                End If

                If AircraftTypeList.Length > 0 Then
                    AircraftTypeList.Replace(",", "", AircraftTypeList.Length - 1, 1)
                    AircraftType = AircraftTypeList.ToString
                End If

                If (AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA") Then
                    myReport = New crptPlanningRequisitionDetailBRD
                ElseIf (AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IRM") Then
                    myReport = New crptRequisitionDetailNewForStarAir
                ElseIf AppSettings("ClientCode") = "Heligo" Then
                    myReport = New crptPlanningRequisitionDetailHeligo
                Else
                    myReport = New crptPlanningRequisitionDetail
                End If
                ds = New dsRequisitionNew
                da.Fill(ds, mRequisitionNew)
                da.Fill(ds, mRequisitionNew.RequisitionItemsNew)
            End If
        End If
        'End
        Dim mCompanyDetail As New CompanyDetail

        Dim mReport As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
                                      mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, "", _
                                      mRequisitionNew.RequisitionNo, mRequisitionNew.UserName, txtEmployee.Text, mRequisitionNew.RecommendedBy, _
                                      mRequisitionNew.Supervisor, AppSettings("Product Version"), AppSettings("SINote"), mRequisitionNew.TransTypeID.ToString, _
                                      , SearchStr8:=mRequisitionNew.AuthorizedBy, SearchStr9:=IIf(mRequisitionNew.TransTypeID = 65, AircraftList.ToString + "/" + cmbRequisitionEngineeringBranches.SelectedItem.ToString, cmbRequisitionEngineeringBranches.SelectedItem.ToString), _
                                      SearchStr10:=AppSettings("Logo"), _
                                      SearchStr11:=AppSettings("ClientCode"), SearchStr12:=CustName, SearchStr13:=CustAddress, SearchStr14:=AircraftType, _
                                      SearchStr15:=WorkOrderNumberList.ToString, SearchStr16:=AircraftList.ToString, SearchStr17:=RequiredByDate, SearchStr18:=mTransactionList.Item(mRequisitionNew.TransTypeID).FormRevisionNo, SearchStr19:=mTransactionList.Item(mRequisitionNew.TransTypeID).FormRevisionDate)

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, mReport)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        If IsSendMail = True Then
            If User.Identity.Name.ToUpper = "BTPLADMIN" Or User.Identity.Name.ToUpper = "BYTZADMIN" Then ' BYTZADMIN For Deccan 'Added by Prashant 15-Oct-2019  Then
                'Do Nothing 
                Exit Sub
            End If
            Dim str As String
            str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Following Parts(s) has been requested by <b>" + User.Identity.Name + "</b>" + " in Requisition " + mRequisitionNew.RequisitionNo + " ,Created on " + New SmartDate(mRequisitionNew.ReqDateFormatted.ToString).FormattedText + " in FlyPal System." + "</font></P></br> ")


            str = str + ("<TABLE BORDER=1 Style=""border-collapse: collapse"" BORDER-COLOR=""black"" ID=""Table2"">")
            ''str = str + ("<tr>" & "<td align=""center"" style=""background-color: #829e82; color: black;"">" & "<font face=""Calibri""><b>Sr. No.</b>" & "</font>" & "</td><td align=""center"" width=""200"" style=""background-color: #829e82; color: black;"" >" & "<font face=""Calibri""><b>Part No</b>" & "</font>" & "</td><td align=""center"" width=""200"" style=""background-color: #829e82; color: black;"" >" & "<font face=""Calibri""><b>Description</b>" & "</font>" & "</td><td align=""center"" style=""background-color: #829e82; color: black;"">" & "<font face=""Calibri""><b>Qty</b>" & "</font>" & "</td>  <td align=""center"" style=""background-color: #829e82; color: black;"">" & "<font face=""Calibri""><b>Reg</b>" & "</font>" & "</td>  <td align=""center"" style=""background-color: #829e82; color: black;"">" & "<font face=""Calibri""><b>WO.No.</b>" & "</font>" & "</td></tr>")
            str = str + ("<tr>" & "<td align=""center"" style=""background-color: #E4E2E1; color: black;"">" & "<font face=""Calibri""><b>Sr. No.</b>" & "</font>" & "</td><td align=""center"" width=""200"" style=""background-color: #E4E2E1; color: black;"" >" & "<font face=""Calibri""><b>Part No</b>" & "</font>" & "</td><td align=""center"" width=""200"" style=""background-color: #E4E2E1; color: black;"" >" & "<font face=""Calibri""><b>Description</b>" & "</font>" & "</td><td align=""center"" style=""background-color: #E4E2E1; color: black;"">" & "<font face=""Calibri""><b>Qty</b>" & "</font>" & "</td> <td align=""center"" style=""background-color: #E4E2E1; color: black;"">" & "<font face=""Calibri""><b>UOM</b>" & "</font>" & "</td> <td align=""center"" style=""background-color: #E4E2E1; color: black;"">" & "<font face=""Calibri""><b>Reg</b>" & "</font>" & "</td>  <td align=""center"" style=""background-color: #E4E2E1; color: black;"">" & "<font face=""Calibri""><b>WO.No.</b>" & "</font>" & "</td> <td align=""center"" style=""background-color: #E4E2E1; color: black;"">" & "<font face=""Calibri""><b>Requirement Reason</b>" & "</font>" & "</td> <td align=""center"" style=""background-color: #E4E2E1; color: black;"">" & "<font face=""Calibri""><b>Remark</b>" & "</font>" & "</td></tr>")
            For i As Integer = 0 To mRequisitionNew.RequisitionItemsNew.Count - 1
                str = str + ("<TR>")

                str = str + ("<TD WIDTH=20px >")
                str = str + ("<font face=""Calibri"">")
                str = str + (mRequisitionNew.RequisitionItemsNew(i).SrNo.ToString) + "."
                str = str + ("</font>")
                str = str + ("</TD>")

                str = str + ("<TD WIDTH=200px >")
                str = str + ("<font face=""Calibri"">")
                str = str + (mRequisitionNew.RequisitionItemsNew(i).PartNo)
                str = str + ("</font>")
                str = str + ("</TD>")

                str = str + ("<TD WIDTH=200px >")
                str = str + ("<font face=""Calibri"">")
                str = str + (mRequisitionNew.RequisitionItemsNew(i).Description)
                str = str + ("</font>")
                str = str + ("</TD>")

                str = str + ("<TD WIDTH=50px >")
                str = str + ("<font face=""Calibri"">")
                str = str + (mRequisitionNew.RequisitionItemsNew(i).RequestedQty.ToString)
                str = str + ("</font>")
                str = str + ("</TD>")

                'Added by Shital on 01-Oct-2021
                str = str + ("<TD WIDTH=50px >")
                str = str + ("<font face=""Calibri"">")
                str = str + (mRequisitionNew.RequisitionItemsNew(i).Unit.ToString)
                str = str + ("</font>")
                str = str + ("</TD>")
                '------------

                str = str + ("<TD WIDTH=20px >")
                str = str + ("<font face=""Calibri"">")
                str = str + (mRequisitionNew.RequisitionItemsNew(i).RegNo.ToString)
                str = str + ("</font>")
                str = str + ("</TD>")

                str = str + ("<TD WIDTH=20px >")
                str = str + ("<font face=""Calibri"">")
                str = str + IIf(mRequisitionNew.RequisitionItemsNew(i).WONo.ToString = "", "-", mRequisitionNew.RequisitionItemsNew(i).WONo.ToString)
                str = str + ("</font>")
                str = str + ("</TD>")


                'Added by Shital on 01-Oct-2021
                str = str + ("<TD WIDTH=50px >")
                str = str + ("<font face=""Calibri"">")
                str = str + (mRequisitionNew.RequisitionItemsNew(i).ReasonForRequest.ToString)
                str = str + ("</font>")
                str = str + ("</TD>")

                str = str + ("<TD WIDTH=50px >")
                str = str + ("<font face=""Calibri"">")
                str = str + (mRequisitionNew.RequisitionItemsNew(i).Remark.ToString)
                str = str + ("</font>")
                str = str + ("</TD>")
                '------------

                str = str + ("</TR>")
            Next

            str = str + ("</TABLE>")

            str = str + ("<p><font face=""Calibri"">")
            str = str + ("<font face=""Calibri"">Please Login to FlyPal® for detailed information." + "</font> ")
            str = str + ("</body></html>")

            'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
            Session("UserEmailID") = mTransactionList.Item(mRequisitionNew.TransTypeID).SendToMailID
            Session("MailsRequire") = mTransactionList.Item(mRequisitionNew.TransTypeID).MailsRequire
            Session("SmtpHost") = mTransactionList.Item(mRequisitionNew.TransTypeID).SmtpHost
            Session("SmtpPort") = mTransactionList.Item(mRequisitionNew.TransTypeID).SmtpPort
            Session("SmtpUser") = mTransactionList.Item(mRequisitionNew.TransTypeID).SmtpUser
            Session("SmtpPassword") = mTransactionList.Item(mRequisitionNew.TransTypeID).SmtpPassword
            Session("FormRevisionNo") = mTransactionList.Item(mRequisitionNew.TransTypeID).FormRevisionNo
            Session("FormRevisionDate") = mTransactionList.Item(mRequisitionNew.TransTypeID).FormRevisionDate
            '----------------------

            SendMailFile.SendMailFile(Session("CrystalReport"), User.Identity.Name, "Requisition Details", mRequisitionNew.RequisitionNo, Info:=str, VendorEmailID:="", ToMailID:=Session("UserEmailID"), CCMailID:="", BCCMailID:="", Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                       SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTransDetail", MessageBox.Show("Mail Sent Successfully", False), True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        End If

    End Sub
    Private Sub ControlVisibility()
        Dim txtValue As TextBox
        For i As Integer = 0 To dgRequisitionItems.Rows.Count - 1
            txtValue = CType(Me.dgRequisitionItems.Rows(i).FindControl("txtQty"), TextBox)
            txtValue.Enabled = CType(IIf(mRequisitionNew.StatusID > 1, False, True), Boolean)
        Next
        btnAuthorized.Visible = (Not mRequisitionNew.IsNew) And (mRequisitionNew.StatusID = 1)
        txtRecommendedBy.Visible = IIf(rdoPartPurchase.Checked = True, True, False)
        lblRecommendedBy.Visible = IIf(rdoPartPurchase.Checked = True, True, False)
        If mRequisitionNew.StatusID > 1 Then
            txtEmployee.Enabled = False
            cmbRequisitionEngineeringBranches.Enabled = False
            rdoPartRequest.Enabled = False
            rdoPartPurchase.Enabled = False
            dgRequisitionItems.Columns(11).Visible = False
            ''dgRequisitionItems.Columns(12).Visible = False
            cmbLocationList.Enabled = False
            txtText.Enabled = False
            txtNo.Enabled = False
            txtRequisitionDate.Enabled = False
            btnCombo.Enabled = False
            btnSave.Visible = False
            txtRecommendedBy.Enabled = False
            txtSupervisor.Enabled = False
            cmbWorkShop.Enabled = False
        Else
            'Commented and Added By Vikrant On 26-Feb-2021 For Heligo01032021
            'txtEmployee.Enabled = True
            mUser = SI.UTILITY.User.GetUser(User.Identity.Name)
            If AppSettings("ClientCode") = "Heligo" And mUser.EmployeeName <> "" Then
                txtEmployee.Enabled = False
            Else
                txtEmployee.Enabled = True
            End If
            'End
            cmbRequisitionEngineeringBranches.Enabled = True
            rdoPartRequest.Enabled = True
            rdoPartPurchase.Enabled = True
            dgRequisitionItems.Columns(11).Visible = True
            ''  dgRequisitionItems.Columns(12).Visible = True
            cmbLocationList.Enabled = True
            'txtText.Enabled = IIf(AppSettings("ClientCode") = "KAS", False, True) 'Commented and added by Prashant on 17-Jun-2021 Kasas17062021
            If AppSettings("ClientCode") = "KAS" Then
                If mTransTypeID = Util.Trans.WorkShopRequisition Then
                    txtText.Enabled = True
                Else
                    txtText.Enabled = False
                End If
            Else
                txtText.Enabled = True
            End If
            txtNo.Enabled = True
            If mRequisitionNew.RequisitionItemsNew.Count > 0 Then
                txtRequisitionDate.Enabled = False
            Else
                If AppSettings("ClientCode") = "IND" Then  'Added By Prashant 12-Aug-2019 As per Points in mail
                    txtRequisitionDate.Enabled = False
                Else
                    txtRequisitionDate.Enabled = True
                End If
            End If
            btnCombo.Enabled = True
            btnSave.Visible = True
            cmbWorkShop.Enabled = True
        End If

        If mRequisitionNew.TransTypeID = Util.Trans.EngineeringRequisition Then
            dgRequisitionItems.Columns(1).Visible = IIf(mRequisitionNew.ReqTypeID = 1, False, True)
            'dgRequisitionItems.Columns(5).Visible = IIf(mRequisitionNew.ReqTypeID = 1, True, False)
            dgRequisitionItems.Columns(6).Visible = True
            dgRequisitionItems.Columns(7).Visible = True
            dgRequisitionItems.Columns(9).Visible = IIf(mRequisitionNew.ReqTypeID = 1, False, True)
        ElseIf mRequisitionNew.TransTypeID = Util.Trans.WorkShopRequisition Then
            dgRequisitionItems.Columns(1).Visible = IIf(mRequisitionNew.ReqTypeID = 1, False, True)
            'dgRequisitionItems.Columns(5).Visible = True
            dgRequisitionItems.Columns(6).Visible = False
            dgRequisitionItems.Columns(7).Visible = False
            dgRequisitionItems.Columns(9).Visible = False
        ElseIf mRequisitionNew.TransTypeID = Util.Trans.StoresRequisition Then
            dgRequisitionItems.Columns(1).Visible = False
            'dgRequisitionItems.Columns(5).Visible = True
            dgRequisitionItems.Columns(6).Visible = False
            dgRequisitionItems.Columns(7).Visible = False
            dgRequisitionItems.Columns(9).Visible = False
        ElseIf mRequisitionNew.TransTypeID = Util.Trans.PlanningRequisition Then
            dgRequisitionItems.Columns(1).Visible = True
            'dgRequisitionItems.Columns(5).Visible = True
            dgRequisitionItems.Columns(6).Visible = True
            dgRequisitionItems.Columns(7).Visible = True
            dgRequisitionItems.Columns(9).Visible = False
        End If
        If Not IsInRole(Rights.Authorized) Then
            btnAuthorized.Enabled = False
            btnAuthorized.ToolTip = "You are not authorized user "
            btnSendMail.Enabled = False
            btnSendMail.ToolTip = "You are not authorized user "
            'Added By Vikrant On 28-Sep-2015 For All28092015
            btnCancel.Enabled = False
            btnCancel.ToolTip = "You are not authorized user "
            'End
        End If
        'If rdoEngg.Checked Then
        '    cmbAdd.Items.Remove(cmbAdd.Items.FindByText("Parts"))
        'End If
        If (mRequisitionNew.RequisitionItemsNew.Count > 0) Then
            rdoPartRequest.Enabled = False
            rdoPartPurchase.Enabled = False
            btnSelectWONo.Enabled = False 'Added By Prashant 17-Feb-2020
            If Session("OpenFromPartNoBinCard") = "OpenFromPartNoBinCard" Then
                Session.Remove("OpenFromPartNoBinCard")
                cmbRequisitionEngineeringBranches.Enabled = True
                btnSelectWONo.Visible = False

                'Else  'Commented By Prashant on 27-Oct-2022 to keep open though items count > 0
                'cmbRequisitionEngineeringBranches.Enabled = False 'Commented By Prashant on 27-Oct-2022 to keep open though items count > 0
            End If
            cmbWorkShop.Enabled = False
        Else
            btnSelectWONo.Enabled = True 'Added By Prashant 17-Feb-2020
        End If
        Dim URL As Stack = CType(Session("ReqURLFromWO"), Stack)
        If Not URL Is Nothing Then
            If URL.Count > 0 Then
                btnSelectWONo.Visible = False
            End If
        End If
        If mRequisitionNew.TransTypeID = Util.Trans.WorkShopRequisition Then
            cmbRequisitionEngineeringBranches.Enabled = False
            'rdoPartPurchase.Enabled = False Commented By Vikrant On 02-May-2016 As Per Dipendra's Requirement
        End If
        btnSendMail.Visible = IIf(mRequisitionNew.StatusID = 2, True, False) 'Added by Vikrant on 16-Jul-2012 For All16072012-4
        'Added By Vikrant On 13-Mar-2014 For All13032014
        'Dim flag As Boolean = False
        'If mRequisitionNew.TransTypeID = Util.Trans.EngineeringRequisition Then
        '    For Each mRequisitionItemNew As RequisitionItemNew In mRequisitionNew.RequisitionItemsNew
        '        If mRequisitionItemNew.ItemID.Equals(Guid.Empty) Then
        '            btnPurchasePrint.Visible = True
        '            flag = True
        '            Exit For
        '        End If
        '    Next
        '    If flag = False Then
        '        btnPurchasePrint.Visible = False
        '    End If
        'End If
        'End
        btnPrint.Enabled = IIf((mRequisitionNew.RequisitionItemsNew.Count = 0 Or mRequisitionNew.IsNew), False, True)
        btnCancel.Visible = (Not mRequisitionNew.IsNew) And (mRequisitionNew.StatusID = 2) 'Added By Vikrant On 28-Sep-2015 For All28092015
        'btnPurchasePrint.Enabled = IIf((mRequisitionNew.RequisitionItemsNew.Count = 0 Or mRequisitionNew.IsNew), False, True)
    End Sub
    Private Sub Save()
        'Authentication
        If Not mRequisitionNew.ReqDate Is System.DBNull.Value Then
            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
            If mCheck.WebAuthentication = True Then
                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")

                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                '---------------------------------

                If DateDiff(DateInterval.Day, CDate(mRequisitionNew.ReqDate), maxAllowableDate) < 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Sales Requisition. <br> Requisition Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If
        End If
        'Authentication

        Dim RequisitionClone As RequisitionNew
        RequisitionClone = mRequisitionNew.Clone
        Try
            If Not mRequisitionNew.RequisitionItemsNew.Count = 0 Then
                setObject()
                setComboDetails()
                'Added By Shweta On 07-Aug-2013 For ALL01082013
                Dim title As String = "Save Alert !"
                Dim message As String = ""
                mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mRequisitionNew.EmployeeID.ToString, mRequisitionNew.ReqDate)
                If (mEmployeeStatus(0).Information <> "") Then
                    message = mEmployeeStatus(0).Information
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAlertMessage", MessageBox.Show(title, message, , False), True)
                    Exit Sub
                End If
                'End
                'Added by Utkarsh ON 21-Nov-2013 FOr TransTextSeries
                'Check if Requisition is blank then call TransTextSeries UI

                If (mRequisitionNew.IsNew) And (mRequisitionNew.Text = "") Then

                    Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(mTransTypeID, mRequisitionNew.ReqDateFormatted) 'All13082014

                    If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mTransTypeID) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mTransTypeID) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mTransTypeID).TransText = "")) Then 'All13082014

                        'Dim str = "openledgersame('wfRequisition_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "');"

                        'Session("BackPagestr_ForTransSeries") = str
                        Dim str = "<script language='javascript'>openledgersame('wfRequisition_Ajax.aspx');</script>"

                        Session("BackPagestr_ForTransSeries") = str

                        Session("TransName_ForTransSeries") = "Requisition"
                        Session("TransTypeID_ForTransSeries") = mTransTypeID  'All13082014
                        Session("TransDate_ForTransSeries") = mRequisitionNew.ReqDateFormatted

                        MSGBoxCtrl.show("Requisition Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "RequisitionTransTextSeriesAlert")
                        Exit Sub

                    Else
                        Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

                        If mAutoRenewTransTextSeries.IsRenewed Then
                            With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mTransTypeID) 'All13082014
                                mRequisitionNew.Text = .TransText
                                mRequisitionNew.No = .StartingTransNo
                                txtText.DataBind()
                                txtNo.DataBind()
                                upnlReqDetails.Update()
                            End With
                        Else
                            'Dim str = "openledgersame('wfRequisition_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "');"

                            'Session("BackPagestr_ForTransSeries") = str
                            Dim str = "<script language='javascript'>openledgersame('wfRequisition_Ajax.aspx');</script>"

                            Session("BackPagestr_ForTransSeries") = str
                            Session("TransName_ForTransSeries") = "Requisition"
                            Session("TransTypeID_ForTransSeries") = mTransTypeID  'All13082014
                            Session("TransDate_ForTransSeries") = mRequisitionNew.ReqDateFormatted

                            MSGBoxCtrl.show("Requisition Transaction Series", "system does not find transaction series for this transaction. Click Ok to enter transaction series.", "", MsgBoxStyle.OkOnly, "RequisitionTransTextSeriesAlert")
                            Exit Sub
                        End If
                    End If
                    'Else
                    '    If mRequisitionNew.IsNew Then
                    '        mRequisitionNew.fetchLastTranTextForRequisition()
                    '    End If
                End If

                'End
                mRequisitionNew.Save()

                Dim mName As String = mTransactionList(CType(mTransTypeID, Util.Trans)).Name
                mRequisitionDetail = mRequisitionNew.RequisitionNo + " Dated : " + mRequisitionNew.ReqDateFormatted + " Requested By : " + txtEmployee.Text + " Requisition : " + mName + " Type : " + IIf(rdoPartPurchase.Checked, "Part Purchase", "Part Request") + " User : " + User.Identity.Name

                If mRequisitionNew.StatusID = 2 Then
                    SendPUSHNotification(mRequisitionNew) 'Added by Prashant on 26-Oct-2021
                    MarkLog(Util.Action.Authorize, mName, mRequisitionDetail, Util.ErrorType.NoError, mRequisitionNew.ID, EventLogID)
                ElseIf mRequisitionNew.StatusID = 4 Then
                    MarkLog(Util.Action.Cancel, mName, mRequisitionDetail, Util.ErrorType.NoError, mRequisitionNew.ID, EventLogID)
                Else
                    MarkLog(Util.Action.Save, mName, mRequisitionDetail, Util.ErrorType.NoError, mRequisitionNew.ID, EventLogID)
                End If

                mRequisitionNew.MarkClean()
                lblTitle.Text = "Requisition ( Saved ...)"
                Session("mRequisitionNew") = mRequisitionNew
                DataFieldBind()
                SetPage()
                ControlVisibility()
                upnlTitle.Update()
                upnlReqDetails.Update()
                upnlStatus.Update()
                upnlActionBtn.Update()
                upnlInfoLabel.Update()
                upnlGridView.Update()
                upnlReqItemAdd.Update()
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Requisition can not be saved without Item.", MsgBoxStyle.OkOnly, "")
                mRequisitionNew = RequisitionClone
                If mRequisitionNew.StatusID = 2 Then
                    mRequisitionNew.StatusID = 1
                End If
                Session("mRequisitionNew") = mRequisitionNew
                Exit Sub
            End If
        Catch ex As SqlClient.SqlException
            Session("RequisitionClone") = RequisitionClone
            If ex.Message.Contains("CK_tabReq_NoRequired") Then
                MSGBoxCtrl.show("Save Alert!", "Requisition No. should be greater than zero.", "", MsgBoxStyle.OkOnly, "")
            Else
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            End If

        Finally
            RequisitionClone = Nothing
        End Try
    End Sub
    Private Sub RemoveSessions() 'ALP
        Session.Remove("PartNoStatus")
        Session.Remove("DescriptionStatus")
        Session.Remove("Unit")
        Session.Remove("mStockPartStatus")
        Session.Remove("mOnOrderPartStatus")
        Session.Remove("mReturnablePartStatus")
        Session.Remove("mTransitPartList")
        Session.Remove("mWorkShopList")
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""

        Select Case mTransTypeID
            Case Util.Trans.EngineeringRequisition
                IsInRoleString = "EngineeringRequisition"
            Case Util.Trans.StoresRequisition
                IsInRoleString = "StoresRequisition"
            Case Util.Trans.WorkShopRequisition
                IsInRoleString = "WorkShopRequisition"
            Case Util.Trans.PlanningRequisition
                IsInRoleString = "PlanningRequisition"

        End Select
        'Depending upon decided IsInRole String; checkign Rights of the User
        Select Case CheckFor
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case Rights.Save
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case Rights.Delete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case Rights.View
                Return User.IsInRole(IsInRoleString + "View")
            Case Rights.Print
                Return User.IsInRole(IsInRoleString + "Print")
            Case Rights.FindNow
                Return User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "View") Or User.IsInRole(IsInRoleString + "Edit") Or User.IsInRole(IsInRoleString + "Delete")
            Case Rights.Authorized
                Return User.IsInRole(IsInRoleString + "Authorized")
        End Select
    End Function
    'Private Sub SetEmpID()
    '    If hdnEmpId.Value <> String.Empty Then
    '        EmpID = hdnEmpId.Value.ToString
    '        EmpName = txtEmployee.Text
    '    Else

    '    End If
    'End Sub
    'Private Sub ClearEmpID()
    '    hdnEmpId.Value = String.Empty
    '    EmpID = String.Empty
    'End Sub
    Public Sub SendPUSHNotification(ByVal tmpRequisitionNew As RequisitionNew) 'Added by Prashant on 26-Oct-2021
        Dim PreviousStepStatus As Boolean = False

        'Step # 1: Get User Devices

        Dim mUserDeviceList As APP_UserDeviceList = APP_UserDeviceList.GetUserDeviceList(3) '2:Requisition

        If mUserDeviceList.Count = 0 Then
            PreviousStepStatus = False
        Else
            PreviousStepStatus = True
        End If

        If PreviousStepStatus = False Then Exit Sub '----------------------------------------------------------------------------------------------------

        'Step # 2: Record PUSH Notification in the table

        Dim UserIDs(50) As Guid
        UserIDs = (From c As APP_UserDeviceList.UserDeviceinfo In mUserDeviceList
                            Select (c.UserID)).Distinct().ToArray()

        Dim Notifications(UserIDs.Count - 1) As APP_UserNotification

        For i As Integer = 0 To UserIDs.Count - 1

            If UserIDs(i).Equals(Guid.Empty) Then Exit For

            Dim mAPP_UserNotification As APP_UserNotification = APP_UserNotification.NewAPP_UserNotification(Guid.NewGuid)

            Try
                With mAPP_UserNotification
                    .UserID = UserIDs(i)
                    .SentOn = Now
                    .Message = "Requisition:- " + tmpRequisitionNew.RequisitionNo + " Dated:- " + tmpRequisitionNew.ReqDateFormatted + " Authorized By:- " + tmpRequisitionNew.AuthorizedBy '"Parts(s) has been requested by " + User.Identity.Name + " in Requisition " + mRequisitionNew.RequisitionNo + " ,Created on " + New SmartDate(mRequisitionNew.ReqDateFormatted.ToString).FormattedText + " in FlyPal System."
                    .ModuleType = 3 'Requisition
                    .ModuleID = tmpRequisitionNew.ID
                End With

                mAPP_UserNotification = CType(mAPP_UserNotification.Save, APP_UserNotification)

                Notifications(i) = mAPP_UserNotification

                PreviousStepStatus = True
            Catch ex As Exception
                PreviousStepStatus = False
            End Try
        Next

        'Dim mAPP_UserNotification As APP_UserNotification = APP_UserNotification.NewAPP_UserNotification(Guid.NewGuid)

        If PreviousStepStatus = False Then Exit Sub '----------------------------------------------------------------------------------------------------

        For Each Notification As APP_UserNotification In Notifications

            Dim errorcount As Integer = 0

StartStep3:

            'Step # 3: Trigger PUSH Notification

            errorcount = errorcount + 1

            System.Net.ServicePointManager.Expect100Continue = True
            System.Net.ServicePointManager.SecurityProtocol = 3072 'System.Net.SecurityProtocolType.Tls

            Dim request = TryCast(System.Net.WebRequest.Create("https://onesignal.com/api/v1/notifications"), System.Net.HttpWebRequest)

            request.KeepAlive = True
            request.Method = "POST"
            request.ContentType = "application/json; charset=utf-8"

            request.Headers.Add("authorization", "Basic YmE0YTUwZDgtMmJkYS00MjMzLWI5NjgtZTkxZmE5MzQ0NzMw")

            Dim serializer = New JavaScriptSerializer()

            'Forming Notification Detail URL
            '
            '
            Dim index As Integer = HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("wfRequisition_Ajax.aspx")
            Dim urlNotificationDetail As String = ""
            urlNotificationDetail = HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, index) + "APP/Launcher.aspx?NotificationID=" + Notification.ID.ToString + "&ModuleID=" + tmpRequisitionNew.ID.ToString + "&username=" + Notification.UserName + "&EventLogSessionID=" + Guid.NewGuid.ToString + "&ModuleTypeID=3"


            Dim filterObject As Object()
            ReDim filterObject(((mUserDeviceList.Count - 1) * 2) + 1)

            Dim idx As Integer = 0
            Dim Ridx As Integer = 0
            For Each info As APP_UserDeviceList.UserDeviceinfo In mUserDeviceList

                If Notification.UserID.Equals(info.UserID) Then


                    If idx = 0 Then
                        filterObject(idx) = New With {Key .field = "tag", Key .key = "DeviceID", Key .value = mUserDeviceList(0).DeviceID.ToString}
                        idx = idx + 1
                    Else
                        Ridx = Ridx + 1

                        filterObject(idx) = New With {Key .[operator] = "OR"}
                        idx = idx + 1

                        filterObject(idx) = New With {Key .field = "tag", Key .key = "DeviceID", Key .value = mUserDeviceList(Ridx).DeviceID.ToString}
                        idx = idx + 1
                    End If

                End If

            Next

            Dim obj = New With {Key .app_id = "f877b4d2-b6e5-4595-a381-87165f6e46a0", Key .contents = New With {Key .en = Notification.Message}, Key .headings = New With {Key .en = "FlyPal"}, Key .filters = filterObject, Key .data = New With {Key .url = urlNotificationDetail.ToString}}

            '---------------------

            Dim param = serializer.Serialize(obj)
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(param)

            Dim responseContent As String = Nothing

            Try

                Using writer = request.GetRequestStream()
                    writer.Write(byteArray, 0, byteArray.Length)
                End Using

                Using response As System.Net.HttpWebResponse = request.GetResponse()

                    Using reader = New System.IO.StreamReader(response.GetResponseStream())

                        responseContent = reader.ReadToEnd()

                    End Using

                End Using

            Catch ex As System.Net.WebException
                System.Diagnostics.Debug.WriteLine(ex.Message)
                System.Diagnostics.Debug.WriteLine(New System.IO.StreamReader(ex.Response.GetResponseStream()).ReadToEnd())

                If errorcount <= 3 Then GoTo StartStep3

            End Try

            System.Diagnostics.Debug.WriteLine(responseContent)
        Next

    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind(Optional ByVal sender As Object = Nothing, Optional ByVal e As System.EventArgs = Nothing)
        mLocationList = LocationList.GetLocationList(0, , , , , , True)
        cmbLocationList.DataSource = mLocationList
        Session("mLocationList") = mLocationList

        'mEmployeeList = EmployeeList.GetEmployeeList(, , "<SELECT>")
        'cmbEmployeeList.DataSource = mEmployeeList
        'Session("mEmployeeList") = mEmployeeList

        dgRequisitionItems.DataSource = mRequisitionNew.RequisitionItemsNew
        txtRequisitionDate.Text = CDate(mRequisitionNew.ReqDate).ToString(AppSettings("DateFormat"))

        mRequisitionEngineeringBranchesList = RequisitionEngineeringBranchesList.GetRequisitionEngineeringBranchesList(mTransTypeID)
        cmbRequisitionEngineeringBranches.DataSource = mRequisitionEngineeringBranchesList

        mTransactionList = TransactionList.GetTransactionList()
        Session("mTransactionList") = mTransactionList

        mWorkShopList = WorkShopList.GetWorkShopList(0, , , True, "(SELECT)")
        Session("mWorkShopList") = mWorkShopList
        cmbWorkShop.DataSource = mWorkShopList

        mUser = SI.UTILITY.User.GetUser(User.Identity.Name) 'Added By Prashant 11-Dec-2018 ALL11122018
        If mRequisitionNew.EmployeeName = "" And mUser.EmployeeName <> "" And mRequisitionNew.IsNew = True Then
            txtEmployee.Text = mUser.EmpNoName
            Call txtEmployee_TextChanged(sender, e)
        Else                                                'End of Added By Prashant 11-Dec-2018 ALL11122018
            txtEmployee.Text = mRequisitionNew.EmployeeName
        End If

        DataBind()

        If mTransTypeID = Util.Trans.PlanningRequisition Then
            cmbIndentType.SelectedValue = mRequisitionNew.IndentTypeID.ToString
        End If

        'If mTransTypeID = Util.Trans.WorkShopRequisition Then
        '    cmbRequisitionEngineeringBranches.SelectedValue = 3
        'End If

        Dim txtValue As TextBox
        Dim mRequisitionItemNew As RequisitionItemNew
        Dim i As Integer = 0
        For Each mRequisitionItemNew In mRequisitionNew.RequisitionItemsNew
            With mRequisitionItemNew
                txtValue = CType(Me.dgRequisitionItems.Rows(i).FindControl("txtQty"), TextBox)
                txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtQty').value)")
            End With
            i = i + 1
        Next
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        'SetEmpID()
        If custValidator.ControlToValidate = "cmbWorkShop" Then
            If mRequisitionNew.TransTypeID = Util.Trans.WorkShopRequisition Then
                If cmbWorkShop.SelectedIndex <= 0 Then
                    custValidator.ErrorMessage = "WorkShop Required."
                    e.IsValid = False
                End If
            End If
        End If
        If custValidator.ControlToValidate = "txtEmployee" Then
            If txtEmployee.Text = "" Then 'Or mRequisitionNew.EmployeeID.Equals(Guid.Empty) Then
                e.IsValid = False
                custValidator.ErrorMessage = "Select Employee from list"
            Else
                e.IsValid = True
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        addAttributes()
        RemoveSessions() 'ALP
        'If CType(Session("AddParts"), String) = "True" Then
        '    'Add selected part(s) to Enquiry Items
        '    AddMultipleParts()
        '    Session("AddParts") = "False"
        'Else
        '    Session("AddParts") = "False"
        'End If
        If Not IsPostBack And Session("sender") = "" Then
            If AppSettings("AutoCompleteTransText") = False Then
                If txtText.Enabled = True Then
                    txtText.Focus()
                End If
            End If
            'Added by Utkarsh on 22-Nov-2013 for Trans Text Series
            If CType(Session("AddTransTextSeries"), String) = "True" AndAlso (Not Session("TransText_ForTransSeries") Is Nothing) Then
                If mRequisitionNew.IsNew Then
                    mRequisitionNew.Text = Session("TransText_ForTransSeries")
                    txtText.Text = mRequisitionNew.Text
                    Session("mRequisitionNew") = mRequisitionNew
                    Session("AddTransTextSeries") = "False"
                    Session.Remove("TransName_ForTransSeries")
                    Session.Remove("TransText_ForTransSeries")
                    Session.Remove("TransNo_ForTransSeries")
                End If
            End If
            'End
            DataFieldBind(sender, e)
            SetPage()
            ControlVisibility()
        End If
    End Sub
    Private Sub dgRequisitionItems_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRequisitionItems.RowCommand
        Dim Index As Int32
        Select Case e.CommandName
            Case "EditRec"
                'If (Not User.IsInRole("NewRequisitionView") And Not User.IsInRole("NewRequisitionEdit")) Then
                '   ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                '    Exit Sub
                'End If
                ''Index = CInt(e.CommandArgument)
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow) 'Ajay 12-Jan-2023
                Index = gvr.RowIndex
                Session("Edit") = True
                setObject()
                setComboDetails()
                mRequisitionNew.RequisitionItemsNew.CurrentIndex = Index
                Session("mRequisitionNew") = mRequisitionNew
                Response.Redirect("wfRequisitionItem_Ajax.aspx?BackPage=wfRequisition_Ajax.aspx")
            Case "DeleteRec"
                'If (Not User.IsInRole("NewRequisitionNew") And mRequisitionNew.IsNew) Or (Not User.IsInRole("NewRequisitionEdit") And Not mRequisitionNew.IsNew) Then
                '   ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                '    Exit Sub
                'End If
                ''  Index = CInt(e.CommandArgument)
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow) 'Ajay 12-Jan-2023
                Index = gvr.RowIndex
                DeleteRecord(Index)
            Case "ShowPartStatus"
                Index = CInt(e.CommandArgument)
                Dim PartNoStatus As String = dgRequisitionItems.Rows(CInt(e.CommandArgument)).Cells(2).Text
                Dim DescriptionStatus As String = dgRequisitionItems.Rows(CInt(e.CommandArgument)).Cells(3).Text
                Dim mFetchItemByName As FetchItemByName = FetchItemByName.GetItemByName(PartNoStatus)
                Dim ItemIDStatus As Guid
                If mFetchItemByName.Count > 0 Then
                    ItemIDStatus = mFetchItemByName(0).ID
                Else
                    ItemIDStatus = Guid.Empty
                End If

                If Not ItemIDStatus.Equals(Guid.Empty) Then
                    Dim mItemStatus As Item = Item.GetItem(ItemIDStatus)
                    Dim LinkID As Guid = mItemStatus.LinkID
                    Dim Unit As String = mItemStatus.UnitName


                    Dim mStockPartStatus As rptStockPartStatus = rptStockPartStatus.GetStockPartStatusList(LinkID)
                    Dim mOnOrderPartStatus As rptOnOrderPartStatus = rptOnOrderPartStatus.GetrptOnOrderPartStatusList(LinkID)
                    Dim mReturnablePartStatus As rptReturnablePartStatus = rptReturnablePartStatus.GetrptReturnnablePartStatusList(LinkID)
                    Dim mTransitPartList As rptTransitPartList = rptTransitPartList.GetTransitPartList(LinkID, Today.Date.ToShortDateString)
                    Dim mRequisitionItemsNew As RequisitionItemsNew = RequisitionItemsNew.GetRequisitionItemsForPartNoStatus(LinkID, AppSettings("ClientCode"))

                    Session("PartNoStatus") = PartNoStatus
                    Session("DescriptionStatus") = DescriptionStatus
                    Session("Unit") = Unit

                    Session("mStockPartStatus") = mStockPartStatus
                    Session("mOnOrderPartStatus") = mOnOrderPartStatus
                    Session("mReturnablePartStatus") = mReturnablePartStatus
                    Session("mTransitPartList") = mTransitPartList
                    Session("mRequisitionItemsNewForPartNoStatus") = mRequisitionItemsNew
                    Session("LinkID") = LinkID
                    Response.Redirect("wfrptShowPartNoStatus_Ajax.aspx?BackPage=wfRequisition_Ajax.aspx")
                Else
                    'Alert Messege-Part Needs To Be Added In Part Master
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Part Needs To Be Added In Part Master.", False), True)
                End If
        End Select
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not IsInRole(Rights.New)) And (Not IsInRole(Rights.Edit)) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If IsValid Then
            Save()
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnCombo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCombo.Click
        If IsValid Then
            If cmbAdd.SelectedIndex = 0 Then
                RemoveSessions() 'ALP
                Session("Add") = True
                setObject()
                setComboDetails()
                mRequisitionNew.RequisitionItemsNew.Add(mRequisitionNew.ID, mRequisitionNew.WorkShopID)
                If Not Session("ReqURLFromWO") Is Nothing Then
                    Dim mnWO As nWO
                    mnWO = Session("mnWO")
                    mRequisitionNew.RequisitionItemsNew.CurrentItem.MachineID = mnWO.MachineID
                    mRequisitionNew.RequisitionItemsNew.CurrentItem.RegNo = mnWO.RegNo
                    mRequisitionNew.RequisitionItemsNew.CurrentItem.WOID = mnWO.ID
                    mRequisitionNew.RequisitionItemsNew.CurrentItem.WONo = mnWO.WONumber
                End If
                Session("mRequisitionNew") = mRequisitionNew
                Response.Redirect("wfRequisitionItem_Ajax.aspx?BackPage=wfRequisition_Ajax.aspx")
            ElseIf cmbAdd.SelectedIndex = 1 Then 'Added By Vikrant On 30-Aug-2016 For ALL30082016
                setComboDetails()
                setObject()
                Session("mRequisitionNew") = mRequisitionNew
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToolsWindow", "OpenToolsWindow();", True)
                'End
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        RemoveSessions()
        'Page.Validate("1")
        Session("IsValid") = IsValid
        setObject()
        setComboDetails()
        If mRequisitionNew.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
            If IsValid Then
                setObject()
                setComboDetails()
            End If
        Else
            MarkLog(Util.Action.Close, mTransactionList(CType(mTransTypeID, Util.Trans)).Name, "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
            Dim URL As Stack = CType(Session("ReqURLFromWO"), Stack)
            If Not URL Is Nothing Then
                If URL.Count > 0 Then
                    Session("MiddleFrame") = Session("MiddleFrameForWO") '12-Jun-2019
                    Response.Redirect(URL.Peek.ToString)
                    Exit Sub
                End If
            End If
            Response.Redirect("Index.aspx")
        End If
    End Sub
    'Added By Vikrant On 19-Nov-2013 For All18102013-1
    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not IsInRole(Rights.Print) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Print(False)
    End Sub
    'End
    Private Sub btnAuthorized_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuthorized.Click
        If IsValid Then
            Session("IsValid") = IsValid
            MSGBoxCtrl.show(MSGBox.Message_title.StatusSubmitted, MSGBox.Message_text.StatusSubmitted, "<Strong> Requisition </Strong>", MsgBoxStyle.YesNo, "Status")
            mRequisitionNew.StatusID = 2
            Session("mRequisitionNew") = mRequisitionNew
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub txtRequisitionDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRequisitionDate.TextChanged
        mRequisitionNew = Session("mRequisitionNew")
        mRequisitionNew.ReqDate = txtRequisitionDate.Text
        txtText.Text = mRequisitionNew.Text
        txtText.DataBind()
        Session("mRequisitionNew") = mRequisitionNew
    End Sub
    Private Sub rdoPartPurchase_CheckedChanged(sender As Object, e As System.EventArgs) Handles rdoPartPurchase.CheckedChanged
        txtRecommendedBy.Visible = True
        lblRecommendedBy.Visible = True
        'Added By Vikrant On 30-Aug-2016 For ALL30082016
        If mRequisitionNew.TransTypeID = Util.Trans.StoresRequisition Then
            mRequisitionNew.ReqTypeID = 0
            'ElseIf mRequisitionNew.TransTypeID = Util.Trans.PlanningRequisition Then 'Commented by Prashant 20-Oct-2020 STR20102020.Add Requisition Type as “Part Purchase or Part Request” in Planning Requisition module
            '    mRequisitionNew.ReqTypeID = 1
        Else
            mRequisitionNew.ReqTypeID = IIf(rdoPartRequest.Checked, 1, 2)
        End If
        txtText.DataBind()
        'End
        'upnlReqDetails.Update()
    End Sub
    Private Sub rdoPartRequest_CheckedChanged(sender As Object, e As System.EventArgs) Handles rdoPartRequest.CheckedChanged
        txtRecommendedBy.Visible = False
        lblRecommendedBy.Visible = False
        'Added By Vikrant On 30-Aug-2016 For ALL30082016
        If mRequisitionNew.TransTypeID = Util.Trans.StoresRequisition Then
            mRequisitionNew.ReqTypeID = 0
            'ElseIf mRequisitionNew.TransTypeID = Util.Trans.PlanningRequisition Then 'Commented by Prashant 20-Oct-2020 STR20102020.Add Requisition Type as “Part Purchase or Part Request” in Planning Requisition module
            '    mRequisitionNew.ReqTypeID = 1
        Else
            mRequisitionNew.ReqTypeID = IIf(rdoPartRequest.Checked, 1, 2)
        End If
        txtText.DataBind()
        'End
        'upnlReqDetails.Update()
    End Sub
    Private Sub cmbWorkShop_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbWorkShop.SelectedIndexChanged
        mRequisitionNew.WorkShopID = New Guid(cmbWorkShop.SelectedValue)
        txtText.DataBind()
    End Sub
    Private Sub cmbRequisitionEngineeringBranches_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbRequisitionEngineeringBranches.SelectedIndexChanged
        If mRequisitionNew.TransTypeID = Util.Trans.EngineeringRequisition Or mRequisitionNew.TransTypeID = Util.Trans.WorkShopRequisition Then
            mRequisitionNew.RequisitionEngineeringBrancheID = cmbRequisitionEngineeringBranches.SelectedValue
        ElseIf mRequisitionNew.TransTypeID = Util.Trans.PlanningRequisition Then
            mRequisitionNew.RequisitionEngineeringBrancheID = 4
        Else
            mRequisitionNew.RequisitionEngineeringBrancheID = 0
        End If
        txtText.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    'Added By Vikrant On 28-Sep-2015 For All28092015
    Private Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
        If IsValid Then
            Dim IsReqUsed As IsInUse = (IsInUse.GetIsInUseForRequisitionInEnqQuoOrderIssue(mRequisitionNew.ID))

            If IsReqUsed.IsInUse Then
                MSGBoxCtrl.show(MSGBox.Message_title.Cancel, MSGBox.Message_text.Cancel, "<Strong>Requisition,It is used in Enquiry/Quotation/Order/Issue</Strong>", MsgBoxStyle.OkOnly, "Status")
                'mRequisitionNew.StatusID = 4
                'Session("mRequisitionNew") = mRequisitionNew
                Exit Sub
            End If
            Session("IsValid") = IsValid
            MSGBoxCtrl.show(MSGBox.Message_title.StatusCanceled, MSGBox.Message_text.StatusCanceled, "<strong>Requisition</strong>", MsgBoxStyle.YesNo, "Status")
            mRequisitionNew.StatusID = 4
            Session("mRequisitionNew") = mRequisitionNew
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    'End
    'Added By Vikrant On 30-Aug-2016 For ALL30082016
    Private Sub hdnimgBtnCommonPartList_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnCommonPartList.Click
        If CType(Session("AddReOrderParts"), String) = "True" Then
            AddReOrderParts()
            Session("AddReOrderParts") = "False"
            dgRequisitionItems.DataSource = mRequisitionNew.RequisitionItemsNew
            dgRequisitionItems.DataBind()
            upnlGridView.Update()
        Else
            Session("AddReOrderParts") = "False"
        End If
    End Sub
    Protected Sub txtEmployee_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'SetEmpID()
        Dim splitName As String
        If txtEmployee.Text.Contains("-") Then
            splitName = txtEmployee.Text.Split("-")(txtEmployee.Text.Split("-").Length - 1).Trim ' txtEmployee.Text.Split("-")(1).Trim
        Else
            splitName = txtEmployee.Text
        End If
        Dim Message As String = ""

        mEmployeeList = EmployeeList.GetEmployeeList(Name:=splitName)

        ' Dim mEmployeeListAutoComplete As EmpNoNameAutoComplete = EmpNoNameAutoComplete.GeEmpNoNameList(splitName)
        If mEmployeeList.Contains(txtEmployee.Text) Then
            'mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(hdnEmpId.Value.ToString, mRequisitionNew.ReqDateFormatted)
            mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mEmployeeList(txtEmployee.Text, "").ID.ToString, mRequisitionNew.ReqDateFormatted)
            If mEmployeeStatus.Count > 0 Then
                If (mEmployeeStatus(0).Information <> "") Then
                    Message = mEmployeeStatus(0).Information
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, Message, MsgBoxStyle.OkOnly, "ResetEmployee")
                    Exit Sub
                End If
                mRequisitionNew.EmployeeID = New Guid(mEmployeeList(txtEmployee.Text, "").ID.ToString)
                mRequisitionNew.EmployeeName = txtEmployee.Text
                mRequisitionNew.NameOfEmployee = mEmployeeList(txtEmployee.Text, "").Name
                mRequisitionNew.EmpNo = mEmployeeList(txtEmployee.Text, "").EmpNo
            Else
                txtEmployee.Text = ""
                If Not mRequisitionNew.EmployeeID.Equals(Guid.Empty) Then
                    'hdnEmpId.Value = mRequisitionNew.EmployeeID.ToString
                    'SetEmpID()
                    txtEmployee.Text = mRequisitionNew.EmployeeName
                End If
            End If
        Else
            txtEmployee.Text = ""
            mRequisitionNew.EmployeeID = Guid.Empty
            mRequisitionNew.EmployeeName = ""
            mRequisitionNew.NameOfEmployee = ""
            mRequisitionNew.EmpNo = ""
        End If
    End Sub
    Private Sub btnSelectWONo_Click(sender As Object, e As System.EventArgs) Handles btnSelectWONo.Click
        If IsValid Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenWOList", "OpenWOList();", True)
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub hdnimgBtnWOList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnWOList.Click
        If Not Session("ID") Is Nothing Then
            'mRequisitionNew.RequisitionItemsNew.CurrentItem.WOID = New Guid(Session("ID").ToString)
            'mRequisitionNew.RequisitionItemsNew.CurrentItem.WONo = Session("No")
            'mRequisitionNew.RequisitionItemsNew.CurrentItem.MachineID = Session("WOMachineID")
            'mRequisitionNew.RequisitionItemsNew.CurrentItem.RegNo = mMachineNameValueList(CType(Session("WOMachineID"), Guid)).RegNo
            'Session.Remove("ID")
            'Session.Remove("No")
            'Session.Remove("WOMachineID")
            dgRequisitionItems.DataSource = mRequisitionNew.RequisitionItemsNew
            dgRequisitionItems.DataBind()
            ControlVisibility()
            upnlGridView.Update()
            upnlReqItemAdd.Update()
        End If
    End Sub
#End Region

#Region " Add Multiple Parts "
    'Private Sub AddMultipleParts()
    '    Dim mRequisitionItemNew As RequisitionItemNew
    '    Dim mRequisitionItemListNew As RequisitionItemListNew = Session("mRequisitionItemListNew")
    '    For Each mRequisitionItemNew In mRequisitionItemListNew
    '        If mRequisitionItemNew.IsSelect Then
    '            If Not mRequisitionNew.RequisitionItemsNew.Contains(mRequisitionItemNew.ItemID) Then
    '                mRequisitionNew.RequisitionItemsNew.Add(mRequisitionNew.ID, mRequisitionNew.WorkShopID)
    '                With mRequisitionNew.RequisitionItemsNew.CurrentItem
    '                    .ItemID = mRequisitionItemNew.ItemID
    '                    .PartNo = mRequisitionItemNew.PartNo
    '                    .Description = mRequisitionItemNew.Description
    '                End With
    '            Else
    '                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Requistion. <Br>That Part already taken for Requisition.", MsgBoxStyle.OkOnly, "")
    '            End If
    '        End If
    '    Next
    '    'If requested for the part which is not available in the List
    '    If Not mRequisitionNew.RequisitionItemsNew.Contains(Session("ItemName")) Then
    '        If Session("ItemName") <> "" Then
    '            mRequisitionNew.RequisitionItemsNew.Add(mRequisitionNew.ID, mRequisitionNew.WorkShopID)
    '            With mRequisitionNew.RequisitionItemsNew.CurrentItem
    '                .PartNo = Session("ItemName")
    '                .Description = Session("Description")
    '                .RequestedQty = 0
    '            End With
    '            ''Else
    '            ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "Requisition. <Br>That Part already taken for Requisition.", MsgBoxStyle.OKOnly)
    '            ''msg1.ReplacePage = "wfRequisitionEngineerDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
    '            ''msg1.Show()
    '            ''MessageBox.Show("Part : '" + mRequisitionItem.ItemName.ToString + "' already taken for Requisition.", "Requisition", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        End If
    '        Session("AddParts") = "False"
    '        Session.Remove("mRequisitionItemNew")
    '    End If

    'End Sub
    Private Sub AddReOrderParts()
        Dim mRequisitionItemNew As RequisitionItemNew
        Dim mRequisitionItemListNew As RequisitionItemListNew = Session("mRequisitionItemListNew")
        For Each mRequisitionItemNew In mRequisitionItemListNew
            If mRequisitionItemNew.IsSelect Then
                If Not mRequisitionNew.RequisitionItemsNew.Contains(mRequisitionItemNew.ItemID) Then
                    mRequisitionNew.RequisitionItemsNew.Add(mRequisitionNew.ID, Guid.Empty)
                    With mRequisitionNew.RequisitionItemsNew.CurrentItem
                        .ItemID = mRequisitionItemNew.ItemID
                        .PartNo = mRequisitionItemNew.PartNo
                        .Description = mRequisitionItemNew.Description
                        .Unit = mRequisitionItemNew.Unit
                        .UnitID = mRequisitionItemNew.UnitID
                        If AppSettings("ClientCode") = "BA" Then
                            .RequestedQty = IIf(mRequisitionItemNew.MinReOrderLevel - (mRequisitionItemNew.OnRequisitionQty + mRequisitionItemNew.OnOrderQty) > 0, mRequisitionItemNew.MinReOrderLevel - (mRequisitionItemNew.OnRequisitionQty + mRequisitionItemNew.OnOrderQty), 0)
                        Else
                            .RequestedQty = IIf(mRequisitionItemNew.MinReOrderLevel > 0, mRequisitionItemNew.MinReOrderLevel, 0)
                        End If


                        .IsOneTimePurchase = mRequisitionItemNew.IsOneTimePurchase
                        If Not mRequisitionItemNew.IsOneTimePurchase Then
                            .MinStockLevel = mRequisitionItemNew.MinStockLevel
                            .MaxStockLevel = mRequisitionItemNew.MaxStockLevel
                            .MinReOrderLevel = mRequisitionItemNew.MinReOrderLevel
                        Else
                            .MinStockLevel = 0
                            .MaxStockLevel = 0
                            .MinReOrderLevel = 0
                        End If

                    End With
                    'Else
                    'MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Requistion. <Br>That Part already taken for Requisition.", MsgBoxStyle.OkOnly, "")
                End If
            End If
        Next
    End Sub
#End Region

#Region " Show BrokenRules "
    Public Sub CustomValidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim CustValidator As CustomValidator
        CustValidator = CType(s, CustomValidator)
        Dim strMsg As String = ""
        setObject()
        setComboDetails()
        If Not mRequisitionNew.IsValid Then
            For i As Integer = 0 To mRequisitionNew.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mRequisitionNew.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        Dim mRequisitionItemNew As RequisitionItemNew
        If Not mRequisitionNew.RequisitionItemsNew.IsValid Then
            For Each mRequisitionItemNew In mRequisitionNew.RequisitionItemsNew
                For i As Integer = 0 To mRequisitionItemNew.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mRequisitionItemNew.PartNo + " : " + mRequisitionItemNew.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
            Next
        End If
        If strMsg.Trim <> "" Then
            CustValidator.ErrorMessage = strMsg
            e.IsValid = False
        End If
        Flag = 1
    End Sub

#End Region

#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetEmployeeList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim itemlist As EmpNoNameAutoComplete
        itemlist = EmpNoNameAutoComplete.GeEmpNoNameList(prefixText)
        If count = 0 Then
            Return (From c As EmpNoNameAutoComplete.EmpListAutoCompleteInfo In itemlist
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.EmpNoName, c.ID.ToString())).ToArray
        Else
            Return (From c As EmpNoNameAutoComplete.EmpListAutoCompleteInfo In itemlist
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.EmpNoName, c.ID.ToString())).Take(count).ToArray
        End If
    End Function
#End Region


End Class