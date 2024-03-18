Imports System.Globalization
Imports System.Web.Services

Public Class Associados
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

    End Sub

    <WebMethod()>
    Public Shared Function SelecionarRegistros(filtro As String) As String

        Try

            Dim associados As List(Of Associado) = New List(Of Associado)

            If IsNumeric(filtro) And filtro.Length = 11 Then
                associados = AssociadoRepository.SelecionarPorFiltro(AssociadoRepository.TipoFiltroAssociado.CPF, filtro)

            ElseIf IsNumeric(filtro) And filtro.Length <> 14 Then
                associados = AssociadoRepository.SelecionarPorFiltro(AssociadoRepository.TipoFiltroAssociado.ID, filtro)

            ElseIf String.IsNullOrEmpty(filtro) Then
                associados = AssociadoRepository.SelecionarPorFiltro(AssociadoRepository.TipoFiltroAssociado.Todos, filtro)

            Else
                associados = AssociadoRepository.SelecionarPorFiltro(AssociadoRepository.TipoFiltroAssociado.Nome, filtro)
            End If

            If associados.Count = 0 Then
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
                                        <th scope='col'>CPF</th>
                                        <th scope='col'>Data Nascimento</th>
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

            For Each item As Associado In associados
                butoes = xbutoes
                butoes = butoes.Replace("###id", item.Id)

                tabela.Append("<tr>" &
                                "<th scope='col'>" & item.Id & "</th>" &
                                "<td>" & item.Nome & "</td>" &
                                "<td>" & item.CPF & "</td>" &
                                "<td>" & item.DataNascimento.ToString("dd/MM/yyyy") & "</td>" &
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
                            <div class="ml-2 h5 mb-3 text-left text-muted">Novo Associado</div>
                            <div class="row ml-0">
                                <div class="col-md-8">
                                    <div class="form-group">
                                        <label id="lbnome" for="nome" class="fs-14px font-weight-500 mb-1">Nome</label>
                                        <input type="text" id="nome" class="form-control fs-14px border-gray rounded" placeholder="Nome da Empresa" runat="server" onfocus="this.select();"/>
                                    </div>
                                </div>
                            </div>
                            <div class="row ml-0">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label id="lbcnpj" for="cpf" class="fs-14px font-weight-500 mb-1">CPF</label>
                                        <input type="text" id="cpf" class="form-control fs-14px border-gray rounded" placeholder="CPF do Associado" runat="server" onfocus="this.select();"/>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label id="lbnascimento" for="nascimento" class="fs-14px font-weight-500 mb-1">Data Nascimento</label>
                                        <input type="date" id="nascimento" class="form-control fs-14px border-gray rounded" runat="server" onfocus="this.select();"/>
                                    </div>
                                </div>
                            </div>
                            <div class="ml-2 h5 mb-3 text-left text-muted">Empresas</div>
                            <div class="row ml-0">
                                <div class="col-md-8">
                                    <div class="form-group">
                                        <label id="lbempresas" for="cbempresas" class="fs-14px font-weight-500 mb-1">Empresas</label>
                                        <select id="cbempresas" class="selectpicker form-control fs-14px" data-style="btn-default" multiple="" data-none-selected-text="Selecionar Empresas" aria-describedby="button-addon2">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <div class="row ml-0 mt-3">
                                <div class="col-md-3">
                                    <button type="button" class="btn bg-primary text-white fs-15px mt-0 width-150 ml-1" onclick="adicionar();">Salvar Associado</button>
                                </div>
                            </div>
                        </div>)


            Dim empresas As List(Of Empresa) = EmpresaRepository.SelecionarTodos()

            If empresas.Count > 0 Then
                opcoes = "<option value='' disabled=''>Selecione Empresas</option>"

                For Each item As Empresa In empresas
                    opcoes += $"<option value='{item.Id}'>{item.Nome} - {item.CNPJ}</option>"
                Next

            Else
                opcoes = "<option value='' disabled>Nenhuma empresa disponível</option>"
            End If

            html.Replace("<option></option>", opcoes)

            Return html.ToString()

        Catch ex As Exception
            Return HtmlClass.Alerta(99, "ATENÇÃO!", "Ocorreu um erro ao tentar realizar a operação.")
        End Try

    End Function


    <WebMethod()>
    Public Shared Function AdicionarRegistro(nome As String, cpf As String, dataNascimento As String, empresas As String) As String

        Try

            If String.IsNullOrEmpty(nome) Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "NOME é obrigatório.")
            End If

            If String.IsNullOrEmpty(cpf) Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "CPF é obrigatório.")
            ElseIf Not IsNumeric(cpf) And cpf.Length <> 11 Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "CPF informado é obrigatório.")
            End If

            Dim data As DateTime
            If String.IsNullOrEmpty(dataNascimento) Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "DATA NASCIMENTO é obrigatório.")

            ElseIf Not DateTime.TryParseExact(dataNascimento, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, data) Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "DATA NASCIMENTO inválida.")

            End If

            Dim idAssociado As Integer = AssociadoRepository.InserirAssociado(nome.ToUpper(), cpf, dataNascimento)

            If idAssociado = -1 Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "Ocorreu um erro ao cadastrar Associado.")
            End If

            Dim idsEmpresas As String() = empresas.Split(","c)

            For Each idEmpresa As String In idsEmpresas
                Dim idRelaciomanento = EmpresaRepository.InserirEmpresasAssociados(idAssociado, idEmpresa)
            Next

            Return HtmlClass.Alerta(10, "ATENÇÃO!", "ASSOCIADO cadastrada com sucesso.")

        Catch ex As Exception
            Return HtmlClass.Alerta(99, "ATENÇÃO!", "Ocorreu um erro ao tentar realizar a operação.")
        End Try

    End Function

    <WebMethod()>
    Public Shared Function EditarRegistro(id As Integer) As String

        Try
            Dim associado As Associado = AssociadoRepository.SelecionarPorId(id)

            If IsNothing(associado) Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "Associado informado não foi encontrada.")
            End If

            Dim html As New StringBuilder
            Dim opcoes As String = String.Empty

            html.Append(<div class="mt-1">
                            <div class="ml-2 h5 mb-3 text-left text-muted">Editar Associado</div>
                            <input type="hidden" id="id" name="id" value="###id"/>
                            <div class="row ml-0">
                                <div class="col-md-8">
                                    <div class="form-group">
                                        <label id="lbnome" for="nome" class="fs-14px font-weight-500 mb-1">Nome</label>
                                        <input type="text" id="nome" class="form-control fs-14px border-gray rounded" placeholder="Nome do Associado" runat="server" onfocus="this.select();" value="###nome"/>
                                    </div>
                                </div>
                            </div>
                            <div class="row ml-0">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label id="lbcnpj" for="cpf" class="fs-14px font-weight-500 mb-1">CPF</label>
                                        <input type="text" id="cpf" class="form-control fs-14px border-gray rounded disabled" placeholder="CPF do Associado" runat="server" onfocus="this.select();" value="###cpf" disabled=""/>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label id="lbnascimento" for="nascimento" class="fs-14px font-weight-500 mb-1">Data Nascimento</label>
                                        <input type="date" id="nascimento" class="form-control fs-14px border-gray rounded" runat="server" onfocus="this.select();" value="###nascimento"/>
                                    </div>
                                </div>
                            </div>
                            <div class="ml-2 h5 mb-3 text-left text-muted">Empresas</div>
                            <div class="row ml-0">
                                <div class="col-md-8">
                                    <div class="form-group">
                                        <label id="lbempresas" for="cbempresas" class="fs-14px font-weight-500 mb-1">Associados</label>
                                        <select id="cbempresas" class="selectpicker form-control fs-14px" data-style="btn-default" multiple="" data-none-selected-text="Selecionar Empresas" aria-describedby="button-addon2">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <div class="row ml-0 mt-3">
                                <div class="col-md-3">
                                    <button type="button" class="btn bg-primary text-white fs-15px mt-0 width-150 ml-1" onclick="atualizar();">Salvar Associado</button>
                                </div>
                            </div>
                        </div>)

            html.Replace("###id", associado.Id)
            html.Replace("###nome", associado.Nome)
            html.Replace("###cpf", associado.CPF)
            html.Replace("###nascimento", associado.DataNascimento.ToString("yyyy-MM-dd"))

            Dim empresasAssociados = EmpresaRepository.SelecionarEmpresasAssociados(id)
            Dim todoasEmpresas = EmpresaRepository.SelecionarTodos()

            If todoasEmpresas.Count > 0 Then
                opcoes = "<option value='' disabled=''>Selecione Empresas</option>"

                For Each empresa As Empresa In todoasEmpresas
                    Dim selecionado As Boolean = empresasAssociados.Any(Function(ae) ae.Id = empresa.Id)

                    If selecionado Then
                        opcoes += $"<option selected value='{empresa.Id}'>{empresa.Nome} - {empresa.CNPJ}</option>"
                    Else
                        opcoes += $"<option value='{empresa.Id}'>{empresa.Nome} - {empresa.CNPJ}</option>"
                    End If
                Next

            Else
                opcoes = "<option value='' disabled=''>Nenhuma empresa disponível</option>"
            End If


            html.Replace("<option></option>", opcoes)

            Return html.ToString()

        Catch ex As Exception
            Return HtmlClass.Alerta(99, "ATENÇÃO!", "Ocorreu um erro ao tentar realizar a operação.")
        End Try

    End Function

    <WebMethod()>
    Public Shared Function AtualizarRegistro(nome As String, dataNascimento As String, id As String, empresas As String) As String

        Try

            If String.IsNullOrEmpty(nome) Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "NOME é obrigatório.")
            End If

            If String.IsNullOrEmpty(id) Or Not IsNumeric(id) Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "Não foi possível identificar o Associado.")
            End If

            Dim data As DateTime
            If String.IsNullOrEmpty(dataNascimento) Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "DATA NASCIMENTO é obrigatório.")

            ElseIf Not DateTime.TryParseExact(dataNascimento, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, data) Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "DATA NASCIMENTO inválida.")

            End If

            Dim resultado As Boolean = AssociadoRepository.AlterarAssociado(nome.ToUpper(), dataNascimento, id)

            If resultado = False Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "Ocorreu um erro ao atualizar o Associado.")
            End If

            Dim removido = AssociadoRepository.DeletarEmpresasAssociado(id)

            If removido = True Then
                Dim idsEmpresas As String() = empresas.Split(","c)

                For Each idEmpresa As String In idsEmpresas
                    Dim idRelaciomanento = EmpresaRepository.InserirEmpresasAssociados(id, idEmpresa)
                Next
            End If

            Return HtmlClass.Alerta(10, "ATENÇÃO!", "ASSOCIADO atualizado com sucesso.")

        Catch ex As Exception
            Return HtmlClass.Alerta(99, "ATENÇÃO!", "Ocorreu um erro ao tentar realizar a operação.")
        End Try

    End Function

    <WebMethod()>
    Public Shared Function ExcluirRegistro(id As Integer) As String

        Try
            Dim associado As Associado = AssociadoRepository.SelecionarPorId(id)

            If IsNothing(associado) Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "Associado não foi encontrada.")
            End If

            Dim delEmpresasAssociado As Boolean = AssociadoRepository.DeletarEmpresasAssociado(id)

            Dim delAssociado As Boolean = AssociadoRepository.DeletarAssociado(id)

            If delAssociado = False Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "Ocorreu um erro ao removido Associado.")
            End If

            Return HtmlClass.Alerta(10, "ATENÇÃO!", "ASSOCIADO removido com sucesso.")

        Catch ex As Exception
            Return HtmlClass.Alerta(99, "ATENÇÃO!", "Ocorreu um erro ao tentar realizar a operação.")
        End Try

    End Function

    <WebMethod()>
    Public Shared Function DetalhesRegistro(id As Integer) As String

        Try
            Dim associado As Associado = AssociadoRepository.SelecionarPorId(id)

            If IsNothing(associado) Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "Associado informado não foi encontrada.")
            End If

            Dim html As New StringBuilder
            Dim opcoes As String = String.Empty

            html.Append(<div class="mt-1">
                            <div class="ml-2 h5 mb-3 text-left text-muted">Detalhes Empresa</div>
                            <div class="d-flex align-items-center ml-2 mb-4">
                                <div class="flex-grow-1">
                                    <h4 class="mb-1">###id - ###nome</h4>
                                    <div class="fs-15px">CPF: ###cpf</div>
                                    <div class="fs-15px">Data Nascimento: ###nascimento</div>
                                </div>
                            </div>
                            <div class="ml-2 h5 mb-3 mt-4 text-left text-muted">Empresas do Associado</div>
                            ###AssociadosEmpresa
                        </div>)

            html.Replace("###id", associado.Id)
            html.Replace("###nome", associado.Nome)
            html.Replace("###cpf", associado.CPF)
            html.Replace("###nascimento", associado.DataNascimento.ToString("dd/MM/yyyy"))

            Dim empresasAssociado = EmpresaRepository.SelecionarEmpresasAssociados(id)
            Dim tabela As New StringBuilder()
            Dim xtabela As New StringBuilder()

            If empresasAssociado.Count > 0 Then
                tabela.Append(<div class='table-responsive-sm'>
                                  <table id='xtable' class='table table-hover table-striped pt-0 m-auto'>
                                      <thead class='thead thead-light headerfix m-0 mt-1'>
                                          <tr>
                                              <th scope='col'>#</th>
                                              <th scope='col'>Nome</th>
                                              <th scope='col'>CNPJ</th>
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
                            "<button type='button' class='btn btn-danger fs-14px ml-2' title='Excluir' onclick='excluirEmpresaAssociado(###idAssociado, ###idEmpresa);'><span class='fa fa-trash'></span></button>")

                For Each item As Empresa In empresasAssociado
                    butoes = xbutoes.Replace("###idAssociado", id).Replace("###idEmpresa", item.Id)

                    xtabela.Append("<tr>" &
                                    "<th scope='col'>" & item.Id & "</th>" &
                                    "<td>" & item.Nome & "</td>" &
                                    "<td>" & item.CNPJ & "</td>" &
                                    "<td align=center>" & butoes & "</td>" &
                                "</tr>")
                Next

                tabela.Replace("<td></td>", xtabela.ToString).ToString()

            Else
                tabela.Append(HtmlClass.Alerta(2, "ATENÇÃO!", "Associado não possui empresas."))
            End If


            html.Replace("###AssociadosEmpresa", tabela.ToString())

            Return html.ToString()

        Catch ex As Exception
            Return HtmlClass.Alerta(99, "ATENÇÃO!", "Ocorreu um erro ao tentar realizar a operação.")
        End Try

    End Function


    <WebMethod()>
    Public Shared Function ExcluirEmpresaAssociado(id As Integer, idEmpresa As Integer) As String

        Try
            Dim empresa As Empresa = EmpresaRepository.SelecionarPorId(idEmpresa)

            If IsNothing(empresa) Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "Empresa informada não foi encontrada.")
            End If

            Dim delEmpresaAssociado As Boolean = AssociadoRepository.DeletarEmpresaDeUmAssociado(id, idEmpresa)

            If delEmpresaAssociado = False Then
                Return HtmlClass.Alerta(99, "ATENÇÃO!", "Ocorreu um erro ao remover Empresa.")
            End If

            Return HtmlClass.Alerta(10, "ATENÇÃO!", "EMPRESA removida com sucesso.")

        Catch ex As Exception
            Return HtmlClass.Alerta(99, "ATENÇÃO!", "Ocorreu um erro ao tentar realizar a operação.")
        End Try

    End Function

End Class