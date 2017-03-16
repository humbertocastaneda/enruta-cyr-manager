
Partial Class Default2
    Inherits System.Web.UI.Page


    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'preguntar("Quieres mostrar el siguiente mensaje?")
        messagebox("Hola Humberto!")

        messagebox()
    End Sub

    Public Sub messagebox(message As String)

        Dim sb As New System.Text.StringBuilder()
        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", sb.ToString())
    End Sub

    Public Sub messagebox()
        Dim sb As New System.Text.StringBuilder()
        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("disp_confirm()")
        sb.Append("};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", sb.ToString())
    End Sub

    Public Sub preguntar(message As String)

        Dim sb As New System.Text.StringBuilder()



        'sb.Append("return confirm('")
        'sb.Append(message)
        'sb.Append("');")

        'ClientScript.RegisterOnSubmitStatement(Me.GetType(), "alert", sb.ToString())

        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("return confirm('")
        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", sb.ToString())
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
       

    End Sub
End Class
