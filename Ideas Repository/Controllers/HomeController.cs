using Ideas_Repository.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using Ideas_Repository.BusinessLogic;
using Ideas_Repository.Abstract;
using Ninject.Modules;
using Ninject;

namespace Ideas_Repository.Controllers
{
    public class HomeController : Controller
    {
        private readonly int pageSize = 4;
        private readonly IIdeasRepositoryBusinessLogic dataManager;
        public HomeController(IIdeasRepositoryBusinessLogic _dataManager)
        {
            dataManager = _dataManager;
        }
        public ActionResult Index()
        {
            return View(GetGridModelByPageNumber(1));
        }
        private GridModel GetGridModelByPageNumber(int pageNumber)
        {
            return dataManager.GetPagedIdeas(pageNumber, pageSize);
        }
        public ActionResult UpdatePageNumber(int pageNumber)
        {
            return PartialView("_GeneralGrid", GetGridModelByPageNumber(pageNumber));
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AdminPanel()
        {
            return View(dataManager.GetAllIdeas());
        }
        public ActionResult Edit(int id)
        {
            var bulletinboarditem = dataManager.FindIdeaById(id);
            if (bulletinboarditem == null)
            {
                return HttpNotFound();
            }
            return View(bulletinboarditem);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(BulletinBoardItem bulletinboarditem)
        {
            if (ModelState.IsValid)
            {
                bulletinboarditem.UserId = WebSecurity.GetUserId(User.Identity.Name);
                bulletinboarditem.UserName = Membership.GetUser().UserName;
                bulletinboarditem.DateOfCreateItem = DateTime.Now;
                dataManager.EditIdea(bulletinboarditem);

                return RedirectToAction("AdminPanel");
            }
            return View(bulletinboarditem);
        }

        public ActionResult Delete(int id)
        {
            var bulletinboarditem = dataManager.FindIdeaById(id);
            if (bulletinboarditem == null)
            {
                return HttpNotFound();
            }
            return View(bulletinboarditem);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (dataManager.FindIdeaById(id).RemovedByUser)
            {
                dataManager.DeleteIdea(id);
                return RedirectToAction("AdminPanel");
            }
            if (dataManager.FindIdeaById(id).UserId == (int)WebSecurity.CurrentUserId)
            {
                dataManager.RemoveByUser(id);
                return RedirectToAction("AdminPanel");
            }

            dataManager.RemoveByAdmin(id);
            return RedirectToAction("AdminPanel");
        }

        public ActionResult InfoConfirmDeletion()
        {
            if (dataManager.HasIdeasInTrash())
            {
                return View(dataManager.GetIdeasInTrash());
            }
            return RedirectToAction("NoConfirmDelete");
        }
        public ActionResult Restore(int id)
        {
            if (dataManager.FindIdeaById(id) == null)
            {
                return HttpNotFound();
            }
            return View(dataManager.FindIdeaById(id));
        }
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public ActionResult RestoreConfirm(int id)
        {
            dataManager.RestoreIdea(id);
            return RedirectToAction("InfoConfirmDeletion");
        }
        public ActionResult NoConfirmDelete()
        {
            return View();
        }
    }
}
