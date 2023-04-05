using PumpClient.PumpServiceReference;
using System;
using System.ServiceModel;


namespace PumpClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InstanceContext instanceContext = new InstanceContext(new CallbackHandler());
            PumpServiceClient client = new PumpServiceClient(instanceContext);

            client.UpdateAndCompileScript(@"c:\temp\sample.script");
            client.RunSript();

            Console.WriteLine("Нажмите любую клавишу для завершения работы...");
            Console.ReadKey();
            client.Close();
        }
    }
}
