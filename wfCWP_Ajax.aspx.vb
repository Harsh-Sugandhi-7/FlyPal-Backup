Imports System.Linq
Imports System.Collections.Generic
Public Class wfCWP_Ajax
    Inherits System.Web.UI.Page

#Region " Enumeration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
    End Enum
#End Region

#Region " Variable Declaration "
    Protected mCWP As CWP
    Protected mWorkShopList As WorkShopList
    Protected mVendorList As VendorList
    Protected mEmployeeListForCombo As EmployeeListForCombo
    Dim mCWPDetailForEventLog As String = String.Empty
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim mPendingOrderItemListForCwp As PendingOrderItemListForCwp

  
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mCWP = Session("mCWP")
        mWorkShopList = Session("mWorkShopList")
        mVendorList = Session("mVendorList")
        mEmployeeListForCombo = Session("mEmployeeListForComboForCWP")
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCWP")
        Session.Remove("mWorkShopList")
        Session.Remove("mVendorList")
        Session.Remove("mEmployeeListForComboForCWP")
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
    End Sub
    Private Sub ControlVisibility()
        txtVisitNo.BackColor = IIf(mCWP.VisitNo = 0, Color.White, Color.Gainsboro)
        ControlVisibilityForAttachment()
    End Sub
    'Added By Vikrant On 01-Sep-2016 For ALL01092016
    Private Sub ControlVisibilityForGrid()
        If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
            dgCWPTaskSheet.Columns(2).Visible = True
            dgCWPTaskSheet.Columns(3).Visible = False
            dgCWPTaskSheet.Columns(4).Visible = False
            dgCWPTaskSheet.Columns(5).Visible = True
            dgCWPTaskSheet.Columns(6).Visible = False
            dgCWPTaskSheet.Columns(7).Visible = False
            dgCWPTaskSheet.Columns(8).Visible = False
            dgCWPTaskSheet.Columns(9).Visible = False


        Else
            dgCWPTaskSheet.Columns(2).Visible = False
            dgCWPTaskSheet.Columns(3).Visible = True
            dgCWPTaskSheet.Columns(4).Visible = True
            dgCWPTaskSheet.Columns(5).Visible = False
            dgCWPTaskSheet.Columns(6).Visible = True
            dgCWPTaskSheet.Columns(7).Visible = True
            dgCWPTaskSheet.Columns(8).Visible = True
            dgCWPTaskSheet.Columns(9).Visible = True

        End If
    End Sub
    'End
    Private Sub ControlVisibilityForAttachment()
        'If mCWP.IsAttachmentAdded Then
        '    ImageButton1.Visible = True
        '    btnDelAttach.Enabled = True
        'Else
        '    ImageButton1.Visible = False
        '    btnDelAttach.Enabled = False
        'End If
    End Sub
    Private Sub DataFieldBind()
        mWorkShopList = WorkShopList.GetWorkShopList(0, , , True, "(SELECT)")
        cmbWorkShop.DataSource = mWorkShopList
        Session("mWorkShopList") = mWorkShopList

        mVendorList = VendorList.GetVendortList(0, , , , , , True, True)
        cmbVendorList.DataSource = mVendorList
        Session("mVendorList") = mVendorList

        mEmployeeListForCombo = EmployeeListForCombo.GetEmployeeListForCombo("(SELECT)")
        cmbCRSEmployeeList.DataSource = mEmployeeListForCombo
        Session("mEmployeeListForComboForCWP") = mEmployeeListForCombo

        'mCWP.CWPDate = Today.Date.ToString(AppSettings("DateFormat"))
        txtCWPDate.Text = mCWP.CWPDateFormatted.ToString

        If Not mCWP.CRSEmployeeID = Guid.Empty Then
            Dim mCRSLicenseNoList = LicenseNoListWithEmployee.GetLicenseNoList(mCWP.CRSEmpName, User.Identity.Name, True, "(SELECT)", False)
            cmbCRSLicenseNo.DataSource = mCRSLicenseNoList

        End If
        If mCWP.CompRemDate Is DBNull.Value Then
            txtRemDate.Text = ""
        Else
            txtRemDate.Text = mCWP.CompRemDateFormatted
        End If

        txtShopWODate.Text = mCWP.ShopWODate

        If mCWP.CWPStartDate Is DBNull.Value Then
            txtCWPStartDate.Text = ""
        Else
            txtCWPStartDate.Text = mCWP.CWPStartDateFormatted
        End If

        If mCWP.CWPEndDate Is DBNull.Value Then
            txtCWPEndDate.Text = ""
        Else
            txtCWPEndDate.Text = mCWP.CWPEndDateFormatted
        End If

        dgCWPInspection.DataSource = mCWP.CWPInspections
        dgCWPComponent.DataSource = mCWP.CWPComps

        dgCWPAttachment.DataSource = mCWP.FileAttachments
        dgStatusList.DataSource = mCWP.CWPStatusChilds

        dgCWPTaskSheet.DataSource = mCWP.CWPTaskSheets
        DataBind()
        'Added By Vikrant On 01-Sep-2016 For ALL01092016
        If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
            Dim txtTechLicenceNo As TextBox
            Dim txtEngLicenceNo As TextBox

            For i As Integer = 0 To dgCWPTaskSheet.Rows.Count - 1
                txtTechLicenceNo = CType(dgCWPTaskSheet.Rows(i).FindControl("txtTechLicenceNo"), TextBox)
                txtEngLicenceNo = CType(dgCWPTaskSheet.Rows(i).FindControl("txtEngLicenceNo"), TextBox)

                If mCWP.CWPTaskSheets(i).TechLicenseNo <> "" And Not mCWP.CWPTaskSheets(i).TechEmployeeID.Equals(Guid.Empty) Then
                    txtTechLicenceNo.Text = mCWP.CWPTaskSheets(i).TechLicenseNo + " [" + mCWP.CWPTaskSheets(i).TechEmpName + "]"
                    txtTechLicenceNo.DataBind()
                End If

                If mCWP.CWPTaskSheets(i).EngLicenseNo <> "" And Not mCWP.CWPTaskSheets(i).EngEmployeeID.Equals(Guid.Empty) Then
                    txtEngLicenceNo.Text = mCWP.CWPTaskSheets(i).EngLicenseNo + " [" + mCWP.CWPTaskSheets(i).EngEmpName + "]"
                    txtEngLicenceNo.DataBind()
                End If
            Next
        End If
        'End
        If mCWP.CRSLicenseNo <> "" Then cmbCRSLicenseNo.SelectedValue = mCWP.CRSLicenseNo
        ''If mCWP.BarcodeNo <> "" Then
        ''    imgBarcode.ImageUrl = "wfBarcode_Ajax.aspx?Barcode=" & mCWP.BarcodeNo
        ''End If

        'Added by Saylee on 18-Jan-2018 for BA15012018
        txtBillOfWorkLicenceNo.Text = IIf(mCWP.BillofWorkEmpName <> "", mCWP.BillofWorkLicenseNo + " [" + mCWP.BillofWorkEmpName + "]", "")
        txtRecommendationLicenceNo.Text = IIf(mCWP.RecommendationEmpName <> "", mCWP.RecommendationLicenseNo + " [" + mCWP.RecommendationEmpName + "]", "")
        txtTaskPerformedTechLicenceNo.Text = IIf(mCWP.TaskPerformedTechEmpName <> "", mCWP.TaskPerformedTechLicenseNo + " [" + mCWP.TaskPerformedTechEmpName + "]", "")
        txtTaskPerformedEngLicenceNo.Text = IIf(mCWP.TaskPerformedEngEmpName <> "", mCWP.TaskPerformedEngLicenseNo + " [" + mCWP.TaskPerformedEngEmpName + "]", "")
        txtFinalTestReportTechLicenceNo.Text = IIf(mCWP.FinalTestReportTechEmpName <> "", mCWP.FinalTestReportTechLicenseNo + " [" + mCWP.FinalTestReportTechEmpName + "]", "")
        txtFinalTestReportEngLicenceNo.Text = IIf(mCWP.FinalTestReportEngEmpName <> "", mCWP.FinalTestReportEngLicenseNo + " [" + mCWP.FinalTestReportEngEmpName + "]", "")

        '**********************************************


    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtVisitNo" Then
            If mCWP.CWPStatusChilds.Count > 1 Then
                For i As Integer = 1 To mCWP.CWPStatusChilds.Count - 1
                    If mCWP.CWPStatusChilds(i).StatusDate < mCWP.CWPStatusChilds(i - 1).StatusDate Then
                        custValidator.ErrorMessage = mCWP.CWPStatusChilds(i).StatusName + " date[" + mCWP.CWPStatusChilds(i).StatusDateFormatted.ToString + "] should be greater than or equal to " + mCWP.CWPStatusChilds(i - 1).StatusName + " date[" + mCWP.CWPStatusChilds(i - 1).StatusDateFormatted.ToString + "]"
                        e.IsValid = False
                        Exit Sub
                    End If
                Next
            End If
            e.IsValid = True
        End If
    End Sub
    Private Function Save(ByVal StatuID As Integer) As Boolean
        Try
            SetObject()
            If StatuID <> 5 Then
                If Not mCWP.CWPStatusChilds.Contains(StatuID) Or StatuID = 4 Then
                    mCWP.StatusID = StatuID
                    mCWP.CWPStatusChilds.Add(mCWP.ID)
                    mCWP.CWPStatusChilds.CurrentItem.StatusID = StatuID
                    mCWP.CWPStatusChilds.CurrentItem.StatusDate = Today.Date.ToString(AppSettings("DateFormat"))
                    mCWP.CWPStatusChilds.CurrentItem.UserID = SI.UTILITY.User.GetUser(User.Identity.Name).UserID
                    mCWP.CWPStatusChilds.CurrentItem.UserName = SI.UTILITY.User.GetUser(User.Identity.Name).Name
                End If
            End If

            If Not mCWP.IsValid Then
                Dim strMSG As String = ""
                'If Not mCWP.IsValid Then
                For i As Integer = 0 To mCWP.GetBrokenRulesCollection.Count - 1
                    strMSG = strMSG + mCWP.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
                'End If
                If strMSG.Trim <> "" Then
                    cvControlValidator.ErrorMessage = strMSG
                    cvControlValidator.IsValid = False
                End If
                mCWP.CWPStatusChilds.Remove(mCWP.CWPStatusChilds.CurrentItem)
                upnlValidationsummary.Update()
                Return False
            End If


            mCWP.Save()
            SaveAttachment()

            Session("mCWP") = mCWP

            mCWPDetailForEventLog = mCWP.CWPTextNo + " Dated : " + mCWP.CWPDateFormatted.ToString + " WorkShop : " + cmbWorkShop.SelectedItem.ToString + " Part Name : " + txtPartName.Text + " Part Description : " + txtPartDescription.Text + " Serial No. : " + txtCompSerialNo.Text + " Created By : " + mCWP.CreatedBy


            MarkLog(Util.Action.Save, "CWP", mCWPDetailForEventLog, Util.ErrorType.NoError, mCWP.ID, EventLogID)

            'If mnWO.StatusID = 2 Then
            '    MarkLog(Util.Action.Authorize, "CWP", mCWPDetailForEventLog, Util.ErrorType.NoError, mCWP.ID, EventLogID)
            'ElseIf mnWO.StatusID = 3 Then
            '    MarkLog(Util.Action.Amend, "CWP", mCWPDetailForEventLog, Util.ErrorType.NoError, mCWP.ID, EventLogID)
            'ElseIf mnWO.StatusID = 4 Then
            '    MarkLog(Util.Action.Cancel, "CWP", mCWPDetailForEventLog, Util.ErrorType.NoError, mCWP.ID, EventLogID)
            'Else
            '    MarkLog(Util.Action.Save, "CWP", mCWPDetailForEventLog, Util.ErrorType.NoError, mCWP.ID, EventLogID)
            'End If

            'mnWO.MarkClean()
            'Session("mnWO") = mnWO
            'Dim WOstr As String = ""
            'If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
            '    WOstr = "Engineering Order"
            'Else
            '    WOstr = "Work Order"
            'End If
            DataFieldBind()
            SetPage()
            Return True
            'SetGrid()
            'ControlVisibility()
            'UpdatePanlels()
        Catch ex As SqlException

            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            End If
        Catch ex As Exception
            Throw ex
        Finally
            'nWOClone = Nothing
        End Try
    End Function
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        IsInRoleString = "CWP"
        Select Case CheckFor
            Case Rights.View
                Return User.IsInRole(IsInRoleString + "View")
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case Rights.Save
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case Rights.Delete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case Rights.Print
                Return User.IsInRole(IsInRoleString + "Print")
        End Select
    End Function
    Private Sub SetTitle()
        If mCWP.IsNew Then
            lblTitle.Text = "CWP [ New ]"
        Else
            lblTitle.Text = "CWP [" + mCWP.CWPTextNo + "]"
        End If
    End Sub
    Private Sub DeleteRecordComp(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "DeleteComp")
        mCWP.CWPComps.CurrentIndex = Index - 1
        Session("mCWP") = mCWP
    End Sub
    Private Sub DeleteRecordInsp(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "DeleteInsp")
        mCWP.CWPInspections.CurrentIndex = Index - 1
        Session("mCWP") = mCWP
    End Sub
    Private Sub DeleteRecordTaskSheet(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "DeleteTaskSheet")
        mCWP.CWPTaskSheets.CurrentIndex = Index - 1
        Session("mCWP") = mCWP
    End Sub

    Private Sub addAttributes()
        txtTurnAroundTime.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtTurnAroundTime').value,event)")
        txtVisitNo.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtVisitNo').value,event)")
    End Sub
    Private Sub SetObject()
        If txtCWPDate.Text <> "" Then
            mCWP.CWPDate = txtCWPDate.Text
        Else
            mCWP.CWPDate = System.DBNull.Value
        End If

        mCWP.CWPText = Trim(txtText.Text)
        mCWP.CWPNo = Val(txtNo.Text)
        mCWP.WorkShopID = New Guid(cmbWorkShop.SelectedValue)
        mCWP.WorkShopName = cmbWorkShop.SelectedItem.ToString
        mCWP.CMMOHMUsed = Trim(txtCMMOHMUsed.Text)
        mCWP.RevStatus = Trim(txtRevStatus.Text)
        mCWP.ShopWONo = Trim(txtShopWONo.Text)
        mCWP.ShopWODate = Trim(txtShopWODate.Text)
        mCWP.PartNo = Trim(txtPartName.Text)
        mCWP.PartDescription = Trim(txtPartDescription.Text)
        mCWP.SerialNo = Trim(txtCompSerialNo.Text)
        mCWP.TSOCSOLSO = Trim(txtTSOCSOLSO.Text)
        mCWP.TSCCSCLSC = Trim(txtTSCCSCLSC.Text)

        mCWP.RegNo = Trim(txtRegNo.Text)
        mCWP.Position = Trim(txtPartPosition.Text)
        mCWP.NHASerialNo = Trim(txtAirframeSrNo.Text)
        mCWP.Station = Trim(txtStation.Text)
        If txtRemDate.Text <> "" Then
            mCWP.CompRemDate = txtRemDate.Text
        Else
            mCWP.CompRemDate = System.DBNull.Value
        End If
        mCWP.CustomerID = New Guid(cmbVendorList.SelectedValue)
        mCWP.CustomerWONo = Trim(txtCustWONo.Text)
        mCWP.TagNo = Trim(txtTagNo.Text)
        mCWP.RemovalReason = Trim(txtRemovalReason.Text)
        mCWP.TurnAroundTime = Val(txtTurnAroundTime.Text)
        mCWP.VisualInspectionDesc = Trim(txtVisualInspection.Text)
        mCWP.PerformInitialTestDesc = Trim(txtInitialTest.Text)
        mCWP.ShopFindings = Trim(txtShopFindings.Text)
        mCWP.Recommendation = Trim(txtRecommendation.Text)
        mCWP.TaskPerformed = Trim(txtTaskPerformed.Text)
        mCWP.FinalTestReport = Trim(txtFinalTestReport.Text)
        mCWP.IncomingModStatus = Trim(txtIncomingModStatus.Text)
        mCWP.OutgoingModStatus = Trim(txtOutgoingModStatus.Text)
        mCWP.LRUControlNo = Trim(txtLRUControlNo.Text)
        mCWP.RNNo = Trim(txtRNNo.Text)
        mCWP.Form1No = Trim(txtForm1No.Text)
        mCWP.CreatedBy = User.Identity.Name
        If txtCWPStartDate.Text <> "" Then
            mCWP.CWPStartDate = txtCWPStartDate.Text
        Else
            mCWP.CWPStartDate = System.DBNull.Value
        End If
        If txtCWPEndDate.Text <> "" Then
            mCWP.CWPEndDate = txtCWPEndDate.Text
        Else
            mCWP.CWPEndDate = System.DBNull.Value
        End If
        mCWP.CRSEmployeeID = New Guid(cmbCRSEmployeeList.SelectedValue)
        mCWP.CRSEmpName = IIf(cmbCRSEmployeeList.SelectedIndex > 0, mEmployeeListForCombo(New Guid(cmbCRSEmployeeList.SelectedValue.ToString)).Name, "")
        mCWP.VisitNo = Val(txtVisitNo.Text)

        'If Not mFileAttach Is Nothing Then
        '    If mFileAttach.Size > 0 Then
        '        mCWP.IsAttachmentAdded = True
        '    Else
        '        mCWP.IsAttachmentAdded = False
        '    End If
        'End If

        'Added By Vikrant On 01-Sep-2016 For ALL01092016
        If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
            Dim txtTechLicenceNo As New TextBox
            Dim txtEngLicenceNo As New TextBox
            Dim TechLicenseNo As String = String.Empty
            Dim TechEmpName As String = String.Empty
            Dim EngLicenseNo As String = String.Empty
            Dim EngEmpName As String = String.Empty
            For i As Integer = 0 To dgCWPTaskSheet.Rows.Count - 1

                TechLicenseNo = String.Empty
                TechEmpName = String.Empty
                EngLicenseNo = String.Empty
                EngEmpName = String.Empty

                txtTechLicenceNo = CType(dgCWPTaskSheet.Rows(i).FindControl("txtTechLicenceNo"), TextBox)
                txtEngLicenceNo = CType(dgCWPTaskSheet.Rows(i).FindControl("txtEngLicenceNo"), TextBox)
                If (txtTechLicenceNo.Text.Trim.IndexOf("[") > 0 And txtTechLicenceNo.Text.Trim.IndexOf("]") > 0) Then
                    TechLicenseNo = txtTechLicenceNo.Text.Substring(0, txtTechLicenceNo.Text.Trim.IndexOf("[")).Trim
                    TechEmpName = Mid(txtTechLicenceNo.Text.Trim, txtTechLicenceNo.Text.Trim.IndexOf("[") + 2, txtTechLicenceNo.Text.Trim.IndexOf("]") - txtTechLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
                Else
                    TechLicenseNo = Trim(txtTechLicenceNo.Text)
                End If

                mCWP.CWPTaskSheets(i).TechEmployeeID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(TechLicenseNo, TechEmpName).EmpID
                mCWP.CWPTaskSheets(i).TechEmpName = TechEmpName
                mCWP.CWPTaskSheets(i).TechLicenseNo = TechLicenseNo

                If (txtEngLicenceNo.Text.Trim.IndexOf("[") > 0 And txtEngLicenceNo.Text.Trim.IndexOf("]") > 0) Then
                    EngLicenseNo = txtEngLicenceNo.Text.Substring(0, txtEngLicenceNo.Text.Trim.IndexOf("[")).Trim
                    EngEmpName = Mid(txtEngLicenceNo.Text.Trim, txtEngLicenceNo.Text.Trim.IndexOf("[") + 2, txtEngLicenceNo.Text.Trim.IndexOf("]") - txtEngLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
                Else
                    EngLicenseNo = Trim(txtEngLicenceNo.Text)
                End If

                mCWP.CWPTaskSheets(i).EngEmployeeID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(EngLicenseNo, EngEmpName).EmpID
                mCWP.CWPTaskSheets(i).EngEmpName = EngEmpName
                mCWP.CWPTaskSheets(i).EngLicenseNo = EngLicenseNo
            Next

        End If
        'End

        'Added by Saylee on 18-Jan-2018 for BA15012018
        'BillOfWorkEmployee
        Dim BillOfWorkLicenseNo As String = String.Empty
        Dim BillOfWorkEmpName As String = String.Empty
        If (txtBillOfWorkLicenceNo.Text.Trim.IndexOf("[") > 0 And txtBillOfWorkLicenceNo.Text.Trim.IndexOf("]") > 0) Then
            BillOfWorkLicenseNo = txtBillOfWorkLicenceNo.Text.Substring(0, txtBillOfWorkLicenceNo.Text.Trim.IndexOf("[")).Trim
            BillOfWorkEmpName = Mid(txtBillOfWorkLicenceNo.Text.Trim, txtBillOfWorkLicenceNo.Text.Trim.IndexOf("[") + 2, txtBillOfWorkLicenceNo.Text.Trim.IndexOf("]") - txtBillOfWorkLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        Else
            BillOfWorkLicenseNo = Trim(txtBillOfWorkLicenceNo.Text)
        End If


        mCWP.BillofWorkEmployeeID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(BillOfWorkLicenseNo, BillOfWorkEmpName).EmpID
        mCWP.BillofWorkEmpName = BillOfWorkEmpName
        mCWP.BillofWorkLicenseNo = BillOfWorkLicenseNo


        'RecommendationEmployee
        Dim RecommendationLicenseNo As String = String.Empty
        Dim RecommendationEmpName As String = String.Empty
        If (txtRecommendationLicenceNo.Text.Trim.IndexOf("[") > 0 And txtRecommendationLicenceNo.Text.Trim.IndexOf("]") > 0) Then
            RecommendationLicenseNo = txtRecommendationLicenceNo.Text.Substring(0, txtRecommendationLicenceNo.Text.Trim.IndexOf("[")).Trim
            RecommendationEmpName = Mid(txtRecommendationLicenceNo.Text.Trim, txtRecommendationLicenceNo.Text.Trim.IndexOf("[") + 2, txtRecommendationLicenceNo.Text.Trim.IndexOf("]") - txtRecommendationLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        Else
            RecommendationLicenseNo = Trim(txtRecommendationLicenceNo.Text)
        End If


        mCWP.RecommendationEmployeeID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(RecommendationLicenseNo, RecommendationEmpName).EmpID
        mCWP.RecommendationEmpName = RecommendationEmpName
        mCWP.RecommendationLicenseNo = RecommendationLicenseNo


        'TaskPerformedTech Employee
        Dim TaskPerformedTechLicenseNo As String = String.Empty
        Dim TaskPerformedTechEmpName As String = String.Empty
        If (txtTaskPerformedTechLicenceNo.Text.Trim.IndexOf("[") > 0 And txtTaskPerformedTechLicenceNo.Text.Trim.IndexOf("]") > 0) Then
            TaskPerformedTechLicenseNo = txtTaskPerformedTechLicenceNo.Text.Substring(0, txtTaskPerformedTechLicenceNo.Text.Trim.IndexOf("[")).Trim
            TaskPerformedTechEmpName = Mid(txtTaskPerformedTechLicenceNo.Text.Trim, txtTaskPerformedTechLicenceNo.Text.Trim.IndexOf("[") + 2, txtTaskPerformedTechLicenceNo.Text.Trim.IndexOf("]") - txtTaskPerformedTechLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        Else
            TaskPerformedTechLicenseNo = Trim(txtTaskPerformedTechLicenceNo.Text)
        End If


        mCWP.TaskPerformedTechEmployeeID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(TaskPerformedTechLicenseNo, TaskPerformedTechEmpName).EmpID
        mCWP.TaskPerformedTechEmpName = TaskPerformedTechEmpName
        mCWP.TaskPerformedTechLicenseNo = TaskPerformedTechLicenseNo


        'TaskPerformedEng Employee
        Dim TaskPerformedEngLicenseNo As String = String.Empty
        Dim TaskPerformedEngEmpName As String = String.Empty
        If (txtTaskPerformedEngLicenceNo.Text.Trim.IndexOf("[") > 0 And txtTaskPerformedEngLicenceNo.Text.Trim.IndexOf("]") > 0) Then
            TaskPerformedEngLicenseNo = txtTaskPerformedEngLicenceNo.Text.Substring(0, txtTaskPerformedEngLicenceNo.Text.Trim.IndexOf("[")).Trim
            TaskPerformedEngEmpName = Mid(txtTaskPerformedEngLicenceNo.Text.Trim, txtTaskPerformedEngLicenceNo.Text.Trim.IndexOf("[") + 2, txtTaskPerformedEngLicenceNo.Text.Trim.IndexOf("]") - txtTaskPerformedEngLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        Else
            TaskPerformedEngLicenseNo = Trim(txtTaskPerformedEngLicenceNo.Text)
        End If


        mCWP.TaskPerformedEngEmployeeID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(TaskPerformedEngLicenseNo, TaskPerformedEngEmpName).EmpID
        mCWP.TaskPerformedEngEmpName = TaskPerformedEngEmpName
        mCWP.TaskPerformedEngLicenseNo = TaskPerformedEngLicenseNo


        'FinalTestReportTech Employee
        Dim FinalTestReportTechLicenseNo As String = String.Empty
        Dim FinalTestReportTechEmpName As String = String.Empty
        If (txtFinalTestReportTechLicenceNo.Text.Trim.IndexOf("[") > 0 And txtFinalTestReportTechLicenceNo.Text.Trim.IndexOf("]") > 0) Then
            FinalTestReportTechLicenseNo = txtFinalTestReportTechLicenceNo.Text.Substring(0, txtFinalTestReportTechLicenceNo.Text.Trim.IndexOf("[")).Trim
            FinalTestReportTechEmpName = Mid(txtFinalTestReportTechLicenceNo.Text.Trim, txtFinalTestReportTechLicenceNo.Text.Trim.IndexOf("[") + 2, txtFinalTestReportTechLicenceNo.Text.Trim.IndexOf("]") - txtFinalTestReportTechLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        Else
            FinalTestReportTechLicenseNo = Trim(txtFinalTestReportTechLicenceNo.Text)
        End If


        mCWP.FinalTestReportTechEmployeeID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(FinalTestReportTechLicenseNo, FinalTestReportTechEmpName).EmpID
        mCWP.FinalTestReportTechEmpName = FinalTestReportTechEmpName
        mCWP.FinalTestReportTechLicenseNo = FinalTestReportTechLicenseNo

        'FinalTestReportEng Employee
        Dim FinalTestReportEngLicenseNo As String = String.Empty
        Dim FinalTestReportEngEmpName As String = String.Empty
        If (txtFinalTestReportEngLicenceNo.Text.Trim.IndexOf("[") > 0 And txtFinalTestReportEngLicenceNo.Text.Trim.IndexOf("]") > 0) Then
            FinalTestReportEngLicenseNo = txtFinalTestReportEngLicenceNo.Text.Substring(0, txtFinalTestReportEngLicenceNo.Text.Trim.IndexOf("[")).Trim
            FinalTestReportEngEmpName = Mid(txtFinalTestReportEngLicenceNo.Text.Trim, txtFinalTestReportEngLicenceNo.Text.Trim.IndexOf("[") + 2, txtFinalTestReportEngLicenceNo.Text.Trim.IndexOf("]") - txtFinalTestReportEngLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        Else
            FinalTestReportEngLicenseNo = Trim(txtFinalTestReportEngLicenceNo.Text)
        End If


        mCWP.FinalTestReportEngEmployeeID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(FinalTestReportEngLicenseNo, FinalTestReportEngEmpName).EmpID
        mCWP.FinalTestReportEngEmpName = FinalTestReportEngEmpName
        mCWP.FinalTestReportEngLicenseNo = FinalTestReportEngLicenseNo
        '********************************************
        mCWP.IsAttachmentAdded = IIf(mCWP.FileAttachments.Count > 0, True, False)
        mCWP.CompPartNo = txtPartNoCopy.Text.ToString  'Added by Shital on 22-Dec-2020
        Session("mCWP") = mCWP
    End Sub
    Private Sub AttachFile()
        '  If MyFile1.Value <> "" Then
        Dim BackupPath As String = ""
        BackupPath = AppSettings("DOCPath") & "New.PDF"

        Try
            If Not mCWP.FileAttachments.Contains(mCWP.ID, CType(Session("FileUpload.FileName"), String)) Then

                mCWP.FileAttachments.Add(mCWP.ID, CType(Session("FileUpload.FileName"), String)) 'Added By Vikrant On 17-Apr-2013 For ALL17042013
                ' mCWP.FileAttachments.CurrentItem.FileName = mFileAttach.FileName
                mCWP.FileAttachments.CurrentItem.ImageFile = CType(Session("ImageFile"), Byte())
                mCWP.FileAttachments.CurrentItem.Size = Session("Size")
                mCWP.FileAttachments.CurrentItem.Extension = Session("Extension")
                '   mCWP.FileAttachments.CurrentItem.SrNo = (mCWP.FileAttachments.Count - 1) + 1

                Session("mCWP") = mCWP
                dgCWPAttachment.DataSource = mCWP.FileAttachments
                dgCWPAttachment.DataBind()

                For i As Integer = 0 To mCWP.FileAttachments.Count - 1
                    Dim txtValue As TextBox
                    txtValue = CType(Me.dgCWPAttachment.Rows(i).FindControl("txtFileName"), TextBox)
                    txtValue.Text = mCWP.FileAttachments(i).FileName
                Next

                Session.Remove("Size")
                Session.Remove("ImageFile")
                Session.Remove("Extension")
                Session.Remove("FileUpload.FileName")
                upnlCWPAttachment.Update()
                upnldgCWPAttachment.Update()
            Else
                Session("mCWP") = mCWP
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub SaveAttachment() '
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                Try
                    mFileAttach.Save()
                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
                End Try
            Else
                If (Not mCWP.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mCWP.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If
    End Sub
    Private Sub ViewImage()
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString

        If mCWP.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mCWP.ID)
        End If
        If mFileAttach.Size > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            End If
        End If
    End Sub
    Private Sub SetPage()
        If mCWP.IsNew Then
            lblStatus.Text = "OPEN"
        Else
            lblStatus.Text = mCWP.StatusName
        End If
    End Sub
    Private Sub DeleteAttachment(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveAttachment")
        mCWP.FileAttachments.CurrentIndex = Index
        Session("mCWP") = mCWP
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Close" Then
                        'Added Code
                        Session("sender") = ""
                        Page.Validate()
                        If Page.IsValid Then
                            If Save(1) = True Then
                                RemoveSession()
                                Response.Redirect("index.aspx")
                            End If
                        Else
                            Session.Remove("IsValid")
                            upnlValidationsummary.Update()
                        End If
                    ElseIf MSGBoxCtrl.Sender = "RemoveAttachment" Then
                        Try
                            Session("Sender") = ""
                            Dim mCWP As CWP
                            mCWP = CType(Session("mCWP"), CWP)
                            mCWP.FileAttachments.Remove(mCWP.FileAttachments.CurrentItem)
                            dgCWPAttachment.DataSource = mCWP.FileAttachments
                            DataBind()
                            upnldgCWPAttachment.Update()
                            upnlCWPAttachment.Update()
                            Session("mCWP") = mCWP

                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "DeleteComp" Then
                        mCWP = Session("mCWP")
                        mCWP.CWPComps.Remove(mCWP.CWPComps.CurrentItem)
                        Session("mCWP") = mCWP
                        dgCWPComponent.DataSource = mCWP.CWPComps
                        dgCWPComponent.DataBind()
                        upnlCWPComponent.Update()
                    ElseIf MSGBoxCtrl.Sender = "DeleteInsp" Then
                        mCWP = Session("mCWP")
                        mCWP.CWPInspections.Remove(mCWP.CWPInspections.CurrentItem)
                        Session("mCWP") = mCWP
                        dgCWPInspection.DataSource = mCWP.CWPInspections
                        dgCWPInspection.DataBind()
                        upnlCWPInspection.Update()
                    ElseIf MSGBoxCtrl.Sender = "DeleteTaskSheet" Then
                        mCWP = Session("mCWP")
                        mCWP.CWPTaskSheets.Remove(mCWP.CWPTaskSheets.CurrentItem)
                        Session("mCWP") = mCWP
                        dgCWPTaskSheet.DataSource = mCWP.CWPTaskSheets
                        dgCWPTaskSheet.DataBind()
                        ControlVisibilityForGrid()
                        upnlCWPTaskSheet.Update()
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session.Remove("IsValid")
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    End If
                Case MsgBoxResult.Ok 'And Session("sender") = ""        'Code Added
                    'If MSGBoxCtrl.Sender = "Status" Then
                    '    ''==========================================WO - 2006-2007-1-17.doc
                    '    If mIssue.StatusID = 2 And Session("sender") <> "Close" Then
                    '        mIssue.StatusID = 1
                    '    ElseIf mIssue.StatusID = 4 Then
                    '        mIssue.StatusID = 2
                    '    End If
                    '    Session("sender") = ""
                    '    Session("mIssue") = mIssue
                    '    ''========================================
                    '    DataFieldBind()
                    '    upnlIssueDetails.Update()

                    '    'Response.Redirect("wfIssue_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    '    'Added by Utkarsh ON 15-Nov-2013 FOr TransTextSeries
                    'ElseIf MSGBoxCtrl.Sender = "ReceiptCumInvoiceTransTextSeriesAlert" Then
                    '    Session("AddTransTextSeries") = "True"
                    '    Session("sender") = "ReceiptCumInvoiceCreation" 'Need to set again
                    '    Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                    'ElseIf MSGBoxCtrl.Sender = "ResetFromStore" Then
                    '    Session("sender") = ""
                    '    cmbStoreList.ClearSelection()
                    '    upnlIssueDetails.Update()
                    'ElseIf MSGBoxCtrl.Sender = "ResetToStore" Then
                    '    Session("sender") = ""
                    '    cmbLocationStore.ClearSelection()
                    '    upnlIssueDetails.Update()
                    'Else
                    '    Session("sender") = ""
                    '    'DataFieldBind()
                    '    'upnlIssueDetails.Update()
                    '    'Response.Redirect("wfIssue_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    'End If
            End Select
        ElseIf Result1 = -1 Then
            'If mIssue.StatusID = 2 And Session("sender") <> "Close" Then
            '    mIssue.StatusID = 1
            'ElseIf mIssue.StatusID = 4 Then
            '    mIssue.StatusID = 2
            'ElseIf mIssue.StatusID = 1 Then  'Added By Prashant 27-Apr-2010
            '    mIssue.StatusID = 2
            'End If
            'Session("mIssue") = mIssue
            'Session("sender") = ""
            'upnlIssueDetails.Update()
            'Response.Redirect("wfIssue_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            'Added by Utkarsh ON 15-Nov-2013 FOr TransTextSeries
        End If
    End Sub
    Private Sub txtCWPEndDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtCWPEndDate.TextChanged
        'If txtCWPEndDate.Text <> "" Then
        '    If IsDate(txtCWPEndDate.Text) Then
        '        txtReleasedDate.Text = txtCWPEndDate.Text
        '    End If
        'Else
        '    txtReleasedDate.Text = ""
        'End If
    End Sub
    Private Sub cmbCRSEmployeeList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbCRSEmployeeList.SelectedIndexChanged
        '  If cmbCRSEmployeeList.SelectedIndex > 0 Then
        Dim mCRSLicenseNoList = LicenseNoListWithEmployee.GetLicenseNoList(mEmployeeListForCombo(New Guid(cmbCRSEmployeeList.SelectedValue.ToString)).Name, User.Identity.Name, True, "(SELECT)", False)

        cmbCRSLicenseNo.DataSource = mCRSLicenseNoList
        cmbCRSLicenseNo.DataBind()
        mCWP.CRSLicenseNo = ""
        Session("mCWP") = mCWP
        '   Else
        '   cmbCRSLicenseNo.ClearSelection()
        '   End If
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        addAttributes()
        If Not Page.IsPostBack Then
            DataFieldBind()
            SetTitle()
            ControlVisibility()
            ControlVisibilityForGrid()
        End If
    End Sub
    Private Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        If (Not IsInRole(Rights.[New]) And mCWP.IsNew) Or (Not IsInRole(Rights.Edit) And Not mCWP.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If IsValid Then
            If Save(1) = True Then
                SetTitle()
                SetPage()
                upnlTitle.Update()
                upnlStatusHeader.Update()
                upnlCWPDetail.Update()
                upnlStatus.Update()
                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
            End If
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
        SetObject()
        If mCWP.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
        Else
            RemoveSession()
            Response.Redirect("index.aspx")
        End If
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        ' mCWP.IsAttachmentAdded = True
        'ControlVisibilityForAttachment()
        AttachFile()
        upnlCWPAttachment.Update()
    End Sub
    'Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
    '    Dim fileSize1 As Integer = 0
    '    Dim file1(fileSize1) As Byte

    '    If mCWP.IsAttachmentAdded And mFileAttach Is Nothing Then
    '        mFileAttach = FileAttach.GetAttachment(mCWP.ID)
    '    End If

    '    mFileAttach.ImageFile = file1
    '    mFileAttach.Size = 0

    '    ImageButton1.Visible = False
    '    btnDelAttach.Enabled = False
    '    IsAttachmentDeleted = True
    '    mCWP.IsAttachmentAdded = False
    '    Session("IsAttachmentDeleted") = IsAttachmentDeleted
    'End Sub
    'Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
    '    ViewImage()
    'End Sub
    'Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
    '    If mCWP.IsAttachmentAdded Then
    '        mFileAttach = FileAttach.GetAttachment(mCWP.ID)
    '    Else
    '        mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mCWP.ID)
    '    End If
    '    Session("mFileAttach") = mFileAttach
    'End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub imgAddInspection_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgAddInspection.Click
        If IsValid Then
            SetObject()
            mCWP.CWPInspections.Add(mCWP.ID)
            Session("mCWP") = mCWP
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenInspectionWindow", "OpenInspectionWindow();", True)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub dgCWPInspection_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCWPInspection.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim Index As Integer = CInt(e.CommandArgument)
                Session("Edit") = True
                SetObject()
                mCWP.CWPInspections.CurrentIndex = Index - 1
                Session("mCWP") = mCWP
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenInspectionWindow", "OpenInspectionWindow();", True)
            Case "DeleteRec"
                Dim Index As Integer = CInt(e.CommandArgument)
                DeleteRecordInsp(Index)
        End Select
    End Sub
    Private Sub dgCWPTaskSheet_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgCWPTaskSheet.PageIndexChanging
        dgCWPTaskSheet.PageIndex = e.NewPageIndex
        dgCWPTaskSheet.DataSource = mCWP.CWPTaskSheets
        Session("mCWP") = mCWP
        dgCWPTaskSheet.DataBind()
        ControlVisibilityForGrid()
    End Sub
    Private Sub dgCWPTaskSheet_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCWPTaskSheet.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim Index As Integer = CInt(e.CommandArgument)
                Session("Edit") = True
                SetObject()
                mCWP.CWPTaskSheets.CurrentIndex = Index - 1
                Session("mCWP") = mCWP
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTaskSheetWindow", "OpenTaskSheetWindow();", True)
            Case "DeleteRec"
                Dim Index As Integer = CInt(e.CommandArgument)
                DeleteRecordTaskSheet(Index)
        End Select
    End Sub
    Private Sub hdnimgBtnInspection_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnInspection.Click
        dgCWPInspection.DataSource = mCWP.CWPInspections
        dgCWPInspection.DataBind()
        SetTitle()
        ControlVisibility()
        upnlCWPInspection.Update()
    End Sub
    Private Sub imgAddComponent_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgAddComponent.Click
        If IsValid Then
            SetObject()
            mCWP.CWPComps.Add(mCWP.ID)
            Session("mCWP") = mCWP
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenCWPCompWindow", "OpenCWPCompWindow();", True)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub dgCWPComponent_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgCWPComponent.PageIndexChanging
        dgCWPComponent.PageIndex = e.NewPageIndex
        dgCWPComponent.DataSource = mCWP.CWPComps
        Session("mCWP") = mCWP
        dgCWPComponent.DataBind()
    End Sub
    Private Sub dgCWPInspection_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgCWPInspection.PageIndexChanging
        dgCWPInspection.PageIndex = e.NewPageIndex
        dgCWPInspection.DataSource = mCWP.CWPInspections
        Session("mCWP") = mCWP
        dgCWPInspection.DataBind()
    End Sub
    Private Sub dgCWPComponent_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCWPComponent.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim Index As Integer = CInt(e.CommandArgument)
                Session("Edit") = True
                SetObject()
                mCWP.CWPComps.CurrentIndex = Index - 1
                Session("mCWP") = mCWP
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenCWPCompWindow", "OpenCWPCompWindow();", True)
            Case "DeleteRec"
                Dim Index As Integer = CInt(e.CommandArgument)
                DeleteRecordComp(Index)
        End Select
    End Sub
    Private Sub hdnimgBtnCWPComp_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnCWPComp.Click
        ' mCWP = CWP.GetCWP(mCWP.ID)
        dgCWPComponent.DataSource = mCWP.CWPComps
        dgCWPComponent.DataBind()
        SetTitle()
        ControlVisibility()
        upnlCWPComponent.Update()
    End Sub
    Private Sub hdnimgBtnCWPStatusChild_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnCWPStatusChild.Click
        dgStatusList.DataSource = mCWP.CWPStatusChilds
        dgStatusList.DataBind()
        upnlStatus.Update()
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As System.EventArgs) Handles btnPrint.Click
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim ds As New dsCWP
        mCWP = Session("mCWP")
        mCWP = CWP.GetCWP(mCWP.ID)

        Dim CWPTextNo As String = txtPartNoCopy.Text.ToString

        If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then 'Added By Vikrant On 01-Sep-2016 For ALL01092016
            myReport = New crptCWPBA
        Else 'End
            myReport = New crptCWP
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
         mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
         mCompanyDetail.WebSite, "", "", "", CWPTextNo, AppSettings("ClientCode"), "", AppSettings("Product Version"), AppSettings("SINote"), "", , "", "", AppSettings("Logo"))

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mCWP)
        da.Fill(ds, mCWP.CWPInspections)
        da.Fill(ds, mCWP.CWPComps)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage)
        da.Fill(ds, mCWP.CWPTaskSheets)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub btnPrintForm_Click(sender As Object, e As System.EventArgs) Handles btnPrintForm.Click
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim ds As New dsCWP
        mCWP = Session("mCWP")

        Dim CWPTextNo As String = txtPartNoCopy.Text.ToString
        myReport = New crptCWPPrintFormOne
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
         mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
         mCompanyDetail.WebSite, "", "", "", CWPTextNo, AppSettings("ClientCode"), "", AppSettings("Product Version"), AppSettings("SINote"), "", , "", "", AppSettings("Logo"))

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mCWP)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub cmbWorkShop_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbWorkShop.SelectedIndexChanged
        If cmbWorkShop.SelectedIndex > 0 Then
            txtStation.Text = mWorkShopList(New Guid(cmbWorkShop.SelectedValue.ToString)).LocationName
        End If
        upnlStation.Update()
    End Sub
    Private Sub cmbCRSLicenseNo_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbCRSLicenseNo.SelectedIndexChanged
        mCWP.CRSLicenseNo = IIf(cmbCRSLicenseNo.SelectedIndex > 0, cmbCRSLicenseNo.SelectedItem.ToString, "")
        Session("mCWP") = mCWP
    End Sub
    Private Sub dgCWPAttachment_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCWPAttachment.RowCommand
        Dim mFileAttachments As FileAttachments
        Select Case e.CommandName
            Case "View"
                Dim Index As Integer = CInt(e.CommandArgument) '+ dgCWPAttachment.PageSize * dgCWPAttachment.PageIndex

                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttachments = mCWP.FileAttachments
                mFileAttachments.CurrentIndex = Index - 1
                If mFileAttachments.CurrentItem.Size > 0 Then
                    Dim path As String = AppSettings("DOCPath") & StrName & mFileAttachments.CurrentItem.Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttachments.CurrentItem.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mFileAttachments.CurrentItem.ImageFile, 0, mFileAttachments.CurrentItem.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
                    End If
                End If
                dgCWPAttachment.DataSource = mCWP.FileAttachments
                DataBind()
                ControlVisibility()
                ControlVisibilityForGrid()
                upnlCWPAttachment.Update()
                upnldgCWPAttachment.Update()
            Case "Remove"
                Dim Index As Integer = CInt(e.CommandArgument) '+ dgCWPAttachment.PageSize * dgCWPAttachment.PageIndex

                DeleteAttachment(Index - 1)
        End Select

    End Sub
    Private Sub btnSubmit_Click(sender As Object, e As System.EventArgs) Handles btnSubmit.Click
        If (Not IsInRole(Rights.[New]) And mCWP.IsNew) Or (Not IsInRole(Rights.Edit) And Not mCWP.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If IsValid Then
            If Save(2) = True Then
                SetTitle()
                SetPage()
                upnlTitle.Update()
                upnlStatusHeader.Update()
                upnlCWPDetail.Update()
                upnlStatus.Update()
            End If
        Else
            upnlValidationsummary.Update()
        End If
    End Sub

    Private Sub btnStart_Click(sender As Object, e As System.EventArgs) Handles btnStart.Click
        If (Not IsInRole(Rights.[New]) And mCWP.IsNew) Or (Not IsInRole(Rights.Edit) And Not mCWP.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If IsValid Then
            If Save(3) = True Then
                SetTitle()
                SetPage()
                upnlTitle.Update()
                upnlStatusHeader.Update()
                upnlCWPDetail.Update()
                upnlStatus.Update()
            End If
        Else
            upnlValidationsummary.Update()
        End If
    End Sub

    Private Sub btnComplete_Click(sender As Object, e As System.EventArgs) Handles btnComplete.Click
        If (Not IsInRole(Rights.[New]) And mCWP.IsNew) Or (Not IsInRole(Rights.Edit) And Not mCWP.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If


        mCWP.CWPStatusChilds.Add(mCWP.ID)
        mCWP.CWPStatusChilds.CurrentItem.StatusID = 5
        mCWP.CWPStatusChilds.CurrentItem.StatusDate = Today.Date.ToString(AppSettings("DateFormat"))
        mCWP.CWPStatusChilds.CurrentItem.UserID = SI.UTILITY.User.GetUser(User.Identity.Name).UserID
        mCWP.CWPStatusChilds.CurrentItem.UserName = SI.UTILITY.User.GetUser(User.Identity.Name).Name

        If IsValid Then
            mCWP.StatusID = 5
            If Save(5) = True Then
                SetTitle()
                SetPage()
                upnlTitle.Update()
                upnlStatusHeader.Update()
                upnlCWPDetail.Update()
                upnlStatus.Update()
            End If
        Else
            mCWP.CWPStatusChilds.Remove(mCWP.CWPStatusChilds.CurrentItem)
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnOnHold_Click(sender As Object, e As System.EventArgs) Handles btnOnHold.Click
        mCWP.CWPStatusChilds.Add(mCWP.ID)
        mCWP.CWPStatusChilds.CurrentItem.StatusID = 4
        mCWP.CWPStatusChilds.CurrentItem.UserID = SI.UTILITY.User.GetUser(User.Identity.Name).UserID
        mCWP.CWPStatusChilds.CurrentItem.UserName = SI.UTILITY.User.GetUser(User.Identity.Name).Name
        Session("mCWP") = mCWP
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenCWPOnHoldWindow", "OpenCWPOnHoldWindow();", True)
    End Sub
    Private Sub dgStatusList_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgStatusList.PageIndexChanging
        dgStatusList.PageIndex = e.NewPageIndex
        dgStatusList.DataSource = mCWP.CWPStatusChilds
        Session("mCWP") = mCWP
        dgStatusList.DataBind()
    End Sub
    'Private Sub txtCompSerialNo_TextChanged(sender As Object, e As System.EventArgs) Handles txtCompSerialNo.TextChanged
    '    Dim LastVisitNo As Integer = mPendingOrderItemListForCwp.LastVisitNo(txtCWPDate.Text, txtPartName.Text, txtCompSerialNo.Text)
    '    If LastVisitNo > 0 Then
    '        mCWP.VisitNo = LastVisitNo + 1
    '    End If
    '    Session("mCWP") = mCWP
    'End Sub
    Private Sub imgAddTaskSheet_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgAddTaskSheet.Click
        If IsValid Then
            SetObject()
            mCWP.CWPTaskSheets.Add(mCWP.ID)
            Session("mCWP") = mCWP
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTaskSheetWindow", "OpenTaskSheetWindow();", True)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub

    Private Sub hdnimgBtnTaskSheet_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnTaskSheet.Click
        Dim mCWPFunctionList As FunctionNameList
        mCWPFunctionList = FunctionNameList.GetFunctionNameList("", "(SELECT)")
        Session("mCWPFunctionList") = mCWPFunctionList
        For i As Integer = 0 To mCWP.CWPTaskSheets.Count - 1
            mCWP.CWPTaskSheets(i).CWPFunctionName = mCWPFunctionList(mCWP.CWPTaskSheets(i).FunctionID).Name
        Next
        dgCWPTaskSheet.DataSource = mCWP.CWPTaskSheets
        dgCWPTaskSheet.DataBind()
        SetTitle()
        ControlVisibility()
        ControlVisibilityForGrid()
        upnlCWPTaskSheet.Update()
    End Sub

    Protected Sub txtBillOfWorkLicenceNo_TextChanged(sender As Object, e As System.EventArgs) 'Added by Saylee on 18-Jan-2018 for BA15012018
        Dim message As String = ""
        Dim mEmployeeStatus As EmployeeStatus
        Dim BillOfWorkEmployeeID As Guid = Guid.Empty
        Dim BillOfWorkLicenseNo As String = String.Empty
        Dim BillOfWorkEmpName As String = String.Empty

        If (txtBillOfWorkLicenceNo.Text.Trim.IndexOf("[") > 0 And txtBillOfWorkLicenceNo.Text.Trim.IndexOf("]") > 0) Then
            BillOfWorkLicenseNo = txtBillOfWorkLicenceNo.Text.Substring(0, txtBillOfWorkLicenceNo.Text.Trim.IndexOf("[")).Trim
            BillOfWorkEmpName = Mid(txtBillOfWorkLicenceNo.Text.Trim, txtBillOfWorkLicenceNo.Text.Trim.IndexOf("[") + 2, txtBillOfWorkLicenceNo.Text.Trim.IndexOf("]") - txtBillOfWorkLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        Else
            BillOfWorkLicenseNo = Trim(txtBillOfWorkLicenceNo.Text)
        End If

        BillOfWorkEmployeeID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(BillOfWorkLicenseNo, BillOfWorkEmpName).EmpID

        mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(BillOfWorkEmployeeID.ToString, mCWP.CWPDateFormatted.ToString)
        If (mEmployeeStatus(0).Information <> "") Then
            txtBillOfWorkLicenceNo.Text = ""
            message = mEmployeeStatus(0).Information
            MSGBoxCtrl.show("Save Alert!", message, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
    End Sub
    Protected Sub txtRecommendationLicenceNo_TextChanged(sender As Object, e As System.EventArgs) 'Added by Saylee on 18-Jan-2018 for BA15012018
        Dim message As String = ""
        Dim mEmployeeStatus As EmployeeStatus
        Dim RecommendationEmployeeID As Guid = Guid.Empty
        Dim RecommendationLicenseNo As String = String.Empty
        Dim RecommendationEmpName As String = String.Empty

        If (txtRecommendationLicenceNo.Text.Trim.IndexOf("[") > 0 And txtRecommendationLicenceNo.Text.Trim.IndexOf("]") > 0) Then
            RecommendationLicenseNo = txtRecommendationLicenceNo.Text.Substring(0, txtRecommendationLicenceNo.Text.Trim.IndexOf("[")).Trim
            RecommendationEmpName = Mid(txtRecommendationLicenceNo.Text.Trim, txtRecommendationLicenceNo.Text.Trim.IndexOf("[") + 2, txtRecommendationLicenceNo.Text.Trim.IndexOf("]") - txtRecommendationLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        Else
            RecommendationLicenseNo = Trim(txtRecommendationLicenceNo.Text)
        End If

        RecommendationEmployeeID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(RecommendationLicenseNo, RecommendationEmpName).EmpID

        mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(RecommendationEmployeeID.ToString, mCWP.CWPDateFormatted.ToString)
        If (mEmployeeStatus(0).Information <> "") Then
            txtRecommendationLicenceNo.Text = ""
            message = mEmployeeStatus(0).Information
            MSGBoxCtrl.show("Save Alert!", message, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
    End Sub
    Protected Sub txtTaskPerformedTechLicenceNo_TextChanged(sender As Object, e As System.EventArgs) 'Added by Saylee on 18-Jan-2018 for BA15012018
        Dim message As String = ""
        Dim mEmployeeStatus As EmployeeStatus
        Dim TaskPerformedTechEmployeeID As Guid = Guid.Empty
        Dim TaskPerformedTechLicenseNo As String = String.Empty
        Dim TaskPerformedTechEmpName As String = String.Empty

        If (txtTaskPerformedTechLicenceNo.Text.Trim.IndexOf("[") > 0 And txtTaskPerformedTechLicenceNo.Text.Trim.IndexOf("]") > 0) Then
            TaskPerformedTechLicenseNo = txtTaskPerformedTechLicenceNo.Text.Substring(0, txtTaskPerformedTechLicenceNo.Text.Trim.IndexOf("[")).Trim
            TaskPerformedTechEmpName = Mid(txtTaskPerformedTechLicenceNo.Text.Trim, txtTaskPerformedTechLicenceNo.Text.Trim.IndexOf("[") + 2, txtTaskPerformedTechLicenceNo.Text.Trim.IndexOf("]") - txtTaskPerformedTechLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        Else
            TaskPerformedTechLicenseNo = Trim(txtTaskPerformedTechLicenceNo.Text)
        End If

        TaskPerformedTechEmployeeID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(TaskPerformedTechLicenseNo, TaskPerformedTechEmpName).EmpID

        mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(TaskPerformedTechEmployeeID.ToString, mCWP.CWPDateFormatted.ToString)
        If (mEmployeeStatus(0).Information <> "") Then
            txtTaskPerformedTechLicenceNo.Text = ""
            message = mEmployeeStatus(0).Information
            MSGBoxCtrl.show("Save Alert!", message, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
    End Sub
    Protected Sub txtTaskPerformedEngLicenceNo_TextChanged(sender As Object, e As System.EventArgs) 'Added by Saylee on 18-Jan-2018 for BA15012018
        Dim message As String = ""
        Dim mEmployeeStatus As EmployeeStatus
        Dim TaskPerformedEngEmployeeID As Guid = Guid.Empty
        Dim TaskPerformedEngLicenseNo As String = String.Empty
        Dim TaskPerformedEngEmpName As String = String.Empty

        If (txtTaskPerformedEngLicenceNo.Text.Trim.IndexOf("[") > 0 And txtTaskPerformedEngLicenceNo.Text.Trim.IndexOf("]") > 0) Then
            TaskPerformedEngLicenseNo = txtTaskPerformedEngLicenceNo.Text.Substring(0, txtTaskPerformedEngLicenceNo.Text.Trim.IndexOf("[")).Trim
            TaskPerformedEngEmpName = Mid(txtTaskPerformedEngLicenceNo.Text.Trim, txtTaskPerformedEngLicenceNo.Text.Trim.IndexOf("[") + 2, txtTaskPerformedEngLicenceNo.Text.Trim.IndexOf("]") - txtTaskPerformedEngLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        Else
            TaskPerformedEngLicenseNo = Trim(txtTaskPerformedEngLicenceNo.Text)
        End If

        TaskPerformedEngEmployeeID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(TaskPerformedEngLicenseNo, TaskPerformedEngEmpName).EmpID

        mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(TaskPerformedEngEmployeeID.ToString, mCWP.CWPDateFormatted.ToString)
        If (mEmployeeStatus(0).Information <> "") Then
            txtTaskPerformedEngLicenceNo.Text = ""
            message = mEmployeeStatus(0).Information
            MSGBoxCtrl.show("Save Alert!", message, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
    End Sub
    Protected Sub txtFinalTestReportTechLicenceNo_TextChanged(sender As Object, e As System.EventArgs) 'Added by Saylee on 18-Jan-2018 for BA15012018
        Dim message As String = ""
        Dim mEmployeeStatus As EmployeeStatus
        Dim FinalTestReportTechEmployeeID As Guid = Guid.Empty
        Dim FinalTestReportTechLicenseNo As String = String.Empty
        Dim FinalTestReportTechEmpName As String = String.Empty

        If (txtFinalTestReportTechLicenceNo.Text.Trim.IndexOf("[") > 0 And txtFinalTestReportTechLicenceNo.Text.Trim.IndexOf("]") > 0) Then
            FinalTestReportTechLicenseNo = txtFinalTestReportTechLicenceNo.Text.Substring(0, txtFinalTestReportTechLicenceNo.Text.Trim.IndexOf("[")).Trim
            FinalTestReportTechEmpName = Mid(txtFinalTestReportTechLicenceNo.Text.Trim, txtFinalTestReportTechLicenceNo.Text.Trim.IndexOf("[") + 2, txtFinalTestReportTechLicenceNo.Text.Trim.IndexOf("]") - txtFinalTestReportTechLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        Else
            FinalTestReportTechLicenseNo = Trim(txtFinalTestReportTechLicenceNo.Text)
        End If

        FinalTestReportTechEmployeeID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(FinalTestReportTechLicenseNo, FinalTestReportTechEmpName).EmpID

        mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(FinalTestReportTechEmployeeID.ToString, mCWP.CWPDateFormatted.ToString)
        If (mEmployeeStatus(0).Information <> "") Then
            txtFinalTestReportTechLicenceNo.Text = ""
            message = mEmployeeStatus(0).Information
            MSGBoxCtrl.show("Save Alert!", message, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
    End Sub
    Protected Sub txtFinalTestReportEngLicenceNo_TextChanged(sender As Object, e As System.EventArgs) 'Added by Saylee on 18-Jan-2018 for BA15012018
        Dim message As String = ""
        Dim mEmployeeStatus As EmployeeStatus
        Dim FinalTestReportEngEmployeeID As Guid = Guid.Empty
        Dim FinalTestReportEngLicenseNo As String = String.Empty
        Dim FinalTestReportEngEmpName As String = String.Empty

        If (txtFinalTestReportEngLicenceNo.Text.Trim.IndexOf("[") > 0 And txtFinalTestReportEngLicenceNo.Text.Trim.IndexOf("]") > 0) Then
            FinalTestReportEngLicenseNo = txtFinalTestReportEngLicenceNo.Text.Substring(0, txtFinalTestReportEngLicenceNo.Text.Trim.IndexOf("[")).Trim
            FinalTestReportEngEmpName = Mid(txtFinalTestReportEngLicenceNo.Text.Trim, txtFinalTestReportEngLicenceNo.Text.Trim.IndexOf("[") + 2, txtFinalTestReportEngLicenceNo.Text.Trim.IndexOf("]") - txtFinalTestReportEngLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        Else
            FinalTestReportEngLicenseNo = Trim(txtFinalTestReportEngLicenceNo.Text)
        End If

        FinalTestReportEngEmployeeID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(FinalTestReportEngLicenseNo, FinalTestReportEngEmpName).EmpID

        mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(FinalTestReportEngEmployeeID.ToString, mCWP.CWPDateFormatted.ToString)
        If (mEmployeeStatus(0).Information <> "") Then
            txtFinalTestReportEngLicenceNo.Text = ""
            message = mEmployeeStatus(0).Information
            MSGBoxCtrl.show("Save Alert!", message, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
    End Sub
    Protected Sub txtTechLicenceNo_TextChanged(sender As Object, e As System.EventArgs)
        Dim currentrow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtTechLicenceNo As TextBox
        txtTechLicenceNo = CType(currentrow.FindControl("txtTechLicenceNo"), TextBox)

        Dim message As String = ""
        Dim mEmployeeStatus As EmployeeStatus
        Dim TechEmployeeID As Guid
        Dim TechLicenseNo As String = String.Empty
        Dim TechEmpName As String = String.Empty

        If (txtTechLicenceNo.Text.Trim.IndexOf("[") > 0 And txtTechLicenceNo.Text.Trim.IndexOf("]") > 0) Then
            TechLicenseNo = txtTechLicenceNo.Text.Substring(0, txtTechLicenceNo.Text.Trim.IndexOf("[")).Trim
            TechEmpName = Mid(txtTechLicenceNo.Text.Trim, txtTechLicenceNo.Text.Trim.IndexOf("[") + 2, txtTechLicenceNo.Text.Trim.IndexOf("]") - txtTechLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        Else
            TechLicenseNo = Trim(txtTechLicenceNo.Text)
        End If

        TechEmployeeID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(TechLicenseNo, TechEmpName).EmpID
        mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(TechEmployeeID.ToString, mCWP.CWPDateFormatted.ToString)
        If (mEmployeeStatus(0).Information <> "") Then
            txtTechLicenceNo.Text = ""
            message = mEmployeeStatus(0).Information
            MSGBoxCtrl.show("Save Alert!", message, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
    End Sub
    Protected Sub txtEngLicenceNo_TextChanged(sender As Object, e As System.EventArgs)
        Dim currentrow As GridViewRow = CType(sender, TextBox).Parent.Parent
        Dim txtEngLicenceNo As TextBox
        txtEngLicenceNo = CType(currentrow.FindControl("txtEngLicenceNo"), TextBox)

        Dim message As String = ""
        Dim mEmployeeStatus As EmployeeStatus
        Dim EngEmployeeID As Guid
        Dim EngLicenseNo As String = String.Empty
        Dim EngEmpName As String = String.Empty

        If (txtEngLicenceNo.Text.Trim.IndexOf("[") > 0 And txtEngLicenceNo.Text.Trim.IndexOf("]") > 0) Then
            EngLicenseNo = txtEngLicenceNo.Text.Substring(0, txtEngLicenceNo.Text.Trim.IndexOf("[")).Trim
            EngEmpName = Mid(txtEngLicenceNo.Text.Trim, txtEngLicenceNo.Text.Trim.IndexOf("[") + 2, txtEngLicenceNo.Text.Trim.IndexOf("]") - txtEngLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        Else
            EngLicenseNo = Trim(txtEngLicenceNo.Text)
        End If

        EngEmployeeID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(EngLicenseNo, EngEmpName).EmpID
        mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(EngEmployeeID.ToString, mCWP.CWPDateFormatted.ToString)
        If (mEmployeeStatus(0).Information <> "") Then
            txtEngLicenceNo.Text = ""
            message = mEmployeeStatus(0).Information
            MSGBoxCtrl.show("Save Alert!", message, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
    End Sub
#End Region

#Region " Service Methods "
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetTextList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim DistinctTextList As DistinctTextListAutoComplete
        DistinctTextList = DistinctTextListAutoComplete.GetDistinctTextList(prefixText, 24)
        If count = 0 Then
            Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).ToArray
        Else
            Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).Take(count).ToArray
        End If
    End Function

    <System.Web.Services.WebMethod(), System.Web.Script.Services.ScriptMethod()>
    Public Shared Function GetLicenseNoList(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim list As LicenseNoListWithEmployee
        list = LicenseNoListWithEmployee.GetLicenseNoList(prefixText)

        If count = 0 Then
            Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In list
               Select c.LicenseNoEmpName).ToList
        Else
            Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In list
                   Select c.LicenseNoEmpName).Take(count).ToList
        End If

    End Function
#End Region

   
End Class