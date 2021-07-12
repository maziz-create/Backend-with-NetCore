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
            services.AddControllers();//a�a��da sana bi adres verdim oradan gelecek istekleri onaylayacaks�n, �al��.

            services.AddCors();

            //bu sat�r� biz ekledik. alttaki jwtli i�ler toolu �al��m�yordu mevzuyu tam anlayamam��t�k. Not: �u an bi i�levi yok bu kodun ��nk� core katman�nda hallediyoruz. DependencyResolvers ve Utilities/IoC i�indekiler buna �al���yor. genel olarak kullan�lacak injectionlar� core'a ta��ma karar� ald�k. Not: tam alt�mdaki kod bir injection �rne�idir.

            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            //http context accessor = her yap�lan istekle ilgili olu�an http context'i. kullan�c� istek yapt���ndan cevap verilene kadarki s�reci takip eden �ey yani http context accessor. Bu arada bu injection bizim farkl� apilerimiz de olsa orada da kullanabilece�imiz bir �ey. O y�zden bunu core'a ta��may� d���n�yoruz. core katman� = asp.net CORE ,, core = framew�rk'�M�Z. biz yapt�k yani boru de�il.
            //----------------------------------------------

            //-------------- jwtli i�ler -------------------
            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true, // tokenin ya�am d�ng�s�n� kontrol edim mi? mesela sen 10 dakika dedin, kontrol edip 10 dakika sonra sonland�ray�m m� tokeni?
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        ValidateIssuerSigningKey = true, //anahtar� da kontrol edeyim mi tokenoptionstaki?
                        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                    };
                });
            //Bu ge�ici bir ��z�md�=>ServiceTool.Create(services); //bu servicetool ' u biz �rettik core/utilities/IoC/ServiceTool . autofac'in �stteki jwtli i�ler k�sm�ndan haberdar olmas�n� istiyoruz bu create kodu ile.
            services.AddDependencyResolvers(new ICoreModule[] {
                new CoreModule() //farkl� bir module'miz olursa e�er onlar� da buraya ekleyebiliriz.
            }); //sadece CoreModule ' � de�il de ba�ka module'lar olursa onlar� da eklemek i�in bunu yaz�yoruz. s�rayla ekleriz diye i�te.
            //-------------- jwtli i�ler -------------------
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /*
         �stte diyor ki bu methodu http isteklerinin pipeline'�nda y�netmek istediklerin i�in kullan.Buraya nereden geldik? 
         Biz �u an exception middleware yaz�yoruz ve app'imizi hata yakalama i�lerine girdirece�iz. 
         core/extension i�erisinde yer alan, engin hocadan ald���m�z ExceptionMiddleware ' de => "�al��acak her �eye bak, e�er bir hata varsa 
        bu hatan�n mesaj�n� g�nder. hatalardan birisi e�er validation kaynakl� ise validationa eklenen hata mesaj�n� g�nder. 
        e�er bir hata yoksa �al��t�r, devam et" diyoruz.
         */
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app ya�am d�ng�s�nde �retmi� oldu�um m
            app.ConfigureCustomExceptionMiddleware();

            app.UseCors(builder=>builder.WithOrigins("http://localhost:4200").AllowAnyHeader());//bu adresten gelebilecek t�m istekleri onayla. g�venilir.

            //a�a��dakilere middleware deniliyor.
            app.UseHttpsRedirection();
            /* App ya�am d�ng�s� burada yaz�lanlar�n �stten alta s�ras�na g�re �al���yor. */

            app.UseRouting();

            app.UseAuthentication(); //sadece giri� yap�labilmesi i�in gerekli olan anahtar. giri�.

            app.UseAuthorization(); //giren �eyin yetkileri? ne yapabilir ne yapamaz. yetkisi varsa ve bir �ey yapmak istiyorsa yapabilir.

            //not! : yukar�daki son 2 �eyin s�ras�n� ka��rma. �nce eve giri� yapars�n sonra icraatlere ge�ersin. 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
