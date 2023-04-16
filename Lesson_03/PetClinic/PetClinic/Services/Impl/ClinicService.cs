using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using PetClinic.Data;
using PetClinic.Data.Tables;
using PetClinicNamespace;
using static PetClinicNamespace.ClinicService;

namespace PetClinic.Services.Impl
{
    [Authorize]
    public class ClinicService : ClinicServiceBase
    {
        private readonly PetClinicDbContext _dbContext;

        public ClinicService(PetClinicDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Client

        public override Task<CreateClientResponse> CreateClient(CreateClientRequest request, ServerCallContext context)
        {
            CreateClientResponse response = new CreateClientResponse();

            try
            {
                var client = new Client
                {
                    Document = request.Document,
                    FirstName = request.FirstName,
                    Surname = request.Surname,
                    Patronymic = request.Patronymic,
                };

                _dbContext.Clients.Add(client);
                _dbContext.SaveChanges();

                response.Clientid = client.Key;
                response.ErrCode = 0;
                response.ErrMessage = string.Empty;

            }
            catch (Exception ex)
            {
                response.Clientid = null;
                response.ErrCode = 1;
                response.ErrMessage = ex.Message;

            }

            return Task.FromResult(response);

        }

        public override Task<DeleteClientResponse> DeleteClient(DeleteClientRequets request, ServerCallContext context)
        {
            DeleteClientResponse response = new DeleteClientResponse();

            try
            {

                var client = _dbContext.Clients.FirstOrDefault(client => client.Key == request.ClientId);

                if (client == null)
                {
                    response.ErrCode = 3;
                    response.ErrMessage = $"No such client with id {request.ClientId} found";
                }
                else
                {
                    var deleteResult = _dbContext.Clients.Remove(client);

                    if (deleteResult == null)
                    {
                        response.ErrCode = 4;
                        response.ErrMessage = $"Error delete client with id {request.ClientId}";
                    }
                    else
                    {
                        response.ErrCode = 0;
                        response.ErrMessage = string.Empty;
                    }

                }
            }
            catch (Exception ex)
            {
                response.ErrCode = 1;
                response.ErrMessage = ex.Message;

            }

            return Task.FromResult(response);
        }

        public override Task<UpdateClientResponse> UpdateClient(UpdateClientRequets request, ServerCallContext context)
        {
            UpdateClientResponse response = new UpdateClientResponse();

            try
            {

                var client = _dbContext.Clients.FirstOrDefault(client => client.Key == request.ClientId);

                if (client == null)
                {
                    response.ErrCode = 3;
                    response.ErrMessage = $"No such client with id {request.ClientId} found";
                }
                else
                {
                    client.FirstName = request.FirstName;
                    client.Surname = request.Surname;
                    client.Patronymic = request.Patronymic;
                    client.Document = request.Document;

                    _dbContext.Clients.Update(client);
                    var updateResult = _dbContext.SaveChanges();

                    if (updateResult == 0)
                    {
                        response.ErrCode = 5;
                        response.ErrMessage = $"Database client update error";
                    }
                    else
                    {
                        response.ErrCode = updateResult;
                        response.ErrMessage = string.Empty;
                    }

                }
            }
            catch (Exception ex)
            {
                response.ErrCode = 1;
                response.ErrMessage = ex.Message;

            }

            return Task.FromResult(response);
        }

        public override Task<GetClientsResponse> GetClients(GetClientsRequest request, ServerCallContext context)
        {
            GetClientsResponse response = new GetClientsResponse();

            try
            {
                response.Clients.AddRange
                (
                    _dbContext.Clients.Select(client => new ClientResponse
                    {
                        ClientId = client.Key,
                        Document = client.Document,
                        FirstName = client.FirstName,
                        Surname = client.Surname,
                        Patronymic = client.Patronymic,
                    }).ToList()
                );

                response.ErrCode = 0;
                response.ErrMessage = string.Empty;

            }
            catch (Exception ex)
            {
                response.Clients.Clear();
                response.ErrCode = 2;
                response.ErrMessage = ex.Message;

            }

            return Task.FromResult(response);
        }

        public override Task<GetClientByIdResponse> GetClientById(GetClientByIdRequest request, ServerCallContext context)
        {
            GetClientByIdResponse response = new GetClientByIdResponse();

            try
            {
                var client = _dbContext.Clients.FirstOrDefault(client => client.Key == request.ClientId);

                if (client == null)
                {
                    response.Client = null;
                    response.ErrCode = 3;
                    response.ErrMessage = $"No such client with id {request.ClientId} found";
                }
                else
                {
                    response.Client = new ClientResponse
                    {
                        ClientId = client.Key,
                        Document = client.Document,
                        FirstName = client.FirstName,
                        Surname = client.Surname,
                        Patronymic = client.Patronymic,
                    };
                    response.ErrCode = 0;
                    response.ErrMessage = string.Empty;
                }

            }
            catch (Exception ex)
            {
                response.Client = null;
                response.ErrCode = 2;
                response.ErrMessage = ex.Message;

            }

            return Task.FromResult(response);
        }

        #endregion

        #region Pet

        public override Task<CreatePetResponse> CreatePet(CreatePetRequest request, ServerCallContext context)
        {
            CreatePetResponse response = new CreatePetResponse();

            try
            {
                //DateTime.TryParse(request.Birthday, out var petBirthday);

                var pet = new Pet
                {
                    ClientId = request.ClientId,
                    Name = request.Name,
                    Birthday = request.Birthday.ToDateTime(),
                 };

                
                _dbContext.Pets.Add(pet);
                _dbContext.SaveChanges();

                response.PetId = pet.Id;
                response.ErrCode = 0;
                response.ErrMessage = string.Empty;

            }
            catch (Exception ex)
            {
                response.PetId = null;
                response.ErrCode = 1;
                response.ErrMessage = ex.Message;

            }

            return Task.FromResult(response);

        }

        public override Task<DeletePetResponse> DeletePet(DeletePetRequets request, ServerCallContext context)
        {
            DeletePetResponse response = new DeletePetResponse();

            try
            {

                var pet = _dbContext.Pets.FirstOrDefault(pet => pet.Id == request.PetId);

                if (pet == null)
                {
                    response.ErrCode = 3;
                    response.ErrMessage = $"No such pet with id {request.PetId} found";
                }
                else
                {
                    var deleteResult = _dbContext.Pets.Remove(pet);

                    if (deleteResult == null)
                    {
                        response.ErrCode = 4;
                        response.ErrMessage = $"Error delete pet with id {request.PetId}";
                    }
                    else
                    {
                        response.ErrCode = 0;
                        response.ErrMessage = string.Empty;
                    }

                }
            }
            catch (Exception ex)
            {
                response.ErrCode = 1;
                response.ErrMessage = ex.Message;

            }

            return Task.FromResult(response);
        }

        public override Task<UpdatePetResponse> UpdatePet(UpdatePetRequets request, ServerCallContext context)
        {
            UpdatePetResponse response = new UpdatePetResponse();

            try
            {
                var pet = _dbContext.Pets.FirstOrDefault(pet => pet.Id == request.PetId);

                if (pet == null)
                {
                    response.ErrCode = 3;
                    response.ErrMessage = $"No such pet with id {request.PetId} found";
                }
                else
                {
                    pet.ClientId = request.ClientId;
                    pet.Name = request.Name;
                    pet.Birthday = request.Birthday.ToDateTime();

                    _dbContext.Pets.Update(pet);
                    var updateResult = _dbContext.SaveChanges();

                    if (updateResult == 0)
                    {
                        response.ErrCode = 5;
                        response.ErrMessage = $"Database pet update error";
                    }
                    else
                    {
                        response.ErrCode = updateResult;
                        response.ErrMessage = string.Empty;
                    }

                }
            }
            catch (Exception ex)
            {
                response.ErrCode = 1;
                response.ErrMessage = ex.Message;

            }

            return Task.FromResult(response);
        }

        public override Task<GetPetsResponse> GetPets(GetPetsRequest request, ServerCallContext context)
        {
            GetPetsResponse response = new GetPetsResponse();

            try
            {
                response.Pets.AddRange
                (
                    _dbContext.Pets.Select(pet => new PetResponse
                    {
                        PetId = pet.Id,
                        ClientId = pet.ClientId,
                        Name = pet.Name,
                        Birthday = pet.Birthday.ToUniversalTime().ToTimestamp(),
                    }).ToList()
                );

                response.ErrCode = 0;
                response.ErrMessage = string.Empty;

            }
            catch (Exception ex)
            {
                response.Pets.Clear();
                response.ErrCode = 2;
                response.ErrMessage = ex.Message;

            }

            return Task.FromResult(response);
        }

        public override Task<GetPetByIdResponse> GetPetById(GetPetByIdRequest request, ServerCallContext context)
        {
            GetPetByIdResponse response = new GetPetByIdResponse();

            try
            {
                var pet = _dbContext.Pets.FirstOrDefault(pet => pet.Id == request.PetId);

                if (pet == null)
                {
                    response.Pet = null;
                    response.ErrCode = 3;
                    response.ErrMessage = $"No such pet with id {request.PetId} found";
                }
                else
                {
                    response.Pet = new PetResponse
                    {
                        PetId = pet.Id,
                        ClientId = pet.ClientId,
                        Name = pet.Name,
                        Birthday = pet.Birthday.ToUniversalTime().ToTimestamp()
                    };
                    response.ErrCode = 0;
                    response.ErrMessage = string.Empty;
                }

            }
            catch (Exception ex)
            {
                response.Pet = null;
                response.ErrCode = 2;
                response.ErrMessage = ex.Message;

            }

            return Task.FromResult(response);
        }

        #endregion

    }
}
