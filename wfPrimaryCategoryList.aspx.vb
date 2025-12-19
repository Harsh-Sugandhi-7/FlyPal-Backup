Public Class wfPrimaryCategoryList
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mCategoryList As CategoryList
    Public mPrimaryCategoryList As PrimaryCategoryList
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mCategoryList = Session("mCategoryList")
        mPrimaryCategoryList = Session("mPrimaryCategoryList")
    End Sub
    Private Sub SetSession()
        Session("mCategoryList") = mCategoryList
        Session("mPrimaryCategoryList") = mPrimaryCategoryList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCategoryList")
        Session.Remove("mPrimaryCategoryList")
    End Sub
    Private Sub ControlVisibility()
        For i As Integer = 0 To dgCategoryList.Items.Count - 1
            Dim cmbValue As DropDownList

            cmbValue = CType(Me.dgCategoryList.Items(i).FindControl("cmbPrimaryCategoryList"), DropDownList)
            If cmbValue.SelectedIndex <= 0 Then
                btnUpdate.Enabled = False
                btnUpdateBottom.Enabled = False
                Exit Sub
            Else
                btnUpdate.Enabled = True
                btnUpdateBottom.Enabled = True
            End If
        Next
    End Sub
    Private Function IsSelectedIndex() As Boolean
        Dim i As Integer = 0
        Dim cmbValue As DropDownList
        For i = 0 To dgCategoryList.Items.Count - 1
            cmbValue = CType(dgCategoryList.Items(i).FindControl("cmbPrimaryCategoryList"), DropDownList)
            If cmbValue.SelectedIndex = 0 Then
                Return True
                'Exit Function
            Else
                Return False
            End If
        Next
    End Function
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If IsSelectedIndex() = True Then
            e.IsValid = False
            custValidator.ErrorMessage = "Select Primary Category"
        Else
            e.IsValid = True
        End If
        'End If
    End Sub
#End Region

#Region " DataFieldBind "
    Public Sub GridBind()
        mCategoryList = CategoryList.GetCategoryList()
        dgCategoryList.DataSource = MCategoryList
        Session("mCategoryList") = MCategoryList

        mPrimaryCategoryList = PrimaryCategoryList.GetPrimaryCategoryList("(SELECT)")
        Session("mPrimaryCategoryList") = mPrimaryCategoryList

        DataBind()
        lblResult.Text = "Category List :" & mCategoryList.Count & " Record(s) found."
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            Session("MiddleFrame") = "wfPrimaryCategoryList.aspx?"
            GridBind()
        End If
        ControlVisibility()
    End Sub
    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click, btnUpdateBottom.Click
        If IsValid Then
            Dim cnt As Integer = 0
            Dim mCategory As Category
            For i As Integer = 0 To dgCategoryList.Items.Count - 1
                Dim cmbValue As DropDownList
                cmbValue = CType(Me.dgCategoryList.Items(i).FindControl("cmbPrimaryCategoryList"), DropDownList)
                mCategory = Category.GetCategory(New Guid(dgCategoryList.Items(i).Cells(0).Text))
                mCategory.GLCode = Trim(mCategory.Name.Substring(0, 2))
                mCategory.ID = New Guid(dgCategoryList.Items(i).Cells(0).Text)
                mCategory.PrimaryCategoryID = cmbValue.SelectedValue
                Try
                    If mCategory.IsDirty Then
                        MarkLog(Util.Action.Save, "PrimaryCategory", "Part Type : " + mCategory.Name + " Changed By : " + User.Identity.Name + " Status : " + cmbValue.SelectedItem.ToString, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    End If
                    mCategory.Save()
                    cnt += 1
                Catch ex As Exception
                    Throw ex.GetBaseException
                    ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("Error In Updating Primary Category."))
                End Try
            Next
            If cnt > 0 Then
                ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("Primary Category Selection Updated Successfully."))
                Session("MiddleFrame") = ""
                Response.Redirect("index.aspx")
            End If
        End If
    End Sub
    Private Sub dgCategoryList_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgCategoryList.SortCommand
        mCategoryList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mCategoryList") = mCategoryList
        dgCategoryList.DataSource = mCategoryList
        dgCategoryList.DataBind()
    End Sub
    Private Sub dgCategoryList_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgCategoryList.PageIndexChanged
        dgCategoryList.CurrentPageIndex = e.NewPageIndex
        dgCategoryList.DataSource = mCategoryList
        Session("mPartTypeList") = mCategoryList
        dgCategoryList.DataBind()
    End Sub
    Protected Sub cmbPrimaryCategoryList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ControlVisibility()
    End Sub
#End Region

End Class