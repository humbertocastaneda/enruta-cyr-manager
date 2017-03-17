Imports MySql.Data.MySqlClient
Imports System.Data

Partial Class Default3
    Inherits paginaGenerica

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Request.QueryString("query") = "" Then
                getDataSet()
            End If
        End If
    End Sub


    Protected Sub dgOrdenes_PageIndexChanged(source As Object, e As DataGridPageChangedEventArgs) Handles dgOrdenes.PageIndexChanged
        dgOrdenes.CurrentPageIndex = e.NewPageIndex
        getDataSet()

    End Sub

    Sub getDataSet()
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString

        Dim conn As New MySqlConnection(cs)

        Try
            Dim selectSQL As String = Request.QueryString("query")

            Dim dsOrdenes As DataSet = conectarMySql(conn, selectSQL, "Ordenes", True)
            dgOrdenes.DataSource = dsOrdenes.Tables("ordenes")

            dgOrdenes.DataBind()
        Catch ex As Exception

        End Try

        Return
    End Sub

    Function estadoAnterior(estado As String) As String
        Dim resultado As String = ""

        If Not IsNothing(estado) Then
            If estado.Equals("EO005") Then
                resultado &= " (PAGADA)"
            ElseIf estado.Equals("EO008") Then
                resultado &= " (CANCELADA)"
            End If

        End If

        Return resultado
    End Function
End Class
