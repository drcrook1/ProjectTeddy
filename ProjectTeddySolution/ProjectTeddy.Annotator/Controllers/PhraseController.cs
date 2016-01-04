using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectTeddy.Annotator.Controllers
{
    [Authorize]
    public class PhraseController : Controller
    {
        // GET: Phrase
        public ActionResult Index()
        {
            return View();
        }
    }
}