$(document).on("ready", function () {
    var opcion_menu = "";

    var JsonMenus = "";
    var obtenerMenu = function () {
        
        $("#AppMenu").empty();
        opcion_menu = '<li>' +
            '<a href=' + root + 'Home/Index>' +
            '<span class="title">Products</span>' +
            '</a>' +
            '</li>';
        $("#AppMenu").append(opcion_menu);
        opcion_menu = "";
        $.ajax({
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            async: false,
            type: 'POST',
            url: '../Home/ObtenerMenu',
            success: function (resultados) {
                //var JsonArray = $.parseJSON(data);
                $.each(resultados, function (index, value) {
                    var jsonStr = JSON.stringify(value.Data);
                    oJson = $.parseJSON(jsonStr);
                    if (value.DataName == "opciones_menu") { JsonMenus = oJson; }
                    
                    $.each(JsonMenus, function (index, value) {
                        opcion_menu = opcion_menu +
                            '<li>' +
                            '<a href="'+root +value.path+'">' +                            
                            '<span class="title">' + value.menu + '</span>' +
                            '</a>' +
                            '</li>';
                        $("#AppMenu").append(opcion_menu);
                        opcion_menu = "";
                    });


                });
            },

            error: function (response) {
                alert("Error" + response);
            }
        });
    }
    obtenerMenu();
});