Imports System.Data.SqlClient

Public Class AppDataBase

    Private Shared connectionString As String = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\EmpresaAssociados.mdf;Integrated Security=True"

    Public Shared Function Selecionar(query As String) As DataTable
        Dim dt As DataTable = New DataTable()

        Using sqlConnection As SqlConnection = New SqlConnection(connectionString)
            Try
                sqlConnection.Open()

                Using adapter As SqlDataAdapter = New SqlDataAdapter(query, sqlConnection)
                    adapter.Fill(dt)
                End Using

            Catch ex As Exception
                Throw ex
            End Try
        End Using

        Return dt
    End Function

    Public Shared Function Inserir(query As String, parametros As SqlParameter()) As Integer
        Using sqlConnection As SqlConnection = New SqlConnection(connectionString)
            Try
                sqlConnection.Open()

                Using sqlCommand As SqlCommand = New SqlCommand(query & "; SELECT SCOPE_IDENTITY();", sqlConnection)
                    If parametros IsNot Nothing Then
                        sqlCommand.Parameters.AddRange(parametros)
                    End If

                    Dim id As Integer = Convert.ToInt32(sqlCommand.ExecuteScalar())

                    Return id
                End Using

            Catch ex As Exception
                Return -1
            End Try
        End Using
    End Function

    Public Shared Function Atualizar(query As String, parametros As SqlParameter()) As Boolean
        Using sqlConnection As SqlConnection = New SqlConnection(connectionString)
            Try
                sqlConnection.Open()

                Using sqlCommand As SqlCommand = New SqlCommand(query, sqlConnection)

                    If parametros IsNot Nothing AndAlso parametros.Length > 0 Then
                        sqlCommand.Parameters.AddRange(parametros)
                    End If

                    Dim linhasAfetadas As Integer = sqlCommand.ExecuteNonQuery()

                    If linhasAfetadas > 0 Then
                        Return True
                    Else
                        Return False
                    End If
                End Using

            Catch ex As Exception
                Return False
            End Try
        End Using
    End Function


    Public Shared Function Deletar(query As String, parametros As SqlParameter()) As Boolean
        Using sqlConnection As SqlConnection = New SqlConnection(connectionString)
            Try
                sqlConnection.Open()

                Using sqlCommand As SqlCommand = New SqlCommand(query, sqlConnection)

                    If parametros IsNot Nothing AndAlso parametros.Length > 0 Then
                        sqlCommand.Parameters.AddRange(parametros)
                    End If

                    Dim linhasAfetadas As Integer = sqlCommand.ExecuteNonQuery()

                    If linhasAfetadas > 0 Then
                        Return True
                    Else
                        Return False
                    End If
                End Using

            Catch ex As Exception
                Return False
            End Try
        End Using
    End Function


End Class
