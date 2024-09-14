using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduzeniethFinal.Models
{
    public class ResultViewModel
    {
        public string Question { get; set; }
        public string SelectedAnswer { get; set; }
        public string CorrectAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }

}