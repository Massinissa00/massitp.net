using System;

namespace mvc.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }  // Propriété Title
        public string Description { get; set; }  // Propriété Description
        public string Location { get; set; }  // Propriété Location
        public DateTime EventDate { get; set; }  // Propriété Date
    }
}
