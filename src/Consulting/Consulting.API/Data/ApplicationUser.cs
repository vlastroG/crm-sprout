using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

namespace Consulting.API.Data {
    public class ApplicationUser : IdentityUser {
        [EmailAddress]
        public override string? Email { get => base.Email; set => base.Email = value; }
    }
}
