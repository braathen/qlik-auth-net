using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace QlikAuthNet
{
    public class Ticket
    {
        private X509Certificate2 certificate_ { get; set; }

        public string UserDirectory { get; set; }
        public string UserId { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ProxyRestUri { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string TargetId { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<Dictionary<string, string>> Attributes { get; set; }

        public class RequestJson
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public String UserDirectory { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public String UserId { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public List<KeyValuePair<string, string>> Attributes { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public String TargetId { get; set; }
        }

        public class ResponseData
        {
            public String UserDirectory;
            public String UserId;
            public List<Dictionary<string, string>> Attributes;
            public String Ticket;
            public String TargetUri;
        }


        public Ticket()
        {
            // First locate the Qlik Sense certificate
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            certificate_ = store.Certificates.Cast<X509Certificate2>().FirstOrDefault(c => c.FriendlyName == "QlikClient");
            store.Close();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }
        /// <summary>
        /// Add a delimited separated string of groups
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="delimiters"></param>
        public void AddGroups(string groups, string delimiters = ";")
        {
            AddAttributes("Group", groups, delimiters);
        }

        /// <summary>
        /// Add a List<string> with groups
        /// </summary>
        /// <param name="groups"></param>
        public void AddGroups(List<string> groups)
        {
            AddAttributes("Group", groups);
        }

        /// <summary>
        /// Add custom attributes, delimited string
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <param name="delimiters"></param>
        public void AddAttributes(string key, string values, string delimiters = ";")
        {
            AddAttributes(key, values.Split(delimiters.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList());
        }

        /// <summary>
        /// Add custom attributes as List<string>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        public void AddAttributes(string key, List<string> values)
        {
            if (Attributes == null)
                Attributes = new List<Dictionary<string, string>>();

            Attributes.AddRange(values.Select(v => new Dictionary<string, string> { { key, v.Trim() } }).ToList());
        }

        /// <summary>
        /// Generates a randomized string to be used as XrfKey 
        /// </summary>
        /// <returns>16 character randomized string</returns>
        public string GenerateXrfKey()
        {
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            var chars = new char[16];
            var rd = new Random();

            for (int i = 0; i < 16; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        /// <summary>
        /// Requests a ticket and redirects back to where the user came from using TargetUri from the ticket response.
        /// </summary>
        /// <returns>If targetId is not provided in the request a ticket will be returned for manual processing</returns>
        public string TicketRequest()
        {
            var context = HttpContext.Current;

            try
            {
                // Get data as json
                var json = ParseRequestData();

                //Create the HTTP Request and add required headers and content in Xrfkey
                string Xrfkey = GenerateXrfKey();

                //Create URL to REST endpoint for tickets
                Uri url = CombineUri(ProxyRestUri, "ticket?Xrfkey=" + Xrfkey);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                // Add the method to authentication the user
                request.Method = "POST";
                request.Accept = "application/json";
                request.Headers.Add("X-Qlik-Xrfkey", Xrfkey);

                if (certificate_ == null)
                    throw new Exception("Certificate not found! Verify AppPool credentials.");

                request.ClientCertificates.Add(certificate_);
                byte[] bodyBytes = Encoding.UTF8.GetBytes(json);

                if (!string.IsNullOrEmpty(json))
                {
                    request.ContentType = "application/json";
                    request.ContentLength = bodyBytes.Length;
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(bodyBytes, 0, bodyBytes.Length);
                    requestStream.Close();
                }

                // make the web request and redirect user
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();

                if (stream != null)
                {
                    var res = JsonConvert.DeserializeObject<ResponseData>(new StreamReader(stream).ReadToEnd());

                    if (String.IsNullOrEmpty(res.TargetUri))
                        return res.Ticket;
                    
                    string redirectUrl;

                    if (res.TargetUri.Contains("?"))
                        redirectUrl = res.TargetUri + "&QlikTicket=" + res.Ticket;
                    else
                        redirectUrl = res.TargetUri + "?QlikTicket=" + res.Ticket;

                    context.Response.Redirect(redirectUrl);
                }
                else
                {
                    throw new Exception("Unknown error");
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return null;
        }

        private string ParseRequestData()
        {
            var context = HttpContext.Current;

            //Verify ProxyRestUri path
            ProxyRestUri = !String.IsNullOrEmpty(ProxyRestUri)
                ? ProxyRestUri
                : context.Request.QueryString["proxyRestUri"];

            if (String.IsNullOrEmpty(ProxyRestUri))
                throw new Exception("ProxyRestUri not defined!");

            //Verify that TargetId is available
            TargetId = !String.IsNullOrEmpty(TargetId)
                ? TargetId
                : context.Request.QueryString["targetId"];

            return JsonConvert.SerializeObject(this);
        }

        private static Uri CombineUri(string baseUri, string relativeOrAbsoluteUri)
        {
            if (!baseUri.EndsWith("/"))
                baseUri += "/";

            return new Uri(new Uri(baseUri), relativeOrAbsoluteUri);
        }
    }
}
