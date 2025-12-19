Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Web.Script.Services
Imports Flypal
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports Newtonsoft.Json
Imports System.Web.Configuration
Imports Flypal.MachineReadOnly
Imports System.Configuration.ConfigurationManager
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web.HttpRequest
Imports Authenticate
' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class FlypalAPI
    Inherits System.Web.Services.WebService

#Region "Security"
    Public Credentials As Cred = New Cred

    Public Class Cred
        Inherits SoapHeader

        Public Sub New()
            Dim bp As BusinessPrincipal = BusinessPrincipal.login("BTPLAdmin", getDBPassword("BTPLAdmin"), "")
        End Sub

        Public Function getDBPassword(ByVal Username As String) As String

            Dim DBPassword As String = ""

            Try
                Dim cn As New SqlConnection(AppSettings("DB:FlyPal"))
                Dim cm As New SqlCommand
                Dim dr As SqlDataReader

                cn.Open()

                With cm

                    .Connection = cn
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "UM_fetchUserByName"

                    .Parameters.AddWithValue("@UserName", Username)

                    dr = cm.ExecuteReader()

                    dr.Read()

                    If (dr.HasRows) Then

                        DBPassword = dr.GetString(2)

                    End If

                End With

                dr.Close()
                cn.Close()

            Catch ex As Exception
                '
            End Try

            Return DBPassword

        End Function

    End Class

#End Region

#Region "JSON Methods"
    <WebMethod(Description:="Vendor List"), SoapHeader("Credentials")> _
     <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub GetVendorListJSON(ByVal Username As String, ByVal Password As String, ByVal VendorName As String) '1


        Dim str As String = ""
        Dim Authenticate As New CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
        If Authenticate.WebAuthentication = False Then
            str = "[{""Error 7"":" + """" + "Subscription has been expired," + """}]"
            Context.Response.Write(str)
            Authenticate = Nothing
            Exit Sub
        End If

        If Username.ToUpper <> "API" Or Password.ToUpper <> "API" Then
            str = "[{""Error 6"":" + """" + "User not found. Please check User Name Password," + """}]"
            Context.Response.Write(str)
            Exit Sub
        End If

        'Dim mlogin As SI.UTILITY.Login = New SI.UTILITY.Login(Username, Password)
        'Dim bp As BusinessPrincipal = BusinessPrincipal.login(mlogin.UserName, mlogin.Password, "")
        'If Thread.CurrentPrincipal.Identity.IsAuthenticated = False Then
        '    str = "[{""Error 6"":" + """" + "User not found. Please check User Name Password," + """}]"
        '    Context.Response.Write(str)
        '    Exit Sub
        'End If


        Dim TempFlypalWebServiceAccessCountSumForVendor As Integer
        Dim mFlypalWebServiceAccessCountForVendor As FlypalWebServiceAccessCount
        Dim mFlypalWebServiceAccessCountForVendorList As FlypalWebServiceAccessCountList
        Dim mFlypalWebServiceList As FlypalWebServiceList
        mFlypalWebServiceList = FlypalWebServiceList.GetFlypalWebServiceList(1)
        mFlypalWebServiceAccessCountForVendorList = FlypalWebServiceAccessCountList.GetFlypalWebServiceAccessCountList(1, Today.Date.ToString)
        Dim FlypalWebServiceAccessCountSumForVendor = From c In mFlypalWebServiceAccessCountForVendorList
                                                      Group c By FlypalWebServiceID = c.FlypalWebServiceID Into Group
                                                     Select New With {Key .FlypalWebServiceID = FlypalWebServiceID, Key .FlypalWebServiceAccessCount = Group.Sum(Function(x) x.FlypalWebServiceAccessCount)}
        For Each variable2 As Object In FlypalWebServiceAccessCountSumForVendor
            TempFlypalWebServiceAccessCountSumForVendor = variable2.FlypalWebServiceAccessCount
        Next

        'STEP # 01 - Call verification by checking Corporate ID & Subscription

        Try
            If TempFlypalWebServiceAccessCountSumForVendor <> mFlypalWebServiceList(0, "").AllowedDailyCount Then
                'STEP # 02 - Call verification by checking credentials 
                'STEP # 03 - Fetching data 

                Dim mVendorList As VendorList = VendorList.GetVendortList(1, Name:=VendorName)

                Dim VendorListWebMethod As Object = Nothing
                VendorListWebMethod = (From c In mVendorList
                                        Select c.Name, c.Code, c.Address, c.Phone1).ToList

                'EventLogUtil.EventLogSave(New Guid(AccessLogArray(0)), Username, Password, AccessLogArray(3), AccessLogArray(1), Now.Date.ToString, "Get List", "Crew Roster List", "Get Crew Roster List successfully", AccessLogArray(3), "Web Service")

                'STEP # 04 - Converting output into JSON format

                str = New JavaScriptSerializer().Serialize(VendorListWebMethod)
                Context.Response.Write(str)
                mFlypalWebServiceAccessCountForVendor = FlypalWebServiceAccessCount.NewFlypalWebServiceAccessCount(Guid.NewGuid)
                mFlypalWebServiceAccessCountForVendor.FlypalWebServiceID = 1
                mFlypalWebServiceAccessCountForVendor.Date = Today.Date
                mFlypalWebServiceAccessCountForVendor.FlypalWebServiceAccessCount = 1
                mFlypalWebServiceAccessCountForVendor.AccessBy = User.Identity.Name
                mFlypalWebServiceAccessCountForVendor.Save()
            Else
                str = "[{""Error 4"":" + """" + "Today's allowable limit for Vendor List API is over." + """}]"
                Context.Response.Write(str)
            End If
        Catch ex As Exception
            str = "[{""Error 5"":" + """" + ex.GetBaseException.Message + """}]"
            Context.Response.Write(str)
            'If Not mapp_LoginUser Is Nothing Then
            '    'EventLogUtil.EventLogSave(New Guid(AccessLogArray(0)), Username, Password, AccessLogArray(3), AccessLogArray(1), Now.Date.ToString, "Get List", "Crew Roster List", ex.GetBaseException.Message, AccessLogArray(3), "Web Service")
            'End If
        End Try
    End Sub
    <WebMethod(Description:="Item List"), SoapHeader("Credentials")> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub GetItemListJSON(ByVal Username As String, ByVal Password As String, ByVal PartNo As String, ByVal Category As String) '2
        Dim str As String = ""
        'Dim mapp_LoginUser As App_LoginUser = Nothing
        'Dim AccessLogArray As String() = SplitAccessLogValues(AccessLog)
        'STEP # 01 - Call verification by checking Corporate ID & Subscription
        Try
            Dim Authenticate As New CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
            If Authenticate.WebAuthentication = False Then
                str = "[{""Error 7"":" + """" + "Subscription has been expired," + """}]"
                Context.Response.Write(str)
                Authenticate = Nothing
                Exit Sub
            End If
            If Username.ToUpper <> "API" Or Password.ToUpper <> "API" Then
                str = "[{""Error 6"":" + """" + "User not found. Please check User Name Password," + """}]"
                Context.Response.Write(str)
                Exit Sub
            End If
            'Dim mlogin As SI.UTILITY.Login = New SI.UTILITY.Login(Username, Password)
            'Dim bp As BusinessPrincipal = BusinessPrincipal.login(mlogin.UserName, mlogin.Password, "")
            'If Thread.CurrentPrincipal.Identity.IsAuthenticated = False Then
            '    str = "[{""Error 6"":" + """" + "User not found. Please check User Name Password," + """}]"
            '    Context.Response.Write(str)
            '    Exit Sub
            'End If

            'STEP # 03 - Fetching data 
            'Dim ItemListAutoComplete As ItemListAutoComplete = ItemListAutoComplete.GetItemList()
            Dim TempFlypalWebServiceAccessCountSumForItem As Integer
            Dim mFlypalWebServiceAccessCountForItem As FlypalWebServiceAccessCount
            Dim mFlypalWebServiceAccessCountForItemList As FlypalWebServiceAccessCountList
            Dim mFlypalWebServiceList As FlypalWebServiceList
            mFlypalWebServiceList = FlypalWebServiceList.GetFlypalWebServiceList(2)
            mFlypalWebServiceAccessCountForItemList = FlypalWebServiceAccessCountList.GetFlypalWebServiceAccessCountList(2, Today.Date.ToString)
            Dim FlypalWebServiceAccessCountSumForItem = From c In mFlypalWebServiceAccessCountForItemList
                                                          Group c By FlypalWebServiceID = c.FlypalWebServiceID Into Group
                                                         Select New With {Key .FlypalWebServiceID = FlypalWebServiceID, Key .FlypalWebServiceAccessCount = Group.Sum(Function(x) x.FlypalWebServiceAccessCount)}
            For Each variable2 As Object In FlypalWebServiceAccessCountSumForItem
                TempFlypalWebServiceAccessCountSumForItem = variable2.FlypalWebServiceAccessCount
            Next
            If TempFlypalWebServiceAccessCountSumForItem <> mFlypalWebServiceList(0, "").AllowedDailyCount Then


                Dim mItemList As ItemList = ItemList.GetItemList(8, ItemName:=PartNo, CategoryName:=Category)
                Dim ItemListWebMethod As Object = Nothing
                ItemListWebMethod = (From c In mItemList
                                        Select c.Name, c.Description, c.CategoryName).ToList
                'EventLogUtil.EventLogSave(New Guid(AccessLogArray(0)), Username, Password, AccessLogArray(3), AccessLogArray(1), Now.Date.ToString, "Get List", "Crew Roster List", "Get Crew Roster List successfully", AccessLogArray(3), "Web Service")

                'STEP # 04 - Converting output into JSON format
                str = New JavaScriptSerializer().Serialize(ItemListWebMethod)
                Context.Response.Write(str)
                mFlypalWebServiceAccessCountForItem = FlypalWebServiceAccessCount.NewFlypalWebServiceAccessCount(Guid.NewGuid)
                mFlypalWebServiceAccessCountForItem.FlypalWebServiceID = 2
                mFlypalWebServiceAccessCountForItem.Date = Today.Date
                mFlypalWebServiceAccessCountForItem.FlypalWebServiceAccessCount = 1
                mFlypalWebServiceAccessCountForItem.AccessBy = User.Identity.Name
                mFlypalWebServiceAccessCountForItem.Save()
            Else
                str = "[{""Error 4"":" + """" + "Today's allowable limit for Item List API is over." + """}]"
                Context.Response.Write(str)
            End If
        Catch ex As Exception
            str = "[{""Error 5"":" + """" + ex.GetBaseException.Message + """}]"
            Context.Response.Write(str)
            'If Not mapp_LoginUser Is Nothing Then
            'EventLogUtil.EventLogSave(New Guid(AccessLogArray(0)), Username, Password, AccessLogArray(3), AccessLogArray(1), Now.Date.ToString, "Get List", "Crew Roster List", ex.GetBaseException.Message, AccessLogArray(3), "Web Service")
            'End If
        End Try
    End Sub
    <WebMethod(Description:="GRN Info"), SoapHeader("Credentials")> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub GetGRNInfoInJSON(ByVal Username As String, ByVal Password As String, ByVal FromDate As String, ByVal ToDate As String) '3
        Dim str As String = ""

        Dim Authenticate As New CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
        If Authenticate.WebAuthentication = False Then
            str = "[{""Error 7"":" + """" + "Subscription has been expired," + """}]"
            Context.Response.Write(str)
            Authenticate = Nothing
            Exit Sub
        End If
        If Username.ToUpper <> "API" Or Password.ToUpper <> "API" Then
            str = "[{""Error 6"":" + """" + "User not found. Please check User Name Password," + """}]"
            Context.Response.Write(str)
            Exit Sub
        End If
        'Dim mlogin As SI.UTILITY.Login = New SI.UTILITY.Login(Username, Password)
        'Dim bp As BusinessPrincipal = BusinessPrincipal.login(mlogin.UserName, mlogin.Password, "")
        'If Thread.CurrentPrincipal.Identity.IsAuthenticated = False Then
        '    str = "[{""Error 6"":" + """" + "User not found. Please check User Name Password," + """}]"
        '    Context.Response.Write(str)
        '    Exit Sub
        'End If

        If FromDate = "" Or ToDate = "" Then
            str = "[{""Error 1"":" + """" + "From Date And To Date Require." + """}]"
            Context.Response.Write(str)
            Exit Sub
        End If
        If (IsDate(FromDate) = False) Then
            str = "[{""Error 2"":" + """" + "Enter From Date Properly." + """}]"
            Context.Response.Write(str)
            Exit Sub
        End If
        If (IsDate(ToDate) = False) Then
            str = "[{""Error 2"":" + """" + "Enter To Date Properly." + """}]"
            Context.Response.Write(str)
            Exit Sub
        End If
        If (FromDate <> "" Or ToDate <> "") And (CDate(FromDate) > CDate(ToDate)) Then
            str = "[{""Error 3"":" + """" + "From Date Should Not Be Grater Than To Date." + """}]"
            Context.Response.Write(str)
            Exit Sub
        End If
        'STEP # 01 - Call verification by checking Corporate ID & Subscription
        Dim TempFlypalWebServiceAccessCountSumForGRN As Integer
        Dim mFlypalWebServiceAccessCountForGRN As FlypalWebServiceAccessCount
        Dim mFlypalWebServiceAccessCountForGRNList As FlypalWebServiceAccessCountList
        Dim mFlypalWebServiceList As FlypalWebServiceList
        mFlypalWebServiceList = FlypalWebServiceList.GetFlypalWebServiceList(3)
        mFlypalWebServiceAccessCountForGRNList = FlypalWebServiceAccessCountList.GetFlypalWebServiceAccessCountList(3, Today.Date.ToString)
        Dim FlypalWebServiceAccessCountSumForGRN = From c In mFlypalWebServiceAccessCountForGRNList
                                                      Group c By FlypalWebServiceID = c.FlypalWebServiceID Into Group
                                                     Select New With {Key .FlypalWebServiceID = FlypalWebServiceID, Key .FlypalWebServiceAccessCount = Group.Sum(Function(x) x.FlypalWebServiceAccessCount)}
        For Each variable2 As Object In FlypalWebServiceAccessCountSumForGRN
            TempFlypalWebServiceAccessCountSumForGRN = variable2.FlypalWebServiceAccessCount
        Next
        Try
            If TempFlypalWebServiceAccessCountSumForGRN <> mFlypalWebServiceList(0, "").AllowedDailyCount Then


                'STEP # 02 - Call verification by checking credentials 
                'STEP # 03 - Fetching data 
                Dim TempFromDate, TempToDate As String
                If FromDate = "" Then
                    TempFromDate = "1/1/1900"
                Else
                    If IsDate(FromDate) Then
                        TempFromDate = CDate(FromDate).ToString
                    Else
                        TempFromDate = "1/1/1900"
                    End If
                End If
                If ToDate = "" Then
                    TempToDate = "1/1/4400"
                Else
                    If IsDate(ToDate) Then
                        TempToDate = CDate(ToDate).ToString
                    Else
                        TempToDate = "1/1/4400"
                    End If
                End If
                Dim mPartsPurchaseStatementListForWebMethod As PartsPurchaseStatementListForWebMethod = PartsPurchaseStatementListForWebMethod.GetPartsPurchaseStatementListForWebMethod(FromDate:=TempFromDate, _
                                                                                                      ToDate:=TempToDate, Value:="Landing Value", ReceiptType:=1, ClientCode:=AppSettings("ClientCode"))

                Dim PartsPurchaseStatementWebMethod As Object = Nothing
                PartsPurchaseStatementWebMethod = (From c In mPartsPurchaseStatementListForWebMethod
                                        Select c.RecNo, c.VendorName, c.RDate, c.PartName, c.PartDescription, c.RecQty, c.EffRate, c.Currency, c.ChargeCurrency, c.ChargeTotal, c.ReceiptType).ToList

                'EventLogUtil.EventLogSave(New Guid(AccessLogArray(0)), Username, Password, AccessLogArray(3), AccessLogArray(1), Now.Date.ToString, "Get List", "Crew Roster List", "Get Crew Roster List successfully", AccessLogArray(3), "Web Service")

                'STEP # 04 - Converting output into JSON format
                str = New JavaScriptSerializer().Serialize(PartsPurchaseStatementWebMethod)
                Context.Response.Write(str)
                mFlypalWebServiceAccessCountForGRN = FlypalWebServiceAccessCount.NewFlypalWebServiceAccessCount(Guid.NewGuid)
                mFlypalWebServiceAccessCountForGRN.FlypalWebServiceID = 3
                mFlypalWebServiceAccessCountForGRN.Date = Today.Date
                mFlypalWebServiceAccessCountForGRN.FlypalWebServiceAccessCount = 1
                mFlypalWebServiceAccessCountForGRN.AccessBy = User.Identity.Name
                mFlypalWebServiceAccessCountForGRN.Save()
            Else
                str = "[{""Error 4"":" + """" + "Today's allowable limit for GRN Info API is over." + """}]"
                Context.Response.Write(str)
            End If

        Catch ex As Exception
            str = "[{""Error 5"":" + """" + ex.GetBaseException.Message + """}]"
            Context.Response.Write(str)
            'If Not mapp_LoginUser Is Nothing Then
            '    'EventLogUtil.EventLogSave(New Guid(AccessLogArray(0)), Username, Password, AccessLogArray(3), AccessLogArray(1), Now.Date.ToString, "Get List", "Crew Roster List", ex.GetBaseException.Message, AccessLogArray(3), "Web Service")
            'End If
        End Try
    End Sub
    <WebMethod(Description:="Issue Info"), SoapHeader("Credentials")> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub GetIssueInfoInJSON(ByVal Username As String, ByVal Password As String, ByVal FromDate As String, ByVal ToDate As String) '4
        Dim str As String = ""

        Dim Authenticate As New CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
        If Authenticate.WebAuthentication = False Then
            str = "[{""Error 7"":" + """" + "Subscription has been expired," + """}]"
            Context.Response.Write(str)
            Authenticate = Nothing
            Exit Sub
        End If
        If Username.ToUpper <> "API" Or Password.ToUpper <> "API" Then
            str = "[{""Error 6"":" + """" + "User not found. Please check User Name Password," + """}]"
            Context.Response.Write(str)
            Exit Sub
        End If
        'Dim mlogin As SI.UTILITY.Login = New SI.UTILITY.Login(Username, Password)
        'Dim bp As BusinessPrincipal = BusinessPrincipal.login(mlogin.UserName, mlogin.Password, "")
        'If Thread.CurrentPrincipal.Identity.IsAuthenticated = False Then
        '    str = "[{""Error 6"":" + """" + "User not found. Please check User Name Password," + """}]"
        '    Context.Response.Write(str)
        '    Exit Sub
        'End If

        If FromDate = "" Or ToDate = "" Then
            str = "[{""Error 1"":" + """" + "From Date And To Date Require." + """}]"
            Context.Response.Write(str)
            Exit Sub
        End If
        If (IsDate(FromDate) = False) Then
            str = "[{""Error 2"":" + """" + "Enter From Date Properly." + """}]"
            Context.Response.Write(str)
            Exit Sub
        End If
        If (IsDate(ToDate) = False) Then
            str = "[{""Error 2"":" + """" + "Enter To Date Properly." + """}]"
            Context.Response.Write(str)
            Exit Sub
        End If
        If (FromDate <> "" Or ToDate <> "") And (CDate(FromDate) > CDate(ToDate)) Then
            str = "[{""Error 3"":" + """" + "From Date Should Not Be Grater Than To Date." + """}]"
            Context.Response.Write(str)
            Exit Sub
        End If
        'STEP # 01 - Call verification by checking Corporate ID & Subscription
        Dim TempFlypalWebServiceAccessCountSumForIssue As Integer
        Dim mFlypalWebServiceAccessCountForIssue As FlypalWebServiceAccessCount
        Dim mFlypalWebServiceAccessCountForIssueList As FlypalWebServiceAccessCountList
        Dim mFlypalWebServiceList As FlypalWebServiceList
        mFlypalWebServiceList = FlypalWebServiceList.GetFlypalWebServiceList(4)
        mFlypalWebServiceAccessCountForIssueList = FlypalWebServiceAccessCountList.GetFlypalWebServiceAccessCountList(4, Today.Date.ToString)
        Dim FlypalWebServiceAccessCountSumForIssue = From c In mFlypalWebServiceAccessCountForIssueList
                                                      Group c By FlypalWebServiceID = c.FlypalWebServiceID Into Group
                                                     Select New With {Key .FlypalWebServiceID = FlypalWebServiceID, Key .FlypalWebServiceAccessCount = Group.Sum(Function(x) x.FlypalWebServiceAccessCount)}
        For Each variable2 As Object In FlypalWebServiceAccessCountSumForIssue
            TempFlypalWebServiceAccessCountSumForIssue = variable2.FlypalWebServiceAccessCount
        Next
        Try
            If TempFlypalWebServiceAccessCountSumForIssue <> mFlypalWebServiceList(0, "").AllowedDailyCount Then

                'STEP # 03 - Fetching data 
                Dim TempFromDate, TempToDate As String
                If FromDate = "" Then
                    TempFromDate = "1/1/1900"
                Else
                    If IsDate(FromDate) Then
                        TempFromDate = CDate(FromDate).ToString
                    Else
                        TempFromDate = "1/1/1900"
                    End If
                End If
                If ToDate = "" Then
                    TempToDate = "1/1/4400"
                Else
                    If IsDate(ToDate) Then
                        TempToDate = CDate(ToDate).ToString
                    Else
                        TempToDate = "1/1/4400"
                    End If
                End If
                Dim mrptConsumption As rptConsumption = rptConsumption.GetConsumption(StartDate:=TempFromDate, EndDate:=TempToDate, Category:="", _
                                                                                      WorkOrderNo:="0", IsValued:=True, Value:="Landing Value")

                Dim rptConsumptionList As Object = Nothing
                rptConsumptionList = (From c In mrptConsumption
                                        Select c.IssueNo, c.IssueToName, c.IssueDate, c.PartName, c.PartDescription, c.IssueQty, c.EffRate).ToList

                'EventLogUtil.EventLogSave(New Guid(AccessLogArray(0)), Username, Password, AccessLogArray(3), AccessLogArray(1), Now.Date.ToString, "Get List", "Crew Roster List", "Get Crew Roster List successfully", AccessLogArray(3), "Web Service")

                'STEP # 04 - Converting output into JSON format
                str = New JavaScriptSerializer().Serialize(rptConsumptionList)
                Context.Response.Write(str)
                mFlypalWebServiceAccessCountForIssue = FlypalWebServiceAccessCount.NewFlypalWebServiceAccessCount(Guid.NewGuid)
                mFlypalWebServiceAccessCountForIssue.FlypalWebServiceID = 4
                mFlypalWebServiceAccessCountForIssue.Date = Today.Date
                mFlypalWebServiceAccessCountForIssue.FlypalWebServiceAccessCount = 1
                mFlypalWebServiceAccessCountForIssue.AccessBy = User.Identity.Name
                mFlypalWebServiceAccessCountForIssue.Save()
            Else
                str = "[{""Error 4"":" + """" + "Today's allowable limit for Issue Info API is over." + """}]"
                Context.Response.Write(str)
            End If
        Catch ex As Exception
            str = "[{""Error 5"":" + """" + ex.GetBaseException.Message + """}]"
            Context.Response.Write(str)
            'If Not mapp_LoginUser Is Nothing Then
            '    'EventLogUtil.EventLogSave(New Guid(AccessLogArray(0)), Username, Password, AccessLogArray(3), AccessLogArray(1), Now.Date.ToString, "Get List", "Crew Roster List", ex.GetBaseException.Message, AccessLogArray(3), "Web Service")
            'End If
        End Try
    End Sub
#End Region

End Class