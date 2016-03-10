using System.Security.Claims;
using System.Threading.Tasks;
using Jewerly.Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Jewerly.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя

            userIdentity.AddClaim(new Claim("Currency", this.CurrencyId.ToString()));
            return userIdentity;
        }
      
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public int CountryId { get; set; }
        public int CurrencyId { get; set; }
        public string City { get; set; }
        public string KindOfActivity { get; set; }

        public virtual Country Country { get; set; }
        public virtual Currency Currency { get; set; }
    }
}