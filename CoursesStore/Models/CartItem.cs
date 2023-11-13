using System.ComponentModel.DataAnnotations;

namespace CoursesStore.Models
{
    public class CartItem
    {
        [Key]
        public string ItemId { get; set; }

        public string UserId { get; set; }

        public int Quantity { get; set; }

        public System.DateTime DateCreated { get; set; }

        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

    }
}
