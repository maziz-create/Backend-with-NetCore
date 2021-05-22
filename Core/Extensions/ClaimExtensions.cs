using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Core.Extensions
{
    public static class ClaimExtensions
        //extension = bir şeyi genişletmek.
        //yazılımda extension = bir class'a yeni propertyler eklemek. önceden var olmayan propertyleri sonradan eklemek.
        //claims = roller idi, biz rol olarak kişinin adını mail adresini ve rollerini paket olarak veriyoruz. ha bi de nameidentifier var o neyse artık :D
    {
        public static void AddEmail(this ICollection<System.Security.Claims.Claim> claims, string email)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));
        }

        public static void AddName(this ICollection<Claim> claims, string name)
        {
            claims.Add(new Claim(ClaimTypes.Name, name));
        }

        public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier)
        {   //bu class ne işe yarıyor bilmiyorum.
            // Id kısmıymış.
            claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));
        }

        public static void AddRoles(this ICollection<Claim> claims, string[] roles)
        {
            roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
        }
    }
}
