Public Class wfLineMaintenanceInvoiceTerm_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mLineMaintInvoice As LineMaintenanceInvoice
    Dim mTerms As Terms
    Dim Type As Int16
#End Region

#Region " Business Properties "
    Private Sub GetSession()
        mTerms = Session("mTerms")
        mLineMaintInvoice = Session("mLineMaintInvoice")
    End Sub
    Private Sub SetSession()
        Session("mTerms") = mTerms
        Session("mLineMaintInvoice") = mLineMaintInvoice
    End Sub
    Private Sub setTerms()
        Dim i As Integer
        While i < mTerms.Count
            If mLineMaintInvoice.LineMaintenanceInvoiceTerms.Contains(mTerms.Item(i).ID) = True Then
                mTerms.Item(i).IsSelected = True
            Else
                mTerms.Item(i).IsSelected = False
            End If
            i = i + 1
        End While
    End Sub
    Private Sub DataFieldBind()
        Type = Request.QueryString("Type")
        mTerms = Terms.GetTerms(mLineMaintInvoice.ID, Type)
        setTerms()
        dgTerm.DataSource = mTerms
        dgTerm.DataBind()
    End Sub
    Private Sub setSelectedTerms()
        Dim chkBox As CheckBox
        Dim Recordno As Integer
        Dim i As Integer

        ' Set Selected Notes value  
        For i = 0 To dgTerm.Rows.Count - 1
            Recordno = i + dgTerm.PageSize * dgTerm.PageIndex
            chkBox = CType(dgTerm.Rows(i).FindControl("chkSelect"), CheckBox)
            mTerms(Recordno).IsSelected = chkBox.Checked
        Next
        Session("mTerms") = mTerms
    End Sub
    Private Sub setObject()
        Dim i As Integer = 0
        While i < mTerms.Count
            If mTerms.Item(i).IsDirty = True Then
                If mTerms.Item(i).IsSelected = True Then
                    If mLineMaintInvoice.LineMaintenanceInvoiceTerms.Contains(mTerms.Item(i).ID) = False Then
                        mLineMaintInvoice.LineMaintenanceInvoiceTerms.Add(mTerms.Item(i).ID)
                        mLineMaintInvoice.LineMaintenanceInvoiceTerms.CurrentItem.Terms = mTerms.Item(i).Terms
                        mLineMaintInvoice.LineMaintenanceInvoiceTerms.CurrentItem.TermID = mTerms.Item(i).ID
                    End If
                Else
                    mLineMaintInvoice.LineMaintenanceInvoiceTerms.Remove(mTerms.Item(i).ID, "")
                End If
            End If
            i = i + 1
        End While
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        If Not IsPostBack Then
            DataFieldBind()
            SetSession()
        End If
    End Sub
    'Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
    '    Session.Remove("mTerms")
    '    Response.Redirect(Request.QueryString("BackPage"))
    'End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        setSelectedTerms()
        setObject()
        Session("mLineMaintInvoice") = mLineMaintInvoice
        Dim mopenas As String = Request.QueryString("Typepup")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub imgbtnTerm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnTerm.Click
        'Response.Redirect("wfTerm.aspx?ChildPage=wfLineMaintenanceInvoiceTerm.aspx" & "&BackPage=" & Request.QueryString("BackPage") & "&Type=" & Request.QueryString("Type"))
    End Sub
    Private Sub hdnimgBtnTerm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnTerm.Click
        DataFieldBind()
        Session("mTerms") = mTerms
        upnlLineMaintInvTermDetails.Update()
    End Sub
#End Region

End Class