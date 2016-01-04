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
    [RoutePrefix("api/WordsData")]
    public class WordsDataController : ApiController
    {
        private AnnotatorModel db = new AnnotatorModel();

        // GET: api/WordsData/RandomWords
        [Route("WordsFromTweet/{id}")]
        [EnableQuery]
        public IQueryable<Word> GetRandomWords(int id)
        {
            //first lets grab a tweet
            var Tweet = (from t in db.Tweets
                         where t.Id == id
                         select t).FirstOrDefault();

            //break it down into an array
            string[] tweetWords = Tweet.Text.ToLower().Split(' ');

            //find only words in that tweet
            var words = (from w in db.Words
                         where tweetWords.Contains(w.Text.ToLower())
                         select w);

            //De Dupe
            var mywords = words
                .GroupBy(word => word.Text)
                .Select(group => group.FirstOrDefault());
                           
            return mywords;
        }

        // GET: api/WordsData
        [EnableQuery]
        public IQueryable<Word> GetWords()
        {
            return db.Words;
        }

        // GET: api/WordsData/5
        [ResponseType(typeof(Word))]
        public async Task<IHttpActionResult> GetWord(int id)
        {
            Word word = await db.Words.FindAsync(id);
            if (word == null)
            {
                return NotFound();
            }

            return Ok(word);
        }

        // PUT: api/WordsData/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutWord(int id, Word word)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != word.Id)
            {
                return BadRequest();
            }

            db.Entry(word).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WordExists(id))
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

        // POST: api/WordsData
        [ResponseType(typeof(Word))]
        public async Task<IHttpActionResult> PostWord(Word word)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Words.Add(word);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = word.Id }, word);
        }

        // DELETE: api/WordsData/5
        [ResponseType(typeof(Word))]
        public async Task<IHttpActionResult> DeleteWord(int id)
        {
            Word word = await db.Words.FindAsync(id);
            if (word == null)
            {
                return NotFound();
            }

            db.Words.Remove(word);
            await db.SaveChangesAsync();

            return Ok(word);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WordExists(int id)
        {
            return db.Words.Count(e => e.Id == id) > 0;
        }
    }
}