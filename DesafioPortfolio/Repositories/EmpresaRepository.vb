Imports System.Data.SqlClient

Public Class EmpresaRepository

    Public Shared Function SelecionarPorId(id As Integer) As Empresa

        Dim sql As String = $"SELECT id, nome, cnpj FROM Empresas WHERE id = {id}"
        Dim dt = AppDataBase.Selecionar(sql)

        If dt.Rows.Count > 0 Then
            Dim empresa As Empresa = ConverterDados.ConverteDataTableEmObjeto(dt, Function(row) New Empresa With {
            .Id = row.Field(Of Integer)("Id"),
            .Nome = row.Field(Of String)("Nome"),
            .CNPJ = row.Field(Of String)("CNPJ")
        })

            Return empresa
        Else
            Return Nothing
        End If

    End Function

    Public Shared Function SelecionarTodos() As List(Of Empresa)

        Dim sql As String = $"SELECT id, nome, cnpj FROM Empresas"
        Dim dt = AppDataBase.Selecionar(sql)

        Dim empresas As List(Of Empresa) = ConverterDados.ConverteDataTableEmLista(dt, Function(row) New Empresa With {
                .Id = row.Field(Of Integer)("Id"),
                .Nome = row.Field(Of String)("Nome"),
                .CNPJ = row.Field(Of String)("CNPJ")
            })

        Return empresas

    End Function

    Public Shared Function SelecionarPorFiltro(filtro As TipoFiltro, valor As String) As List(Of Empresa)

        Dim consulta As String = ""
        Dim sql As String = $"SELECT id, nome, cnpj FROM Empresas"

        Select Case filtro
            Case TipoFiltro.CNPJ
                consulta = sql & $" WHERE CNPJ = '{valor}'"
            Case TipoFiltro.Nome
                consulta = sql & $" WHERE Nome LIKE '%{valor}%'"
            Case TipoFiltro.ID
                consulta = sql & $" WHERE ID = {CInt(valor)}"
            Case TipoFiltro.Todos
                consulta = sql
        End Select

        Dim dt = AppDataBase.Selecionar(consulta)

        Dim empresas As List(Of Empresa) = ConverterDados.ConverteDataTableEmLista(dt, Function(row) New Empresa With {
                .Id = row.Field(Of Integer)("Id"),
                .Nome = row.Field(Of String)("Nome"),
                .CNPJ = row.Field(Of String)("CNPJ")
            })

        Return empresas

    End Function

    Public Shared Function InserirEmpresa(nome As String, cnpj As String) As Integer

        Dim sql As String = $"INSERT INTO Empresas (Nome, CNPJ) VALUES (@Nome, @CNPJ)"

        Dim parametros As SqlParameter() = {
            New SqlParameter("@Nome", nome),
            New SqlParameter("@CNPJ", cnpj)
        }

        Dim id As Integer = AppDataBase.Inserir(sql, parametros)

        Return id

    End Function

    Public Shared Function InserirEmpresasAssociados(associadoId As Integer, empresaId As Integer) As Integer

        Dim sql As String = $"INSERT INTO AssociadosEmpresas (AssociadoId, EmpresaId) VALUES (@AssociadoId, @EmpresaId)"

        Dim parametros As SqlParameter() = {
            New SqlParameter("@AssociadoId", associadoId),
            New SqlParameter("@EmpresaId", empresaId)
        }

        Dim id As Integer = AppDataBase.Inserir(sql, parametros)

        Return id

    End Function

    Public Shared Function AlterarEmpresa(nome As String, idEmpresa As Integer) As Boolean

        Dim sql As String = $"UPDATE Empresas SET Nome = @Nome WHERE Id = @Id"

        Dim parametros As SqlParameter() = {
            New SqlParameter("@Nome", nome),
            New SqlParameter("@Id", idEmpresa)
        }

        Dim resultado As Boolean = AppDataBase.Atualizar(sql, parametros)

        Return resultado

    End Function

    Public Shared Function SelecionarEmpresasAssociados(idAssociado As String) As List(Of Empresa)

        Dim sql As String = $"SELECT e.Id, e.Nome, e.CNPJ FROM Empresas e INNER JOIN AssociadosEmpresas ae ON e.Id = ae.EmpresaId " &
                            $"WHERE ae.AssociadoId = {idAssociado}"
        Dim dt = AppDataBase.Selecionar(sql)

        Dim empresas As List(Of Empresa) = ConverterDados.ConverteDataTableEmLista(dt, Function(row) New Empresa With {
                .Id = row.Field(Of Integer)("Id"),
                .Nome = row.Field(Of String)("Nome"),
                .CNPJ = row.Field(Of String)("CNPJ")
            })

        Return empresas

    End Function

    Public Shared Function DeletarEmpresa(idEmpresa As Integer) As Boolean

        Dim sql As String = $"DELETE FROM Empresas WHERE Id = @EmpresaId"

        Dim parametros As SqlParameter() = {
            New SqlParameter("@EmpresaId", idEmpresa)
        }

        Dim resultado As Boolean = AppDataBase.Deletar(sql, parametros)

        Return resultado

    End Function

    Public Shared Function DeletarAssociadosEmpresa(idEmpresa As Integer) As Boolean

        Dim sql As String = $"DELETE FROM AssociadosEmpresas WHERE EmpresaId = @EmpresaId"

        Dim parametros As SqlParameter() = {
            New SqlParameter("@EmpresaId", idEmpresa)
        }

        Dim resultado As Boolean = AppDataBase.Deletar(sql, parametros)

        Return resultado

    End Function

    Public Shared Function DeletarAssociadoDeUmaEmpresa(idAssociado As Integer, idEmpresa As Integer) As Boolean

        Dim sql As String = $"DELETE FROM AssociadosEmpresas WHERE AssociadoId = @AssociadoId AND EmpresaId = @EmpresaId"

        Dim parametros As SqlParameter() = {
            New SqlParameter("@AssociadoId", idAssociado),
            New SqlParameter("@EmpresaId", idEmpresa)
        }

        Dim resultado As Boolean = AppDataBase.Deletar(sql, parametros)

        Return resultado

    End Function

    Public Enum TipoFiltro
        CNPJ
        Nome
        ID
        Todos
    End Enum

End Class
