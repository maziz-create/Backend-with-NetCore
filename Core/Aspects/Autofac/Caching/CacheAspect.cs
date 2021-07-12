using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Caching;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection; //  MICROSOFT BUNU EKLEYEMİYOR HİÇ. ALTTAKİ GetService kısmı bunun yüzünden çalışmıyor.

namespace Core.Aspects.Autofac.Caching
{
    //Intercept yöntemiyle aspect yaptık. OnBefore gibi bir şey. 
    //Method çalışırken çalışan bir şey. Zaten caching reflection mantığı ile kurulan bir yapı. reflection = bir methodun çalışma anında bir şeyin çalıştırılması. 
    
    public class CacheAspect : MethodInterception
    {
        private int _duration;
        private ICacheManager _cacheManager;

        public CacheAspect(int duration = 60)
        {
            _duration = duration;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }

        public override void Intercept(IInvocation invocation)  //invocation = method demek
        {
            var methodName = string.Format($"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}");
            var arguments = invocation.Arguments.ToList();
            var key = $"{methodName}({string.Join(",", arguments.Select(x => x?.ToString() ?? "<Null>"))})"; // 2 soru işaretinin anlamı: varsa soldakini ekle yoksa sağdakini ekle demek. x? ne demek => x=null olabilir demek. "," = virgülle ayır sağdakileri demek. Join = birleştir sağdaki her şeyi demek. tabiki string olarak.
            //invocation = cachede kullanılacak olan method
            //arguments = invocationun parametreleri.
            //key = invocationun tam olarak hangi method olduğunu gösteren bir anahtar. bu değeri string olarak atıyoruz ve önce ReflectedType ile bulunduğu yerin namespace'ini, sonrasında ise methodun adını ve varsa parametrelerini veriyoruz. üstteki arguments.selectte o oyunların yapılma sebebi bu.
            if (_cacheManager.IsAdd(key)) //BP
            {
                invocation.ReturnValue = _cacheManager.Get(key);        //cachede varsa eğer buradan gönderiyor. yoksa aşağıdaki invocation.Proceed();'e giriyor.
                return;
            }
            invocation.Proceed(); //aga bu cachede yok devamke git DB'e...
            _cacheManager.Add(key, invocation.ReturnValue, _duration);
        }
    }
}
