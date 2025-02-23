using System.ComponentModel.DataAnnotations;

namespace People2.Models
{
    public class PersonMod
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FName { get; set; }
        [Required]
        public string LName { get; set; }
        [Required]
        public string IdNum { get; set; }
    }
}
