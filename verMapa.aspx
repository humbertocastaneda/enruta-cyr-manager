<%@ Page Language="VB" AutoEventWireup="false" CodeFile="verMapa.aspx.vb" Inherits="verMapa" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
     <title>
          Adding Markers to a Google Maps from database
     </title>
 
     <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>
    <script type="text/javascript">

        function setCookie(name, value, expires) {
            document.cookie = name + "=" + value + "; path=/" + ((expires == null) ? "" : "; expires=" + expires.toGMTString());
        }
    </script>
</head>
 
<body onload="initialize()">
     <form id="form1" runat="server">
          <div id="mapArea" style="width: 500px; height: 500px;">
          </div>
 
          <asp:Literal ID="Literal1" runat="server"></asp:Literal>
     </form>
</body>
</html>
