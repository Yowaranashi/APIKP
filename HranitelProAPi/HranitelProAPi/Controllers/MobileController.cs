using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HranitelProAPi.Controllers
{
    public class MobileController : Controller
    {
        // GET: MobileController
        public ActionResult Index()
        {
            return View();
        }

        // GET: MobileController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MobileController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MobileController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MobileController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MobileController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MobileController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MobileController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
