using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ProjectTeddy.Analytics;

namespace ProjectTeddy.Annotator.Controllers
{
    struct VizziniResponse
    {
        string Response;
        string SentenceType;
        int BearId;
    }
    public class VizziniController : ApiController
    {
        // POST: api/Vizzini
        [ResponseType(typeof(string))]
        public async Task<IHttpActionResult> GetResponse(string tweet)
        {
            var s = await Task.Run(() =>
            {
                return ResponseEngine.GetBestResponse(tweet);
            });
            return Ok(s);
        }
    }
}
