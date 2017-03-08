Imports MySql.Data.MySqlClient
Imports System.Data


Partial Class _default
    Inherits paginaGenerica

    Protected Sub message_generico(mensaje As String)
        lbldialogo.Text = mensaje
        mp1.Show()
    End Sub

    Public Function Autenticar(usuario As String, password As String) As Boolean

        Try
            'consulta a la base de datos
            Dim sql As String = "SELECT COUNT(*) canti " & _
                                "FROM Usuarios " & _
                                "WHERE userName = '" & usuario & "' AND password = '" & password & "'"

            Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString
            Dim conn As New MySqlConnection(cs)

            Dim ds As DataSet = paginaGenerica.conectarMySql(conn, sql, "usuarios", True)

            If ds.Tables(0).Rows(0).Item("canti") > 0 Then
                Return True
            Else
                lbl_msg.Text = "Verifique el usuario y la contraseña"
                message_generico("Verifique el usuario y la contraseña")
                ' messagebox("Verifique el usuario y la contraseña")
                Return False
            End If
        Catch ex As Exception
            lbl_msg.Text = "Error" & ex.Message
            message_generico("Error" & ex.Message)
            'messagebox("Error" & ex.Message)
            Return False
        End Try

    End Function



    Protected Sub Login1_Authenticate(sender As Object, e As AuthenticateEventArgs) Handles Login1.Authenticate

        If Autenticar(Login1.UserName, Login1.Password) Then
            If getRol(Login1.UserName).Equals("RU004") Then
                FormsAuthentication.SetAuthCookie(Login1.UserName, True)
                Response.Redirect("historicoSF.aspx")
                Return
            End If


            FormsAuthentication.RedirectFromLoginPage(Login1.UserName, True)

        Else
            'Response.Redirect("login.aspx", True)
        End If

        '  If Autenticar(Login1.UserName, Login1.Password) Then

        '      'FormsAuthentication.RedirectFromLoginPage(Login1.UserName, Login1.RememberMeSet)
        '      Dim tkt As FormsAuthenticationTicket
        '      Dim cookiestr As String
        '      Dim ck As HttpCookie

        '      tkt = New FormsAuthenticationTicket(1, Login1.UserName, DateTime.Now(), _
        'DateTime.Now.AddMinutes(30), Login1.RememberMeSet, "LogOn")
        '      cookiestr = FormsAuthentication.Encrypt(tkt)
        '      ck = New HttpCookie(FormsAuthentication.FormsCookieName(), cookiestr)
        '      If (Login1.RememberMeSet) Then ck.Expires = tkt.Expiration
        '      ck.Path = FormsAuthentication.FormsCookiePath()
        '      Response.Cookies.Add(ck)

        '      Dim strRedirect As String
        '      strRedirect = Request("default.aspx")
        '      If strRedirect <> "" Then
        '          Response.Redirect(strRedirect, True)
        '      Else
        '          strRedirect = "default.aspx"
        '          Server.Transfer(strRedirect)
        '      End If
        '  End If
    End Sub

    'Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
    '    If (HttpContext.Current.User.Identity.IsAuthenticated) Then
    '        Response.Redirect("verOrdenes.aspx")
    '    End If
    'End Sub
  

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            If Not Request.QueryString("log") = "" Then
                cerrarSession()
                Response.Redirect("default.aspx")
                Return
            End If
        End If

        'If (HttpContext.Current.User.Identity.IsAuthenticated) Then
        '    Response.Redirect("default.aspx")

        'End If

        'messagebox(getFecha("yyyyMMddHHmmss"))
    End Sub

End Class
