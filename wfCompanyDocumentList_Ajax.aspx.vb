'Added by Vikrant On 12-Oct-2021 For ALL12102021
Public Class wfCompanyDocumentList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    'Public mCompany As Company
    Public mDocumentList As DocumentList
    Public mCompanyDocument As CompanyDocument
    Public mCompanyDocumentList As CompanyDocumentList
    Public mCompanyDocumentHistoryList As CompanyDocumentHistoryList
    Dim EventLogID As Guid
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        'mCompany = CType(Session("mCompany"), Company)
        mCompanyDocumentList = Session("mCompanyDocumentList")
    End Sub
    Private Sub SetSession()
        'Session("mCompany") = mCompany
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCompanyList")
        Session.Remove("Text")
        Session.Remove("Index")
    End Sub
    Private Sub NewDocumentRecord()
        mCompanyDocument = CompanyDocument.NewCompanyDocument
        Session("mCompanyDocument") = mCompanyDocument
    End Sub
    Private Sub EditDocumentRecord(ByVal mID As Guid)
        mCompanyDocument = CompanyDocument.GetCompanyDocument(mID)
        Session("mCompanyDocument") = mCompanyDocument
    End Sub
    Private Sub DeleteDocumentRecord(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteDocument")
        mCompanyDocument = CompanyDocument.GetCompanyDocument(mID)
        Session("mCompanyDocument") = mCompanyDocument
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0

        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "DeleteDocument" Then
                        Try
                            Session("sender") = ""
                            mCompanyDocument = Session("mCompanyDocument")
                            CompanyDocument.DeleteCompanyDocument(mCompanyDocument.ID)
                            DataFieldBind()
                            'SetGrid()
                            ControlEnability()
                            upnlGrid.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MarkLog(Flypal.Util.Action.Delete, "CompanyDocument", "Can't delete : " + "Doc No. : " + mCompanyDocument.DocNo + " Document : " + mCompanyDocument.DocumentName + " is Currently in use", Flypal.Util.ErrorType.NoError, mCompanyDocument.ID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Flypal.Util.Action.Delete, "CompanyDocument", "Doc No. : " + mCompanyDocument.DocNo + " Document : " + mCompanyDocument.DocumentName, Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = ""
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
        End If
    End Sub
    Private Sub SetGrid()
        'Dim s As Integer   'Document
        'Dim lnkDocumentView As LinkButton 'ButtonColumn 
        'Dim lnkDocumentHistory As LinkButton
        'Dim DocumentHistoryCount As Boolean
        'For m As Integer = 0 To dgDocumentList.Rows.Count - 1
        '    s = CType(Me.dgDocumentList.Rows.Item(m).Cells(15).Text, Integer)
        '    DocumentHistoryCount = CType(Me.dgDocumentList.Rows.Item(m).Cells(17).Text, Boolean)
        '    If s <= 0 Then
        '        lnkDocumentView = CType(dgDocumentList.Rows.Item(m).Cells(14).FindControl("lnkDocumentView"), LinkButton)
        '        lnkDocumentView.Enabled = False
        '    End If
        '    If DocumentHistoryCount = False Then
        '        lnkDocumentHistory = CType(dgDocumentList.Rows.Item(m).Cells(16).FindControl("lnkDocumentHistory"), LinkButton)
        '        lnkDocumentHistory.Enabled = False
        '    End If
        'Next
    End Sub
    Private Sub ControlEnability()
        If mCompanyDocumentList.Count > 15 Then
            btnAdd.Visible = True
            btnBack.Visible = True
            btnPrint.Visible = True
        Else
            btnAdd.Visible = False
            btnBack.Visible = False
            btnPrint.Visible = False
        End If
        If mCompanyDocumentList.Count > 0 Then
            btnPrint.Enabled = True
            btnPrintTop.Enabled = True
        Else
            btnPrint.Enabled = False
            btnPrintTop.Enabled = False
        End If
    End Sub
#End Region

#Region " Data Binding "
    Public Sub DataFieldBind()
        mDocumentList = DocumentList.GetDocumentList("", "(ALL)")
        cmbDocumentList.DataSource = mDocumentList
        'DOCUMENT LIST
        'mCompanyDocumentList = CompanyDocumentList.GetCompanyDocumentList(mCompany.ID)
        mCompanyDocumentList = CompanyDocumentList.GetCompanyDocumentList(Guid.Empty, DocumentOrContractID:=1)
        dgDocumentList.DataSource = mCompanyDocumentList
        Session("mCompanyDocumentList") = mCompanyDocumentList
        lblDocs.Text = "List of Organisation Approval : " & mCompanyDocumentList.Count.ToString & " Record(s) found."
        DataBind() 'CHK Bind TextBox Individually
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            DataFieldBind()
            'SetGrid()
            ControlEnability()
        End If
    End Sub
    Private Sub btnDocumentAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click, btnAddTop.Click
        'If User.IsInRole("CompanyDocumentsNew") = False Then
        '    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
        '    Exit Sub
        'End If
        NewDocumentRecord()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenCompanyDocumentWindow", "OpenCompanyDocumentWindow();", True)
    End Sub
    Private Sub dgDocumentList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDocumentList.RowCommand
        Dim Idx As Int32
        Dim mID As New Guid
        Select Case e.CommandName
            Case "EditRec"
                'Idx = CInt(e.CommandArgument) + dgDocumentList.PageIndex * dgDocumentList.PageSize
                'mID = New Guid(dgDocumentList.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)
                mID = New Guid(e.CommandArgument.ToString)
                'If User.IsInRole("CompanyDocumentsEdit") = False Then
                '    'MarkLog(Util.Action.Edit, "CompanyDocument", User.Identity.Name & " is not Authorized User to edit " + mCompany.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                '    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                '    Exit Sub
                'End If
                EditDocumentRecord(mID)
                Session("IsRenew") = False
                MarkLog(Flypal.Util.Action.Edit, "CompanyDocument", "Doc No. : " + mCompanyDocument.DocNo + " Document : " + mCompanyDocument.DocumentName, Flypal.Util.ErrorType.NoError, mCompanyDocument.ID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenCompanyDocumentWindow", "OpenCompanyDocumentWindow();", True)
            Case "DeleteRec"
                'Idx = CInt(e.CommandArgument) + dgDocumentList.PageIndex * dgDocumentList.PageSize
                'mID = New Guid(dgDocumentList.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)
                mID = New Guid(e.CommandArgument.ToString)
                'If User.IsInRole("CompanyDocumentsDelete") = False Then
                '    'MarkLog(Util.Action.Delete, "CompanyDocument", User.Identity.Name & " is not Authorized User to edit " + mCompany.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                '    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                '    Exit Sub
                'End If
                DeleteDocumentRecord(mID)
            Case "View"
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim rowIndex As Integer = gvr.RowIndex
                Idx = rowIndex + dgDocumentList.PageIndex * dgDocumentList.PageSize
                mID = New Guid(dgDocumentList.DataKeys(rowIndex).Values("ID").ToString)

                mCompanyDocument = CompanyDocument.GetCompanyDocument(mID)
                If mCompanyDocument.ImageSize > 0 Then
                    Dim path As String = AppSettings("DOCPath") & StrName & mCompanyDocument.FileExtension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mCompanyDocument.FileExtension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mCompanyDocument.ImageFile, 0, mCompanyDocument.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
                    End If
                Else
                    MSGBoxCtrl.show("Attachment!", "No Attach File Present.", "", MsgBoxStyle.OkOnly, "")
                End If
            Case "Renew"
                'If User.IsInRole("CompanyDocumentsEdit") = False Then
                '    'MarkLog(Util.Action.Edit, "CompanyDocument", User.Identity.Name & " is not Authorized User to edit " + mCompany.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                '    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                '    Exit Sub
                'End If
                'Idx = CInt(e.CommandArgument) + dgDocumentList.PageIndex * dgDocumentList.PageSize
                'mID = New Guid(dgDocumentList.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)
                mID = New Guid(e.CommandArgument.ToString)
                mCompanyDocument = CompanyDocument.GetCompanyDocument(mID)
                SetSession()
                mCompanyDocument = CompanyDocument.NewRenew(mCompanyDocument, True)
                Session("IsRenew") = True
                Session("mCompanyDocument") = mCompanyDocument
                Session.Remove("mFileAttach")
                MarkLog(Flypal.Util.Action.Comply, "CompanyDocument", "Doc No. : " + mCompanyDocument.DocNo + " Document : " + mCompanyDocument.DocumentName, Flypal.Util.ErrorType.NoError, mCompanyDocument.ID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenCompanyDocumentWindow", "OpenCompanyDocumentWindow();", True)
            Case "History"
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim rowIndex As Integer = gvr.RowIndex
                'Idx = rowIndex + dgDocumentList.PageIndex * dgDocumentList.PageSize
                'mID = New Guid(dgDocumentList.DataKeys(rowIndex).Values("ID").ToString)
                mID = New Guid(e.CommandArgument.ToString)
                mCompanyDocument = CompanyDocument.GetCompanyDocument(mID)
                Session("mCompanyDocument") = mCompanyDocument
                Dim mVendorID As Guid = New Guid(dgDocumentList.DataKeys(rowIndex).Values("VendorID").ToString)
                Session("mVendorID") = mVendorID.ToString
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenCompanyDocumentHistoryWindow", "OpenCompanyDocumentHistoryWindow();", True)
        End Select
    End Sub
    Private Sub dgDocumentList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgDocumentList.Sorting
        mCompanyDocumentList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgDocumentList.DataSource = mCompanyDocumentList
        Session("mCompanyDocumentList") = mCompanyDocumentList
        dgDocumentList.DataBind()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnCompanyDocument_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnCompanyDocument.Click
        DataFieldBind()
        'SetGrid()
        ControlEnability()
        upnlGrid.Update()
    End Sub
    Private Sub btnPrintTop_Click(sender As Object, e As System.EventArgs) Handles btnPrintTop.Click, btnPrint.Click
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        'Dim rpt As rptPendingWorkOrderforDelivery
        Dim ds As New dsCompanyDocument
        myReport = New crptCompanyDocument
        mCompanyDocumentList = CompanyDocumentList.GetCompanyDocumentList(Guid.Empty, DocumentOrContractID:=1, DocumentID:=cmbDocumentList.SelectedValue)
        If mCompanyDocumentList.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
            mCompanyDetail.WebSite, ReportName:="", SearchStr1:=IIf(cmbDocumentList.SelectedIndex = 0, "", cmbDocumentList.SelectedItem.Text), _
            SearchStr2:="", SearchStr3:="", SearchStr4:="", SearchStr5:="", ProductVersion:="", SINote:="", SearchStr6:="", SearchStr10:=AppSettings("Logo"), _
            SearchStr11:=AppSettings("MROISONo"), SearchStr12:="TELEFAX:" & mCompanyDetail.Fax & " " & mCompanyDetail.Email & ",  www.amanaviation.in", _
            SearchStr13:="", SearchStr14:="")

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, "rptImage", mrptImage)
        da.Fill(ds, "ReportData", Report)
        da.Fill(ds, "CompanyDocumentList", mCompanyDocumentList)

        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub btnFindNow_Click(sender As Object, e As System.EventArgs) Handles btnFindNow.Click
        mCompanyDocumentList = CompanyDocumentList.GetCompanyDocumentList(Guid.Empty, "", "", DocumentOrContractID:=1, _
                                                                          DocumentID:=cmbDocumentList.SelectedValue)
        dgDocumentList.DataSource = mCompanyDocumentList
        dgDocumentList.DataBind()
        Session("mCompanyDocumentList") = mCompanyDocumentList
        lblDocs.Text = "List of Organisation Approval : " & mCompanyDocumentList.Count.ToString & " Record(s) found."
        ControlEnability()
        upnlGrid.Update()
    End Sub
#End Region

End Class