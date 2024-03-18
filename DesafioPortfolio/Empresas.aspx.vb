Imports System.Web.Services

Public Class Empresas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

    End Sub

    <WebMethod()>
    Public Shared Function SelecionarRegistros(filtro As String) As String

        Try

            Dim empresas As List(Of Empresa) = New List(Of Empresa)

            If IsNumeric(filtro) And filtro.Length = 14 Then
                empresas = EmpresaRepository.SelecionarPorFiltro(EmpresaRepository.TipoFiltro.CNPJ, filtro)

            ElseIf IsNumeric(filtro) And filtro.Length <> 14 Then
                empresas = EmpresaRepository.SelecionarPorFiltro(EmpresaRepository.TipoFiltro.ID, filtro)

            ElseIf String.IsNullOrEmpty(filtro) Then
                empresas = EmpresaRepository.SelecionarPorFiltro(EmpresaRepository.TipoFiltro.Todos, filtro)
            Else
                empresas = EmpresaRepository.SelecionarPorFiltro(EmpresaRepository.TipoFiltro.Nome, filtro)
            End If

            If empresas.Count = 0 Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "Nenhum registro encontrado.")
            End If

            Dim html As New StringBuilder()
            Dim tabela As New StringBuilder()

            html.Append(<div class='table-responsive-sm'>
                            <table id='xtable' class='table table-hover table-striped pt-0 m-auto'>
                                <thead class='thead thead-light headerfix m-0 mt-1'>
                                    <tr>
                                        <th scope='col'>#</th>
                                        <th scope='col'>Nome</th>
                                        <th scope='col'>CNPJ</th>
                                        <th scope='col'>...</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <td></td>
                                </tbody>
                            </table>
                        </div>)

            Dim butoes As String = String.Empty
            Dim xbutoes As String = String.Join(Environment.NewLine,
                            "<button type='button' class='btn btn-info fs-14px ml-2' title='Detalhes' onclick='detalhes(###id);'><span class='fa fa-file-alt'></span></button>" &
                            "<button type='button' class='btn btn-warning fs-14px ml-2' title='Editar' onclick='editar(###id);'><span class='fa fa-edit'></span></button>" &
                            "<button type='button' class='btn btn-danger fs-14px ml-2' title='Excluir' onclick='excluir(###id);'><span class='fa fa-trash'></span></button>")

            For Each item As Empresa In empresas
                butoes = xbutoes
                butoes = butoes.Replace("###id", item.Id)

                tabela.Append("<tr>" &
                                "<th scope='col'>" & item.Id & "</th>" &
                                "<td>" & item.Nome & "</td>" &
                                "<td>" & item.CNPJ & "</td>" &
                                "<td align=center>" & butoes & "</td>" &
                            "</tr>")
            Next

            Return html.Replace("<td></td>", tabela.ToString).ToString

        Catch ex As Exception
            Return HtmlClass.Alerta(99, "ATENÇÃO!", "Ocorreu um erro ao tentar realizar a operação.")
        End Try

    End Function

    <WebMethod()>
    Public Shared Function NovoRegistro() As String

        Try
            Dim html As New StringBuilder
            Dim opcoes As String = String.Empty

            html.Append(<div class="mt-1">
                            <div class="ml-2 h5 mb-3 text-left text-muted">Nova Empresa</div>
                            <div class="row ml-0">
                                <div class="col-md-8">
                                    <div class="form-group">
                                        <label id="lbnome" for="nome" class="fs-14px font-weight-500 mb-1">Nome</label>
                                        <input type="text" id="nome" class="form-control fs-14px border-gray rounded" placeholder="Nome da Empresa" runat="server" onfocus="this.select();"/>
                                    </div>
                                </div>
                            </div>
                            <div class="row ml-0">
                                <div class="col-md-8">
                                    <div class="form-group">
                                        <label id="lbcnpj" for="cnpj" class="fs-14px font-weight-500 mb-1">CNPJ</label>
                                        <input type="text" id="cnpj" class="form-control fs-14px border-gray rounded" placeholder="CNPJ da Empresa" runat="server" onfocus="this.select();"/>
                                    </div>
                                </div>
                            </div>
                            <div class="ml-2 h5 mb-3 text-left text-muted">Associados</div>
                            <div class="row ml-0">
                                <div class="col-md-8">
                                    <div class="form-group">
                                        <label id="lbassociados" for="cbassociados" class="fs-14px font-weight-500 mb-1">Associados</label>
                                        <select id="cbassociados" class="selectpicker form-control fs-14px" data-style="btn-default" multiple="" data-none-selected-text="Selecionar Associados" aria-describedby="button-addon2">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <div class="row ml-0 mt-3">
                                <div class="col-md-3">
                                    <button type="button" class="btn bg-primary text-white fs-15px mt-0 width-150 ml-1" onclick="adicionar();">Salvar Empresa</button>
                                </div>
                            </div>
                        </div>)


            Dim associados As List(Of Associado) = AssociadoRepository.SelecionarTodos()

            If associados.Count > 0 Then
                opcoes = "<option value='' disabled=''>Selecione Associados</option>"

                For Each item As Associado In associados
                    opcoes += $"<option value='{item.Id}'>{item.Nome} - {item.CPF}</option>"
                Next

            Else
                opcoes = "<option value='' disabled>Nenhum associado disponível</option>"
            End If

            html.Replace("<option></option>", opcoes)

            Return html.ToString()

        Catch ex As Exception
            Return HtmlClass.Alerta(99, "ATENÇÃO!", "Ocorreu um erro ao tentar realizar a operação.")
        End Try

    End Function


    <WebMethod()>
    Public Shared Function AdicionarRegistro(nome As String, cnpj As String, associados As String) As String

        Try

            If String.IsNullOrEmpty(nome) Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "NOME é obrigatório.")
            End If

            If String.IsNullOrEmpty(cnpj) Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "CNPJ é obrigatório.")
            ElseIf Not IsNumeric(cnpj) And cnpj.Length <> 14 Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "CNPJ informado é obrigatório.")
            End If

            Dim idEmpresa As Integer = EmpresaRepository.InserirEmpresa(nome.ToUpper(), cnpj)

            If idEmpresa = -1 Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "Ocorreu um erro ao cadastrar a Empresa.")
            End If

            Dim idsAssociados As String() = associados.Split(","c)

            For Each idAssociado As String In idsAssociados
                Dim idRelaciomanento = EmpresaRepository.InserirEmpresasAssociados(idAssociado, idEmpresa)
            Next

            Return HtmlClass.Alerta(10, "ATENÇÃO!", "EMPRESA cadastrada com sucesso.")

        Catch ex As Exception
            Return HtmlClass.Alerta(99, "ATENÇÃO!", "Ocorreu um erro ao tentar realizar a operação.")
        End Try

    End Function

    <WebMethod()>
    Public Shared Function EditarRegistro(id As Integer) As String

        Try
            Dim empresa As Empresa = EmpresaRepository.SelecionarPorId(id)

            If IsNothing(empresa) Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "Empresa informada não foi encontrada.")
            End If

            Dim html As New StringBuilder
            Dim opcoes As String = String.Empty

            html.Append(<div class="mt-1">
                            <div class="ml-2 h5 mb-3 text-left text-muted">Editar Empresa</div>
                            <input type="hidden" id="id" name="id" value="###id"/>
                            <div class="row ml-0">
                                <div class="col-md-8">
                                    <div class="form-group">
                                        <label id="lbnome" for="nome" class="fs-14px font-weight-500 mb-1">Nome</label>
                                        <input type="text" id="nome" class="form-control fs-14px border-gray rounded" placeholder="Nome da Empresa" runat="server" onfocus="this.select();" value="###nome"/>
                                    </div>
                                </div>
                            </div>
                            <div class="row ml-0">
                                <div class="col-md-8">
                                    <div class="form-group">
                                        <label id="lbcnpj" for="cnpj" class="fs-14px font-weight-500 mb-1">CNPJ</label>
                                        <input type="text" id="cnpj" class="form-control fs-14px border-gray rounded disabled" placeholder="CNPJ da Empresa" runat="server" onfocus="this.select();" value="###cnpj" disabled=""/>
                                    </div>
                                </div>
                            </div>
                            <div class="ml-2 h5 mb-3 text-left text-muted">Associados</div>
                            <div class="row ml-0">
                                <div class="col-md-8">
                                    <div class="form-group">
                                        <label id="lbassociados" for="cbassociados" class="fs-14px font-weight-500 mb-1">Associados</label>
                                        <select id="cbassociados" class="selectpicker form-control fs-14px" data-style="btn-default" multiple="" data-none-selected-text="Selecionar Associados" aria-describedby="button-addon2">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <div class="row ml-0 mt-3">
                                <div class="col-md-3">
                                    <button type="button" class="btn bg-primary text-white fs-15px mt-0 width-150 ml-1" onclick="atualizar();">Salvar Empresa</button>
                                </div>
                            </div>
                        </div>)

            html.Replace("###id", empresa.Id)
            html.Replace("###nome", empresa.Nome)
            html.Replace("###cnpj", empresa.CNPJ)

            Dim associadosEmpresa = AssociadoRepository.SelecionarAssociadosEmpresa(id)
            Dim todosAssociados = AssociadoRepository.SelecionarTodos()

            If todosAssociados.Count > 0 Then
                opcoes = "<option value='' disabled=''>Selecione Associados</option>"

                For Each associado As Associado In todosAssociados
                    Dim selecionado As Boolean = associadosEmpresa.Any(Function(ae) ae.Id = associado.Id)

                    If selecionado Then
                        opcoes += $"<option selected value='{associado.Id}'>{associado.Nome} - {associado.CPF}</option>"
                    Else
                        opcoes += $"<option value='{associado.Id}'>{associado.Nome} - {associado.CPF}</option>"
                    End If
                Next

            Else
                opcoes = "<option value='' disabled=''>Nenhum associado disponível</option>"
            End If


            html.Replace("<option></option>", opcoes)

            Return html.ToString()

        Catch ex As Exception
            Return HtmlClass.Alerta(99, "ATENÇÃO!", "Ocorreu um erro ao tentar realizar a operação.")
        End Try

    End Function

    <WebMethod()>
    Public Shared Function AtualizarRegistro(nome As String, id As String, associados As String) As String

        Try

            If String.IsNullOrEmpty(nome) Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "NOME da Empresa é obrigatório.")
            End If

            If String.IsNullOrEmpty(id) Or Not IsNumeric(id) Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "Não foi possível identificar a Empresa.")
            End If

            Dim resultado As Boolean = EmpresaRepository.AlterarEmpresa(nome.ToUpper(), id)

            If resultado = False Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "Ocorreu um erro ao alterar a Empresa.")
            End If

            Dim removido = EmpresaRepository.DeletarAssociadosEmpresa(id)

            If removido = True Then
                Dim idsAssociados As String() = associados.Split(","c)

                For Each idAssociado As String In idsAssociados
                    Dim idRelaciomanento = EmpresaRepository.InserirEmpresasAssociados(idAssociado, id)
                Next
            End If

            Return HtmlClass.Alerta(10, "ATENÇÃO!", "EMPRESA alterada com sucesso.")

        Catch ex As Exception
            Return HtmlClass.Alerta(99, "ATENÇÃO!", "Ocorreu um erro ao tentar realizar a operação.")
        End Try

    End Function

    <WebMethod()>
    Public Shared Function ExcluirRegistro(id As Integer) As String

        Try
            Dim empresa As Empresa = EmpresaRepository.SelecionarPorId(id)

            If IsNothing(empresa) Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "Empresa informada não foi encontrada.")
            End If

            Dim delAssociadosEmpresa As Boolean = EmpresaRepository.DeletarAssociadosEmpresa(id)

            Dim delEmpresa As Boolean = EmpresaRepository.DeletarEmpresa(id)

            If delEmpresa = False Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "Ocorreu um erro ao deletar Empresa.")
            End If

            Return HtmlClass.Alerta(10, "ATENÇÃO!", "EMPRESA deletada com sucesso.")

        Catch ex As Exception
            Return HtmlClass.Alerta(99, "ATENÇÃO!", "Ocorreu um erro ao tentar realizar a operação.")
        End Try

    End Function

    <WebMethod()>
    Public Shared Function DetalhesRegistro(id As Integer) As String

        Try
            Dim empresa As Empresa = EmpresaRepository.SelecionarPorId(id)

            If IsNothing(empresa) Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "Empresa informada não foi encontrada.")
            End If

            Dim html As New StringBuilder
            Dim opcoes As String = String.Empty

            html.Append(<div class="mt-1">
                            <div class="ml-2 h5 mb-3 text-left text-muted">Detalhes Empresa</div>
                            <div class="d-flex align-items-center ml-2 mb-4">
                                <div class="flex-grow-1">
                                    <h4 class="mb-1">###id - ###nome</h4>
                                    <div class="fs-15px">CNPJ: ###cnpj</div>
                                </div>
                            </div>
                            <div class="ml-2 h5 mb-3 mt-4 text-left text-muted">Associados Empresa</div>
                            ###AssociadosEmpresa
                        </div>)

            html.Replace("###id", empresa.Id)
            html.Replace("###nome", empresa.Nome)
            html.Replace("###cnpj", empresa.CNPJ)

            Dim associadosEmpresa = AssociadoRepository.SelecionarAssociadosEmpresa(id)
            Dim tabela As New StringBuilder()
            Dim xtabela As New StringBuilder()

            If associadosEmpresa.Count > 0 Then
                tabela.Append(<div class='table-responsive-sm'>
                                  <table id='xtable' class='table table-hover table-striped pt-0 m-auto'>
                                      <thead class='thead thead-light headerfix m-0 mt-1'>
                                          <tr>
                                              <th scope='col'>#</th>
                                              <th scope='col'>Nome</th>
                                              <th scope='col'>CPF</th>
                                              <th scope='col'>Data Nascimento</th>
                                              <th scope='col'></th>
                                          </tr>
                                      </thead>
                                      <tbody>
                                          <td></td>
                                      </tbody>
                                  </table>
                              </div>)

                Dim butoes As String = String.Empty
                Dim xbutoes As String = String.Join(Environment.NewLine,
                            "<button type='button' class='btn btn-danger fs-14px ml-2' title='Excluir' onclick='excluirAssociadoEmpresa(###idAssociado, ###idEmpresa);'><span class='fa fa-trash'></span></button>")

                For Each item As Associado In associadosEmpresa
                    butoes = xbutoes.Replace("###idAssociado", item.Id).Replace("###idEmpresa", id)

                    xtabela.Append("<tr>" &
                                    "<th scope='col'>" & item.Id & "</th>" &
                                    "<td>" & item.Nome & "</td>" &
                                    "<td>" & item.CPF & "</td>" &
                                    "<td>" & item.DataNascimento.ToString("dd/MM/yyyy") & "</td>" &
                                    "<td align=center>" & butoes & "</td>" &
                                "</tr>")
                Next

                tabela.Replace("<td></td>", xtabela.ToString).ToString()

            Else
                tabela.Append(HtmlClass.Alerta(2, "ATENÇÃO!", "Empresa não possui associados."))
            End If


            html.Replace("###AssociadosEmpresa", tabela.ToString())

            Return html.ToString()

        Catch ex As Exception
            Return HtmlClass.Alerta(99, "ATENÇÃO!", "Ocorreu um erro ao tentar realizar a operação.")
        End Try

    End Function


    <WebMethod()>
    Public Shared Function ExcluirAssociadoEmpresa(idAssociado As Integer, id As Integer) As String

        Try
            Dim associado As Associado = AssociadoRepository.SelecionarPorId(idAssociado)

            If IsNothing(associado) Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "Associado informado não foi encontrada.")
            End If

            Dim delEmpresasAssociados As Boolean = EmpresaRepository.DeletarAssociadoDeUmaEmpresa(idAssociado, id)

            If delEmpresasAssociados = False Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "Ocorreu um erro ao remover Associado.")
            End If

            Return HtmlClass.Alerta(10, "ATENÇÃO!", "ASSOCIADO removido com sucesso.")

        Catch ex As Exception
            Return HtmlClass.Alerta(99, "ATENÇÃO!", "Ocorreu um erro ao tentar realizar a operação.")
        End Try

    End Function

End Class