using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using PetClinicNamespace;
using static PetClinicNamespace.ClinicService;

namespace PetClinicClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Настройка для использования незащищенного соединения
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            using var grpcCahnnel = GrpcChannel.ForAddress("http://localhost:5001");
            ClinicServiceClient client = new ClinicServiceClient(grpcCahnnel);

            Console.Write("\n Введите фамилию клиента: ");
            string clientSurname = Console.ReadLine();

            Console.Write(" Введите имя клиента: ");
            string clientFirstName = Console.ReadLine();

            Console.Write(" Введите отчество клиента: ");
            string clientPatrynomic = Console.ReadLine();

            Console.Write(" Введите документ: ");
            string clientDocument = Console.ReadLine();

            var clientCreateResponse = client.CreateClient(new CreateClientRequest
            {
                FirstName = clientFirstName,
                Surname = clientSurname,
                Patronymic = clientPatrynomic,
                Document = clientDocument
            });

            if (clientCreateResponse != null && clientCreateResponse.ErrCode == 0)
            {
                Console.WriteLine($" Client #{clientCreateResponse.Clientid} created successfully.");
            }
            else
            {
                Console.WriteLine($" Client create error. Error code {clientCreateResponse?.ErrCode}. Error message: {clientCreateResponse?.ErrMessage}");
            }

            var clients = client.GetClients(new GetClientsRequest());

            if (clients != null && clients.ErrCode == 0)
            {
                int cnt = 0;
                Console.WriteLine("\n Список клиентов:");
                foreach (var c in clients.Clients)
                {
                    cnt++;
                    Console.WriteLine($" [ {cnt} ] {c.Surname} {c.FirstName} {c.Patronymic} {c.Document}");
                }
            }
            else
            {
                Console.WriteLine($" Get clients error. Error code {clients?.ErrCode}. Error message: {clients?.ErrMessage}");
            }

            Console.Write("\n Введите имя питомца: ");
            string petName = Console.ReadLine();

            Console.Write(" Введите id клиента: ");
            int.TryParse(Console.ReadLine(), out int clientId);

            var petCreateResponse = client.CreatePet(new CreatePetRequest
            {
                ClientId = clientId,
                Name = petName,
                Birthday = DateTime.UtcNow.ToTimestamp(),
            });

            if (petCreateResponse != null && petCreateResponse.ErrCode == 0)
            {
                Console.WriteLine($" Pet #{petCreateResponse.PetId} created successfully.");
            }
            else
            {
                Console.WriteLine($" Pet create error. Error code {petCreateResponse?.ErrCode}. Error message: {petCreateResponse?.ErrMessage}");
            }

            var pets = client.GetPets(new GetPetsRequest());

            if (pets != null && pets.ErrCode == 0)
            {
                int cnt = 0;
                Console.WriteLine("\n Список питомцев:");
                foreach (var p in pets.Pets)
                {
                    cnt++;
                    Console.WriteLine($" [ {cnt} ] {p.Name} {p.Birthday.ToDateTime()} Хозяин - [{p.ClientId}]");
                }
            }
            else
            {
                Console.WriteLine($" Get pets error. Error code {pets?.ErrCode}. Error message: {pets?.ErrMessage}");
            }


            Console.ReadKey(true);

        }
    }
}