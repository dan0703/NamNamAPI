using Microsoft.AspNetCore.Mvc;
using NamNamAPI.Business;
using NamNamAPI.Domain;
using NamNamAPI.Models;
using NamNamAPI.Utility;
using System.Collections.Generic;

namespace NamNamAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : ControllerBase
    {
        private ReviewProvider reviewProvider;
      
        public ReviewController([FromBody] ReviewProvider _reviewProvider)
        {
            reviewProvider = _reviewProvider;
            
        }



        [HttpGet("Getreview/{idRecipe}")]
        public ActionResult GetReviews(string idRecipe)
        {
           (int code , List<ReviewDomain> reviews) = reviewProvider.getReviews(idRecipe);
           if(code == 200)
           {
            return Ok(reviews);
           }
            return StatusCode(code);

        }

         [HttpPost("setReview")]
        public ActionResult SetReview([FromBody]ReviewDomain review)
        {
           int code  = reviewProvider.setReview(review);
           if(code == 200)
           {
            return Ok();
           }
            return StatusCode(code);

        }

        

    }

}
