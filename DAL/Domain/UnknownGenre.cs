﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Domain
{
    /// <summary>
    /// Неизвестный жанр, если при создании книги не указать жанр, по умолчанию будет этот жанр
    /// </summary>
    public static class UnknownGenre
    {
        public const string Name = "Неизвестный жанр";
        public const string Id = "e5372338-ee97-408b-82c2-ab7e3ca6d145";
    }
}
