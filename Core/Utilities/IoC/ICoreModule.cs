using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.IoC
{
    public interface ICoreModule    //bu classın amacı farklı projelerinde kullanabileceğin injectionları sürekli tek tek bir yerlerde yazmak yerine tek bir yerden oluşturman ve yönetmen. bu injectionlardan biri => IHttpAccessor => web apinin elemanıdır kendisi. Sisteme istekte bulunan kişinin isteğinin takibini isteğe cevap verilene kadar takip eder.
        //injection örnek 2 => caching 
        //bazı bilindik injectionlar => business/DependencyResolvers/Autofac içinde gördüklerin :) IUserDal istendiğinde sen git EfUserDal ' ı kullan diyoruz mesela. Peki her sistemde kullanılabilir mi ? hayır. Bu yüzden oradakiler businesste kalacak, şu ankiler core'da olacak... Bizim bütün sistemlerimizde bi istek gelecek ve o isteği takip eden bir IHttpAccessor'a ihtiyacımız olacak. Mevzu bu.
    {
        void Load(IServiceCollection serviceCollection); //bu genel bağımlılıkları yükleyecek. altta söylediğim sadece bi örnek
        //daha açıklayıcı: biz buna bir serviceCollection vereceğiz. Nerede bu ? api/startup ' a girersen en baştaki methodun parametrelerinde göreceksin. biz ona serviceCollection vereceğiz, yükleme işini o yapacak.
        //bir IServiceCollection alacak(onun referansını tuttuğu bir başka şey aslında), bir şey döndürmeden kendi içinde işlem yapacak olan method. IHttpAccessorlu mevzuları bu halledecek sanırım. dikkat.
    }
}
