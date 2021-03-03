using OxfordBookShoppe.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OxfordBookShoppe.Data.Services
{
    public interface IBookData
    {
        IEnumerable<Book> GetAll();
        
        Book GetById(int id);
        
        void Add(Book book);
       
        void Update(Book book);
        void Delete(int id);
       
    }
}
