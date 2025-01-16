using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Product_Catalog_Web_Application.Models;
using Product_Catalog_Web_Application.ViewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Product_Catalog_Web_Application.Helper
{
    public class JwtToken
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        public JwtToken(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _config = configuration;
        }
        public async Task<MyToken> GenerateToken(ApplicationUser myUser)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, myUser.Id));
            claims.Add(new Claim(ClaimTypes.Name, myUser.UserName));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var RoleClaim = await _userManager.GetRolesAsync(myUser);
            foreach (var role in RoleClaim)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            //Secret Key
            SymmetricSecurityKey SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]));
            // Apply algorithm and SecretKey
            SigningCredentials Sign = new SigningCredentials(SecretKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                expires: DateTime.Now.AddHours(1),
                claims: claims,
                signingCredentials: Sign

            );
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string expire = DateTime.Now.AddHours(1).ToString();
            return new MyToken(handler.WriteToken(token), expire);
        }
    }
}
