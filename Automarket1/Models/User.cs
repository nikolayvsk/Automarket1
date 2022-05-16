using Microsoft.AspNetCore.Identity;

namespace Automarket1
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}
