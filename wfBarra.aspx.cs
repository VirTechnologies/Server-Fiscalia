using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class wfBarra : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios obj;
        DataSet dsModulos = new DataSet();
        DataSet dsSubModulos = new DataSet();
        DataSet dsSubModulos2 =new DataSet();
        String msg, script;
        int i, j, k, y, Menu1Va, Menu2Va, Menu3Va;
        int Submenus1, Submenus2;
        bool nuevo = true;
        //return;

        if (nuevo)
        {
            CrearMenuLateral(dsModulos, Session["IDUSUARIO"].ToString());
        }
        else
        {
            obj = new clsblUsuarios();
            msg = obj.RetornarModulosUsuario(ref dsModulos, Session["IDUSUARIO"].ToString());
            if (msg == "")
            {
                /*
                MenuX=new Array(
                "ItemText",
                "Link",
                "background image",
                number of sub elements,
                height,
                width,
                "bgcolor",
                "bghighcolor",
                "fontcolor",
                "fonthighcolor",
                "bordercolor",
                "fontfamily",
                fontsize,
                fontbold,
                fontitalic,
                "textalign",
                "statustext");
                 Color and font variables defined in the menu tree take precedence over the global variables
                 Fontsize, fontbold and fontitalic are ignored when set to -1.
                 For rollover images ItemText format is:  "rollover?"+BaseHref+"Image1.jpg?"+BaseHref+"Image2.jpg" 
                */

                script = "<script language=" + (char)34 + "javascript" + (char)34 + ">" + Environment.NewLine;
                Menu1Va = 1;
                for (i = 0; i < dsModulos.Tables[0].Rows.Count; i++)
                {
                    msg = obj.RetornarSubModulosUsuario(ref dsSubModulos, Session["IDUSUARIO"].ToString(), dsModulos.Tables[0].Rows[i]["id_modulo"].ToString());
                    Submenus1 = 0;
                    if (dsSubModulos.Tables.Count > 0)
                        Submenus1 = dsSubModulos.Tables[0].Rows.Count;
                    script += " Menu" + Menu1Va.ToString() + "=new Array('" + dsModulos.Tables[0].Rows[i]["modulo"].ToString() + "    ','" + dsModulos.Tables[0].Rows[i]["pagina"].ToString() + "',''," + Submenus1.ToString() + ",40,184,'','#0072BB','','','','',-1,-1,-1,'','InteliTurnos Admin'); " + Environment.NewLine;
                    if (dsSubModulos.Tables.Count > 0)
                    {
                        Menu2Va = 1;
                        for (j = 0; j < dsSubModulos.Tables[0].Rows.Count; j++)
                        {
                            msg = obj.RetornarSubModulosUsuario(ref dsSubModulos2, Session["IDUSUARIO"].ToString(), dsSubModulos.Tables[0].Rows[j]["id_modulo"].ToString());
                            Submenus2 = 0;
                            if (dsSubModulos2.Tables.Count > 0)
                                Submenus2 = dsSubModulos2.Tables[0].Rows.Count;
                            script += " Menu" + Menu1Va.ToString() + "_" + Menu2Va.ToString() + "=new Array('" + dsSubModulos.Tables[0].Rows[j]["modulo"].ToString() + "    ','" + dsSubModulos.Tables[0].Rows[j]["pagina"].ToString() + "',''," + Submenus2.ToString() + ",40,184,'','#0072BB','','','','',-1,-1,-1,'',''); " + Environment.NewLine;
                            Menu3Va = 1;
                            for (k = 0; k < dsSubModulos2.Tables[0].Rows.Count; k++)
                            {
                                script += " Menu" + Menu1Va.ToString() + "_" + Menu2Va.ToString() + "_" + Menu3Va.ToString() + "=new Array('" + dsSubModulos2.Tables[0].Rows[k]["modulo"].ToString() + "    ','" + dsSubModulos2.Tables[0].Rows[k]["pagina"].ToString() + "','','',40,184,'','#0072BB','','','','',-1,-1,-1,'',''); " + Environment.NewLine;
                                Menu3Va = Menu3Va + 1;
                            }
                            Menu2Va = Menu2Va + 1;
                        }
                    }
                    Menu1Va = Menu1Va + 1;
                }
                script += " Menu" + Menu1Va.ToString() + "=new Array('Cerrar Sesión','CerrarSesion.aspx','',0,40,184,'','#0072BB','','','','',-1,-1,'','right','InteliTurnos Admin'); " + Environment.NewLine;
                script += "var NoOffFirstLineMenus=" + (Menu1Va).ToString() + ";			// Number of main menu  items" + Environment.NewLine;
                script = script + "</script> " + Environment.NewLine;
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Botones", script);
                y = 0;
                //for (i=1;i<=Menu1Va;i++)
                //{
                //    Response.Write("<span style='position:absolute; left:2px; top:" + y.ToString() + "px;'><img src='images/boton.jpg'></span>");
                //    y = y + 25;
                //}
            }
        }
    }

    protected void CrearMenuLateral(DataSet modulos, string usuario)
    {
        clsblUsuarios obj;
        DataSet dsModulos = new DataSet();
        DataSet dsSubModulos = new DataSet();
        DataSet dsSubModulos2 = new DataSet();
        String msg, htmlmenu;
        int i, j, k;

        obj = new clsblUsuarios();
        msg = obj.RetornarModulosUsuario(ref dsModulos, Session["IDUSUARIO"].ToString());
        if (msg == "")
        {
            htmlmenu = "";
            for (i = 0; i < dsModulos.Tables[0].Rows.Count; i++)
            {
                htmlmenu += "<li class='nav-item dropdown'>" + Environment.NewLine;
                if (dsModulos.Tables[0].Rows[i]["pagina"].ToString() != "")
                {
                    htmlmenu += "<a class='nav-link' target='contenido' href='" + dsModulos.Tables[0].Rows[i]["pagina"].ToString() + "' id='navbarLink" + dsModulos.Tables[0].Rows[i]["id_modulo"].ToString() + "'>";
                    htmlmenu += dsModulos.Tables[0].Rows[i]["modulo"].ToString() + "</a>" + Environment.NewLine;
                }
                else
                {
                    string idnavbar = "navbarDropdownId" + dsModulos.Tables[0].Rows[i]["id_modulo"].ToString();
                    htmlmenu += "<a class='nav-link dropdown-toggle' href='#' id='" + idnavbar + "' data-toggle='dropdown' aria-haspopup='true' aria-expanded='true'>";
                    htmlmenu += dsModulos.Tables[0].Rows[i]["modulo"].ToString() + "</a>" + Environment.NewLine;
                    msg = obj.RetornarSubModulosUsuario(ref dsSubModulos, Session["IDUSUARIO"].ToString(), dsModulos.Tables[0].Rows[i]["id_modulo"].ToString());
                    if (dsSubModulos.Tables.Count > 0)
                    {
                        htmlmenu += "<ul class='dropdown-menu dropdown-menu-right' aria-labelledby='" + idnavbar + "'>" + Environment.NewLine;

                        for (j = 0; j < dsSubModulos.Tables[0].Rows.Count; j++)
                        {
                            if (dsSubModulos.Tables[0].Rows[j]["pagina"].ToString() != "")
                            {
                                htmlmenu += "<li><a class='dropdown-item' target='contenido' href='" + dsSubModulos.Tables[0].Rows[j]["pagina"].ToString() + "'>" + dsSubModulos.Tables[0].Rows[j]["modulo"].ToString() + "</a></li>" + Environment.NewLine;
                            }
                            else
                            {
                                htmlmenu += "<li class='dropdown-submenu'><a class='dropdown-item dropdown-toggle' href='#'>" + dsSubModulos.Tables[0].Rows[j]["modulo"].ToString() + "</a>" + Environment.NewLine;
                                msg = obj.RetornarSubModulosUsuario(ref dsSubModulos2, Session["IDUSUARIO"].ToString(), dsSubModulos.Tables[0].Rows[j]["id_modulo"].ToString());
                                if (dsSubModulos2.Tables.Count > 0)
                                {
                                    htmlmenu += "<ul class='dropdown-menu'>";
                                }
                                //htmlmenu += " Menu" + Menu1Va.ToString() + "_" + Menu2Va.ToString() + "=new Array('" + dsSubModulos.Tables[0].Rows[j]["modulo"].ToString() + "    ','" + dsSubModulos.Tables[0].Rows[j]["pagina"].ToString() + "',''," + Submenus2.ToString() + ",40,184,'','#0072BB','','','','',-1,-1,-1,'',''); " + Environment.NewLine;
                                for (k = 0; k < dsSubModulos2.Tables[0].Rows.Count; k++)
                                {
                                    htmlmenu += "<li><a class='dropdown-item' target='contenido' href='" + dsSubModulos2.Tables[0].Rows[k]["pagina"].ToString() + "'>" + dsSubModulos2.Tables[0].Rows[k]["modulo"].ToString() + "</a></li>" + Environment.NewLine;
                                    //htmlmenu += " Menu" + Menu1Va.ToString() + "_" + Menu2Va.ToString() + "_" + Menu3Va.ToString() + "=new Array('" + dsSubModulos2.Tables[0].Rows[k]["modulo"].ToString() + "    ','" + dsSubModulos2.Tables[0].Rows[k]["pagina"].ToString() + "','','',40,184,'','#0072BB','','','','',-1,-1,-1,'',''); " + Environment.NewLine;
                                }
                                htmlmenu += "</ul>" + Environment.NewLine;
                            }
                        }
                        htmlmenu += "</ul>" + Environment.NewLine;
                    }
                }
            }
            htmlmenu += "<li class='nav-item'>" + Environment.NewLine;
            htmlmenu += "<a class='nav-link' target='contenido' href='CerrarSesion.aspx'>CERRAR SESIÓN</a>" + Environment.NewLine;
            htmlmenu += "</li>" + Environment.NewLine; // CIERRA BOTÓN CERRAR SESIÓN
            htmlmenu += "</li>" + Environment.NewLine; // CIERRA TODO EL MENÚ LATERAL
            botonesLaterales.InnerHtml = htmlmenu;
        }
    }
}
