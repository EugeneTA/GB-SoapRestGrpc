using PetClinicNamespace;

namespace PetClinic.Services
{
    public interface IAuthenticateService
    {
        AuthenticationResponse Login(AuthenticationRequest authenticationRequest);

        public SessionContext GetSession(string sessionToken);
    }
}
