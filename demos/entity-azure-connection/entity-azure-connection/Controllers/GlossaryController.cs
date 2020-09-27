using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace entity_azure_connection.Controllers
{
    namespace Glossary.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class GlossaryController : ControllerBase
        {
            private static List<GlossaryItem> Glossary = new List<GlossaryItem> {
            new GlossaryItem
            {
                Term= "Access Token",
                Definition = "A credential that can be used by an application to access an API. It informs the API that the bearer of the token has been authorized to access the API and perform specific actions specified by the scope that has been granted."
            },
            new GlossaryItem
            {
                Term= "JWT",
                Definition = "An open, industry standard RFC 7519 method for representing claims securely between two parties. "
            },
            new GlossaryItem
            {
                Term= "OpenID",
                Definition = "An open standard for authentication that allows applications to verify users are who they say they are without needing to collect, store, and therefore become liable for a user’s login information."
            }
        };


            BloggingContext _context;
            private readonly ILogger<WeatherForecastController> _logger;

            public GlossaryController(ILogger<WeatherForecastController> logger, BloggingContext context)
            {
                _logger = logger;
                _context = context;
            }

            [HttpGet]
            public ActionResult<List<GlossaryItem>> Get()
            {
                //string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                //using(_context)
                //{
                //    var user = await _context.Blogs.Where(x => x.UserId == userId).FirstOrDefaultAsync();
                //    if(user == null)
                //    {
                //        user = new Blog()
                //        {
                //            Urls = "Created a new user since last one was shit lol",
                //            UserId = userId,
                //        };
                //        _context.Add<Blog>(user);
                //        await _context.SaveChangesAsync();
                //    }

                //    return Ok(user);
                //}
                var list = new List<GlossaryItem>() { new GlossaryItem() { Term = "This is a term", Definition = "This is a definition" }, new GlossaryItem() { Term = "This is a 2", Definition = "This is a 2" } };

                return Ok(list);
            }


            [HttpPost]
            [Authorize]
            public ActionResult Post(GlossaryItem glossaryItem)
            {
                var existingGlossaryItem = Glossary.Find(item =>
                        item.Term.Equals(glossaryItem.Term, StringComparison.InvariantCultureIgnoreCase));

                if (existingGlossaryItem != null)
                {
                    return Conflict("Cannot create the term because it already exists.");
                }
                else
                {
                    Glossary.Add(glossaryItem);
                    var resourceUrl = Path.Combine(Request.Path.ToString(), Uri.EscapeUriString(glossaryItem.Term));
                    return Created(resourceUrl, glossaryItem);
                }
            }


            [HttpDelete]
            [Route("{term}")]
            [Authorize]
            public ActionResult Delete(string term)
            {
                var glossaryItem = Glossary.Find(item =>
                       item.Term.Equals(term, StringComparison.InvariantCultureIgnoreCase));

                if (glossaryItem == null)
                {
                    return NotFound();
                }
                else
                {
                    Glossary.Remove(glossaryItem);
                    return NoContent();
                }
            }
        }
    }
}
