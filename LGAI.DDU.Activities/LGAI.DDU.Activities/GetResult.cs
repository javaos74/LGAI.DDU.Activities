using LGAI.DDU.Activities.Helpers;
using LGAI.DDU.Activities.Model;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LGAI.DDU.Activities
{
    public class GetResult : CodeActivity
    {
        public InArgument<string> Endpoint { get; set; }
        public InArgument<string> ApiKey { get; set; }
        public InArgument<string> RequestId { get; set; }
        public OutArgument<string> ErrorMessage { get; set; }
        public OutArgument<DDUResult> Result { get; set; }

        private UiPathHttpClient _httpClient;
        private DDUResult _dduresult;
        private string _errorMessage;
        public GetResult()
        {
            // this is the default endpoint
            _httpClient = new UiPathHttpClient("https://gw.lgair.net");
        }

        private DDUResult ExecuteInternal(string endpoint, string apikey, string requestid)
        {
            _httpClient.setEndpoint(endpoint);
            _httpClient.setApiKey(apikey);
            _errorMessage = string.Empty;

            try
            {
                var resp1 = _httpClient.GetResult(requestid).Result;
                if (resp1.status == System.Net.HttpStatusCode.OK)
                {
                    var result = JsonConvert.DeserializeObject<DDUResult>(resp1.body);
                    return result;
                }
                else
                {
                    _errorMessage = resp1.body;
                    return null;
                }
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                return null;
            }
        }
        protected override void Execute(CodeActivityContext context)
        {
            var endpoint = Endpoint.Get(context);
            var apikey = ApiKey.Get(context);
            var requestid = RequestId.Get(context);

            _dduresult = ExecuteInternal(endpoint, apikey, requestid);
            if (_dduresult != null)
            {
                ErrorMessage.Set(context, string.Empty);
                Result.Set(context, _dduresult);
            }
            else
            {
                ErrorMessage.Set(context, _errorMessage);
            }
        }
    }
}
