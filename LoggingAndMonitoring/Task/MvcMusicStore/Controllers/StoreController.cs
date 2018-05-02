using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcMusicStore.Models;
using PerformanceCounterHelper;
using PerformanceCounters;
using NLog;
using System;

namespace MvcMusicStore.Controllers
{
    public class StoreController : Controller
    {
        public readonly ILogger logger;
        private readonly MusicStoreEntities _storeContext = new MusicStoreEntities();

        public StoreController()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        // GET: /Store/
        public async Task<ActionResult> Index()
        {
            return View(await _storeContext.Genres.ToListAsync());
        }

        // GET: /Store/Browse?genre=Disco
        public async Task<ActionResult> Browse(string genre)
        {
            if (genre == "Metal")
            {
                var counterHelper = CounterHelperManager.GetHelper();
                counterHelper.Increment(Counters.MetalSelect);
            }

            if (genre == "Rock")
            {
                try
                {
                    throw new System.Exception("Rock genre is broken");
                }
                catch(Exception ex)
                {
                    logger.Error(@"StoreController\Browse - " + ex.Message);
                }
            }

            return View(await _storeContext.Genres.Include("Albums").SingleAsync(g => g.Name == genre));
        }

        public async Task<ActionResult> Details(int id)
        {
            var album = await _storeContext.Albums.FindAsync(id);

            return album != null ? View(album) : (ActionResult)HttpNotFound();
        }

        [ChildActionOnly]
        public ActionResult GenreMenu()
        {
            return PartialView(
                _storeContext.Genres.OrderByDescending(
                    g => g.Albums.Sum(a => a.OrderDetails.Sum(od => od.Quantity))).Take(9).ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _storeContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}