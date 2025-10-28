using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HranitelProAPi.Controllers
{
    public class PassRequestController : Controller
    {
        // GET: PassRequestController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PassRequestController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PassRequestController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PassRequestController/Create
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

        // GET: PassRequestController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PassRequestController/Edit/5
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

        // GET: PassRequestController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PassRequestController/Delete/5
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
