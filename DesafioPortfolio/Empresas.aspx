<%@ Page Title="Empresas" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Empresas.aspx.vb" Inherits="DesafioPortfolio.Empresas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
    <title>Empresas - AppSystem</title>   
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="True"></asp:ScriptManager>

        <div class="col-xl-12">
            <h3 class="page-header h3">Empresas</h3>
            <hr class="mb-2">

            <div class="card">
                <div class="card-body">
                    <div class="row ml-1 mt-0 mb-2">
                        <div id="divpesquisa" class="col-xl-10 mb-1 mt-1" style="display: block;">
                            <div class="input-group">
                                <input class="form-control fs-15px border-gray rounded" type="Search" placeholder="Pesquisar por Nome, CNPJ ou ID ..." id="pesquisa"><span class="btn btn-primary ml-2" onclick="selecionar();"><span class="fa fa-search mr-1"></span>Pesquisar</span>
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
                url: "empresas.aspx/SelecionarRegistros",
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
                url: "empresas.aspx/NovoRegistro",
                data: '{}',
                contentType: "application/json; charset=utf-8",
                DataType: "json",
                success: function (msg) {
                    document.getElementById('mensagem').innerHTML = msg.d;

                    $('.bootstrap-select').remove();
                    $('.selectpicker').selectpicker();

                    const cnpjInput = document.querySelector('#cnpj');
                    SomenteNumeros(cnpjInput);

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
            let cnpj = document.getElementById("cnpj").value;

            let selectAssociados = document.getElementById("cbassociados");
            let associados = "";

            for (var i = 0; i < selectAssociados.options.length; i++) {
                var option = selectAssociados.options[i];
                if (option.selected) {
                    associados += option.value + ",";
                }
            }

            if (associados.endsWith(",")) {
                associados = associados.slice(0, -1);
            }

            $.ajax({
                type: "POST",
                url: "empresas.aspx/AdicionarRegistro",
                data: '{nome: "' + nome + '", cnpj: "' + cnpj + '", associados: "' + associados + '"}',
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
                url: "empresas.aspx/EditarRegistro",
                data: '{id: ' + id + '}',
                contentType: "application/json; charset=utf-8",
                DataType: "json",
                success: function (msg) {
                    document.getElementById('mensagem').innerHTML = msg.d;

                    $('.bootstrap-select').remove();
                    $('.selectpicker').selectpicker();

                    const cnpjInput = document.querySelector('#cnpj');
                    SomenteNumeros(cnpjInput);

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
                url: "empresas.aspx/DetalhesRegistro",
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
            let id = document.getElementById("id").value;

            let selectAssociados = document.getElementById("cbassociados");
            let associados = "";

            for (var i = 0; i < selectAssociados.options.length; i++) {
                var option = selectAssociados.options[i];
                if (option.selected) {
                    associados += option.value + ",";
                }
            }

            if (associados.endsWith(",")) {
                associados = associados.slice(0, -1);
            }

            $.ajax({
                type: "POST",
                url: "empresas.aspx/AtualizarRegistro",
                data: '{nome: "' + nome + '", id: "' + id + '", associados: "' + associados + '"}',
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
                url: "empresas.aspx/ExcluirRegistro",
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

        function excluirAssociadoEmpresa(idAssociado, id) {
            $('#modalLoad').modal('show');
            document.getElementById('mensagem').innerHTML = "";
            document.getElementById('tab').innerHTML = "";
            document.getElementById("pesquisa").value = "";

            $.ajax({
                type: "POST",
                url: "empresas.aspx/ExcluirAssociadoEmpresa",
                data: '{idAssociado: ' + idAssociado + ', id: ' + id + '}',
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
