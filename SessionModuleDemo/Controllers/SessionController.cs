using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace SessionModuleDemo.Controllers
{
    public class SessionController : ApiController
    {
        public JObject GetAddSession(string id)
        {
            //Create new instance, make sure to update ProxyRestUri
            var req = new QlikAuthNet.Session()
            {
                UserDirectory = "QLIK",
                UserId = "rikard",
                ProxyRestUri = "https://localhost:4243/qps",
                SessionId = id
            };

            //Add a list of groups (delimiter separated string or List<string>)
            req.AddGroups("Group1;Group2;Group3");

            //Add some custom attributes (delimiter separated string or List<string>)
            req.AddAttributes("Email", "rfn@qlikdude.com");
            req.AddAttributes("Country", "Sweden");
            req.AddAttributes("Phone", "+46-012-345678");

            //Send request
            var res = req.SessionRequest();

            //Parse and return the result
            return res != null ? JObject.Parse(res) : null;
        }
    }
}
