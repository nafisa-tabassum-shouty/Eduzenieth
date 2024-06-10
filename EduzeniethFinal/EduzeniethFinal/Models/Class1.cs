using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EduzeniethFinal.Models
{
    public class Class1
    {
        [Required]
        public HttpPostedFileBase File { get; set; }
        // Property to store the file path
        public string FilePath { get; set; }
        public string Posts { get; set; }
    }
}