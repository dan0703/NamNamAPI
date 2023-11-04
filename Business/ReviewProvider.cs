using Microsoft.AspNetCore.Http.Features;
using NamNamAPI.Domain;
using NamNamAPI.Models;
using Newtonsoft.utility;
using System.Globalization;

namespace NamNamAPI.Business
{
    public class ReviewProvider
    {
        private NamnamContext connectionModel;

        public ReviewProvider(NamnamContext _connectionModel)
        {
            string Culture = "es-MX";
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Culture);
            connectionModel = _connectionModel;
        }

        public (int, List<ReviewDomain>) getReviews(string idRecipe)
        {
            int code = 200;
            List<ReviewDomain> reviewList = new List<ReviewDomain>();
            try
            {
                var list = connectionModel.Reviews.Where(a => a.RecipeIdRecipe == idRecipe).ToList();
                foreach (var item in list)
                {
                    ReviewDomain reviewtemp = new ReviewDomain();
                    reviewtemp.idReview = item.IdReview;
                    reviewtemp.review = item.Review1;
                    reviewtemp.rate = item.Rate;
                    reviewtemp.User_idUser = item.UserIdUser;
                    reviewtemp.Recipe_idRecipe = item.RecipeIdRecipe;
                    reviewList.Add(reviewtemp);
                }
            }
            catch (Exception e)
            {
                code = 500;
            }
            return (code, reviewList);
        }

        public int setReview(ReviewDomain review)
        {
            int code = 500;
            try
            {
                Review reviewTemp = new Review();
                reviewTemp.IdReview = GenerateRandomID.GenerateID();
                reviewTemp.Review1 = review.review;
                reviewTemp.Rate = review.rate;
                reviewTemp.UserIdUser = review.User_idUser;
                reviewTemp.RecipeIdRecipe = review.Recipe_idRecipe;
                connectionModel.Reviews.Add(reviewTemp);
                int change = connectionModel.SaveChanges();
                if (change == 1)
                {
                    code = 200;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                code = 500;
            }
            return code;
        }

        public int EditReview(ReviewDomain review)
        {
            int code = 500;
            try
            {
                Review reviewTemp = connectionModel.Reviews.Find(review.idReview);
                if (reviewTemp != null)
                {
                    reviewTemp.Review1 = review.review;
                    reviewTemp.Rate = review.rate;
                    int change = connectionModel.SaveChanges();
                    if (change == 1)
                    {
                        code = 200;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                code = 500;
            }
            return code;
        }

    }
}
