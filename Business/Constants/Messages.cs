using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Business.Constants
{
    public static class Messages
    {
        public static string ProductAdded = "Ürün eklendi."; //baş harflerini büyük yazıyoruz public değişken olduğu için 
        public static string ProductNameInvalid = "Ürün ismi geçersiz.";
        public static string MaintenanceTime = "Sistem bakımda.";
        public static string ProductsListed = "Ürünler listelendi.";
        public static string ProductCountOfCategoryError = "Bu kategori için ürün ekleme sınırına ulaşıldı.";
        public static string ProductNameAlreadyExists = "Sistemde böyle bir ürün zaten var.";
        public static string CategoryLimitExceeded = "Kategori limiti aşıldı.";
        public static string AuthorizationDenied = "Yetkiniz yok.";
        public static string UserRegistered = "Kayıt Başarılı.";
        public static string UserNotFound = "Kullanıcı bulunamadı.";
        public static string PasswordError = "Şifre hatalı.";
        public static string SuccessfulLogin = "Giriş yapıldı.";
        public static string UserAlreadyExists = "Kullanıcı zaten var.";
        public static string AccessTokenCreated = "Access Token oluşturuldu.";
        public static string Updated = "Güncelleme başarılı";
    }
}
