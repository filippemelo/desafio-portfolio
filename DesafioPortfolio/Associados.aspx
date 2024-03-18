<%@ Page Title="Empresas" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Associados.aspx.vb" Inherits="DesafioPortfolio.Associados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
    <title>Associados - AppSystem</title>   
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="True"></asp:ScriptManager>

        <div class="col-xl-12">
            <h3 class="page-header h3">Associados</h3>
            <hr class="mb-2">

            <div class="card">
                <div class="card-body">
                    <div class="row ml-1 mt-0 mb-2">
                        <div id="divpesquisa" class="col-xl-10 mb-1 mt-1" style="display: block;">
                            <div class="input-group">
                                <input class="form-control fs-15px border-gray rounded" type="Search" placeholder="Pesquisar por Nome, CPF ou ID ..." id="pesquisa"><span class="btn btn-primary ml-2" onclick="selecionar();"><span class="fa fa-search mr-1"></span>Pesquisar</span>
                            </div>
                        </div>
                        <div class="col-md-auto mb-1 mt-1">
                            <button type="button" id="btnnovo" class="btn btn-outline-primary fs-15px mt-0 width-100" onclick="novo();" style="display: block;"><span class="fa fa-plus mr-1"></span>Novo</button>
                        </div>
                    </div>
                    <div id="mensagem" class="w-100 m-auto p-2 pt-0"></div>
                    <div id="tab" class="w-100 m-auto text-center table-responsive p-2 pt-0"></div>
                </div>
            </div>

        </div>
    </form>

</asp:Content>

<asp:Content ID="Scripts" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script type="text/javascript">
        function SomenteNumeros(inputElement) {
            inputElement.addEventListener('input', function () {
                this.value = this.value.replace(/[^0-9]/g, '');
            });
        }
        
        function selecionar() {
            $('#modalLoad').modal('show');
            document.getElementById('mensagem').innerHTML = "";
            document.getElementById('tab').innerHTML = "";

            let filtro = document.getElementById("pesquisa").value;

            $.ajax({
                type: "POST",
                url: "associados.aspx/SelecionarRegistros",
                data: '{filtro: "' + filtro + '"}',
                contentType: "application/json; charset=utf-8",
                DataType: "json",
                success: function (msg) {
                    document.getElementById('tab').innerHTML = msg.d;

                    setTimeout(function () {
                        $('#modalLoad').modal('hide');
                    }, 500);
                }

            });
           
        };

        function novo() {
            $('#modalLoad').modal('show');
            document.getElementById('mensagem').innerHTML = "";
            document.getElementById('tab').innerHTML = "";
            document.getElementById("pesquisa").value = "";

            $.ajax({
                type: "POST",
                url: "associados.aspx/NovoRegistro",
                data: '{}',
                contentType: "application/json; charset=utf-8",
                DataType: "json",
                success: function (msg) {
                    document.getElementById('mensagem').innerHTML = msg.d;

                    $('.bootstrap-select').remove();
                    $('.selectpicker').selectpicker();

                    const cpfInput = document.querySelector('#cpf');
                    SomenteNumeros(cpfInput);

                    setTimeout(function () {
                        $('#modalLoad').modal('hide');
                    }, 500);
                }

            });

        };

        function adicionar() {
            $('#modalLoad').modal('show');
            document.getElementById('tab').innerHTML = "";
            let nome = document.getElementById("nome").value;
            let cpf = document.getElementById("cpf").value;
            let dataNascimento = document.getElementById("nascimento").value;

            let selectEmpresas = document.getElementById("cbempresas");
            let empresas = "";

            for (var i = 0; i < selectEmpresas.options.length; i++) {
                var option = selectEmpresas.options[i];
                if (option.selected) {
                    empresas += option.value + ",";
                }
            }

            if (empresas.endsWith(",")) {
                empresas = empresas.slice(0, -1);
            }

            $.ajax({
                type: "POST",
                url: "associados.aspx/AdicionarRegistro",
                data: '{nome: "' + nome + '", cpf: "' + cpf + '", dataNascimento: "' + dataNascimento + '", empresas: "' + empresas + '"}',
                contentType: "application/json; charset=utf-8",
                DataType: "json",
                success: function (msg) {
                    if (msg.d.indexOf("alert-success") > 0) {
                        document.getElementById('mensagem').innerHTML = "";
                        document.getElementById('tab').innerHTML = msg.d;
                    } else {
                        document.getElementById('tab').innerHTML = msg.d;

                        let tab = $("#tab");
                        if (tab.length) {
                            $('html, body').animate({
                                scrollTop: tab.offset().top
                            }, 1000);
                        }
                    };

                    setTimeout(function () {
                        $('#modalLoad').modal('hide');
                    }, 500);
                }

            });
        };

        function editar(id) {
            $('#modalLoad').modal('show');
            document.getElementById('mensagem').innerHTML = "";
            document.getElementById('tab').innerHTML = "";
            document.getElementById("pesquisa").value = "";

            $.ajax({
                type: "POST",
                url: "associados.aspx/EditarRegistro",
                data: '{id: ' + id + '}',
                contentType: "application/json; charset=utf-8",
                DataType: "json",
                success: function (msg) {
                    document.getElementById('mensagem').innerHTML = msg.d;

                    $('.bootstrap-select').remove();
                    $('.selectpicker').selectpicker();

                    const cpfInput = document.querySelector('#cpf');
                    SomenteNumeros(cpfInput);

                    setTimeout(function () {
                        $('#modalLoad').modal('hide');
                    }, 500);
                }

            });

        };

        function detalhes(id) {
            $('#modalLoad').modal('show');
            document.getElementById('mensagem').innerHTML = "";
            document.getElementById('tab').innerHTML = "";
            document.getElementById("pesquisa").value = "";

            $.ajax({
                type: "POST",
                url: "associados.aspx/DetalhesRegistro",
                data: '{id: ' + id + '}',
                contentType: "application/json; charset=utf-8",
                DataType: "json",
                success: function (msg) {
                    document.getElementById('mensagem').innerHTML = msg.d;

                    setTimeout(function () {
                        $('#modalLoad').modal('hide');
                    }, 500);
                }

            });

        };

        function atualizar() {
            $('#modalLoad').modal('show');
            document.getElementById('tab').innerHTML = "";
            let nome = document.getElementById("nome").value;
            let dataNascimento = document.getElementById("nascimento").value;
            let id = document.getElementById("id").value;

            let selectEmpresas = document.getElementById("cbempresas");
            let empresas = "";

            for (var i = 0; i < selectEmpresas.options.length; i++) {
                var option = selectEmpresas.options[i];
                if (option.selected) {
                    empresas += option.value + ",";
                }
            }

            if (empresas.endsWith(",")) {
                empresas = empresas.slice(0, -1);
            }

            $.ajax({
                type: "POST",
                url: "associados.aspx/AtualizarRegistro",
                data: '{nome: "' + nome + '", dataNascimento: "' + dataNascimento + '", id: "' + id + '", empresas: "' + empresas + '"}',
                contentType: "application/json; charset=utf-8",
                DataType: "json",
                success: function (msg) {
                    if (msg.d.indexOf("alert-success") > 0) {
                        document.getElementById('mensagem').innerHTML = "";
                        document.getElementById('tab').innerHTML = msg.d;
                    } else {
                        document.getElementById('tab').innerHTML = msg.d;

                        let tab = $("#tab");
                        if (tab.length) {
                            $('html, body').animate({
                                scrollTop: tab.offset().top
                            }, 1000);
                        }
                    };

                    setTimeout(function () {
                        $('#modalLoad').modal('hide');
                    }, 500);
                }

            });
        };

        function excluir(id) {
            $('#modalLoad').modal('show');
            document.getElementById('mensagem').innerHTML = "";
            document.getElementById('tab').innerHTML = "";
            document.getElementById("pesquisa").value = "";

            $.ajax({
                type: "POST",
                url: "associados.aspx/ExcluirRegistro",
                data: '{id: ' + id + '}',
                contentType: "application/json; charset=utf-8",
                DataType: "json",
                success: function (msg) {
                    document.getElementById('tab').innerHTML = msg.d;

                    setTimeout(function () {
                        $('#modalLoad').modal('hide');
                    }, 500);
                }

            });

        };

        function excluirEmpresaAssociado(id, idEmpresa) {
            $('#modalLoad').modal('show');
            document.getElementById('mensagem').innerHTML = "";
            document.getElementById('tab').innerHTML = "";
            document.getElementById("pesquisa").value = "";

            $.ajax({
                type: "POST",
                url: "associados.aspx/ExcluirEmpresaAssociado",
                data: '{id: ' + id + ', idEmpresa: ' + idEmpresa + '}',
                contentType: "application/json; charset=utf-8",
                DataType: "json",
                success: function (msg) {
                    document.getElementById('tab').innerHTML = msg.d;

                    setTimeout(function () {
                        $('#modalLoad').modal('hide');
                    }, 500);
                }

            });

        };

    </script>
</asp:Content>
