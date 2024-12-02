using API127.Models;
using ERP.Infrastructure.Persistence.Interceptors;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API127.Models
{
    public class Villa : BaseAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Details { get; set; }
        public double? Rate { get; set; }
        public int? Sqft { get; set; }
        public int? Occupancy { get; set; }
        public string? ImageUrl { get; set; }
        public string? Amenitys { get; set; }
        public string? def { get;set; }
        public ICollection<VillaNumber> villaNumber { get;set;}
    }
}
