using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduzeniethFinal.Models
{
    public class Announcement
    {
        public int PID {  get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string PostedBy { get; set; }
        public DateTime PostedDate { get; set; }
        public List<CommentModel> Comments { get; set; }
    }
    public class Admin
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    public class CommentModel
    {
        public string CommentContent { get; set; }
        public string CommentedBy { get; set; }
    }
}