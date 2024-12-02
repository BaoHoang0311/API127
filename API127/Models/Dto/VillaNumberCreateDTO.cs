using System.ComponentModel.DataAnnotations;

namespace API127.Models.Dto
{
    public class VillaNumberCreateDTO
    {
        [Required]
        public int VillaNo { get; set; }
        [Required]
        public int VillaID { get; set; }
        public string SpecialDetails { get; set; }
        public string SpecialDetails2 { get; set; }
    }
}
