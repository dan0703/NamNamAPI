namespace NamNamAPI.Controllers
{
    using Domain;
    //token classes
    using System.Security.Claims;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private IConfiguration config;
        // private LoginProvider _login;

        public LoginController(IConfiguration config)
        {
            this.config = config;
            //codigo para iniciiarlizar el provider
        }

        [HttpPost("ValiLoginUser")]
        public ActionResult ValidateLoginUser([FromBody] LoginDomain loginCredentials)
        {
            //llamada aprovider
            //

            UserDomain userTest = new UserDomain() { firstname = "das", email = "correo@homail", password = "123" };
            if (loginCredentials.email.Equals("aa"))
            {
                string jwtToken = GenerateToken(userTest);
                return new JsonResult(new
                {
                    code = 200,
                    user = userTest,
                    token = jwtToken
                });
            }
            return new JsonResult(new { code = 403});


        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string GenerateToken(UserDomain userDomain)
        {
            string token = "";
            try
            {
                var claims = new[]
            {
            new Claim(ClaimTypes.Name, userDomain.firstname),
            new Claim(ClaimTypes.Email, userDomain.email)

        };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT:Key").Value));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var securityToken = new JwtSecurityToken(
                                    claims: claims,
                                    expires: DateTime.Now.AddMinutes(60),
                                    signingCredentials: creds
                    );
                token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return token;


        }
    }
}
