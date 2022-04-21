using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeUrDJ_MVC.Models.ViewModels
{
    public class UpdateQueueViewModel
    {
        public List<QueueDTO> QueueDTOs { get; set; }
        public ApplicationUser User { get; set; }
    }
}
