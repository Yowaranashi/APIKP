using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HranitelProAPi.Controllers
{
    public class ImportController : Controller
    {
        // GET: ImportController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ImportController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ImportController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ImportController/Create
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

        // GET: ImportController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ImportController/Edit/5
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

        // GET: ImportController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ImportController/Delete/5
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
