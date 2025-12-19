Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Public Class wfManual_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mManual As Manual
    Dim mRevision As Revision
    Dim EventLogID As Guid
    Dim mFileAttach As FileAttach
    Private checkedIds As New List(Of String)()
    Dim mNoRevNo As String = ""
    Dim mRemarkNote As String = ""
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mManual = Session("mManual")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mManual")
    End Sub
    Public Sub SetTitle()
        If mManual.IsNew Then
            lblTitle.Text = "Manual [New]"
        Else
            lblTitle.Text = "Manual Detail " & "[" & mManual.Name & "]"
        End If
        upnlTitle.Update()
    End Sub
    Private Sub setObject()
        mManual.Name = txtName.Text.Trim
        mManual.ApplicableFor = txtApplicableFor.Text.Trim
        mManual.Note = txtNote.Text.Trim
        mManual.ShortDesc = txtDescription.Text.Trim
        mManual.MCategoryID = New Guid(cmbCategoryList.SelectedValue)
        mManual.IsInUse = chkIsInUse.Checked
        mManual.Validity = chkValidity.Checked 'Added by Saylee on 10-Nov-2009
        mManual.ManualNo = Trim(txtManualNo.Text) 'Added By Vikrant On 26-Nov-2018 For APFT26112018
        Session("mManual") = mManual
    End Sub
    Private Sub DeleteRevision(ByVal Index As Int32)
        'MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "DeleteRevision")
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteRevision")
        mManual.Revisions.CurrentIndex = Index
        Session("mManual") = mManual
    End Sub
    Private Sub DeleteProperty(ByVal Index As Int32)
        'MSGBoxCtrl.show(MSGBox.Message_title.Remove, MSGBox.Message_text.Remove, "", MsgBoxStyle.YesNo, "DeleteProperty")
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteProperty")
        mManual.ManualPropertyValues.CurrentIndex = Index
        Session("mManual") = mManual
    End Sub
    Private Sub DeleteSubscriber(ByVal Index As Int32) 'Added By Vikrant On 19-Mar-2014 For ALL19032014
        'MSGBoxCtrl.show(MSGBox.Message_title.Remove, MSGBox.Message_text.Remove, "", MsgBoxStyle.YesNo, "DeleteSubscriber")
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteSubscriber")
        mManual.ManualSubscribers.CurrentIndex = Index
        Session("mManual") = mManual
    End Sub 'End
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "DeleteRevision" Then
                        Try
                            mManual.Revisions.Remove(mManual.Revisions.CurrentItem)
                            dgRevisions.DataSource = mManual.Revisions
                            dgRevisions.DataBind()
                            SetGrid()
                            upnlRevisions.Update()
                            Session("mManual") = mManual
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "DeleteProperty" Then
                        Try
                            mManual.ManualPropertyValues.Remove(mManual.ManualPropertyValues.CurrentItem)
                            dgManualPropertyValues.DataSource = mManual.ManualPropertyValues
                            dgManualPropertyValues.DataBind()
                            upnlPropertyValue.Update()
                            Session("mManual") = mManual
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "DeleteSubscriber" Then
                        Try
                            mManual.ManualSubscribers.Remove(mManual.ManualSubscribers.CurrentIndex)
                            dgSubscriberList.DataSource = mManual.ManualSubscribers
                            dgSubscriberList.DataBind()
                            btnSendNotification.DataBind()
                            upnlButtons.Update()
                            upnlSubscriber.Update()
                            Session("mManual") = mManual
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "Close" Then
                        If (Not User.IsInRole("ManualNew")) And (Not User.IsInRole("ManualEdit")) Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                            Exit Sub
                        End If
                        setObject()
                        Try
                            If mManual.IsSavable Then
                                If mManual.Revisions.Count = 0 Then
                                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Manual can not save without Revision", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                Else
                                    mManual.ApplyEdit()
                                    mManual = mManual.Save
                                    Session("mManual") = mManual
                                    DataFieldBind()
                                    SetGrid()
                                    btnPrint.Enabled = True
                                    SetTitle()
                                    UpdatePanel()
                                    Dim mManualDetail As String = "Name: " + mManual.Name + " Category: " + mManual.MCategoryName + IIf(txtManualNo.Text.Trim <> "", " Manual No.: " + Trim(txtManualNo.Text), "")
                                    MarkLog(Util.Action.Save, "Manual", mManualDetail, Util.ErrorType.HandledError, mManual.ID, EventLogID)
                                    Response.Redirect("Index.aspx")
                                End If
                            Else
                                cvControlValidator.ErrorMessage = mManual.GetBrokenRulesString
                                cvControlValidator.IsValid = mManual.IsSavable
                                upnlValidationsummary.Update()
                            End If
                        Catch ex As Exception
                            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "You can not add duplicate entry in Manual.", MsgBoxStyle.OkOnly, "")
                            btnPrint.Enabled = False
                            upnlButtons.Update()
                            Exit Sub
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Response.Redirect("Index.aspx")
                    End If
            End Select
        End If
    End Sub
    Private Sub UpdatePanel()
        upnlTitle.Update()
        upnlManualDetails.DataBind()
        upnlManualDetails.Update()
        upnlRevisions.Update()
        upnlPropertyValue.Update()
        upnlSubscriber.Update()
        upnlButtons.DataBind()
        upnlButtons.Update()
        upnlMManualSubscription.DataBind()
        upnlMManualSubscription.Update()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        cmbCategoryList.DataSource = MCategoryList.GetMCategoryList(, "<SELECT>")
        dgManualPropertyValues.DataSource = mManual.ManualPropertyValues
        dgRevisions.DataSource = mManual.Revisions
        dgSubscriberList.DataSource = mManual.ManualSubscribers
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
            dgRevisions.Columns(3).HeaderText = "Subscription No."
            dgRevisions.Columns(6).HeaderText = "Expiry Date"
        Else
            dgRevisions.Columns(3).HeaderText = "Revision No."
            dgRevisions.Columns(6).HeaderText = "Next Revision Date"
        End If
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
            lblRevisions.Text = "Manual Subscription(s)"
        Else
            lblRevisions.Text = "Manual Revision(s)"
        End If
        DataBind()
    End Sub
    Private Sub SetGrid()
        Dim P As Boolean
        For j As Integer = 0 To dgRevisions.Rows.Count - 1
            P = CType(Me.dgRevisions.Rows(j).Cells(14).Text, Boolean)
            If P = False Then
                dgRevisions.Rows(j).Cells(13).Enabled = False
            End If
        Next
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            txtName.Focus()
            DataFieldBind()
            SetTitle()
            SetGrid()
        End If
    End Sub
    Private Sub btnAddRevision_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnAddRevision.Click
        mManual.Revisions.Add(mManual.ID)
        Session("mManual") = mManual
        Session("EditRevisions") = False
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenManualRevisionWindow", "OpenManualRevisionWindow();", True)
    End Sub
    Private Sub dgRevisions_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRevisions.RowCommand
        Select Case e.CommandName
            Case "EditView"
                Dim Index As Integer = CInt(e.CommandArgument) + dgRevisions.PageIndex * dgRevisions.PageSize
                Session("EditRevisions") = True
                mManual.Revisions.CurrentIndex = Index
                Session("mManual") = mManual
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenManualRevisionWindow", "OpenManualRevisionWindow();", True)
            Case "DeleteRecord"
                Dim Index As Integer = CInt(e.CommandArgument) + dgRevisions.PageIndex * dgRevisions.PageSize
                DeleteRevision(Index)
            Case "ViewRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgRevisions.PageIndex * dgRevisions.PageSize
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------
                mFileAttach = FileAttach.GetAttachment(mManual.Revisions(Index).ID)
                Session("mFileAttach") = mFileAttach
                If mFileAttach.Size > 0 Then
                    Dim path As String = AppSettings("DOCPath") & StrName & mFileAttach.Extension
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
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                    End If
                Else
                End If
        End Select
    End Sub
    Private Sub btnAddPropertyValue_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnAddPropertyValue.Click
        mManual.ManualPropertyValues.Add(mManual.ID)
        Session("mManual") = mManual
        Session("EditPropertyValues") = False
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPropertyValueWindow", "OpenPropertyValueWindow();", True)
    End Sub
    Private Sub dgManualPropertyValues_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgManualPropertyValues.RowCommand
        Select Case e.CommandName
            Case "EditView"
                Dim Index As Integer = CInt(e.CommandArgument) + dgManualPropertyValues.PageIndex * dgManualPropertyValues.PageSize
                Session("EditPropertyValues") = True
                mManual.ManualPropertyValues.CurrentIndex = Index
                Session("mManual") = mManual
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPropertyValueWindow", "OpenPropertyValueWindow();", True)
            Case "DeleteRecord"
                Dim Index As Integer = CInt(e.CommandArgument) + dgManualPropertyValues.PageIndex * dgManualPropertyValues.PageSize
                DeleteProperty(Index)
        End Select
    End Sub
    Private Sub btnAddCategory_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnAddCategory.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenManualCategoryWindow", "OpenManualCategoryWindow();", True)
    End Sub
    Private Sub btnAddSubscriber_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnAddSubscriber.Click
        mManual.ManualSubscribers.Add(mManual.ID)
        Session("mManual") = mManual
        Session("EditSubscriber") = False
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSubscriberWindow", "OpenSubscriberWindow();", True)
    End Sub
    Private Sub dgSubscriberList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSubscriberList.RowCommand
        Select Case e.CommandName
            Case "EditView"
                Dim Index As Integer = CInt(e.CommandArgument) + dgSubscriberList.PageIndex * dgSubscriberList.PageSize
                Session("EditSubscriber") = True
                mManual.ManualSubscribers.CurrentIndex = Index
                Session("mManual") = mManual
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSubscriberWindow", "OpenSubscriberWindow();", True)
            Case "DeleteRecord"
                Dim Index As Integer = CInt(e.CommandArgument) + dgSubscriberList.PageIndex * dgSubscriberList.PageSize
                DeleteSubscriber(Index)
        End Select
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("ManualNew")) And (Not User.IsInRole("ManualEdit")) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        setObject()
        If IsValid = False Then upnlValidationsummary.Update() : Exit Sub
        Try
            If mManual.IsSavable Then
                If mManual.Revisions.Count = 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Manual can not save without Revision", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    mManual.ApplyEdit()
                    mManual = mManual.Save
                    mManual = Manual.GetManual(mManual.ID)
                    Session("mManual") = mManual
                    DataFieldBind()
                    SetGrid()
                    btnPrint.Enabled = True
                    SetTitle()
                    UpdatePanel()
                    Dim mManualDetail As String = "Name: " + mManual.Name + ", Category: " + mManual.MCategoryName + IIf(txtManualNo.Text.Trim <> "", ", Manual No.: " + Trim(txtManualNo.Text), "") + " saved successfully by " + User.Identity.Name
                    MarkLog(Util.Action.Save, "Manual", mManualDetail, Util.ErrorType.HandledError, mManual.ID, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                End If
            End If
        Catch ex As Exception
            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "You can not add duplicate entry in Manual.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End Try
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "Manual", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        setObject()
        If mManual.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.Save, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
        Else
            RemoveSession()
            Response.Redirect("Index.aspx")
        End If
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim mCompanyDetail As New Flypal.CompanyDetail
        Dim da As New CSLA10.Data.ObjectAdapter
        Dim ds As New dsManual
        Dim Obj As Manual
        Dim ObjRev As Revisions
        Dim ObjVal As ManualPropertyValues
        Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
            Rpt = New crManualForTAAL
        Else
            Rpt = New crManual
        End If
        Obj = Manual.GetManual(mManual.ID)
        ObjRev = Obj.Revisions
        ObjVal = Obj.ManualPropertyValues

        Dim Report As New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
              mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
              mCompanyDetail.WebSite, "Manual Detail Report", "lblManual.text", "mManualSelection", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
        Dim mrptImage As rptImage = rptImage.GetImage(ds) 'Added by Shweta on 27-Feb-2012
        da.Fill(ds, Obj)
        da.Fill(ds, ObjRev)
        da.Fill(ds, ObjVal)
        da.Fill(ds, mrptImage) 'Added by Shweta on 27-Feb-2012
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
    End Sub
    Private Sub btnSendNotification_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSendNotification.Click
        If (Not User.IsInRole("ManualNew")) And (Not User.IsInRole("ManualEdit")) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If

        Dim checkString = Request.Form("chkSelect")
        If checkString Is Nothing Then
            MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

       

        Dim str As String
        Dim mSendMailFile As New SendMailFile
        Dim ToMailIDs As New StringBuilder
        Dim SubScribers As New StringBuilder
        ' we'll need a split to get the individual ids
        Dim values = checkString.Split(","c)
        For Each value As String In values
            If mManual.ManualSubscribers.Contains(New Guid(value)) Then
                SubScribers.Append(mManual.ManualSubscribers(New Guid(value)).EmployeeName + "(" + mManual.ManualSubscribers(New Guid(value)).Email + ")" + ",")
                ToMailIDs.Append(mManual.ManualSubscribers(New Guid(value)).Email + ",")
            End If
        Next

        values = ""
        checkString = Nothing

        'For i As Integer = 0 To mManual.ManualSubscribers.Count - 1

        'Next

        str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Following Manual(s) has been Updated / Revised in FlyPal Manual System and need your attentions." + "</font></P></br> ")
        str = str + ("<font face=""Calibri"">Please Login to FlyPal® for detailed information." + "</font> ")

        str = str + ("<p><font face=""Calibri"">")
        str = str + ("<b>Manual Name: " + "</b>" + IIf(mManual.Name = "", "-", mManual.Name) + "<b> Manual No.:</b> " + IIf(mManual.ManualNo = "", "-", mManual.ManualNo) + "<b>" + " Description: " + "</b>" + IIf(mManual.ShortDesc = "", "-", mManual.ShortDesc) + "<b>" + " Category: " + "</b>" + IIf(mManual.MCategoryName = "", "-", mManual.MCategoryName))
        str = str + ("</font></p>")

        str = str + ("<p><font face=""Calibri"">")

        If mManual.Revisions(mManual.Revisions.Count - 1).No = "" And mManual.Revisions(mManual.Revisions.Count - 1).RevNo = "" Then
            mNoRevNo = ""
        ElseIf mManual.Revisions(mManual.Revisions.Count - 1).No <> "" And mManual.Revisions(mManual.Revisions.Count - 1).RevNo = "" Then
            mNoRevNo = "<b>Last Revision No.: " + "</b>" + mManual.Revisions(mManual.Revisions.Count - 1).No
        ElseIf mManual.Revisions(mManual.Revisions.Count - 1).No = "" And mManual.Revisions(mManual.Revisions.Count - 1).RevNo <> "" Then
            mNoRevNo = "<b>Last Revision No.: " + "</b>" + mManual.Revisions(mManual.Revisions.Count - 1).RevNo
        ElseIf mManual.Revisions(mManual.Revisions.Count - 1).No <> "" And mManual.Revisions(mManual.Revisions.Count - 1).RevNo <> "" Then
            mNoRevNo = "<b>Last Revision No.: " + "</b>" + mManual.Revisions(mManual.Revisions.Count - 1).No + "/ " + mManual.Revisions(mManual.Revisions.Count - 1).RevNo
        Else
            mNoRevNo = ""
        End If
        str = str + (mNoRevNo + "<b>" + " Effective Date: " + "</b>" + IIf(mManual.Revisions(mManual.Revisions.Count - 1).RevDate = "", "-", mManual.Revisions(mManual.Revisions.Count - 1).RevDate) + "<b>" + " Next Revision Date: " + "</b>" + IIf(mManual.Revisions(mManual.Revisions.Count - 1).EffectiveDate = "", "-", mManual.Revisions(mManual.Revisions.Count - 1).EffectiveDate))
        str = str + ("</font></p>")

        If mManual.Revisions(mManual.Revisions.Count - 1).Remark = "" And mManual.Revisions(mManual.Revisions.Count - 1).Note = "" Then
            mRemarkNote = ""
        ElseIf mManual.Revisions(mManual.Revisions.Count - 1).Remark <> "" And mManual.Revisions(mManual.Revisions.Count - 1).Note = "" Then
            mRemarkNote = "<b>Remark / Note: " + "</b>" + mManual.Revisions(mManual.Revisions.Count - 1).Remark
        ElseIf mManual.Revisions(mManual.Revisions.Count - 1).Remark = "" And mManual.Revisions(mManual.Revisions.Count - 1).Note <> "" Then
            mRemarkNote = "<b>Remark / Note: " + "</b>" + mManual.Revisions(mManual.Revisions.Count - 1).Note
        ElseIf mManual.Revisions(mManual.Revisions.Count - 1).Remark <> "" And mManual.Revisions(mManual.Revisions.Count - 1).Note <> "" Then
            mRemarkNote = "<b>Remark / Note: " + "</b>" + mManual.Revisions(mManual.Revisions.Count - 1).Remark + "/ " + mManual.Revisions(mManual.Revisions.Count - 1).Note
        Else
            mRemarkNote = ""
        End If

        str = str + ("<p><font face=""Calibri"">")
        str = str + mRemarkNote
        str = str + ("</font></p>")

        str = str + ("<p><font face=""Calibri"">")
        str = str + ("<b>Soft Copy Available: " + "</b>" + IIf(mManual.Revisions(mManual.Revisions.Count - 1).SoftCopy, "Yes", "No") + "<b>" + " Hard Copy Available: " + "</b>" + IIf(mManual.Revisions(mManual.Revisions.Count - 1).HardCopy, "Yes", "No"))
        str = str + ("</font></p>")

        str = str + ("</body></html>")

        SendMailFile.SendMailFile(, User.Identity.Name, "Manual Revision Notification", Info:=str, ToMailID:=ToMailIDs.ToString.Substring(0, ToMailIDs.Length - 1), Remark:="", ReportGeneratedBy:="")
        Dim mManualDetail As String = "Manual Revision Notification sent successfully to " + SubScribers.ToString.TrimEnd(",") + " by " + User.Identity.Name
        MarkLog(Util.Action.SendMail, "Manual", mManualDetail, Util.ErrorType.HandledError, mManual.Revisions(mManual.Revisions.Count - 1).ID, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTransDetail", MessageBox.Show("Mail Sent Successfully", False), True)

        mManual = Manual.GetManual(mManual.ID)
        Session("mManual") = mManual
        DataFieldBind()
        SetGrid()
        btnPrint.Enabled = True
        SetTitle()
        UpdatePanel()
    End Sub
    Private Sub hdnBtnManualCategory_Click(sender As Object, e As System.EventArgs) Handles hdnBtnManualCategory.Click
        cmbCategoryList.DataSource = MCategoryList.GetMCategoryList(, "<SELECT>")
        cmbCategoryList.DataBind()
        upnlManualDetails.Update()
    End Sub
    Private Sub hdnBtnManualRevision_Click(sender As Object, e As System.EventArgs) Handles hdnBtnManualRevision.Click
        dgRevisions.DataSource = mManual.Revisions
        dgRevisions.DataBind()
        SetGrid()
        upnlRevisions.Update()
    End Sub
    Private Sub hdnBtnPropertyValue_Click(sender As Object, e As System.EventArgs) Handles hdnBtnPropertyValue.Click
        dgManualPropertyValues.DataSource = mManual.ManualPropertyValues
        dgManualPropertyValues.DataBind()
        upnlPropertyValue.Update()
    End Sub
    Private Sub hdnBtnSubscriber_Click(sender As Object, e As System.EventArgs) Handles hdnBtnSubscriber.Click
        dgSubscriberList.DataSource = mManual.ManualSubscribers
        dgSubscriberList.DataBind()
        upnlButtons.DataBind()
        upnlButtons.Update()
        upnlSubscriber.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub btnRenew_Click(sender As Object, e As System.EventArgs) Handles btnRenew.Click
        'mManual.MManualSubscriptions.Add(mManual.ID)
        Session("mManual") = mManual
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenMManualSubscriptionWindow", "OpenMManualSubscriptionWindow();", True)
    End Sub
    Private Sub chkValidity_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkValidity.CheckedChanged
        If chkValidity.Checked = True Then
            btnRenew.Enabled = False
        Else
            btnRenew.Enabled = True
        End If
    End Sub
    Private Sub hdnMManualSubscription_Click(sender As Object, e As System.EventArgs) Handles hdnMManualSubscription.Click
        If mManual.MManualSubscriptions.Count > 0 Then

            'Dim q = (From n In mManual.MManualSubscriptions
            '            Select g.OrderByDescending(t >= t.Date).FirstOrDefault())


            Dim q = (From n In mManual.MManualSubscriptions Order By CDate(n.ToDate) Descending).FirstOrDefault()

            mManual.FromDate = q.FromDate 'mManual.MManualSubscriptions(mManual.MManualSubscriptions.Count - 1).FromDate
            mManual.ToDate = q.ToDate 'mManual.MManualSubscriptions(mManual.MManualSubscriptions.Count - 1).ToDate
            mManual.Validity = False
            upnlMManualSubscription.DataBind()
            upnlMManualSubscription.Update()
        Else
            mManual.FromDate = ""
            mManual.ToDate = ""
            upnlMManualSubscription.DataBind()
            upnlMManualSubscription.Update()
        End If
    End Sub
#End Region

#Region "Checked Selection"
    Public Function NumeroChequeInclus(ByVal numero As String) As String
        If (checkedIds.Contains(numero)) Then
            Return "checked"
        Else
            Return String.Empty
        End If
        Return String.Empty
    End Function
#End Region
End Class