using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.JWT
{
    public class AccessToken           //property class'ı
        //kullanıcılar sistemimize istekte bulunduğunda kullanıcı bilgilerinin yanına tokenlarını da eklerler ve bu bir paket olarak bizim sistemimize gelir. biz o tokene göre tanırız o kullanıcıyı.
    {
        public string Token { get; set; } //anlamsız karakterler içeren token yani şifre. JWT ' nin tam kendisi bu. Kullanıcı api üzerinden yani postman'den kullanıcı adı ve parolasını verecek. biz de ona bir tane token verecez ve ne zaman sonlanacağının bilgisini veriyor olacağız.
        public DateTime Expiration { get; set; } // verilen tokenin geçerlilik süresi. API/appsettings.json/TokenOptions/AccessTokenExpiration altında bunu 10 olarak yani 10 dakika olarak vermiştik. Bu o işte.



        //buraya eklenecek bir başka şey : reflesh token. kullanıcının tokeni bittiği zamanki mevzular... lazım olursa araştır!
    }
}
