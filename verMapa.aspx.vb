Imports System.Data
Imports MySql.Data.MySqlClient

Partial Class verMapa
    Inherits paginaGenerica

    ReadOnly VER_RUTA = 0
    ReadOnly VER_GPS = 1


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Establecemos el titulo de la ventana

        'markers = GetMarkers()
        Dim cs As String = ConfigurationManager.ConnectionStrings(getBD()).ConnectionString


        Dim conn As New MySqlConnection(cs)

        
        Try
            Me.Title = Request.QueryString("titulo")
            conn.Open()
            'Obtenemos el query de la ventana pasada
            Dim selectSQL = Request.QueryString("q")
            Dim ds As DataSet

            Select Case Request.QueryString("tipo")
                Case VER_RUTA
                    ds = conectarMySql(conn, selectSQL, "lecutras", False)
                    verPuntosConRuta(ds)
                Case VER_GPS
                    ds = conectarMySql(conn, selectSQL, "rutaGPS", False)
                    verPuntosGPS(ds)
            End Select

        Catch ex As Exception
            Literal1.Text = "<script type='text/javascript'>" & _
                "function initialize() {" & _
                "}" & _
        "</script>"
        Finally
            If Not IsNothing(conn) Then
                conn.Close()
            End If
        End Try
       


    End Sub

    Sub verPuntosGPS(ds As DataSet)
        

        Dim path As String = ""
        Dim pathIntermitente = ""
        Dim paths As New ArrayList
        Dim estadoAnterior = ""

        Dim markers = ""
        Dim ruta = ""
        Dim centro = ""

        'Vamos a obtener el path
        For Each row As DataRow In ds.Tables("rutaGPS").Rows
            If row.Item("latitud").ToString.Equals("0.0") Or row.Item("latitud").ToString.Trim.Equals("") Then
                Continue For
            End If
            path += "new google.maps.LatLng(" & row.Item("latitud") & "," & row.Item("longitud") & "), "

            If Not row("tipo").ToString.Equals(estadoAnterior) And Not estadoAnterior.Equals("") And Not (estadoAnterior.Equals("TP000") Or estadoAnterior.Equals("TP003")) Then
                pathIntermitente += "new google.maps.LatLng(" & row.Item("latitud") & "," & row.Item("longitud") & ") "
                paths.Add(estadoAnterior + "|" + pathIntermitente)
                pathIntermitente = ""
            ElseIf Not row("tipo").ToString.Equals("TP000") And Not row("tipo").ToString.Equals("TP003") Then
                pathIntermitente += "new google.maps.LatLng(" & row.Item("latitud") & "," & row.Item("longitud") & "), "
            End If

            estadoAnterior = row("tipo").ToString

        Next

        If Not pathIntermitente.Equals("") Then
            path = path.Substring(0, path.Length - 2)
            pathIntermitente = pathIntermitente.Substring(0, pathIntermitente.Length - 2)
            paths.Add(estadoAnterior + "|" + pathIntermitente)
            pathIntermitente = ""
        End If

        ruta &= getPath(0, path, "#0000FF")
        Dim consecutivo As Integer = 1
        For Each camino As String In paths
            Dim elementos = camino.Split("|")
            If elementos(0).Equals("TP002") Then
                ruta &= getPath(consecutivo, elementos(1), "#ff0000")


            Else
                ruta &= getPath(consecutivo, elementos(1), "#ffff00")
            End If
            consecutivo += 1
        Next



        consecutivo = 0



        For Each row As DataRow In ds.Tables("rutaGPS").Rows
            If row.Item("latitud").ToString.Equals("0.0") Or row.Item("latitud").ToString.Trim.Equals("") Then
                Continue For
            End If
            If Not IsDBNull(row.Item("latitud")) Then

                Dim ultima = ds.Tables("rutaGPS").Rows.Count - 1 = consecutivo
                Dim primera = consecutivo = 0

                centro = row.Item("latitud") & ", " & row.Item("longitud")
                If row("tipo").ToString.Equals("TP001") Then
                    If primera Then
                        markers &= GetMarkers(consecutivo, row.Item("latitud"), row.Item("longitud"), "http://maps.google.com/mapfiles/kml/paddle/ylw-stars_maps.png", _
                                          "<b>Punto no Obtenido</b> <br>Fecha " & row.Item("fecha"))
                    ElseIf ultima Then
                        markers &= GetMarkers(consecutivo, row.Item("latitud"), row.Item("longitud"), "http://maps.google.com/mapfiles/kml/paddle/ylw-circle_maps.png", _
                                          "<b>Punto no Obtenido</b> <br>Fecha " & row.Item("fecha"))
                    Else
                        markers &= GetMarkers(consecutivo, row.Item("latitud"), row.Item("longitud"), "http://maps.google.com/mapfiles/kml/paddle/ylw-blank_maps.png", _
                                          "<b>Punto no Obtenido</b> <br>Fech " & row.Item("fecha"))
                    End If


                ElseIf row("tipo").ToString.Equals("TP002") Then
                    If primera Then
                        markers &= GetMarkers(consecutivo, row.Item("latitud"), row.Item("longitud"), "http://maps.google.com/mapfiles/kml/paddle/red-stars_maps.png", _
                                          "<b>Celular Apagado</b> <br>Fecha " & row.Item("fecha"))
                    ElseIf ultima Then
                        markers &= GetMarkers(consecutivo, row.Item("latitud"), row.Item("longitud"), "http://maps.google.com/mapfiles/kml/paddle/red-circle_maps.png", _
                                          "<b>Celular Apagado</b> <br>Fecha " & row.Item("fecha"))
                    Else
                        markers &= GetMarkers(consecutivo, row.Item("latitud"), row.Item("longitud"), "http://maps.google.com/mapfiles/kml/paddle/red-blank_maps.png", _
                                          "<b>Celular Apagado</b> <br>Fecha " & row.Item("fecha"))
                    End If

                ElseIf row("tipo").ToString.Equals("TP003") Then

                    If primera Then
                        markers &= GetMarkers(consecutivo, row.Item("latitud"), row.Item("longitud"), "http://maps.google.com/mapfiles/kml/paddle/grn-stars_maps.png", _
                                          "<b>Celular Encendido</b> <br>Fecha " & row.Item("fecha"))
                    ElseIf ultima Then
                        markers &= GetMarkers(consecutivo, row.Item("latitud"), row.Item("longitud"), "http://maps.google.com/mapfiles/kml/paddle/grn-circle_maps.png", _
                                          "<b>Celular Encendido</b> <br>Fecha " & row.Item("fecha"))
                    Else
                        markers &= GetMarkers(consecutivo, row.Item("latitud"), row.Item("longitud"), "http://maps.google.com/mapfiles/kml/paddle/grn-blank_maps.png", _
                                         "<b>Celular Encendido</b> <br>Fecha " & row.Item("fecha"))
                    End If



                Else

                    If primera Then
                        markers &= GetMarkers(consecutivo, row.Item("latitud"), row.Item("longitud"), "http://maps.google.com/mapfiles/kml/paddle/blu-stars_maps.png", _
                                          "<b>Punto</b> <br>Fecha " & row.Item("fecha"))
                    ElseIf ultima Then
                        markers &= GetMarkers(consecutivo, row.Item("latitud"), row.Item("longitud"), "http://maps.google.com/mapfiles/kml/paddle/blu-circle_maps.png", _
                                                               "<b>Punto</b> <br>Fecha de Ejecucion " & row.Item("fecha"))
                    Else
                        markers &= GetMarkers(consecutivo, row.Item("latitud"), row.Item("longitud"), "http://maps.google.com/mapfiles/kml/paddle/blu-blank_maps.png", _
                                                               "<b>Punto</b> <br>Fecha de Ejecucion " & row.Item("fecha"))
                    End If


                End If

            End If
            consecutivo += 1
        Next
        inicia(centro, markers, ruta)
    End Sub

    Sub verPuntosConRuta(ds As DataSet)
        Dim markers = ""
        Dim ruta = ""
        Dim path As String = ""
        'For Each row As DataRow In ds.Tables("Ordenes").Rows
        '    If row.Item("latitud").ToString.Equals("0.0") Or row.Item("latitud").ToString.Trim.Equals("") Then
        '        Continue For
        '    End If
        '    path &= "new google.maps.LatLng(" & row.Item("latitud") & "," & row.Item("longitud") & "), "
        'Next

        'If path.Length > 0 Then
        '    path = path.Substring(0, path.Length - 2)
        'End If

        ruta = "" 'getPath(0, path, "#0000FF")

        Dim i As Integer = 0
        Dim centro As String = ""
        For Each row As DataRow In ds.Tables(0).Rows
            If row.Item("latitud").ToString.Equals("0.0") Or row.Item("latitud").ToString.Trim.Equals("") Then
                Continue For
            End If
            If Not IsDBNull(row.Item("latitud")) Then

                markers &= GetMarkers(i, row.Item("latitud"), row.Item("longitud"), "http://maps.google.com/mapfiles/kml/paddle/blu-circle_maps.png", "<b>Clave Usuario " & row.Item("clave_usuario") & " " & row.Item("direccion") & " " & row.Item("colonia") & "</b> <br>Fecha de Lectura " & row.Item("fechadeejecucion"))
                centro = row.Item("latitud") & ", " & row.Item("longitud")
            End If

            i += 1

        Next
        inicia(centro, markers, ruta)

    End Sub

    Public Sub inicia(centro As String, markers As String, ruta As String)
        Dim zoom = "13"
        If Not IsNothing(Request.Cookies("zoom")) Then
            zoom = Request.Cookies("zoom").Value
        End If

        Dim listener = "google.maps.event.addListener(myMap, 'click', function(evento) {" & _
                      "if (infoWindow){" & _
                            "infoWindow.close();" & _
                        "}" & _
                      "});"

        Literal1.Text = "<script type='text/javascript'>" & _
        "var infoWindow; function initialize() {" & _
        "var bounds = new google.maps.LatLngBounds(); var mapOptions = {" & _
        "center: new google.maps.LatLng(" & centro & ")," & _
        "zoom: " & zoom & "," & _
        "mapTypeId : google.maps.MapTypeId.ROADMAP" & _
        "};" & _
        "var myMap = new google.maps.Map(document.getElementById('mapArea'), mapOptions);" & _
        markers & _
         ruta & _
         listener & _
         "google.maps.event.addListener(myMap, 'zoom_changed', function() {" & _
    "mapzoom = myMap.getZoom();" & _
    "var exp = new Date(); exp.setTime(exp.getTime() + (1000 * 60 * 60 * 24 * 30));setCookie(""zoom"",mapzoom, exp);" & _
"});" & _
 "	document.getElementById('mapArea').style.height=window.outerHeight - 100 + ""px"";" & _
        "	document.getElementById('mapArea').style.width=window.outerWidth + ""px""; myMap.fitBounds(bounds);" & _
        "}" & _
        "</script>"
    End Sub

    Protected Function GetMarkers(id As String, lat As String, len As String, Optional icon As String = "", Optional text As String = "") As String
        Dim markers As String = ""
        Dim opcional As String = ""
        Dim listener As String = ""

        If Not icon.Equals("") Then
            opcional &= "icon: '" & icon & "', "
        End If

        If Not text.Equals("") Then
            listener = "google.maps.event.addListener(marker" & id & ", 'click', function() {" & _
                "if (infoWindow) infoWindow.close();" & _
                    "  infoWindow=new google.maps.InfoWindow();" & _
                      "infoWindow.setContent(""<div style= 'font-family:Calibri'>" + text + "</div>"");" & _
                      " infoWindow.open(myMap, this); " & _
                      "});"
        End If

        markers = markers & "var marker" & id & "= new google.maps.Marker({" & _
        "position: new google.maps.LatLng( " & lat & ", " & _
       len & ")," & _
       opcional & _
        "map: myMap});bounds.extend(new google.maps.LatLng( " & lat & ", " & _
       len & "));" & _
        listener
        Return markers
    End Function

    Protected Function getPath(id As String, direcciones As String, Optional color As String = "#ff0060") As String
        Dim markers As String = ""



        'Inicializamos la linea

        markers = markers + "var line" + id + "= new google.maps.Polyline({" & _
"strokeColor:  """ & color & """, " & _
        "strokeOpacity: 1.0," & _
        "strokeWeight: 2" & _
        "});"

        'Le indicamos las coordenadas
        '      markers &= " var direcciones = [" & _
        '  "new google.maps.LatLng(25.6667, -100.3167)," & _
        '  "new google.maps.LatLng(24.9667, -100.55)" & _
        '"];"

        markers &= " var direcciones" + id + " = [" & direcciones & "];"

        markers &= " line" & id & ".setPath(direcciones" + id + ");"
        markers &= " line" & id & ".setMap(myMap);"

        Return markers
    End Function

End Class