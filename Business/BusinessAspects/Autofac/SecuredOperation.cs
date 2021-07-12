using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Core.Extensions;
using Business.Constants;

namespace Business.BusinessAspects.Autofac
{
    // bu class JWT için
    public class SecuredOperation : MethodInterception
    {
        private string[] _roles;
        private IHttpContextAccessor _httpContextAccessor;

        public SecuredOperation(string roles)
        {
            _roles = roles.Split(','); //gelen roller virgülle ayrılıyor. product.add,admin falan yazmıştık manager kısmında. split napıyor ? bir string'i senin belirlediğin bir karaktere göre parçalıyor ve array'a atıyor. yani roller olarak product.add,admin,editor olarak gelecek ve biz bunları split kodu ile virgüllerle ayırıp array'a atıyoruz. yani _roles burada bir string[]
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>(); //IHttpContext asp.netin içinde gelen bir şey. enjekte edilmesi gerekiyor. ne işe yaradığını bilmiyorum.

        }

        protected override void OnBefore(IInvocation invocation)
        {
            var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles(); // o anki kullanıcının claim rollerini bul. bu kişinin yetkileri neler?
            foreach (var role in _roles)
                //bu kullanıcının claimlerini gez. içinde ilgili claim varsa return et yani methodu çalıştırmaya devam et. hangi method ? bizim doğrulama için üzerine [blablabla] yazacağımız methodu çalıştırmaya devam et.
            {
                if (roleClaims.Contains(role))
                {
                    return;
                }
                //yetkisi yok mu ? o zaman authorizationDenied hatası döndür.
            }
            throw new Exception(Messages.AuthorizationDenied);//ilgili hatayı fırlatır.
        }
    }
}
