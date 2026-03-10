namespace Application.Web.Utility
{
    public class StaticDetails
    {
        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";
        public const string TokenCookie = "JwtToken";
        public enum RequestType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
    
}
