using Core.Utilities.IoC;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using System.Linq;

namespace Core.CrossCuttingConcerns.Caching.Microsoft
{
    //not: burada bir Adapter Pattern yer alıyor. Bir şey enjekte edileceğinde bu pattern'i kullanabilirsin diyorlar. Yani sana hazır bir şey verilir ama sen yine de interface/manager kısımlarını üret, üret ki yarın bir gün farklı yönteme geçtiğinde sadece yeni managerini yazıp interface'nin tuttuğu referansı değiştir. tak çıkart yap yani.
    //MemoryCacheManager micorosftun ürünüdür. InMemory Caching için buna ihtiyaç duyduk.
    public class MemoryCacheManager : ICacheManager //MemCacheManager diye bir şey de var. kullanmıyoruz.
    {
        IMemoryCache _memoryCache;//bu interface microsoftun memorycache kütüphanesinde yer alıyor. usingten de görülebilir.
        //NOT: IMemoryCache'nin injectionu için sakın constructor kullanma! bunun bi crossCuttingConcern olduğunu unutma. Bunlar zincirde yer almadığı için bu şekilde enjekte edemezsin bu sayfaya IMemoryCache'i
        //Peki IMemoryCache'i nasıl enjekte edeceğiz? CoreModulede .net core altyapısında bulunan serviceCollection.AddMemoryCache(); kodu ile...

        //Peki başka bir caching yönteminde ne yapıyoruz? Gerekli kodları caching içine yaptıktan sonra ilgili interface'i ve onun arkasında, onun referansını tuttuğu manager sınıfı (illa isminde manager olacak değil)  gidip CoreModule kısmına ekliyoruz. Eski şeyi siliyoruz... Eskiden ICacheManager istendiğinde MemoryCacheManager(yani burası) veriliyordu, artık RedisCacheManager verilsin diyeceğiz. Redisi ise yazmış bulunduracağız sistemimizde. Olay budur.
        public MemoryCacheManager()
        {
            _memoryCache = ServiceTool.ServiceProvider.GetService<IMemoryCache>(); //microsoft tarafından memory'de instance'ı oluşan IMemoryCache'nin referansını bize ver. Yukarda özellikle kasik methodla bu injectionun olmayacağını, core modulede bazı işlemler yapıldığını anlattım. burası da sonu sanırım.
        }
        public void Add(string key, object value, int duration)
        {
            _memoryCache.Set(key, value, TimeSpan.FromMinutes(duration));//TimeSpan: dakika olarak eklenmesini sağlıyor.
            //bu kodla birlikte verilen süre kadar cachede o cevap tutulacaktır.
            //duration=> devam süresi.

        }

        public T Get<T>(string key)
            //ilerde type casting(tip dönüşümü) yapmamak için bu yöntem daha çok kullanışlıymış object tipine göre.
        {
            return _memoryCache.Get<T>(key);
        }

        public object Get(string key)
        {
            return _memoryCache.Get(key);
        }

        public bool IsAdd(string key)
            //bellekte şu an böyle bir cache var mı?
        {
            return _memoryCache.TryGetValue(key, out _);
            //out _ anlamı: TryGetValue methodu hem key istiyor bize bool döndürecek ama hem de o var olan şeyi de döndürmek istiyor. biz ona sadece öyle bir cache var mı diye bak diyoruz, o bakıp var ya  da yok diyor, varsa eğer var olan şeyi de döndürüyor. biz bunu istemiyoruz bu yüzden out _ gönderiyoruz. o da zaten outlu şeyler gönder demişti ben orada set edeyim referansına yerleştireyim sen istediğin yerde kullan... ama biz diyoz ki zahmet edip de bana cache değerini verme :)
            //out _ gönderimi C# için klasikleşmiştir. benzer senaryolarda kullanılır.
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public void RemoveByPattern(string pattern)
            //burada reflection var yani kodu çalışma anında çalıştırma, koda çalışma anında mühahale etme vb...
        {
            var cacheEntriesCollectionDefinition = typeof(MemoryCache).GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var cacheEntriesCollection = cacheEntriesCollectionDefinition.GetValue(_memoryCache) as dynamic;
            List<ICacheEntry> cacheCollectionValues = new List<ICacheEntry>();

            foreach (var cacheItem in cacheEntriesCollection)
            {
                ICacheEntry cacheItemValue = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null);
                cacheCollectionValues.Add(cacheItemValue);
            }

            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = cacheCollectionValues.Where(d => regex.IsMatch(d.Key.ToString())).Select(d => d.Key).ToList();

            foreach (var key in keysToRemove)
            {
                _memoryCache.Remove(key);
            }
        }
    }
}
