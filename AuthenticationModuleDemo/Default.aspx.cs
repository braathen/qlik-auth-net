using System;
using System.Text;
using Newtonsoft.Json;
using QlikAuthNet;

namespace AuthenticationModuleDemo
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Initialization stuff for this demo
            Page.MaintainScrollPositionOnPostBack = true;

            //Display url parameters from proxy
            frmProxyRestUri.InnerText = Request.QueryString["proxyRestUri"];
            frmTargetId.InnerText = Request.QueryString["targetId"];

            if (String.IsNullOrEmpty(Request.QueryString["proxyRestUri"]))
            {
                Response.Write("Please don't access this Authentication Module directly. Use a virtual proxy instead!");
                Response.End();
            }
            #endregion

            if (Page.IsPostBack)
            {
                var button = Request.Form["submit"];

                //Create a new ticket instance, add user directory and userid
                var req = new Ticket
                {
                    UserDirectory = frmUserDirectory.Value,
                    UserId = frmUserId.Value,
                };

                //Add some custom attributes
                req.AddAttributes(Request["frmAttrib1"], Request["frmList1"]);
                req.AddAttributes(Request["frmAttrib2"], Request["frmList2"]);
                req.AddAttributes(Request["frmAttrib3"], Request["frmList3"]);

                //Add the targetId (which normally is processed automatically)
                req.TargetId = Request.QueryString["targetId"];

                #region Display request information for this demo
                //This step is only to show what is happening in this demo...
                if (button == "buildrequest" && frmUserId.Value != "")
                {
                    var jsonPretty = JsonConvert.SerializeObject(req, Formatting.Indented);

                    var sb = new StringBuilder();
                    sb.Append("POST " + Request.QueryString["proxyRestUri"] + "ticket?Xrfkey=0123456789abcdef" + Environment.NewLine + Environment.NewLine);
                    sb.Append("HEADERS:" + Environment.NewLine);
                    sb.Append("X-Qlik-Xrfkey: 0123456789abcdef" + Environment.NewLine);
                    sb.Append("Content-Type: application/json" + Environment.NewLine + Environment.NewLine);
                    sb.Append("BODY:" + Environment.NewLine);
                    sb.Append(jsonPretty);

                    frmRequestString.InnerText = sb.ToString();

                    panel2.Visible = true;
                }
                #endregion

                if (button == "sendrequest")
                {
                    //Send ticket request
                    var res = req.TicketRequest();

                    //If something went wroong, display the returned message
                    if (!String.IsNullOrEmpty(res))
                    {
                        frmError.InnerText = res;
                        errorpanel.Visible = true;
                    }
                }
            }
        }
    }
}