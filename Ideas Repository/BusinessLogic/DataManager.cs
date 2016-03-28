using Ideas_Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ideas_Repository.Models;
using System.Data;

namespace Ideas_Repository.BusinessLogic
{
    public class DataManager : IIdeasRepositoryBusinessLogic
    {
        private UsersContext db = new UsersContext();
        public GridModel GetPagedIdeas(int pageNumber, int pageSize)
        {
            var data = db.BulletinBoardItems.Where(n => !n.RemovedByAdmin && !n.RemovedByUser).OrderBy(n => n.DateOfCreateItem).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
            var allNotesCount = db.BulletinBoardItems.Where(n => !n.RemovedByAdmin && !n.RemovedByUser).Count();

            return new GridModel()
            {
                BulletinBoardList = data,
                CurrentPageNumber = pageNumber,
                PagesNumber = (int)Math.Ceiling((double)allNotesCount / pageSize)
            };
        }
        public IEnumerable<BulletinBoardItem> GetAllIdeas()
        {
            return db.BulletinBoardItems.ToList();
        }
        public BulletinBoardItem FindIdeaById(int id)
        {
            return db.BulletinBoardItems.Find(id);
        }
        public void EditIdea(BulletinBoardItem bulletinboarditem)
        {
            db.Entry(bulletinboarditem).State = EntityState.Modified;
            db.SaveChanges();
        }
        public void RemoveByUser(int id)
        {
            FindIdeaById(id).RemovedByUser = true;
            db.SaveChanges();
        }
        public void RemoveByAdmin(int id)
        {
            FindIdeaById(id).RemovedByAdmin = true;
            db.SaveChanges();
        }
        public void DeleteIdea(int id)
        {
            db.BulletinBoardItems.Remove(FindIdeaById(id));
            db.SaveChanges();
        }
        public bool HasIdeasInTrash()
        {
            return (db.BulletinBoardItems.Where(n => n.RemovedByAdmin || n.RemovedByUser).Any());
        }
        public IEnumerable<BulletinBoardItem> GetIdeasInTrash()
        {
            return db.BulletinBoardItems.Where(n => n.RemovedByAdmin || n.RemovedByUser).ToList();
        }
        public void RestoreIdea(int id)
        {
            FindIdeaById(id).RemovedByAdmin = false;
            FindIdeaById(id).RemovedByUser = false;
            db.SaveChanges();
        }
        public IEnumerable<BulletinBoardItem> GetUserIdeas(int userId)
        {
            return db.BulletinBoardItems.Where(n => !n.RemovedByAdmin && !n.RemovedByUser && n.UserId == userId).ToList();
        }
        public void CreateIdeas(BulletinBoardItem bulletinBoardItem)
        {
            bulletinBoardItem.RemovedByAdmin = false;
            bulletinBoardItem.RemovedByUser = false;
            db.BulletinBoardItems.Add(bulletinBoardItem);
            db.SaveChanges();
        }
        public bool HasIdeasInTrashByAdmin(int userId)
        {
            return db.BulletinBoardItems.Where(n => n.RemovedByAdmin && n.UserId == userId).Any();
        }
        public IEnumerable<BulletinBoardItem> GetIdeasInTrashByUser(int userId)
        {
            return db.BulletinBoardItems.Where(n => n.RemovedByAdmin && n.UserId == userId).ToList();
        }
    }
}