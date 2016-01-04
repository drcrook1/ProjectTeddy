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
    public class AnnotatedConversationsDataController : ApiController
    {
        private AnnotatorModel db = new AnnotatorModel();

        // GET: api/AnnotatedConversationsData
        [EnableQuery]
        public IQueryable<AnnotatedConversation> GetAnnotatedConversations()
        {
            return db.AnnotatedConversations;
        }

        // GET: api/AnnotatedConversationsData/5
        [ResponseType(typeof(AnnotatedConversation))]
        public async Task<IHttpActionResult> GetAnnotatedConversation(int id)
        {
            AnnotatedConversation annotatedConversation = await db.AnnotatedConversations.FindAsync(id);
            if (annotatedConversation == null)
            {
                return NotFound();
            }

            return Ok(annotatedConversation);
        }

        // PUT: api/AnnotatedConversationsData/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAnnotatedConversation(int id, AnnotatedConversation annotatedConversation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != annotatedConversation.Id)
            {
                return BadRequest();
            }

            db.Entry(annotatedConversation).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnnotatedConversationExists(id))
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

        // POST: api/AnnotatedConversationsData
        [ResponseType(typeof(AnnotatedConversation))]
        public async Task<IHttpActionResult> PostAnnotatedConversation(AnnotatedConversation annotatedConversation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AnnotatedConversations.Add(annotatedConversation);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AnnotatedConversationExists(annotatedConversation.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = annotatedConversation.Id }, annotatedConversation);
        }

        // DELETE: api/AnnotatedConversationsData/5
        [ResponseType(typeof(AnnotatedConversation))]
        public async Task<IHttpActionResult> DeleteAnnotatedConversation(int id)
        {
            AnnotatedConversation annotatedConversation = await db.AnnotatedConversations.FindAsync(id);
            if (annotatedConversation == null)
            {
                return NotFound();
            }

            db.AnnotatedConversations.Remove(annotatedConversation);
            await db.SaveChangesAsync();

            return Ok(annotatedConversation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AnnotatedConversationExists(int id)
        {
            return db.AnnotatedConversations.Count(e => e.Id == id) > 0;
        }
    }
}