'Added  By Vikrant On 25-Aug-2014
Imports System.Text
Public Class wfSendMail_Ajax
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
    Dim mTransTypeID As Integer 'All13082014
    Dim mRequisitionCustomer As RequisitionCustomer
    Dim CustName As String = String.Empty
    Dim CustAddress As String = String.Empty
    Dim AircraftType As String = String.Empty
    Dim RequiredByDate As String
    Dim mTransactionList As TransactionList  'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mRequisitionNew = Session("mRequisitionNew")
        mTransTypeID = Session("TransTypeID")
        mTransactionList = Session("mTransactionList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    End Sub
    Private Sub setSession()
        Session("mRequisitionNew") = mRequisitionNew
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
#End Region

#Region "Events"
    Private Sub wfSendMail_Ajax_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        getSession()
        If Not IsPostBack Then
            txtMailIDs.Focus()
            txtCCIDs.Text = Employee.GetEmployee(mRequisitionNew.EmployeeID).Email
            'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
            If mRequisitionNew.TransTypeID = 65 Then
                lblToMailID.Text = mTransactionList.Item(Trans.EngineeringRequisition).SendToMailID
                Session("ToSendMailIDs") = mTransactionList.Item(Trans.EngineeringRequisition).SendToMailID
                Session("CcSendMailIDs") = mTransactionList.Item(Trans.EngineeringRequisition).SendCCMailID
                txtMailIDs.Text = Session("ToSendMailIDs")
                txtCCIDs.Text = Session("CcSendMailIDs")
                Session("SmtpHost") = mTransactionList.Item(Trans.EngineeringRequisition).SmtpHost
                Session("SmtpPort") = mTransactionList.Item(Trans.EngineeringRequisition).SmtpPort
                Session("SmtpUser") = mTransactionList.Item(Trans.EngineeringRequisition).SmtpUser
                Session("SmtpPassword") = mTransactionList.Item(Trans.EngineeringRequisition).SmtpPassword
            ElseIf mRequisitionNew.TransTypeID = 71 Then
                lblToMailID.Text = mTransactionList.Item(Trans.StoresRequisition).SendToMailID
                Session("ToSendMailIDs") = mTransactionList.Item(Trans.StoresRequisition).SendToMailID
                Session("CcSendMailIDs") = mTransactionList.Item(Trans.StoresRequisition).SendCCMailID
                txtMailIDs.Text = Session("ToSendMailIDs")
                txtCCIDs.Text = Session("CcSendMailIDs")
                Session("SmtpHost") = mTransactionList.Item(Trans.StoresRequisition).SmtpHost
                Session("SmtpPort") = mTransactionList.Item(Trans.StoresRequisition).SmtpPort
                Session("SmtpUser") = mTransactionList.Item(Trans.StoresRequisition).SmtpUser
                Session("SmtpPassword") = mTransactionList.Item(Trans.StoresRequisition).SmtpPassword
            ElseIf mRequisitionNew.TransTypeID = 72 Then
                lblToMailID.Text = mTransactionList.Item(Trans.WorkShopRequisition).SendToMailID
                Session("ToSendMailIDs") = mTransactionList.Item(Trans.WorkShopRequisition).SendToMailID
                Session("CcSendMailIDs") = mTransactionList.Item(Trans.WorkShopRequisition).SendCCMailID
                txtMailIDs.Text = Session("ToSendMailIDs")
                txtCCIDs.Text = Session("CcSendMailIDs")
                Session("SmtpHost") = mTransactionList.Item(Trans.WorkShopRequisition).SmtpHost
                Session("SmtpPort") = mTransactionList.Item(Trans.WorkShopRequisition).SmtpPort
                Session("SmtpUser") = mTransactionList.Item(Trans.WorkShopRequisition).SmtpUser
                Session("SmtpPassword") = mTransactionList.Item(Trans.WorkShopRequisition).SmtpPassword
            ElseIf mRequisitionNew.TransTypeID = 77 Then
                lblToMailID.Text = mTransactionList.Item(Trans.PlanningRequisition).SendToMailID
                Session("ToSendMailIDs") = mTransactionList.Item(Trans.PlanningRequisition).SendToMailID
                Session("CcSendMailIDs") = mTransactionList.Item(Trans.PlanningRequisition).SendCCMailID
                txtMailIDs.Text = Session("ToSendMailIDs")
                txtCCIDs.Text = Session("CcSendMailIDs")
                Session("SmtpHost") = mTransactionList.Item(Trans.PlanningRequisition).SmtpHost
                Session("SmtpPort") = mTransactionList.Item(Trans.PlanningRequisition).SmtpPort
                Session("SmtpUser") = mTransactionList.Item(Trans.PlanningRequisition).SmtpUser
                Session("SmtpPassword") = mTransactionList.Item(Trans.PlanningRequisition).SmtpPassword
            End If
            upnlSendMailDetails.Update()
            '----------------------
        End If
    End Sub
    Private Sub btnSendMail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSendMail.Click
        If Not IsInRole(Rights.Authorized) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim da As New CSLA.Data.ObjectAdapter
        Dim rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As DataSet
        Dim AircraftList As New StringBuilder
        Dim WorkOrderNumberList As New StringBuilder
        Dim AircraftTypeList As New StringBuilder
        Dim mCompanyDetail As New CompanyDetail
        mRequisitionNew = RequisitionNew.GetRequisition(mRequisitionNew.ID) ' Need to fetch again to get saved user name. As uer name changed on this page again on authrize if other user authorized it.
        If AppSettings("ClientCode") = "Novo" Then 'Added By Prashant On 07-Dec-2018 For NovoAir07122018
            rpt = New crptRequisitionDetailNovoAir
            ds = New dsRequisitionNew
            da.Fill(ds, mRequisitionNew)
            da.Fill(ds, mRequisitionNew.RequisitionItemsNew)
        Else
            If mRequisitionNew.TransTypeID = Util.Trans.EngineeringRequisition Or mRequisitionNew.TransTypeID = Util.Trans.WorkShopRequisition Then
                If mRequisitionNew.ReqTypeID = 1 Then 'Part Request Or mRequisitionNew.TransTypeID = 72 Then 

                    ds = New dsIssueAgainstRequisitionItem

                    If AppSettings("ClientCode") = "STR" Then
                        rpt = New crptIssueAgainstRequisitionItemStarAir
                    Else
                        rpt = New crptIssueAgainstRequisitionItem
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
                ElseIf mRequisitionNew.ReqTypeID = 2 Then 'Part Purchase
                    If (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
                        rpt = New crptPurchaseOrderAgainstRequisitionItemDeccan
                    Else
                        rpt = New crptPurchaseOrderAgainstRequisitionItem
                    End If
                    ds = New dsPurchaseOrderAgainstRequisitionItem
                    Dim mPurchaseOrderAgainstRequisitionItem As PurchaseOrderAgainstRequisitionItem = PurchaseOrderAgainstRequisitionItem.GetPurchaseOrderAgainstRequisitionItem(mRequisitionNew.ID)

                    da.Fill(ds, mPurchaseOrderAgainstRequisitionItem)
                End If
            ElseIf mRequisitionNew.TransTypeID = 71 Then 'Stores Requisition
                ds = New dsIssueAgainstRequisitionItem
                rpt = New crptMaterialReplanishmentNote
                Dim mMaterialReplanishmentNote As MaterialReplanishmentNote = MaterialReplanishmentNote.GetMaterialReplanishmentNote(mRequisitionNew.ID.ToString)
                da.Fill(ds, mMaterialReplanishmentNote)
            ElseIf mRequisitionNew.TransTypeID = Util.Trans.PlanningRequisition Then
                Dim mTemp As New Hashtable
                Dim mTempWoNoList As New Hashtable
                Dim mTempAircraftTypeList As New Hashtable
                For i As Integer = 0 To mRequisitionNew.RequisitionItemsNew.Count - 1
                    If Not mTemp.ContainsValue(mRequisitionNew.RequisitionItemsNew(i).RegNo) And mRequisitionNew.RequisitionItemsNew(i).RegNo <> "" Then
                        mTemp.Add(i, mRequisitionNew.RequisitionItemsNew(i).RegNo)
                        AircraftList.Append(mTemp(i) + ",")
                    End If

                    If Not mTempWoNoList.ContainsValue(mRequisitionNew.RequisitionItemsNew(i).WONoNRCNo) And mRequisitionNew.RequisitionItemsNew(i).WONoNRCNo <> "" Then
                        mTempWoNoList.Add(i, mRequisitionNew.RequisitionItemsNew(i).WONoNRCNo)
                        WorkOrderNumberList.Append(mTempWoNoList(i) + ",")
                    End If

                    If AppSettings("ClientCode") = "STR" Then
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
                    rpt = New crptPlanningRequisitionDetailBRD
                ElseIf AppSettings("ClientCode") = "STR" Then
                    rpt = New crptRequisitionDetailNewForStarAir
                ElseIf AppSettings("ClientCode") = "Heligo" Then
                    rpt = New crptPlanningRequisitionDetailHeligo
                Else
                    rpt = New crptPlanningRequisitionDetail
                End If
                ds = New dsRequisitionNew
                da.Fill(ds, mRequisitionNew)
                da.Fill(ds, mRequisitionNew.RequisitionItemsNew)
            End If
        End If

        Dim mrptLetterHead As rptLetterHead = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", AppSettings("Logo"))

        Dim mReport As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
                                    mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, "", _
                                    mRequisitionNew.RequisitionNo, mRequisitionNew.UserName, mRequisitionNew.EmployeeName, mRequisitionNew.RecommendedBy, _
                                    mRequisitionNew.Supervisor, AppSettings("Product Version"), AppSettings("SINote"), mRequisitionNew.TransTypeID.ToString, _
                                    , SearchStr8:=mRequisitionNew.AuthorizedBy, SearchStr9:=IIf(mRequisitionNew.TransTypeID = 65, AircraftList.ToString + "/" + mRequisitionNew.RequisitionEngineeringBrancheName, mRequisitionNew.RequisitionEngineeringBrancheName), _
                                    SearchStr10:=AppSettings("Logo"), _
                                    SearchStr11:=AppSettings("ClientCode"), SearchStr12:=CustName, SearchStr13:=CustAddress, SearchStr14:=AircraftType, _
                                    SearchStr15:=WorkOrderNumberList.ToString, SearchStr16:=AircraftList.ToString, searchstr17:=RequiredByDate)

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, mReport)
        da.Fill(ds, mrptLetterHead)
        rpt.SetDataSource(ds)
        Session("CrystalReport") = rpt

        Dim str As String
        str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Following Parts(s) has been requested by <b>" + User.Identity.Name + "</b>" + " in Requisition " + mRequisitionNew.RequisitionNo + " ,Created on " + New SmartDate(mRequisitionNew.ReqDateFormatted.ToString).FormattedText + " in FlyPal System." + "</font></P></br> ")
        str = str + ("<TABLE BORDER=1 Style=""border-collapse: collapse"" BORDER-COLOR=""black"" ID=""Table2"">")
        'str = str + ("<tr>" & "<td align=""center"" style=""background-color: #829e82; color: black;"">" & "<font face=""Calibri""><b>Sr. No.</b>" & "</font>" & "</td><td align=""center"" width=""200"" style=""background-color: #829e82; color: black;"" >" & "<font face=""Calibri""><b>Part No</b>" & "</font>" & "</td><td align=""center"" width=""200"" style=""background-color: #829e82; color: black;"" >" & "<font face=""Calibri""><b>Description</b>" & "</font>" & "</td><td align=""center"" style=""background-color: #829e82; color: black;"">" & "<font face=""Calibri""><b>Qty</b>" & "</font>" & "</td>  <td align=""center"" style=""background-color: #829e82; color: black;"">" & "<font face=""Calibri""><b>Reg</b>" & "</font>" & "</td>  <td align=""center"" style=""background-color: #829e82; color: black;"">" & "<font face=""Calibri""><b>WO.No.</b>" & "</font>" & "</td></tr>")
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

        Dim ToMailID As String = ""
        If lblToMailID.Text.Trim <> "" And txtMailIDs.Text.Trim <> "" Then
            ToMailID = lblToMailID.Text.Trim + "," + Trim(txtMailIDs.Text)
        ElseIf lblToMailID.Text.Trim <> "" Then
            ToMailID = lblToMailID.Text.Trim
        ElseIf txtMailIDs.Text.Trim <> "" Then
            ToMailID = txtMailIDs.Text.Trim
            End If

            Dim ReqType As String = ""
            If mRequisitionNew.TransTypeID = 71 Then
                ReqType = "Part Purchase"
            Else
                ReqType = mRequisitionNew.ReqTypeName.ToString
            End If
            Try
                SendMailFile.SendMailFile(Session("CrystalReport"), User.Identity.Name, "Requisition Details - " + ReqType.ToString, mRequisitionNew.RequisitionNo, Info:=str, VendorEmailID:="", ToMailID:=ToMailID, CCMailID:=Trim(txtCCIDs.Text), BCCMailID:=Trim(txtBCCIDs.Text), Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                              SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTransDetail", MessageBox.Show("Mail Sent Successfully", False), True)
            Catch ex As Exception
                MSGBoxCtrl.show("Error", "Error Sending Mail", ex.InnerException.ToString + ex.Message.ToString, MsgBoxStyle.OkOnly, "")
            End Try
    End Sub
    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Added by vikrant for popup
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End
    End Sub
#End Region

End Class