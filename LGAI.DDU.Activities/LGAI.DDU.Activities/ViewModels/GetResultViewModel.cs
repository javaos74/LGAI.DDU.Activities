using LGAI.DDU.Activities.Model;
using LGAI.DDU.Activities.Helpers;
using System.Activities;
using System.Activities.DesignViewModels;
using UiPath.Platform.ResourceHandling;
using Microsoft.CodeAnalysis.Emit;

namespace LGAI.DDU.Activities.ViewModels
{
    public class GetResultViewModel : DesignPropertiesViewModel
    {
        /*
         * The result property comes from the activity's base class
         */
        public DesignInArgument<string> Endpoint { get; set; }
        public DesignInArgument<string> ApiKey { get; set; }
        public DesignOutArgument<string> ErrorMessage { get; set; }
        public DesignInArgument<string> RequestId { get; set; }
        public DesignOutArgument<DDUResult> Result { get; set; }


        public GetResultViewModel(IDesignServices services) : base(services)
        {
        }

        protected override void InitializeModel()
        {
            /*
             * The base call will initialize the properties of the view model with the values from the xaml or with the default values from the activity
             */
            base.InitializeModel();

            PersistValuesChangedDuringInit(); // mandatory call only when you change the values of properties during initialization
            int propertyOrderIndex = 1;
            Endpoint.OrderIndex = propertyOrderIndex++;
            ApiKey.OrderIndex = propertyOrderIndex++;
            RequestId.OrderIndex = propertyOrderIndex++;
            Result.OrderIndex = propertyOrderIndex++;
            ErrorMessage.OrderIndex = propertyOrderIndex++;
        }
    }
}
