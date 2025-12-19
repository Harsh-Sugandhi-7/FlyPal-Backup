'************************************
'Created by:   Harsh Sugandhi
'Created on:   28th April 2025
'Created for:  FLYPAL-2360 Helper Methods to use the DataBinding logic of Grid View for API as well.
'************************************


Imports System.Collections.Generic


Public Class LogParameterListHelper

	Public Function GetLogParameterList(_AssemblyParameterListForAssemblyStatus As AssemblyParameterListForAssemblyStatus,
										_Log As Log,
										_LogParameterList As LogParameters) As (DataSource As LogParameters,
																				Columns As List(Of TemplateColumn),
																				mLog As Log)

		Dim dynamicColumns As New List(Of TemplateColumn)()

		Try


			If _AssemblyParameterListForAssemblyStatus.Count >= 0 Then

				Dim a As Integer
				For a = 0 To _AssemblyParameterListForAssemblyStatus.Count - 1

					If _Log.LogParameters.Contains(_AssemblyParameterListForAssemblyStatus(a).ParameterID,
												   _AssemblyParameterListForAssemblyStatus(a).AssemblyID) Then
						' Do Nothing
					Else

						_Log.LogParameters.Add(_Log.ID,
											   _AssemblyParameterListForAssemblyStatus(a).ParameterID,
											   _AssemblyParameterListForAssemblyStatus(a).AssemblyID,
											   _AssemblyParameterListForAssemblyStatus(a).AssemblyInfo,
											   _AssemblyParameterListForAssemblyStatus(a).AssemblyTypeName,
											   _AssemblyParameterListForAssemblyStatus(a).MinValue,
											   _AssemblyParameterListForAssemblyStatus(a).MaxValue)

					End If

				Next

				Dim i As Integer

				For i = 0 To _AssemblyParameterListForAssemblyStatus.Count - 1

					If (_LogParameterList IsNot Nothing AndAlso
						_LogParameterList.Contains(_AssemblyParameterListForAssemblyStatus(i).ParameterID)) Then
						' Do Nothing
					Else

						If _LogParameterList Is Nothing Then _LogParameterList = LogParameters.NewLogParameters

						_LogParameterList.Add(_Log.ID,
											  _AssemblyParameterListForAssemblyStatus(i).ParameterID,
											  _AssemblyParameterListForAssemblyStatus(i).AssemblyID,
											  _AssemblyParameterListForAssemblyStatus(i).AssemblyInfo,
											  _AssemblyParameterListForAssemblyStatus(i).AssemblyTypeName,
											  _AssemblyParameterListForAssemblyStatus(i).MinValue,
											  _AssemblyParameterListForAssemblyStatus(i).MaxValue)

					End If

				Next

				Dim j As Integer
				Dim k As Integer = 0
				Dim AssemblyID As Guid = Guid.Empty

				For j = 0 To _AssemblyParameterListForAssemblyStatus.Count - 1

					If Not AssemblyID.Equals(_AssemblyParameterListForAssemblyStatus(j).AssemblyID) Then

						'TemplateColumn
						Dim column As New TemplateColumn
						Dim ParameterValue As Decimal = 0D

						If _Log.LogParameters.Contains(_AssemblyParameterListForAssemblyStatus(j).ParameterID,
													   _AssemblyParameterListForAssemblyStatus(j).AssemblyID) Then

							ParameterValue = _Log.LogParameters(_AssemblyParameterListForAssemblyStatus(j).ParameterID,
																_AssemblyParameterListForAssemblyStatus(j).AssemblyID).ParameterValue

						End If

						column.HeaderText = _AssemblyParameterListForAssemblyStatus(j).ModelName & " " &
											_AssemblyParameterListForAssemblyStatus(j).SerialNo & " " &
											_AssemblyParameterListForAssemblyStatus(j).Position

						column.ItemTemplate = New GridViewTemplateColumn(ListItemType.Item,
																		 _AssemblyParameterListForAssemblyStatus(j).AssemblyInfo,
																		 "parameter" & k,
																		 ParameterValue,
																		 "AssemblyID" & k,
																		 _AssemblyParameterListForAssemblyStatus(j).AssemblyID.ToString,
																		 "clsTextBoxTagSearchRightAlignQty_Ajax")
						column.HeaderStyle.Wrap = True

						dynamicColumns.Add(column)

						AssemblyID = _AssemblyParameterListForAssemblyStatus(j).AssemblyID

						k = k + 1

					End If

				Next

			End If

			Return (_LogParameterList, dynamicColumns, _Log)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function BuildUnifiedGridRows(staticRowData As LogParameters,
										 dynamicRowData As List(Of TemplateColumn),
										 _AssemblyParameterListForAssemblyStatus As AssemblyParameterListForAssemblyStatus,
										 _Log As Log) As List(Of Dictionary(Of String, Object))

		Dim unifiedRows As New List(Of Dictionary(Of String, Object))

		Try

			For Each staticData As LogParameter In staticRowData

				Dim rowData As New Dictionary(Of String, Object)

				' Static Fields
				rowData("mID") = staticData.ID
				rowData("mLogID") = staticData.LogID
				rowData("mParameterID") = staticData.ParameterID
				rowData("mParameterValue") = staticData.ParameterValue
				rowData("mParameterName") = staticData.ParameterName
				rowData("mParameterDescription") = staticData.ParameterDescription
				rowData("mAssemblyName") = staticData.AssemblyName
				rowData("mAssemblyTypeName") = staticData.AssemblyTypeName
				rowData("mAssemblyID") = staticData.AssemblyID
				rowData("mMaxValue") = staticData.MaxValue
				rowData("mMinValue") = staticData.MinValue

				' Dynamic Fields
				For Each dynamicData As TemplateColumn In dynamicRowData

					Dim _ItemTemplate = CType(dynamicData.ItemTemplate, GridViewTemplateColumn)
					Dim dynamicDataAssemblyID As New Guid(CType(_ItemTemplate, GridViewTemplateColumn).mAssemblyID.ToString)
					Dim staticDataParameterID As Guid = staticData.ParameterID

					If _AssemblyParameterListForAssemblyStatus.Contains(staticDataParameterID, dynamicDataAssemblyID) Then

						If _Log.LogParameters.Contains(staticDataParameterID, dynamicDataAssemblyID) Then
							rowData(dynamicData.HeaderText) = _Log.LogParameters(staticDataParameterID, dynamicDataAssemblyID).ParameterValue
						End If

					Else
						rowData(dynamicData.HeaderText) = 0
					End If

				Next

				rowData("mIsNew") = staticData.IsNew
				rowData("mIsDeleted") = staticData.IsDeleted
				rowData("mIsDirty") = staticData.IsDirty

				unifiedRows.Add(rowData)

			Next

			Return unifiedRows

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

End Class
