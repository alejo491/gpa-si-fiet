var Position, Tamaño
$(window).load(function () {
    Position = 1;
    Tamaño = $("#numPostCarrucel").text();
    setInterval("Next()", 7000);
});

function Next() {
    var nodoactual = "#Node" + Position;
    var Aactual = "#A" + Position;
    Position++;
    if (Position > Tamaño)
        Position = 1;
    var nodoproximo = "#Node" + Position;
    var Aproximo = "#A" + Position;
    $(nodoactual).css("display", "none");
    $(nodoproximo).fadeIn("slow");
    $(Aactual).attr("class", "Position");
    $(Aproximo).attr("class", "ASelected");
}
function Back() {
    var nodoactual = "#Node" + Position;
    var Aactual = "#A" + Position;
    Position--;
    if (Position < 1)
        Position = Tamaño;
    var nodoproximo = "#Node" + Position;
    var Aproximo = "#A" + Position;
    $(nodoactual).css("display", "none");
    $(nodoproximo).fadeIn("slow");
    $(Aactual).attr("class", "Position");
    $(Aproximo).attr("class", "ASelected");
}
function goToPosition(p) {
    var nodoactual = "#Node" + Position;
    var Aactual = "#A" + Position;
    if (p != Position)
        Position = p;
    var nodoproximo = "#Node" + Position;
    var Aproximo = "#A" + Position;
    $(nodoactual).css("display", "none");
    $(nodoproximo).fadeIn("slow");
    $(Aactual).attr("class", "Position");
    $(Aproximo).attr("class", "ASelected");
}