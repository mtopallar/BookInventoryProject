using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Utilities.IoC
{
    //WebAPI Startup ya da Autofac de oluşturulan injectionları oluşturmamızı sağlar.
    public static class ServiceTool //Bu tool ile zincir dışında kalan uygulamalarda -zincir derken mesela web api business ı, business dal ı, çağırır ama mesela aspect bu zincirin dışındadır- dependency injection yapabiliyoruz. Bu tool u kullanmazsa zincir dışında kalan kısma bakılmayacağı için injectionlar çalışmayacaktır (asp.net web API onu göremez.). Service tool bizim injection altyapımızı okumaya yarıyan bir tool.
    {
        public static IServiceProvider ServiceProvider { get; private set; } //Bu satır ile CoreModule de instance ını oluşturduğumuz servis interfacelerinin somutlarını alıyoruz. Zincir dışındaki bir servisin enjeksiyonunu yapmak için zincir dışındaki sınıfın ctor unda örneğin ServiceTool.ServiceProvider.GetService<IMemoryCache>(); şeklinde çağırarak Modülde başlatılmış olan yani instance ı oluşturulmuş olan serviceCollection.AddMemoryCache(); servisinin somutuna erişiyoruz.

        public static IServiceCollection Create(IServiceCollection services) //.NET in servislerini (service collectionlarını) al ve onları kendin build et. Bu kod webAPI ya da autofac de oluşturulan injectionları oluşturabilmemize yarar. Mesela bu tool ile istediğimiz herhangi bir interface in servisteki karşılığını bu tool ile alabiliyoruz.
        {
            ServiceProvider = services.BuildServiceProvider();
            return services;
        }
    }

    // IServiceCollections zaten Microsoft.Extensions.DependencyInjection dan geliyor.
    
}

//** Bu tool ile Service Collection nesnesinin arkasındaki ServiceProvider nesnesine erişiyoruz. ServiceProvider bizim çin merkezi servis ynetim nesnesi olacak. .NETCore kendi içindeki interfaceleri bildiği ve kendiliğinden enjekte edebildiği için (IServiceCollection gibi) bu interfaceleri direk kullanabiliyoruz. Bu interface in karşılığı budur dememiz gerekmiyor. Özetle biz bu Tool ile .NetCore un kendi servislerine erişebiliyoruz demek oluyor.
