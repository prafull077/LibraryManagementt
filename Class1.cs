using System;
using System.Collections.Generic;

// Book class
public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public bool Available { get; set; }

    public Book(string title, string author, string isbn)
    {
        Title = title;
        Author = author;
        ISBN = isbn;
        Available = true;
    }

    public override string ToString()
    {
        return $"Title: {Title}, Author: {Author}, ISBN: {ISBN}, Available: {Available}";
    }
}

// EBook class
public class EBook : Book
{
    public int FileSize { get; set; }

    public EBook(string title, string author, string isbn, string fileSizeStr) : base(title, author, isbn)
    {
        if (int.TryParse(fileSizeStr, out int fileSize))
        {
            FileSize = fileSize;
        }
        else
        {
            FileSize = 0;
            Console.WriteLine($"Invalid file size: {fileSizeStr}. Setting file size to 0 MB.");
        }
    }

    public override string ToString()
    {
        return base.ToString() + $", File Size: {FileSize} MB";
    }
}

// Library class
public class Library
{
    private List<Book> books;

    public Library()
    {
        books = new List<Book>();
    }

    public void AddBook(Book book)
    {
        books.Add(book);
    }

    public void RemoveBook(string isbn)
    {
        Book book = books.Find(b => b.ISBN == isbn);
        if (book != null)
        {
            books.Remove(book);
            Console.WriteLine($"Removed book: {book}");
        }
        else
        {
            Console.WriteLine($"Book with ISBN '{isbn}' not found.");
        }
    }

    public Book SearchByTitle(string title)
    {
        Book book = books.Find(b => b.Title == title);
        if (book != null)
        {
            return book;
        }
        else
        {
            Console.WriteLine($"Book with title '{title}' not found.");
            return null;
        }
    }

    public void ListAllBooks()
    {
        if (books.Count == 0)
        {
            Console.WriteLine("The library is empty.");
        }
        else
        {
            Console.WriteLine("All Books:");
            foreach (var book in books)
            {
                Console.WriteLine(book);
            }
        }
    }
}

// BookFactory class
public class BookFactory
{
    public static Book CreateBook(string title, string author, string isbn)
    {
        return new Book(title, author, isbn);
    }

    public static Book CreateEBook(string title, string author, string isbn, string fileSize)
    {
        return new EBook(title, author, isbn, fileSize);
    }
}

// LibraryManager class
public class LibraryManager
{
    private static LibraryManager instance;
    private Library library;

    private LibraryManager()
    {
        library = new Library();
    }

    public static LibraryManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LibraryManager();
            }
            return instance;
        }
    }

    public void Run()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("\nLibrary Management System");
            Console.WriteLine("1. Add Book");
            Console.WriteLine("2. Remove Book");
            Console.WriteLine("3. Search Book");
            Console.WriteLine("4. List All Books");
            Console.WriteLine("5. Exit");

            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddBook();
                    break;
                case "2":
                    RemoveBook();
                    break;
                case "3":
                    SearchBook();
                    break;
                case "4":
                    library.ListAllBooks();
                    break;
                case "5":
                    running = false;
                    Console.WriteLine("Exiting...");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private void AddBook()
    {
        Console.Write("Enter book title: ");
        string title = Console.ReadLine();
        Console.Write("Enter book author: ");
        string author = Console.ReadLine();
        Console.Write("Enter book ISBN: ");
        string isbn = Console.ReadLine();
        Console.Write("Is this an eBook? (y/n): ");
        string isEBook = Console.ReadLine();

        if (isEBook.ToLower() == "y")
        {
            Console.Write("Enter file size (in MB): ");
            string fileSizeStr = Console.ReadLine();
            Book book = BookFactory.CreateEBook(title, author, isbn, fileSizeStr);
            library.AddBook(book);
            Console.WriteLine($"Added eBook: {book}");
        }
        else
        {
            Book book = BookFactory.CreateBook(title, author, isbn);
            library.AddBook(book);
            Console.WriteLine($"Added book: {book}");
        }
    }

    private void RemoveBook()
    {
        Console.Write("Enter book ISBN to remove: ");
        string isbn = Console.ReadLine();
        library.RemoveBook(isbn);
    }

    private void SearchBook()
    {
        Console.Write("Enter book title to search: ");
        string title = Console.ReadLine();
        Book book = library.SearchByTitle(title);
        if (book != null)
        {
            Console.WriteLine(book);
        }
    }

    static void Main(string[] args)
    {
        LibraryManager.Instance.Run();
    }
}