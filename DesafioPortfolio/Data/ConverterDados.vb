Public Class ConverterDados

    Public Shared Function ConverteDataTableEmLista(Of T)(dt As DataTable, mapper As Func(Of DataRow, T)) As List(Of T)
        Return dt.AsEnumerable().Select(Function(row) mapper(row)).ToList()
    End Function

    Public Shared Function ConverteDataTableEmObjeto(Of T)(dt As DataTable, mapper As Func(Of DataRow, T)) As T
        If dt.Rows.Count > 0 Then
            Return mapper(dt.Rows(0))
        Else
            Return Nothing
        End If
    End Function

End Class
