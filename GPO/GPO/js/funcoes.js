/* cabecera_visible.js compiled from X 4.01 with XC 0.29b. Distributed under GNU LGPL. For copyrights, license, documentation and more visit Cross-Browser.com */
var xOp7Up,xOp6Dn,xIE4Up,xIE4,xIE5,xNN4,xUA=navigator.userAgent.toLowerCase();if(window.opera){var i=xUA.indexOf('opera');if(i!=-1){var v=parseInt(xUA.charAt(i+6));xOp7Up=v>=7;xOp6Dn=v<7;}}else if(navigator.vendor!='KDE' && document.all && xUA.indexOf('msie')!=-1){xIE4Up=parseFloat(navigator.appVersion)>=4;xIE4=xUA.indexOf('msie 4')!=-1;xIE5=xUA.indexOf('msie 5')!=-1;}else if(document.layers){xNN4=true;}xMac=xUA.indexOf('mac')!=-1;function xDef(){for(var i=0; i<arguments.length; ++i){if(typeof(arguments[i])=='undefined') return false;}return true;}function xGetElementById(e){if(typeof(e)=='string') {if(document.getElementById) e=document.getElementById(e);else if(document.all) e=document.all[e];else e=null;}return e;}function xLeft(e, iX){if(!(e=xGetElementById(e))) return 0;var css=xDef(e.style);if (css && xStr(e.style.left)) {if(xNum(iX)) e.style.left=iX+'px';else {iX=parseInt(e.style.left);if(isNaN(iX)) iX=0;}}else if(css && xDef(e.style.pixelLeft)) {if(xNum(iX)) e.style.pixelLeft=iX;else iX=e.style.pixelLeft;}return iX;}function xMoveTo(e,x,y){xLeft(e,x);xTop(e,y);}function xNum(){for(var i=0; i<arguments.length; ++i){if(isNaN(arguments[i]) || typeof(arguments[i])!='number') return false;}return true;}function xScrollTop(e, bWin){var offset=0;if (!xDef(e) || bWin || e == document || e.tagName.toLowerCase() == 'html' || e.tagName.toLowerCase() == 'body') {var w = window;if (bWin && e) w = e;if(w.document.documentElement && w.document.documentElement.scrollTop) offset=w.document.documentElement.scrollTop;else if(w.document.body && xDef(w.document.body.scrollTop)) offset=w.document.body.scrollTop;}else {e = xGetElementById(e);if (e && xNum(e.scrollTop)) offset = e.scrollTop;}return offset;}function xStr(s){for(var i=0; i<arguments.length; ++i){if(typeof(arguments[i])!='string') return false;}return true;}function xTop(e, iY){if(!(e=xGetElementById(e))) return 0;var css=xDef(e.style);if(css && xStr(e.style.top)) {if(xNum(iY)) e.style.top=iY+'px';else {iY=parseInt(e.style.top);if(isNaN(iY)) iY=0;}}else if(css && xDef(e.style.pixelTop)) {if(xNum(iY)) e.style.pixelTop=iY;else iY=e.style.pixelTop;}return iY;}                

function move_aguarde(){
try{
xMoveTo("object1",0,xScrollTop());
xMoveTo("object2",0,xScrollTop());
}catch(e){}
}

window.onscroll=move_aguarde

function FiltroInteiro(event){
    var tecla;
    var key;
    var strValidos = '0123456789';
    if( navigator.appName.indexOf('Netscape')!= -1 )
        tecla= event.which;
    else
        tecla= event.keyCode;
    caractere = String.fromCharCode( tecla);
    if ((strValidos.indexOf(caractere) == -1) && (tecla != 0) && (tecla != 8))
        return false;
    else
        return true;
}

function FiltroRealENegativo(event){
    var tecla;
    var key;
    var strValidos = '-,0123456789';
    if( navigator.appName.indexOf('Netscape')!= -1 )
        tecla= event.which;
    else
        tecla= event.keyCode;
    caractere = String.fromCharCode( tecla);
    if ((strValidos.indexOf(caractere) == -1) && (tecla != 0) && (tecla != 8))
        return false;
    else
        return true;
}

function Trim(str){return str.replace(/^\s+|\s+$/g,"");}

function MascaraFaixaPercentual(event, controle){
    var tecla;
    var strValidos = '-,0123456789';
    if( navigator.appName.indexOf('Netscape')!= -1 )
        tecla= event.which;
    else
        tecla= event.keyCode;
    caractere = String.fromCharCode(tecla);
    if (caractere == '-'){
        if (controle.value.indexOf('-') == -1){
            controle.value = '-' + controle.value;
        }
        return false;
    }
    if (caractere == ','){
        if (controle.value.indexOf(',') != -1){
            return false;
        }
    }
    if ((strValidos.indexOf(caractere) == -1) && (tecla != 0) && (tecla != 8))
        return false;
    else{
        return true;
    }    
}
    
function MascaraFaixaPercentual2(event, controle){
    if (controle.value.length > 0){
        if (controle.value.indexOf(',') != -1){
            var posVirgula = controle.value.indexOf(',');
            var tamTexto = controle.value.length;
            var posCursor = doGetCaretPosition(controle);
            var lRetorno = '';
            var tecla;
            var strValidos = ',0123456789';
            if( navigator.appName.indexOf('Netscape')!= -1 )
                tecla= event.which;
            else
                tecla= event.keyCode;
            caractere = String.fromCharCode(tecla);
    /*        
alert(posVirgula);
alert(tamTexto);
alert(posCursor);
alert(caractere);
*/
//alert(tecla);
            
            if ((posCursor > posVirgula) || (tecla==188)){
                if (((strValidos.indexOf(caractere) != -1) && (tecla != 0) && (tecla != 8)) || (tecla==188)){
                    if (tamTexto - posVirgula > 1) {
                        for (i = 0;  i < tamTexto;  i++){
                            ch = controle.value.charAt(i);
                            if (ch != ',') lRetorno += ch;
                            if (i == (tamTexto - 2)) lRetorno += ',';
                        }
                        controle.value = lRetorno;
                    }
                }
            }
        }
    }
}

function doGetCaretPosition (ctrl) {
    var CaretPos = 0;
    if (document.selection) { //IE
        ctrl.focus ();
        var Sel = document.selection.createRange ();
        Sel.moveStart ('character', -ctrl.value.length);
        CaretPos = Sel.text.length;
    }
    else if (ctrl.selectionStart || ctrl.selectionStart == '0'){ // Firefox
        CaretPos = ctrl.selectionStart;
    }
    return (CaretPos);
}


function MascaraFaixaValor(event, controle){
    var tecla;
    var key;
    var strValidos = '0123456789';
    if( navigator.appName.indexOf('Netscape')!= -1 )
        tecla= event.which;
    else
        tecla= event.keyCode;
    caractere = String.fromCharCode( tecla);
    if (caractere == '-'){
        if (controle.value.indexOf('-') == -1){
            controle.value = '-' + controle.value;
        }
        return false;
    }
    if ((strValidos.indexOf(caractere) == -1) && (tecla != 0) && (tecla != 8))
        return false;
    else
        return true;
}

function MascaraValorPercentual(event, controle){
    var tecla;
    var strValidos = ',0123456789';
    if( navigator.appName.indexOf('Netscape')!= -1 )
        tecla= event.which;
    else
        tecla= event.keyCode;
    caractere = String.fromCharCode(tecla);
    if (caractere == ','){
        if (controle.value.indexOf(',') != -1){
            return false;
        }
    }
    if ((strValidos.indexOf(caractere) == -1) && (tecla != 0) && (tecla != 8))
        return false;
    else{
        return true;
    }    
}
    
function MascaraValorPercentual2(event, controle){
    if (controle.value.length > 0){
        if (controle.value.indexOf(',') != -1){
            var qtdDecimais = 2;
            var posVirgula = controle.value.indexOf(',');
            var tamTexto = controle.value.length;
            var posCursor = doGetCaretPosition(controle);
            var lRetorno = '';
            var tecla;
            var strValidos = ',0123456789';
            if( navigator.appName.indexOf('Netscape')!= -1 )
                tecla= event.which;
            else
                tecla= event.keyCode;
            caractere = String.fromCharCode(tecla);
      
//alert(posVirgula);
//alert(tamTexto);
//alert(posCursor);
/*
alert(caractere);
*/
//alert(tecla);
//alert(tamTexto - posVirgula);
            
            if ((posCursor > posVirgula) || (tecla==188)){
                if (((strValidos.indexOf(caractere) != -1) && (tecla != 0) && (tecla != 8)) || (tecla==188)){
                    if ((tamTexto - posVirgula > (qtdDecimais + 1)) || (tamTexto - posVirgula >= qtdDecimais)) {
                        for (i = 0;  i < tamTexto;  i++){
                            ch = controle.value.charAt(i);
                                if (ch != ',') lRetorno += ch;
                                if (i == (tamTexto - (qtdDecimais + 1))) lRetorno += ',';
                            //alert(ch);
                            //alert(lRetorno);
                        }
                        controle.value = lRetorno;
                    }
                }
            }
        }
    }
}



function MascaraValorPercentualeNegativo(event, controle){
    var tecla;
    var strValidos = ',0123456789';
    if( navigator.appName.indexOf('Netscape')!= -1 )
        tecla= event.which;
    else
        tecla= event.keyCode;
    caractere = String.fromCharCode(tecla);
    if (caractere == '-'){
        if (controle.value.indexOf('-') == -1){
            controle.value = '-' + controle.value;
        }
        return false;
    }
    if (caractere == ','){
        if (controle.value.indexOf(',') != -1){
            return false;
        }
    }
    if ((strValidos.indexOf(caractere) == -1) && (tecla != 0) && (tecla != 8))
        return false;
    else{
        return true;
    }    
}
    
function MascaraValorPercentualeNegativo2(event, controle){
    if (controle.value.length > 0){
        if (controle.value.indexOf(',') != -1){
            var qtdDecimais = 2;
            var posVirgula = controle.value.indexOf(',');
            var tamTexto = controle.value.length;
            var posCursor = doGetCaretPosition(controle);
            var lRetorno = '';
            var tecla;
            var strValidos = '-,0123456789';
            if( navigator.appName.indexOf('Netscape')!= -1 )
                tecla= event.which;
            else
                tecla= event.keyCode;
            caractere = String.fromCharCode(tecla);
      
//alert(posVirgula);
//alert(tamTexto);
//alert(posCursor);
/*
alert(caractere);
*/
//alert(tecla);
//alert(tamTexto - posVirgula);
            
            if ((posCursor > posVirgula) || (tecla==188)){
                if (((strValidos.indexOf(caractere) != -1) && (tecla != 0) && (tecla != 8)) || (tecla==188)){
                    if ((tamTexto - posVirgula > (qtdDecimais + 1)) || (tamTexto - posVirgula >= qtdDecimais)) {
                        for (i = 0;  i < tamTexto;  i++){
                            ch = controle.value.charAt(i);
                                if (ch != ',') lRetorno += ch;
                                if (i == (tamTexto - (qtdDecimais + 1))) lRetorno += ',';
                            //alert(ch);
                            //alert(lRetorno);
                        }
                        controle.value = lRetorno;
                    }
                }
            }
        }
    }
}

// Verifica se data2 é maior que data1
function ComparaDataMaior(data1, data2){
    if ( parseInt( data2.split( "/" )[2].toString() + data2.split( "/" )[1].toString() + data2.split( "/" )[0].toString() ) > parseInt( data1.split( "/" )[2].toString() + data1.split( "/" )[1].toString() + data1.split( "/" )[0].toString() ) )
    {  
        return false;
    }else
    {  
        return true;
    }
}

function MarcarTodos(crtl){
	for (i=0; i < document.forms[0].elements.length; i++){
		if (document.forms[0].elements[i].type == 'checkbox'){
			document.forms[0].elements[i].checked = crtl.checked;
		}
	}
}