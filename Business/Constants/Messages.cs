using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Concrete;

namespace Business.Constants
{
    public static class Messages
    {
        //Secured Operations
        public static string AuthorizationDenied = "Bu işlem için yetkiniz yok.";

        // AuthManager
        public static string UserRegistered="Kullanıcı kaydı başarılı.";
        public static string UserNotFound="Kullanıcı bulunamadı.";
        public static string PasswordError="Hatalı parola.";
        public static string LoginSuccessfull="Giriş başarılı.";
        public static string UserAlreadyExists="Bu mail adresi daha önce kullanılmış.";
        public static string AccessTokenCreated="Yetkilendirme başarılı.";
        
        //Authors
        public static string GetAllAuthorsSuccessfully="Tüm yazarlar başarıyla listelendi.";
        public static string GetAuthorByIdSuccessfully="Yazar detaylarına başarıyla ulaşıldı.";
        public static string AuthorAddedSuccessfully="Yazar başarıyla kaydedildi.";
        public static string AuthorUpdatedSuccessfully="Yazar bilgileri başarıyla güncellendi.";
        public static string AuthorDeletedSuccessfully="Yazar başarıyla silindi.";
        
        //Books
        public static string GetAllBooksSuccessfully="Tüm kitaplar başarıyla listelendi.";
        public static string GetBookByIdSuccessfully="Kitap detaylarına başarıyla ulaşıldı.";
        public static string BookAddedSuccessfully="Kitap başarıyla kaydedildi.";
        public static string BookUpdatedSuccessfully="Kitap bilgileri başarıyla güncellendi.";
        //public static string BookDeletedSuccessfully="Kitap başarıyla silindi.";
        
        //Genres
        public static string GetAllGenresSuccessfully="Tüm türler başarıyla listelendi.";
        public static string GetGenreByIdSuccessfully="Tür detaylarına başarıyla ulaşıldı.";
        public static string GenreAddedSuccessfully="Tür başarıyla kaydedildi.";
        public static string GenraUpdatedSuccessfuully="Tür bilgileri başarıyla güncellendi.";
        public static string GenreDeletedSuccessfully="Tür başarıyla silindi.";
    }
}
