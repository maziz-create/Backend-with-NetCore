using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        /*
            .net core ekibi => app'ın çalışma yaşam döngüsünde eklemek istediğin middleware var mı? => app.UseMiddleware<MİDDLEWARE>();
            biz => biz bir ExceptionMiddleware yazdık, al bunu kullan.
         */

        /*
            Yazdığımız middleware ' e dair açıklama=>
            Kötü kodlarda hata ayıklama yapılırken hata oluşabilecek her yere try catch konulur. buna gerek yok.
            Yazdığımız middleware'i webapi/startup.cs altına ekledik. middlewaremiz yapılan bütün her şeyi sarmalıyor, bütün her işlemi 
            bir try catch'den geçiriyor ve sorun çıktığı zaman uygun şekilde hatayı döndürüyor. Sorun çıkmadığında devam et ve gerekli işlemi
            hallet diyor. bu işlem getall products olabilirdi... 
            bu işlemin bir de şu güzel yanı var =>
            çıkan hatanın tipini önden görebiliyorsun ve ona göre özel hata mesajları gönderebiliyorsun istek yapan yere.
            biz bu projede eğer hata validation tipindeyse direkt validationun hata mesajını gönder diyoruz. o hata mesajını da biz belirliyoruz.
            çünkü validationu fluent validation kullanarak, 
         */
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
