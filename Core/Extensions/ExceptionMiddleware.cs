using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace Core.Extensions
{
    //kampın 20. gününe ait.
    /*
    burası bir hata olduğu zamanki çalışma mevzularını gösteren bir alan. sarmalıyor her şeyi. eğer bir hata yoksa devam et derken
    hata olduğu zaman ona göre error gönderten bir kısım.
    */
    public class ExceptionMiddleware
    {
        private RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                //hata yoksa devamke
                await _next(httpContext);
            }
            //buradaki e değişkeni birazdan hata olarak yakalayağımız şey olacak.
            catch (Exception e)
            {
                //gelen hataya "e" dedik. ve onu yakaladık.
                await HandleExceptionAsync(httpContext, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception e)
        {
            /*content type ne demek? => ben senin tarayıcına bir json gönderdim haberin olsun demek-'miş.
            sağdaki şey ise işte json gönderildiğine dair bir şey. bu content type tamamen gönderilen şeyle alakalı.
            resim gönderseydin eğer sağ tarafı ona göre değiştirecektin sanırım.*/
            httpContext.Response.ContentType = "application/json";
            /*
                ve sonrasında biz internal server error gönderdik. istersek farklı bir error da göndertebilirdik.
             */
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            /*
                HttpStatusCode nedir ? Hani şu 200 OK, 404 Error not found falan var ya onları barındıran yer işte.
             */

            string message = "Internal Server Error";
            /*
                aşağıda ne yapıyoruz? hani biz sistemimize validation ekledik ve validation araçlarından fluent validation kullanmıştık.
                ha işte diyoruz ki eğer sistemdeki hata validation kaynaklı ise bize validationdan gelen mesajı bize ver ve biz de bu hatayı üreten şeye
                (bu şey postman olabilir mesela, ya da frontend yazan kişi http isteği attıktan sonra hatayı konsolda görebilir.)
                bu mesajı iletelim.

                eğer biz fluent validationdaki RuleFor kısımlarında ürün ismi 3 harften az olamaz diye bir kural koyduysak, bu hatayı üreten sisteme
                hata mesajını iletelim. hata mesajını da muhtemelen rulefor kısmında belirtmişizdir.(bakmak lazım.)
             */
            //e.GetType() => e ' nin type'ına bakıyor. bu hata validation hatası mı onu sorgulayacağız.
            //typeof(ValidationException) => normal bir validation hatasının type'ını alıyor. oluşturulan hata bir validation hatası mı ona bakacağız.
            //ekstra not: internal server error => 500 status code'lu hata.
            IEnumerable<ValidationFailure> errors; //error listemiz.
            if (e.GetType() == typeof(ValidationException))
            {
                message = e.Message;
                //aşağısı => ben doğrulama yaptım, senin bir validation exception olduğunu biliyorum.
                //bana döndürdüğün validation hatalarını bir liste halinde dönder.

                errors = ((ValidationException)e).Errors;
                httpContext.Response.StatusCode = 400;
                //neden statuscode'u değiştirdik?
                /*
                    Çünkü üstte status code'u 500 vermiştik. altta ise hata neyse onu gönder dedik. bu arada 500 => internal server error.
                    Burada ise hatanın ne olduğunu biliyoruz. validation hatası. bu hata tipinin bir veritabanı hatası gibi gösterilmesini istemeyiz...
                    ,hata mesajımız 400 (=> BadRequest) olarak gösterilse daha iyi olur.
                 */

                return httpContext.Response.WriteAsync(new ValidationErrorDetails
                {
                    StatusCode = 400,
                    Message = message, /*
                    Messageyi hoca sırf görmemiz için koymuş. Önerilen bir şey değil. Sistem hakkında bilgi vermek doğru değil aslında. Mesaj iptal edilebilir
                    ya da daha genel bir şey yazılabilir. Bu değiştirme buradan değil validation sisteminden halledilmeli. O da business katmanındadır
                    yüksek ihtimalle.
                                        */
                    //ValidationErrors eskiden normal Errorstu. Anlamlı kılmak istedik.
                    ValidationErrors = errors
                }.ToString());
            }

            //alttaki yer "eğer hata verirse" kısmı.
            //bir status code döndürdük bir de message. 
            //status code biliyosun 100 OK, 404 Not Found.
            return httpContext.Response.WriteAsync(new ErrorDetails
            {
                //buralar sistemsel hatalar için. üstteki errordetails özel olanı. sistemsel olmayıp validationdan gelen hatalara özel ürettiğimiz alan.
                StatusCode = httpContext.Response.StatusCode,
                Message = message
            }.ToString());
        }
    }
}
