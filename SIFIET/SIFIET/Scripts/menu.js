$.fn.extend({
    botonMenu: function () {
        this.each(function () {
            $(this).on("click", mensaje);
        });
    }
});

function mensaje() {
    $(this).children(0).toggle("fast");
    if ($(this).hasClass("seleccionado")) {
        $(this).removeClass("seleccionado");
    } else {
        $(this).addClass("seleccionado");
    }
}


$(document).on("ready", main);

function main() {
    $(".boton").botonMenu();
    
}