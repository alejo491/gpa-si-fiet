$(window).load(function () {
    var postid = $("#idpost").text();
    setInterval("actualizar_num_likes('"+postid+"')", 10000);
    setInterval("actualizar_prom_start('"+postid+"')", 10000);
    setInterval("MostrarComentarios('"+postid+"')", 10000);
});
var qualification;
var bandera;
function hover(idstar) {
    var div;
    for(var i = 1; i <= idstar; i++){
        div = "#star" + i;
        $(div).attr("class", "startselected");
    }
}

function unhover() {
    for (var i = 1; i <= 5; i++) {
        div = "#star" + i;
        if(i <= qualification)
            $(div).attr("class", "startselecteduser");
        else
            $(div).attr("class", "startunselected");
    }
}

function setQualification(q,b){
    qualification = q;
    bandera = b;
}

function calificar(cal, postid, start){
    if(start == "" && bandera == "0"){    
        $.ajax({
            type: "POST",
            url: "/Startbox/Create", 
            traditional: true,
            data: {idPost:postid, q:cal},
            async:false,
            success : function(data){
                for (var i = 1; i <= 5; i++) {
                    div = "#star" + i;
                    $(div).unbind("click");
                    if(i=="1"){$(div).bind("click", function() {calificar('1', postid, data)});}
                    if(i=="2"){$(div).bind("click", function() {calificar('2', postid, data)});}   
                    if(i=="3"){$(div).bind("click", function() {calificar('3', postid, data)});}   
                    if(i=="4"){$(div).bind("click", function() {calificar('4', postid, data)});}
                    if(i=="5"){$(div).bind("click", function() {calificar('5', postid, data)});}                 
                }
                bandera = 1;
                qualification = cal;
                unhover();
            }
        });
    }
    if(start != "" && bandera == 1){
        $.ajax({
            type: "POST",
            url: "/Startbox/Edit", 
            traditional: true,
            data: {idstar:start,q:cal},
            async:false,
        });
        qualification = cal;
        unhover();
    }
    actualizar_prom_start(postid);
}

function ClickLike(postid, tipo, like) {   
    if (tipo == 0) {
        $.ajax({
            type: "POST",
            url: "/Like/Create", 
            traditional: true,
            data: {post:postid},
            async:false,
            success : function(data){
                $("#LinknoLike").unbind("click");
                $("#LinknoLike").bind("click", function() {ClickLike(postid, '1', data)});
            }
        });
        actualizar_num_likes(postid);
        $("#LinkLike").css("display", "none");
        $("#LinknoLike").css("display", "block");       
    }
    if (tipo == 1) {
        $.ajax({
            type: "POST",
            url: "/Like/DeleteConfirmed", 
            traditional: true,
            data: {post:postid, id: like},  
            async:false,
        });
        actualizar_num_likes(postid);
        $("#LinkLike").css("display", "block");
        $("#LinknoLike").css("display", "none");         
    }
    
    
}   

function actualizar_num_likes(postid){
        $.ajax({
            type: "POST",
            url: "/Like/get_likes", 
            traditional: true,
            data: {id:postid},
            success : function(data){
                $("#NumLike").html("&nbsp&nbsp&nbsp" + data + " Usuarios les Gusta");  
            }
        });
}
function actualizar_prom_start(postid){
        $.ajax({
            type: "POST",
            url: "/Startbox/Index", 
            traditional: true,
            data: {id:postid},
            success : function(data){
                $("#PromStart").html("&nbsp&nbsp&nbsp" + data + " De calificación promedio");  
            }
        });
}

function Comentar(postid){
    var comentario = $("#comment").val();
    $.ajax({
            type: "POST",
            url: "/Comment/Create", 
            traditional: true,
            data: {id:postid, text:comentario}            
        });
    //$("#comments").prepend('<div class="comentario">'+comentario+'</div>');
    $("#comment").val("");
    MostrarComentarios(postid);
}
function MostrarComentarios(postid){
    $("#loading").css("display", "block");  
    $.ajax({
        type: "POST",
        url: "/Comment/Index", 
        traditional: true,
        data: {id:postid},     
        success : function(data){
            $("#loading").css("display", "none");  
            $("#comments").html(data);  
        }  
    });     
}