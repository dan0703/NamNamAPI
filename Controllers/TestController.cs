namespace NamNamAPI.Controllers
{
    using Domain;
    using System.Security.Claims;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using NamNamAPI.Business;
  
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private TestProvider testProvider;
        public TestController(TestProvider testProvider)
        {
           this.testProvider = testProvider;
        }

        [HttpGet("TestConnectionBD")]
        public ActionResult GetUsertest()
        {
            (int code, List<UserDomain>userList,string report) = testProvider.getUsers();
            return new JsonResult(new{
                code = code,
                users = userList,
                report = report
            });
        }
    }
}