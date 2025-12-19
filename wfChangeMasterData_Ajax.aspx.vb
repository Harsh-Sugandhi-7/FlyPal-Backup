'Pls do not chnage name in Master List Combo,if Name is changed then change it entire coding.Name is concatenated everywhere insteadof Index so as to allow flexibility of order of master type in combo
Public Class wfChangeMasterData_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declartation "
    Dim mCurrencyList As CurrencyList
    Dim mNomenclatureList As NomenclatureList
    Dim mManufacturerList As ManufacturerList
    Dim mCategoryList As CategoryList
    Dim mUpdateMasterData As UpdateMasterData
    Dim mItemList As ItemList
    Dim mUnitList As UnitList
    Dim mVendorList As VendorList
    Dim mEmployeeListForCombo As EmployeeListForCombo
    Dim mTrainingList As TrainingList
    Dim mTrainingOrgList As TrainingOrgList
    Dim mWorkShopList As WorkShopList
    Dim mCompanyList As CompanyList
    Dim mCityInvList As CityInvList
    Dim mCityList As CityList
    Dim mPlaceList As PlaceList
    Dim mStoreList As StoreList
    Dim mTaskCardList As TaskCardList
    Dim mModelList As ModelList
    Dim mATAList As ATAList
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
    End Sub
    Private Sub RemoveSession()

    End Sub
    Private Sub ControlVisibility()
        lblList.Text = IIf(cmbMasterList.SelectedIndex = 0, "List", cmbMasterList.SelectedItem.ToString + " List")
        lblName.Text = IIf(cmbMasterList.SelectedItem.ToString = "ATA", "Nomenclature to be Updated", "Name to be Updated")
        cmbNoList.Visible = (cmbMasterList.SelectedIndex = 0)
        cmbCurrency.Visible = (cmbMasterList.SelectedItem.ToString = "Currency")
        cmbNomenclature.Visible = (cmbMasterList.SelectedItem.ToString = "Nomenclature")
        cmbManufacturer.Visible = (cmbMasterList.SelectedItem.ToString = "Manufacturer")
        cmbCategory.Visible = (cmbMasterList.SelectedItem.ToString = "Category")
        cmbItem.Visible = (cmbMasterList.SelectedItem.ToString = "Item")
        cmbVendor.Visible = (cmbMasterList.SelectedItem.ToString = "Vendor")
        cmbEmployee.Visible = (cmbMasterList.SelectedItem.ToString = "Employee")
        cmbUnit.Visible = (cmbMasterList.SelectedItem.ToString = "Unit")
        cmbATA.Visible = (cmbMasterList.SelectedItem.ToString = "ATA")
        cmbModel.Visible = (cmbMasterList.SelectedItem.ToString = "Model")
        cmbtaskcard.Visible = (cmbMasterList.SelectedItem.ToString = "Task Card")
        cmbStore.Visible = (cmbMasterList.SelectedItem.ToString = "Store")
        cmbPlace.Visible = (cmbMasterList.SelectedItem.ToString = "Place")
        cmbCityMain.Visible = (cmbMasterList.SelectedItem.ToString = "City Maintenance")
        cmbCityInv.Visible = (cmbMasterList.SelectedItem.ToString = "City Inventory")
        cmbCompany.Visible = (cmbMasterList.SelectedItem.ToString = "Company")
        cmbWorkshop.Visible = (cmbMasterList.SelectedItem.ToString = "Workshop")
        cmbTrainingOrg.Visible = (cmbMasterList.SelectedItem.ToString = "Training Org")
        cmbTraining.Visible = (cmbMasterList.SelectedItem.ToString = "Training")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes

                Case MsgBoxResult.No

                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "SaveSuccessCurrency" Then
                        DataFieldBind("Currency")
                        txtUpdateName.Text = ""
                        upnlDetails.Update()
                    End If
                    If MSGBoxCtrl.Sender = "SaveSuccessNomenclature" Then
                        DataFieldBind("Nomenclature")
                        txtUpdateName.Text = ""
                        upnlDetails.Update()
                    End If
                    If MSGBoxCtrl.Sender = "SaveSuccessManufacturer" Then
                        DataFieldBind("Manufacturer")
                        txtUpdateName.Text = ""
                        upnlDetails.Update()
                    End If
                    If MSGBoxCtrl.Sender = "SaveSuccessCategory" Then
                        DataFieldBind("Category")
                        txtUpdateName.Text = ""
                        upnlDetails.Update()
                    End If
                    If MSGBoxCtrl.Sender = "SaveSuccessItem" Then
                        DataFieldBind("Item")
                        txtUpdateName.Text = ""
                        upnlDetails.Update()
                    End If
                    If MSGBoxCtrl.Sender = "SaveSuccessVendor" Then
                        DataFieldBind("Vendor")
                        txtUpdateName.Text = ""
                        upnlDetails.Update()
                    End If
                    If MSGBoxCtrl.Sender = "SaveSuccessEmployee" Then
                        DataFieldBind("Employee")
                        txtUpdateName.Text = ""
                        upnlDetails.Update()
                    End If
                    If MSGBoxCtrl.Sender = "SaveSuccessUnit" Then
                        DataFieldBind("Unit")
                        txtUpdateName.Text = ""
                        upnlDetails.Update()
                    End If
                    If MSGBoxCtrl.Sender = "SaveSuccessModel" Then
                        DataFieldBind("Model")
                        txtUpdateName.Text = ""
                        upnlDetails.Update()
                    End If
                    If MSGBoxCtrl.Sender = "SaveSuccessStore" Then
                        DataFieldBind("Store")
                        txtUpdateName.Text = ""
                        upnlDetails.Update()
                    End If
                    If MSGBoxCtrl.Sender = "SaveSuccessItem" Then
                        DataFieldBind("Item")
                        txtUpdateName.Text = ""
                        upnlDetails.Update()
                    End If
                    If MSGBoxCtrl.Sender = "SaveSuccessPlace" Then
                        DataFieldBind("Place")
                        txtUpdateName.Text = ""
                        upnlDetails.Update()
                    End If
                    If MSGBoxCtrl.Sender = "SaveSuccessWorkshop" Then
                        DataFieldBind("Workshop")
                        txtUpdateName.Text = ""
                        upnlDetails.Update()
                    End If
                    If MSGBoxCtrl.Sender = "SaveSuccessCompany" Then
                        DataFieldBind("Company")
                        txtUpdateName.Text = ""
                        upnlDetails.Update()
                    End If
                    If MSGBoxCtrl.Sender = "SaveSuccessItem" Then
                        DataFieldBind("Item")
                        txtUpdateName.Text = ""
                        upnlDetails.Update()
                    End If
                    If MSGBoxCtrl.Sender = "SaveSuccessItem" Then
                        DataFieldBind("Item")
                        txtUpdateName.Text = ""
                        upnlDetails.Update()
                    End If
                    If MSGBoxCtrl.Sender = "SaveSuccessItem" Then
                        DataFieldBind("Item")
                        txtUpdateName.Text = ""
                        upnlDetails.Update()
                    End If
                    If MSGBoxCtrl.Sender = "SaveSuccessItem" Then
                        DataFieldBind("Item")
                        txtUpdateName.Text = ""
                        upnlDetails.Update()
                    End If
                    If MSGBoxCtrl.Sender = "SaveSuccessItem" Then
                        DataFieldBind("Item")
                        txtUpdateName.Text = ""
                        upnlDetails.Update()
                    End If
            End Select
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind(Optional ByVal ListName As String = "")
        If ListName = "Currency" Then
            mCurrencyList = CurrencyList.GetCurrencyList("", "", True)
            cmbCurrency.DataSource = mCurrencyList
            cmbCurrency.DataBind()
        End If
        If ListName = "Nomenclature" Then
            mNomenclatureList = NomenclatureList.GetNomenclatureList("(SELECT)")
            cmbNomenclature.DataSource = mNomenclatureList
            cmbNomenclature.DataBind()
        End If
        If ListName = "Manufacturer" Then
            mManufacturerList = ManufacturerList.GetManufacturerList("", "(SELECT)")
            cmbManufacturer.DataSource = mManufacturerList
            cmbManufacturer.DataBind()
        End If
        If ListName = "Category" Then
            mCategoryList = CategoryList.GetCategoryList("(SELECT)")
            cmbCategory.DataSource = mCategoryList
            cmbCategory.DataBind()
        End If
        If ListName = "Item" Then
            mItemList = ItemList.GetItemList(1)
            cmbItem.DataSource = mItemList
            cmbItem.DataBind()
        End If
        If ListName = "Vendor" Then
            mVendorList = VendorList.GetVendorstList(0, SelectTag:="(SELECT)")
            cmbVendor.DataSource = mVendorList
            cmbVendor.DataBind()
        End If
        If ListName = "Employee" Then
            mEmployeeListForCombo = EmployeeListForCombo.GetEmployeeListForCombo("(SELECT)")
            cmbEmployee.DataSource = mEmployeeListForCombo
            cmbEmployee.DataBind()
        End If
        If ListName = "Unit" Then
            mUnitList = UnitList.GetUnitList(True)
            cmbUnit.DataSource = mUnitList
            cmbUnit.DataBind()
        End If
        If ListName = "Model" Then
            mModelList = ModelList.GetModelList(IsSelectTagRequired:=ModelList.IsSelectTagRequired.True)
            cmbModel.DataSource = mModelList
            cmbModel.DataBind()
        End If
        If ListName = "Store" Then
            mStoreList = StoreList.GetStoreList(0, , True)
            cmbStore.DataSource = mStoreList
            cmbStore.DataBind()
        End If
        If ListName = "Place" Then
            mPlaceList = PlaceList.GetPlaceList(AddTopItem:="(SELECT)")
            cmbPlace.DataSource = mPlaceList
            cmbPlace.DataBind()
        End If
        If ListName = "ATA" Then
            mATAList = ATAList.GetATAList(AddTopItem:="(SELECT)")
            cmbATA.DataSource = mATAList
            cmbATA.DataBind()
        End If
        If ListName = "Workshop" Then
            mWorkShopList = WorkShopList.GetWorkShopList(0, TagText:="(SELECT)")
            cmbWorkshop.DataSource = mWorkShopList
            cmbWorkshop.DataBind()
        End If
        If ListName = "Company" Then
            mCompanyList = CompanyList.GetCompanyList(IsTagRequired:=True, Tag:="(SELECT)")
            cmbCompany.DataSource = mCompanyList
            cmbCompany.DataBind()
        End If
        If ListName = "Training" Then
            mTrainingList = TrainingList.GetTrainingList(, , , "(SELECT)")
            cmbTraining.DataSource = mTrainingList
            cmbTraining.DataBind()
        End If
        If ListName = "Training Org" Then
            mTrainingOrgList = TrainingOrgList.GetTrainingOrgList(, , , "(SELECT)")
            cmbTrainingOrg.DataSource = mTrainingOrgList
            cmbTrainingOrg.DataBind()
        End If
        If ListName = "Task Card" Then
            mTaskCardList = TaskCardList.GetTaskCardList(, "(SELECT)")
            cmbtaskcard.DataSource = mTaskCardList
            cmbtaskcard.DataBind()
        End If
        If ListName = "City Maintenance" Then
            mCityList = CityList.GetCityList(, "(SELECT)")
            cmbCityMain.DataSource = mCityList
            cmbCityMain.DataBind()
        End If
        If ListName = "City Inventory" Then
            mCityInvList = CityInvList.GetCityList(0, , , True)
            cmbCityInv.DataSource = mCityInvList
            cmbCityInv.DataBind()
        End If
        If ListName = "Unit" Then
            mUnitList = UnitList.GetUnitList(True)
            cmbUnit.DataSource = mUnitList
            cmbUnit.DataBind()
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            DataBind()
            ControlVisibility()
        End If
    End Sub
    Private Sub cmbMasterList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbMasterList.SelectedIndexChanged
        DataFieldBind(cmbMasterList.SelectedItem.ToString)
        'Select Case cmbMasterList.SelectedItem.ToString
        '    Case "Currency"
        '        DataFieldBind("Currency")
        '    Case "Nomenclature"
        '        DataFieldBind("Nomenclature")
        '    Case "Manufacturer"
        '        DataFieldBind("Manufacturer")
        '    Case "Category"
        '        DataFieldBind("Category")
        '    Case "Item"
        '        DataFieldBind("Item")
        '    Case "Vendor"
        '        DataFieldBind("Vendor")
        '    Case "Employee"
        '        DataFieldBind("Employee")
        '    Case "Unit"
        '        DataFieldBind("Unit")
        '    Case "Model"
        '        DataFieldBind("Model")
        '    Case "Store"
        '        DataFieldBind("Store")
        '    Case "Place"
        '        DataFieldBind("Place")
        '    Case "WorkShop"
        '        DataFieldBind("WorkShop")
        '    Case "ATA"
        '        DataFieldBind("ATA")
        '    Case "Company"
        '        DataFieldBind("Company")
        '    Case "Item"
        '        DataFieldBind("Item")
        '    Case "Item"
        '        DataFieldBind("Item")
        '    Case "Item"
        '        DataFieldBind("Item")
        '    Case "Item"
        '        DataFieldBind("Item")
        '    Case "Item"
        '        DataFieldBind("Item")
        'End Select
        ControlVisibility()
        txtUpdateName.Text = ""
        upnlDetails.Update()
    End Sub
    Private Sub btnUpdate_Click(sender As Object, e As System.EventArgs) Handles btnUpdate.Click
        Select Case cmbMasterList.SelectedItem.ToString
            Case "Currency"
                Try
                    Dim mCurrency As Currency
                    mCurrency = Currency.GetCurrency(New Guid(cmbCurrency.SelectedValue))
                    mCurrency.Name = txtUpdateName.Text.Trim
                    If mCurrency.IsValid Then
                        mCurrency.Save()
                        MarkLog(Util.Action.Save, "ChangeMasterData", "Master:Currency, Old Currency Name:" + cmbCurrency.SelectedItem.ToString + ", Changed Currency Name:" + txtUpdateName.Text.Trim + ", Changed By:" + User.Identity.Name, Util.ErrorType.NoError, mCurrency.ID, EventLogID)
                    Else
                        MSGBoxCtrl.show("Error!", "Record Updation..!!", mCurrency.GetBrokenRulesString, MsgBoxStyle.OkOnly, "CurrencyError")
                        Exit Sub
                    End If
                Catch ex As SqlException
                    If ex.Number = 8145 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    ElseIf ex.Number = 2627 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    ElseIf ex.Number = 547 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    End If
                Catch ex As Exception
                    MSGBoxCtrl.show("Error!", "Record Updation..!!", ex.Message, MsgBoxStyle.OkOnly, "CurrencyError")
                    Exit Sub
                End Try
            Case "Nomenclature"
                Try
                    Dim mNomenClature As NomenClature
                    mNomenClature = NomenClature.GetNomenclature(New Guid(cmbNomenclature.SelectedValue))
                    mNomenClature.Name = txtUpdateName.Text.Trim
                    If mNomenClature.IsValid Then
                        mNomenClature.Save()
                        MarkLog(Util.Action.Save, "ChangeMasterData", "Master:NomenClature, Old NomenClature Name:" + cmbNomenclature.SelectedItem.ToString + ", Changed NomenClature Name:" + txtUpdateName.Text.Trim + ", Changed By:" + User.Identity.Name, Util.ErrorType.NoError, mNomenClature.ID, EventLogID)
                    Else
                        MSGBoxCtrl.show("Error!", "Record Updation..!!", mNomenClature.GetBrokenRulesString, MsgBoxStyle.OkOnly, "NomenClatureError")
                        Exit Sub
                    End If
                Catch ex As SqlException
                    If ex.Number = 8145 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    ElseIf ex.Number = 2627 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    ElseIf ex.Number = 547 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    End If
                Catch ex As Exception
                    MSGBoxCtrl.show("Error!", "Record Updation..!!", ex.Message, MsgBoxStyle.OkOnly, "NomenClatureError")
                    Exit Sub
                End Try
            Case "Manufacturer"
                Try
                    Dim mManufacturer As Manufacturer
                    mManufacturer = Manufacturer.GetManufacturer(New Guid(cmbManufacturer.SelectedValue))
                    mManufacturer.Name = txtUpdateName.Text.Trim
                    If mManufacturer.IsValid Then
                        mManufacturer.Save()
                        MarkLog(Util.Action.Save, "ChangeMasterData", "Master:Manufacturer, Old Manufacturer Name:" + cmbManufacturer.SelectedItem.ToString + ", Changed Manufacturer Name:" + txtUpdateName.Text.Trim + ", Changed By:" + User.Identity.Name, Util.ErrorType.NoError, mManufacturer.ID, EventLogID)
                    Else
                        MSGBoxCtrl.show("Error!", "Record Updation..!!", mManufacturer.GetBrokenRulesString, MsgBoxStyle.OkOnly, "ManufacturerError")
                        Exit Sub
                    End If
                Catch ex As SqlException
                    If ex.Number = 8145 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    ElseIf ex.Number = 2627 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    ElseIf ex.Number = 547 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    End If
                Catch ex As Exception
                    MSGBoxCtrl.show("Error!", "Record Updation..!!", ex.Message, MsgBoxStyle.OkOnly, "ManufacturerError")
                    Exit Sub
                End Try
            Case "Category"
                Try
                    Dim mCategory As Category
                    mCategory = Category.GetCategory(New Guid(cmbCategory.SelectedValue))
                    mCategory.Name = txtUpdateName.Text.Trim
                    If mCategory.IsValid Then
                        mCategory.Save()
                        MarkLog(Util.Action.Save, "ChangeMasterData", "Master:Category, Old Category Name:" + cmbCategory.SelectedItem.ToString + ", Changed Category Name:" + txtUpdateName.Text.Trim + ", Changed By:" + User.Identity.Name, Util.ErrorType.NoError, mCategory.ID, EventLogID)
                    Else
                        MSGBoxCtrl.show("Error!", "Record Updation..!!", mCategory.GetBrokenRulesString, MsgBoxStyle.OkOnly, "CategoryError")
                        Exit Sub
                    End If
                Catch ex As SqlException
                    If ex.Number = 8145 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    ElseIf ex.Number = 2627 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    ElseIf ex.Number = 547 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    End If
                Catch ex As Exception
                    MSGBoxCtrl.show("Error!", "Record Updation..!!", ex.Message, MsgBoxStyle.OkOnly, "CategoryError")
                    Exit Sub
                End Try
            Case "Item"
                Try
                    Dim mUpdate As New UpdateMasterData
                    mUpdate.Update("Item", New Guid(cmbItem.SelectedValue), txtUpdateName.Text.Trim)
                    ' MarkLog(Util.Action.Save, "ChangeMasterData", "22", Util.ErrorType.NoError, MCategory.ID, EventLogID)
                    '"Master:Item, Old Name:" + cmbItem.SelectedItem.ToString + " Changed Name:" + txtUpdateName.Text.Trim + " Changed By:" + User.Identity.Name

                Catch ex As SqlException
                    If ex.Number = 8145 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2627 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2601 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 547 Then

                        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    End If
                    Exit Sub
                Finally
                    MarkLog(Util.Action.Save, "ChangeMasterData", "Master:Item, Old Name:" + cmbItem.SelectedItem.ToString + " Changed Name:" + txtUpdateName.Text.Trim + " Changed By: " + User.Identity.Name, ErrorType.NoError, Guid.Empty, EventLogID)

                End Try
            Case "Vendor"
                Try
                    Dim mUpdate As New UpdateMasterData
                    mUpdate.Update("Vendor", New Guid(cmbVendor.SelectedValue), txtUpdateName.Text.Trim)
                Catch ex As SqlException
                    If ex.Number = 8145 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2627 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2601 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 547 Then

                        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    End If
                    Exit Sub
                Finally
                    MarkLog(Util.Action.Save, "ChangeMasterData", "Master:Vendor, Old Name:" + cmbVendor.SelectedItem.ToString + " Changed Name:" + txtUpdateName.Text.Trim + " Changed By: " + User.Identity.Name, ErrorType.NoError, Guid.Empty, EventLogID)

                End Try
            Case "Employee"
                Try
                    Dim mUpdate As New UpdateMasterData
                    mUpdate.Update("Employee", New Guid(cmbEmployee.SelectedValue), txtUpdateName.Text.Trim)
                Catch ex As SqlException
                    If ex.Number = 8145 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2627 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2601 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 547 Then

                        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    End If
                    Exit Sub
                Finally
                    MarkLog(Util.Action.Save, "ChangeMasterData", "Master:Employee, Old Name:" + cmbEmployee.SelectedItem.ToString + " Changed Name:" + txtUpdateName.Text.Trim + " Changed By: " + User.Identity.Name, ErrorType.NoError, Guid.Empty, EventLogID)

                End Try
            Case "Unit"
                Try
                    Dim mUpdate As New UpdateMasterData
                    mUpdate.Update("Unit", New Guid(cmbUnit.SelectedValue), txtUpdateName.Text.Trim)
                Catch ex As SqlException
                    If ex.Number = 8145 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2627 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2601 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 547 Then

                        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    End If
                    Exit Sub
                Finally
                    MarkLog(Util.Action.Save, "ChangeMasterData", "Master:Unit, Old Name:" + cmbUnit.SelectedItem.ToString + " Changed Name:" + txtUpdateName.Text.Trim + " Changed By: " + User.Identity.Name, ErrorType.NoError, Guid.Empty, EventLogID)

                End Try
            Case "Model"
                Try
                    Dim mUpdate As New UpdateMasterData
                    mUpdate.Update("Model", New Guid(cmbModel.SelectedValue), txtUpdateName.Text.Trim)
                Catch ex As SqlException
                    If ex.Number = 8145 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2627 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2601 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 547 Then

                        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    End If
                    Exit Sub
                Finally
                    MarkLog(Util.Action.Save, "ChangeMasterData", "Master:Model, Old Name:" + cmbModel.SelectedItem.ToString + " Changed Name:" + txtUpdateName.Text.Trim + " Changed By: " + User.Identity.Name, ErrorType.NoError, Guid.Empty, EventLogID)

                End Try
            Case "Place"
                Try
                    Dim mUpdate As New UpdateMasterData
                    mUpdate.Update("Place", New Guid(cmbPlace.SelectedValue), txtUpdateName.Text.Trim)
                Catch ex As SqlException
                    If ex.Number = 8145 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2627 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2601 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 547 Then

                        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    End If
                    Exit Sub
                Finally
                    MarkLog(Util.Action.Save, "ChangeMasterData", "Master:Place, Old Name:" + cmbPlace.SelectedItem.ToString + " Changed Name:" + txtUpdateName.Text.Trim + " Changed By: " + User.Identity.Name, ErrorType.NoError, Guid.Empty, EventLogID)

                End Try
            Case "Store"
                Try
                    Dim mUpdate As New UpdateMasterData
                    mUpdate.Update("Store", New Guid(cmbStore.SelectedValue), txtUpdateName.Text.Trim)
                Catch ex As SqlException
                    If ex.Number = 8145 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2627 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2601 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 547 Then

                        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    End If
                    Exit Sub
                Finally
                    MarkLog(Util.Action.Save, "ChangeMasterData", "Master:Store, Old Name:" + cmbStore.SelectedItem.ToString + " Changed Name:" + txtUpdateName.Text.Trim + " Changed By: " + User.Identity.Name, ErrorType.NoError, Guid.Empty, EventLogID)

                End Try
            Case "Workshop"
                Try
                    Dim mUpdate As New UpdateMasterData
                    mUpdate.Update("Workshop", New Guid(cmbWorkshop.SelectedValue), txtUpdateName.Text.Trim)
                Catch ex As SqlException
                    If ex.Number = 8145 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2627 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2601 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 547 Then

                        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    End If
                    Exit Sub
                Finally
                    MarkLog(Util.Action.Save, "ChangeMasterData", "Master:Workshop, Old Name:" + cmbWorkshop.SelectedItem.ToString + " Changed Name:" + txtUpdateName.Text.Trim + " Changed By: " + User.Identity.Name, ErrorType.NoError, Guid.Empty, EventLogID)

                End Try
            Case "Company"
                Try
                    Dim mUpdate As New UpdateMasterData
                    mUpdate.Update("Company", New Guid(cmbCompany.SelectedValue), txtUpdateName.Text.Trim)
                Catch ex As SqlException
                    If ex.Number = 8145 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2627 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2601 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 547 Then

                        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    End If
                    Exit Sub
                Finally
                    MarkLog(Util.Action.Save, "ChangeMasterData", "Master:Company, Old Name:" + cmbCompany.SelectedItem.ToString + " Changed Name:" + txtUpdateName.Text.Trim + " Changed By: " + User.Identity.Name, ErrorType.NoError, Guid.Empty, EventLogID)

                End Try
            Case "ATA"
                Try
                    Dim mUpdate As New UpdateMasterData
                    mUpdate.Update("ATA", New Guid(cmbATA.SelectedValue), txtUpdateName.Text.Trim)
                Catch ex As SqlException
                    If ex.Message.Contains("UKtabATAATANomenclature") Then
                        MSGBoxCtrl.show("Error!", "Record Updation..!!", "Entered ATA Nomenclature already exist.Please enter different Nomenclature to update.", MsgBoxStyle.OkOnly, "ATAError")
                        Exit Sub
                    ElseIf ex.Message.Contains("CCtabATAATANomenclature") Then
                        MSGBoxCtrl.show("Error!", "Record Updation..!!", "ATA Nomenclature can not be blank.Please enter Nomenclature to update.", MsgBoxStyle.OkOnly, "ATAError")
                        Exit Sub
                    Else
                        MSGBoxCtrl.show("Error!", "Record Updation..!!", ex.Message, MsgBoxStyle.OkOnly, "ATAError")
                        Exit Sub
                    End If
                Finally
                    MarkLog(Util.Action.Save, "ChangeMasterData", "Master:ATA, Old Name:" + cmbATA.SelectedItem.ToString + " Changed Name:" + txtUpdateName.Text.Trim + " Changed By: " + User.Identity.Name, ErrorType.NoError, Guid.Empty, EventLogID)

                End Try
            Case "Training"
                Try
                    Dim mUpdate As New UpdateMasterData
                    mUpdate.Update("Training", New Guid(cmbTraining.SelectedValue), txtUpdateName.Text.Trim)
                Catch ex As SqlException
                    If ex.Number = 8145 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2627 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2601 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 547 Then

                        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    End If
                Finally
                    MarkLog(Util.Action.Save, "ChangeMasterData", "Master:Training, Old Name:" + cmbTraining.SelectedItem.ToString + " Changed Name:" + txtUpdateName.Text.Trim + " Changed By: " + User.Identity.Name, ErrorType.NoError, Guid.Empty, EventLogID)

                End Try
            Case "Training Org"
                Try
                    Dim mUpdate As New UpdateMasterData
                    mUpdate.Update("Training Org", New Guid(cmbTrainingOrg.SelectedValue), txtUpdateName.Text.Trim)
                Catch ex As SqlException
                    If ex.Number = 8145 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2627 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2601 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 547 Then

                        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    End If
                    Exit Sub
                Finally
                    MarkLog(Util.Action.Save, "ChangeMasterData", "Master:Training Org, Old Name:" + cmbTrainingOrg.SelectedItem.ToString + " Changed Name:" + txtUpdateName.Text.Trim + " Changed By: " + User.Identity.Name, ErrorType.NoError, Guid.Empty, EventLogID)

                End Try

            Case "Task Card"
                Try
                    Dim mUpdate As New UpdateMasterData
                    mUpdate.Update("Task Card", New Guid(cmbTaskCard.SelectedValue), txtUpdateName.Text.Trim)
                Catch ex As SqlException
                    If ex.Number = 8145 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2627 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2601 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 547 Then

                        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    End If
                    Exit Sub
                Finally
                    MarkLog(Util.Action.Save, "ChangeMasterData", "Master:Task Card, Old Name:" + cmbTaskCard.SelectedItem.ToString + " Changed Name:" + txtUpdateName.Text.Trim + " Changed By: " + User.Identity.Name, ErrorType.NoError, Guid.Empty, EventLogID)

                End Try

            Case "City Maintenance"
                Try
                    Dim mUpdate As New UpdateMasterData
                    mUpdate.Update("City Maintenance", New Guid(cmbCityMain.SelectedValue), txtUpdateName.Text.Trim)
                Catch ex As SqlException
                    If ex.Number = 8145 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2627 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2601 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 547 Then

                        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    End If
                    Exit Sub
                Finally
                    MarkLog(Util.Action.Save, "ChangeMasterData", "Master:City Maintenance, Old Name:" + cmbCityMain.SelectedItem.ToString + " Changed Name:" + txtUpdateName.Text.Trim + " Changed By: " + User.Identity.Name, ErrorType.NoError, Guid.Empty, EventLogID)

                End Try
            Case "City Inventory"
                Try
                    Dim mUpdate As New UpdateMasterData
                    mUpdate.Update("City Inventory", New Guid(cmbCityInv.SelectedValue), txtUpdateName.Text.Trim)
                Catch ex As SqlException
                    If ex.Number = 8145 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2627 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 2601 Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ElseIf ex.Number = 547 Then

                        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    End If
                    Exit Sub
                Finally
                    MarkLog(Util.Action.Save, "ChangeMasterData", "Master:City Inventory, Old Name:" + cmbCityInv.SelectedItem.ToString + " Changed Name:" + txtUpdateName.Text.Trim + " Changed By: " + User.Identity.Name, ErrorType.NoError, Guid.Empty, EventLogID)

                End Try
        End Select
        MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "") ' "SaveSuccess" + cmbMasterList.SelectedItem.ToString)
        DataFieldBind(cmbMasterList.SelectedItem.ToString)
        ControlVisibility()
        txtUpdateName.Text = ""
        upnlDetails.Update()
    End Sub
    Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "ChangeMasterData", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("index.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region





End Class