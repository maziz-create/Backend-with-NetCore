using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.JWT
{
    public interface ITokenHelper   //neden interface? Çünkü biz şu an token üretimini JWT teknolojisi ile yapıyoruz. Yarın bir gün başka bir teknoloji ile de yapabiliriz. 
    {
        AccessToken CreateToken(User user, List<OperationClaim> operationClaims);//bu bizim token üretecek mekanizmamızın ta kendisi.
        //user = bu tokeni kim kullanacak?
        //list<OperationClaim> = bu usere vereceğimiz tokenin içine hangi yetkiler konsun? 
        //Sistem nasıl çalışıyor? siteye girip kullanıcı adı şifre girdikten sonra verilen istek apimize ulaşıyor. isteğin içinde pakete dahil olan token bilgisi doğru ise eğer apimiz bu kullanıcıya ve onun operation claimslerine yani ona verilen yetkilere bakacak. sonra bunlara uygun olaraktan yani o kişinin her şeyini içeren bir json web token yani JWT üretecek ve o kişiye gönderecek. O kişi de bu sayede sisteme giriş yapacak.

        //yani aga sen bize kullanıcıyı ve onun yetkilerini ver biz de sana token ile onun geçerlilik süresini verelim. seal the deal.
    }
}
