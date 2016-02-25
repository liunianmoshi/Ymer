using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Ymer.Caching;

namespace Ymer.Samples.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private ICacheManager cacheManager;

        private IClock clock;

        private ISignals signals;

        private ITagSignals tagSignals;

        private string key = "gewgwg32w";

        private int count = 0;

        public HomeController(ICacheManager cacheManager, IClock clock, ISignals signals, ITagSignals tagSignals)
        {
            this.cacheManager = cacheManager;

            this.clock = clock;
            this.signals = signals;
            this.tagSignals = tagSignals;
        }

        // GET: Home
        public ActionResult Index()
        {
            var a = cacheManager.Get("xxx", c =>
            {

                return "xxx";
            });

            var b = cacheManager.Get("xxx", c =>
            {
                return 1;
            });



            return View();
        }
    }
}