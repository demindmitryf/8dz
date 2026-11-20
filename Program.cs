
using System;
using System.Collections;
using System.IO;

enum AccountType
{
    Текущий,
    Сберегательный
}

class BankTransaction
{
    private readonly DateTime date;
    private readonly decimal amount;

    public BankTransaction(decimal amount)
    {
        date = DateTime.Now;
        this.amount = amount;
    }

    public override string ToString()
    {
        return date + " | Сумма операции: " + amount;
    }
}

class BankAccount : IDisposable
{
    private int accountNumber;
    private decimal balance;
    private AccountType type;
    private Queue transactions;
    private bool disposed = false;

    public BankAccount()
    {
        balance = 0;
        type = AccountType.Текущий;
        transactions = new Queue();
        GenerateNumber();
    }

    public BankAccount(decimal balance)
    {
        this.balance = balance;
        type = AccountType.Текущий;
        transactions = new Queue();
        GenerateNumber();
    }

    public BankAccount(AccountType type)
    {
        balance = 0;
        this.type = type;
        transactions = new Queue();
        GenerateNumber();
    }

    public BankAccount(decimal balance, AccountType type)
    {
        this.balance = balance;
        this.type = type;
        transactions = new Queue();
        GenerateNumber();
    }

    private void GenerateNumber()
    {
        Random random = new Random();
        accountNumber = random.Next(100000, 999999);
    }

    public void Deposit(decimal amount)
    {
        if (amount > 0)
        {
            balance += amount;
            transactions.Enqueue(new BankTransaction(amount));
            Console.WriteLine("Деньги успешно добавлены.");
        }
        else
        {
            Console.WriteLine("Сумма должна быть больше нуля.");
        }
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Сумма должна быть больше нуля.");
        }
        else if (amount > balance)
        {
            Console.WriteLine("Недостаточно средств.");
        }
        else
        {
            balance -= amount;
            transactions.Enqueue(new BankTransaction(-amount));
            Console.WriteLine("Деньги успешно сняты.");
        }
    }

    public void PrintInfo()
    {
        Console.WriteLine("Номер счета: " + accountNumber);
        Console.WriteLine("Баланс: " + balance);
        Console.WriteLine("Тип счета: " + type);
    }

    public void Dispose()
    {
        if (!disposed)
        {
            string fileName = "Transactions_" + accountNumber + ".txt";

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (BankTransaction transaction in transactions)
                {
                    writer.WriteLine(transaction);
                }
            }

            disposed = true;
            GC.SuppressFinalize(this);

            Console.WriteLine("История операций записана в файл " + fileName);
        }
    }
}

class Program
{
    static void Main()
    {
        BankAccount account = new BankAccount(1000, AccountType.Текущий);

        account.PrintInfo();

        Console.WriteLine();

        account.Deposit(500);
        account.Withdraw(200);
        account.Deposit(300);

        Console.WriteLine();
        account.PrintInfo();

        account.Dispose();

        Console.ReadKey();
    }
}
