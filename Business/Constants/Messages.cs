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
        public static string GetAllBooksForAddToLibrarySuccessfully="Tüm kitaplar kütüphane formatında listelenmiştir.";
        public static string GetBookForAddToLibraryByIsbnSuccessfully="Isbn numarasına göre kitap kütüphane formatında getirilmiştir.";
        public static string GetBookForAddToLibraryByBookNameSuccessfully="Kitap adına göre kitaplar kütüphane formatında listelenmiştir.";
        public static string GetBookForAddToLibraryByPublisherIdSuccessfully="Yayınevine göre kitaplar kütüphane formatında listelenmiştir.";
        public static string GetBookForAddToLibraryByAuthorIdSuccessfully="Yazara göre kitaplar kütüphane formatında başarıyla listelenmiştir.";
        public static string GetBookForAddToLibraryByNationalityIdSuccessfully="Yazar ülkesine göre kitaplar kütüphane formatında başarıyla listelenmiştir.";
        public static string GetBookForAddToLibraryByGenreIdSuccessfully="Türe göre kitaplar kütüphane formatında başarıyla listelenmiştir.";

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
        public static string ClaimAddedSuccessfully="Yetkiler başarıyla eklendi.";
        public static string GetOperationClaimByNameSuccessfully="Ada göre yetki detayları başarıyla getirildi.";
        public static string PredefinedClaimsListedSuccessfully="Öntanımlı yetkiler başarıyla listelendi.";
        public static string OperationClaimAlreadyExists="Bu yetki sisteme daha önce eklenmiş.";
        public static string GetClaimByIdSuccessfully="Yetki detayları başarıyla getirildi.";
        public static string ClaimDeletedSuccessfully="Yetki başarıyla silindi.";
        
        
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
        public static string GetAllUserBookEntitiesSuccessfully="Kullanıcı kitapları başarıyla listelenmiştir.";
        public static string AllUserBookDeletedSuccessfully="Kullanıcının kütüphanesi başarıyla silinmiştir.";

        //UserOperationClaims
        public static string GetAllUserOperaitonClaimsSuccessfully="Tüm kullanıcı rolleri başarıyla listelendi.";
        public static string GetUserOperationClaimByIdSuccessfully="Kullanıcının rolleri başarıyla listelendi.";
        public static string UserRoleSuccessfullyAddedToUser="Kullanıcıya kullanıcı rolü başarıyla eklendi.";
        public static string UserOperationClaimAddedSuccessfully="Kullanıcıya rolü başarıyla eklenmiştir.";
        public static string UserOperationClaimUpdatedSuccessfully="Kullanıcını rolü başarıyla güncellenmiştir.";
        public static string UserOperationClaimDeletedSuccessfully="Kullanıcı rolü başarıyla silinmiştir.";
        
        //UserManager
        public static string GetUsersAllClaimsSuccessfully="Kullanıcının tüm rolleri rol formatında getirildi.";
        public static string GetAllUserDetailsWitrRolesSuccessfully="Tüm kullanıcıların kullanıcı ve rol detayları başarıyla listelendi.";
        public static string GetUserDetailsWithRolesByUserIdSuccessfully="Kullanıcının rolleri detaylarıyla listelendi.";
        public static string GetAllUsersSuccessfully="Tüm kullanıcılar başarıyla listelendi.";
        public static string GetUserByEmailSuccessfully="Mail adresine göre kullanıcı detayları başarıyla listelendi.";
        public static string GetUserByIdSuccessfully="Kullanıcı detayları başarıyla getirildi.";
        public static string UserAddedSuccessfully="Kaydınız başarıyla yapılmıştır.";
        public static string CurrentUserPasswordError="Mevcut şifrenizi hatalı girdiniz.";
        public static string UserUpdatedSuccessfully="Kullanıcı bilgileriniz başarıyla güncellendi.";
        public static string NewEmailAlreadyExists="Kullanmak istediğiniz Email başka kullanıcı tarafından kullanılıyor.";
        public static string UserAndUsersBooksDeletedSuccessfullyByAdmin="Kullanıcı (ve kullanıcının kütüphanesi) yönetici tarafından başarıyla silinmiştir.";
        public static string UserAndUsersBooksDeletedSuccessfullyByUser="Kullanıcıya ait tüm kayıtlar kullanıcı isteği ile silinmiştir.";
        
    }
}
