using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoursesStore.Models
{
    public class Course
    {
        public int Id { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }

        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Range(1, 1000)]
        public int EffectCount { get; set; }

        //Зображення яке знаходиться на головній сторінці
        [Required]
        [NotMapped]
        public IFormFile? PreviewImage { get; set; }

        //Відео на верху сторінки проекту
        [Required]
        [NotMapped]
        public IFormFile? MainVideo { get; set; }

        //Анімований постер проекту, той що знаходться поряд з кнопкою купити
        [Required]
        [NotMapped]
        public IFormFile? CardVideo { get; set; }

        //Зображення для прикладу (те над яким надпис Before)
        [Required]
        [NotMapped]
        public IFormFile? BeforeExampleImage { get; set; }

        //Зображення для прикладу (те над яким надпис After)
        [Required]
        [NotMapped]
        public IFormFile? AfterExampleImage { get; set; }
    }
}
