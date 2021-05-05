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
        public static string GenreUpdatedSuccessfully="Tür bilgileri başarıyla güncellendi.";
        public static string GenreDeletedSuccessfully="Tür başarıyla silindi.";

        //Nationalities
        public static string GetAllNationalitySuccessfull="Tüm ülkeler başarıyla listelendi.";
        public static string GetNationalityByIdSuccessfully="Ülke detayları başarıyla getirildi.";
        public static string NationalityAddedSuccessfully="Ülke başarıyla eklendi.";
        public static string NationalityUpdatedSuccessfully="Ükle bilgileri başarıyla güncellendi.";
        public static string NationalityDeletedSuccessfully="Ülke başarıyla silindi.";
        
        //Operation Claims
        public static string GetAllOperationClaimsSuccessfully="Tüm yetkiler başarıyla listelendi.";
        public static string ClaimAlreadyAdded="Yetki zaten mevcut.";
        public static string AllClaimsAddedSuccessfully="Öntanımlı yetkiler başarıyla eklendi.";
        public static string OperationClaimDeletedSuccessfully="Yetki başarıyla silindi.";
        
        
        //Publishers
        public static string GetAllPublishersSuccessfully="Tüm yayınevleri başarıyla listelendi.";
        public static string GetPublisherByIdSuccessfully="Yayınevi detayları başarıyla getirildi.";
        public static string PublisherAddedSuccessfully="Yayınevi başarıyla eklendi.";
        public static string UpdatedPublisherSuccessfully="Yayınevi bilgileri başarıyla güncellendi.";
        public static string DeletePublisherSuccessfully="Yayınevi başarıyla silindi.";
        
        //UserBooks
        public static string GetUsersAllBooksSuccessfully="Tüm kitaplarınız başarıyla listelendi.";
        public static string GetUsersAllBooksWichHasNoteSuccessfully="Not eklediğiniz tüm kitaplarınız başarıyla listelendi.";
        public static string GetUsersAllBooksByPublisherId="Yayınevine göre tüm kitaplarınız başarıyla listelendi.";
        public static string GetUsersAllBooksByAuthorId="Yazara göre tüm kitaplarınız başarıyla listelendi.";
        public static string GetUsersAllBooksByAuthorNationality="Yazar ülkesine göre tüm kitaplarınız başarıyla listelendi.";
        public static string GetUsersAllBookByGenreIdSuccessfully="Türe göre tüm kitaplarınız başarıyla getirildi.";
        public static string GetUsersAllBookByReadStatueSuccessfully="Okuma durumunuza göre kitap listeniz başarıyla listelendi.";
        public static string UserBookAddedSuccessfully="Kitap kütüphanenize başarıyla eklendi.";
        public static string UserBookUpdatedSuccessfully="Kitap detaylarınız başarıyla güncelleştirildi.";
        public static string UserBookDeletedSuccessfully="Kitap kütüphanenizden başarıyla silinmiştir.";

        //UserOperationClaims
        public static string GetAllUserOperaitonClaimsSuccessfully="Tüm kullanıcı rolleri başarıyla listelendi.";
        public static string GetUserOperationClaimByIdSuccessfully="Kullanıcının rolleri başarıyla listelendi.";
        
        public static string UserOperationClaimAddedSuccessfully="Kullanıcıya rolü başarıyla eklenmiştir.";
        public static string UserOperationClaimUpdatedSuccessfully="Kullanıcını rolü başarıyla güncellenmiştir.";
        public static string UserOperationClaimDeletedSuccessfully="Kullanıcı rolü başarıyla silinmiştir.";
        
        //UserManager
        public static string GetUsersAllClaimsSuccessfully="Kullanıcının tüm rolleri rol formatında getirildi.";
        public static string GetAllUserDetailsWitrRolesSuccessfully="Tüm kullanıcıların kullanıcı ve rol detayları başarıyla listelendi.";
        public static string GetUserDetailsWithRolesByUserIdSuccessfully="Kullanıcının rolleri detaylarıyla listelendi.";
    }
}
