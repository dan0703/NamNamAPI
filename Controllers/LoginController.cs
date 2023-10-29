namespace NamNamAPI.Controllers
{
    using Domain;
    //token classes
    using System.Security.Claims;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using NamNamAPI.Business;
    using NamNamAPI.Models;
    using System.Net;
    using Newtonsoft.utility;
    using System.Net.Http;

    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private IConfiguration config;
        private readonly LoginProvider _login;

        public LoginController(IConfiguration config, LoginProvider login)
        {
            this.config = config;
            _login = login;
        }

        [ApiExplorerSettings(IgnoreApi = false)]
        [HttpPost("LoginUser")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> ValidateLoginUser([FromBody] LoginDomain loginCredentials)
        {
            try
            {
                UserDomain user = await _login.Login(loginCredentials);
                string jwtToken = "";
                if (user == null)
                {
                    return NotFound(StatusCodes.Status404NotFound);
                } else{
                     jwtToken = GenerateToken(user);
                }
                return Ok(new JsonResult(new {jwtToken, user.idUser}));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [ApiExplorerSettings(IgnoreApi = false)]
        [HttpPost("RegisterUser")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> RegisterUser([FromBody] UserDomain userInformation){
            try{

                userInformation.idUser = GenerateRandomID.GenerateID();

                var result = await _login.Register(userInformation);
                if(result == null){
                    return StatusCode((int)HttpStatusCode.InternalServerError);
                }
                return Ok(result);
            }
            catch(Exception ex){
                Console.WriteLine(ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
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
