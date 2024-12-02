using System.ComponentModel.DataAnnotations;

namespace API127.Models.Dto
{
    public class VillaCreateDTO
    {
        [MinLength(3)]
        public string? Name { get; set; } // swagger : name
        public int MyProperty { get; set; } // swagger : myProperty
        public int Sqft { get;set;}
        public int Occupancy { get;set;}
        public string? Hello_Hi { get; set; }
        public string? Details { get; set; }
        public double Rate { get; set; }
        public string? ImageUrl { get; set; }
        public string? Amenity { get; set; }
        public bool Deleted { get;set;}
    }
    public class VillaCreateAPIDTO1
    {
        public string? ImageUrl { get; set; }
        public IFormFile? Files { get; set; }
    }
    public class VillaCreateAPIDTO2
    {
        public string? ImageUrl { get; set; }
    }
}
