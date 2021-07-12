using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.Microsoft;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Core.DependencyResolvers
{
    public class CoreModule : ICoreModule //servis bağımlılıklarımız burada çözümlenecek. 
        //eskiden bağımlılıklarımızı startup ' a yazıyorduk ya mesela örnek olarak alttaki HttpContextAccessor, ha işte artık onlar burada
    {
        public void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMemoryCache();//bu kod arka planda ICacheManager instance'ı oluşturuyor. Biz MemoryCacheManager içinde yani o an kullanmak istediğimiz cache yönteminin class'ının referansını burada üretilen ICacheManager ' e veriyoruz. Bu şekilde microsoftun memorycache kodları ile bizim kodlarımızın bağlantısı kurulmuş oluyor.
            //bu kod .net altyapısında mevcut. not!!: redis caching'e geçtiğin taktirde bu satıra gerek kalmıyor. bu satır memorycachemanager içinde inject ettiğin IMemoryCache interface'inin referans tuttuğu iş classını veriyor sana. ve neden ayrı olarak yazıyoruz çünkü microsoftun kodu olduğu için kendileri otomatik üretmiş... peki neden memorycachemanager içerisinde klasik constructor yöntemi ile yapmadık? orada açıkladım. cross cutting concerns mevzuları...
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            serviceCollection.AddSingleton<ICacheManager, MemoryCacheManager>();
            //serviceCollection.AddSingleton<Stopwatch>(); //Performans yönetimi için yerleştirdiğimiz saat sayacının instance'ı için.
            serviceCollection.AddSingleton<Stopwatch>();
        }
    }
}
