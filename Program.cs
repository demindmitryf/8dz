
using System;

class Song
{
    private string name;
    private string author;
    private Song prev;

    public Song()
    {
        name = "Неизвестно";
        author = "Неизвестен";
        prev = null;
    }

    public Song(string name, string author)
    {
        this.name = name;
        this.author = author;
        prev = null;
    }

    public Song(string name, string author, Song prev)
    {
        this.name = name;
        this.author = author;
        this.prev = prev;
    }

    public void Print()
    {
        Console.WriteLine("Название: " + name);
        Console.WriteLine("Автор: " + author);

        if (prev != null)
        {
            Console.WriteLine("Предыдущая песня: " + prev.name);
        }
        else
        {
            Console.WriteLine("Предыдущей песни нет.");
        }
    }

    public string Title()
    {
        return name + " - " + author;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is Song))
        {
            return false;
        }

        Song song = (Song)obj;

        return name == song.name && author == song.author;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(name, author);
    }
}

class Program
{
    static void Main()
    {
        Song song1 = new Song("Кукушка", "Виктор Цой");
        Song song2 = new Song("Группа крови", "Виктор Цой", song1);
        Song song3 = new Song("Звезда по имени Солнце", "Виктор Цой", song2);
        Song song4 = new Song("Перемен", "Виктор Цой", song3);

        Song mySong = new Song();

        Console.WriteLine("Список песен:");
        Console.WriteLine();

        song1.Print();
        Console.WriteLine();

        song2.Print();
        Console.WriteLine();

        song3.Print();
        Console.WriteLine();

        song4.Print();
        Console.WriteLine();

        Console.WriteLine("Название первой песни: " + song1.Title());

        Console.WriteLine();

        if (song1.Equals(song2))
        {
            Console.WriteLine("Первые две песни одинаковые.");
        }
        else
        {
            Console.WriteLine("Первые две песни разные.");
        }

        Console.ReadKey();
    }
}
