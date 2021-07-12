using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CrossCuttingConcerns.Caching
{
    //bu interface'e neden ihtiyaç duyuyoruz? Farklı cache yöntemleri var! Biz burada InMemory caching kullanıcaz. Farklı yöntemlerin ortak methodları olacak muhtemelen ve biz burada o methodları ICacheManager üzerinden farklı yöntemlerin classlarına implemente edeceğiz.

    //InMemory Caching: web sitemizin bulunduğu makinanın belleğinde belli süre ile verilmiş belli methodların çıktıları yer alıyor. kullanıcılar o isteği defalarca kez yaptığı zaman her seferinde veritabanına gidilip yorulma işlemi olmasın diye (ve çıktı değişmediği için, hepsinin isteği aynı çıktıyı verdiği için) aynı cevap tüm kullanıcılara verilir. bu cevap ise makinanın belleğinde tutulduğu için InMemory Caching olarak isimlendirilir.
    public interface ICacheManager
    {
        void Add(string key, object value, int duration); // object tipindeki bir şey HER ŞEY olabilir. "var" veri tipi gibi. diğer bir deyiş ile object => tüm her şeyin base'i dir. gönderdiğimiz CEVAP her şey olabileceği için bu yöntemi tercih ediyoruz.
        //duration: bu cachede ne kadar duracak? memoryde bu cevabı ne kadar süre boyunca tutalım? dakika ya da saat cinsinden tutabiliriz.

        T Get<T>(string key);//Bir key verilecek ve sen bize T döndüreceksin. Peki <T> ne ? Döndürülen şey ne olursa olsun onu bir genericList yap demek. eğer tek elemanlı yani tek bir obje, tek bir şey dönecekse buna gerek yok ama işimizi sağlama almak için böyle yapıyoruz. Bu demek oluyor ki her şey ama her şey bi genericList olarak dönecek.
        //T Get<T>   alternatifi =>    object Get(string key); gelen şey ne olursa o şekilde gönder. Listeyse liste şekilde, tek elemanlı bi şeyse o şekilde gönder. object'in açıklamasını yukarda yaptık.
        object Get(string key);//kafasına göre, isterse kullanır :)
        bool IsAdd(string key);//aradığımız methodun cevabı cachede var mı? varsa oradan getiricez, yoksa veritabanından getirip memory'e süreye ve keye göre ekleme DE yapacak.
        void Remove(string key); //cacheden istediğimiz cevabı uçurmak için
        void RemoveByPattern(string pattern); //başında Get olanları ya da içinde Category geçenleri uçur. parametreli cachelere tam üstteki Remove methodu yetişemediği için buna ihtiyaç duyuluyor.
        //Bu bir pattern'dir. RegularExpirationPattern dendi.

    }
}
