Public Class wfGreetings
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mCompanyDetailForGreetings As CompanyDetailForGreetings
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        mCompanyDetailForGreetings = Session("mCompanyDetailForGreetings")
    End Sub
    Private Sub Page_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        Session.Remove("mCompanyDetailForGreetings")
    End Sub
#End Region
  
End Class