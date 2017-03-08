﻿Imports MySql.Data.MySqlClient
Imports System.Data

Partial Class _Default
    Inherits paginaGenerica

    Public nombre, apPat, apMat, apodo, estado As String
    Public idEmpleado, nomina As Long
    Dim esNuevo As Boolean = False

    Dim SIN_SELECCION As Integer = 0
    Dim SELECCIONAR_EMPLEADO As Integer = 1
    Dim NUEVO_EMPLEADO As Integer = 2

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'If Not isLoged() Then
        '    Return
        'End If

        If Not IsPostBack Then
            setMenu(contenedor)
            Try
                actualizaListaTecnicos()
            Catch ex As Exception

            End Try




            ManejaEstadoBotones(SIN_SELECCION)


        End If
        If Not Me.ViewState("idEmpleado") = Nothing Then
            idEmpleado = CType(Me.ViewState("idEmpleado"), Long)

        End If

        If Not Me.ViewState("esNuevo") = Nothing Then
            esNuevo = CType(Me.ViewState("esNuevo"), Boolean)

        End If


    End Sub

    Public Sub actualizaListaTecnicos()
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString
        Dim conn As New MySqlConnection(cs)
        Try
            conn.Open()
            lbEmpleados.Items.Clear()

            Dim selectSQL = "Select idEmpleado idEmpleado ,  if(isnull(alias) or LENGTH(alias)=0, CONCAT_WS(' ',nombre,  appat, apmat), alias) nombreCompleto  from empleadoslect order by if(isnull(alias) or LENGTH(alias)=0, CONCAT_WS(' ',nombre,  appat, apmat), alias)"

           
            Dim pubs As DataSet = conectarMySql(conn, selectSQL, "empleadoslect", False)
            lbEmpleados.DataSource = pubs
            lbEmpleados.DataTextField = "nombreCompleto"
            lbEmpleados.DataValueField = "idEmpleado"





            lbEmpleados.DataBind()



        Catch ex As Exception
            Throw ex

        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try


    End Sub

    Public Sub actualizaArchivoDeUsuarios()

    End Sub

    Public Sub ManejaEstadoBotones(choose As Integer)
        ib_cancelar.Visible = True
        'ib_eliminar.Visible = True
        ib_guardar.Visible = True
        ib_nuevo.Visible = True
        lbEmpleados.Enabled = True
        datos.Visible = True
        Me.txtNomina.Enabled = False
        Select Case choose
            Case SIN_SELECCION
                ib_cancelar.Visible = False
                'ib_eliminar.Visible = False
                ib_guardar.Visible = False
                datos.Visible = False
            Case NUEVO_EMPLEADO
                'ib_eliminar.Visible = False
                ib_nuevo.Visible = False
                lbEmpleados.Enabled = False
                Me.txtNomina.Enabled = True
                ddl_estadosTecnico.SelectedValue = "EG001"
        End Select

    End Sub

    Protected Sub lbEmpleados_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbEmpleados.SelectedIndexChanged
        setDatos(lbEmpleados.SelectedValue)
    End Sub


    Protected Sub setDatos(idEmpleado As String)
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString
        Dim conn As New MySqlConnection(cs)
        Try
            conn.Open()
            'lbEmpleados.Items.Clear()

            Dim selectSQL = "Select idempleado , CONCAT_WS(' ', nombre, appat, apmat)nombreCompleto, nombre, appat, apmat, alias, nomina, estado  from empleadoslect where idempleado=" & idEmpleado

            Dim ds As DataSet = conectarMySql(conn, selectSQL, "empleadoslect", False)

            Dim row As DataRow = ds.Tables(0).Rows(0)

            nombre = row("nombre")
            apPat = row("appat")
            apMat = row("apMat")
            nomina = row("nomina")
            If Not IsDBNull(row("alias")) Then
                apodo = row("alias")
            End If
            estado = row("estado")
            ddl_estadosTecnico.SelectedValue = estado

            Me.idEmpleado = row("idEmpleado")

            Me.ViewState("idEmpleado") = idEmpleado

        Catch ex As Exception
            Throw ex

        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try

        ManejaEstadoBotones(SELECCIONAR_EMPLEADO)
        datos.DataBind()
    End Sub

    Protected Sub ib_guardar_Click(sender As Object, e As EventArgs) Handles ib_guardar.Click
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString
        Dim conn As New MySqlConnection(cs)
        Dim cmd As New MySqlCommand()

        Try
            conn.Open()

            cmd.Connection = conn

            Dim ls_statement As String

            If Not esNuevo Then
                ls_statement = "Update empleadoslect set nombre='" & txtNombre.Text & "'" & _
                    ", apPat='" & txtApPat.Text & "', " & "apMat='" & txtApMat.Text & "'" & _
                    ", nomina=" & txtNomina.Text & ", alias='" & txtAlias.Text & "', estado='" & ddl_estadosTecnico.SelectedValue & "' where idEmpleado=" & idEmpleado

            Else
                ls_statement = "insert into empleadoslect( nombre, appat, apmat, nomina, alias, estado) values " & _
                    "('" & txtNombre.Text & "'" & _
                    ", '" & txtApPat.Text & "', " & "'" & txtApMat.Text & "', " & txtNomina.Text & ", '" & txtAlias.Text & "'" & ", '" & ddl_estadosTecnico.SelectedValue & "')"
            End If
            cmd.CommandText = ls_statement

            cmd.ExecuteNonQuery()


            actualizaListaTecnicos()

            If esNuevo Then

                setDatos(getLastInsertedId(conn, False))
                Me.ViewState("esNuevo") = False


                'seleccionamos el ultimo, debe ser el ultimo insertado
                lbEmpleados.SelectedIndex = lbEmpleados.Items.Count - 1
                ManejaEstadoBotones(SELECCIONAR_EMPLEADO)
            Else
                buscarEmpleadoLista(idEmpleado)

            End If

            getCadenaUsuarios(conn)
            messagebox("Técnico guardado")
        Catch ex As Exception
            messagebox("Ocurrio un problema al guardar: " & ex.Message)
        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If

        End Try
    End Sub


    Public Sub buscarEmpleadoLista(selected As Integer)
        For i As Integer = 0 To lbEmpleados.Items.Count - 1
            If lbEmpleados.Items(i).Value = selected Then
                lbEmpleados.SelectedIndex = i
                Return
            End If

        Next
    End Sub

    Protected Sub ib_nuevo_Click(sender As Object, e As EventArgs) Handles ib_nuevo.Click
        clearData()
        esNuevo = True
        Me.ViewState("esNuevo") = esNuevo
        ManejaEstadoBotones(NUEVO_EMPLEADO)
    End Sub

    Public Sub clearData()
        idEmpleado = 0
        nombre = ""
        apPat = ""
        apMat = ""
        datos.DataBind()
        lbEmpleados.ClearSelection()
    End Sub

    Protected Sub ib_cancelar_Click(sender As Object, e As EventArgs) Handles ib_cancelar.Click
        lbEmpleados.Enabled = True
        esNuevo = False
        Me.ViewState("esNuevo") = esNuevo
        If lbEmpleados.SelectedIndex >= 0 Then
            setDatos(idEmpleado)
            ManejaEstadoBotones(SELECCIONAR_EMPLEADO)
        Else
            ManejaEstadoBotones(SIN_SELECCION)
        End If
    End Sub

    Protected Sub ib_eliminar_Click(sender As Object, e As EventArgs) Handles ib_eliminar.Click
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString
        Dim conn As New MySqlConnection(cs)
        Dim cmd As New MySqlCommand()

        Try
            conn.Open()

            cmd.Connection = conn

            Dim ls_statement As String


            ls_statement = "delete from empleadoslect where idEmpleado=" & idEmpleado

            cmd.CommandText = ls_statement

            cmd.ExecuteNonQuery()


            actualizaListaTecnicos()
            clearData()
            ManejaEstadoBotones(SIN_SELECCION)
            getCadenaUsuarios(conn)

        Catch ex As Exception

        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If

        End Try
    End Sub

End Class
