Imports System.Text
Partial Class wfAutoInventoryList
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim mVendorListAutoComplete As VendorListAutoComplete

        'Added By Utkarsh On 14-Dec-2011
        Dim mDistinctTextAutoComplete As DistinctTextListAutoComplete
        Dim mAircraftList As AircraftListAutoComplete
        Dim mWorkShopListAutoComplete As WorkShopListAutoComplete
        Dim mStoresList As StoreListAutoComplete
        Dim TextType As String = Request.QueryString("TextType")
        'End
        Dim mEmployeeList As EmployeeListAutoComplete  'Added by Vikrant
        Dim mLocationList As LocationListAutoComplete   'Added by Vikrant

        Dim mModelList As ModelListAutoComplete
        Dim mSerialNoListAutoComplete As SerialNoListAutoComplete 'Added BY Vikrant on 8-June-2012 For All07062012
        Dim mDistinctOrderAircraftRegAutoComplete As DistinctOrderAircraftRegAutoComplete 'Added By Utkarsh On 05-Feb-2013 FOR Heligo054022013 
        Dim mDistinctModuleNameListAutoComplete As DistinctModuleNameListAutoComplete 'Added By Shweta On 11-March-2013 FOR ALL11032013 - 2
        Dim mDistinctUnusedReturnIssueToAutoComplete As DistinctUnusedReturnIssueToAutoComplete
        Dim mInternalOrderNumberListAutoComplete As InternalOrderNumberListAutoComplete 'Added by Shital on 22-Feb-2021

        Dim LookInType As String = Request.QueryString("LookInType")
        Dim CustomerID As String = Request.QueryString("CustomerID")
        Dim AssemblyTypID As Integer = Request.QueryString("AssemblyTypID")
        Dim mRelNoteNolist As ReleaseNoteNoList
        Dim PartID As String = Request.QueryString("PartID")
        Dim PreFixText As String = Request.QueryString("q")
        Dim Type As String = Request.QueryString("Type")
        Dim sb As StringBuilder = New StringBuilder

        If Type = "Customer" Then
            mVendorListAutoComplete = VendorListAutoComplete.GetVendorListAutoComplete(PreFixText, Type)
            For i As Integer = 0 To mVendorListAutoComplete.Count - 1
                sb.Append(mVendorListAutoComplete.Item(i).Name).Append(Environment.NewLine)
            Next
            Response.Write(sb.ToString)

        ElseIf Type = "Supplier" Then
            mVendorListAutoComplete = VendorListAutoComplete.GetVendorListAutoComplete(PreFixText, Type)
            For i As Integer = 0 To mVendorListAutoComplete.Count - 1
                sb.Append(mVendorListAutoComplete.Item(i).Name).Append(Environment.NewLine)
            Next
            Response.Write(sb.ToString)
        ElseIf Type = "Aircraft" Then
            mAircraftList = AircraftListAutoComplete.GetAircraftList(PreFixText)
            For i As Integer = 0 To mAircraftList.Count - 1
                sb.Append(mAircraftList.Item(i).RegNo).Append(Environment.NewLine)
            Next
            Response.Write(sb.ToString)

        ElseIf Type = "Store" Then
            If LookInType = "0" Or LookInType = "1" Or LookInType = "2" Then
                mStoresList = StoreListAutoComplete.GetStoreListByCustomerAutoComplete(LookInType, PreFixText, CustomerID)
            Else
                mStoresList = StoreListAutoComplete.GetStoreListAutoComplete(PreFixText)
            End If
            For i As Integer = 0 To mStoresList.Count - 1
                sb.Append(mStoresList.Item(i).Name).Append(Environment.NewLine)
            Next
            Response.Write(sb.ToString)
        ElseIf Type = "WorkShop" Then
            mWorkShopListAutoComplete = WorkShopListAutoComplete.GetWorkShopListAutoComplete(PreFixText)
            For i As Integer = 0 To mWorkShopListAutoComplete.Count - 1
                sb.Append(mWorkShopListAutoComplete.Item(i).WorkShop).Append(Environment.NewLine)
            Next
            Response.Write(sb.ToString)
        ElseIf Type = "Text" Then
            mDistinctTextAutoComplete = DistinctTextListAutoComplete.GetDistinctTextList(PreFixText, TextType)
            For i As Integer = 0 To mDistinctTextAutoComplete.Count - 1
                sb.Append(mDistinctTextAutoComplete.Item(i).Text).Append(Environment.NewLine)
            Next
            Response.Write(sb.ToString)
        ElseIf Type = "Employee" Then  'Added by Vikrant
            mEmployeeList = EmployeeListAutoComplete.GetEmployeeList(PreFixText)
            For i As Integer = 0 To mEmployeeList.Count - 1
                sb.Append(mEmployeeList.Item(i).Name).Append(Environment.NewLine)
            Next
            Response.Write(sb.ToString)

        ElseIf Type = "Location" Then  'Added by Vikrant
            mLocationList = LocationListAutoComplete.GetLocationList(PreFixText)
            For i As Integer = 0 To mLocationList.Count - 1
                sb.Append(mLocationList.Item(i).Name).Append(Environment.NewLine)
            Next
            Response.Write(sb.ToString)
        ElseIf Type = "Model" Then
            mModelList = ModelListAutoComplete.GetModelList(PreFixText, AssemblyTypID)
            For i As Integer = 0 To mModelList.Count - 1
                sb.Append(mModelList.Item(i).Name).Append(Environment.NewLine)
            Next
            Response.Write(sb.ToString)
        ElseIf Type = "ReleaseNoteNo" Then
            PartID = Session("PartID")
            mRelNoteNolist = ReleaseNoteNoList.GetReleaseNoteNoList(New Guid(PartID))
            For i As Integer = 0 To mRelNoteNolist.Count - 1
                sb.Append(mRelNoteNolist.Item(i).ReleaseNoteNo).Append(Environment.NewLine)
            Next
            Response.Write(sb.ToString)
        ElseIf Type = "SerialNo" Then 'Added BY Vikrant on 8-June-2012 For All07062012
            mSerialNoListAutoComplete = SerialNoListAutoComplete.GetSerialNoList(PreFixText, PartID, LookInType)
            For i As Integer = 0 To mSerialNoListAutoComplete.Count - 1
                sb.Append(mSerialNoListAutoComplete.Item(i).SerialNo).Append(Environment.NewLine)
            Next
            Response.Write(sb.ToString)
        ElseIf Type = "Vendor" Then
            mVendorListAutoComplete = VendorListAutoComplete.GetVendorListAutoComplete(PreFixText, "")
            For i As Integer = 0 To mVendorListAutoComplete.Count - 1
                sb.Append(mVendorListAutoComplete.Item(i).Name).Append(Environment.NewLine)
            Next
            Response.Write(sb.ToString)
            'Added By Utkarsh On 05-Feb-2013 FOR Heligo054022013 
        ElseIf Type = "OrderAircraftReg" Then
            mDistinctOrderAircraftRegAutoComplete = DistinctOrderAircraftRegAutoComplete.GetDistinctOrderAircraftRegList(PreFixText)
            For i As Integer = 0 To mDistinctOrderAircraftRegAutoComplete.Count - 1
                sb.Append(mDistinctOrderAircraftRegAutoComplete.Item(i).RegNo).Append(Environment.NewLine)
            Next
            Response.Write(sb.ToString)
        ElseIf Type = "ModuleName" Then 'Added By Shweta On 11-March-2013 FOR ALL11032013 - 2
            mDistinctModuleNameListAutoComplete = DistinctModuleNameListAutoComplete.GetDistinctModuleNameListAutoComplete(PreFixText)
            For i As Integer = 0 To mDistinctModuleNameListAutoComplete.Count - 1
                sb.Append(mDistinctModuleNameListAutoComplete.Item(i).DistinctModuleName).Append(Environment.NewLine)
            Next
            Response.Write(sb.ToString)
        ElseIf Type = "EmployeeLicNo" Then  'Added by Vikrant
            Dim mEmpList As EmpNoNameAutoComplete
            mEmpList = EmpNoNameAutoComplete.GeEmpNoNameList(PreFixText)
            For i As Integer = 0 To mEmpList.Count - 1
                sb.Append(mEmpList.Item(i).EmpNoName).Append(Environment.NewLine)
            Next
            Response.Write(sb.ToString)
        ElseIf Type = "EmployeeDesignation" Then  'Added by Vikrant
            Dim mEmpList As EmployeeDesignationListAutoComplete
            mEmpList = EmployeeDesignationListAutoComplete.GetEmployeeList(PreFixText)
            For i As Integer = 0 To mEmpList.Count - 1
                sb.Append(mEmpList.Item(i).Name).Append(Environment.NewLine)
            Next
            Response.Write(sb.ToString)
        ElseIf Type = "UnusedReturnIssueTo" Then
            mDistinctUnusedReturnIssueToAutoComplete = DistinctUnusedReturnIssueToAutoComplete.GetDistinctList(PreFixText)
            For i As Integer = 0 To mDistinctUnusedReturnIssueToAutoComplete.Count - 1
                sb.Append(mDistinctUnusedReturnIssueToAutoComplete.Item(i).IssueTo).Append(Environment.NewLine)
            Next
            Response.Write(sb.ToString)
        ElseIf Type = "InternalOrderNo" Then
            mInternalOrderNumberListAutoComplete = InternalOrderNumberListAutoComplete.GetIntOrderNoList(PreFixText)
            For i As Integer = 0 To mInternalOrderNumberListAutoComplete.Count - 1
                sb.Append(mInternalOrderNumberListAutoComplete.Item(i).IntOrderNo).Append(Environment.NewLine)
            Next
            Response.Write(sb.ToString)
        End If
        'End
    End Sub

End Class
