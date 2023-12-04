﻿using System;
using System.Collections.Generic;
using static System.Reflection.Metadata.BlobBuilder;

// Single Responsibility Principle (SRP): Separating concerns into different classes

// Enumeration for Book Genre
public enum Genre
{
    Fiction,
    Mystery,
    ScienceFiction,
    Romance,
    NonFiction
}

// Book class representing a book in the library
public class Book
{
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public string ISBN { get; set; } = "";
    public Genre Genre { get; set; } = Genre.Fiction;

    // Change for first check-in (assuming commit) 
    public string TestString = "";

    public override string TestMethod()
    {
        return "nothingg";
    }

    public override string ToString()
    {
        return $"{Title} by {Author} (ISBN: {ISBN}, Genre: {Genre})";
    }
}

// Single Responsibility Principle (SRP): BookFormatter class for formatting books
public class BookFormatter
{
    public string FormatBook(Book book)
    {
        return $"{book.Title} by {book.Author} (ISBN: {book.ISBN}, Genre: {book.Genre})";
    }
}

// Interface Segregation Principle (ISP): Separating interfaces

// Iterator Interface
public interface IIterator
{
    Book First();
    Book Next();
    Book Previous();
    bool IsDone();
    Book CurrentItem();
}

// Catalog Interface
public interface ICatalog
{
    void AddBook(Book book);
    void RemoveBook(Book book);
    Book SearchBook(string title);
    IEnumerable<Book> FilterByGenre(Genre genre);
    IIterator GetIterator();
}

// Open/Closed Principle (OCP): Introducing a generic BookFilter

// Generic interface for book filters
public interface IBookFilter
{
    bool Filter(Book book);
}

// BookFilter implementation for filtering by genre
public class GenreFilter : IBookFilter
{
    private Genre genre;

    public GenreFilter(Genre genre)
    {
        this.genre = genre;
    }

    public bool Filter(Book book)
    {
        return book.Genre == genre;
    }
}

// LibraryCatalog class implementing the Catalog Interface
public class LibraryCatalog : ICatalog
{
    private List<Book> books = new List<Book>();
    private BookFormatter bookFormatter = new BookFormatter();

    public List<Book> Books => books; // Property to access books list

    public void AddBook(Book book)
    {
        books.Add(book);
    }

    public void RemoveBook(Book book)
    {
        books.Remove(book);
    }

    public Book SearchBook(string title)
    {
        return books.Find(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<Book> FilterByGenre(Genre genre)
    {
        // Open/Closed Principle (OCP): Using a generic filter
        IBookFilter filter = new GenreFilter(genre);
        return books.FindAll(book => filter.Filter(book));
    }

    public IIterator GetIterator()
    {
        return new BookIterator(this);
    }

    // Single Responsibility Principle (SRP): Separate formatting concern
    public string FormatBook(Book book)
    {
        return bookFormatter.FormatBook(book);
    }
}

// BookIterator class implementing the Iterator Interface
public class BookIterator : IIterator
{
    private LibraryCatalog catalog;
    private int current = 0;

    public BookIterator(LibraryCatalog catalog)
    {
        this.catalog = catalog;
    }
    public IEnumerable<Book> FilterByAuthor(string author)
    {
        return books.FindAll(book => book.Author.Equals(author, StringComparison.OrdinalIgnoreCase));
    }

    // Display books by a specific genre and author
    public IEnumerable<Book> FilterByGenreAndAuthor(Genre genre, string author)
    {
        // Open/Closed Principle (OCP): Using a generic filter for genre
        IBookFilter genreFilter = new GenreFilter(genre);

        return books.FindAll(book => genreFilter.Filter(book) && book.Author.Equals(author, StringComparison.OrdinalIgnoreCase));
    }
    public Book First()
    {
        current = 0;
        return catalog.Books.Count > 0 ? catalog.Books[current] : null;
    }

    public Book Next()
    {
        current++;
        return !IsDone() ? catalog.Books[current] : null;
    }

    public Book Previous()
    {
        current--;
        return current >= 0 ? catalog.Books[current] : null;
    }

    public bool IsDone()
    {
        return current >= catalog.Books.Count || current < 0;
    }

    public Book CurrentItem()
    {
        return catalog.Books.Count > 0 && current >= 0 && current < catalog.Books.Count ? catalog.Books[current] : null;
    }
}

// Main class for testing the project
class Program
{
    static void Main()
    {
        // Create a Library Catalog
        LibraryCatalog catalog = new LibraryCatalog();

        // Add books to the catalog
        catalog.AddBook(new Book { Title = "Book1", Author = "Author1", ISBN = "111111", Genre = Genre.Fiction });
        catalog.AddBook(new Book { Title = "Book2", Author = "Author2", ISBN = "222222", Genre = Genre.Romance });
        catalog.AddBook(new Book { Title = "Book3", Author = "Author3", ISBN = "333333", Genre = Genre.Mystery });
        catalog.AddBook(new Book { Title = "Book4", Author = "Author4", ISBN = "444444", Genre = Genre.ScienceFiction });
        catalog.AddBook(new Book { Title = "Book5", Author = "Author5", ISBN = "555555", Genre = Genre.NonFiction });
        // Search for a book
        Book searchedBook = catalog.SearchBook("Book2");
        Console.WriteLine($"Searched Book: {catalog.FormatBook(searchedBook)}");

        // Iterate through all books in the catalog
        IIterator iterator = catalog.GetIterator();
        Console.WriteLine("\nAll Books:");
        while (!iterator.IsDone())
        {
            Console.WriteLine(catalog.FormatBook(iterator.CurrentItem()));
            iterator.Next();
        }

        // Filter books by genre and print the result
        Genre filterGenre = Genre.Mystery;
        Console.WriteLine($"\nBooks in {filterGenre} genre:");
        foreach (Book book in catalog.FilterByGenre(filterGenre))
        {
            Console.WriteLine(catalog.FormatBook(book));
        }

        // Iterate backward through all books in the catalog
        Console.WriteLine("\nAll Books (Backward Iteration):");
        while (iterator.Previous() != null)
        {
            Console.WriteLine(catalog.FormatBook(iterator.CurrentItem()));
        }
    }
}
