using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ProjectTeddy.Core.EntityFramework;
using System.Web.OData;

namespace ProjectTeddy.Annotator.Controllers
{
    public class AnnotatedTweetsDataController : ApiController
    {
        private AnnotatorModel db = new AnnotatorModel();

        // GET: api/AnnotatedTweetsData
        [EnableQuery]
        public IQueryable<AnnotatedTweet> GetAnnotatedTweets()
        {
            return db.AnnotatedTweets;
        }

        // GET: api/AnnotatedTweetsData/5
        [ResponseType(typeof(AnnotatedTweet))]
        public async Task<IHttpActionResult> GetAnnotatedTweet(int id)
        {
            AnnotatedTweet annotatedTweet = await db.AnnotatedTweets.FindAsync(id);
            if (annotatedTweet == null)
            {
                return NotFound();
            }

            return Ok(annotatedTweet);
        }

        // PUT: api/AnnotatedTweetsData/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAnnotatedTweet(int id, AnnotatedTweet annotatedTweet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != annotatedTweet.Id)
            {
                return BadRequest();
            }

            db.Entry(annotatedTweet).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnnotatedTweetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/AnnotatedTweetsData
        [ResponseType(typeof(AnnotatedTweet))]
        public async Task<IHttpActionResult> PostAnnotatedTweet(AnnotatedTweet annotatedTweet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AnnotatedTweets.Add(annotatedTweet);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException myexcep)
            {
                if (AnnotatedTweetExists(annotatedTweet.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw myexcep;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = annotatedTweet.Id }, annotatedTweet);
        }

        // DELETE: api/AnnotatedTweetsData/5
        [ResponseType(typeof(AnnotatedTweet))]
        public async Task<IHttpActionResult> DeleteAnnotatedTweet(int id)
        {
            AnnotatedTweet annotatedTweet = await db.AnnotatedTweets.FindAsync(id);
            if (annotatedTweet == null)
            {
                return NotFound();
            }

            db.AnnotatedTweets.Remove(annotatedTweet);
            await db.SaveChangesAsync();

            return Ok(annotatedTweet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AnnotatedTweetExists(int id)
        {
            return db.AnnotatedTweets.Count(e => e.Id == id) > 0;
        }
    }
}