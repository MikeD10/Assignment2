using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    public class AnnouncementsDetailsViewModel
    {
        public Announcements Announcements { get; set; }
        public List<Comments> Comments { get; set; }
        public int AnnouncementsId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
