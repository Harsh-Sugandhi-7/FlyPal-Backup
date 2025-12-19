Imports System.Collections.Generic
Imports javax.transaction
Imports Image = System.Web.UI.WebControls.Image

Public Class wfEmpCAAuthorization_Ajax
    Inherits System.Web.UI.Page
#Region " Enumeration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
        Authorized = 7
    End Enum
    Private Enum RequstFor
        Supplier = 0
        Customer = 1
    End Enum
#End Region

#Region " Variable Declaration "
    Public mEmpCAAuthorization As EmpCAAuthorization
    Public mEmpCAAuthorizationDetail As EmpCAAuthorizationDetail
    Public mEmpCAAuthorizationList As EmpCAAuthorizationList
    Public mEmployeeList As EmployeeList
    Dim EventLogID As Guid
    Dim mEmpCAAuthorizationDetail1 As String
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim mUser As User
    Dim email As Thread
    Dim mCAAuthorizationScopeList As CAAuthorizationScopeList
    Dim mCALimitationList As CALimitationList
    Dim mModuleList As ModuleList
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mEmpCAAuthorization = CType(Session("mEmpCAAuthorization"), EmpCAAuthorization)
        mEmployeeList = CType(Session("mEmployeeList"), EmployeeList)
        mFileAttach = Session("mFileAttachEmpCAAuthorization")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        mModuleList = Session("mModuleList") 'Added by Sachin
        mEmpCAAuthorizationList = Session("mEmpCAAuthorizationList")

    End Sub
    Private Sub SetSession()
        Session("mEmpCAAuthorization") = mEmpCAAuthorization
        Session("mEmployeeList") = mEmployeeList
        Session("mFileAttachEmpCAAuthorization") = mFileAttach
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
        'End
    End Sub
    Private Sub RemoveSessions()
        Session.Remove("mEmpCAAuthorization")
        Session.Remove("mEmployeeList")
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        Session.Remove("mFileAttachEmpCAAuthorization")
        'End
    End Sub

    Private Sub Save()
        'Authentication
        If Not mEmpCAAuthorization.EmpCAAuthorizationDate Is System.DBNull.Value Then
            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
            If mCheck.WebAuthentication = True Then
                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")
                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                If DateDiff(DateInterval.Day, CDate(mEmpCAAuthorization.EmpCAAuthorizationDate), maxAllowableDate) < 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Goods Receipt. <br> Goods Receipt Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If
        End If
        Dim EmpCAAuthorizationClone As EmpCAAuthorization
        EmpCAAuthorizationClone = mEmpCAAuthorization.Clone
        Try

            'check whether min. one item & charge is present while saving
            If Not mEmpCAAuthorization.EmpCAAuthorizationDetails.Count = 0 Then
                'save the object
                SetObject()
                setObjectAuthorizationDetail()

                If mEmpCAAuthorization.IsValid Then
                    Dim i As Integer
                    While i < mEmpCAAuthorization.EmpCAAuthorizationDetails.Count
                        i = i + 1
                    End While
                    mEmpCAAuthorization.ApplyEdit()

                    mEmpCAAuthorization.Save()

                    SaveAttachment()

                    mEmpCAAuthorizationDetail1 = "Authorization No.: " + mEmpCAAuthorization.CANumber + " Dated: " + mEmpCAAuthorization.EmpCAAuthorizationDateFormatted + " Employee: " + mEmpCAAuthorization.EmployeeName
                    MarkLog(Util.Action.Save, "EmpCAAuthorization", mEmpCAAuthorizationDetail1, Util.ErrorType.NoError, mEmpCAAuthorization.ID, EventLogID)
                    mEmpCAAuthorization.MarkClean()
                    Session("mEmpCAAuthorization") = mEmpCAAuthorization
                    DataFieldBind()

                    SetGrid()
                    SetControl()
                    ControlVisibility()
                    ControlVisibilityForFileAttachment()
                    SetControlStatus(mEmpCAAuthorization.StatusID)

                    If mEmpCAAuthorization.IsNew Then
                        lblTitle.Text = "Company Authorization [New]"
                    Else
                        lblTitle.Text = "Company Authorization"

                    End If
                    upnlTitle.Update()
                    upnlEmpCAAuthorizationDetails.Update()
                    'upnlEmpCAAuthorization.Update()
                    upnlEmpCAAuthorizationDetail.Update()
                    upnlEmpCAAuthorizationTerms.Update()
                    upnlButtons.Update()
                    upnlStatusName.Update()

                    MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                Else
                    Dim mRule As String = ""
                    If mEmpCAAuthorization.GetBrokenRulesCollection.Count > 0 Then
                        mRule = mEmpCAAuthorization.GetBrokenRulesCollection(0).Description
                    ElseIf mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.GetBrokenRulesCollection.Count > 0 Then
                        mRule = mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.GetBrokenRulesCollection(0).Description
                    End If
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, mRule, MsgBoxStyle.OkOnly, "")
                    mRule = ""
                    mEmpCAAuthorization = EmpCAAuthorizationClone
                    SetObject()
                    setObjectAuthorizationDetail()
                    Session("mEmpCAAuthorization") = mEmpCAAuthorization
                    DataFieldBind()
                    Exit Sub
                End If
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Company Authorization can not be saved without details.", MsgBoxStyle.OkOnly, "")
                mEmpCAAuthorization = EmpCAAuthorizationClone
                SetObject()
                setObjectAuthorizationDetail()
                Session("mEmpCAAuthorization") = mEmpCAAuthorization
                DataFieldBind()
                Exit Sub
            End If
        Catch ex As SqlException
            Session("EmpCAAuthorizationClone") = EmpCAAuthorizationClone
            If ex.Number = 8114 Or ex.Number = 8115 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf ex.Number = 547 Then
                If InStr(ex.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or InStr(ex.Message, "CCtabIssueItemReceiptBalanceQty", CompareMethod.Text) Or InStr(ex.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or InStr(ex.Message, "*15-TB02-CX07*", CompareMethod.Text) Or InStr(ex.Message, "*17-TB02-CX06*", CompareMethod.Text) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex.Message.Substring(ex.Message.IndexOf("PartNo.:")) + " Goods Receipt Qty can not be greater than Order / Issue Qty.", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf InStr(ex.Message, "CCtabOrderItemReceiptBalanceQty", CompareMethod.Text) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex.Message.Substring(ex.Message.IndexOf("PartNo.:")) + "Goods Receipt Qty can not be greater than Order Qty.</br></br><b>Please amend Purchase Order for Receipt of excess quantity.</b>", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf InStr(ex.Message, "FKtabInvoiceChargetabCharge", CompareMethod.Text) Then
                    MSGBoxCtrl.Show("Alert!", "Other Charge Deleted ! ", "Other charge Not Available<Br><BR>Selected Charge is no longer exist in the Database <BR><BR> Remove Charge and try Again", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    MSGBoxCtrl.Show("Alert!", "Save Alert ! " + "</br>" + "There is some problem in Saving Goods Receipt. <BR> <BR>  Please Check the Entry and Try Again  !", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If
        Catch ex1 As Exception
            If InStr(ex1.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or InStr(ex1.Message, "CCtabIssueItemReceiptBalanceQty", CompareMethod.Text) Or InStr(ex1.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or InStr(ex1.Message, "*15-TB02-CX07*", CompareMethod.Text) Or InStr(ex1.Message, "*17-TB02-CX06*", CompareMethod.Text) Then
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + " Goods Receipt Qty can not be greater than Order / Issue Qty.", MsgBoxStyle.OkOnly, "Status")
                mEmpCAAuthorization = EmpCAAuthorizationClone
                SetObject()
                Session("mEmpCAAuthorization") = mEmpCAAuthorization
                DataFieldBind()
                Exit Sub
            ElseIf InStr(ex1.Message, "CCtabOrderItemReceiptBalanceQty", CompareMethod.Text) Then
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Goods Receipt Qty can not be greater than Order Qty.</br><b>Please amend Purchase Order Quantity & make Goods Receipt again.</b>", MsgBoxStyle.OkOnly, "")
            Else
                MSGBoxCtrl.Show("Alert!", "Save Alert ! " + "</br>" + "There is some problem in Saving Goods Receipt. <BR> <BR>  Please Check the Entry and Try Again  !", "", MsgBoxStyle.OkOnly, "Status")
                mEmpCAAuthorization = EmpCAAuthorizationClone
                SetObject()
                Session("mEmpCAAuthorization") = mEmpCAAuthorization
                DataFieldBind()
                Exit Sub
            End If
            mEmpCAAuthorization = EmpCAAuthorizationClone
            Session("mEmpCAAuthorization") = mEmpCAAuthorization
        Finally
            EmpCAAuthorizationClone = Nothing
        End Try
    End Sub

    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub SetObject()
        If txtEmpCAAuthorizationDate.Text = "" Then
            mEmpCAAuthorization.EmpCAAuthorizationDate = Today.Date
        Else
            mEmpCAAuthorization.EmpCAAuthorizationDate = CDate(txtEmpCAAuthorizationDate.Text)
        End If
        mEmpCAAuthorization.Text = txtEmpCAAuthorizationText.Text.Trim
        mEmpCAAuthorization.No = Val(txtEmpCAAuthorizationNo.Text)
        mEmpCAAuthorization.CANumber = Trim(txtCANo.Text)
        mEmpCAAuthorization.RevisionNo = Trim(txtRevisionNo.Text)
        mEmpCAAuthorization.EmployeeID = New Guid(cmbEmployee.SelectedValue)
        mEmpCAAuthorization.EmployeeCode = txtEmployeeCode.Text.Trim
        If txtFromDate.Text = "" Then
            mEmpCAAuthorization.CAInitialIssueDate = System.DBNull.Value
        Else
            mEmpCAAuthorization.CAInitialIssueDate = CDate(txtFromDate.Text)
        End If
        If txtToDate.Text = "" Then
            mEmpCAAuthorization.CAValidUpto = System.DBNull.Value
        Else
            mEmpCAAuthorization.CAValidUpto = CDate(txtToDate.Text)
        End If
        mEmpCAAuthorization.Remark = Trim(txtRemark.Text)
        mEmpCAAuthorization.AMELCat = txtAMELCat.Text.Trim
        mEmpCAAuthorization.RevisionNo = txtRevisionNo.Text
        If txtRevisionDate.Text = "" Then
            mEmpCAAuthorization.RevisionDate = System.DBNull.Value
        Else
            mEmpCAAuthorization.RevisionDate = CDate(txtRevisionDate.Text)
        End If
        mEmpCAAuthorization.AMELNo = txtAMELNo.Text
        If txtDateOfExpiry.Text = "" Then
            mEmpCAAuthorization.DateOfExpiry = System.DBNull.Value
        Else
            mEmpCAAuthorization.DateOfExpiry = CDate(txtDateOfExpiry.Text)
        End If

        If txtContinuationTrainingValidity.Text = "" Then
            mEmpCAAuthorization.ContinuationTrainingValidity = System.DBNull.Value
        Else
            mEmpCAAuthorization.ContinuationTrainingValidity = CDate(txtContinuationTrainingValidity.Text)
        End If
    End Sub

    Private Sub setObjectAuthorizationDetail()
        Dim mEmpCAAuthorizationClone As EmpCAAuthorization
        mEmpCAAuthorizationClone = mEmpCAAuthorization.Clone
        Try
            Dim child As EmpCAAuthorizationDetail
            Dim ID As Guid
            For i As Integer = 0 To dgEmpCAAuthorizationDetail.Rows.Count - 1
                ID = New Guid(dgEmpCAAuthorizationDetail.DataKeys(i).Values("ID").ToString)
                child = mEmpCAAuthorization.EmpCAAuthorizationDetails.Item(ID)
                Dim txtAuthorizationDetails As TextBox = TryCast(dgEmpCAAuthorizationDetail.Rows(i).FindControl("txtAuthorizationDetails"), TextBox)
                Dim txtLimitations As TextBox = TryCast(dgEmpCAAuthorizationDetail.Rows(i).FindControl("txtLimitations"), TextBox)
                Dim txtRevNo As TextBox = TryCast(dgEmpCAAuthorizationDetail.Rows(i).FindControl("txtRevNo"), TextBox)
                Dim txtRevDate As TextBox = TryCast(dgEmpCAAuthorizationDetail.Rows(i).FindControl("txtRev"), TextBox)


                child.AuthorizationDetails = txtAuthorizationDetails.Text.Trim
                child.LimitationsDetails = txtLimitations.Text.Trim
                child.RevNo = txtRevNo.Text.Trim
                If txtRevDate.Text = "" Then
                    child.RevDate = System.DBNull.Value
                Else
                    child.RevDate = CDate(txtRevDate.Text)
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentIndex = Index
        Session("mEmpCAAuthorization") = mEmpCAAuthorization
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            mEmpCAAuthorization = CType(Session("mEmpCAAuthorization"), EmpCAAuthorization)
                            mEmpCAAuthorization.EmpCAAuthorizationDetails.Remove(mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem)
                            mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentIndex = mEmpCAAuthorization.EmpCAAuthorizationDetails.Count - 1
                            dgEmpCAAuthorizationDetail.DataSource = mEmpCAAuthorization.EmpCAAuthorizationDetails
                            dgEmpCAAuthorizationDetail.DataBind()
                            SetGrid()
                            SetControl()
                            ControlVisibility()
                            upnlEmpCAAuthorizationDetail.Update()
                            Session("mEmpCAAuthorization") = mEmpCAAuthorization
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "Status" Then
                        Session("sender") = ""
                        If mEmpCAAuthorization.IsValid = True Then
                            Session.Remove("IsValid")
                            mEmpCAAuthorization.StatusID = 2
                            Save()
                            Session.Remove("IsValid")
                        Else
                            If CustomValidate1() = False Then
                                upnlValidationsummary.Update()
                                Exit Sub
                            End If
                        End If
                    End If
                    If MSGBoxCtrl.Sender = "Close" Then
                        If CustomValidateAuthorization() = False Then upnlValidationsummary.Update() : Exit Sub
                        If mEmpCAAuthorization.IsValid = True Then
                            Session.Remove("IsValid")
                            DataFieldBind()
                            If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                                Exit Sub
                            End If
                            Save()
                        Else
                            Session.Remove("IsValid")
                            If CustomValidate1() = False Then
                                upnlValidationsummary.Update()
                                Exit Sub
                            End If
                        End If
                    End If
                    If MSGBoxCtrl.Sender = "DeleteDetailAttachment" Then
                        mFileAttach = FileAttach.GetAttachment(mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.ID)
                        mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.FileAttachments.CurrentIndex = 0
                        mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.FileAttachments.Remove(mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.FileAttachments.CurrentItem)
                        mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.IsAttachmentAdded = False
                        dgEmpCAAuthorizationDetail.DataSource = mEmpCAAuthorization.EmpCAAuthorizationDetails
                        dgEmpCAAuthorizationDetail.DataBind()
                        SetGrid()
                        SetControl()
                        ControlVisibility()
                        upnlEmpCAAuthorizationDetail.Update()
                    End If
                    If MSGBoxCtrl.Sender = "DeleteTerm" Then
                        Try
                            Session("Sender") = ""
                            Dim mEmpCAAuthorization As EmpCAAuthorization
                            mEmpCAAuthorization = CType(Session("mEmpCAAuthorization"), EmpCAAuthorization)
                            mEmpCAAuthorization.EmpCAAuthorizationTerms.Remove(mEmpCAAuthorization.EmpCAAuthorizationTerms.CurrentItem)
                            Session("mEmpCAAuthorization") = mEmpCAAuthorization
                            dgEmpCAAuthorizationTerms.DataSource = mEmpCAAuthorization.EmpCAAuthorizationTerms
                            dgEmpCAAuthorizationTerms.DataBind()
                            upnlEmpCAAuthorizationTerms.Update()
                        Catch ex As SqlException
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(ex.Message, False), True)
                            Exit Sub
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session.Remove("IsValid")
                        Session.Remove("mModuleName")
                        Session.Remove("mPendingItemList")
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    End If

                Case MsgBoxResult.Ok
                    'If MSGBoxCtrl.Sender = "RCITransTextSeriesAlert" Then
                    '    Session("sender") = ""
                    '    Session("AddTransTextSeries") = "True"
                    '    Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                    'End If
                    If MSGBoxCtrl.Sender = "Status" Then
                        Session("sender") = ""
                        If mEmpCAAuthorization.StatusID = 2 Then
                            mEmpCAAuthorization.StatusID = 1

                        End If
                        Session("mEmpCAAuthorization") = mEmpCAAuthorization
                        DataFieldBind()
                    End If
            End Select
        End If
    End Sub
    Private Sub SetReceivedFromDetails(ByVal ToType As Int16)

    End Sub
    Private Sub addAttributes()
        txtEmpCAAuthorizationNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtEmpCAAuthorizationNo').value,event)")
    End Sub

    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = "EmpCAAuthorization"
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
            Case Rights.Authorized
                Return User.IsInRole(IsInRoleString + "Authorized")
        End Select
    End Function
    Private Sub ControlVisibility()
        btnAuthorized.Visible = (Not mEmpCAAuthorization.EmpCAAuthorizationDetails.Count = 0) And (Not mEmpCAAuthorization.IsNew) And (mEmpCAAuthorization.StatusID = 1)
        ImgDetailsAdd.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)
        ImgDetailsAddBottom.Visible = IIf(mEmpCAAuthorization.StatusID = 1 And mEmpCAAuthorization.EmpCAAuthorizationDetails.Count > 5, True, False)
        imgTermsAdd.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)
        'pnlCAAuthorizationDetails.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)
        txtEmpCAAuthorizationDate.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)
        txtEmpCAAuthorizationText.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)
        txtEmpCAAuthorizationNo.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)
        '   txtAMELNo.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)
        '  txtAMELCat.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)

        If mEmpCAAuthorization.StatusID > 1 Or Session("IsEmpCAAuthorizationForRenew") = "True" Then
            cmbEmployee.Enabled = False
            txtEmployeeCode.Enabled = False
            txtCANo.Enabled = False
            txtAMELNo.Enabled = False
            txtAMELCat.Enabled = False
            txtContinuationTrainingValidity.Enabled = False
        Else
            cmbEmployee.Enabled = True
            txtEmployeeCode.Enabled = True
            txtCANo.Enabled = True
            txtAMELNo.Enabled = True
            txtAMELCat.Enabled = True
            txtContinuationTrainingValidity.Enabled = True
        End If


        txtDateOfExpiry.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)
        txtRevisionNo.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)
        txtRevisionDate.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)
        txtFromDate.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)
        txtToDate.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)
        'pnlCAAuthorizationDetails.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)
        'btnDelAttach.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)
        'ImageButton1.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)
        txtRemark.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)

        '' pnlCompanyDetails.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)
        pnlTermsDetails.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)
        dgEmpCAAuthorizationTerms.Columns(2).Visible = IIf(mEmpCAAuthorization.StatusID > 1, False, True)
        For j As Integer = 0 To mEmpCAAuthorization.EmpCAAuthorizationDetails.Count - 1
            Dim DeleteRecord As ImageButton = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(10).FindControl("DeleteRecord"), ImageButton)
            DeleteRecord.Visible = IIf(mEmpCAAuthorization.StatusID > 1, False, True)

            Dim View As ImageButton = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(10).FindControl("View"), ImageButton)
            View.Visible = IIf(mEmpCAAuthorization.EmpCAAuthorizationDetails(j).IsAttachmentAdded = True, True, False)

            Dim lnkArrow As Image = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(10).FindControl("lnkArrow"), Image)

            If DeleteRecord.Visible = False And View.Visible = False Then
                lnkArrow.Visible = False
            End If


            Dim txtAuthorizationDetails As TextBox = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(1).FindControl("txtAuthorizationDetails"), TextBox)
            txtAuthorizationDetails.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)

            Dim lnkbtnAddSCOPE As LinkButton = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(2).FindControl("lnkbtnAddSCOPE"), LinkButton)
            lnkbtnAddSCOPE.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)

            Dim lnkbtnAddLICENSE As LinkButton = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(4).FindControl("lnkbtnAddLICENSE"), LinkButton)
            lnkbtnAddLICENSE.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)


            Dim txtLimitations As TextBox = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(6).FindControl("txtLimitations"), TextBox)
            txtLimitations.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)

            Dim txtRevNo As TextBox = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(7).FindControl("txtRevNo"), TextBox)
            txtRevNo.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)

            Dim txtRev As TextBox = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(8).FindControl("txtRev"), TextBox)
            txtRev.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)

        Next
    End Sub
    Private Sub ControlVisibilityForFileAttachment()
        If mEmpCAAuthorization.IsAttachmentAdded Then
            ImageButton1.Visible = True
            If mEmpCAAuthorization.StatusID = 1 Then
                btnDelAttach.Enabled = True
            Else
                btnDelAttach.Enabled = False
            End If
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Public Sub SetReport(Optional ByVal ByMail As Boolean = False)

    End Sub
    Private Sub DeleteCATerms(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveTerm, MSGBox.Message_text.RemoveTerm, "", MsgBoxStyle.YesNo, "DeleteTerm")
        mEmpCAAuthorization.EmpCAAuthorizationTerms.CurrentIndex = Index
        Session("mEmpCAAuthorization") = mEmpCAAuthorization
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mEmployeeList = EmployeeList.GetEmployeeList("", "", "(SELECT)")
        cmbEmployee.DataSource = mEmployeeList
        Session("mEmployeeList") = mEmployeeList

        dgEmpCAAuthorizationDetail.DataSource = mEmpCAAuthorization.EmpCAAuthorizationDetails


        ''Added by Sachin
        dgEmpCAAuthorizationTerms.DataSource = mEmpCAAuthorization.EmpCAAuthorizationTerms
        ''
        Session("mEmpCAAuthorization") = mEmpCAAuthorization
        txtEmpCAAuthorizationDate.Text = mEmpCAAuthorization.EmpCAAuthorizationDateFormatted.ToString
        txtFromDate.Text = mEmpCAAuthorization.CAInitialIssueDateFormatted.ToString
        txtToDate.Text = mEmpCAAuthorization.CAValidUptoFormatted.ToString
        txtContinuationTrainingValidity.Text = mEmpCAAuthorization.ContinuationTrainingValidityFormatted.ToString()

        DataBind()
        cmbEmployee.SelectedValue = mEmpCAAuthorization.EmployeeID.ToString
    End Sub
    Private Sub SetGrid()

        For j As Integer = 0 To dgEmpCAAuthorizationDetail.Rows.Count - 1
            Dim lnkbtnAddSCOPE As LinkButton = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(2).FindControl("lnkbtnAddSCOPE"), LinkButton)
            lnkbtnAddSCOPE.Text = dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(3).Text

            Dim lnkbtnAddLICENSE As LinkButton = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(4).FindControl("lnkbtnAddLICENSE"), LinkButton)
            lnkbtnAddLICENSE.Text = dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(5).Text
        Next
    End Sub
    Private Sub SetControl()

        For j As Integer = 0 To mEmpCAAuthorization.EmpCAAuthorizationDetails.Count - 1
            Dim txtRev As TextBox = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(8).FindControl("txtRev"), TextBox)
            txtRev.Text = mEmpCAAuthorization.EmpCAAuthorizationDetails(j).RevDateFormatted.ToString

            Dim DeleteRecord As ImageButton = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(10).FindControl("DeleteRecord"), ImageButton)
            DeleteRecord.Visible = IIf(mEmpCAAuthorization.StatusID > 1, False, True)
        Next
    End Sub
    Private Sub SetControlStatus(ByVal StatusId As Int16)
        btnSave.Visible = IIf(StatusId > 1, False, True)
        ImgDetailsAdd.Visible = IIf(StatusId > 1, False, True)
        imgTermsAdd.Visible = IIf(StatusId > 1, False, True)
        dgEmpCAAuthorizationDetail.Columns(9).Visible = IIf(StatusId > 1, False, True)
        btnSelectFile.Disabled = IIf(StatusId > 1, True, False)
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator
        CustValid = CType(s, CustomValidator)
        If CustValid.ControlToValidate = "cmbEmployee" Then


        ElseIf CustValid.ControlToValidate = "txtRemark" Then
            If Len(Trim(txtRemark.Text)) > 1000 Then
                CustValid.ErrorMessage = "Max. Length of Remark should be 1000."
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
        addAttributes()

        If Not IsPostBack And Session("Sender") = "" Then
            If CType(Session("AddTransTextSeries"), String) = "True" AndAlso (Not Session("TransText_ForTransSeries") Is Nothing) Then
                If mEmpCAAuthorization.IsNew Then
                    mEmpCAAuthorization.Text = Session("TransText_ForTransSeries")
                    Session("mEmpCAAuthorization") = mEmpCAAuthorization
                    Session("AddTransTextSeries") = "False"
                    Session.Remove("TransName_ForTransSeries")
                    Session.Remove("TransText_ForTransSeries")
                    Session.Remove("TransNo_ForTransSeries")
                End If
            End If
            'End
            DataFieldBind()
            SetControl()
            SetGrid()
            SetControlStatus(mEmpCAAuthorization.StatusID)
            ControlVisibility()
            ControlVisibilityForFileAttachment()
        End If
        If mEmpCAAuthorization.IsNew Then
            lblTitle.Text = "Company Authorization [New]"
        Else
            lblTitle.Text = "Company Authorization"
        End If

    End Sub
    Public Function CustomValidateAuthorization() As Boolean
        Dim strError As String = String.Empty
        Dim strError1 As String = String.Empty
        Dim strError2 As String = String.Empty
        Dim strError3 As String = String.Empty
        Dim strError4 As String = String.Empty

        Dim txtAuthorizationDetails As TextBox
        Dim rfvAuthorizationDetails As RequiredFieldValidator
        Dim upnlAuthorizationDetailsValidate As UpdatePanel

        Dim txtLimitations As TextBox
        Dim rfvLimitations As RequiredFieldValidator
        Dim upnlLimitationsValidate As UpdatePanel


        For j As Integer = 0 To dgEmpCAAuthorizationDetail.Rows.Count - 1

            'AuthorizationDetails
            rfvAuthorizationDetails = CType(Me.dgEmpCAAuthorizationDetail.Rows(j).FindControl("rfvAuthorizationDetails"), RequiredFieldValidator)
            upnlAuthorizationDetailsValidate = CType(Me.dgEmpCAAuthorizationDetail.Rows(j).FindControl("upnlAuthorizationDetailsValidate"), UpdatePanel)
            txtAuthorizationDetails = CType(Me.dgEmpCAAuthorizationDetail.Rows(j).FindControl("txtAuthorizationDetails"), TextBox)

            If txtAuthorizationDetails.Text = "" Then
                rfvAuthorizationDetails.IsValid = False
                rfvAuthorizationDetails.Text = "* Authorization Details Required"
                strError2 = "* Authorization Details Required"
                upnlAuthorizationDetailsValidate.Update()
            End If

            'Limitations
            rfvLimitations = CType(Me.dgEmpCAAuthorizationDetail.Rows(j).FindControl("rfvLimitations"), RequiredFieldValidator)
            upnlLimitationsValidate = CType(Me.dgEmpCAAuthorizationDetail.Rows(j).FindControl("upnlLimitationsValidate"), UpdatePanel)
            txtLimitations = CType(Me.dgEmpCAAuthorizationDetail.Rows(j).FindControl("txtLimitations"), TextBox)

            If txtLimitations.Text = "" Then
                rfvLimitations.IsValid = False
                rfvLimitations.Text = "* Limitations Required"
                strError2 = "* Limitations Required"
                upnlLimitationsValidate.Update()
            End If

        Next



        If strError <> "" Or strError1 <> "" Or strError3 <> "" Or strError4 <> "" Or strError2 <> "" Then
            Return False
        Else

        End If

        Return True
    End Function
    Private Sub ImgDetailsAdd_Click(sender As Object, e As ImageClickEventArgs) Handles ImgDetailsAdd.Click, ImgDetailsAddBottom.Click
        'If IsValid Then
        '    SetObject()
        '    'mEmpCAAuthorization.EmpCAAuthorizationDetails.Add(mEmpCAAuthorization.ID)
        '    Session("mEmpCAAuthorization") = mEmpCAAuthorization
        '    Session("EditEmpCAAuthorizationDetail") = False
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpCAAuthorizationDetailWindow", "OpenEmpCAAuthorizationDetailWindow();", True)
        'Else
        '    upnlValidationsummary.Update()
        'End If

        If CustomValidateAuthorization() = False Then upnlValidationsummary.Update() : Exit Sub
        SetObject()
        setObjectAuthorizationDetail()
        mEmpCAAuthorization.EmpCAAuthorizationDetails.Add(mEmpCAAuthorization.ID)
        dgEmpCAAuthorizationDetail.DataSource = mEmpCAAuthorization.EmpCAAuthorizationDetails
        dgEmpCAAuthorizationDetail.DataBind()

        SetGrid()
        SetControl()
        ControlVisibility()
        Dim lblAuthorizationDetailsStar As Label
        lblAuthorizationDetailsStar = dgEmpCAAuthorizationDetail.HeaderRow().FindControl("lblAuthorizationDetailsStar")
        lblAuthorizationDetailsStar.Visible = True


        Dim lblLimitationsStar As Label
        lblLimitationsStar = dgEmpCAAuthorizationDetail.HeaderRow().FindControl("lblLimitationsStar")
        lblLimitationsStar.Visible = True


        upnlEmpCAAuthorizationDetail.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, " CallLostCAAuthorizationResize()", " CallLostCAAuthorizationResize();", True)

    End Sub

    Private Sub dgEmpCAAuthorizationDetail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgEmpCAAuthorizationDetail.RowCommand
        Dim mQtyBalReceived As Decimal = 0
        Select Case e.CommandName
            'Case "EditView"
            '    Dim index As Int32 = CInt(e.CommandArgument) + dgEmpCAAuthorizationDetail.PageIndex * dgEmpCAAuthorizationDetail.PageSize
            '    'If mEmpCAAuthorization.EmpCAAuthorizationDetails.Item(Index:=index).OrderNumber <> "" Then
            '    '    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Can not edit as used in order " + mEmpCAAuthorization.EmpCAAuthorizationDetails.Item(Index:=index).OrderNumber, MsgBoxStyle.OkOnly, "")
            '    '    Exit Sub
            '    'End If
            '    'If mEmpCAAuthorization.EmpCAAuthorizationDetails.Item(Index:=index).WorkOrderNumber <> "" Then
            '    '    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Can not edit as used in work order " + mEmpCAAuthorization.EmpCAAuthorizationDetails.Item(Index:=index).WorkOrderNumber, MsgBoxStyle.OkOnly, "")
            '    '    Exit Sub
            '    'End If
            '    SetObject()
            '    setObjectAuthorizationDetail()
            '    mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentIndex = index
            '    Session("mEmpCAAuthorization") = mEmpCAAuthorization
            '    Session("EditEmpCAAuthorizationDetail") = True
            '    Dim tmpEmpCAAuthorization As EmpCAAuthorization = mEmpCAAuthorization.Clone
            '    Session("tmpEmpCAAuthorization") = tmpEmpCAAuthorization
            '    Session("ItemIndex") = mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentIndex
            '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpCAAuthorizationDetailWindow", "OpenEmpCAAuthorizationDetailWindow();", True)
            Case "SCOPERec"

                If CustomValidateAuthorization() = False Or CustomValidate1() = False Then upnlValidationsummary.Update() : Exit Sub


                Dim index As Int32 = (CInt(e.CommandArgument) - 1) + dgEmpCAAuthorizationDetail.PageIndex * dgEmpCAAuthorizationDetail.PageSize
                mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentIndex = index

                'If mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.IsNew Then
                '    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Record need to be saved before adding Scope ", MsgBoxStyle.OkOnly, "")
                '    Exit Sub
                'End If
                SetObject()
                setObjectAuthorizationDetail()
                Session("mEmpCAAuthorization") = mEmpCAAuthorization
                Session("mEmpCAAuthorizationDetail") = mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenScopeWindow", "OpenScopeWindow()", True)
            Case "LICENSRec"

                If CustomValidateAuthorization() = False Or CustomValidate1() = False Then upnlValidationsummary.Update() : Exit Sub


                Dim index As Int32 = (CInt(e.CommandArgument) - 1) + dgEmpCAAuthorizationDetail.PageIndex * dgEmpCAAuthorizationDetail.PageSize
                mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentIndex = index

                'If mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.IsNew Then
                '    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Record need to be saved before adding Limitation ", MsgBoxStyle.OkOnly, "")
                '    Exit Sub
                'End If
                SetObject()
                setObjectAuthorizationDetail()
                Session("mEmpCAAuthorization") = mEmpCAAuthorization
                Session("mEmpCAAuthorizationDetail") = mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenLimitationWindow", "OpenLimitationWindow()", True)

            Case "DeleteRecord"
                Dim index As Int32 = (CInt(e.CommandArgument) - 1) + dgEmpCAAuthorizationDetail.PageIndex * dgEmpCAAuthorizationDetail.PageSize
                'If mEmpCAAuthorization.EmpCAAuthorizationDetails.Item(Index:=index).OrderNumber <> "" Then
                '    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Can not remove as used in order " + mEmpCAAuthorization.EmpCAAuthorizationDetails.Item(Index:=index).OrderNumber, MsgBoxStyle.OkOnly, "")
                '    Exit Sub
                'End If
                'If mEmpCAAuthorization.EmpCAAuthorizationDetails.Item(Index:=index).WorkOrderNumber <> "" Then
                '    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Can not remove as used in work order " + mEmpCAAuthorization.EmpCAAuthorizationDetails.Item(Index:=index).WorkOrderNumber, MsgBoxStyle.OkOnly, "")
                '    Exit Sub
                'End If
                DeleteRecord(index)
            Case "SelectFile"
                SetObject()
                setObjectAuthorizationDetail()
                Dim index As Int32 = (CInt(e.CommandArgument) - 1) + dgEmpCAAuthorizationDetail.PageIndex * dgEmpCAAuthorizationDetail.PageSize
                mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentIndex = index

                'If mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.IsAttachmentAdded = True Then
                '    mFileAttach = FileAttach.GetAttachment(mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.ID)
                'Else
                '    mFileAttach = FileAttach.NewAttachment(Guid.Empty, mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.ID)
                'End If
                Session("mEmpCAAuthorization") = mEmpCAAuthorization
                Session("DetailAttachement") = "True"
                'Session("mFileAttach") = Nothing
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow()", True)
            Case "RemoveAttachRec"
                Dim index As Int32 = (CInt(e.CommandArgument) - 1) + dgEmpCAAuthorizationDetail.PageIndex * dgEmpCAAuthorizationDetail.PageSize
                mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentIndex = index
                MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "DeleteDetailAttachment")

            Case "ViewRec"
                Dim index As Int32 = (CInt(e.CommandArgument) - 1) + dgEmpCAAuthorizationDetail.PageIndex * dgEmpCAAuthorizationDetail.PageSize
                mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentIndex = index
                If mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.IsAttachmentAdded And mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.FileAttachments.Count > 0 Then
                    mFileAttach = FileAttach.GetAttachment(mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.ID)
                    'Session("mFileAttach") = mFileAttach

                    Dim No As New Random
                    Dim StrName As String = "abc" & No.Next.ToString
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
                End If
        End Select
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not IsInRole(Rights.Print) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim da As New CSLA.Data.ObjectAdapter
        Dim MyReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim obj As EmpCAAuthorization
        Dim mCompanyDetail As New CompanyDetail
        Dim ds As New dsEmpCAAuthorization
        Dim mrptImage As rptImage
        Dim mEmployeeImage As EmployeeImage = Nothing

        obj = EmpCAAuthorization.GetEmpCAAuthorization(mEmpCAAuthorization.ID)
        mCAAuthorizationScopeList = CAAuthorizationScopeList.GetCAAuthorizationScope()
        mCALimitationList = CALimitationList.GetCALimitation()
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim mReport As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax,
                                          mCompanyDetail.Email, WebSite:="", ReportName:="", SearchStr1:=New SmartDate(txtFromDate.Text).FormattedText,
                                          SearchStr2:=New SmartDate(txtToDate.Text).FormattedText,
                                          SearchStr3:=mModuleList.Item("EmpCAAuthorization").FormRevisionNo,
                                          SearchStr4:=mModuleList.Item("EmpCAAuthorization").FormRevisionDate, SearchStr5:=AppSettings("Government Authority"), ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"),
                                          SearchStr6:="", SearchStr7:="", SearchStr8:="", SearchStr9:=AppSettings("Logo"), SearchStr10:=AppSettings("ClientCode"), ApprovalNo:=mCompanyDetail.ApprovalNo)

        'Page 1 : First Page Emp CAAuthorization
        MyReport = New crptEmpCAAuthorization
        ds.Clear()
        mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
        mEmployeeImage = EmployeeImage.GetImage(ds, DataTableName:="EmployeeImage", ID:=mEmpCAAuthorization.ID.ToString)
        da.Fill(ds, "EmpCAAuthorization", obj)

        da.Fill(ds, "EmpCAAuthorizationDetails", obj.EmpCAAuthorizationDetails)
        da.Fill(ds, "EmpCAAuthorizationTerms", obj.EmpCAAuthorizationTerms)
        da.Fill(ds, mReport)
        da.Fill(ds, mrptImage)
        da.Fill(ds, mEmployeeImage)

        MyReport.SetDataSource(ds)
        Session("CrystalReport") = MyReport

        Dim PDFNo As Integer = 1
        Dim PDFNoChild As Integer = 1
        Dim tmp As Integer
        Dim a As New Random
        Dim pageCount As Integer = 0

        Dim pdfList As New System.Collections.ArrayList

        Dim MyFile1 = ""
        Dim myExportOption As CrystalDecisions.Shared.ExportOptions
        Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

        MyFile1 = "C:\Temp\" & "EmpCAAuthorization" & tmp & PDFNo.ToString & ".pdf"
        MyReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

        myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
        myDiskOption.DiskFileName = MyFile1
        myExportOption = MyReport.ExportOptions
        With myExportOption
            .DestinationOptions = myDiskOption
            .ExportDestinationType = ExportDestinationType.DiskFile
            .ExportFormatType = ExportFormatType.PortableDocFormat
        End With
        MyReport.Export()
        MyReport.Close()
        MyReport.Dispose()
        GC.Collect()
        pdfList.Add(MyFile1)
        PDFNo = PDFNo + 1
        'Page 1 End

        'Page 2 : Second Page Authorization Scope
        MyReport = New crptScopeOfAuthorization
        ds.Clear()
        mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
        da.Fill(ds, "CAAuthorizationScopeList", mCAAuthorizationScopeList)
        da.Fill(ds, "CALimitationList", mCALimitationList)
        da.Fill(ds, mReport)
        da.Fill(ds, mrptImage)

        MyReport.SetDataSource(ds)

        Session("CrystalReport") = MyReport

        MyFile1 = "C:\Temp\" & "CAAuthorizationScope" & tmp & PDFNo.ToString & ".pdf"
        MyReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

        myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
        myDiskOption.DiskFileName = MyFile1
        myExportOption = MyReport.ExportOptions
        With myExportOption
            .DestinationOptions = myDiskOption
            .ExportDestinationType = ExportDestinationType.DiskFile
            .ExportFormatType = ExportFormatType.PortableDocFormat
        End With
        MyReport.Export()
        MyReport.Close()
        MyReport.Dispose()
        GC.Collect()

        pdfList.Add(MyFile1)
        PDFNo = PDFNo + 1
        'Page 2 : End


        '''''END: Merge ALL reports
        Dim MergedPath As String = "C:\Temp\" & "temp_myMergedPdf.pdf"
        Dim MergedPath_WM As String = "C:\Temp\" & "ReliabilityReport.pdf"

        Dim filesByte As New List(Of Byte())()

        For Each file__1 As String In pdfList 'files
            filesByte.Add(File.ReadAllBytes(file__1))
        Next

        File.WriteAllBytes(MergedPath, Flypal.PDFMergers.MergeFiles(filesByte))
        AddWatermarkText(MergedPath, MergedPath_WM, "Page ", , , iTextSharp.text.BaseColor.BLACK, , 0.0, pageCount, ReportName:="EmpCAAuthorization")


        Session("CrystalReport") = MergedPath_WM
        Session("PrintReportWithAttachment") = "True"

        Dim DeleteThis As String = "Authorization"
        Dim Files As String() = Directory.GetFiles("C:\Temp\")

        For Each file__1 As String In Files
            If file__1.ToUpper().Contains(DeleteThis.ToUpper()) Then
                File.Delete(file__1)
            End If
        Next
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        MarkLog(Util.Action.Close, "EmpCAAuthorization", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

        SetObject()
        setObjectAuthorizationDetail()
        Session("IsValid") = IsValid
        If mEmpCAAuthorization.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.CloseConfirm, "", MsgBoxStyle.YesNo, "Close")
            If IsValid Then
                SetObject()
                setObjectAuthorizationDetail()
            End If
        Else
            RemoveSessions()
            mEmployeeList = Nothing
            mEmpCAAuthorization = Nothing
            Session.Remove("IsEmpCAAuthorizationForRenew")
            Response.Redirect("Index.aspx")
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        'SetObject()  '''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'If IsValid Then

        If Session("IsEmpCAAuthorizationForRenew") = "True" Then
            'Do nothing
        Else
            If mEmpCAAuthorizationList.Contains(mEmpCAAuthorization.ID, New Guid(cmbEmployee.SelectedValue)) Then
                'MSGBoxCtrl.show("Alert..!!", "Employee Authorization already added for " + cmbEmployee.SelectedItem.ToString, MsgBoxStyle.YesNo, "")
                MSGBoxCtrl.Show("Alert!", "Employee Authorization already added for " + cmbEmployee.SelectedItem.ToString, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        End If

        If CustomValidateAuthorization() = False Then upnlValidationsummary.Update() : Exit Sub

        If IsValid = True And CustomValidate1() = True Then
            Save()
        Else
            upnlValidationsummary.Update()

        End If
    End Sub
    'Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    SetReport()
    '    Dim Str1 As String
    '    Str1 = "openTranDetail();"
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
    'End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    Private Sub GetAttachment()
        If mEmpCAAuthorization.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mEmpCAAuthorization.ID)
            Session("mFileAttachEmpCAAuthorization") = mFileAttach
        End If
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
                If (Not mEmpCAAuthorization.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mEmpCAAuthorization.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If
    End Sub
    Private Sub ViewImage()
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        GetAttachment()
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
    Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        If (Not IsInRole(Rights.Authorized) And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        GetAttachment()
        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0
        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        IsAttachmentDeleted = True
        mEmpCAAuthorization.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
        ControlVisibility()
        ControlVisibilityForFileAttachment()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        If Session("DetailAttachement") = "False" Then
            mEmpCAAuthorization.IsAttachmentAdded = True
            Session("mFileAttachEmpCAAuthorization") = Session("mFileAttach")
            Session("mFileAttach") = Nothing
            Session.Remove("mFileAttach")
            ControlVisibilityForFileAttachment()
            upnlAttachFile.Update()
        ElseIf Session("DetailAttachement") = "True" Then

            AttachFileuthorizationDetails()

            dgEmpCAAuthorizationDetail.DataSource = mEmpCAAuthorization.EmpCAAuthorizationDetails
            dgEmpCAAuthorizationDetail.DataBind()
            SetGrid()
            SetControl()
            ControlVisibility()
            upnlEmpCAAuthorizationDetail.Update()
        End If
        Session.Remove("DetailAttachement")


    End Sub
    Private Sub AttachFileuthorizationDetails()
        Dim BackupPath As String = ""
        BackupPath = AppSettings("DOCPath") & "New.PDF"
        If Not Session("Extension") = ".pdf" Then
            MSGBoxCtrl.Show("Alert!", "Please attach in PDF only!", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Try
            mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.IsAttachmentAdded = True
            If Not mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.FileAttachments.Contains(mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.ID, CType(Session("FileUpload.FileName"), String)) Then
                mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.FileAttachments.Add(mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.ID, CType(Session("FileUpload.FileName"), String)) 'Added By Vikrant On 17-Apr-2013 For ALL17042013
                ' mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.FileAttachments.CurrentItem.FileName = mFileAttach.FileName
                mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.FileAttachments.CurrentItem.ImageFile = CType(Session("ImageFile"), Byte())
                mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.FileAttachments.CurrentItem.Size = Session("Size")
                mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.FileAttachments.CurrentItem.Extension = Session("Extension")

                Session("mEmpCAAuthorization") = mEmpCAAuthorization
                Session.Remove("Size")
                Session.Remove("ImageFile")
                Session.Remove("Extension")
                Session.Remove("FileUpload.FileName")

            Else
                Session("mEmpCAAuthorization") = mEmpCAAuthorization
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
            End If
        Catch ex As Exception
        Finally
        End Try
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If (Not IsInRole(Rights.Authorized) And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        If mEmpCAAuthorization.IsAttachmentAdded = True Then
            mFileAttach = FileAttach.GetAttachment(mEmpCAAuthorization.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.Empty, mEmpCAAuthorization.ID)
        End If

        Session("mFileAttach") = mFileAttach
        Session("DetailAttachement") = "False"
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow()", True)
    End Sub

    Private Sub cmbEmployee_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbEmployee.SelectedIndexChanged
        If cmbEmployee.SelectedIndex = 0 Then
            mEmpCAAuthorization.EmployeeCode = ""
            txtEmployeeCode.Text = ""
            mEmpCAAuthorization.AMELNo = ""
            txtAMELNo.Text = ""
        Else

            If mEmpCAAuthorizationList.Contains(mEmpCAAuthorization.ID, New Guid(cmbEmployee.SelectedValue)) Then
                'MSGBoxCtrl.show("Alert..!!", "Employee Authorization already added for " + cmbEmployee.SelectedItem.ToString, MsgBoxStyle.YesNo, "")
                MSGBoxCtrl.Show("Alert!", "Employee Authorization already added for " + cmbEmployee.SelectedItem.ToString, "", MsgBoxStyle.OkOnly, "")
                cmbEmployee.ClearSelection()
                upnlEmpCAAuthorizationDetails.Update()
                Exit Sub
            End If
            mEmpCAAuthorization.EmployeeCode = mEmployeeList(New Guid(cmbEmployee.SelectedValue)).EmpNo
            txtEmployeeCode.Text = mEmpCAAuthorization.EmployeeCode
            txtEmployeeCode.DataBind()
            mEmpCAAuthorization.AMELNo = mEmployeeList(New Guid(cmbEmployee.SelectedValue)).LicenseNo
            txtAMELNo.Text = mEmpCAAuthorization.AMELNo
            txtAMELNo.DataBind()
            mEmpCAAuthorization.AMELCat = Employee.GetEmployee(New Guid(cmbEmployee.SelectedValue)).CAT 'mEmployeeList(New Guid(cmbEmployee.SelectedValue)).ca
            txtAMELCat.Text = mEmpCAAuthorization.AMELCat
            txtAMELCat.DataBind()
        End If
    End Sub

    Private Sub imgTermsAdd_Click(sender As Object, e As ImageClickEventArgs) Handles imgTermsAdd.Click
        If IsValid Then
            SetObject()
            'mEmpCAAuthorization.EmpCAAuthorizationDetails.Add(mEmpCAAuthorization.ID)
            Session("mEmpCAAuthorization") = mEmpCAAuthorization
            Session("EditEmpCAAuthorizationDetail") = False
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTermWindow", "OpenTermWindow()", True)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub dgEmpCAAuthorizationTerms_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgEmpCAAuthorizationTerms.RowCommand
        Select Case e.CommandName
            Case "DeleteRecord"
                Dim Index As Integer = CInt(e.CommandArgument) '+ dgEmpCAAuthorizationTerms.PageIndex * dgEmpCAAuthorizationTerms.PageSize
                DeleteCATerms(Index)
        End Select
    End Sub
    Private Sub hdnBtnEmpCAAuthorizationDetail_Click(sender As Object, e As EventArgs) Handles hdnBtnEmpCAAuthorizationDetail.Click, hdnBtnAuthorizationDetailsLimitation.Click
        mEmpCAAuthorization = EmpCAAuthorization.GetEmpCAAuthorization(mEmpCAAuthorization.ID)
        Session("mEmpCAAuthorization") = mEmpCAAuthorization
        'dgEmpCAAuthorizationDetail.DataSource = mEmpCAAuthorization.EmpCAAuthorizationDetails
        'dgEmpCAAuthorizationDetail.DataBind()
        DataFieldBind()
        SetGrid()
        SetControl()
        ControlVisibility()
        upnlEmpCAAuthorizationDetail.Update()
        upnlEmpCAAuthorizationDetails.Update()
    End Sub
    Private Sub hdnBtnAuthorizationDetailsScope_Click(sender As Object, e As EventArgs) Handles hdnBtnAuthorizationDetailsScope.Click
        mEmpCAAuthorization = EmpCAAuthorization.GetEmpCAAuthorization(mEmpCAAuthorization.ID)
        Session("mEmpCAAuthorization") = mEmpCAAuthorization
        'dgEmpCAAuthorizationDetail.DataSource = mEmpCAAuthorization.EmpCAAuthorizationDetails
        'dgEmpCAAuthorizationDetail.DataBind()
        DataFieldBind()
        SetGrid()
        SetControl()
        ControlVisibility()
        upnlEmpCAAuthorizationDetail.Update()
        upnlEmpCAAuthorizationDetails.Update()
    End Sub
    Private Sub hdnimgBtnCAAuthorizationTerm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnCAAuthorizationTerm.Click
        DataFieldBind()
        Session("mEmpCAAuthorization") = mEmpCAAuthorization
        SetGrid()
        SetControl()
        ControlVisibility()
        upnlEmpCAAuthorizationDetail.Update()
        upnlEmpCAAuthorizationTerms.Update()
    End Sub
#End Region

#Region " Status "
    Private Sub btnAuthorized_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuthorized.Click
        'If IsValid Then
        If CustomValidateAuthorization() = False Then upnlValidationsummary.Update() : Exit Sub
        If IsValid = True And CustomValidate1() = True Then
            MSGBoxCtrl.show(MSGBox.Message_title.StatusAuthorized, MSGBox.Message_text.StatusAuthorized, "<Strong> CA Authorization </Strong>", MsgBoxStyle.YesNo, "Status")
            Session("IsValid") = IsValid
            Session("mEmpCAAuthorization") = mEmpCAAuthorization
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
#End Region

#Region " Show BrokenRules "
    Public Function CustomValidate1() As Boolean
        Dim strMsg As String = ""
        SetObject()
        setObjectAuthorizationDetail()

        If mEmpCAAuthorization.IsValid = False Then
            For i As Integer = 0 To mEmpCAAuthorization.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mEmpCAAuthorization.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        Dim mEmpCAAuthorizationDetail As EmpCAAuthorizationDetail
        If mEmpCAAuthorization.EmpCAAuthorizationDetails.IsValid = False Then
            For Each mEmpCAAuthorizationDetail In mEmpCAAuthorization.EmpCAAuthorizationDetails
                For i As Integer = 0 To mEmpCAAuthorizationDetail.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mEmpCAAuthorizationDetail.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
            Next
        End If
        If strMsg.Trim <> "" Then
            CustValidator.ErrorMessage = strMsg
            CustValidator.IsValid = False
            Return False
        End If
        Return True
    End Function




#End Region

End Class