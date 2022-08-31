using SourceCode.SmartObjects.Services.ServiceSDK;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;


namespace JSReportForK2
{


    public class JSReportServiceBrokerClass : ServiceAssemblyBase
    {

        public override string DescribeSchema()
        {
            //TODO: Since this is a static broker, you would add static service objects using attribute decoration.
            //The recommended approach is to create separate classes for each of your static service objects
            //in the sample implementation, we iterate over each of the classes in the assembly and if they are decorated with the
            //ServiceObjectAttribute, we add them as service objects.
            //if you prefer, you can manually add Service Objects like this instead:
            this.Service.ServiceObjects.Create(new ServiceObject(typeof(JSReportForK2.JSReport)));

            this.Service.Name = "JSReport";
            this.Service.MetaData.DisplayName = "JSReport";
            this.Service.MetaData.Description = "JSReport";

            ServicePackage.IsSuccessful = true;

            return base.DescribeSchema();
        }

        public override void Extend()
        {
            //throw new Exception("The method or operation is not implemented."); 
        }

        public override string GetConfigSection()
        {
            //In this example, we are adding two configuration values, one required and one optional, and one with a default value
            this.Service.ServiceConfiguration.Add("URLJSReport", true, "http://myJsReport.com/");
            this.Service.ServiceConfiguration.Add("Login", true, string.Empty);
            this.Service.ServiceConfiguration.Add("Password", true, string.Empty);
            //return the configuration as XML
            return base.GetConfigSection();
        }

    }
}
