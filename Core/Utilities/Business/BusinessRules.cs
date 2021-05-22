using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Business
{
    public class BusinessRules
    {
        //Neden static kullanıyoruz? Çünkü ProductManagerda BusinessRules.Run(IResult[]); olarak kullanmak istediğimiz için. Böyle yaptığımızda BusinessRulesden yeni bir obje üretmemiz yani newlememiz gerekmeden methodlara erişebiliyoruz.
        public static IResult Run(params IResult[] logics) //logic=iş kuralı demek
                                                           //params dediğin zaman Run içine istediğin kadar IResult gönderebiliyorsun. logics = array'ın adı. burada bi polimorfizim tipi işlem yapıyoruz unutma! birbirinden farklı ve ortak yanları sadece IResult olan farklı iş kuralları buraya gelecek.
                                                           //C# arka planda business/concrete/productmanager/BusinessRules.Run(blabla) blabla ile gönderdiği tüm IResultları array'a atıyor ve buraya gönderiyor.
        {
            foreach (var logic in logics)
            {
                if (!logic.Success)
                {
                    return logic;
                }
            }
            return null;

        }
    }
}
