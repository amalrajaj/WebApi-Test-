using System.ComponentModel.DataAnnotations;

namespace BOL.ModelDtos
{
    public class StudentDto
    {
        public int StudentId { get; set; }
        [Required]
        [MaxLength(30)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", 
        ErrorMessage = "Characters are not allowed.")]
        public string StudentName { get; set; }
        public System.DateTime Dob { get; set; }
        [Required]
        public string Email { get; set; }
        [Range(1,7,ErrorMessage = "Class mus be  1 to 7")]
        public int Class { get; set; }
    }
}
