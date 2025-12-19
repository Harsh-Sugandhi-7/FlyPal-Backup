'AJAX Conversion By Vikrant On 30-Jun-2014

Public Class wfCommonVendorList_Ajax
    Inherits System.Web.UI.Page

#Region "Variables and Declarations"
    Dim mVendors As Vendors
    Dim mtmpVendors As Vendors
    Dim mEnquiry As Enquiry
    Public mTransTypeID As Integer
#End Region

#Region "Business Method"
    Private Sub GetSession()
        mEnquiry = CType(Session("mEnquiry"), Enquiry)
        mVendors = Session("mVendors")
        mtmpVendors = Session("mtmpVendors")
        mTransTypeID = Session("mTransTypeID")
    End Sub
    Private Sub SetSession()
        Session("mEnquiry") = mEnquiry
        Session("mVendors") = mVendors
        Session("mtmpVendors") = mtmpVendors
    End Sub
    Public Sub ResetValues()
        Dim chkSelect As CheckBox
        For I As Integer = 0 To dgVendorList.Rows.Count - 1
            chkSelect = CType(dgVendorList.Rows(I).FindControl("chkSelect"), CheckBox)
            If mVendors(I).IsSelect And chkSelect.Checked Then
                mVendors.Item(I).IsSelect = Not chkSelect.Checked
                chkSelect.Checked = False
                mVendors.Item(I).MarkClean()
            End If
        Next
        Session("mVendors") = mVendors
    End Sub
    Private Sub setSelectedSuppliers()
        Dim chkBox As CheckBox
        For i As Integer = 0 To dgVendorList.Rows.Count - 1
            chkBox = CType(dgVendorList.Rows(i).FindControl("chkSelect"), CheckBox)
            mVendors(i).IsSelect = chkBox.Checked
        Next
        Session("mVendors") = mVendors
    End Sub

#End Region

#Region "DataBind"
    Private Sub SetObject()
        Dim i As Integer = 0
        While i < mVendors.Count
            If mVendors(i).IsDirty = True Then
                If mVendors(i).IsSelect = True Then
                    If mEnquiry.EnquirySuppliers.Contains(mVendors(i).ID) = False Then
                        mEnquiry.EnquirySuppliers.Add(mEnquiry.ID)
                        mEnquiry.EnquirySuppliers.CurrentItem.VendorID = mVendors(i).ID
                        mEnquiry.EnquirySuppliers.CurrentItem.VendorName = mVendors(i).Name
                        mEnquiry.EnquirySuppliers.CurrentItem.ContactPerson = mVendors(i).ContactPerson
                        mEnquiry.EnquirySuppliers.CurrentItem.VendorAddress = mVendors(i).Address
                        mEnquiry.EnquirySuppliers.CurrentItem.Phone = mVendors(i).Phone1 + " " + mVendors(i).Phone2 + " " + mVendors(i).Phone3
                        mEnquiry.EnquirySuppliers.CurrentItem.VendorMail = mVendors(i).Email
                    End If
                Else
                    mEnquiry.EnquirySuppliers.Remove(mVendors(i).ID, "")
                End If
            End If
            i = i + 1
        End While
    End Sub
    Private Sub GetSelectedVendors()
        If Not mVendors Is Nothing And mVendors.Count > 0 Then
            Dim mVendor As Vendor
            For Each mVendor In mtmpVendors
                If mVendors.Contains(mVendor) Then
                    mVendors.Item(mVendor.ID).IsSelect = mVendor.IsSelect
                End If
            Next
            Session("mVendors") = mVendors
            DataBind()
        End If
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        If Not IsPostBack Then
            'ResetValues()
            FindNow(0)
        Else
            dgVendorList.DataSource = mVendors
            dgVendorList.DataBind()
        End If
        'ControlVisibility()
    End Sub
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click, btnOkTop.Click
        setSelectedSuppliers()
        SetObject()
        Session("Vendors") = "True"
        Session("SelectVendors") = "True"
        Session("mEnquiry") = mEnquiry
        'Added by vikrant for popup
        'Dim mopenas As String = Request.QueryString("Type")
        'If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
        '    Exit Sub
        'End If
        'End
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        ''Added by vikrant for popup
        'Dim mopenas As String = Request.QueryString("Type")
        'If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
        '    Exit Sub
        'End If
        ''End
        Session("MiddleFrame") = "wfEnquiryList_Ajax.aspx?TransTypeId=" & mTransTypeID
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    'Added By Prashant 18-June-2009 for grid sorting
    Private Sub dgVendorList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgVendorList.Sorting
        mVendors.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mVendors") = mVendors
        dgVendorList.DataSource = mVendors
        dgVendorList.DataBind()
    End Sub
    '-----------------------------------------------
    Private Sub imgbtnAddNewSupplier_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnAddNewSupplier.Click
        Session.Remove("SearchIndex")
        Response.Redirect("wfVendorList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&BackPage1=wfCommonVendorList_Ajax.aspx")
    End Sub
#End Region

#Region "FindNow"
    Private Function getVendorStatus(ByVal TransTypeID As Integer, ByVal Type As Integer) As Boolean
        If Type = 0 Then                                  ''Purchase Enquiry 
            Select Case CType(TransTypeID, Trans)
                Case Util.Trans.RequestingForQuotation
                    Return True
                Case Util.Trans.OverHaulRepairEnquiry
                    Return True
                Case Util.Trans.RentialLeaseEnquiry
                    Return True
                Case Else
                    Return False
            End Select
        ElseIf Type = 1 Then                              'Sales Enquiry        
            Select Case CType(TransTypeID, Trans)
                Case Util.Trans.Enquiry
                    Return True
                Case Else
                    Return False
            End Select
        End If
    End Function
    Private Sub FindNow(ByVal Index As Int32)
        Select Case Index
            Case 0 'All
                mVendors = Vendors.GetVendortList(0, , , , , , , getVendorStatus(mEnquiry.TransTypeID, 1), getVendorStatus(mEnquiry.TransTypeID, 0))
        End Select
        '' setVendors()
        dgVendorList.DataSource = mVendors
        Session("mVendors") = mVendors
        dgVendorList.DataBind()
        lblResult.Text = "List of Vendors : " & mVendors.Count & " Record(s) found."
        If Not mtmpVendors Is Nothing Then GetSelectedVendors()
    End Sub
#End Region

    
   
End Class