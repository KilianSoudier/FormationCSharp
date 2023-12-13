using Formation.DTO.Login;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Formation.Services
{
    public class UserProfile
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public Dictionary<string, string> Roles { get; set; }
            = new Dictionary<string, string>();
    }
    public interface IAuthService
    {
        /// <summary>
        /// Identification d'un utilisateur depuis une source de confiance par login et mot de passe
        /// </summary>
        /// <param name="username">Login</param>
        /// <param name="password">Mot de passe</param>
        /// <returns></returns>
        Task<UserProfile?> AuthenticateUserAsync(string username, string password);

        /// <summary>
        /// Création de token à partir d'un profil utilisateur identifié
        /// </summary>
        /// <param name="user">Profil utilisateur identifié</param>
        /// <returns>Jwt token</returns>
        string GetToken(UserProfile user);
    }
    public class AuthService : IAuthService
    {
        private readonly IConfiguration configuration;
        public AuthService(IConfiguration configuration)
        {
            this.configuration = configuration;   
        }
        
        public Task<UserProfile?> AuthenticateUserAsync(string username, string password)
        {
            return Task.FromResult<UserProfile?>(new UserProfile        //Renvoie une task de userprofile générique
            {
                Username = username,
                Email = "test@test.com",
                Roles = new Dictionary<string, string>
                {
                    { "","" }
                }
            });
        }
        
        public string GetToken(UserProfile user)
        {
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
            var claims = new List<Claim>()
            {
                new Claim("Id", Guid.NewGuid().ToString()),     //claim : revendiquer Ici on revendique le username et le mail.
                    new Claim(JwtRegisteredClaimNames.Sub, user.Username),      //Format standard rfc.
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };
            claims.AddRange(user.Roles.Select(r => new Claim(r.Key, r.Value)));
            var subject = new ClaimsIdentity(claims);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),         //Clé
                SecurityAlgorithms.HmacSha512Signature) //Algorithme
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);  //Objet trop complexe à exploiter
            var jwtToken = tokenHandler.WriteToken(token);          //Objet qui sera envoyé au client

            return jwtToken;
        }
    }
}
