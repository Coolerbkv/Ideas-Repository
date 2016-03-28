using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Ideas_Repository.Abstract;
namespace Ideas_Repository.Models
{
    public class BulletinBoardItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [UIHint("MultilineText")]
        public string Message { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime DateOfCreateItem { get; set; }
        public bool RemovedByAdmin { get; set; }
        public bool RemovedByUser { get; set; }
    }
}