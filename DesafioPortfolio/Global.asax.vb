﻿Imports System.Web.Optimization

Public Class Global_asax
    Inherits HttpApplication

    Sub Application_Start(sender As Object, e As EventArgs)
        ' É acionado quando o aplicativo é iniciado
        RouteConfig.RegisterRoutes(RouteTable.Routes)
    End Sub
End Class