using Microsoft.AspNetCore.Identity;

namespace ECommerceBE.Domain.Entities.Identity
{
    public class AppUser : IdentityUser<string>
    {
        public string NameSurname { get; set; }
    }
}
