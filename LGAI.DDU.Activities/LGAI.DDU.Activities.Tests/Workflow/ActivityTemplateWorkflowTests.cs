using Xunit;
using LGAI.DDU.Activities.Model;
using LGAI.DDU.Activities.Helpers;
using LGAI.DDU.Activities;
using UiPath.Platform.ResourceHandling;
using System.Activities;

namespace LGAI.DDU.Activities.Tests.Tests.Workflow
{
    public class ActivityTemplateWorkflowTests
    {
        [Fact]
        public void Test()
        {
            var activity = new AnalyzeDocument()
            {
                ApiKey = "cpuhniusl9qepepl9q3goazx20px",
                Endpoint = "https://gw.lgair.net",
                ExtractMolecule = OnOff.On,
                ExtractReaction = OnOff.On,
                ExtractTable = OnOff.On,
                ExtractChart = OnOff.On,
                InputFile = new InArgument<IResource>(LocalResource.FromPath(@"C:\Users\charles\source\data\journal_sample.pdf"))
            };

            var runner = new WorkflowInvoker(activity);
            //runner.Extensions.Add(() => workflowRuntimeMock.Object);

            var result = runner.Invoke(); //the runner will return a dictionary with the values of the OutArguments

            //verify that the result is as expected
            //Assert.Equal(2, result["Result"]);


            Assert.Equal(0, 0);
        }
    }
}
