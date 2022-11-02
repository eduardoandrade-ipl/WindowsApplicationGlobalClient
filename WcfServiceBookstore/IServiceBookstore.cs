using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfServiceBookstore
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IServiceBookstore
    {
        [OperationContract]
        void AddBook(Book newBook);

        [OperationContract]
        List<Book> GetBooks();

        [OperationContract]
        List<Book> GetBookByCategory(BookCategory category);

        [OperationContract]
        Book GetBookByTitle(string title);

        [OperationContract]
        bool DeleteBook(string title);

        [OperationContract]
        Book[] GetBooksByTitle(string title);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public double Price { get; set; }
        public BookCategory Category { get; set; }

        public Book(string title, string author, int year, double price, BookCategory category)
        {
            this.Title = title;
            this.Author = author;
            this.Year = year;
            this.Price = price;
            this.Category = category;
        }

        public Book()
        {
        }
    }

    public enum BookCategory
    {
        WEB,
        CHILDREN,
        SCIENCE,
        ROMANCE,
        BIOGRAPHIES,
        OTHER
    }
}
