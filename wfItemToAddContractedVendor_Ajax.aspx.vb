Public Class wfItemToAddContractedVendor_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mItemListToAddContractedVendor As ItemListToAddContractedVendor
    Public mCategoryLists As CategoryList
    Public mContractVendorList As VendorList
    Dim EventLogID As Guid
    Public mModelList As ModelList
    Dim SearchCriteria As String = String.Empty
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mItemListToAddContractedVendor = Session("mItemListToAddContractedVendor")
        mContractVendorList = Session("mContractVendorList")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mItemListToAddContractedVendor")
        Session.Remove("MiddleFrame")
        Session.Remove("mContractVendorList")
    End Sub
    Private Sub FindNow(ByVal Index As Int32)
        mItemListToAddContractedVendor = ItemListToAddContractedVendor.GetItemListToAddContractedVendor(txtSearch.Text.Trim, cmbCategory.SelectedValue.ToString)
        Session("mItemListToAddContractedVendor") = mItemListToAddContractedVendor
        gdvItem.DataSource = mItemListToAddContractedVendor
        gdvItem.DataBind()
        UpdateItemGridView()
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfItemToAddContractedVendor_Ajax.aspx?" Then
            RemoveSession()
        End If
    End Sub
    Private Sub SetControl()
        mCategoryLists = CategoryList.GetCategoryList("ALL")
        cmbCategory.DataSource = mCategoryLists

        mContractVendorList = VendorList.GetVendortList(0, , , , , , True, True, True, True)
        Session("mContractVendorList") = mContractVendorList

        DataBind()

        FindNow(0)
    End Sub
    Private Sub UpdateItemGridView()
        lblResult.Text = "List of Part as per criteria : " & mItemListToAddContractedVendor.Count.ToString & " Record(s) found."
        gdvItem.DataBind()
        upnlgrid.Update()
    End Sub
    Private Sub Save()
        Dim cmbVendorList As DropDownList
        Dim L As Integer = 0
        For i As Integer = 0 To gdvItem.Rows.Count - 1
            cmbVendorList = CType(Me.gdvItem.Rows(i).FindControl("cmbVendorList"), DropDownList)
            If cmbVendorList.SelectedIndex > 0 Then
                mItemListToAddContractedVendor(i).ContractedVendorID = New Guid(cmbVendorList.SelectedValue)
                Try
                    ItemListToAddContractedVendor.UpdateItemToAddContractedVendor(mItemListToAddContractedVendor(i).ItemID, mItemListToAddContractedVendor(i).ContractedVendorID)
                    L = 1
                Catch ex As Exception
                    MSGBoxCtrl.show("Alert", "Error In Updating", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End Try
            End If
        Next
        If L = 1 Then
            MSGBoxCtrl.show("Updated Successfully", "Updated Successfully", "", MsgBoxStyle.OkOnly, "")
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Save" Then
                        Try
                            Save()
                        Catch ex As Exception
                            Throw ex
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Save" Then
                    End If
            End Select
        End If
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 19-Jul-2011 For All19072011
        If Not IsPostBack And Session("sender") = "" Then
            Session("MiddleFrame") = "wfItemToAddContractedVendor_Ajax.aspx?"
            SetControl()
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        If IsValid Then
            FindNow(0)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub tabUpdateTop_Click(sender As Object, e As System.EventArgs) Handles btnUpdateTop.Click, btnUpdate.Click
        If hdnVendorIDList.Value <> "" Then
            Try
                MSGBoxCtrl.show("Update Alert", "This will set selected vendor(s) to respective Part(s). Do you want to continue? ", "", MsgBoxStyle.YesNo, "Save")
                Exit Sub
            Catch ex As Exception
            Finally
            End Try
            'Save()
        Else
            MSGBoxCtrl.show("Alert", "Please Select Vendor From List", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
            'upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

End Class