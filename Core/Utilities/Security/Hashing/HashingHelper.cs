using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.Hashing
{
    public class HashingHelper
    {
        //void döndürecek. bu da demek oluyor ki mevzuyu burada çözecek. hash ve salt passwordün out olarak yazılmasının sebebi şifrelerin burada hashlenip saltlanıp referansa kaydedilmesi yüzünden.
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
            // biz bir password verecez ve o bize passwordHash ve passwordSalt döndürecek.
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
                //hmac = kriptografi tarafında kullanacağımız klas olacak.
            {
                passwordSalt = hmac.Key; //saltlı password bizim güçlendirdiğimiz password. Burada başka şeyler de kullanılabilirmiş. hmac.Key yerine.
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); //=> yukarıda verdiğimiz saltlı passwordu key olarak kullandık ve o key ile verilen password'u hashledik. yani hashlerken her zaman elimizde bi parola(byte array) ve bir de key olmalı. key'e passwordSalt değeri atanıyor, dikkat et.
                                                                                   //hashlı password normal bir passwordun bir algoritma ile başka bir şeye döndürüldüğü halidir. 0011aziz şifresi MkKDsLf ' e dönüştürülür mesela. veri tabanında öyle saklanır.
            }
            //password = kullanıcının girdiği şifre.
            //passwordHash = belirli bir kriptografi algoritması ile şifrelenmiş şifre :)
            //passwordSalt = bizim parolayı güçlendirilmiş hale getirme işlemimiz. 
            //hashleme işlemi salt'a göre yapılıyor. 
        }

        //hash ve salt password out olarak verilmedi çünkü derdimiz değiştirmek değil. doğrulamak(verify)
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) //yukarda key değerini passwordsalt olarak verdik. burada tekrar verip o key ile giriş yaptık. aşağıda ise o keyle bize gönderilen passwordu hashlıyoruz, daha sonra bu hashlenen şey bizim yukarda hashlediğimiz passworda uyuyor mu uymuyor mu ona bakıyoruz. password karşılaştırması yani.
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i]!=passwordHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;  
        }
    }
}
