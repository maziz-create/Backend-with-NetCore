using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Extensions
{
    //extension yapabilmen için o classın static olması gerekir.
    public static class ServiceCollectionExtensions
        //servis koleksiyonunu da extens edebileyim yani genişletebileyim. Startupta yer alan AddDependencyResolvers methodunu eklemek istiyorum. Ekleyeyim ki istediğim kadar Module'ları servisime ekleyip sisteme dahil edebileyim...
    {
        //IServiceCollection nedir? apimizin servis bağımlılıklarını ekleyebildiğimiz ya da araya girmesini istediğimiz servisleri eklediğimiz koleksiyonun ta kendisidir.
        public static IServiceCollection AddDependencyResolvers(this IServiceCollection serviceCollection, ICoreModule[] modules) //neyi genişletmek istersen onu this ile veriyorsun. genişletmek istediğimiz: IServiceCollection ,, neyi koyacaz içine ? birbirinden farklı CoreModule'ları. O yüzden de diyoruz ki sana ICoreModule[] arrayının parametre olarak verilebileceği bir fonksiyon üret. bu fonksiyon ona verilen coremodule'ları alsın sisteme enjekte etsin. bu şekilde webapi ' de ihtiyacımız olan bağımlılıkları veya araya girmesi gereken servisleri buradan sisteme dahil edelim.
            //istediğim kadar ICoreModule tarzı şeyleri ekleyebileyim... aslında burada polimorfizm yapılıyor. farklı coremodule'ları tek seferde buradan tek obje olarak gönderim yapacağız(sanırım)
        {
            foreach (var module in modules)
            {
                module.Load(serviceCollection); //modülleri bu şekilde yüklüyoruz.
            }
            // şimdi ise serviceCollectiona yüklenen servisleri geri döndüreceğiz. bunlar startupta çekilecek.

            return ServiceTool.Create(serviceCollection);
            //ServiceTool.Create => bizim ürettiğimiz bir şey. Core/Utilities/IoC içerisinde mevcut.
            //herhangi bir serviceCollection'u sisteme dahil ediyor.
            //normalde ServiceTool.Create geçici bir çözümdü ve biz bunu webapi/startup'a yazdık. artık buradan topyekün göndereceğiz serviceCollectionların.


            //bu hareket ile bütün injectionları tek bir yerde toplayabiliriz. (core+webapi)
        }
    }
                                       
}
