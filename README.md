1.	Saga pattern mikroservis mimarisinde hangi sorunları çözmeye çalışır?
Mikroservis mimarisinde birbirinden bağımsız birçok servis bir arada çalışır. Bir işlem başlatıldığı zaman işlemi gerçekleştirmek için gerekli olan servisler tetiklenir ve hepsi kendi başına görevini yapar. Bu işlem sırasında çalışan 5 tane servisten herhangi birisinde hata olur ise işlemin gerçekleşmemesi ve servislerin yaptığı işlemlerin geri alınması gerekir. Aksi takdirde servisler arasında ciddi veri tutarsızlıkları yaşanabilir. Saga Pattern bir işlem başlatıldığı zaman servislerin belli bir iş akışı içerisinde birbirini dinleyerek sırayla çalışmasını ve olası bir failover durumunda servislerin yaptığı işlemleri geri alarak veri tutarsızlıklarının önüne geçmemizi sağlar. Servisler birbirini dinleyerek sırayla çalıştığı için hata alındığı zaman bu hatanın tespiti daha kolay olur. Servislerin sırayla çalışması aynı anda çalışmasına göre daha performanslıdır Saga Pattern bu sayede uygulamaya performans kazandırır. Birden fazla servis aynı anda çalıştığı zaman aynı veriyi aynı anda değiştirmeye çalışabilir bu da beklenmeyen sorunlara yol açabilir. Yani kısaca toparlamak gerekir ise Saga Pattern servislerin birbirini dinleyerek sıra ile çalışmasını ve bir hata durumunda gerçekleşen işlemlerin geri alınması veya düzeltilmesi aksiyonlarını gerçekleştirerek, veri tutarsızlığının önüne geçilmesi, hata yönetiminin ve tespitinin kolaylaşması, uygulama performansının artması, uzun işlem akışlarının daha yönetilebilir olması gibi birçok fayda sağlamaktadır.

2.	Saga patterndeki choreography ve orchestration yaklaşımları arasındaki temel fark nedir?
Saga Pattern iki farklı yaklaşım ile uygulanabilir. Orchestration-Based Saga ve Choreography-Based Saga. Orchestration yaklaşımında işlemleri yöneten merkezi bir servis (orchestrator) vardır. Orchestrator bir işlemin gerçekleşmesi için gerekli olan iş akışını bilir ve servisleri sırasıyla çalıştırır. İş akışı A servisi ile başlar, A servisi olumlu sonuç döndürür ise bu bilgiyi orchestartora gönderir ve orchestrator sıradaki B servisini çalıştırır, B servisi olumsuz sonuç döndürür ise bunu orchestratora gönderir ve orchestrator iş akışını durdurup A ve B servisi için rollback senaryolarını başlatır. Choreography yaklaşımında merkezi bir yönetici servis bulunmaz. Servisler işlem gerçekleştikten sonra işlemin gerçekleştiğine dair bir event çalıştırır. A servisi işlem başarılı olur ise A_BASARILI eventini çalıştırır ve bu eventi B servisi dinler bu sayede B servisi tetiklenmiş olur ve B servisi çalışır. B servisi başarısız olur ise B_BASARISIZ eventi çalışır, bu eventi A servisi dinler ve rollback için gerekli aksiyonları başlatır. Sonuca gelirsek Orchestration yaklaşımında işlem sırasını merkezi bir kontrol belirler, Choreography yaklaşımında ise servisler birbirini dinleyerek kendi başlarına hareket eder.

3.	Orchestration Saga pattern avantajları ve dezavantajları nelerdir?
Distributed transaction yönetimini merkezileştirir. +
İşlem süreci merkezi bir katmanda (orchestrator) yönetildiği için süreçleri izlemeyi kolaylaştırır. +
Tüm adımlar merkezi bir noktada gözlemlenebildiği için hata yönetimini kolaylaştırır. +
Rollback yönetimi basittir. +
Servisler orchestratora bağımlı olduğu için orchestartorda hata alınması durumunda tüm süreç etkilenir. –
Çok sayıda mikroservisin bulunduğu bir sistemde bu süreci yönetmek daha karmaşık hale gelebilir. –

4.	.NET Unit Test
Projelerde 4 farklı test süreci vardır ve bunlardan birisi unit testtir. Unit test uygulamadaki metotları test ettiğimiz süreçtir.  Unit test yazmak Arrange, Act ve Assert olmak üzere üç aşamadan oluşur. Arrange aşamasında test edilecek metodun kullanacağı değişkenler tanımlanır, nesneler oluşturulur. Act aşamasında test edilecek metodu çalıştırırız. Assert aşaması ise Act aşamasında alınan sonuçların doğrulandığı kısımdır.

5.	Xunit Kütüphanesi
Xunit, C# dilinde birim testleri yazmak için sıkça kullanılan bir kütüphanedir. Bize birçok test fonksiyonu ve kavram kazandırır.

[Fact]
Parametre almayacak test senaryolarında kullanırız.

[Theory]
Dinamik test senaryoları için kullanılır. InlineData ile farklı veri setlerini aynı test senaryosunda çalıştırabiliriz.

Assert
Testlerin doğrulunu kontrol etmek için kullanılır. Bir testin başarılı olup olmadığını belirlemek için Assert metotlarını kullanırız. En sık kullanılan assert işlemleri;

Assert.Equal(expected, actual)
Beklenen değer ile gerçek değer karşılaştırılır. Eğer değerlerler eşit değilse test başarısız olur.

Assert.Same(expectedObject, actualObject)
Bu metod iki nesnenin aynı referansa sahip olup olmadığını kontrol eder.

Assert.True(condition) Assert.False(condition)
Bu metodlar belirli bir koşulun doğru ya da yanlış olup olmadığını kontrol eder.

Assert.Null(object) Assert.NotNull(object)
Bu metodlar bir nesnenin null olup olmadığını kontrol etmek için kullanılır.

Assert.Throws<TException>(action)
Bu metod bir eylemin beklenen türde exception getirip getirmediğini kontrol eder.

6.	Moq Kütüphanesi
Mock kavramı istediğimiz bir nesnenin yerine geçebilen fake nesnelerdir. Bu objelerin istediğimiz gibi davranmalarını sağlayabiliriz. Mocklanan bir nesne dış bağımlılıklardan izole olur ve bu sayede sadece o nesnenin çalışma mantığını test etmiş oluruz. Örneğin bir CRUD işlemini test ederken nesneleri ve veritabanını mocklamazsak veritabanında değişiklikler meydana gelecektir, fakat bu işlemleri test amacıyla çalıştırdığımız için bunun olmasını istemeyiz. Ayrıca veritabanında olan bir hata testin başarısız olmasına neden olabilir. Moq ile bunların önüne geçeriz.
