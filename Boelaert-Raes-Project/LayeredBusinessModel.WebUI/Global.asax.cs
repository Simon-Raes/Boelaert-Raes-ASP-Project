using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.SessionState;
using CustomException;

namespace LayeredBusinessModel.WebUI
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition
            {
                Path = "~/scripts/jquery-1.7.1.min.js", // wanneer je online bent moet deze map niet bestaan
                DebugPath = "~/scripts/jquery-1.7.1.js",
                CdnPath = "http://ajax.microsoft.com/ajax/jQuery/jquery-1.4.1.min.js",
                CdnDebugPath = "http://ajax.microsoft.com/ajax/jQuery/jquery-1.4.1.js"
            });
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Exception ex = Server.GetLastError();
            //if (ex.InnerException as NoRecordException != null) //.innerException moet weg wanner overal de exceptions correct worden opgegooid
            //{
            //    //only NoRecordExceptions will be handled here
            //    //Response.Redirect(string.Format("Error.aspx?aspxerrorpath={0}&nored=true", Request.Url.PathAndQuery));
            //}
            //else
            //{
            //    //all other exceptions will be handled here
            //    Response.Redirect(string.Format("Error.aspx?aspxerrorpath={0}", Request.Url.PathAndQuery));
            //}
            //Server.ClearError();
            
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

    }
}