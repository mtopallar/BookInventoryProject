using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Concrete;
using Entities.Concrete;

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
        public static string AuthorAlreadyAdded="Yazar sistemde zaten mevcut.";
        public static string NoActiveAuthorsFound="Sistemde kullanılabilecek yazar bulunmamaktadır.Sistem yöneticinize danışınız.";
        public static string CanNotFindActiveAuthor="Belirtilen yazara erişilemiyor.Sistem yöneticinize danışınız.";
        
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
        public static string GetBooksForAddToLibraryListByNativeStatueSuccessfully="Yerli / yabancı seçiminize göre kitap listesi başarıyla listelenmiştir.";
        public static string GetBookForAddToLibraryByGenreIdSuccessfully="Türe göre kitaplar kütüphane formatında başarıyla listelenmiştir.";
        public static string BookAddedAlreadyBefore="Kitap sistemde zaten mevcut.";

        //Genres
        public static string GetAllGenresSuccessfully="Tüm türler başarıyla listelendi.";
        public static string GetGenreByIdSuccessfully="Tür detaylarına başarıyla ulaşıldı.";
        public static string GenreAddedSuccessfully="Tür başarıyla kaydedildi.";
        public static string GenreUpdatedSuccessfully="Tür bilgileri başarıyla güncellendi.";
        public static string GenreDeletedSuccessfully="Tür başarıyla silindi.";
        public static string GenreAlreadyAdded="Tür sistemde zaten mevcut.";
        

        //Operation Claims
        public static string GetAllOperationClaimsSuccessfully="Tüm yetkiler başarıyla listelendi.";
        public static string ClaimAddedSuccessfully="Yetki başarıyla eklendi.";
        public static string GetOperationClaimByNameSuccessfully="Ada göre yetki detayları başarıyla getirildi.";
        public static string PredefinedClaimsListedSuccessfully="Öntanımlı yetkiler başarıyla listelendi.";
        public static string GetClaimByIdSuccessfully="Yetki detayları başarıyla getirildi.";
        public static string ClaimDeletedSuccessfully="Yetki başarıyla silindi.";
        public static string OperationClaimAlreadyAdded="Yetki sistemde zaten mevcut.";
        public static string ClaimNotFoundOrNotActive = "Kullanıcı yetkisinde yok ya da aktif değil.";
        public static string OperationClaimWrongIdOrClaimNotActive="Rol detaylarına ulaşılamıyor. Hatalı id ya da rol aktif değil.";
        
        
        //Publishers
        public static string GetAllPublishersSuccessfully="Tüm yayınevleri başarıyla listelendi.";
        public static string GetPublisherByIdSuccessfully="Yayınevi detayları başarıyla getirildi.";
        public static string PublisherAddedSuccessfully="Yayınevi başarıyla eklendi.";
        public static string UpdatedPublisherSuccessfully="Yayınevi bilgileri başarıyla güncellendi.";
        public static string DeletePublisherSuccessfully="Yayınevi başarıyla silindi.";
        public static string PublisherAlreadyAdded="Yayınevi sistemde zaten mevcut.";
        
        //UserBooks
        public static string GetUsersAllBooksSuccessfully="Tüm kitaplarınız başarıyla listelendi.";
        public static string GetUsersAllBooksWichHasNoteSuccessfully="Not eklediğiniz tüm kitaplarınız başarıyla listelendi.";
        public static string GetUsersAllBooksByPublisherId="Yayınevine göre tüm kitaplarınız başarıyla listelendi.";
        public static string GetUsersAllBooksByAuthorId="Yazara göre tüm kitaplarınız başarıyla listelendi.";
        public static string GetUserBooksByNativeStatueSuccessfully="Yerli / Yabancı seçiminize göre kitaplarınız başarıyla listelenmiştir.";
        public static string GetUsersAllBookByGenreIdSuccessfully="Türe göre tüm kitaplarınız başarıyla getirildi.";
        public static string GetUsersAllBookByReadStatueSuccessfully="Okuma durumunuza göre kitap listeniz başarıyla listelendi.";
        public static string UserBookAddedSuccessfully="Kitap kütüphanenize başarıyla eklendi.";
        public static string UserBookUpdatedSuccessfully="Kitap detaylarınız başarıyla güncelleştirildi.";
        public static string UserBookDeletedSuccessfully="Kitap kütüphanenizden başarıyla silinmiştir.";
        public static string GetAllUserBookEntitiesSuccessfully="Kullanıcı kitapları başarıyla listelenmiştir.";
        public static string AllUserBookDeletedSuccessfully="Kullanıcının kütüphanesi başarıyla silinmiştir.";
        public static string UserBookAddedAlready="Kitap kütüphanenize daha önce eklenmiş.";

        //UserOperationClaims
        public static string GetAllUserOperaitonClaimsSuccessfully="Tüm kullanıcı yetkileri başarıyla listelendi.";
        public static string GetUserOperationClaimByIdSuccessfully="Kullanıcının yetkileri başarıyla listelendi.";
        public static string UserRoleSuccessfullyAddedToUser="Yeni kullanıcıya yetkisi başarıyla eklendi.";
        public static string UserOperationClaimAddedSuccessfully="Kullanıcıya yetkisi başarıyla eklenmiştir.";
        public static string UserOperationClaimUpdatedSuccessfully="Kullanıcını yetkisi başarıyla güncellenmiştir.";
        public static string UserOperationClaimDeletedSuccessfully="Kullanıcı yetkisi başarıyla silinmiştir.";
        public static string UserOperationClaimDeletedSuccessfullyByUser="Kullanıcının yetkileri kullanıcı isteğiyle başarıyla silinmiştir.";
        
        
        //UserManager
        public static string GetUsersAllClaimsSuccessfully="Kullanıcının tüm rolleri rol formatında getirildi.";
        public static string GetAllUserDetailsWitrRolesSuccessfully="Tüm kullanıcıların detayları ve sahip olduğu roller başarıyla listelendi.";
        public static string GetUserDetailsWithRolesByUserIdSuccessfully="Kullanıcının detayları ve sahip olduğu roller başarıyla listelendi.";
        public static string GetAllUsersSuccessfully="Tüm kullanıcılar başarıyla listelendi.";
        public static string GetUserByEmailSuccessfully="Mail adresine göre kullanıcı detayları başarıyla listelendi.";
        public static string GetUserByIdSuccessfully="Kullanıcı detayları başarıyla getirildi.";
        public static string UserAddedSuccessfully="Kaydınız başarıyla yapılmıştır.";
        public static string CurrentUserPasswordError="Mevcut şifrenizi hatalı girdiniz.";
        public static string UserUpdatedSuccessfully="Kullanıcı bilgileriniz başarıyla güncellendi.";
        public static string NewEmailAlreadyExists="Kullanmak istediğiniz Email başka kullanıcı tarafından kullanılıyor.";
        public static string UserAndUsersBooksDeletedSuccessfullyByAdmin="Kullanıcı, kullanıcının tüm yetkileri kütüphanesi yönetici tarafından başarıyla silinmiştir.";
        public static string UserAndUsersBooksAndUserClaimsDeletedSuccessfullyByUser="Kullanıcıya ait tüm kayıtlar kullanıcı isteği ile silinmiştir.";
        public static string UserNotFoundByThisMail="Girilen mail ile kayıtlı kullanıcı bulunmamaktadır.";
        
    }
}
