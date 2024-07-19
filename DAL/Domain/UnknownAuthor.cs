using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Domain
{
    /// <summary>
    /// Неизвестный автор, если при создании книги не указать автора, по умолчанию будет этот автор
    /// </summary>
    static public class UnknownAuthor
    {
        public const string Name = "Неизвестный автор";
        public const string Id = "0bf3eaaa-107f-434e-85bc-49653b07515a";
    }
}
