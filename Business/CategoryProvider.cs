using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace NamNamAPI.Business
{
    using System.Globalization;
    using Microsoft.EntityFrameworkCore;
    using NamNamAPI.Domain;
    using NamNamAPI.Models;
    using Newtonsoft.utility;

    public class CategoryProvider
    {
        private NamnamContext connectionModel;

        public CategoryProvider(NamnamContext _connectionModel)
        {
            string Culture = "es-MX";
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Culture);
            connectionModel = _connectionModel;
        }

        public (int, List<CategoryDomain>, string) getCategories()
        {
            int code = 200;
            List<CategoryDomain> categoryList = new List<CategoryDomain>();
            string report = "";
            try
            {
                var listTemp = connectionModel.Categories.ToList();
                foreach (var item in listTemp)
                {
                    CategoryDomain category = new CategoryDomain
                    {
                        idCategory = item.IdCategory,
                        categoryName = item.CategoryName
                    };
                    categoryList.Add(category);
                }
            }
            catch (Exception e)
            {
                code = 500;
                report = e.Message;
            }
            return (code, categoryList, report);
        }

        public (int, GetPreferenceResponse) getUserPreferences(string idUser)
        {
            int code = 200;
            List<CategoryDomain> categoryList = new List<CategoryDomain>();
            GetPreferenceResponse preferencesUser = new GetPreferenceResponse();
            preferencesUser.categories = new List<CategoryDomain>();
            preferencesUser.Userpreferences = new List<CategoryDomain>();
            try
            {

                var preferencesForUser = connectionModel.Preferences
            .Where(p => p.IdUser == idUser)
            .Include(p => p.IdCategoryNavigation) // Opcional: carga la categoría relacionada
            .ToList();

                if (preferencesForUser.Count >= 1)
                {
                    foreach (var item in preferencesForUser)
                    {
                        CategoryDomain categoryTemp = new CategoryDomain();
                        categoryTemp.idCategory = item.IdCategoryNavigation.IdCategory;
                        categoryTemp.categoryName = item.IdCategoryNavigation.CategoryName;
                        categoryList.Add(categoryTemp);
                    }
                    preferencesUser.Userpreferences = categoryList;
                }
                (int cadeCategories, List<CategoryDomain> list, string error) = getCategories(); 
                preferencesUser.categories = list;
            }
            catch (Exception e)
            {
                code = 500;
            }

            return (code, preferencesUser);
        }


        public int SetUserPreferences(SetPreferenceResponse preferencesResponse)
        {
            int code = 200;
            List<CategoryDomain> categoryList = new List<CategoryDomain>();
            try
            {
                
                //eliminando
                var deleteElementList = connectionModel.Preferences.Where(a => a.IdUser.Equals(preferencesResponse.IdUser)).ToList();
                if(deleteElementList.Count >= 1)
                {
                    connectionModel.Preferences.RemoveRange(deleteElementList);
                    connectionModel.SaveChanges();
                }
                
                //agregando
                if (preferencesResponse.UserPreference.Count >= 0)
                {
                    List<Preference> preferenceModelList = new List<Preference>();
                    foreach (var item in preferencesResponse.UserPreference)
                    {
                        Preference preferenceTemp = new Preference();
                        preferenceTemp.IdPreference = GenerateRandomID.GenerateID();
                        preferenceTemp.IdCategory = item;
                        preferenceTemp.IdUser = preferencesResponse.IdUser;
                        preferenceModelList.Add(preferenceTemp);
                    }
                    connectionModel.Preferences.AddRange(preferenceModelList);
                    int changes = 0;
                    changes = connectionModel.SaveChanges();
                }
            }
            catch (Exception e)
            {
                code = 500;
            }

            return code;
        }
    }
}

