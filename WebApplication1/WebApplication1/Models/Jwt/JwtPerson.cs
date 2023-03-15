namespace WebApplication1.Models.Jwt
{
    public class JwtPerson
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "Admin";
        public JwtPerson(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
