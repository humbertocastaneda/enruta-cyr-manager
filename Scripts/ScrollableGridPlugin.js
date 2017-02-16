(function ($) {
    $.fn.Scrollable = function (options) {
        var defaults = {
            ScrollHeight: 300,
            Width: 0
        };
        var options = $.extend(defaults, options);
        return this.each(function () {
            var grid = $(this).get(0);
            var gridWidth = grid.offsetWidth;
            var gridHeight = grid.offsetHeight;
            var headerCellWidths = new Array();

            var gridRowOriginal = grid.getElementsByTagName("TR")[0];

            for (var i = 0; i < gridRowOriginal.getElementsByTagName("TD").length; i++) {
                headerCellWidths[i] = gridRowOriginal.getElementsByTagName("TD")[i].offsetWidth;
            }
            grid.parentNode.appendChild(document.createElement("div"));
            var parentDiv = grid.parentNode;

            var table = document.createElement("table");
            for (i = 0; i < grid.attributes.length; i++) {
                if (grid.attributes[i].specified && grid.attributes[i].name != "id") {
                    table.setAttribute(grid.attributes[i].name, grid.attributes[i].value);
                }
            }
            table.style.cssText = grid.style.cssText;
            table.style.width = (gridWidth) + "px"; //Tamaño del header
            table.appendChild(document.createElement("tbody"));
            table.getElementsByTagName("tbody")[0].appendChild(grid.getElementsByTagName("TR")[0]);
            //var cells = table.getElementsByTagName("TH");
            var cells = table.getElementsByTagName("TD");

            var gridRow = grid.getElementsByTagName("TR")[0];
            
            var debug="";
            for (var i = 0; i < cells.length; i++) {
                var width;
                //if (headerCellWidths[i] > gridRow.getElementsByTagName("TD")[i].offsetWidth) {
                    width = headerCellWidths[i];
                //}
                //else {
                //    width = gridRow.getElementsByTagName("TD")[i].offsetWidth;
                //}

                    debug += (width - 3)+", "

                cells[i].style.width = parseInt(width - 3) + "px";
                gridRow.getElementsByTagName("TD")[i].style.width = parseInt(width - 3) + "px";
            }
            parentDiv.removeChild(grid);
            //alert(debug);

            var dummyHeader = document.createElement("div");
            dummyHeader.appendChild(table);
            parentDiv.appendChild(dummyHeader);
            if (options.Width > 0) {
                gridWidth = options.Width;
            }
            var scrollableDiv = document.createElement("div");
            if (parseInt(gridHeight) > options.ScrollHeight) {
                gridWidth = parseInt(gridWidth) + 17;
            }
            scrollableDiv.style.cssText = "overflow:auto;height:" + options.ScrollHeight + "px; width:" + gridWidth + "px"; //Tamaño de la tabla
            scrollableDiv.appendChild(grid);
            parentDiv.appendChild(scrollableDiv);

            $(scrollableDiv).attr('id', "gridFrozen");
        });
    };

    //$.fn.cambiaResolucion = function (options) {
    //    var defaults = {
    //        ScrollHeight: 500,
    //        Width: 0
    //    };
    //    var options = $.extend(defaults, options);
    //    return this.each(function () {
    //        var grid = $(this).get(0);
    //        var gridWidth = $(document).width() ;
    //        var gridHeight = grid.offsetHeight;
    //        var headerCellWidths = new Array();

    //        var gridRowOriginal = grid.getElementsByTagName("TR")[0];

    //        for (var i = 0; i < gridRowOriginal.getElementsByTagName("TD").length; i++) {
    //            headerCellWidths[i] = gridRowOriginal.getElementsByTagName("TD")[i].offsetWidth;
    //        }
    //        grid.parentNode.appendChild(document.createElement("div"));
    //        var parentDiv = grid.parentNode;

    //        var table = document.createElement("table");
    //        for (i = 0; i < grid.attributes.length; i++) {
    //            if (grid.attributes[i].specified && grid.attributes[i].name != "id") {
    //                table.setAttribute(grid.attributes[i].name, grid.attributes[i].value);
    //            }
    //        }
    //        table.style.cssText = grid.style.cssText;
    //        table.style.width = (gridWidth) + "px"; //Tamaño del header
    //        table.appendChild(document.createElement("tbody"));
    //        table.getElementsByTagName("tbody")[0].appendChild(grid.getElementsByTagName("TR")[0]);
    //        var cells = table.getElementsByTagName("TH");
    //        var cells = table.getElementsByTagName("TD");

    //        var gridRow = grid.getElementsByTagName("TR")[0];

    //        var debug = "";
    //        for (var i = 0; i < cells.length; i++) {
    //            var width;
    //            if (headerCellWidths[i] > gridRow.getElementsByTagName("TD")[i].offsetWidth) {
    //            width = headerCellWidths[i];
    //            }
    //            else {
    //                width = gridRow.getElementsByTagName("TD")[i].offsetWidth;
    //            }

    //            debug += (width - 3) + ", "

    //            cells[i].style.width = parseInt(width - 3) + "px";
    //            gridRow.getElementsByTagName("TD")[i].style.width = parseInt(width - 3) + "px";
    //        }
    //        parentDiv.removeChild(grid);
    //        alert(debug);

    //        var dummyHeader = document.createElement("div");
    //        dummyHeader.appendChild(table);
    //        parentDiv.appendChild(dummyHeader);
    //        if (options.Width > 0) {
    //            gridWidth = options.Width;
    //        }
    //        var scrollableDiv = document.getElementById("gridFrozen");
    //        if (parseInt(gridHeight) > options.ScrollHeight) {
    //            gridWidth = parseInt(gridWidth) + 17;
    //        }
    //        scrollableDiv.style.cssText = "overflow:auto;height:" + options.ScrollHeight + "px; width:" + gridWidth + "px"; //Tamaño de la tabla
    //        scrollableDiv.appendChild(grid);
    //        parentDiv.appendChild(scrollableDiv);
    //        alert("Holi");
    //    });
    //};

   
}

)(jQuery);