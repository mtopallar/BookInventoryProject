using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Core.Extensions
{
    public static class ClaimsPrincipalExtensions // JWT yani kullanıcının API'ye gönderdiği token içinde gelen claimleri okumak için claimsprincipal(claimlere erişmek için .net in kendi class ı) extend ettik. ilk metod claim type ı parametre olarak alır ve sonucu claim type a göre döndürürken ikinci metodda direk claimsPrincipal.ClaimRoles() ile direk rolleri dönüyoruz.
    {
        public static List<string> Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            // mesela claim type olarak roles diyoruz ve roles claimlerini List<string> olarak dönüyor.
            //** Burada erişebileceklerimiz sadece role ile sınırlı değil mesela olur da gerekirse claim in id sine, dğier claimlere vs erişmek de bu yöntemle mümkün.
            var result = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value).ToList();
            return result;
        }

        public static List<string> ClaimRoles(this ClaimsPrincipal claimsPrincipal)
        {
            // yukarıdaki metod yardımıyla claimtype ı roles olan claimleri direk dönüyor.
            return claimsPrincipal?.Claims(ClaimTypes.Role);
        }
    }
}
