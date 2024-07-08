using BLL.Interfaces;
using DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UnitOfWorkService
    {
        public UnitOfWorkService(AppDbContext context)
        {
            BookService = new BookService(context);
        }
        IBookService BookService;
    }
}
