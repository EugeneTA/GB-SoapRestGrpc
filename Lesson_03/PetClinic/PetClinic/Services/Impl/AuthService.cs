using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;
using PetClinic.Models;
using PetClinicNamespace;
using System.Net.Http.Headers;
using static PetClinicNamespace.AuthenticateService;

namespace PetClinic.Services.Impl
{
    [Authorize]
    public class AuthService : AuthenticateServiceBase
    {
        private readonly IAuthenticateService _authenticateService;

        public AuthService(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        [AllowAnonymous]
        public override Task<AuthenticationResponse> Login(AuthenticationRequest request, ServerCallContext context)
        {
            AuthenticationResponse response = _authenticateService.Login(new AuthenticationRequest
            {
                Login = request.Login,
                Password = request.Password,
            });

            if (response != null)
            {
                if (response.Status == (int)AuthenticationStatus.Success)
                {
                    context.ResponseTrailers.Add("X-Session-Token", response.Context.SessionToken);
                    return Task.FromResult(response);
                }

                return Task.FromResult(new AuthenticationResponse
                {
                    Status = response.Status,
                    Context = new SessionContext()
                });

            }

            return Task.FromResult(new AuthenticationResponse
            {
                Status = (int)AuthenticationStatus.UserNotFound,
                Context = new SessionContext()
            });

            //return Task.FromResult(new AuthenticationResponse
            //{
            //    Status  = response.Status,
            //    Context = new SessionContext
            //    {
            //        SessionId = response.Context.SessionId,
            //        SessionToken = response.Context.SessionToken,
            //        Account = new AccountDto
            //        {
            //            AccountId = response.Context.Account.AccountId,
            //            EMail = response.Context.Account.EMail,
            //            FirstName = response.Context.Account.FirstName,
            //            LastName = response.Context.Account.LastName,
            //            SecondName = response.Context.Account.SecondName,
            //            Locked = response.Context.Account.Locked
            //        }
            //    }
            //});

        }

        public override Task<GetSessionResponse> GetSession(GetSessionRequest request, ServerCallContext context)
        {
            var authorizationHeader = context.RequestHeaders.FirstOrDefault(h => h.Key == HeaderNames.Authorization);
            if (authorizationHeader != null) 
            {
                AuthenticationHeaderValue.TryParse(authorizationHeader.Value, out var headerValue);

                if (headerValue != null)
                {
                    var scheme = headerValue.Scheme; // "Bearer"
                    var token = headerValue.Parameter; // Token

                    if (string.IsNullOrEmpty(token) == false)
                    {

                        SessionContext sessionContext = _authenticateService.GetSession(token);

                        if (sessionContext != null)
                        {
                            Task.FromResult(new GetSessionResponse
                            {
                                Context = sessionContext,
                            });
                        }
                    }
                }
            }

            return Task.FromResult(new GetSessionResponse());
        }
    }
}
