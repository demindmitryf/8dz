
using System;
using System.Collections.Generic;

enum ProjectStatus
{
    Проект,
    Исполнение,
    Закрыт
}

enum TaskStatus
{
    Назначена,
    ВРаботе,
    НаПроверке,
    Выполнена
}

class Person
{
    public string Name { get; set; }

    public Person(string name)
    {
        Name = name;
    }
}

class Report
{
    public string Text { get; set; }
    public DateTime Date { get; set; }
    public Person Executor { get; set; }

    public Report(string text, Person executor)
    {
        Text = text;
        Executor = executor;
        Date = DateTime.Now;
    }
}

class TaskItem
{
    public int Number { get; set; }
    public string Description { get; set; }
    public DateTime Deadline { get; set; }
    public Person Initiator { get; set; }
    public Person Executor { get; set; }
    public TaskStatus Status { get; set; }
    public Report Report { get; set; }

    public TaskItem(int number, string description,
        DateTime deadline, Person initiator, Person executor)
    {
        Number = number;
        Description = description;
        Deadline = deadline;
        Initiator = initiator;
        Executor = executor;
        Status = TaskStatus.Назначена;
        Report = null;
    }

    public void PrintInfo()
    {
        Console.WriteLine("Номер задачи: " + Number);
        Console.WriteLine("Описание: " + Description);
        Console.WriteLine("Срок: " + Deadline.ToShortDateString());
        Console.WriteLine("Инициатор: " + Initiator.Name);

        if (Executor != null)
        {
            Console.WriteLine("Исполнитель: " + Executor.Name);
        }
        else
        {
            Console.WriteLine("Исполнитель не назначен.");
        }

        Console.WriteLine("Статус: " + Status);
    }
}

class Project
{
    public string Description { get; set; }
    public DateTime Deadline { get; set; }
    public Person Customer { get; set; }
    public Person TeamLeader { get; set; }
    public ProjectStatus Status { get; set; }
    public List<TaskItem> Tasks { get; set; }

    public Project(string description, DateTime deadline,
        Person customer, Person teamLeader)
    {
        Description = description;
        Deadline = deadline;
        Customer = customer;
        TeamLeader = teamLeader;
        Status = ProjectStatus.Проект;
        Tasks = new List<TaskItem>();
    }

    public void AddTask(string description, DateTime deadline,
        Person executor)
    {
        if (Status != ProjectStatus.Проект)
        {
            Console.WriteLine("Нельзя создавать задачи. Проект уже запущен.");
            return;
        }

        if (executor == null)
        {
            Console.WriteLine("Не указан исполнитель.");
            return;
        }

        TaskItem task = new TaskItem(
            Tasks.Count + 1,
            description,
            deadline,
            TeamLeader,
            executor
        );

        Tasks.Add(task);

        Console.WriteLine("Задача успешно создана.");
    }

    public void StartProject()
    {
        if (Status == ProjectStatus.Проект && Tasks.Count > 0)
        {
            Status = ProjectStatus.Исполнение;
            Console.WriteLine("Проект переведен в статус Исполнение.");
        }
        else
        {
            Console.WriteLine("Нельзя запустить проект.");
        }
    }

    public void CheckProject()
    {
        if (Tasks.Count == 0)
        {
            return;
        }

        foreach (TaskItem task in Tasks)
        {
            if (task.Status != TaskStatus.Выполнена)
            {
                return;
            }
        }

        Status = ProjectStatus.Закрыт;
        Console.WriteLine("Все задачи выполнены. Проект закрыт.");
    }

    public void PrintInfo()
    {
        Console.WriteLine("\nИнформация о проекте");
        Console.WriteLine("Описание: " + Description);
        Console.WriteLine("Заказчик: " + Customer.Name);
        Console.WriteLine("Тимлид: " + TeamLeader.Name);
        Console.WriteLine("Срок: " + Deadline.ToShortDateString());
        Console.WriteLine("Статус: " + Status);
        Console.WriteLine("Количество задач: " + Tasks.Count);
    }
}

class Program
{
    static void Main()
    {
        List<Person> team = new List<Person>();

        team.Add(new Person("Александр"));
        team.Add(new Person("Иван"));
        team.Add(new Person("Дмитрий"));
        team.Add(new Person("Сергей"));
        team.Add(new Person("Андрей"));
        team.Add(new Person("Максим"));
        team.Add(new Person("Никита"));
        team.Add(new Person("Михаил"));
        team.Add(new Person("Егор"));
        team.Add(new Person("Артем"));

        Person teamLeader = team[0];
        Person customer = new Person("Заказчик");

        Project project = new Project(
            "Разработка информационной системы",
            DateTime.Now.AddMonths(2),
            customer,
            teamLeader
        );

        Console.WriteLine("TASK MANAGER");
        Console.WriteLine();

        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine("Создание задачи для " + team[i].Name);

            Console.Write("Введите описание задачи: ");
            string description = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(description))
            {
                description = "Выполнение задачи " + (i + 1);
            }

            project.AddTask(
                description,
                DateTime.Now.AddDays(14),
                team[i]
            );

            Console.WriteLine();
        }

        project.StartProject();

        foreach (TaskItem task in project.Tasks)
        {
            Console.WriteLine("\n----------------------------");
            task.PrintInfo();

            while (task.Status == TaskStatus.Назначена)
            {
                Console.WriteLine("\nДействия:");
                Console.WriteLine("1 - Принять задачу");
                Console.WriteLine("2 - Отклонить задачу");
                Console.WriteLine("3 - Делегировать задачу");

                Console.Write("Выберите действие: ");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    task.Status = TaskStatus.ВРаботе;
                    Console.WriteLine("Задача принята в работу.");
                }
                else if (choice == "2")
                {
                    Console.WriteLine("Задача отклонена.");
                    task.Executor = null;

                    Console.WriteLine("Тимлид должен назначить другого исполнителя.");

                    for (int i = 0; i < team.Count; i++)
                    {
                        Console.WriteLine((i + 1) + " - " + team[i].Name);
                    }

                    Console.Write("Выберите нового исполнителя: ");
                    int number;

                    if (int.TryParse(Console.ReadLine(), out number)
                        && number >= 1 && number <= team.Count)
                    {
                        task.Executor = team[number - 1];
                        task.Status = TaskStatus.Назначена;
                    }
                    else
                    {
                        Console.WriteLine("Неверный номер. Назначаем первого участника.");
                        task.Executor = team[0];
                        task.Status = TaskStatus.Назначена;
                    }
                }
                else if (choice == "3")
                {
                    Console.WriteLine("Выберите нового исполнителя:");

                    for (int i = 0; i < team.Count; i++)
                    {
                        Console.WriteLine((i + 1) + " - " + team[i].Name);
                    }

                    Console.Write("Номер сотрудника: ");
                    int number;

                    if (int.TryParse(Console.ReadLine(), out number)
                        && number >= 1 && number <= team.Count)
                    {
                        task.Executor = team[number - 1];
                        task.Status = TaskStatus.Назначена;

                        Console.WriteLine("Задача делегирована сотруднику "
                            + task.Executor.Name);
                    }
                    else
                    {
                        Console.WriteLine("Неверный номер.");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный выбор.");
                }
            }

            if (task.Status == TaskStatus.ВРаботе)
            {
                Console.WriteLine("\nЗадача выполняется.");
                Console.WriteLine("Исполнитель: " + task.Executor.Name);

                Console.Write("Введите отчет о выполненной работе: ");
                string reportText = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(reportText))
                {
                    reportText = "Работа выполнена.";
                }

                task.Report = new Report(reportText, task.Executor);
                task.Status = TaskStatus.НаПроверке;

                Console.WriteLine("\nОтчет отправлен инициатору: "
                    + task.Initiator.Name);

                Console.WriteLine("Текст отчета: " + task.Report.Text);
                Console.WriteLine("Исполнитель: " + task.Report.Executor.Name);
                Console.WriteLine("Дата: " + task.Report.Date);

                Console.WriteLine("\nПроверка отчета:");
                Console.WriteLine("1 - Утвердить отчет");
                Console.WriteLine("2 - Вернуть на доработку");

                Console.Write("Выберите действие: ");
                string check = Console.ReadLine();

                if (check == "1")
                {
                    task.Status = TaskStatus.Выполнена;
                    Console.WriteLine("Отчет утвержден. Задача выполнена.");
                }
                else
                {
                    Console.WriteLine("Отчет возвращен на доработку.");

                    Console.Write("Введите исправленный отчет: ");
                    string newText = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(newText))
                    {
                        task.Report.Text = newText;
                        task.Report.Date = DateTime.Now;
                    }

                    Console.WriteLine("Отчет отправлен повторно.");
                    Console.WriteLine("1 - Утвердить отчет");
                    Console.WriteLine("2 - Вернуть на доработку");

                    Console.Write("Выберите действие: ");
                    check = Console.ReadLine();

                    if (check == "1")
                    {
                        task.Status = TaskStatus.Выполнена;
                        Console.WriteLine("Отчет утвержден. Задача выполнена.");
                    }
                    else
                    {
                        Console.WriteLine("Задача осталась на проверке.");
                    }
                }
            }

            project.CheckProject();
        }

        Console.WriteLine("\n============================");
        Console.WriteLine("ИТОГИ РАБОТЫ");
        project.PrintInfo();

        Console.WriteLine("\nСписок задач:");

        foreach (TaskItem task in project.Tasks)
        {
            Console.WriteLine("\n----------------------------");
            task.PrintInfo();
        }

        Console.ReadKey();
    }
}
