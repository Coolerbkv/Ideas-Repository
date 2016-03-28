using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ideas_Repository.Models
{
    public class GridModel
    {
         public IEnumerable<BulletinBoardItem> BulletinBoardList { get; set; }
         public int PagesNumber { get; set; }
         public int CurrentPageNumber { get; set; }    
    }
}