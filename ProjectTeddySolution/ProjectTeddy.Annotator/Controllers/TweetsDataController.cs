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
using Microsoft.AspNet.Identity;

namespace ProjectTeddy.Annotator.Controllers
{
    [RoutePrefix("api/TweetsData")]
    public class TweetsDataController : ApiController
    {
        private AnnotatorModel db = new AnnotatorModel();

        // GET: api/TweetsData/RandomTweet
        [Route("RandomTweet")]
        public async Task<Tweet> GetRandomTweet()
        {

            //Return a random tweet 
            // not attached to any user. 
            var tweet = await db.Tweets.OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();
                                               
            return tweet;

        }

        // GET: api/TweetsData/RandomTweetByUser
        [Route("RandomTweetByUser")]
        public async Task<Tweet> GetRandomTweetByUser()
        {
            String currentUserID = User.Identity.GetUserId().ToString();

            //This only returns tweets that user has NOT annotated. 

            var tweet = await (from t in db.Tweets
                          where !(from a in db.AnnotatedTweets
                                  where a.TweetId == t.Id
                                  select a.AnnotatedBy).Contains(currentUserID)
                          select t
                          ).OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();


            return tweet;
        }

        // GET: api/TweetsData
        [EnableQuery]
        public IQueryable<Tweet> GetTweets()
        {

            return db.Tweets;
            
        }

        // GET: myurl/api/TweetsData/5
        [ResponseType(typeof(Tweet))]
        public async Task<IHttpActionResult> GetTweet(int id)
        {
            Tweet tweet = await db.Tweets.FindAsync(id);
            if (tweet == null)
            {
                return NotFound();
            }

            return Ok(tweet);
        }

        // PUT: api/TweetsData/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTweet(int id, Tweet tweet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tweet.Id)
            {
                return BadRequest();
            }

            db.Entry(tweet).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TweetExists(id))
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

        // POST: api/TweetsData
        [ResponseType(typeof(Tweet))]
        public async Task<IHttpActionResult> PostTweet(Tweet tweet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tweets.Add(tweet);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tweet.Id }, tweet);
        }

        // DELETE: api/TweetsData/5
        [ResponseType(typeof(Tweet))]
        public async Task<IHttpActionResult> DeleteTweet(int id)
        {
            Tweet tweet = await db.Tweets.FindAsync(id);
            if (tweet == null)
            {
                return NotFound();
            }

            db.Tweets.Remove(tweet);
            await db.SaveChangesAsync();

            return Ok(tweet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TweetExists(int id)
        {
            return db.Tweets.Count(e => e.Id == id) > 0;
        }
    }
}