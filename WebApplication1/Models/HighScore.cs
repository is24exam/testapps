using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class HighScore
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Please enter Name")]
        [MaxLength(100,ErrorMessage ="MaxLength is 100")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Score")]
        public int Score { get; set; }
    }
}