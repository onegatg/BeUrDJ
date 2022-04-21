using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeUrDJ_MVC.Models
{
    public class CreateSessionViewModel
    {
        [Required]
        [Display(Name = "Session Name")]
        [DataType(DataType.Text)]
        public string SessionName { get; set; }
        [Required]
        [Display(Name = "Session Description")]
        [DataType(DataType.Text)]
        public string SessionDescription { get; set; }
        [Required]
        [Display(Name = "DJ Name")]
        [DataType(DataType.Text)]
        public string DJName { get; set; }
        [Required]
        [Display(Name = "DJ Description")]
        [DataType(DataType.Text)]
        public string DJDescription { get; set; }
       
    }
}
