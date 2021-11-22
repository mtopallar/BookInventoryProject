using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Concrete;
using Core.Utilities.Security.Jwt;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Constants
{
    public static class Messages
    {
        //Secured Operations
        public static string AuthorizationDenied = "Bu işlem için yetkiniz yok.";

        // AuthManager
        public static string UserRegistered = "Kullanıcı kaydı başarılı.";
        public static string UserNotFound = "Kullanıcı bulunamadı.";
        public static string PasswordError = "Hatalı parola.";
        public static string LoginSuccessfull = "Giriş başarılı.";
        public static string UserAlreadyExists = "Bu mail adresi daha önce kullanılmış.";
        public static string AccessTokenCreated = "Yetkilendirme başarılı.";
        public static string UserHasNoActiveRoleToCreateAccessToken="Kullanıcının sistemde aktif rolü bulunmadığı için erişim yetkisi oluşturulamıyor.";
        public static string CanNotReachedUserRoleToCheckRoleBeforeCreateAccessToken= "Sistemi kullanabilmeniz için gerekli olan kullanıcı rolü size tanımlı olmadığından erişim yetkisi oluşturulamıyor.Sistem yöneticinizi bilgilendiriniz.";

        //Authors
        public static string GetAllAuthorsSuccessfully = "Tüm yazarlar başarıyla listelendi.";
        public static string GetAuthorByIdSuccessfully = "Yazar detaylarına başarıyla ulaşıldı.";
        public static string AuthorAddedSuccessfully = "Yazar başarıyla kaydedildi.";
        public static string AuthorUpdatedSuccessfully = "Yazar bilgileri başarıyla güncellendi.";
        public static string AuthorDeletedSuccessfully = "Yazar başarıyla silindi.";
        public static string AuthorAlreadyAdded = "Yazar sistemde zaten mevcut.";
        public static string NoActiveAuthorsFound = "Sistemde kullanılabilecek yazar bulunmamaktadır.Sistem yöneticinize danışınız.";
        public static string CanNotFindActiveAuthor = "Yazar detaylarına erişilemiyor. Hatalı id ya da yazar kullanımda değil.";
        public static string AuthorActivatedNotUpdated = "Güncelleme için girdiğiniz bilgiler sistemden daha önce silinmiş bir yazara ait. Güncelleme işlemi yerine silinen yazar tekrar sisteme dahil edildi ve artık sistemde kullanılabilir.";

        //Books
        public static string GetAllBooksSuccessfully = "Tüm kitaplar başarıyla listelendi.";
        public static string GetBookByIdSuccessfully = "Kitap detaylarına başarıyla ulaşıldı.";
        public static string BookAddedSuccessfully = "Kitap başarıyla kaydedildi.";
        public static string BookUpdatedSuccessfully = "Kitap bilgileri başarıyla güncellendi.";
        public static string GetAllBooksForAddToLibrarySuccessfully = "Tüm kitaplar kütüphane formatında listelenmiştir.";
        public static string GetBookByIdForAddToLibrarySuccessfully = "Kitap detayları kütüphane formatında listelenmiştir.";
        public static string GetBookForAddToLibraryByIsbnSuccessfully = "Isbn numarasına göre kitap kütüphane formatında getirilmiştir.";
        public static string GetBookForAddToLibraryByBookNameSuccessfully = "Kitap adına göre kitaplar kütüphane formatında listelenmiştir.";
        public static string GetBookForAddToLibraryByPublisherIdSuccessfully = "Yayınevine göre kitaplar kütüphane formatında listelenmiştir.";
        public static string GetBookForAddToLibraryByAuthorIdSuccessfully = "Yazara göre kitaplar kütüphane formatında başarıyla listelenmiştir.";
        public static string GetBooksForAddToLibraryListByNativeStatueSuccessfully = "Yerli / yabancı seçiminize göre kitap listesi başarıyla listelenmiştir.";
        public static string GetBookForAddToLibraryByGenreIdSuccessfully = "Türe göre kitaplar kütüphane formatında başarıyla listelenmiştir.";
        public static string BookAddedAlreadyBefore = "Kitap sistemde zaten mevcut.";
        public static string CanNotFindAnyBook = "Sistemde kullanılabilir herhangi bir kitap bulunmamaktadır.";
        public static string WrongBookId = "Kitap detaylarına ulaşılamıyor. Hatalı id.";
        public static string WrongIsbnNumber = "ISBN numarası hatalı ya da belirtilen ISBN numarasına sahip sistemde mevcut değil.";
        public static string WrongBookName = "Hatalı kitap adı ya da belirtilen isimde bir kitap sistemde mevcut değil.";
        public static string WrongPublisher = "Sistemde belirtilen yayınevine ait kitap mevcut değil.";
        public static string WrongAuthor = "Sistemde belirtilen yazara ait kitap mevcut değil.";
        public static string NoBookByThisNativeSelection = "Mevcut yerli / yabancı seçiminize göre sistemde kitap mevcut değil.";
        public static string WrongGenre = "Sistemde belirtilen türde kitap mevcut değil.";

        //Genres
        public static string GetAllGenresSuccessfully = "Tüm türler başarıyla listelendi.";
        public static string GetGenreByIdSuccessfully = "Tür detaylarına başarıyla ulaşıldı.";
        public static string GenreAddedSuccessfully = "Tür başarıyla kaydedildi.";
        public static string GenreUpdatedSuccessfully = "Tür bilgileri başarıyla güncellendi.";
        public static string GenreDeletedSuccessfully = "Tür başarıyla silindi.";
        public static string GenreAlreadyAdded = "Tür sistemde zaten mevcut.";
        public static string WrongGenreIdOrClaimNotActive = "Tür detaylarına ulaşılamıyor. Hatalı id ya da tür kullanımda değil";
        public static string NoActiveGenreFound = "Sistemde kullanılabilecek tür bulunmamaktadır.Sistem yöneticinize danışınız.";
        public static string GenreActivatedNotUpdated = "Güncelleme için girdiğiniz bilgiler sistemden daha önce silinmiş bir türe ait. Güncelleme işlemi yerine silinen tür tekrar sisteme dahil edildi ve artık sistemde kullanılabilir.";


        //Operation Claims
        public static string GetAllOperationClaimsSuccessfully = "Tüm yetkiler başarıyla listelendi.";
        public static string ClaimAddedSuccessfully = "Yetki başarıyla eklendi.";
        public static string GetOperationClaimByNameSuccessfully = "Ada göre yetki detayları başarıyla getirildi.";
        public static string PredefinedClaimsListedSuccessfully = "Öntanımlı yetkiler başarıyla listelendi.";
        public static string GetClaimByIdSuccessfully = "Yetki detayları başarıyla getirildi.";
        public static string ClaimDeletedSuccessfully = "Yetki hem yetkiler tablosundan hem de tüm kullanıcılardan başarıyla silindi.";
        public static string OperationClaimAlreadyAdded = "Yetki sistemde zaten mevcut.";
        public static string ClaimNotFoundOrNotActive = "Yetki sistemde yok ya da aktif değil.";
        public static string OperationClaimWrongIdOrClaimNotActive = "Rol detaylarına ulaşılamıyor. Hatalı id ya da rol kullanımda değil.";
        public static string CanNotDeleteUserOrAdminRole = " Sistemin kullanılması için gerekli olan user veya admin rollerini sistemden silemezsiniz.";
        public static string DeletedRoleDeletedByUserAtTheSameTime="Silinen rol tüm kullanıcılardan da başarıyla silinmiştir.";


        //Publishers
        public static string GetAllPublishersSuccessfully = "Tüm yayınevleri başarıyla listelendi.";
        public static string GetPublisherByIdSuccessfully = "Yayınevi detayları başarıyla getirildi.";
        public static string PublisherAddedSuccessfully = "Yayınevi başarıyla eklendi.";
        public static string UpdatedPublisherSuccessfully = "Yayınevi bilgileri başarıyla güncellendi.";
        public static string DeletePublisherSuccessfully = "Yayınevi başarıyla silindi.";
        public static string PublisherAlreadyAdded = "Yayınevi sistemde zaten mevcut.";
        public static string NoActivePubliserFound = "Sistemde kullanılabilecek yayınevi bulunmamaktadır.Sistem yöneticinize danışınız.";
        public static string PublisherWrongIdOrPublisherNotActive = "Yayınevi detaylarına ulaşılamıyor. Hatalı id ya da yayınevi kullanımda değil.";
        public static string PublisherActivatedNotUpdated = "Güncelleme için girdiğiniz bilgiler sistemden daha önce silinmiş bir yayınevine ait. Güncelleme işlemi yerine silinen yayınevi tekrar sisteme dahil edildi ve artık sistemde kullanılabilir.";

        //UserBooks
        public static string GetUsersAllBooksSuccessfully = "Tüm kitaplarınız başarıyla listelendi.";
        public static string GetUsersAllBooksWichHasNoteSuccessfully = "Not eklediğiniz tüm kitaplarınız başarıyla listelendi.";
        public static string GetUsersAllBooksByPublisherId = "Yayınevine göre tüm kitaplarınız başarıyla listelendi.";
        public static string GetUsersAllBooksByAuthorId = "Yazara göre tüm kitaplarınız başarıyla listelendi.";
        public static string GetUserBooksByNativeStatueSuccessfully = "Yerli / Yabancı seçiminize göre kitaplarınız başarıyla listelenmiştir.";
        public static string GetUsersAllBookByGenreIdSuccessfully = "Türe göre tüm kitaplarınız başarıyla getirildi.";
        public static string GetUsersAllBookByReadStatueSuccessfully = "Okuma durumunuza göre kitap listeniz başarıyla listelendi.";
        public static string UserBookAddedSuccessfully = "Kitap kütüphanenize başarıyla eklendi.";
        public static string UserBookUpdatedSuccessfully = "Kitap detaylarınız başarıyla güncelleştirildi.";
        public static string UserBookDeletedSuccessfully = "Kitap kütüphanenizden başarıyla silinmiştir.";
        public static string GetAllUserBookEntitiesSuccessfully = "Kullanıcı kitapları başarıyla listelenmiştir.";
        public static string AllUserBookDeletedSuccessfully = "Kullanıcının kütüphanesi başarıyla silinmiştir.";
        public static string UserBookAddedAlready = "Kitap kütüphanenizde zaten mevcut.";
        public static string NoBookHasNote = "Kütüphanenizde not içeren kitabınız bulunmamaktadır.";
        public static string ThereAreNoUserBooks = "Kütüphanenizde hiç kitap bulunmmaktadır.";
        public static string NoUserBookFoundByThisPublisherId = "Kütüphanenizde bu yayınevine ait kitap bulunmamaktadır.";
        public static string NoUserBookFoundByThisAuthorId = "Kütüphanenizde bu yazara ait kitap bulunmamaktadır.";
        public static string NoUserBookFoundByThisNativeStatue = "Kütüphanenizde yerli/yabancı seçiminize uygun kitap bulunmamaktadır.";
        public static string NoUserBookFoundByThisGenreId = "Kütüphanenizde bu türe ait kitap bulunmamaktadır.";
        public static string NoUserBookFoundByThisReadStatue = "Kütüphanenizde belirttiğiniz okuma durumuna göre kitap bulunmamaktadır.";
        public static string CanNotFindUserBook = "Kütüphanenizdeki kitap detaylarına erişilemiyor.";

        //UserOperationClaims
        public static string UserRoleSuccessfullyAddedToUser = "Yeni kullanıcıya yetkisi başarıyla eklendi.";
        public static string UserOperationClaimAddedSuccessfully = "Kullanıcıya yetkisi başarıyla eklenmiştir.";
        public static string UserOperationClaimUpdatedSuccessfully = "Kullanıcını yetkisi başarıyla güncellenmiştir.";
        public static string UserOperationClaimDeletedSuccessfully = "Kullanıcı yetkisi başarıyla silinmiştir.";
        public static string UserOperationClaimDeletedSuccessfullyByUser = "Kullanıcının yetkileri kullanıcı isteğiyle başarıyla silinmiştir.";
        public static string UserOperationClaimNotFoundById = "Belirtilen rol detaylarına ulaşılamıyor.";
        public static string UserHasTheRoleAlready = "Kullanıcı zaten bu role sahip.";
        public static string NotFindAnyClaimByThisId = "Belirtilen rol kullanıcı rolleri listesinde bulunmuyor. Id hatalı ya da hiç bir kullanıcı bu role sahip değil.";
        public static string ClaimsListedByClaimId = "Rol detayına göre kullanıcı rolleri başarıyla listelendi.";

        //UserManager
        public static string GetUsersAllClaimsSuccessfully = "Kullanıcının tüm rolleri rol formatında getirildi.";
        public static string GetAllUserDetailsWitrRolesSuccessfully = "Tüm kullanıcıların detayları ve sahip olduğu roller başarıyla listelendi.";
        public static string GetUserDetailsWithRolesByUserIdSuccessfully = "Kullanıcının detayları ve sahip olduğu roller başarıyla listelendi.";
        public static string GetAllUsersSuccessfully = "Tüm kullanıcılar başarıyla listelendi.";
        public static string GetUserByEmailSuccessfully = "Mail adresine göre kullanıcı detayları başarıyla listelendi.";
        public static string GetUserByIdSuccessfully = "Kullanıcı detayları başarıyla getirildi.";
        public static string UserAddedSuccessfully = "Kaydınız başarıyla yapılmıştır.";
        public static string CurrentUserPasswordError = "Mevcut şifrenizi hatalı girdiniz.";
        public static string UserUpdatedSuccessfully = "Kullanıcı bilgileriniz başarıyla güncellendi.";
        public static string NewEmailAlreadyExists = "Kullanmak istediğiniz Email başka kullanıcı tarafından kullanılıyor.";
        public static string UserAndUsersBooksDeletedSuccessfullyByAdmin = "Kullanıcı, kullanıcının tüm yetkileri ve kütüphanesi yönetici tarafından başarıyla silinmiştir.";
        public static string UserAndUsersBooksAndUserClaimsDeletedSuccessfullyByUser = "Kullanıcıya ait tüm kayıtlar kullanıcı isteği ile silinmiştir.";
        public static string UserNotFoundByThisMail = "Girilen mail ile kayıtlı kullanıcı bulunmamaktadır.";
        public static string WrongUserId = "Kullanıcı detaylarına ulaşılamıyor.";
        public static string UserRoleMustBeAddedAndActive = "Sistemi kullanabilmeniz için gerekli olan kullanıcı yetkisi yönetici tarafından sisteme tanımlanmadığından işlem gerçekleştirilemiyor. Sistem yöneticisini bilgilendiriniz.";
        public static string UserHasNoActiveRole = "Kullanıcının aktif herhangi bir rolü bulunmamaktadır.";


        //BookValidator
        public static string IsbnNotValid = "ISBN numarası 13 karakter olmalı ve sadece rakamdan oluşmalıdır.";
        
    }
}
