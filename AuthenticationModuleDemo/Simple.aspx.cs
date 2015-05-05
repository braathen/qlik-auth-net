using System;
using QlikAuthNet;

namespace AuthenticationModuleDemo
{
    public partial class Simple : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["proxyRestUri"]))
            {
                //Create new instance, supply UserDirectory and UserId
                var req = new Ticket
                {
                    UserDirectory = "QLIK",
                    UserId = "rikard",
                };

                //Add a list of groups (delimiteer separated string or List<string>)
                req.AddGroups("Group1;Group2;Group3");

                //Add some custom attributes (delimiteer separated string or List<string>)
                req.AddAttributes("Email", "rfn@qlikdude.com");

                //Perform ticket request
                req.TicketRequest();
            }
            else
            {
                Response.Write("Please don't access this Authentication Module directly. Use a virtual proxy instead!");
            }
        }
    }
}