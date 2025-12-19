Partial Class wfSearch
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

#Region " Variable Declaration "
    Public mSearchList As SearchList
    Public Type As String
#End Region

#Region " Data Binding "
    Private Sub GetSession()
        mSearchList = CType(Session("mSearchList"), SearchList)
        Type = CType(Session("Type"), String)
    End Sub
    Private Sub SetSession()
        Session("mSearchList") = mSearchList
        Session("Type") = Type
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mSearchList")
        Session.Remove("Type")
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub FindNow(ByVal Type As String, Optional ByVal Name As String = "", Optional ByVal Code As String = "")
        mSearchList = SearchList.GetSearchList(Type, Name, Code)

        Session("mSearchList") = mSearchList
        dgList.DataSource = mSearchList
        dgList.DataBind()
    End Sub
    Private Sub ControlVisibility(ByVal Type As String)
        dgList.Columns(1).Visible = IIf(Type = "Place", True, False)
        dgList.Columns(3).Visible = IIf(Type = "Place", True, False)
        dgList.Columns(4).Visible = IIf(Type = "Pilot", True, False) 'Added by Saylee on 4-Mar-2011

        lblCode.Visible = IIf(Type = "Place", True, False)
        txtCode.Visible = IIf(Type = "Place", True, False)

        If Type = "Place" Then
            SetFocus(txtCode)
        Else
            SetFocus(txtName)
        End If
    End Sub
    Private Sub SetPage()
        If Type = "Pilot" Then
            lbltitle.Text = "List of " & "Flying Crew"
            lblResult.Text = "List of " & " " & "Flying Crew" & " : " & mSearchList.Count & " Record(s)  found."
            btnFindNow.ToolTip = "Click to find the " & "Flying Crew" & " as per search criteria"
            btnCloseTop.ToolTip = "Click to close List of " & "Flying Crew" & " screen"
            btnClose.ToolTip = "Click to close List of " & "Flying Crew" & " screen"
        Else
            lbltitle.Text = "List of " & Type
            lblResult.Text = "List of " & " " & Type & " : " & mSearchList.Count & " Record(s)  found."
            btnFindNow.ToolTip = "Click to find the " & Type & " as per search criteria"
            btnCloseTop.ToolTip = "Click to close List of " & Type & " screen"
            btnClose.ToolTip = "Click to close List of " & Type & " screen"
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()

        If Not IsPostBack And Session("sender") = "" Then
            Type = Request.QueryString("Type")
            Session("Type") = Type
            FindNow(Type)
        End If
        SetPage()
        ControlVisibility(Type)
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        FindNow(Type, txtName.Text, txtCode.Text)
        SetPage()
    End Sub
    Private Sub dgList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgList.ItemCommand
        Dim Id As String = e.Item.Cells(0).Text
        Dim Name As String = e.Item.Cells(1).Text
        Select Case e.CommandName
            Case "Select"
                If Type = "Company" Then
                    Name = e.Item.Cells(2).Text
                    Response.Redirect(Request.QueryString("BackPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage") & "&Type=-1&Id=" & Id & "&Name=" & Server.UrlEncode(Name))
                ElseIf Type = "Pilot" Then
                    Name = e.Item.Cells(2).Text
                    Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&Type=-1&Id=" & Id & "&Name=" & Server.UrlEncode(Name) & "&AddType=" & Request.QueryString("AddType"))
                Else
                    Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&Type=-1&Id=" & Id & "&Name=" & Server.UrlEncode(Name) & "&AddType=" & Request.QueryString("AddType"))
                End If
        End Select
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        RemoveSession()
        If Type = "Company" Then
            Response.Redirect(Request.QueryString("BackPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))
        Else
            Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&AddType=" & Request.QueryString("AddType"))
        End If
    End Sub
    Private Sub dgList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgList.SortCommand
        mSearchList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mSearchList") = mSearchList
        dgList.DataSource = mSearchList
        dgList.DataBind()
    End Sub
#End Region


End Class
