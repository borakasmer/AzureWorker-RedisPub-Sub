using System.Collections.Generic;

public interface IServiceResponse<T>
    {
        //“List” : Kayıt kümesi dönüleceği zaman kullanılır. Örneğin bir sayfa üzerindeki tüm yetkiler liste şeklinde bu property’e atanabilir.
        IList<T> List { get; set; }

        //“Entity” : Tek bir kaydın dönülmesi durumunda, kullanılır.
        T Entity { get; set; }

        //“Count” : Dönen toplam kayıt sayısıdır.
        int Count { get; set; }

        //“IsSuccessful” : İşlemin başarılma durumunu gösterir.Insert, Update, Delete lerdeki success. Bu property'e Mobil tarafta da ihtiyaç duyulmakta.
        bool IsSuccessful { get; set; }
        string ExceptionMessage { get; set; }
    }