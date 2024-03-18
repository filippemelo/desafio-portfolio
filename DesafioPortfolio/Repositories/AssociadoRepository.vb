Imports System.Data.SqlClient
Imports DesafioPortfolio.EmpresaRepository

Public Class AssociadoRepository

    Public Shared Function SelecionarPorId(id As Integer) As Associado

        Dim sql As String = $"SELECT Id, Nome, CPF, DataNascimento FROM Associados WHERE id = {id}"
        Dim dt = AppDataBase.Selecionar(sql)

        If dt.Rows.Count > 0 Then
            Dim associado As Associado = ConverterDados.ConverteDataTableEmObjeto(dt, Function(row) New Associado With {
                .Id = row.Field(Of Integer)("Id"),
                .Nome = row.Field(Of String)("Nome"),
                .CPF = row.Field(Of String)("CPF"),
                .DataNascimento = row.Field(Of DateTime)("DataNascimento")
            })

            Return associado
        Else
            Return Nothing
        End If

    End Function

    Public Shared Function SelecionarTodos() As List(Of Associado)

        Dim sql As String = $"SELECT Id, Nome, Cpf, DataNascimento FROM Associados"
        Dim dt = AppDataBase.Selecionar(sql)

        Dim associados As List(Of Associado) = ConverterDados.ConverteDataTableEmLista(dt, Function(row) New Associado With {
                .Id = row.Field(Of Integer)("Id"),
                .Nome = row.Field(Of String)("Nome"),
                .CPF = row.Field(Of String)("CPF"),
                .DataNascimento = row.Field(Of DateTime)("DataNascimento")
            })

        Return associados

    End Function

    Public Shared Function SelecionarAssociadosEmpresa(idEmpresa As String) As List(Of Associado)

        Dim sql As String = $"SELECT a.Id, a.Nome, a.CPF, a.DataNascimento FROM Associados a INNER JOIN AssociadosEmpresas ae ON a.Id = ae.AssociadoId " &
                            $"WHERE ae.EmpresaId = {idEmpresa}"
        Dim dt = AppDataBase.Selecionar(sql)

        Dim associados As List(Of Associado) = ConverterDados.ConverteDataTableEmLista(dt, Function(row) New Associado With {
                .Id = row.Field(Of Integer)("Id"),
                .Nome = row.Field(Of String)("Nome"),
                .CPF = row.Field(Of String)("CPF"),
                .DataNascimento = row.Field(Of DateTime)("DataNascimento")
            })

        Return associados

    End Function

    Public Shared Function InserirAssociado(nome As String, cpf As String, dataNascimento As Date) As Integer

        Dim sql As String = $"INSERT INTO Associados (Nome, CPF, DataNascimento) VALUES (@Nome, @CPF, @DataNascimento)"

        Dim parametros As SqlParameter() = {
            New SqlParameter("@Nome", nome),
            New SqlParameter("@CPF", cpf),
            New SqlParameter("@DataNascimento", dataNascimento)
        }

        Dim id As Integer = AppDataBase.Inserir(sql, parametros)

        Return id

    End Function

    Public Shared Function AlterarAssociado(nome As String, dataNascimento As Date, idAssociado As Integer) As Boolean

        Dim sql As String = $"UPDATE Associados SET Nome = @Nome, DataNascimento = @DataNascimento WHERE Id = @Id"

        Dim parametros As SqlParameter() = {
            New SqlParameter("@Nome", nome),
            New SqlParameter("@DataNascimento", dataNascimento),
            New SqlParameter("@Id", idAssociado)
        }

        Dim resultado As Boolean = AppDataBase.Atualizar(sql, parametros)

        Return resultado

    End Function

    Public Shared Function DeletarEmpresasAssociado(idAssociado As Integer) As Boolean

        Dim sql As String = $"DELETE FROM AssociadosEmpresas WHERE AssociadoId = @AssociadoId"

        Dim parametros As SqlParameter() = {
            New SqlParameter("@AssociadoId", idAssociado)
        }

        Dim resultado As Boolean = AppDataBase.Deletar(sql, parametros)

        Return resultado

    End Function

    Public Shared Function DeletarAssociado(idAssociado As Integer) As Boolean

        Dim sql As String = $"DELETE FROM Associados WHERE Id = @AssociadoId"

        Dim parametros As SqlParameter() = {
            New SqlParameter("@AssociadoId", idAssociado)
        }

        Dim resultado As Boolean = AppDataBase.Deletar(sql, parametros)

        Return resultado

    End Function

    Public Shared Function DeletarEmpresaDeUmAssociado(idAssociado As Integer, idEmpresa As Integer) As Boolean

        Dim sql As String = $"DELETE FROM AssociadosEmpresas WHERE AssociadoId = @AssociadoId AND EmpresaId = @EmpresaId"

        Dim parametros As SqlParameter() = {
            New SqlParameter("@AssociadoId", idAssociado),
            New SqlParameter("@EmpresaId", idEmpresa)
        }

        Dim resultado As Boolean = AppDataBase.Deletar(sql, parametros)

        Return resultado

    End Function


    Public Shared Function SelecionarPorFiltro(filtro As TipoFiltro, valor As String) As List(Of Associado)

        Dim consulta As String = ""
        Dim sql As String = $"SELECT Id, Nome, CPF, DataNascimento FROM Associados"

        Select Case filtro
            Case TipoFiltroAssociado.CPF
                consulta = sql & $" WHERE CPF = '{valor}'"
            Case TipoFiltroAssociado.Nome
                consulta = sql & $" WHERE Nome LIKE '%{valor}%'"
            Case TipoFiltroAssociado.ID
                consulta = sql & $" WHERE ID = {CInt(valor)}"
            Case TipoFiltroAssociado.Todos
                consulta = sql
        End Select

        Dim dt = AppDataBase.Selecionar(consulta)

        Dim associados As List(Of Associado) = ConverterDados.ConverteDataTableEmLista(dt, Function(row) New Associado With {
                .Id = row.Field(Of Integer)("Id"),
                .Nome = row.Field(Of String)("Nome"),
                .CPF = row.Field(Of String)("CPF"),
                .DataNascimento = row.Field(Of DateTime)("DataNascimento")
            })

        Return associados

    End Function

    Public Enum TipoFiltroAssociado
        CPF
        Nome
        ID
        Todos
    End Enum

End Class
