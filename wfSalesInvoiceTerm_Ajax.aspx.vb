Public Class wfSalesInvoiceTerm_Ajax
    Inherits Page

#Region " Variable Declaration "

    Dim mTerms As Terms
    Public mSalesInvoice As SalesInvoice
    Dim Type As Int16

#End Region

#Region " Business Properties "

    Private Sub GetSession()
        mTerms = Session("mTerms")
        mSalesInvoice = Session("mSalesInvoice")
    End Sub

    Private Sub SetSession()
        Session("mTerms") = mTerms
        Session("mSalesInvoice") = mSalesInvoice
    End Sub

    Private Sub SetTerms()

        Dim i As Integer

        While i < mTerms.Count

            If mSalesInvoice.SalesInvoiceTerms.Contains(mTerms.Item(i).ID) = True Then
                mTerms.Item(i).IsSelected = True
            Else
                mTerms.Item(i).IsSelected = False
            End If

            i = i + 1

        End While

    End Sub

    Private Sub DataFieldBind()

        mTerms = Terms.GetTerms(mSalesInvoice.ID, 9)
        SetTerms()
        dgTerm.DataSource = mTerms
        dgTerm.DataBind()

    End Sub

    Private Sub SetSelectedTerms()

        Dim chkBox As CheckBox
        Dim Recordno, PageItems As Integer
        Dim i As Integer
        PageItems = dgTerm.Rows.Count - 1

        For i = 0 To PageItems

            Recordno = i + dgTerm.PageSize * dgTerm.PageIndex
            chkBox = CType(dgTerm.Rows(i).FindControl("chkSelect"), CheckBox)
            mTerms(Recordno).IsSelected = chkBox.Checked

        Next

        Session("mTerms") = mTerms

    End Sub

    Private Sub SetObject()

        Dim i As Integer = 0

        While i < mTerms.Count

            If mTerms.Item(i).IsDirty = True Then

                If mTerms.Item(i).IsSelected = True Then

                    If mSalesInvoice.SalesInvoiceTerms.Contains(mTerms.Item(i).ID) = False Then

                        mSalesInvoice.SalesInvoiceTerms.Add(mTerms.Item(i).ID)
                        mSalesInvoice.SalesInvoiceTerms.CurrentItem.Terms = mTerms.Item(i).Terms
                        mSalesInvoice.SalesInvoiceTerms.CurrentItem.TermID = mTerms.Item(i).ID

                    End If

                Else
                    mSalesInvoice.SalesInvoiceTerms.Remove(mTerms.Item(i).ID, "")
                End If

            End If

            i = i + 1

        End While

    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        GetSession()
        If Not IsPostBack Then
            DataFieldBind()
            SetSession()
        End If

    End Sub

    Private Sub SelectTerms(sender As Object, e As EventArgs) Handles btnOK.Click

        SetSelectedTerms()
        SetObject()
        Session("mSalesInvoice") = mSalesInvoice

        Dim openAs As String = Request.QueryString("Typepup")

        If openAs IsNot Nothing AndAlso openAs = "pup" Then

            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "On Select",
                                                "CallParentCallback();",
                                                True)
            Exit Sub

        End If

    End Sub

    Private Sub Cancle(sender As Object, e As EventArgs) Handles btnClose.Click

        Session.Remove("mTerms")

        Dim openAs As String = Request.QueryString("Typepup")

        If openAs IsNot Nothing AndAlso openAs = "pup" Then

            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "On Close",
                                                "CallParentCallback();",
                                                True)

            Exit Sub

        End If

    End Sub

    Private Sub HdnImgBtnTerm_Click(sender As Object, e As EventArgs) Handles hdnimgBtnTerm.Click

        DataFieldBind()
        Session("mTerms") = mTerms
        upnlTerm.Update()

    End Sub

#End Region

End Class