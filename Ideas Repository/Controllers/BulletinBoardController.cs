using Ideas_Repository.Filters;
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
using Ninject;

namespace Ideas_Repository.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class BulletinBoardController : Controller
    {
        private readonly IIdeasRepositoryBusinessLogic dataManager;
        public BulletinBoardController(IIdeasRepositoryBusinessLogic _dataManager)
        {
            dataManager = _dataManager;
        }
        public ActionResult Index()
        {
            var userId = WebSecurity.GetUserId(User.Identity.Name);
            return View(dataManager.GetUserIdeas(userId));
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(BulletinBoardItem bulletinboardItem)
        {
            if (ModelState.IsValid)
            {
                bulletinboardItem.UserId = WebSecurity.GetUserId(User.Identity.Name);
                bulletinboardItem.UserName = Membership.GetUser().UserName;
                bulletinboardItem.DateOfCreateItem = DateTime.Now;
                dataManager.CreateIdeas(bulletinboardItem);

                return RedirectToAction("Index");
            }
            return View(bulletinboardItem);
        }
        public ActionResult Edit(int id)
        {
            if (dataManager.FindIdeaById(id) == null)
            {
                return HttpNotFound();
            }
            return View(dataManager.FindIdeaById(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(BulletinBoardItem bulletinboardItem)
        {
            if (ModelState.IsValid)
            {
                bulletinboardItem.UserId = WebSecurity.GetUserId(User.Identity.Name);
                bulletinboardItem.UserName = Membership.GetUser().UserName;
                bulletinboardItem.DateOfCreateItem = DateTime.Now;
                dataManager.EditIdea(bulletinboardItem);

                return RedirectToAction("Index");
            }
            return View(bulletinboardItem);
        }
        public ActionResult Delete(int id)
        {
            if (dataManager.FindIdeaById(id) == null)
            {
                return HttpNotFound();
            }
            return View(dataManager.FindIdeaById(id));
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (dataManager.FindIdeaById(id).RemovedByAdmin)
            {
                dataManager.DeleteIdea(id);
                return RedirectToAction("InfoConfirmDeletion");
            }
            dataManager.RemoveByUser(id);
            return RedirectToAction("Index");
        }
        public ActionResult InfoConfirmDeletion()
        {
            var userId = WebSecurity.GetUserId(User.Identity.Name);

            if (dataManager.HasIdeasInTrashByAdmin(userId))
            {
                return View(dataManager.GetIdeasInTrashByUser(userId));
            }

            return RedirectToAction("NoConfirmDelete");
        }
        public ActionResult NoConfirmDelete()
        {
            return View();
        }

    }
}

