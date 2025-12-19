' Rajnish -  18-09-2006
Partial Class wfLogPax_Ajax
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Variable Declarations "
    Public mLogPax As LogPax
    Public mLogPaxList As LogPaxList
    Public mLog As Log
    Public mMachine As Machine
    Dim mLogPaxDetail As String
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mLog = CType(Session("mLog"), Log)
        mMachine = CType(Session("mMachine"), Machine)
        mLogPax = CType(Session("mLogPax"), LogPax)
        mLogPaxList = CType(Session("mLogPaxList"), LogPaxList)
    End Sub
    Private Sub SetSession()
        Session("mLog") = mLog
        Session("mMachine") = mMachine
        Session("mLogPax") = mLogPax
        Session("mLogPaxList") = mLogPaxList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mLogPaxList")
    End Sub
    Private Sub NewLogPax()
        mLogPax = LogPax.NewLogPax(mLog.ID)
        Session("mLogPax") = mLogPax
        SetPage() 'added
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mLogPax = LogPax.GetLogPax(mId)
        Session("mLogPax") = mLogPax
        SetPage() 'added
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        '''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        '''''msg1.ReplacePage = "wfLogPax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
        '''''Session("sender") = "Delete"
        '''''msg1.Show()

        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")

        mLogPax = LogPax.GetLogPax(mId)
        Session("mLogPax") = mLogPax
    End Sub
    Private Sub SetObject()
        With mLogPax
            '.SerialNo = Val(txtSerialNo.Text.Trim)
            .CompanyName = txtCompanyName.Text.Trim
            .PassengerName = txtPassengerName.Text.Trim
            .PassengerWeight = CDec(Val(txtPassengerWeight.Text.Trim))
            .LuggageWeight = CDec(Val(txtLuggageWeight.Text.Trim))
            .PercentUsage = CDec(Val(txtPercentUsage.Text.Trim))
        End With
        Session("mLogPax") = mLogPax
    End Sub
    Private Sub SetFromSearch()
        Dim Type As Short = Val(Request.QueryString("Type"))
        Dim Id As String = Request.QueryString("Id")
        Dim Name As String = Request.QueryString("Name")
        If Type = -1 Then
            mLogPax.CompanyID = New Guid(Id)
            mLogPax.CompanyName = Name
        End If
        Session("mLogPax") = mLogPax
    End Sub
    Private Overloads Sub SetFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Try
            Dim str As String
            'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
            'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
            str = "document.getElementById('" + cntrl.ClientID + "').focus();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
        Catch ex As Exception
            '
        End Try
    End Sub
    Private Sub SetPage()

        lblResult.Text = "Log Pax List: " & mLogPaxList.Count & " Record(s) found."

        If mLogPax.IsNew Then
            lbltitle.Text = "Log Pax [New]"
        Else
            lbltitle.Text = "Log Pax [" & mLogPax.PassengerName & "]"
        End If
        upnlTitle.Update()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult

        'If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
        '    Result1 = -1
        'Else
        '    Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        'End If

        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try

                            mLogPax = CType(Session("mLogPax"), LogPax)

                            mLogPaxDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted + " " + "Company Name : " + mLogPax.CompanyName + " Passenger Name : " + mLogPax.PassengerName
                            MarkLog(Util.Action.Delete, "Log Pax", mLogPaxDetail, Util.ErrorType.HandledError, mLogPax.ID, EventLogID)

                            LogPax.DeleteLogPax(mLogPax.ID)
                            NewLogPax()
                            '''''DataBind()

                            DataFieldBind() 'added
                            SetPage() 'added

                            upnlDetails.Update()
                            upnlErrorList.Update() 'added


                            '''''Response.Redirect("wfLogPax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                '''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                                '''''msg1.ReplacePage = "wfLogPax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
                                '''''msg1.Show()

                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")

                            ElseIf ex.Number = 2627 Then
                                '''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                                '''''msg1.ReplacePage = "wfLogPax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
                                '''''msg1.Show()

                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")

                            ElseIf ex.Number = 547 Then
                                '''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                                '''''msg1.ReplacePage = "wfLogPax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
                                '''''msg1.Show()

                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")

                            End If
                            DataFieldBind()
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        NewLogPax()

                        DataFieldBind() 'added
                        upnlDetails.Update()
                        upnlErrorList.Update() 'added
                    End If

                    '''''Response.Redirect("wfLogPax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added

                    '''''DataFieldBind()
                    '''''Response.Redirect("wfLogPax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))
                Case MsgBoxResult.Ok And MSGBoxCtrl.Sender = "Authorization"  'Code Added

                    '''''DataFieldBind()
                    '''''Response.Redirect("wfLogPax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))
            End Select
        ElseIf Result1 = -1 Then

            '''''Response.Redirect("wfLogPax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))
        ElseIf Result1 = 0 Then   'Code Added

            '''''DataFieldBind()
        End If
    End Sub
    Private Sub addAttributes()
        txtPassengerWeight.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtPassengerWeight').value,event)")
        txtLuggageWeight.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtLuggageWeight').value,event)")
        txtPercentUsage.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtPercentUsage').value,event)")
    End Sub
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        mLogPaxList = LogPaxList.GetLogPaxList(mLog.ID)
        Session("mLogPaxList") = mLogPaxList

        dgLogPaxList.DataSource = mLogPaxList
        dgLogPaxList.DataBind()

        'code added by Deven on 2/4/8************************       
        'If AppSettings("LogBookTimeEntry") = "UTC" Then
        If mMachine.IsUTC Then '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
            txtDepDateTime.Text = mLog.SouUniverseDateTimeFormatted.ToString
            txtArrDateTime.Text = mLog.DesUniverseDateTimeFormatted.ToString
        Else
            txtDepDateTime.Text = mLog.SouLocalDateTimeFormatted.ToString
            txtArrDateTime.Text = mLog.DesLocalDateTimeFormatted.ToString
        End If
        '****************************************************   

        DataBind()
    End Sub

#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            If imgbtnCompany.Enabled = True Then
                'Changed and added by Amrita on 8-Jan-08 suggested by Deven Sir
                'SetFocus(imgbtnCompany)
                SetFocus(imgbtnCompanyName)
            End If
            SetFromSearch()
            DataFieldBind()
        End If
        '''''MessageBoxResult()
        SetPage()
    End Sub
    '----------------------------------------------
    'Changed and added by Amrita on 8-Jan-08 suggested by Deven Sir
    Private Sub imgbtnCompanyName_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnCompanyName.Click
        SetObject()
        Dim str As String

        'removed <script> tag while working with - ScriptManager.RegisterStartupScript
        str = "openledgersame('wfSearch.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=wfLogPax_Ajax.aspx&Type=Company');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True) 'changed
    End Sub
    '---------------------------------------------
    Private Sub imgbtnCompany_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnCompany.Click
        SetObject()
        Dim str As String

        'removed <script> tag while working with - ScriptManager.RegisterStartupScript
        str = "openledgersame('wfCompany_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=wfLogPax_Ajax.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True) 'changed
    End Sub
    Private Sub dgLogPaxList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgLogPaxList.ItemCommand
        Dim mId As Guid = New Guid(e.Item.Cells(0).Text)
        Dim mCompanyName As String = e.Item.Cells(2).Text
        Dim mPassengerName As String = e.Item.Cells(3).Text
        Select Case e.CommandName
            Case "Edit"
                If (Not User.IsInRole("LogView") And Not User.IsInRole("LogEdit")) Then
                    '''''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                    '''''msg.ReplacePage = "wfLogPax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
                    '''''msg.Show()

                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")

                    Exit Sub
                End If

                mLogPaxDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted + " " + "Company Name : " + mCompanyName + " Passenger Name : " + mPassengerName
                MarkLog(Util.Action.Edit, "Log Pax", mLogPaxDetail, Util.ErrorType.HandledError, mId, EventLogID)

                EditRecord(mId)

                DataFieldBind() 'added
                upnlErrorList.Update() 'added
                upnlDetails.Update() 'added

                '''''Response.Redirect("wfLogPax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))
            Case "Delete"
                If (Not User.IsInRole("LogDelete")) Then

                    '''''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
                    '''''msg.ReplacePage = "wfLogPax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
                    '''''msg.Show()

                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")

                    Exit Sub
                End If
                DeleteRecord(mId)
        End Select
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If imgbtnCompany.Enabled = True Then
            'Changed and added by Amrita on 8-Jan-08 suggested by Amrita
            'SetFocus(imgbtnCompany)
            SetFocus(imgbtnCompanyName)
        End If
        txtCompanyName.Text = ""
        txtPassengerName.Text = ""
        txtPassengerWeight.Text = 0
        txtLuggageWeight.Text = 0
        txtPercentUsage.Text = 0
        lbltitle.Text = "Log Pax [New]"

        upnlErrorList.Update() 'added
    End Sub

    'Added function CustomValidate1  (solution for replacing REQUIRED FIELD VALIDATOR with CUSTOM VALIDATOR) 
    'Please refer HTML Changes -
    ' -- Required Field validator removed
    ' -- Custome Validator added (if not already there..) for any of Control. And Used in below function.
    Public Function CustomValidate1() As Boolean
        Dim strMSG As String = ""

        If txtCompanyName.Text = "" Then strMSG = "Company Name Required" + "<Br>"
        If txtPassengerName.Text = "" Then strMSG = strMSG + "Passenger Name Required" + "<Br>"

        If strMSG.Trim <> "" Then
            cvControlValidator.ErrorMessage = strMSG
            cvControlValidator.IsValid = False
            Return False
        End If
        Return True
    End Function

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("LogNew") And mLog.IsNew) Or (Not User.IsInRole("LogEdit") And Not mLog.IsNew) Then
            SetObject()
            SetSession()

            '''''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            '''''msg.ReplacePage = "wfLogPax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
            '''''Session("sender") = "Authorization"
            '''''msg.Show()

            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")

            Exit Sub
        End If

        'Replace IsValid with CustomValidate1  (solution for replacing REQUIRED FIELD VALIDATOR with CUSTOM VALIDATOR) 
        If Not CustomValidate1() Then upnlErrorList.Update() : Exit Sub
        Try
            SetObject()
            mLogPax.Save()

            mLogPaxDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted + " " + "Company Name : " + mLogPax.CompanyName + " Passenger Name : " + mLogPax.PassengerName
            MarkLog(Util.Action.Save, "Log Pax", mLogPaxDetail, Util.ErrorType.HandledError, mLogPax.ID, EventLogID)

            NewLogPax()
            DataFieldBind()
            SetSession()
            SetPage()
        Catch ex As SqlException
            If ex.Number = 8145 Then

                '''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                '''''msg1.ReplacePage = "wfLogPax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
                '''''Session("sender") = "Delete"
                '''''msg1.Show()

                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")

            ElseIf ex.Number = 2627 Then
                '''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                '''''msg1.ReplacePage = "wfLogPax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
                '''''Session("sender") = "Delete"
                '''''msg1.Show()

                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")

            ElseIf ex.Number = 547 Then
                '''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                '''''msg1.ReplacePage = "wfLogPax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
                '''''Session("sender") = "Delete"
                '''''msg1.Show()

                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")

            End If
        End Try

    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseClick.Click
        RemoveSession()
        'Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
        Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage")) 'Added by Prashant 23-Aug-2018
    End Sub
#End Region

#Region " Report "

#Region " Report Variable Declaration "
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Private SearchStr1 As String
    Private SearchStr2 As String
#End Region

#Region " Event "
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click

        If (Not User.IsInRole("LogPrint")) Then
            '''''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            '''''msg.ReplacePage = "wfLogPax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
            '''''msg.Show()
            '''''Exit Sub

            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")

        End If

        Dim Rpt As New crListLogPax
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList

        'Detail Section
        ReportDetails.Add(New rptStatus(, 0, , _
                  lblAircraft.Text, , lblDeparture.Text, , lblArrival.Text, , , , , , _
                     txtRegNo.Text, txtDeparture.Text, txtArrival.Text, , , , , , , _
                    lblDepDateTime.Text, txtDepDateTime.Text, lblArrDateTime.Text, txtArrDateTime.Text, _
                    , , , , ))

        'Pax Log List
        ReportDetails.Add(New rptStatus(, 1, , _
        , , , dgLogPaxList.Columns.Item(1).HeaderText, , dgLogPaxList.Columns.Item(2).HeaderText, dgLogPaxList.Columns.Item(3).HeaderText, _
         dgLogPaxList.Columns.Item(4).HeaderText, dgLogPaxList.Columns.Item(5).HeaderText, dgLogPaxList.Columns.Item(6).HeaderText, , _
        , , , , , , , , , , _
        , , , , , , ))

        Dim TotalCount As Integer
        TotalCount = Me.mLogPaxList.Count
        Dim I As Integer

        For I = 0 To TotalCount - 1
            Dim str(5) As String
            str(0) = ""
            str(1) = ""
            str(2) = ""
            str(3) = ""
            str(4) = ""
            str(5) = ""
            If Me.dgLogPaxList.Items(I).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgLogPaxList.Items(I).Cells.Item(1).Text
            If Me.dgLogPaxList.Items(I).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgLogPaxList.Items(I).Cells.Item(2).Text
            If Me.dgLogPaxList.Items(I).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgLogPaxList.Items(I).Cells.Item(3).Text
            If Me.dgLogPaxList.Items(I).Cells.Item(4).Text <> "&nbsp;" Then str(3) = Me.dgLogPaxList.Items(I).Cells.Item(4).Text
            If Me.dgLogPaxList.Items(I).Cells.Item(5).Text <> "&nbsp;" Then str(4) = Me.dgLogPaxList.Items(I).Cells.Item(5).Text
            If Me.dgLogPaxList.Items(I).Cells.Item(6).Text <> "&nbsp;" Then str(5) = Me.dgLogPaxList.Items(I).Cells.Item(6).Text

            ReportDetails.Add(New rptStatus(, 2, , _
             , , , str(0), , str(1), str(2), str(3), str(4), str(5), , _
        , , , , , , , , , , , , , , , , ))
        Next

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
   mCompanyDetail.WebSite, "Log Pax List Report", "", "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"))
        If mLogPaxList.Count = 0 Then
            '''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            '''''msg1.ReplacePage = "wfLogPax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
            '''''msg1.Show()

            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")

            Exit Sub
        End If
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        Dim Str1 As String
        Str1 = "openTranDetail();" 'removed <script> tag while working with - ScriptManager.RegisterStartupScript
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True) 'changed
    End Sub

    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

#End Region

#End Region




End Class
