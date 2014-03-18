function AutorizarPost(postid) {
    var cb = "#CBA" + postid + ":checked";
    var value = 0;
    if($(cb).val() == 'true')
    {
        value = 1;
    }
    $.ajax({
        type: "POST",
        url: "/Post/AutorizarPost", 
        traditional: true,
        data: {id:postid, value:value}, 
    });
}

function PostPrincipal(postid) {
    var cb = "#CBM" + postid + ":checked";
    var value = 0;
    if ($(cb).val() == 'true') {
        value = 1;
    }
    $.ajax({
        type: "POST",
        url: "/Post/PostPrincipal",
        traditional: true,
        data: { id: postid, value: value },        
    });
}

function CambiarEstadoPost(postid) {
    var cb = "#CBE" + postid + ":checked";
    var value = 0;
    if ($(cb).val() == 'true') {
        value = 1;
    }
    $.ajax({
        type: "POST",
        url: "/Post/CambiarEstadoPost",
        traditional: true,
        data: { id: postid, value: value },        
    });
}

function DarFormato()
{
    var i=0;
    var caracter;
    var count;
    var cadena = $("#texto-sin-formato").text();
    var pi = 0;
    for(i; i<cadena.length; i++){
        caracter = cadena.charCodeAt(i);        
        if(caracter == "13" || caracter == "10"){
            if(cadena.charCodeAt(i+1) != "13" && cadena.charCodeAt(i+1) != "10"){
                pi=i;
            } 
            if(cadena.charCodeAt(i-1) != "13" && cadena.charCodeAt(i-1) != "10"){
                $("#texto-con-formato").append('<p>'+cadena.substring(pi,i)+'</p>');
            } 
        }        
    }
    if(cadena.charCodeAt(i) != "13" && cadena.charCodeAt(i) != "10"){        
        $("#texto-con-formato").append('<p>'+cadena.substring(pi,i)+'</p>');
    }
}