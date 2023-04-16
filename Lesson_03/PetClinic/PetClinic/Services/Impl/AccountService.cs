using Grpc.Core;
using PetClinic.Data.Tables;
using PetClinic.Data;
using static PetClinicNamespace.AccountService;
using PetClinicNamespace;
using Microsoft.AspNetCore.Authorization;
using PetClinic.Utils;

namespace PetClinic.Services.Impl
{
    [Authorize]
    public class AccountService : AccountServiceBase
    {
        private readonly PetClinicDbContext _dbContext;

        public AccountService(PetClinicDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Account

        [AllowAnonymous]
        public override Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request, ServerCallContext context)
        {
            CreateAccountResponse response = new CreateAccountResponse();

            try
            {
                (var passwordSalt, var passwordHash) = PasswordUtils.CreatePasswordHash(request.Password);

                var account = new Account
                {
                    PasswordSalt = passwordSalt,
                    PasswordHash = passwordHash,
                    EMail = request.Email,
                    FirstName = request.FirstName,
                    SecondName = request.SecondName,
                    LastName = request.LastName,
                };

                _dbContext.Accounts.Add(account);
                _dbContext.SaveChanges();

                response.AccountId = account.Id;
                response.ErrCode = 0;
                response.ErrMessage = string.Empty;

            }
            catch (Exception ex)
            {
                response.AccountId = null;
                response.ErrCode = 1;
                response.ErrMessage = ex.Message;

            }

            return Task.FromResult(response);

        }

        [Authorize]
        public override Task<DeleteAccountResponse> DeleteAccount(DeleteAccountRequets request, ServerCallContext context)
        {
            DeleteAccountResponse response = new DeleteAccountResponse();

            try
            {
                var account = _dbContext.Accounts.FirstOrDefault(account => account.Id == request.AccountId);

                if (account == null)
                {
                    response.ErrCode = 3;
                    response.ErrMessage = $"No such account with id {request.AccountId} found";
                }
                else
                {
                    var deleteResult = _dbContext.Accounts.Remove(account);

                    if (deleteResult == null)
                    {
                        response.ErrCode = 4;
                        response.ErrMessage = $"Error delete account with id {request.AccountId}";
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

        [Authorize]
        public override Task<UpdateAccountResponse> UpdateAccount(UpdateAccountRequets request, ServerCallContext context)
        {
            UpdateAccountResponse response = new UpdateAccountResponse();

            try
            {

                var account = _dbContext.Accounts.FirstOrDefault(account => account.Id == request.AccountId);

                if (account == null)
                {
                    response.ErrCode = 3;
                    response.ErrMessage = $"No such account with id {request.AccountId} found";
                }
                else
                {
                    account.FirstName = request.FirstName;
                    account.SecondName = request.SecondName;
                    account.LastName = request.LastName;
                    account.EMail = request.Email;

                    _dbContext.Accounts.Update(account);
                    var updateResult = _dbContext.SaveChanges();

                    if (updateResult == 0)
                    {
                        response.ErrCode = 5;
                        response.ErrMessage = $"Database account update error";
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

       
        public override Task<GetAccountByIdResponse> GetAccountById(GetAccountByIdRequest request, ServerCallContext context)
        {
            GetAccountByIdResponse response = new GetAccountByIdResponse();

            try
            {
                var account = _dbContext.Accounts.FirstOrDefault(account => account.Id == request.AccountId);

                if (account == null)
                {
                    response.Account = null;
                    response.ErrCode = 3;
                    response.ErrMessage = $"No such account with id {request.AccountId} found";
                }
                else
                {
                    response.Account = new AccountResponse
                    {
                        AccountId = account.Id,
                        Email = account.EMail,
                        FirstName = account.FirstName,
                        SecondName = account.SecondName,
                        LastName = account.LastName,
                    };
                    response.ErrCode = 0;
                    response.ErrMessage = string.Empty;
                }

            }
            catch (Exception ex)
            {
                response.Account = null;
                response.ErrCode = 2;
                response.ErrMessage = ex.Message;

            }

            return Task.FromResult(response);
        }

        #endregion

    }
}
