using Microsoft.AspNetCore.Mvc;
using LibraryApi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class DemoController : Controller
    {
        IGenerateIds idGenerator;

        public DemoController(IGenerateIds idGenerator)
        {
            this.idGenerator = idGenerator;
        }

        [HttpGet("/beerme/{qty:int}")]
        public IActionResult GetBeer(int qty)
        {
            return Ok($"giving you {qty} beers");
        }

        [HttpGet("blogs/{year:int:min(2015)}/{month:int:range(1,12)}/{day:int:range(1,31)}")]
        public IActionResult GetPostsFor(int year, int month, int day)
        {
            return Ok($"Getting Blof Posts for {year}/{month}/{day}");
        }

        [HttpGet("/magazines")]
        public IActionResult GetMagazines([FromQuery] string topic)
        {
            return Ok($"Giving your Magazines for {topic}!");
        }

        [HttpGet("/whoami")]
        public IActionResult WhoAmI([FromHeader(Name = "User-Agent")] string ua)
        {
            return Ok($"I see you are running {ua}");
        }

        [HttpPost("/courseenrollments")]
        public IActionResult EnrollForCourse([FromBody] EnrollmentRequest enrollment)
        {
            var response = new EnrollmentResponse
            {
                CourseId = enrollment.CourseId,
                Instructor = enrollment.Instructor,
                EnrollmentId = Guid.NewGuid()
            };
            return Ok(response);
        }
    }



    public class EnrollmentRequest
    {
        public string CourseId { get; set; }
        public string Instructor { get; set; }

        public Guid EnrollemntId { get; set; }
    }

    public class EnrollmentResponse
    {
        public string CourseId { get; set; }
        public string Instructor { get; set; }

        public Guid EnrollmentId { get; set; }
    }

}
