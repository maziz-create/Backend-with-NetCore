using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.Encryption
{
    
    //Encryption = Kriptografi
    public class SecurityKeyHelper
    {
        //bu classın derdi string olarak verilen securitykeyleri byte[] (byte array) haline getirmek. çünkü asp.nette yer alan jason web tokenlar bytearray olarak istiyorlar her şeyi.
        //--API/appsettings.json/tokenoptions/securitykey kısmı.
        public static SecurityKey CreateSecurityKey(string securityKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)); // bizim bir anahtara ihtiyacımız var. bu anahtarlar simetrik ve asimetrik olarak ayrılıyor.

            //elimizde uyduruk olarak mysupersecretkeymysupersecretkey vardı ya hani. bunu işte JWT servislerine parametre olarak geçemiyorsun. çünkü bu bir string ve o sistem senden byte[] yani byte array yani byte dizisi istiyor. yazılan kod da byte array halinde simetrik security key yapıyor.
        }
    }
}
