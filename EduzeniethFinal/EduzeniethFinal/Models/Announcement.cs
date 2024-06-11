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
        public List<string> Comments { get; set; }
    }
}