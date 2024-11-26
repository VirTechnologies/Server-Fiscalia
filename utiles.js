function open_o(site,w,h){
  window.open(site,"_blank","width="+w+",height="+h+",resizable=yes,status=yes,scrollbars=yes,menubar=no,toolbar=no,location=no");
}

/*
function openModalWindow(page, w, h)
{
  window.open(page,"_blank","width="+w+",height="+h+",resizable=yes,status=yes,scrollbars=yes,menubar=no,toolbar=no,location=no");
}
*/

function openModalWindow(page, w, h)
{
   var pagina=document.location.toString();
   var s = window.showModalDialog(page, '', 'dialogHeight:'+h+'px;dialogWidth:'+w+'px;status:no;help:no;maximize=yes;');
   if (s)
   {
     if (s!="")
     {
     	 if (s!="login")
     	 {
		     if (testIsValidObject(document.form1.Id_Retornado))
		     {
	         document.getElementById("Id_Retornado").value=s;
	       }  
	     }else
	     {
         window.parent.location="wfLogin.aspx";
         return;
	     }
     }  
   }  
   window.location=pagina;
}


function Oculta(td, visible)
{
  if (testIsValidObject(document.getElementById(td)))
  {
      var show=(document.all)? "block" : "table-cell";
      if (visible==true) 
      {
        document.getElementById(td).style.display=show;
        document.getElementById("TabVisible").value=td;
      }  
      else
      {
        document.getElementById(td).style.display="none";
      };	
  };
}

function TabSeleccionado(tab)
{
	if (testIsValidObject(document.getElementById(tab)))
	{
		for (var i = 0 ; i < document.anchors.length ; i++)
		{
	       var str=document.anchors[i].id;
	       if (str!="")
	       {
	           if (str.indexOf("hl")!=-1)
		       {
		           if (document.anchors[i].id == tab)
		           {
                     document.anchors[i].className="btab1";
		           }else
		           {
                     document.anchors[i].className="btab2";
		           }
		       }
	       }
	    }	   
    }  
}

function DeseaCerrar()
{
/*
   if (event.clientY < 0) 
   {
	   	 event.returnValue = false;  
	 };    
*/	 
}

function ClickCerrar()
{
//	var agree=confirm("No olvide grabar los datos! Desea salir?");
//  if (agree)
    window.close();
}

function viewSource()
{
   d=window.open();
   d.document.open('text/plain').write(document.documentElement.outerHTML);
}

function DigitoVerificacion(nit) { 
	if (testIsValidObject(document.form1.digito_verificacion))
	{
		nume = parseInt(nit); 
		ceros = ""; 
		if(nume >= 1) 
		ceros = "00000000000000"; 
		if(nume >= 10) 
		ceros = "0000000000000"; 
		if(nume >= 100) 
		ceros = "000000000000"; 
		if(nume >= 1000) 
		ceros = "00000000000"; 
		if(nume >= 10000) 
		ceros = "0000000000"; 
		if(nume >= 100000) 
		ceros = "000000000"; 
		if(nume >= 1000000) 
		ceros = "00000000"; 
		if(nume >= 10000000) 
		ceros = "0000000"; 
		if(nume >= 100000000) 
		ceros = "000000"; 
		if(nume >= 1000000000) 
		ceros = "00000"; 
		if(nume >= 10000000000) 
		ceros = "0000"; 
		if(nume >= 100000000000) 
		ceros = "000"; 
		if(nume >= 1000000000000) 
		ceros = "00"; 
		if(nume >= 10000000000000) 
		ceros = "0"; 
		if(nume >= 100000000000000) 
		ceros = ""; 
		
		
		li_peso= new Array(); 
		li_peso[0] = 71; 
		li_peso[1] = 67; 
		li_peso[2] = 59; 
		li_peso[3] = 53; 
		li_peso[4] = 47; 
		li_peso[5] = 43; 
		li_peso[6] = 41; 
		li_peso[7] = 37; //8 
		li_peso[8] = 29; //3 
		li_peso[9] = 23; //0 
		li_peso[10] = 19; //1 
		li_peso[11] = 17; //2 
		li_peso[12] = 13; //0 
		li_peso[13] = 7; //9 
		li_peso[14] = 3; //9 
		
		ls_str_nit = ceros + nit; 
		li_suma = 0; 
		for(i = 0; i < 15; i++){ 
		li_suma += ls_str_nit.substring(i,i+1) * li_peso[i]; 
		} 
		digito_chequeo = li_suma%11; 
		if (digito_chequeo >= 2) 
		digito_chequeo = 11 - digito_chequeo; 
		if (ceros!="")
		  document.form1.digito_verificacion.value = digito_chequeo; 
		else  
		  document.form1.digito_verificacion.value = ""; 
  }		  
} 

function testIsValidObject(objToTest) { //JavaScript function to check a form element exists
if (objToTest == null || objToTest == undefined) {
return false;
}
return true;
}

function VerificaDigitoCarga()
{
	if (testIsValidObject(document.form1.digito_verificacion))
    DigitoVerificacion(document.form1.numero_identificacion.value)	
}


// Title: Tigra Calendar
// URL: http://www.softcomplex.com/products/tigra_calendar/
// Version: 3.2 (American date format)
// Date: 10/14/2002 (mm/dd/yyyy)
// Feedback: feedback@softcomplex.com (specify product title in the subject)
// Note: Permission given to use this script in ANY kind of applications if
//    header lines are left unchanged.
// Note: Script consists of two files: calendar?.js and calendar.html
// About us: Our company provides offshore IT consulting services.
//    Contact us at sales@softcomplex.com if you have any programming task you
//    want to be handled by professionals. Our typical hourly rate is $20.

// if two digit year input dates after this year considered 20 century.
var NUM_CENTYEAR = 30;
// is time input control required by default
var BUL_TIMECOMPONENT = false;
// are year scrolling buttons required by default
var BUL_YEARSCROLL = true;

var calendars = [];
var RE_NUM = /^\-?\d+$/;

function calendar2(obj_target) {

	// assing methods
	this.gen_date = cal_gen_date2;
	this.gen_time = cal_gen_time2;
	this.gen_tsmp = cal_gen_tsmp2;
	this.prs_date = cal_prs_date2;
	this.prs_time = cal_prs_time2;
	this.prs_tsmp = cal_prs_tsmp2;
	this.popup    = cal_popup2;

	// validate input parameters
    if (obj_target)
    {
	    if (!obj_target)
		    return cal_error("Error al crear calendario: control no especificado");
	    if (obj_target.value == null)
		    return cal_error("Error al crear calendario: el parámetro especificado no es válido para el caledario");
	    this.target = obj_target;
	    this.time_comp = BUL_TIMECOMPONENT;
	    this.year_scroll = BUL_YEARSCROLL;
    	
	    // register in global collections
	    this.id = calendars.length;
	    calendars[this.id] = this;
    };
}

function cal_popup2 (str_datetime) {
	this.dt_current = this.prs_tsmp(str_datetime ? str_datetime : this.target.value);
	if (!this.dt_current) return;

	var obj_calwindow = window.open(
		'calendar.html?datetime=' + this.dt_current.valueOf()+ '&id=' + this.id,
		'Calendar', 'width=200,height='+(this.time_comp ? 215 : 205)+
		',status=yes,resizable=yes,top=200,left=200,dependent=no,alwaysRaised=yes'
	);
	obj_calwindow.opener = window;
	obj_calwindow.focus();
}

// timestamp generating function
function cal_gen_tsmp2 (dt_datetime) {
	return(this.gen_date(dt_datetime) + ' ' + this.gen_time(dt_datetime));
}

// date generating function
function cal_gen_date2 (dt_datetime) {
	return (
		 (dt_datetime.getDate() < 10 ? '0' : '') + dt_datetime.getDate() + "/"
		+(dt_datetime.getMonth() < 9 ? '0' : '') + (dt_datetime.getMonth() + 1) + "/"
		+ dt_datetime.getFullYear()
	);
}
// time generating function
function cal_gen_time2 (dt_datetime) {
	return (
		(dt_datetime.getHours() < 10 ? '0' : '') + dt_datetime.getHours() + ":"
		+ (dt_datetime.getMinutes() < 10 ? '0' : '') + (dt_datetime.getMinutes()) + ":"
		+ (dt_datetime.getSeconds() < 10 ? '0' : '') + (dt_datetime.getSeconds())
	);
}

// timestamp parsing function
function cal_prs_tsmp2 (str_datetime) {
	// if no parameter specified return current timestamp
	if (!str_datetime)
		return (new Date());

	// if positive integer treat as milliseconds from epoch
	if (RE_NUM.exec(str_datetime))
		return new Date(str_datetime);

	// else treat as date in string format
	var arr_datetime = str_datetime.split(' ');
	return this.prs_time(arr_datetime[1], this.prs_date(arr_datetime[0]));
}

// date parsing function
function cal_prs_date2 (str_date) {

	var arr_date = str_date.split('/');

	if (arr_date.length != 3) return alert ("Formato de fecha inválido: '" + str_date + "'.\nFormato acceptado: dd-mm-yyyy.");
	if (!arr_date[0]) return alert ("Formato de fecha inválido: '" + str_date + "'.\nEl valor para el día del mes no es aceptable.");
	if (!RE_NUM.exec(arr_date[0])) return alert ("Día de mes inválido: '" + arr_date[0] + "'.\nLos valores permitidos son enteros sin signo.");
	if (!arr_date[1]) return alert ("Formato de fecha inválido: '" + str_date + "'.\nNo aparece el mes.");
	if (!RE_NUM.exec(arr_date[1])) return alert ("Valor de mes inválido: '" + arr_date[1] + "'.\nLos valores permitidos son enteros sin signo.");
	if (!arr_date[2]) return alert ("Formato de fecha inválido: '" + str_date + "'.\nAño no suministrado.");
	if (!RE_NUM.exec(arr_date[2])) return alert ("IValor de año inválido: '" + arr_date[2] + "'.\nLos valores permitidos son enteros sin signo.");

	var dt_date = new Date();
	dt_date.setDate(1);

	if (arr_date[1] < 1 || arr_date[1] > 12) return alert ("Invalid month value: '" + arr_date[1] + "'.\nAllowed range is 01-12.");
	dt_date.setMonth(arr_date[1]-1);
	 
	if (arr_date[2] < 100) arr_date[2] = Number(arr_date[2]) + (arr_date[2] < NUM_CENTYEAR ? 2000 : 1900);
	dt_date.setFullYear(arr_date[2]);

	var dt_numdays = new Date(arr_date[2], arr_date[1], 0);
	dt_date.setDate(arr_date[0]);
	if (dt_date.getMonth() != (arr_date[1]-1)) return alert ("Invalid day of month value: '" + arr_date[0] + "'.\nAllowed range is 01-"+dt_numdays.getDate()+".");

	return (dt_date)
}

// time parsing function
function cal_prs_time2 (str_time, dt_date) {
	if (!dt_date) return null;
	var arr_time = String(str_time ? str_time : '').split(':');

	if (!arr_time[0]) dt_date.setHours(0);
	else if (RE_NUM.exec(arr_time[0])) 
		if (arr_time[0] < 24) dt_date.setHours(arr_time[0]);
		else return cal_error ("Valor de hora inválido: '" + arr_time[0] + "'.\nEl rango permitido es 00-23.");
	else return cal_error ("Valor de hora inválido: '" + arr_time[0] + "'.\nLos valores permitidos son enteros sin signo.");
	
	if (!arr_time[1]) dt_date.setMinutes(0);
	else if (RE_NUM.exec(arr_time[1]))
		if (arr_time[1] < 60) dt_date.setMinutes(arr_time[1]);
		else return cal_error ("Valor de minutos inválido: '" + arr_time[1] + "'.\El rango permitido es 00-59.");
	else return cal_error ("Valor de minutos inválido: '" + arr_time[1] + "'.\nLos valores permitidos son enteros sin signo.");

	if (!arr_time[2]) dt_date.setSeconds(0);
	else if (RE_NUM.exec(arr_time[2]))
		if (arr_time[2] < 60) dt_date.setSeconds(arr_time[2]);
		else return cal_error ("Valor de segundos inválido: '" + arr_time[2] + "'.\nEl rango permitido es 00-59.");
	else return cal_error ("Valor de segundos inválido: '" + arr_time[2] + "'.\nLos valores permitidos son enteros sin signo.");

	dt_date.setMilliseconds(0);
	return dt_date;
}

function cal_error (str_message) {
	alert (str_message);
	return null;
}

 function intOnly(val) {
	if ((event.keyCode == 8) || (event.keyCode == 46) || (event.keyCode == 37) || (event.keyCode == 39) || (event.keyCode == 45))
    event.returnValue = true;
  if ( ((event.keyCode < 48) || (event.keyCode > 57)) && (event.keyCode != 45))
    event.returnValue = false;
  else  
    event.returnValue = true;
 return true;   
}

function doubleOnly(val) { //number is Keycode 48 - 57
 	if ((event.keyCode == 8) || (event.keyCode == 37) || (event.keyCode == 39))
    event.returnValue = true;
  else
	{
	 	if (event.keyCode == 44)
	    event.returnValue = true;
	  else
  	{
		  if ( (event.keyCode < 48 || event.keyCode > 57)) 
		    event.returnValue = false;
		  else  
		    event.returnValue = true;
  	}  
	}  
}


