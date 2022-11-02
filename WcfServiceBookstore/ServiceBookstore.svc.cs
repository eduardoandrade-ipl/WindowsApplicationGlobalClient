using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;

namespace WcfServiceBookstore
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IServiceBookstore
    {
        string FILEPATH = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"App_Data\bookstore.xml";
        public void AddBook(Book newBook)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(FILEPATH);

            XmlNode bookstoreNode = doc.SelectSingleNode("/bookstore");

            XmlElement bookNode = doc.CreateElement("book");
            bookNode.SetAttribute("category", newBook.Category.ToString());
            XmlElement titleNode = doc.CreateElement("title");
            titleNode.InnerText = newBook.Title;
            bookNode.AppendChild(titleNode);
            XmlElement authorNode = doc.CreateElement("author");
            authorNode.InnerText = newBook.Author;
            bookNode.AppendChild(authorNode);
            XmlElement yearNode = doc.CreateElement("year");
            yearNode.InnerText = newBook.Year.ToString();
            bookNode.AppendChild(yearNode);
            XmlElement priceNode = doc.CreateElement("price");
            priceNode.InnerText = Convert.ToString(newBook.Price, NumberFormatInfo.InvariantInfo);
            bookNode.AppendChild(priceNode);

            bookstoreNode.AppendChild(bookNode);

            doc.Save(FILEPATH);
        }

        public bool DeleteBook(string title)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(FILEPATH);

            //XmlNode bookstoreNode = doc.SelectSingleNode("/bookstore");
            XmlNode bookNode = doc.SelectSingleNode("/bookstore/book[title='" + title + "']");

            if (bookNode != null)
            {
                XmlNode bookstoreNode = bookNode.ParentNode;
                bookstoreNode.RemoveChild(bookNode);
                doc.Save(FILEPATH);
                return true;
            }
            else return false;
        }

        public List<Book> GetBookByCategory(BookCategory category)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(FILEPATH);

            XmlNodeList bookNodes = doc.SelectNodes("/bookstore/book[@category='" + category.ToString() + "']");

            List<Book> books = new List<Book>();
            foreach (XmlNode bookNode in bookNodes)
            {
                string titleNode = bookNode["title"].InnerText;
                string authorNode = bookNode["author"].InnerText;
                XmlNode yearNode = bookNode.SelectSingleNode("year");
                XmlNode priceNode = bookNode.SelectSingleNode("price");
                XmlAttribute categoryNode = bookNode.Attributes["category"];

                Book book = new Book(
                    titleNode,
                    authorNode,
                    Convert.ToInt32(yearNode.InnerText),
                    Convert.ToDouble(priceNode.InnerText, NumberFormatInfo.InvariantInfo),
                    (BookCategory)Enum.Parse(typeof(BookCategory), categoryNode.Value)
                );

                books.Add(book);
            }

            return books;
        }

        public Book GetBookByTitle(string title)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(FILEPATH);

            XmlNode titleNode = doc.SelectSingleNode("/bookstore/book[title='" + title + "']");

            if (titleNode == null)
            {
                return null;
            }
            else
            {
                string authorNode = titleNode["author"].InnerText;
                string yearNode = titleNode["year"].InnerText;
                string priceNode = titleNode["price"].InnerText;
                string categoryNode = titleNode.Attributes["category"].Value;

                Book book = new Book
                {
                    Title = titleNode["title"].InnerText,
                    Author = authorNode,
                    Year = Convert.ToInt32(yearNode),
                    Price = Convert.ToDouble(priceNode, NumberFormatInfo.InvariantInfo),
                    Category = (BookCategory)Enum.Parse(typeof(BookCategory), categoryNode)
                };

                return book;
            }
        }

        public List<Book> GetBooks()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(FILEPATH);

            List<Book> books = new List<Book>();

            XmlNodeList bookNodes = doc.SelectNodes("/bookstore/book");

            foreach (XmlNode bookNode in bookNodes)
            {
                string titleNode = bookNode["title"].InnerText;
                string authorNode = bookNode["author"].InnerText;
                XmlNode yearNode = bookNode.SelectSingleNode("year");
                XmlNode priceNode = bookNode.SelectSingleNode("price");
                XmlNode categoryNode = bookNode.Attributes["category"];

                Book book = new Book();
                book.Title = titleNode;
                book.Author = authorNode;
                book.Year = Convert.ToInt32(yearNode.InnerText);
                book.Price = Convert.ToDouble(priceNode.InnerText, NumberFormatInfo.InvariantInfo);
                book.Category = (BookCategory)Enum.Parse(typeof(BookCategory), categoryNode.Value);

                books.Add(book);
            }

            return books;
        }

        public Book[] GetBooksByTitle(string title)
        {
            List<Book> books = new List<Book>();
            XmlDocument doc = new XmlDocument();
            doc.Load(FILEPATH);

            XmlNodeList titleNodes = doc.SelectNodes("/bookstore/book[contains(title,'" + title + "')]");

            if (titleNodes.Count != 0)
            {
                foreach (XmlNode titleNode in titleNodes)
                {
                    string authorNode = titleNode["author"].InnerText;
                    string yearNode = titleNode["year"].InnerText;
                    string priceNode = titleNode["price"].InnerText;
                    string categoryNode = titleNode.Attributes["category"].Value;

                    Book book = new Book
                    {
                        Title = titleNode["title"].InnerText,
                        Author = authorNode,
                        Year = Convert.ToInt32(yearNode),
                        Price = Convert.ToDouble(priceNode, NumberFormatInfo.InvariantInfo),
                        Category = (BookCategory)Enum.Parse(typeof(BookCategory), categoryNode)
                    };
                    books.Add(book);
                }
                return books.ToArray();
            }
            return null;
        }
    }
}
