using GetDogeOpReturn.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using GetDogeOpReturn.Entities.Web;

namespace GetDogeOpReturn.Utils
{
    public class SendWebRequest
    {
        public static async Task<WebResult> SendGetRequest(string url)
        {
            WebResult retval = new WebResult();
            try
            {
                WebRequest request = WebRequest.Create(url);

                WebResponse response = request.GetResponse(); 
                
                Stream dataStream = response.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);


                retval.Text = await reader.ReadToEndAsync();
            }
            catch (Exception e)
            {
                retval.Error = e.Message;
            }

            return retval;
        }
    }
}
