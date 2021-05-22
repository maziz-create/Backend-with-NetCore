using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.Encryption
{
    public class SigningCredentialsHelper
    {
        public static SigningCredentials CreateSigningCredentials(SecurityKey securityKey)
            //bu class hashing yapıyor. parolayı hashlıyor yani. passwordHash buradan çıkıyor.
        {
            // hashing yapacan ya hani. anahtar olarak securityKey ' i kullan, anahtar olarak da algoritmalar içerisinden HmacSha512 ' yi kullan. Bu algoritma şifrelerimizi hashleyecek algoritma idi. Hashing/HashingHelper.cs classında belirtmiştik.
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
            //şifre doğrulama mevzuları sadece kullanıcılar için değil asp.net'in kendisinin de ihtiyacı olan bir şey. apinin de ihtiyacı var yani. o yüzden böyle bir class'a ihtiyacımız var.

            //not: hmaclerin sayısı güncelliği ve hashleme gücü ile doğru orantılı

            //not: buradaki doğrulama mevzusu yönetici için aslında. zaten belirtmiştim aspnetin kendisi için diye. sen böyle bi sistem yöneteceksin, şifreni de yazmıştın zaten, şimdi doğrula o şifreyi.

            //not: credential nedir ? bir sisteme giriş yaparken kullandığın kullanıcı adın ve şifren yani giriş bilgilerinin hepsi senin credentialların oluyor.
        }
    }
}
