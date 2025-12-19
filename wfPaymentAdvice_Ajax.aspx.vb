Imports System.Configuration.ConfigurationManager
Imports System.Collections.Generic
Imports System.IO
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports System.Web.UI.DataVisualization.Charting
Imports System.Drawing
Imports System.Linq
Imports System.Text
Public Class wfPaymentAdvice_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mPaymentAdvice As PaymentAdvice
    Public mPaymentAdviceList As PaymentAdviceList
    Public mSupplierList As VendorList
    Public mCurrencyList As CurrencyList
    Public Flag As Integer
    Dim mUser As User
    Public mModeOfPaymentList As TypeList
    Dim mFileAttach As FileAttach
    Dim mFileAttach1 As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Public rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

#Region " Enum "
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

#Region " Business Methods "
    Private Sub getSession()
        mPaymentAdvice = Session("mPaymentAdvice")
        mPaymentAdviceList = Session("mPaymentAdviceList")
        mModeOfPaymentList = Session("mModeOfPaymentList")
        mSupplierList = Session("mSupplierList")
        mCurrencyList = Session("mCurrencyList")
        mFileAttach = Session("mFileAttach")

        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        '  rpt = Session("CrystalReport")
    End Sub
    Private Sub setSession()
        Session("mPaymentAdvice") = mPaymentAdvice
        Session("mPaymentAdviceList") = mPaymentAdviceList
        Session("mSupplierList") = mSupplierList
        Session("mModeOfPaymentList") = mModeOfPaymentList
        Session("mCurrencyList") = mCurrencyList
        Session("mFileAttach") = mFileAttach

        Session("IsAttachmentDeleted") = IsAttachmentDeleted
        Session("CrystalReport") = rpt
    End Sub
    Private Sub RemoveSession()
        'Session.Remove("mPaymentAdvice")

        Session.Remove("mSupplierList")
        Session.Remove("mCurrencyList")
        Session.Remove("mModeOfPaymentList")
        Session.Remove("IsAttachmentDeleted")
        Session.Remove("mFileAttach")

    End Sub
    Private Sub SetControl()

    End Sub
    Private Sub addAttributes()
        txtChequeNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtChequeNo').value,event)")
    End Sub
    Private Sub ControlVisibility()
        dgPaymentItems.Columns(5).Visible = Not (mPaymentAdvice.StatusID = 2) And Not (mPaymentAdvice.IsPaymentDone)
        dgPaymentItems.Columns(6).Visible = Not (mPaymentAdvice.StatusID = 2) And Not (mPaymentAdvice.IsPaymentDone)
        btnAdd.Visible = IIf(Not mPaymentAdvice.StatusID = 2 And Not (mPaymentAdvice.CurrencyID.Equals(Guid.Empty)) And Not (mPaymentAdvice.VendorID.Equals(Guid.Empty)) And Not (mPaymentAdvice.IsPaymentDone), True, False)


        If Session("IsFromPendingPAPaymentPage") = True Then
            pnlPaymentDetails.Visible = True
            pnlPaymentDetails.Enabled = True
            pnlButtons.Visible = False
        Else
            If mPaymentAdvice.IsPaymentDone Then
                pnlPaymentDetails.Visible = True
                ' pnlPaymentDetails.Enabled = False

                ImageButton2.Enabled = IIf(mPaymentAdvice.IsPaymentFileAttachment = True, True, False)
                UpdatePanel4.Update()
                btnClose.Visible = True
            Else
                pnlPaymentDetails.Visible = False
                pnlButtons.Visible = True
            End If

        End If

        btnSelectFiles.Enabled = (mPaymentAdvice.StatusID <> 2)
       
        ControlVisibilityForAttachment()

        upnlPaymentDoneDetails.Update()
        upnlPaymentDetails.DataBind()
        upnlPaymentDetails.Update()
        UpdatePanel1.DataBind()
        'Added by vikrant on 08-Aug-2018 For ALL08082018
        Dim Txt As TextBox
        dgPAAttachment.Columns(6).Visible = Not (mPaymentAdvice.StatusID = 2) And Not (mPaymentAdvice.IsPaymentDone)
        For i As Integer = 0 To dgPAAttachment.Rows.Count - 1
            Txt = CType(dgPAAttachment.Rows(i).FindControl("txtFileName"), TextBox)
            Txt.Enabled = False
        Next
        'End
        UpdatePanel1.Update()
        upnlbtnAdd.Update()
        upnlbuttons.Update()
        upnldgPaymentItems.Update()
    End Sub
    Private Sub ControlVisibilityForAttachment()
        'If mPaymentAdvice.IsPAFileAttachment Then
        '    ImageButton1.Visible = True
        '    btnDelAttach.Enabled = CType(IIf(mPaymentAdvice.StatusID >= 2, False, True), Boolean)
        'Else
        '    ImageButton1.Visible = False
        '    btnDelAttach.Enabled = False
        'End If
        If mPaymentAdvice.IsPaymentFileAttachment Then
            ImageButton2.Visible = True
            btnDelAttach1.Enabled = IIf(mPaymentAdvice.IsPaymentDone = True, False, True) 'True
        Else
            ImageButton2.Visible = False
            btnDelAttach1.Enabled = False
        End If
        UpdatePanel4.Update()
        upnlPaymentDetails.Update()
    End Sub
    Private Sub Print()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsPaymentAdvice
        rpt = New crptPaymentAdvice

        Dim mCompanyDetail As CompanyDetail

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

        Dim Report As New ReportData(mCompanyDetail.CompanyName, _
        mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
        mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
        "Payment Advice", "", "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        mPaymentAdvice = PaymentAdvice.GetPaymentAdvice(mPaymentAdvice.ID)
        Session("mPaymentAdvice") = mPaymentAdvice

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        da.Fill(ds, mPaymentAdvice)
        da.Fill(ds, "PaymentAdviceItem", mPaymentAdvice.PaymentAdviceItems)
        rpt.SetDataSource(ds)
        Session("CrystalReport") = rpt
    End Sub
    Private Sub ViewImage(ByVal Sort As Integer)
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        If Sort = 1 Then
            If mPaymentAdvice.IsPAFileAttachment And Session("IsPAFileAttachment") Is Nothing Then
                mFileAttach = FileAttach.GetAttachment(mPaymentAdvice.ID, 1)
            Else
                mFileAttach = Session("IsPaymentFileAttachment")
            End If
        Else
            If mPaymentAdvice.IsPaymentFileAttachment And Session("IsPaymentFileAttachment") Is Nothing Then
                mFileAttach = FileAttach.GetAttachment(mPaymentAdvice.ID, 2)
            Else
                mFileAttach = Session("IsPaymentFileAttachment")
            End If
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
    Private Sub setObject()
        If calPaymentDate.Text = "" Then
            mPaymentAdvice.PaymentAdviceDate = Today.Date
        Else
            mPaymentAdvice.PaymentAdviceDate = CDate(calPaymentDate.Text)
        End If
        mPaymentAdvice.Text = txtRef.Text
        mPaymentAdvice.No = txtRefNo.Text

        mPaymentAdvice.PaymentTo = txtToText.Text
        mPaymentAdvice.PaymentFrom = txtFrom.Text
        mPaymentAdvice.VendorID = New Guid(cmbVendorList.SelectedValue)
        mPaymentAdvice.VendorName = cmbVendorList.SelectedItem.ToString

        mPaymentAdvice.CurrencyID = New Guid(cmbcurrency.SelectedValue)
        mPaymentAdvice.CurrencyName = cmbcurrency.SelectedItem.ToString
        mPaymentAdvice.ModeOfPaymentID = CInt(cmbModeOfPayment.SelectedValue)
        mPaymentAdvice.ModeOfPaymentName = cmbModeOfPayment.SelectedItem.ToString
        mPaymentAdvice.Note = txtNote.Text

        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mPaymentAdvice.IsPAFileAttachment = True
            Else
                mPaymentAdvice.IsPAFileAttachment = False
            End If
        End If
        For i As Integer = 0 To mPaymentAdvice.FileAttachments.Count - 1
            Dim txtValue As TextBox
            txtValue = CType(Me.dgPAAttachment.Rows(i).FindControl("txtFileName"), TextBox)
            mPaymentAdvice.FileAttachments(i).FileName = txtValue.Text.Trim
        Next
        mPaymentAdvice.IsPAFileAttachment = IIf(mPaymentAdvice.FileAttachments.Count > 0, True, False)
        Session("mPaymentAdvice") = mPaymentAdvice
    End Sub
    Private Sub SaveAttachment(ByVal Sort As Integer) '
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                Try
                    If Sort = 1 Then
                        mFileAttach.Sort = 1
                    Else
                        mFileAttach.Sort = 2
                        mFileAttach.SrNo = 1
                    End If

                    mFileAttach.Save()
                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
                End Try
            Else
                If (Not mPaymentAdvice.IsNew) And IsAttachmentDeleted Then
                    If Session("IsFromPendingPAPaymentPage") = True Then
                        FileAttach.DeleteAttachment(mFileAttach.ID, mPaymentAdvice.ID, 2)
                    Else
                        FileAttach.DeleteAttachment(mFileAttach.ID, mPaymentAdvice.ID, 1)
                    End If

                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If
    End Sub
    Private Sub SetPage()
        If Not mPaymentAdvice.IsNew Then
            lblTitle.Text = "Payment Advice [" & mPaymentAdvice.PaymentNo & "]"
        Else
            lblTitle.Text = "Payment Advice [NEW]"
        End If
        upnlTitle.Update()
    End Sub

    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "Delete")
        mPaymentAdvice.PaymentAdviceItems.CurrentIndex = Index
        Session("mPaymentAdvice") = mPaymentAdvice
    End Sub

    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtPaymentRef" Then
            If txtPaymentRef.Text.ToString = "" Then
                custValidator.ErrorMessage = "Please enter Payment Reference."
                e.IsValid = False
            End If
        End If
        If custValidator.ControlToValidate = "txtBank" Then
            If txtBank.Text.ToString = "" Then
                custValidator.ErrorMessage = "Please enter Bank Details."
                e.IsValid = False
            End If
        End If
        If custValidator.ControlToValidate = "txtChequeNo" Then
            If txtChequeNo.Text.ToString = "" Then
                custValidator.ErrorMessage = "Please enter Cheque No."
                e.IsValid = False
            End If
        End If
        If custValidator.ControlToValidate = "txtPaymentDate" Then
            If Not (txtPaymentDate.Text.ToString = "") Then
                If CDate(txtPaymentDate.Text.ToString) < CDate(mPaymentAdvice.PaymentAdviceDateFormatted.ToString) Then
                    custValidator.ErrorMessage = "Payment done date Should be grater than payment advice date."
                    e.IsValid = False
                End If
            End If

        End If

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
                            Dim mPaymentAdvice As PaymentAdvice
                            mPaymentAdvice = CType(Session("mPaymentAdvice"), PaymentAdvice)

                            Dim OrderDetail As String = mPaymentAdvice.PaymentAdviceItems.CurrentItem.OrderTextNo + " Dated : " + mPaymentAdvice.PaymentAdviceItems.CurrentItem.OrderDateFormatted + " to " + mPaymentAdvice.VendorName & " Created By : " & mPaymentAdvice.CreatedBy
                            MarkLog(Util.Action.Remove, "Payment Advice", OrderDetail, Util.ErrorType.NoError, mPaymentAdvice.ID, EventLogID)

                            mPaymentAdvice.PaymentAdviceItems.Remove(mPaymentAdvice.PaymentAdviceItems.CurrentItem)
                            mPaymentAdvice.CalculateTotal()

                            'If mPaymentAdvice.IsRoundOff = True Then
                            '    mPaymentAdvice.RoundCGrandTotal()
                            'End If

                            Session("mPaymentAdvice") = mPaymentAdvice
                            dgPaymentItems.DataSource = mPaymentAdvice.PaymentAdviceItems
                            dgPAAttachment.DataSource = mPaymentAdvice.FileAttachments
                            DataBind()
                            upnlPaymentItems.Update()
                            upnldgPaymentItems.Update()
                            upnlTotalAmount.Update()
                            upnlPaymentDetails.Update()

                        Catch ex As SqlException

                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(ex.Message, False), True)
                            Exit Sub
                        Finally
                            'Dim OrderDetail As String = mPaymentAdvice.PaymentNo + " Dated : " + mPaymentAdvice.PaymentDateFormatted + " to " + mPaymentAdvice.VendorName & " Created By : " & mPaymentAdvice.CreatedBy
                            'MarkLog(Util.Action.Delete, "Payment Advice", OrderDetail, Util.ErrorType.NoError, mPaymentAdvice.ID, EventLogID)
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "Status" Then
                        Session("sender") = ""
                        If Session("IsValid") Then
                            Session.Remove("IsValid")
                            mPaymentAdvice.StatusID = 2
                            mPaymentAdvice.AuthorizedBy = User.Identity.Name
                            Session("mPaymentAdvice") = mPaymentAdvice
                            Save()
                            SetPage()
                            dgPAAttachment.DataSource = mPaymentAdvice.FileAttachments
                            dgPAAttachment.DataBind()

                            ControlVisibility()
                            upnldgPAAttachment.Update()
                            upnlPAAttachment.Update()
                            upnlStatusName.DataBind()
                            upnlbuttons.DataBind()
                            upnlbuttons.Update()
                            upnlStatusName.Update()
                        Else
                            Session.Remove("IsValid")
                        End If
                    End If
                    If MSGBoxCtrl.Sender = "PAAuthorized" Then
                        Session("sender") = ""
                        MSGBoxCtrl.show(MSGBox.Message_title.StatusAuthorized, MSGBox.Message_text.StatusAuthorized, "<strong>Payment Advice</strong>", MsgBoxStyle.YesNo, "Status")
                        Exit Sub
                    End If

                    If MSGBoxCtrl.Sender = "Close" Then
                        Session("sender") = ""
                        If mPaymentAdvice.IsValid = True Then
                            Session.Remove("IsValid")
                            DataFieldBind()
                            If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user ", False), True)
                                Exit Sub
                            End If

                            If Save() Then
                                RemoveSession()
                                Response.Redirect("Index.aspx")
                            Else
                                Exit Sub
                            End If
                        Else
                            If CustomValidate2() = False Then
                                upnlValidationsummary.Update()
                                Exit Sub
                            End If
                        End If
                    End If

                    If MSGBoxCtrl.Sender = "Sendmail" Then
                        Session("ACToPA") = True
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPaymentAdviceSendMailWindow", "OpenPaymentAdviceSendMailWindow();", True)
                    End If

                    If MSGBoxCtrl.Sender = "RemoveAttachment" Then
                        Try
                            Session("Sender") = ""
                            Dim mPaymentAdvice As PaymentAdvice
                            mPaymentAdvice = CType(Session("mPaymentAdvice"), PaymentAdvice)
                            mPaymentAdvice.FileAttachments.Remove(mPaymentAdvice.FileAttachments.CurrentItem)
                            dgPaymentItems.DataSource = mPaymentAdvice.PaymentAdviceItems
                            dgPAAttachment.DataSource = mPaymentAdvice.FileAttachments
                            DataBind()
                            upnldgPAAttachment.Update()
                            upnlPAAttachment.Update()
                            Session("mPaymentAdvice") = mPaymentAdvice

                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        End Try
                    End If

                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session.Remove("IsValid")
                        Session("Sender") = ""
                        If mPaymentAdvice.IsNew Then Session.Remove("mPaymentAdvice")
                        RemoveSession()
                        Response.Redirect("Index.aspx")
                    End If

                    If MSGBoxCtrl.Sender = "Status" Or MSGBoxCtrl.Sender = "PAAuthorized" Then
                        Session("sender") = ""
                        Session.Remove("IsValid")
                        If mPaymentAdvice.IsNew Then Session.Remove("mPaymentAdvice")
                        RemoveSession()

                    End If

                    If MSGBoxCtrl.Sender = "Sendmail" Then
                        Session.Remove("IsValid")
                        Session("Sender") = ""
                        If mPaymentAdvice.IsNew Then Session.Remove("mPaymentAdvice")
                        RemoveSession()
                        'Response.Redirect("Index.aspx")
                    End If

            End Select
        End If
    End Sub
    'Added By Saylee on 09-Aug-2018 For ALL08082018
    Private Sub PrintWithOrder(Optional ByVal ByMail As Boolean = False)
        Dim da1 As New CSLA.Data.ObjectAdapter
        Dim ds1 As New dsPaymentAdvice
        rpt = New crptPaymentAdvice

        Dim mCompanyDetail As CompanyDetail

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

        Dim Report As New ReportData(mCompanyDetail.CompanyName, _
        mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
        mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
        "Payment Advice", SearchStr1:=AppSettings("ClientCode"), SearchStr2:="", SearchStr3:="", SearchStr4:="", SearchStr5:="", _
        ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), SearchStr6:="", SearchStr7:="", SearchStr8:="", SearchStr9:="", _
        SearchStr10:=AppSettings("Logo"))

        mPaymentAdvice = PaymentAdvice.GetPaymentAdvice(mPaymentAdvice.ID)
        Session("mPaymentAdvice") = mPaymentAdvice

        Dim mrptImage1 As rptImage = rptImage.GetImage(ds1)
        da1.Fill(ds1, mrptImage1)
        da1.Fill(ds1, Report)
        da1.Fill(ds1, mPaymentAdvice)
        da1.Fill(ds1, "PaymentAdviceItem", mPaymentAdvice.PaymentAdviceItems)
        rpt.SetDataSource(ds1)
        Session("CrystalReport") = rpt


        Dim MyFile1 = "C:\Temp\" & mPaymentAdvice.Text.Replace("/", "-") & ".pdf"

        Dim pdfList As New System.Collections.ArrayList

        Dim PDFNo As Integer = 1
        Dim PDFNoChild As Integer = 1
        PDFNo = PDFNo + 1

        Dim tmp As Integer
        Dim a As New Random

        tmp = a.Next

        rpt = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

        Dim myExportOption As CrystalDecisions.Shared.ExportOptions
        Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions


        myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
        myDiskOption.DiskFileName = MyFile1
        myExportOption = rpt.ExportOptions
        With myExportOption
            .DestinationOptions = myDiskOption
            .ExportDestinationType = ExportDestinationType.DiskFile
            .ExportFormatType = ExportFormatType.PortableDocFormat
        End With
        rpt.Export()
        rpt.Close()
        rpt.Dispose()
        GC.Collect()

        Dim pageCount As Integer = 0

        pdfList.Add(MyFile1)


        Dim mOrder As Order
        For j As Integer = 0 To mPaymentAdvice.PaymentAdviceItems.Count - 1
            mOrder = Order.GetOrder(mPaymentAdvice.PaymentAdviceItems(j).OrderID)


            Dim BaseCurrencysymbol As String = ""

            Dim da As New CSLA.Data.ObjectAdapter
            Dim rpt As CrystalDecisions.CrystalReports.Engine.ReportClass

            If CDate(mOrder.OrderDate.ToString) <= CDate("30-Jun-2017") Or mOrder.Visibility = 3 Then
                'Added By Vikrant on 2-July-2011 For FlyGer02072012
                If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "FG" Then
                    rpt = New crptOrderDetailPortraitForFlyGeorgia
                ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "JA" Then
                    rpt = New crptOrderDetailPortraitForJA
                Else
                    If mOrder.TransTypeID = 5 Then
                        'rpt = New crptOrder
                        If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
                            rpt = New crptOrderDetailPortraitForInd
                        ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then 'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                            rpt = New crptOrderDetailPortraitForHeligo
                        ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "HL" Then
                            rpt = New crptOrderDetailPortraitForHL
                            'Added By Shweta On 5th Feb-2013 for YA04022013-1
                        ElseIf (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
                            rpt = New crptOrderDetailPortraitForYA
                        ElseIf (AppSettings("ClientCode") = "CGA") Then
                            rpt = New crptOrderDetailPortraitForChhattisgarh 'Added By Prashant On 26-Aug-2014  CGA26082014
                        ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo") Then
                            rpt = New crptOrderDetailPortraitBA 'Added By Prashant On 30-Oct-2014  BA30102014
                        ElseIf (AppSettings("ClientCode") = "MID") Then
                            rpt = New crptOrderDetailPortraitForMidex
                        ElseIf (AppSettings("ClientCode") = "GEP") Then 'Added By Prashant On 16-Feb-2017
                            rpt = New crptOrderDetailPortraitForGEP
                        ElseIf (AppSettings("ClientCode") = "LAMA") Then
                            rpt = New crptOrderDetailPortraitLAMA
                        Else
                            rpt = New crptOrderDetailPortrait
                        End If
                    ElseIf mOrder.TransTypeID = 31 Then
                        'rpt = New crptOrderExchOH
                        If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
                            rpt = New crptOrderExchOHDetailPortraitForInd
                        ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then 'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                            rpt = New crptOrderExchOHDetailPortraitForHeligo
                        ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022 
                            rpt = New crptOrderExchOHDetailPortraitForDeccan
                            'Added By Shweta On 5th Feb-2013 for YA04022013-1
                        ElseIf (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
                            rpt = New crptOrderExchOHDetailPortraitForYA
                        ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo") Then  'Added By Prashant On 31-Jul-2014  BA31072014
                            rpt = New crptOrderExchOHDetailPortraitBA
                        ElseIf (AppSettings("ClientCode") = "CGA") Then
                            rpt = New crptOrderExchOHDetailPortraitForChhattisgarh 'Added By Prashant On 26-Aug-2014  CGA26082014
                        ElseIf (AppSettings("ClientCode") = "MID") Then
                            rpt = New crptOrderExchOHDetailPortraitForMidex
                        ElseIf (AppSettings("ClientCode") = "GEP") Then 'Added By Prashant On 16-Feb-2017
                            rpt = New crptOrderExchOHDetailPortraitForGEP
                        ElseIf (AppSettings("ClientCode") = "LAMA") Then
                            rpt = New crptOrderExchOHDetailPortraitLAMA
                        Else
                            rpt = New crptOrderExchOHDetailPortrait
                        End If
                    ElseIf mOrder.TransTypeID = 38 Then
                        If mOrder.IsOverhaul = True Then
                            'rpt = New crptOrderExchOH
                            If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
                                rpt = New crptOrderExchOHDetailPortraitForInd
                            ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then 'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                                rpt = New crptOrderExchOHDetailPortraitForHeligo
                            ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ" Then ' SPZ Code added by Saylee on 13-Jun-2022 
                                rpt = New crptOrderExchOHDetailPortraitForDeccan
                                'Added By Shweta On 5th Feb-2013 for YA04022013-1
                            ElseIf (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
                                rpt = New crptOrderExchOHDetailPortraitForYA
                            ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo") Then 'Added By Prashant On 31-Jul-2014  BA31072014
                                rpt = New crptOrderExchOHDetailPortraitBA
                            ElseIf (AppSettings("ClientCode") = "CGA") Then
                                rpt = New crptOrderExchOHDetailPortraitForChhattisgarh 'Added By Prashant On 26-Aug-2014  CGA26082014
                            ElseIf (AppSettings("ClientCode") = "MID") Then
                                rpt = New crptOrderExchOHDetailPortraitForMidex
                            ElseIf (AppSettings("ClientCode") = "GEP") Then 'Added By Prashant On 16-Feb-2017
                                rpt = New crptOrderExchOHDetailPortraitForGEP
                            ElseIf (AppSettings("ClientCode") = "LAMA") Then
                                rpt = New crptOrderExchOHDetailPortraitLAMA
                            Else
                                rpt = New crptOrderExchOHDetailPortrait
                            End If
                        Else
                            If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
                                rpt = New crptOrderWOForInd
                            ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then 'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                                rpt = New crptOrderWOForHeligo
                            ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022 
                                rpt = New crptOrderWOForDeccan
                                'Added By Shweta On 5th Feb-2013 for YA04022013-1
                            ElseIf (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
                                rpt = New crptOrderWOForYA
                            ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo") Then 'Added By Prashant On 31-Jul-2014  BA31072014
                                rpt = New crptOrderWOBA
                            ElseIf (AppSettings("ClientCode") = "CGA") Then
                                rpt = New crptOrderWOForChhattisgarh    'Added By Prashant On 26-Aug-2014  CGA26082014
                            ElseIf (AppSettings("ClientCode") = "MID") Then
                                rpt = New crptOrderExchOHDetailPortraitForMidex
                            ElseIf (AppSettings("ClientCode") = "GEP") Then 'Added By Prashant On 16-Feb-2017
                                rpt = New crptOrderWOForGEP
                            ElseIf (AppSettings("ClientCode") = "LAMA") Then
                                rpt = New crptOrderWOLAMA
                            Else
                                rpt = New crptOrderWO
                            End If
                        End If
                    ElseIf mOrder.TransTypeID = 39 Then
                        'rpt = New crptOrder
                        If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
                            rpt = New crptOrderDetailPortraitForInd
                        ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then 'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                            rpt = New crptOrderDetailPortraitForHeligo
                        ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "HL" Then
                            rpt = New crptOrderDetailPortraitForHL
                            'Added By Shweta On 5th Feb-2013 for YA04022013-1
                        ElseIf (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
                            rpt = New crptOrderDetailPortraitForYA
                        ElseIf (AppSettings("ClientCode") = "CGA") Then
                            rpt = New crptOrderDetailPortraitForChhattisgarh 'Added By Prashant On 26-Aug-2014  CGA26082014
                        ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo") Then
                            rpt = New crptOrderDetailPortraitBA 'Added By Prashant On 30-Oct-2014  BA30102014
                        ElseIf (AppSettings("ClientCode") = "GEP") Then 'Added By Prashant On 16-Feb-2017
                            rpt = New crptOrderDetailPortraitForGEP
                        ElseIf (AppSettings("ClientCode") = "LAMA") Then
                            rpt = New crptOrderDetailPortraitLAMA
                        Else
                            rpt = New crptOrderDetailPortrait
                        End If
                    End If
                End If
            Else
                rpt = New crptOrderGSTDetail
            End If
            '------------------
            'Added By Utkarsh ON 15-May-2013 FOR All13052013-1
            Dim mListOfKitItemsForOrderItem As ListOfKitItemsForOrderItem
            If CBool(AppSettings("ShowKitItems")) Then
                mListOfKitItemsForOrderItem = ListOfKitItemsForOrderItem.GetListOfKitItemsForOrderItems(mOrder.ID)
            End If

            'End
            Dim obj As rptOrders
            Dim objChilds As rptOrderChields
            Dim letter As rptLetterHead
            Dim ds As New dsOrder
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            obj = rptOrders.GetOrders(mOrder.ID)
            objChilds = rptOrderChields.GetOrderChields(mOrder.ID)
            'Added By Utkarsh(SearchStr1 Parameter Value) ON 15-May-2013 FOR All13052013-1
            If CBool(AppSettings("ShowKitItems")) Then
                letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", mListOfKitItemsForOrderItem.Count, AppSettings("Logo"), AppSettings("AdvancePayment"))
            Else
                letter = rptLetterHead.GetLetterHeadInfo(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "0", AppSettings("Logo"), AppSettings("AdvancePayment"))

            End If
            If letter.Count > 0 Then
                BaseCurrencysymbol = letter(0).BaseCurrencysymbol
                Session("BaseCurrencysymbol") = BaseCurrencysymbol
            End If
            da.Fill(ds, obj)
            da.Fill(ds, objChilds)
            da.Fill(ds, letter)
            da.Fill(ds, mrptImage)
            'Added By Utkarsh ON 15-May-2013 FOR All13052013-1
            If CBool(AppSettings("ShowKitItems")) Then
                da.Fill(ds, mListOfKitItemsForOrderItem)
            End If
            'End
            rpt.SetDataSource(ds)
            Session("CrystalReport") = rpt

            tmp = a.Next

            Dim MyFile2 As String = "C:\Temp\" & mOrder.Text.Replace("/", "-") & PDFNoChild.ToString & ".pdf"

            rpt = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)


            myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
            myDiskOption.DiskFileName = MyFile2
            myExportOption = rpt.ExportOptions
            With myExportOption
                .DestinationOptions = myDiskOption
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
            End With
            rpt.Export()
            rpt.Close()
            rpt.Dispose()
            GC.Collect()

            pdfList.Add(MyFile2)
            PDFNo = PDFNo + 1
            PDFNoChild = PDFNoChild + 1

        Next
        Dim MergedPath As String = "C:\Temp\" & "temp_myMergedPdf.pdf"
        Dim MergedPath_WM As String = "C:\Temp\" & "PaymentAdvice.pdf"

        Dim filesByte As New List(Of Byte())()
        For Each file__1 As String In pdfList 'files
            filesByte.Add(File.ReadAllBytes(file__1))
        Next

        File.WriteAllBytes(MergedPath, Flypal.PDFMergers.MergeFiles(filesByte))

        AddWatermarkText(MergedPath, MergedPath_WM, mPaymentAdvice.Text, , , iTextSharp.text.BaseColor.GRAY, , 0.0, pageCount)
        ''//********************************************Set Sessions*********************************************************//




        Session("PrintReportWithAttachment") = "True"

        If ByMail = True Then
            'do nothing
            Session("ReportPath") = MergedPath_WM
        Else
            Dim DeleteThis As String = mPaymentAdvice.Text
            Dim Files As String() = Directory.GetFiles("C:\Temp\")

            For Each file__1 As String In Files
                If file__1.ToUpper().Contains(DeleteThis.ToUpper()) Then
                    File.Delete(file__1)
                End If
            Next
            Session("CrystalReport") = MergedPath_WM
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        End If


    End Sub
    'End
    Private Function Save() As Boolean
        'Authentication
        If Not mPaymentAdvice.PaymentDate Is System.DBNull.Value Then
            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
            If mCheck.WebAuthentication = True Then
                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")
                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                If DateDiff(DateInterval.Day, CDate(mPaymentAdvice.PaymentDate), maxAllowableDate) < 0 Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(" Your subscription is expired. can not save Payment Advice. <br> Payment Advice Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), False), True)
                    Exit Function
                End If
            End If
        End If
        'Authentication
        Try
            If Not mPaymentAdvice.PaymentAdviceItems.Count = 0 Then
                setObject()
                setSession()
                Session("mPaymentAdvice") = mPaymentAdvice
                mPaymentAdvice.ApplyEdit()
                'Check if OrderText is blank then call TransTextSeries UI
                If (mPaymentAdvice.IsNew) And (mPaymentAdvice.Text = "") Then

                    Dim mPreviousTransTextSeries As TransTextSeries = TransTextSeries.GetTransTextPreviousSeries(mPaymentAdvice.TransTypeID, mPaymentAdvice.PaymentAdviceDateFormatted)

                    If (mPreviousTransTextSeries.IsAutoRenew = False) Or ((mPreviousTransTextSeries.IsAutoRenew = True) And (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mPaymentAdvice.TransTypeID) = False) Or (mPreviousTransTextSeries.TransTextSeriesDetails.Contains(mPaymentAdvice.TransTypeID) = True AndAlso mPreviousTransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mPaymentAdvice.TransTypeID).TransText = "")) Then

                        Dim str = "<script language='javascript'>openledgersame('wfPaymentAdvice_Ajax.aspx');</script>"

                        Session("BackPagestr_ForTransSeries") = str

                        Session("TransName_ForTransSeries") = "PaymentAdvice"
                        Session("TransTypeID_ForTransSeries") = mPaymentAdvice.TransTypeID
                        Session("TransDate_ForTransSeries") = mPaymentAdvice.PaymentAdviceDateFormatted
                        Session("AddTransTextSeries") = "True"

                        Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")

                    Else
                        Dim mAutoRenewTransTextSeries As AutoRenewTransTextSeries = AutoRenewTransTextSeries.RenewIt(mPreviousTransTextSeries)

                        If mAutoRenewTransTextSeries.IsRenewed Then
                            With mAutoRenewTransTextSeries.Renewed_TransTextSeries.TransTextSeriesDetails.ItemByTransTypeID(mPaymentAdvice.TransTypeID)
                                mPaymentAdvice.Text = .TransText
                                mPaymentAdvice.No = .StartingTransNo
                            End With
                        Else
                            Dim str = "<script language='javascript'>openledgersame('wfPaymentAdvice_Ajax.aspx');</script>"

                            Session("BackPagestr_ForTransSeries") = str

                            Session("TransName_ForTransSeries") = "Order"
                            Session("TransTypeID_ForTransSeries") = mPaymentAdvice.TransTypeID
                            Session("TransDate_ForTransSeries") = mPaymentAdvice.PaymentAdviceDateFormatted
                            Session("AddTransTextSeries") = "True"

                            Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0", False)
                        End If
                    End If

                End If

                mPaymentAdvice.Save()
                If Session("IsFromPendingPAPaymentPage") = True Then
                    SaveAttachment(2)
                Else
                    SaveAttachment(1)
                End If



                upnlbuttons.DataBind()
                upnlbuttons.Update()

                Dim PaymentAdviceDetail As String = mPaymentAdvice.PaymentNo.ToString + " Dated : " + mPaymentAdvice.PaymentAdviceDateFormatted.ToString + " to " + mPaymentAdvice.VendorName.ToString & " Created By : " & User.Identity.Name

                If mPaymentAdvice.StatusID = 2 Then
                    If mPaymentAdvice.IsPaymentDone Then
                        MarkLog(Util.Action.Save, "Payment Done", PaymentAdviceDetail & " Authorized By : " & mPaymentAdvice.AuthorizedBy, Util.ErrorType.NoError, mPaymentAdvice.ID, EventLogID)
                    Else
                        MarkLog(Util.Action.Authorize, "Payment Advice", PaymentAdviceDetail & " Authorized By : " & mPaymentAdvice.AuthorizedBy, Util.ErrorType.NoError, mPaymentAdvice.ID, EventLogID)
                    End If

                ElseIf mPaymentAdvice.StatusID = 3 Then
                    MarkLog(Util.Action.Amend, "Payment Advice", PaymentAdviceDetail, Util.ErrorType.NoError, mPaymentAdvice.ID, EventLogID)
                ElseIf mPaymentAdvice.StatusID = 4 Then
                    MarkLog(Util.Action.Cancel, "Payment Advice", PaymentAdviceDetail, Util.ErrorType.NoError, mPaymentAdvice.ID, EventLogID)
                Else
                    MarkLog(Util.Action.Save, "Payment Advice", PaymentAdviceDetail, Util.ErrorType.NoError, mPaymentAdvice.ID, EventLogID)
                End If

                mPaymentAdvice.MarkClean()
                lblTitle.Text = "Payment Advice ( Saved ...)"
                Session("mPaymentAdvice") = mPaymentAdvice
                SetPage()
                Return True

            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Payment Advice can not be saved without Oder Info.", False), True)
                Exit Function
            End If
        Catch ex As SqlException
            If ex.Number = 2627 Then
                MSGBoxCtrl.show("Save Alert!", "Payment Advice with Same Supplier No. already present in the system.", "Please enter different Supplier Invoice No. for item", MsgBoxStyle.OkOnly, "DupMsg")
                Exit Function
            End If
        Catch ex As Exception
            MSGBoxCtrl.show("Alert!", "Can Not Save ! " + "</br>" + ex.Message, "", MsgBoxStyle.OkOnly, "")
            Exit Function
        End Try

    End Function
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        'Deciding IsInRole String to check Rights
        If Session("IsFromPendingPAPaymentPage") = True Then
            IsInRoleString = "PendingPA"
        Else
            IsInRoleString = "PaymentAdvice"
        End If


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
    Public Function CustomValidate2() As Boolean
        Dim strMsg As String = ""
        setObject()
        If Not mPaymentAdvice.IsValid Then
            For i As Integer = 0 To mPaymentAdvice.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mPaymentAdvice.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        Dim mPaymentAdviceItem As PaymentAdviceItem
        If Not mPaymentAdvice.PaymentAdviceItems.IsValid Then
            For Each mPaymentAdviceItem In mPaymentAdvice.PaymentAdviceItems
                For i As Integer = 0 To mPaymentAdviceItem.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mPaymentAdviceItem.OrderTextNo + " : " + mPaymentAdviceItem.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
            Next
        End If
        If strMsg <> "" Then
            CustValidator.ErrorMessage = strMsg
            CustValidator.IsValid = False
            Return False
        End If
        Return True
    End Function
    Private Sub AttachMyFile()
        Dim BackupPath As String = ""
        BackupPath = AppSettings("DOCPath") & "New.PDF"

        Try
            If Not mPaymentAdvice.FileAttachments.Contains(mPaymentAdvice.ID, CType(Session("FileUpload.FileName"), String)) Then

                mPaymentAdvice.FileAttachments.Add(mPaymentAdvice.ID, CType(Session("FileUpload.FileName"), String))
                ' mPaymentAdvice.FileAttachments.CurrentItem.FileName = mFileAttach.FileName
                mPaymentAdvice.FileAttachments.CurrentItem.ImageFile = CType(Session("ImageFile"), Byte())
                mPaymentAdvice.FileAttachments.CurrentItem.Size = Session("Size")
                mPaymentAdvice.FileAttachments.CurrentItem.Extension = Session("Extension")
                mPaymentAdvice.FileAttachments.CurrentItem.Sort = 1
                '   mPaymentAdvice.FileAttachments.CurrentItem.SrNo = (mPaymentAdvice.FileAttachments.Count - 1) + 1

                Session("mPaymentAdvice") = mPaymentAdvice
                dgPAAttachment.DataSource = mPaymentAdvice.FileAttachments
                dgPAAttachment.DataBind()

                For i As Integer = 0 To mPaymentAdvice.FileAttachments.Count - 1
                    Dim txtValue As TextBox
                    txtValue = CType(Me.dgPAAttachment.Rows(i).FindControl("txtFileName"), TextBox)
                    txtValue.Text = mPaymentAdvice.FileAttachments(i).FileName
                Next

                Session.Remove("Size")
                Session.Remove("ImageFile")
                Session.Remove("Extension")
                Session.Remove("FileUpload.FileName")
                upnlPAAttachment.Update()
                upnldgPAAttachment.Update()
            Else
                Session("mPaymentAdvice") = mPaymentAdvice
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub DeleteAttachment(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveAttachment")
        mPaymentAdvice.FileAttachments.CurrentIndex = Index
        Session("mPaymentAdvice") = mPaymentAdvice
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCurrencyList = CurrencyList.GetCurrencyList(, , True)
        cmbcurrency.DataSource = mCurrencyList
        Session("mCurrencyList") = mCurrencyList

        mSupplierList = VendorList.GetVendortList(0, , , , , , True, False, True)
        Session("mSupplierList") = mSupplierList
        cmbVendorList.DataSource = mSupplierList

        mModeOfPaymentList = TypeList.GetTypeList(True, "(SELECT)")
        Session("mModeOfPaymentList") = mModeOfPaymentList
        cmbModeOfPayment.DataSource = mModeOfPaymentList

        dgPaymentItems.DataSource = mPaymentAdvice.PaymentAdviceItems
        dgPAAttachment.DataSource = mPaymentAdvice.FileAttachments
        DataBind()
        upnlbuttons.DataBind()
        upnlbuttons.Update()
        upnlPaymentDetails.Update()
        upnlTotalAmount.Update()
    End Sub
    'Private Sub DatadgPaymentItemsFieldBind()
    '    dgPaymentItems.DataSource = mPaymentAdvice.PaymentAdviceItems
    '    dgPAAttachment.DataSource = mPaymentAdvice.FileAttachments
    '    DataBind()
    '    upnlPaymentDetails.Update()
    'End Sub

#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        EventLogID = CType(Session("EventLogID"), Guid)
        addAttributes()
        getSession()
        If Not IsPostBack Then
            If CType(Session("AddTransTextSeries"), String) = "True" AndAlso (Not Session("TransText_ForTransSeries") Is Nothing) Then
                If Session("sender") = "IssueCreate" Then
                    '
                Else
                    If mPaymentAdvice.IsNew Then

                        mPaymentAdvice.Text = Session("TransText_ForTransSeries")
                        txtRef.Text = mPaymentAdvice.Text
                        Session("mPaymentAdvice") = mPaymentAdvice

                        Session("AddTransTextSeries") = "False"

                        Session.Remove("TransName_ForTransSeries")
                        Session.Remove("TransText_ForTransSeries")
                        Session.Remove("TransNo_ForTransSeries")
                    End If
                End If
            End If


            DataFieldBind()
            SetControl()
            ControlVisibility()
            SetPage()
        End If

    End Sub
    'Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
    '    Dim fileSize1 As Integer = 0
    '    Dim file1(fileSize1) As Byte

    '    If mPaymentAdvice.IsPAFileAttachment And mFileAttach Is Nothing Then
    '        mFileAttach = FileAttach.GetAttachment(mPaymentAdvice.ID, 1)
    '    End If

    '    mFileAttach.ImageFile = file1
    '    mFileAttach.Size = 0

    '    ImageButton1.Visible = False
    '    btnDelAttach.Enabled = False
    '    IsAttachmentDeleted = True
    '    mPaymentAdvice.IsPAFileAttachment = False
    '    Session("IsAttachmentDeleted") = IsAttachmentDeleted

    'End Sub
    Private Sub btnDelAttach1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach1.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte

        If mPaymentAdvice.IsPaymentFileAttachment And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mPaymentAdvice.ID, 2)
        End If

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton2.Visible = False
        btnDelAttach1.Enabled = False
        IsAttachmentDeleted = True
        mPaymentAdvice.IsPaymentFileAttachment = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted

    End Sub

    'Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
    '    If mPaymentAdvice.IsPAFileAttachment Then
    '        mFileAttach = FileAttach.GetAttachment(mPaymentAdvice.ID, 1)
    '    Else
    '        mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mPaymentAdvice.ID)
    '    End If
    '    Session("mFileAttach") = mFileAttach
    'End Sub



    'Private Sub btnSelectFile1_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile1.ServerClick
    '    If mPaymentAdvice.IsPaymentFileAttachment Then
    '        mFileAttach = FileAttach.GetAttachment(mPaymentAdvice.ID, 2)
    '    Else
    '        mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mPaymentAdvice.ID)
    '    End If
    '    Session("mFileAttach") = mFileAttach
    'End Sub
    Private Sub btnSelectFile3_Click(sender As Object, e As System.EventArgs) Handles btnSelectFile3.Click
        If mPaymentAdvice.IsPaymentFileAttachment Then
            mFileAttach = FileAttach.GetAttachment(mPaymentAdvice.ID, 2)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mPaymentAdvice.ID)
        End If
        Session("mFileAttach") = mFileAttach
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow();", True)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click

        If mPaymentAdvice.IsDirty Then
            Session("IsValid") = "True"
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.CloseConfirm, "", MsgBoxStyle.YesNo, "Close")
        Else
            If mPaymentAdvice.IsNew Then
                Session.Remove("mPaymentAdvice")
            End If
            RemoveSession()
            'Session("MiddleFrame") = ""
            Response.Redirect("Index.aspx")
        End If


    End Sub
    Private Sub btnClosePaymentDetails_Click(sender As Object, e As System.EventArgs) Handles btnClosePaymentDetails.Click
        If mPaymentAdvice.IsDirty Then
            Session("IsValid") = "True"
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.CloseConfirm, "", MsgBoxStyle.YesNo, "Close")
        Else
            If mPaymentAdvice.IsNew Then
                Session.Remove("mPaymentAdvice")
            End If
            RemoveSession()
            Response.Redirect("Index.aspx")
        End If

    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnOrdersForPaymentAdvice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnOrdersForPaymentAdvice.Click, hdnBtnPendingOrdersForPaymentAdvice.Click
        dgPaymentItems.DataSource = mPaymentAdvice.PaymentAdviceItems
        dgPaymentItems.DataBind()

        dgPAAttachment.DataSource = mPaymentAdvice.FileAttachments
        dgPAAttachment.DataBind()

        mPaymentAdvice.CalculateTotal()
        txtTotalAmt.DataBind()
        upnlPaymentDetails.DataBind()

        ControlVisibility()

        upnlPaymentItems.Update()
        ' upnldgPaymentItems.DataBind()
        upnldgPaymentItems.Update()
        upnlTotalAmount.Update()
        upnlPaymentDetails.Update()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        If Session("IsFromPendingPAPaymentPage") = True Then
            mPaymentAdvice.IsPaymentFileAttachment = True
            Session("IsPaymentFileAttachment") = mFileAttach
            ControlVisibilityForAttachment()
            UpdatePanel4.Update()
        Else
            AttachMyFile()
            upnlPAAttachment.Update()
        End If
    End Sub
    Private Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        setObject()
        setSession()
        If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user ", False), True)
            Exit Sub
        End If
        If IsValid Then
            If mPaymentAdvice.IsValid Then
                If Save() Then

                    upnlStatusName.DataBind()
                    upnlPaymentDetails.DataBind()

                    upnlStatusName.Update()
                    upnlPaymentDetails.Update()
                    upnlValidationsummary.Update()

                    MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                End If
            Else
                If CustomValidate2() = False Then
                    upnlValidationsummary.Update()
                    Exit Sub
                End If

            End If
        Else
        End If
    End Sub

    Private Sub btnSavePaymentDetails_Click(sender As Object, e As System.EventArgs) Handles btnSavePaymentDetails.Click

        If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user ", False), True)
            Exit Sub
        End If

        If Page.IsValid Then

            mPaymentAdvice.PaymentReference = txtPaymentRef.Text
            mPaymentAdvice.PaymentDate = txtPaymentDate.Text
            mPaymentAdvice.Bank = txtBank.Text
            mPaymentAdvice.ChequeNo = txtChequeNo.Text
            mPaymentAdvice.IsPaymentDone = True
            Session("mPaymentAdvice") = mPaymentAdvice

            setSession()
            If mPaymentAdvice.IsValid Then
                If Save() Then

                    upnlStatusName.DataBind()
                    upnlPaymentDetails.DataBind()

                    upnlStatusName.Update()
                    upnlPaymentDetails.Update()

                    ' MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")

                    If mPaymentAdvice.IsPaymentFileAttachment = False Then
                        MSGBoxCtrl.show("Alert!", "There is no file attachment do you want to continue send mail?", "", MsgBoxStyle.YesNo, "Sendmail")
                    Else
                        Session("ACToPA") = True
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPaymentAdviceSendMailWindow", "OpenPaymentAdviceSendMailWindow();", True)
                    End If

                End If
            Else
                upnlValidationsummary.Update()
                Exit Sub
            End If
        Else
            unplPendingPADetails.Update()
            Exit Sub
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As System.EventArgs) Handles btnAdd.Click

        setObject()
        Session("mPaymentAdvice") = mPaymentAdvice

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPendingOrdersPaymentAdviceWindow", "OpenPendingOrdersPaymentAdviceWindow();", True)

    End Sub
    'Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
    '    ViewImage(1)
    'End Sub
    Private Sub ImageButton2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        ViewImage(2)
    End Sub

    Private Sub dgPaymentItems_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPaymentItems.RowCommand
        Select Case e.CommandName
            Case "EditView"

                Dim Index As Integer = CInt(e.CommandArgument)
                mPaymentAdvice.PaymentAdviceItems.CurrentIndex = Index
                Session("mPaymentAdvice") = mPaymentAdvice
                Dim OrderDetail As String = mPaymentAdvice.PaymentNo + " Dated : " + mPaymentAdvice.PaymentDateFormatted + " to " + mPaymentAdvice.VendorName & " Created By : " & mPaymentAdvice.CreatedBy
                MarkLog(Util.Action.Edit, "Payment Advice", OrderDetail, Util.ErrorType.NoError, mPaymentAdvice.ID, EventLogID)
                Session("PaymentAdviceEdit") = True
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenOrdersForPaymentAdviceWindow", "OpenOrdersForPaymentAdviceWindow();", True)

            Case "DeleteRecord"
                Dim Index As Integer = CInt(e.CommandArgument)
                DeleteRecord(Index)

        End Select
    End Sub
    Private Sub btnSaveAttachmentApprove_Click(sender As Object, e As System.EventArgs) Handles btnSaveAttachmentApprove.Click
        If Not IsInRole(Rights.Authorized) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If IsValid Then
            Session("IsValid") = IsValid
            If mPaymentAdvice.FileAttachments.Count <> 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.StatusAuthorized, MSGBox.Message_text.StatusAuthorized, "<strong>Payment Advice</strong>", MsgBoxStyle.YesNo, "Status")
                Exit Sub
            Else
                MSGBoxCtrl.show("Alert", "This payment Advice has no attachment.Do you want to continue", "", MsgBoxStyle.YesNo, "PAAuthorized")
                Exit Sub
            End If
        End If
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As System.EventArgs) Handles btnPrint.Click
        If Not IsInRole(Rights.Print) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        'Print()
        PrintWithOrder() 'Added By Saylee on 09-Aug-2018 For ALL08082018
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub

    Private Sub btnSendMail_Click(sender As Object, e As System.EventArgs) Handles btnSendMail.Click
        'Print()
        PrintWithOrder(True) 'Added By Saylee on 09-Aug-2018 For ALL08082018
        Session("PAToAC") = True
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPaymentAdviceSendMailWindow", "OpenPaymentAdviceSendMailWindow();", True)
    End Sub

    Private Sub cmbcurrency_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbcurrency.SelectedIndexChanged, cmbVendorList.SelectedIndexChanged
        If cmbVendorList.SelectedIndex > 0 And cmbcurrency.SelectedIndex > 0 Then
            btnAdd.Visible = True
        Else
            btnAdd.Visible = False
        End If
        upnlbtnAdd.Update()
    End Sub

    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
        If Session("ACToPA") = True Then

            Dim No As New Random
            Dim StrName As String = "abc" & No.Next.ToString
            If Not Session("CloseWithoutSendMail") = True Then
                If mPaymentAdvice.IsPaymentFileAttachment And mFileAttach Is Nothing Then
                    mFileAttach = FileAttach.GetAttachment(mPaymentAdvice.ID, 2)
                End If
                If Not mFileAttach Is Nothing Then
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
                            '--------------

                            Dim str As String
                            str = "This Payment is doned By : " & User.Identity.Name
                            Dim StrMailBody As String = ""

                            StrMailBody = "<html>"
                            StrMailBody = StrMailBody + "<head>"
                            StrMailBody = StrMailBody + "</head>"
                            StrMailBody = StrMailBody + "<body style=""font-family: Tahoma; font-size: smaller;"">"
                            ' StrMailBody = StrMailBody + "<b>Dear" + mPaymentAdvice.PaymentTo.ToString + "</b>"
                            StrMailBody = StrMailBody + "<br /><br />"
                            StrMailBody = StrMailBody + "Please find the attached approved payment advice for Supplier " + mPaymentAdvice.VendorName.ToString
                            StrMailBody = StrMailBody + "<br /> <br />"
                            Try
                                SendMailFile.SendMailFile(Nothing, User.Identity.Name, "Payment Done For " + mPaymentAdvice.VendorName.ToString, mPaymentAdvice.PaymentNo, _
                                                          Info:=StrMailBody.ToString, VendorEmailID:="", ToMailID:=Session("ToMailIDs").ToString.Trim, CCMailID:=Trim(Session("CCMailIDs")), _
                                                          ReportPath:=Session("DOCPath"), BCCMailID:="", Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"))
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTransDetail", MessageBox.Show("Mail Sent Successfully", False), True)

                            Catch ex As Exception
                                MSGBoxCtrl.show("Error", "Error Sending Mail", ex.InnerException.ToString + ex.Message.ToString, MsgBoxStyle.OkOnly, "")
                            End Try
                        End If
                    End If
                Else
                    Dim StrMailBody As String = ""

                    StrMailBody = "<html>"
                    StrMailBody = StrMailBody + "<head>"
                    StrMailBody = StrMailBody + "</head>"
                    StrMailBody = StrMailBody + "<body style=""font-family: Tahoma; font-size: smaller;"">"
                    StrMailBody = StrMailBody + "<br /><br />"
                    StrMailBody = StrMailBody + "Payment Done against payment advice " + mPaymentAdvice.PaymentNo + " for Supplier " + mPaymentAdvice.VendorName.ToString
                    StrMailBody = StrMailBody + "<br /> <br />"
                    Try
                        SendMailFile.SendMailFile(Nothing, User.Identity.Name, "Payment Done For " + mPaymentAdvice.VendorName.ToString, mPaymentAdvice.PaymentNo, _
                                                  Info:=StrMailBody.ToString, VendorEmailID:="", ToMailID:=Session("ToMailIDs").ToString.Trim, CCMailID:=Trim(Session("CCMailIDs")), _
                                                  ReportPath:="", BCCMailID:="", Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"))
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTransDetail", MessageBox.Show("Mail Sent Successfully", False), True)

                    Catch ex As Exception
                        MSGBoxCtrl.show("Error", "Error Sending Mail", ex.InnerException.ToString + ex.Message.ToString, MsgBoxStyle.OkOnly, "")
                    End Try
                End If
            End If
            MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
            Session.Remove("CloseWithoutSendMail")
            Session("CloseWithoutSendMail") = Nothing
        End If
    End Sub
    Private Sub calPaymentDate_TextChanged(sender As Object, e As System.EventArgs) Handles calPaymentDate.TextChanged
        mPaymentAdvice = Session("mPaymentAdvice")
        mPaymentAdvice.PaymentAdviceDate = calPaymentDate.Text
        txtRef.Text = mPaymentAdvice.Text
        txtRef.DataBind()
        Session("mPaymentAdvice") = mPaymentAdvice
        upnlPaymentDoneDetails.Update()
    End Sub
    Private Sub btnSelectFiles_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnSelectFiles.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow();", True)
    End Sub
    Private Sub dgPAAttachment_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPAAttachment.RowCommand
        Dim mFileAttachments As FileAttachments
        Select Case e.CommandName
            Case "View"
                Dim Index As Integer = CInt(e.CommandArgument) '+ dgPAAttachment.PageSize * dgPAAttachment.PageIndex

                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttachments = mPaymentAdvice.FileAttachments
                'mFileAttachments.CurrentIndex = Index - 1

                If mFileAttachments.Count = 1 Then
                    mFileAttachments.CurrentIndex = 0
                Else
                    mFileAttachments.CurrentIndex = Index - 1
                End If

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
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
                    End If
                End If
                dgPAAttachment.DataSource = mPaymentAdvice.FileAttachments
                dgPAAttachment.DataBind()
                ControlVisibility()
                upnlPAAttachment.Update()
                upnldgPAAttachment.Update()
            Case "Remove"
                'Dim Index As Integer = CInt(e.CommandArgument) '+ dgPAAttachment.PageSize * dgPAAttachment.PageIndex
                Dim Index As Integer = CInt(e.CommandArgument) + dgPAAttachment.PageSize * dgPAAttachment.PageIndex
                ' DeleteAttachment(Index)
                mFileAttachments = mPaymentAdvice.FileAttachments
                If mFileAttachments.Count = 1 Then
                    DeleteAttachment(0)
                Else
                    DeleteAttachment(Index - 1)
                End If
        End Select

    End Sub
#End Region



End Class
