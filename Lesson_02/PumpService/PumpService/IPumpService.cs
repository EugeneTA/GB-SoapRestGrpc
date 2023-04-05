using System.ServiceModel;


namespace PumpService
{
    [ServiceContract(Namespace= "http://Miscrosoft.ServiceModel.Samples", SessionMode = SessionMode.Required, CallbackContract = typeof(IPumpServiceCallback))]
    public interface IPumpService
    {
        [OperationContract]
        void RunSript();

        [OperationContract]
        void UpdateAndCompileScript(string filename);
    }
}
