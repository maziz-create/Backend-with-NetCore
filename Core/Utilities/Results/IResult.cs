using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    //temel void komutları için başlangıç
    //mesela add void fonksiyonu çalıştıktan sonra işlem gerçekleşti mi, ne oldu ne bitti diye geri yanıt verilmesi gerekiyor...
    public interface IResult
    {
        bool Success { get; }
        string Message { get; }
    }
}
