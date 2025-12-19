Imports System.Linq
Imports System.Text
Imports System.Collections.Generic
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Linq.Enumerable
Imports System
Imports System.IO
Partial Class wfVendorReplace_Ajax
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

#Region "Variable Declaration"
    Dim ChkModelIDs As String()
    Dim ChkVendorIDs As String()
    Dim VendorIDsIsToBeReplaced As New StringBuilder

    Dim ChkVendorNames As String()
    Dim mVendorNames As New StringBuilder
    Dim mVendorList As VendorList
    Dim mV As Vendor

    Public Flag As Boolean = False
    Dim MsgText As String
    Dim Detail As String
#End Region

#Region "Business Methods"
    Private Sub SetSession()
    End Sub
    Private Sub GetSession()
        MsgText = Session("MsgText")
    End Sub
    Private Sub UpdateCategoryIDOfItems()
        Try
            ChkVendorIDs = (From c As System.Web.UI.WebControls.ListItem In ListOfVendor.Items
                            Where c.Selected = True
                            Select (c.Value)).ToArray

            ChkVendorNames = (From c As System.Web.UI.WebControls.ListItem In ListOfVendor.Items
                              Where c.Selected = True
                              Select (c.Text)).ToArray
            If ChkVendorIDs.Length > 0 Then
                VendorIDsIsToBeReplaced.Append("<VendorID>")
                For i As Integer = 0 To ChkVendorIDs.Count - 1
                    VendorIDsIsToBeReplaced.Append("<id>")
                    VendorIDsIsToBeReplaced.Append(ChkVendorIDs(i))
                    VendorIDsIsToBeReplaced.Append("</id>")

                    mVendorNames.Append(ChkVendorNames(i))
                    mVendorNames.Append(",")
                    mVendorNames.Append(" ")
                Next
                VendorIDsIsToBeReplaced.Append("</VendorID>")
            End If

            mV = Vendor.GetVendor(ID:=New Guid(cmbVendorList.SelectedValue))
            Flag = Session("Flag")
            mV.UpdateData(VendorIDsIsToBeReplaced:=VendorIDsIsToBeReplaced.ToString, ID:=cmbVendorList.SelectedValue)
            Session.Remove("Flag")
            Detail = "Vendors : " & mVendorNames.ToString & " Replaced By : " & cmbVendorList.SelectedItem.Text & IIf(Flag = True, (" Deleted Vendors : " & mVendorNames.ToString), "")
            MarkLog(Util.Action.Save, "VendorReplace", Detail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Continue1" Then
                        MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, MsgText, MsgBoxStyle.YesNo, "Continue2")
                        Exit Sub
                    End If
                    If MSGBoxCtrl.Sender = "Continue2" Then
                        UpdateCategoryIDOfItems()
                        DataFieldBind()
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "disableEnableOnPageLoad", "disableEnableOnPageLoad()", True)
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Continue1" Or MSGBoxCtrl.Sender = "Continue2" Then
                        DataFieldBind()
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "disableEnableOnPageLoad", "disableEnableOnPageLoad()", True)
                    End If
            End Select
        End If
    End Sub
    Private Sub AddAttributes()
        btnReplaceNDelete.Attributes("onclick") = "javascript: document.body.style.cursor = 'wait';"
    End Sub
#End Region

#Region "Data Binding"
    Private Sub DataFieldBind()
        mVendorList = VendorList.GetVendortList(LookInType:=0, IsSelectTagRequired:=True)
        'ListOfVendor.DataSource = mVendorList
        cmbVendorList.DataSource = mVendorList
        Session("mVendorList") = mVendorList
        'ListOfVendor.DataBind()
        cmbVendorList.DataBind()
        cmbForValidation.DataSource = mVendorList
        cmbForValidation.DataBind()
        upnlVendor.Update()
    End Sub
    Private Sub RemoveSession()
    End Sub
    Private Sub SetValues()
        ChkVendorIDs = (From c As System.Web.UI.WebControls.ListItem In ListOfVendor.Items
                        Where c.Selected = True
                        Select (c.Value)).ToArray

        ChkVendorNames = (From c As System.Web.UI.WebControls.ListItem In ListOfVendor.Items
                          Where c.Selected = True
                          Select (c.Text)).ToArray
        If ChkVendorIDs.Length > 0 Then
            VendorIDsIsToBeReplaced.Append("<VendorID>")
            For i As Integer = 0 To ChkVendorIDs.Count - 1
                VendorIDsIsToBeReplaced.Append("<id>")
                VendorIDsIsToBeReplaced.Append(ChkVendorIDs(i))
                VendorIDsIsToBeReplaced.Append("</id>")

                mVendorNames.Append(ChkVendorNames(i))
                mVendorNames.Append(",")
                mVendorNames.Append(" ")
            Next
            VendorIDsIsToBeReplaced.Append("</VendorID>")
        End If
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbVendorList" Then
            If cmbVendorList.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Select vendor from the list."
                e.IsValid = False
            End If
        End If
        If custValidator.ControlToValidate = "cmbForValidation" Then
            ChkVendorIDs = (From c As System.Web.UI.WebControls.ListItem In ListOfVendor.Items
                            Where c.Selected = True
                            Select (c.Value)).ToArray
            If ChkVendorIDs.Length <= 0 Then
                custValidator.ErrorMessage = "Select vendor(s) to be replace from the list."
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        If Not Page.IsPostBack Then
            DataFieldBind()
        End If
    End Sub
    Private Sub btnReplaceNDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReplaceNDelete.Click
        If IsValid Then
            MsgText = "You Are Going To Replace & Delete Selected Vendor(s) " & " with " & cmbVendorList.SelectedItem.Text & "." & "<BR> <BR> Do you want to continue? "
            Flag = True
            Session("MsgText") = MsgText
            Session("Flag") = Flag
            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, MsgText, MsgBoxStyle.YesNo, "Continue1")
            'SetValues()
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Public Sub VendorToBeReplaceList()
        Dim ListOfVendors As Object = Nothing
        mV = Vendor.GetVendor(ID:=New Guid(cmbVendorList.SelectedValue))
        mVendorList = VendorList.GetVendortList(LookInType:=0,
                                                IsCustomer:=mV.IsCustomer,
                                                IsSupplier:=mV.IsSupplier,
                                                IsServiceProvider:=mV.IsServiceProvider)
        ListOfVendors = (From c In mVendorList
                         Where c.ID <> mV.ID
                         Select c).ToList

        ListOfVendor.DataSource = ListOfVendors
        ListOfVendor.DataBind()
        upnlVendorsIsToBeReplaced.Update()
    End Sub
    Private Sub cmbVendorList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbVendorList.SelectedIndexChanged
        VendorToBeReplaceList()
    End Sub
#End Region

End Class
