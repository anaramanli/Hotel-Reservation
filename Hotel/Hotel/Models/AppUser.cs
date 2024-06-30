using Microsoft.AspNetCore.Identity;

namespace Hotel.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? Birthdate { get; set; }

        //Relations
        public Customer? Customer { get; set; }

        public ICollection<UserReservation>? UserReservations { get; set; }

        // Relation to Comment
        public ICollection<Comment> Comments { get; set; }
    }
}
