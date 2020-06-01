namespace Identity.Account.Payload
{
    public class AccountResponseModel
    {
        public string Guid { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string SocialSecurityNumber { get; set; }
    }
}