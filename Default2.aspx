<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default2.aspx.vb" Inherits="Default2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
      #divSample{width:150px;height:200px;overflow:auto}
</style>
</head>
    <script type="text/javascript" src="Scripts/jquery-2.0.3.js"></script>
    <script  type="text/javascript">
        function EnybyClipboard() {
            this.saveSelection = false;
            this.callback = false;
            this.pastedText = false;

            this.restoreSelection = function () {
                if (this.saveSelection) {
                    window.getSelection().removeAllRanges();
                    for (var i = 0; i < this.saveSelection.length; i++) {
                        window.getSelection().addRange(this.saveSelection[i]);
                    }
                    this.saveSelection = false;
                }
            };

            this.copyText = function (text) {
                var div = $('special_copy');
                if (!div) {
                    div = new Element('pre', {
                        'id': 'special_copy',
                        'style': 'opacity: 0;position: absolute;top: -10000px;right: 0;'
                    });
                    div.injectInside(document.body);
                }
                div.set('text', text);
                if (document.createRange) {
                    var rng = document.createRange();
                    rng.selectNodeContents(div);
                    this.saveSelection = [];
                    var selection = window.getSelection();
                    for (var i = 0; i < selection.rangeCount; i++) {
                        this.saveSelection[i] = selection.getRangeAt(i);
                    }
                    window.getSelection().removeAllRanges();
                    window.getSelection().addRange(rng);
                    setTimeout(this.restoreSelection.bind(this), 100);
                } else return alert('Copy not work. :(');
            };

            this.getPastedText = function () {
                if (!this.pastedText) alert('Nothing to paste. :(');
                return this.pastedText;
            };

            this.pasteText = function (callback) {
                var div = $('special_paste');
                if (!div) {
                    div = new Element('textarea', {
                        'id': 'special_paste',
                        'style': 'opacity: 0;position: absolute;top: -10000px;right: 0;'
                    });
                    div.injectInside(document.body);
                    div.addEvent('keyup', function () {
                        if (this.callback) {
                            this.pastedText = $('special_paste').get('value');
                            this.callback.call(null, this.pastedText);
                            this.callback = false;
                            this.pastedText = false;
                            setTimeout(this.restoreSelection.bind(this), 100);
                        }
                    }.bind(this));
                }
                div.set('value', '');
                if (document.createRange) {
                    var rng = document.createRange();
                    rng.selectNodeContents(div);
                    this.saveSelection = [];
                    var selection = window.getSelection();
                    for (var i = 0; i < selection.rangeCount; i++) {
                        this.saveSelection[i] = selection.getRangeAt(i);
                    }
                    window.getSelection().removeAllRanges();
                    window.getSelection().addRange(rng);
                    div.focus();
                    this.callback = callback;
                } else return alert('Fail to paste. :(');
            };
        }

        function copiar( text){
            enyby_clip = new EnybyClipboard(); //init 

            enyby_clip.copyText(text); // place this in CTRL+C handler and return true;
        }

           </script>



    
<body>

    <form id="form1" runat="server">
        <div id="divTextToClipboard">Este es el texto que quiero copiar</div>
    <button class="btnCopyToClipboard" onclick="copiar('hola')">Copiar texto</button>
    </form>
</body>
</html>
