using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Script.Services;
using GPO.DsAdmin;

/// <summary>
/// Summary description for WSAjax
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService()]
public class WSAjax : System.Web.Services.WebService
{

    public WSAjax()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    //[WebMethod]
    //public bool AlterarINFORMATIVOSStatus(Int32 codigo)
    //{
    //    using (INFORMATIVOS objINFORMATIVOS = new INFORMATIVOS())
    //    {
    //        objINFORMATIVOS.AlterarINFORMATIVOSRotaciona(codigo);
    //    }
    //    return true;
    //}

    //[WebMethod]
    //public bool AlterarUSUARIOStatus(Int32 codigo)
    //{
    //    using (USUARIO objUSUARIO = new USUARIO())
    //    {
    //        objUSUARIO.AlterarUSUARIOStatus(codigo);
    //    }
    //    return true;
    //}
    
}

