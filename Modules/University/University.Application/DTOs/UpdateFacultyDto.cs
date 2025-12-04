using System.ComponentModel.DataAnnotations;

namespace University.Application.DTOs
{
    public class UpdateFacultyDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public string? Code { get; set; }
    }
}
