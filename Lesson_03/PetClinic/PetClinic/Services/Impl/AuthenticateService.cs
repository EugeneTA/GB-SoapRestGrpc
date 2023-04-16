using Microsoft.IdentityModel.Tokens;
using PetClinic.Data;
using PetClinic.Data.Tables;
using PetClinic.Models;
using PetClinic.Models.Dto;
using PetClinic.Utils;
using PetClinicNamespace;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PetClinic.Services.Impl
{
    public class AuthenticateService: IAuthenticateService
    {
        public const string SecretKey = "jkrDkfyqnnf+!RsfgrWdlfkd";
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly Dictionary<string, SessionContext> _sessions =
            new Dictionary<string, SessionContext>();

        public AuthenticateService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public SessionContext GetSession(string sessionToken)
        {
            SessionContext sessionContext;

            lock (_sessions)
            {
                _sessions.TryGetValue(sessionToken, out sessionContext);
            }

            if (sessionContext == null)
            {
                using IServiceScope scope = _serviceScopeFactory.CreateScope();
                PetClinicDbContext context = scope.ServiceProvider.GetRequiredService<PetClinicDbContext>();

                AccountSession session = context.AccountSessions.FirstOrDefault(item => item.SessionToken == sessionToken);

                if (session == null)
                    return null;

                Account account = context.Accounts.FirstOrDefault(item => item.Id == session.AccountId);

                sessionContext = GetSessionContext(account, session);

                lock (_sessions)
                {
                    _sessions[sessionToken] = sessionContext;
                }

            }

            return sessionContext;

        }

        public AuthenticationResponse Login(AuthenticationRequest authenticationRequest)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            PetClinicDbContext context = scope.ServiceProvider.GetRequiredService<PetClinicDbContext>();

            Account account = FindAccountByLogin(context, authenticationRequest.Login);

            if (account == null)
            {
                return new AuthenticationResponse
                {
                    Status = (int)AuthenticationStatus.UserNotFound
                };
            }

            if (!PasswordUtils.VerifyPassword(authenticationRequest.Password, account.PasswordSalt, account.PasswordHash))
            {
                return new AuthenticationResponse
                {
                    Status = (int)AuthenticationStatus.InvalidPassword
                };
            }



            AccountSession session = new AccountSession
            {
                AccountId = account.Id,
                SessionToken = CreateSessionToken(account),
                TimeCreated = DateTime.Now,
                TimeLastRequest = DateTime.Now,
                IsClosed = false,
            };

            context.AccountSessions.Add(session);
            context.SaveChanges();




            SessionContext sessionDto = GetSessionContext(account, session);

            lock (_sessions)
            {
                _sessions[session.SessionToken] = sessionDto;
            }

            return new AuthenticationResponse
            {
                Status = (int)AuthenticationStatus.Success,
                Context = sessionDto
            };

        }

        private SessionDto GetSessionDto(Account account, AccountSession accountSession)
        {
            return new SessionDto
            {
                SessionId = accountSession.Id,
                SessionToken = accountSession.SessionToken,
                Account = new AccountDto
                {
                    AccountId = account.Id,
                    EMail = account.EMail,
                    FirstName = account.FirstName,
                    LastName = account.LastName,
                    SecondName = account.SecondName,
                    Locked = account.Locked
                }
            };
        }

        private SessionContext GetSessionContext(Account account, AccountSession accountSession)
        {
            return new SessionContext
            {
                SessionId = accountSession.Id,
                SessionToken = accountSession.SessionToken,
                Account = new AccountDto
                {
                    AccountId = account.Id,
                    EMail = account.EMail,
                    FirstName = account.FirstName,
                    LastName = account.LastName,
                    SecondName = account.SecondName,
                    Locked = account.Locked
                }
            };
        }

        private string CreateSessionToken(Account account)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(SecretKey);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]{
                        new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                        new Claim(ClaimTypes.Name, account.EMail),
                    }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        private Account FindAccountByLogin(PetClinicDbContext context, string login)
        {
            return context
                .Accounts
                .FirstOrDefault(account => account.EMail == login);
        }

        private AccountSession FindTokenByAccountId(PetClinicDbContext context, int accountID)
        {
            return context.AccountSessions
                .FirstOrDefault(account => (account.Id == accountID && account.IsClosed == false));

        }
    }
}
