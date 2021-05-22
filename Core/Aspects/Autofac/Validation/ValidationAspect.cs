using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Interceptors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Aspects.Autofac.Validation
{
    public class ValidationAspect : MethodInterception  //ValidationAspect = aspectimiz
        //Aspect ne demek ? methodun başın sonunda ya da hata verdiğinde çalışacak olan yapı. Sen neresinde çalışsın istiyorsan orasında çalışır...
        //Validation aspectimiz nedir ? Bir attribute'tür :D Methodun bi yerinde çalışsın ve o methoda dair bir şeyler akıtsın bize, yani o methoda anlam katsın.
    {
        private Type _validatorType;
        public ValidationAspect(Type validatorType)
        //Bana bir doğrulama type'ı ver. neyi doğrulayacağım?
        //Verilen yer: business/concrete/productmanager/add methodunun üstündeki         [ValidationAspect(typeof(ProductValidator))] yapısı... typeof(ProductValidator) diyoruz ve böylelikle validation aspectimizin istediği validator type'ını veriyoruz. sen ürünleri doğrulayacaksın diyoruz...
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
            {
                throw new System.Exception("Bu bir doğrulama sınıfı değil");
            }

            _validatorType = validatorType;
        }
        protected override void OnBefore(IInvocation invocation)
            //methodun başında çalışacağız, okey, peki ne yapacağız?
        {
            //alttaki kod: Activator.CreateInstance kodu çalışma anında newleme yapar. genel bi newleme değildir. Altta diyor ki biz sana ProductValidator gönderdik, bunun şuan için minik bir newlenmeye, bellekte oluşturulmaya ihtiyacı var... Ama işte IValidator tipinde kullanmak istiyorum. Referans adresini değiştirme fakat IValidator gibi kullanabilmemi sağla.
            var validator = (IValidator)Activator.CreateInstance(_validatorType);
            //alttaki kod: bize gelen productvalidatorun tam olarak hangi entity nesnesi üzerinden olduğunu yakala. yani productvalidatorun product üzerindeki bilgilerde doğrulama yapacağını söylüyoruz açıkçası. entityType = product enitity oluyor.
            var entityType = _validatorType.BaseType.GetGenericArguments()[0];
            //alttaki kod: invocation = işlemin yapılmak istendiği method. gel sen o methodun yani add methodunun argümanlarını gez.eğer oradaki bir tip benim az önce yakaladığım product validatorun entity tipi olan product tipindeyse onu entitites ' e yerleştir. yani dizi haline getir. Bunu LINQ ile yapıyoruz zaten biliyorsun.
            var entities = invocation.Arguments.Where(t => t.GetType() == entityType);
            //alttaki kod: entities dizisindeki eşyleri entity üzerinden validate et(doğrula). ValidationTool kullanılıyor 
            foreach (var entity in entities)
            {
                ValidationTool.Validate(validator, entity);
            }
        }
    }
}
