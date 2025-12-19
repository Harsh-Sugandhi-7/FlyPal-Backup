Partial Class wfrptReleaseNoteNo
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "
    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents lblAdd As System.Web.UI.WebControls.Label
    Protected WithEvents btnSave As System.Web.UI.WebControls.Button
    Protected WithEvents btnAdd As System.Web.UI.WebControls.Button
    Protected WithEvents txtConvFactor As System.Web.UI.WebControls.TextBox
    Protected WithEvents cvConvFactor As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents Validationsummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents rfvName As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents valError As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents txt As System.Web.UI.WebControls.TextBox
    'Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents reset As System.Web.UI.WebControls.Button

    Protected WithEvents print As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblConvFactor As System.Web.UI.WebControls.Label
    Protected WithEvents rfvSymbol As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfvConvFactor As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents dgCurrency As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblCurrencyName As System.Web.UI.WebControls.Label
    Protected WithEvents lblSymbol As System.Web.UI.WebControls.Label
    Protected WithEvents cvName As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents lblSearch As System.Web.UI.WebControls.Label
    Protected WithEvents txtCurrentReleaseNoteDate As SIControls.SICalendar
    Protected WithEvents txtChangeReleaseNoteDate As SIControls.SICalendar
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub
#End Region

#Region "Variable Declartation "
    Dim mReleaseNoteDate As String
    Public mReceiptItemID As Guid
    Dim mReleaseNoteNo As String
    Dim mReceiptDate As String
    Dim mChangedReleaseNoteNo As String
    Dim mChangedReleaseNoteDate As String
    Public mItemID As Guid
    Dim TempPartNo As String
    Dim SerialNo As String
    Dim ReceiptNo As String
    Dim EventLogID As Guid 'Added by saylee on 1-Aug-2011
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mReceiptItemID = CType(Session("mReceiptItemID"), Guid)
        mItemID = CType(Session("mItemID"), Guid)
        mReleaseNoteNo = CType(Session("mReleaseNoteNo"), String)
        mReleaseNoteDate = CType(Session("mReleaseNoteDate"), String)
        mReceiptDate = CType(Session("mReceiptDate"), String)
        TempPartNo = Session("TempPartNo")
        SerialNo = Session("SerialNo")
        ReceiptNo = Session("ReceiptNo")
    End Sub
    Private Sub SetSession()
        Session("mReceiptItemID") = mReceiptItemID
        Session("mItemID") = mItemID
        Session("mReleaseNoteNo") = mReleaseNoteNo
        Session("mReleaseNoteDate") = mReleaseNoteDate
        Session("mReceiptDate") = mReceiptDate
        Session("TempPartNo") = TempPartNo
        Session("SerialNo") = SerialNo
        Session("ReceiptNo") = ReceiptNo
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mReceiptItemID")
        Session.Remove("mItemID")
        Session.Remove("mReleaseNoteNo")
        Session.Remove("mReleaseNoteDate")
        Session.Remove("mReceiptDate")
        Session.Remove("TempPartNo")
        Session.Remove("SerialNo")
        Session.Remove("ReceiptNo")
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
            Result1 = -1
        Else
            Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        End If

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    mChangedReleaseNoteNo = Session("mChangedReleaseNoteNo")
                    txtChangedReleaseNoteNo.Text = mChangedReleaseNoteNo.ToString

                    mChangedReleaseNoteDate = Session("mChangedReleaseNoteDate")
                    txtChangeReleaseNoteDate.Value = mChangedReleaseNoteDate

                    Session.Remove("mChangedReleaseNoteNo")
                    Session.Remove("mChangedReleaseNoteDate")
                    Save()
                Case MsgBoxResult.No
                    RemoveSession()
                    Response.Redirect(Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            mChangedReleaseNoteNo = Session("mChangedReleaseNoteNo")
            txtChangedReleaseNoteNo.Text = mChangedReleaseNoteNo.ToString

            mChangedReleaseNoteDate = Session("mChangedReleaseNoteDate")
            txtChangeReleaseNoteDate.Value = mChangedReleaseNoteDate

            Session.Remove("mChangedReleaseNoteNo")
            Session.Remove("mChangedReleaseNoteDate")
            Response.Redirect("wfrptReleaseNoteNo.aspx?BackPage=" & Request.QueryString("BackPage"))
        End If
    End Sub
    Public Sub Save()
        'If Len(txtChangedReleaseNoteNo.Text) <> 0 Then
        ChangeReleaseNoteNoList.ChangeReleaseNoteNo(mReceiptItemID, mItemID, txtCurrentReleaseNoteNo.Text, txtChangedReleaseNoteNo.Text, txtCurrentReleaseNoteDate.Value.ToString, txtChangeReleaseNoteDate.Value.ToString)
        Dim ReceiptInfo As String
        Dim SerialNoInfo As String
        If SerialNo.ToString = "&nbsp;" Then
            SerialNoInfo = ""
        Else
            SerialNoInfo = SerialNo
        End If
        ReceiptInfo = "Part No. : " + TempPartNo + "  Serial No. : " + SerialNoInfo + " Receipt No. : " + ReceiptNo + " Old Release Note No. : " + txtCurrentReleaseNoteNo.Text + " New Release Note No. : " + txtChangedReleaseNoteNo.Text
        MarkLog(Util.Action.Save, "Receipt : Change Release Note No.", ReceiptInfo, Util.ErrorType.NoError, mReceiptItemID, EventLogID)
        Response.Redirect(Request.QueryString("BackPage"))
        'End If
    End Sub
    Public Sub Customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator
        CustValid = CType(s, CustomValidator)

        If CustValid.ControlToValidate = "txtChangeReleaseNoteDate" Then
            If CDate(txtChangeReleaseNoteDate.Value.ToString) > CDate(mReceiptDate.ToString) Then
                CustValid.ErrorMessage = "Release note date should be less or equal to receipt date"
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
        txtCurrentReleaseNoteNo.Text = mReleaseNoteNo
        txtCurrentReleaseNoteDate.ReadOnly = True
        EventLogID = CType(Session("EventLogID"), Guid)
        If mReleaseNoteDate.ToString = "&nbsp;" Then
            txtCurrentReleaseNoteDate.Value = ""
        Else
            txtCurrentReleaseNoteDate.Value = mReleaseNoteDate
        End If

        If txtChangedReleaseNoteNo.Enabled = True Then
            setFocus(txtChangedReleaseNoteNo)
        End If
        If Not IsPostBack Then

        End If
        MessageBoxResult()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If IsValid Then
            mChangedReleaseNoteNo = txtChangedReleaseNoteNo.Text
            Session("mChangedReleaseNoteNo") = mChangedReleaseNoteNo

            mChangedReleaseNoteDate = txtChangeReleaseNoteDate.Value.ToString
            Session("mChangedReleaseNoteDate") = mChangedReleaseNoteDate

            If (txtChangeReleaseNoteDate.Value.ToString = "" And txtChangedReleaseNoteNo.Text = "") Then
                Dim msg As New SIMsgBox(Page, "Alert!", "Either enter Release Note No. or select Release Note Date", "", MsgBoxStyle.OKOnly)
                msg.ReplacePage = "wfrptReleaseNoteNo.aspx?BackPage=" & Request.QueryString("BackPage")
                msg.Show()
                Exit Sub
            Else
                If txtCurrentReleaseNoteDate.Value.ToString <> "" And txtChangeReleaseNoteDate.Value.ToString = "" Then
                    Dim msg As New SIMsgBox(Page, "Alert!", "Release Note Date Not Selected. Do you want to continue?", "", MsgBoxStyle.YesNo)
                    msg.ReplacePage = "wfrptReleaseNoteNo.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg.Show()
                ElseIf txtCurrentReleaseNoteNo.Text <> "" And txtChangedReleaseNoteNo.Text = "" Then
                    Dim msg As New SIMsgBox(Page, "Alert!", "Release Note No. Not Entered. Do you want to continue?", "", MsgBoxStyle.YesNo)
                    msg.ReplacePage = "wfrptReleaseNoteNo.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg.Show()
                Else
                    Save()
                End If
            End If
        End If
    End Sub
#End Region

End Class
