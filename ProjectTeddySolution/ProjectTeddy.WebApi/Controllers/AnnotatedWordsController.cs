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
using ProjectTeddy.WebApi.EntityFramework;

namespace ProjectTeddy.WebApi.Controllers
{
    public class AnnotatedWordsController : ApiController
    {
        private AnnotatorModel db = new AnnotatorModel();

        // GET: api/AnnotatedWords
        public IQueryable<AnnotatedWord> GetAnnotatedWords()
        {
            return db.AnnotatedWords;
        }

        // GET: api/AnnotatedWords/5
        [ResponseType(typeof(AnnotatedWord))]
        public async Task<IHttpActionResult> GetAnnotatedWord(int id)
        {
            AnnotatedWord annotatedWord = await db.AnnotatedWords.FindAsync(id);
            if (annotatedWord == null)
            {
                return NotFound();
            }

            return Ok(annotatedWord);
        }

        // PUT: api/AnnotatedWords/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAnnotatedWord(int id, AnnotatedWord annotatedWord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != annotatedWord.Id)
            {
                return BadRequest();
            }

            db.Entry(annotatedWord).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnnotatedWordExists(id))
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

        // POST: api/AnnotatedWords
        [ResponseType(typeof(AnnotatedWord))]
        public async Task<IHttpActionResult> PostAnnotatedWord(AnnotatedWord annotatedWord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AnnotatedWords.Add(annotatedWord);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = annotatedWord.Id }, annotatedWord);
        }

        // DELETE: api/AnnotatedWords/5
        [ResponseType(typeof(AnnotatedWord))]
        public async Task<IHttpActionResult> DeleteAnnotatedWord(int id)
        {
            AnnotatedWord annotatedWord = await db.AnnotatedWords.FindAsync(id);
            if (annotatedWord == null)
            {
                return NotFound();
            }

            db.AnnotatedWords.Remove(annotatedWord);
            await db.SaveChangesAsync();

            return Ok(annotatedWord);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AnnotatedWordExists(int id)
        {
            return db.AnnotatedWords.Count(e => e.Id == id) > 0;
        }
    }
}