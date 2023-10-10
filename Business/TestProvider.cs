namespace NamNamAPI.Business
{
    using System.Globalization;
    using NamNamAPI.Domain;
    using NamNamAPI.Models;

    public class TestProvider
    {
        private NamnamContext connectionModel;

        public TestProvider(NamnamContext _connectionModel)
        {
            string Culture = "es-MX";
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Culture);
            connectionModel = _connectionModel;
        }

        public (int, List<UserDomain>,string) getUsers()
        {
            int code = 200;
            List<UserDomain> userList = new List<UserDomain>();
            string report = "";
            try
            {
                var listTemp = connectionModel.Users.ToList();
                foreach (var item in listTemp)
                {
                    UserDomain user = new UserDomain
                    {
                        idUser = item.IdUser,
                        firstname = item.FirstName,
                        lastname = item.LastName,
                        email = item.Email,
                        password = item.Password
                    };
                    userList.Add(user);
                }
            }
            catch (Exception e)
            {
                code = 500;
                report = e.Message;
            }
            return (code, userList,report);
        }
    }
}
