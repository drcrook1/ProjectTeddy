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
    public class ConversationsDataController : ApiController
    {
        private AnnotatorModel db = new AnnotatorModel();

        // GET: api/ConversationsData
        [EnableQuery]
        public IQueryable<Conversation> GetConversations()
        {
            return db.Conversations;
        }

        // GET: api/ConversationsData/5
        [ResponseType(typeof(Conversation))]
        public async Task<IHttpActionResult> GetConversation(int id)
        {
            Conversation conversation = await db.Conversations.FindAsync(id);
            if (conversation == null)
            {
                return NotFound();
            }

            return Ok(conversation);
        }

        // PUT: api/ConversationsData/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutConversation(int id, Conversation conversation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != conversation.Id)
            {
                return BadRequest();
            }

            db.Entry(conversation).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConversationExists(id))
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

        // POST: api/ConversationsData
        [ResponseType(typeof(Conversation))]
        public async Task<IHttpActionResult> PostConversation(Conversation conversation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Conversations.Add(conversation);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = conversation.Id }, conversation);
        }

        // DELETE: api/ConversationsData/5
        [ResponseType(typeof(Conversation))]
        public async Task<IHttpActionResult> DeleteConversation(int id)
        {
            Conversation conversation = await db.Conversations.FindAsync(id);
            if (conversation == null)
            {
                return NotFound();
            }

            db.Conversations.Remove(conversation);
            await db.SaveChangesAsync();

            return Ok(conversation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ConversationExists(int id)
        {
            return db.Conversations.Count(e => e.Id == id) > 0;
        }
    }
}