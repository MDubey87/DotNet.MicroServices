namespace Application.Web.Services.Interfaces
{
    public interface ITokenProvider
    {
        public string? GetToken();
        public void SetToken(string token);
        public void ClearToken();
    }
}
