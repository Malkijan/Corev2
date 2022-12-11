namespace Core.Security.EmailAuthenticator;

public interface IEmailAutenticatorHelper
{
    public Task<string> CreateEmailActivationKey();
    public Task<string> CreateEmailActivationCode();
}