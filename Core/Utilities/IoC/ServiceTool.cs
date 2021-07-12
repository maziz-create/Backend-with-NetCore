using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.IoC
{
    public static class ServiceTool
    {
        //not: buradaki ServiceTool tamamiyle .net ' in altyapısını kullanır. burası da ICoreModule gibi bi şey işte tam açıklayamıyorum ama service collection var ise demek ki api/startup ' a hizmet ediyor ve bazı hizmetleri yüklüyor sisteme.
        public static IServiceProvider ServiceProvider { get; private set; }
        //mevzuyu anlamadım ama hoca şey diyor .net servislerini burada build ediliyor işte... 
        public static IServiceCollection Create(IServiceCollection services)
        {
            ServiceProvider = services.BuildServiceProvider();
            return services;
        }
    }
}
