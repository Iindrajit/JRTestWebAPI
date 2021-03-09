using System.Collections.Generic;

namespace TestApp.Models
{
    public class ListingInfo
    {
        public string From { get; set; }
        public string To { get; set; }
        public List<Listing> Listings { get; set; }
    }
}
