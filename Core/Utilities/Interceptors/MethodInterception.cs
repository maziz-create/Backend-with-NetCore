using Castle.DynamicProxy;
using System;

namespace Core.Utilities.Interceptors
{
    public abstract class MethodInterception : MethodInterceptionBaseAttribute
    {
        //on before = methoddan önce çalışacak olan aspect
        //diğerleri de adında yazdığı gibi
        //
        //invocation = business methodu. add/delete/...
        //
        //protected virtual void Methodismi(Parametreler) demek : ezmek istenilen bir method demek. Biz yazdığımız aspect'e gidip orada bu methodlardan kullanmak istediğimizi kullanıcaz. Mesela doğrulama aspectine gittik, methodlardan önce çalışmasını istediğimiz için önce doğrulama(validation) aspectimize sen bir method interceptionsun dedik sonra onbefore methodunu orada ezerek kullandık. artık bizim validation aspectimiz istediğimiz her methodun önünde çalışabilir. doğrulama maksadı ile... neyi doğrulayacak ? mesela mail adresinde "ğ" olmasın. hangi kurallar ile doğrulayacak ? Business/ValidationRules/FluentValidation/ProductValidator/RuleFor methodları...
        protected virtual void OnBefore(IInvocation invocation) { }
        protected virtual void OnAfter(IInvocation invocation) { }
        protected virtual void OnException(IInvocation invocation, System.Exception e) { }
        //on exception = hata verdiğinde
        protected virtual void OnSuccess(IInvocation invocation) { }
        public override void Intercept(IInvocation invocation)
        {
            var isSuccess = true;
            OnBefore(invocation);
            try
            {
                invocation.Proceed(); //_carDal.Add(); ' a karşılık gelir.
            }
            catch (Exception e)
            {
                isSuccess = false;
                OnException(invocation, e);
                throw;
            }
            finally
            {
                if (isSuccess)
                {
                    OnSuccess(invocation);
                }
            }
            OnAfter(invocation);
        }
    }
}
