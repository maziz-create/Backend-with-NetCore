using Business.Abstract;
using Business.Concrete;
using Core.DependencyResolvers;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();//aþaðýda sana bi adres verdim oradan gelecek istekleri onaylayacaksýn, çalýþ.

            services.AddCors();

            //bu satýrý biz ekledik. alttaki jwtli iþler toolu çalýþmýyordu mevzuyu tam anlayamamýþtýk. Not: þu an bi iþlevi yok bu kodun çünkü core katmanýnda hallediyoruz. DependencyResolvers ve Utilities/IoC içindekiler buna çalýþýyor. genel olarak kullanýlacak injectionlarý core'a taþýma kararý aldýk. Not: tam altýmdaki kod bir injection örneðidir.

            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            //http context accessor = her yapýlan istekle ilgili oluþan http context'i. kullanýcý istek yaptýðýndan cevap verilene kadarki süreci takip eden þey yani http context accessor. Bu arada bu injection bizim farklý apilerimiz de olsa orada da kullanabileceðimiz bir þey. O yüzden bunu core'a taþýmayý düþünüyoruz. core katmaný = asp.net CORE ,, core = framewörk'ÜMÜZ. biz yaptýk yani boru deðil.
            //----------------------------------------------

            //-------------- jwtli iþler -------------------
            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true, // tokenin yaþam döngüsünü kontrol edim mi? mesela sen 10 dakika dedin, kontrol edip 10 dakika sonra sonlandýrayým mý tokeni?
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        ValidateIssuerSigningKey = true, //anahtarý da kontrol edeyim mi tokenoptionstaki?
                        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                    };
                });
            //Bu geçici bir çözümdü=>ServiceTool.Create(services); //bu servicetool ' u biz ürettik core/utilities/IoC/ServiceTool . autofac'in üstteki jwtli iþler kýsmýndan haberdar olmasýný istiyoruz bu create kodu ile.
            services.AddDependencyResolvers(new ICoreModule[] {
                new CoreModule() //farklý bir module'miz olursa eðer onlarý da buraya ekleyebiliriz.
            }); //sadece CoreModule ' ý deðil de baþka module'lar olursa onlarý da eklemek için bunu yazýyoruz. sýrayla ekleriz diye iþte.
            //-------------- jwtli iþler -------------------
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /*
         üstte diyor ki bu methodu http isteklerinin pipeline'ýnda yönetmek istediklerin için kullan.Buraya nereden geldik? 
         Biz þu an exception middleware yazýyoruz ve app'imizi hata yakalama iþlerine girdireceðiz. 
         core/extension içerisinde yer alan, engin hocadan aldýðýmýz ExceptionMiddleware ' de => "çalýþacak her þeye bak, eðer bir hata varsa 
        bu hatanýn mesajýný gönder. hatalardan birisi eðer validation kaynaklý ise validationa eklenen hata mesajýný gönder. 
        eðer bir hata yoksa çalýþtýr, devam et" diyoruz.
         */
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app yaþam döngüsünde üretmiþ olduðum m
            app.ConfigureCustomExceptionMiddleware();

            app.UseCors(builder=>builder.WithOrigins("http://localhost:4200").AllowAnyHeader());//bu adresten gelebilecek tüm istekleri onayla. güvenilir.

            //aþaðýdakilere middleware deniliyor.
            app.UseHttpsRedirection();
            /* App yaþam döngüsü burada yazýlanlarýn üstten alta sýrasýna göre çalýþýyor. */

            app.UseRouting();

            app.UseAuthentication(); //sadece giriþ yapýlabilmesi için gerekli olan anahtar. giriþ.

            app.UseAuthorization(); //giren þeyin yetkileri? ne yapabilir ne yapamaz. yetkisi varsa ve bir þey yapmak istiyorsa yapabilir.

            //not! : yukarýdaki son 2 þeyin sýrasýný kaçýrma. Önce eve giriþ yaparsýn sonra icraatlere geçersin. 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
