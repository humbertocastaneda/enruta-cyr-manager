<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default2.aspx.vb" Inherits="Default2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>


    <script type="text/javascript">
        function muestraMensaje() {
                return confirm("Aún hay ordene(s) por ser asignadas, ¿Desea continuar?");
        }
    </script>

<body>

    <form id="form1" runat="server">
        <asp:Button ID="Button1" runat="server" Text="Button" OnClientClick="return muestraMensaje()" />
    </form>
</body>
</html>
