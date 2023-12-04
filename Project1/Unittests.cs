using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq; // Add this for ToList()

// Test class for LibraryCatalog
[TestClass]
public class LibraryCatalogTests
{
    // Test adding a book to the catalog
    [TestMethod]
    public void TestAddBook()
    {
        // Arrange
        LibraryCatalog catalog = new LibraryCatalog();
        Book book = new Book { Title = "TestBook", Author = "TestAuthor", ISBN = "TestISBN", Genre = Genre.Fiction };

        // Act
        catalog.AddBook(book);

        // Assert
        Assert.IsTrue(catalog.Books.Contains(book));
    }

    // Test removing a book from the catalog
    [TestMethod]
    public void TestRemoveBook()
    {
        // Arrange
        LibraryCatalog catalog = new LibraryCatalog();
        Book book = new Book { Title = "TestBook", Author = "TestAuthor", ISBN = "TestISBN", Genre = Genre.Fiction };
        catalog.AddBook(book);

        // Act
        catalog.RemoveBook(book);

        // Assert
        Assert.IsFalse(catalog.Books.Contains(book));
    }

    // Test searching for a book in the catalog
    [TestMethod]
    public void TestSearchBook()
    {
        // Arrange
        LibraryCatalog catalog = new LibraryCatalog();
        Book book = new Book { Title = "TestBook", Author = "TestAuthor", ISBN = "TestISBN", Genre = Genre.Fiction };
        catalog.AddBook(book);

        // Act
        Book searchedBook = catalog.SearchBook("TestBook");

        // Assert
        Assert.AreEqual(book, searchedBook);
    }

    // Test filtering books by genre in the catalog
    [TestMethod]
    public void TestFilterByGenre()
    {
        // Arrange
        LibraryCatalog catalog = new LibraryCatalog();
        Book book1 = new Book { Title = "Book1", Author = "Author1", ISBN = "111111", Genre = Genre.Fiction };
        Book book2 = new Book { Title = "Book2", Author = "Author2", ISBN = "222222", Genre = Genre.Romance };
        catalog.AddBook(book1);
        catalog.AddBook(book2);

        // Act
        var filteredBooks = catalog.FilterByGenre(Genre.Romance).ToList(); // Fix: Convert to List

        // Assert
        CollectionAssert.Contains(filteredBooks, book2);
        CollectionAssert.DoesNotContain(filteredBooks, book1);
    }

    // Test the iterator in the catalog
    [TestMethod]
    public void TestIterator()
    {
        // Arrange
        LibraryCatalog catalog = new LibraryCatalog();
        Book book1 = new Book { Title = "Book1", Author = "Author1", ISBN = "111111", Genre = Genre.Fiction };
        Book book2 = new Book { Title = "Book2", Author = "Author2", ISBN = "222222", Genre = Genre.Romance };
        catalog.AddBook(book1);
        catalog.AddBook(book2);

        // Act
        IIterator iterator = catalog.GetIterator();
        var firstBook = iterator.First();

        // Assert
        Assert.AreEqual(book1, firstBook);
    }
}
