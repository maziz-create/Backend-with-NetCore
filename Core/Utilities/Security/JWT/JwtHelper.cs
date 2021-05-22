using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.Security.Encryption;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Core.Utilities.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
        public IConfiguration Configuration { get; }
        //sadece get'i olan proplar = read-only = sadece okunabilir, değiştirilemez. yani set edilemez.
        //IConfiguration = API/appsettings.json'u okumaya yarıyor. tabi orada şu an tek ihtiyacımız oken TokenOptions kısmı.
        private TokenOptions _tokenOptions;
        //TokenOptions = IConfiguration ile okuduğum değerleri burada bir değişkene atamam gerekli... Konfigürasyon sınıfı ile okuduğun değerleri atacağın bir obje açacaksın işte.
        private DateTime _accessTokenExpiration; 
        //_accessTokenExpiration = verilen accesstoken ne zaman sona erecek ? şu andan itibaren 10 dakika içerisinde mesela...
        public JwtHelper(IConfiguration configuration)//dikkat! burada bir constructor var :)
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

        }
        public AccessToken CreateToken(User user, List<OperationClaim> operationClaims)
        {
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);//bu satırın amacı: verilecek olan süre şu anın saatinden 10 dakika sonrası. 10 dakika ise webapi/appsettings.json ' da elle verildi. token dakikası. şu anın tarihine addminutes yap 10 dakikamızı ekle diyor. 
            //benim bunu oluştururken bir security key'e ihtiyacım var... Aa sen uğraşma ben bi tane yaptım, aşağı bak.
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey); //verilen string olan security key'i byte[] hale getirmek için kendi ürettiğimiz securitykeyhelper'e gönderdik. üst commentte bahsettiğim yer burası. yeni bir anahtar üretilmedi, üretilmiş string anahtar byte[] hale getirildi.
            
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };

        }

        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user,
            SigningCredentials signingCredentials, List<OperationClaim> operationClaims)
        {
            var jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now, //verilen süreyi şu andan itibaren olarakdan vereceksin diyoruz.
                claims: SetClaims(user, operationClaims),
                signingCredentials: signingCredentials
            );
            return jwt;
        }

        private IEnumerable<Claim> SetClaims(User user, List<OperationClaim> operationClaims)
            //yetkileri claim obje listesine yerleştirelim.
        {
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName($"{user.FirstName} {user.LastName}");
            claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());

            return claims;
        }
    }
}
