namespace NamNamAPI.Business
{

    using System.Globalization;
    using Microsoft.EntityFrameworkCore;
    using NamNamAPI.Domain;
    using NamNamAPI.Models;


    public class LoginProvider
    {
        private readonly NamnamContext _connectionModel;

        public LoginProvider(NamnamContext connectionModel)
        {
            string Culture = "es-MX";
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Culture);
            _connectionModel = connectionModel;
        }

        public async Task<UserDomain> Login(LoginDomain login)
        {
            bool canConnect = await _connectionModel.Database.CanConnectAsync();
            try
            {
                if (!canConnect)
                {
                    throw new Exception("No se pudo establecer conexión con la base de datos.");
                }
                else
                {
                    var user = await _connectionModel.Users.Where(x => x.Email == login.email && x.Password == login.password).FirstOrDefaultAsync();

                    return new UserDomain
                    {
                        idUser = user.IdUser,
                        firstname = user.FirstName,
                        email = user.Email,
                    };
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}