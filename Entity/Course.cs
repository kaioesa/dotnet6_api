using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webapi_dotnet6.Entity
{
    public class Course
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do curso é obrigatório")]
        [StringLength(20, ErrorMessage = "O nome do curso não pode exceder 20 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "A duração do curso é obrigatória")]
        [Range(
            2400,
            7200,
            ErrorMessage = "A duração de graduação deve ter entre 2400 a 7200 horas"
        )]
        public int Duration { get; set; }

        [Required(ErrorMessage = "O tipo de graduação é obrigatório")]
        [MaxLength(20, ErrorMessage = "O tipo de graduação não pode exceder 20 caracteres")]
        public string GraduationType { get; set; }

        [Required(ErrorMessage = "A modalidade de graduação é obrigatória")]
        [MaxLength(20, ErrorMessage = "A modelidade de graduação não pode exceder 20 caracteres")]
        public string GraduationStyle { get; set; }
    }
}
