using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ideas_Repository.Models;

namespace Ideas_Repository.Abstract
{
    public interface IIdeasRepositoryBusinessLogic
    {
        GridModel GetPagedIdeas(int pageNumber, int pageSize);
        IEnumerable<BulletinBoardItem> GetAllIdeas();
        BulletinBoardItem FindIdeaById(int id);
        void EditIdea(BulletinBoardItem bulletinboarditem);
        void RemoveByUser(int id);
        void RemoveByAdmin(int id);
        void DeleteIdea(int id);
        bool HasIdeasInTrash();
        IEnumerable<BulletinBoardItem> GetIdeasInTrash();
        void RestoreIdea(int id);
        IEnumerable<BulletinBoardItem> GetUserIdeas(int userId);
        void CreateIdeas(BulletinBoardItem bulletinBoardItem);
        bool HasIdeasInTrashByAdmin(int userId);
        IEnumerable<BulletinBoardItem> GetIdeasInTrashByUser(int userId);
    }
}
