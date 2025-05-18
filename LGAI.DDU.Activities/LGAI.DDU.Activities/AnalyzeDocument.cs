
using System.Diagnostics;
using LGAI.DDU.Activities.Helpers;
using LGAI.DDU.Activities.Model;
using System.Activities;
using System.Net;
using UiPath.Platform.ResourceHandling;
using Newtonsoft.Json;
using System.Activities.DesignViewModels;


namespace LGAI.DDU.Activities
{
    public class AnalyzeDocument : CodeActivity // This base class exposes an OutArgument named Result
    {

        public InArgument<string> Endpoint { get; set; }
        public InArgument<string> ApiKey { get; set; }
        public InArgument<IResource> InputFile { get; set; }
        public InArgument<OnOff> WaitResult { get; set; } = OnOff.On;

        public InArgument<OnOff> ExtractMolecule { get; set; } = OnOff.Off;
        public InArgument<OnOff> ExtractReaction { get; set; } = OnOff.On;
        public InArgument<OnOff> ExtractTable { get; set; } = OnOff.Off;
        public InArgument<OnOff> ExtractChart { get; set; } = OnOff.Off;

        public OutArgument<string> ErrorMessage { get; set; }
        public OutArgument<string> RequestId { get; set; }
        public OutArgument<int> WaitSeconds { get; set; }
        public OutArgument<DDUResult> Result { get; set; }


        private UiPathHttpClient _httpClient;
        private DDUResult _dduresult;
        private string _errorMessage;   
        private string _requestId;  
        private int _waitSeconds = 0; // default value is 0 seconds 

        public AnalyzeDocument()
        {
            // this is the default endpoint
            _httpClient = new UiPathHttpClient("https://gw.lgair.net");

        }
        private async Task<DDUResult> ExecuteInternalAsync(string endpoint, string apikey, IResource fileresource, OnOff molecule, OnOff reaction, OnOff table, OnOff chart, OnOff waitresult)
        {
            var _dict = new Dictionary<string, object>();
            _dict.Add("molecule", molecule == OnOff.On);
            _dict.Add("reaction", reaction == OnOff.On);
            _dict.Add("table", table == OnOff.On);
            _dict.Add("chart", chart == OnOff.On);
            _dict.Add("inputs_format", "bytes");
            // use this to automatically attach the debugger to the process
            //Debugger.Launch();
            _httpClient.setEndpoint(endpoint);
            _httpClient.setApiKey(apikey);
            _httpClient.AddFileResource(fileresource, "inputs");
            _httpClient.AddField("params", JsonConvert.SerializeObject(_dict));
            _errorMessage = string.Empty;

            try
            {
                var resp1 = await _httpClient.Analyze();
                if (resp1.status == HttpStatusCode.OK)
                {

                    var resp = JsonConvert.DeserializeObject<DDUResponse>(resp1.body);
#if DEBUG
                    Console.WriteLine($"respons Id: {resp.Id} estimatedTime: {resp.outputs[0].EstimatedTime}");
#endif
                    Console.WriteLine($"요청을 처리하는데 약 {resp.outputs[0].EstimatedTime}초가 걸립니다...");
                    _requestId = resp.Id;
                    _waitSeconds = resp.outputs[0].EstimatedTime;   
                    if (waitresult == OnOff.On)
                    {
                        await Task.Delay(_waitSeconds * 1000);

                        _httpClient.Clear();
                        _httpClient.setEndpoint(endpoint);
                        _httpClient.setApiKey(apikey);
                        var resp2 = await _httpClient.GetResult(resp.Id);
                        if (resp2.status == HttpStatusCode.OK)
                        {
                            _dduresult = JsonConvert.DeserializeObject<DDUResult>(resp2.body);
                        }
                        else
                        {
                            _errorMessage = resp2.body;
                        }
                    }
                }
                else
                {
                    _errorMessage = resp1.body;
                }
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
            }
            return _dduresult;

        }

        protected  IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
        {
            var endpoint = Endpoint.Get(context);
            var apikey = ApiKey.Get(context);
            var fileresource = InputFile.Get(context);
            var molecule = ExtractMolecule.Get(context);
            var reaction = ExtractReaction.Get(context);
            var table = ExtractTable.Get(context);
            var chart = ExtractChart.Get(context);
            var waitresult = WaitResult.Get(context);

            var task = new Task ( _ => ExecuteInternalAsync(endpoint, apikey, fileresource, molecule, reaction, table, chart, waitresult), state);
            task.Start();
            if (callback != null)
            {
                task.ContinueWith(t => callback(t));
                task.Wait();
            }
            return task;
        }

        protected  void EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            var task = (Task)result;

            if (task.IsCompletedSuccessfully)
            {
                ErrorMessage.Set(context, string.Empty);
                Result.Set(context, _dduresult);
                WaitSeconds.Set(context, _waitSeconds);
            }
            else
            {
                ErrorMessage.Set(context, _errorMessage);
            }
        }

        protected override void Execute(CodeActivityContext context)
        {
            var endpoint = Endpoint.Get(context);
            var apikey = ApiKey.Get(context);
            var fileresource = InputFile.Get(context);
            var molecule = ExtractMolecule.Get(context);
            var reaction = ExtractReaction.Get(context);
            var table = ExtractTable.Get(context);
            var chart = ExtractChart.Get(context);
            var waitresult = WaitResult.Get(context);

            var result = ExecuteInternalAsync(endpoint, apikey, fileresource, molecule, reaction, table, chart, waitresult).Result;
            if (string.IsNullOrEmpty(_errorMessage))
            {
                ErrorMessage.Set(context, string.Empty);
                RequestId.Set(context, _requestId);
                Result.Set(context, _dduresult);
                WaitSeconds.Set(context, _waitSeconds);
            }
            else
            {
                ErrorMessage.Set(context, _errorMessage);
                RequestId.Set(context, string.Empty);
            }
        }
    }
}
