Public Class HtmlClass

    Public Shared Function Alerta(tipo As Integer, titulo As String, texto As String) As String

        Dim alert As String = "<div class='alert ###classAlert alert-dismissible fade show m-20' role='alert'><strong><font style='vertical-align: inherit;'><font style='vertical-align: inherit;'>###Title</font></font></strong><font style='vertical-align: inherit;'><font style='vertical-align: inherit;'> ###Text
                              </font></font><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button></div>"

        Select Case tipo
            Case 1 '//Primary
                alert = alert.Replace("###classAlert", "alert-primary").Replace("###Title", titulo).Replace("###Text", texto)

            Case 2 '//Secondary
                alert = alert.Replace("###classAlert", "alert-secondary").Replace("###Title", titulo).Replace("###Text", texto)

            Case 4 '//Info
                alert = alert.Replace("###classAlert", "alert-info").Replace("###Title", titulo).Replace("###Text", texto)

            Case 5 '//Warning
                alert = alert.Replace("###classAlert", "alert-warning").Replace("###Title", titulo).Replace("###Text", texto)

            Case 7 '//Light
                alert = alert.Replace("###classAlert", "alert-light").Replace("###Title", titulo).Replace("###Text", texto)

            Case 8 '//Dark
                alert = alert.Replace("###classAlert", "alert-dark").Replace("###Title", titulo).Replace("###Text", texto)

            Case 10 '//Success
                alert = alert.Replace("###classAlert", "alert-success").Replace("###Title", titulo).Replace("###Text", texto)

            Case 99 '//Danger
                alert = alert.Replace("###classAlert", "alert-danger").Replace("###Title", titulo).Replace("###Text", texto)
        End Select

        Return alert

    End Function

End Class
