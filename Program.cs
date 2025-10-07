using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonStudentSystem
{
    // Базовий клас "Людина"
    public class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public int BirthMonth { get; set; }
        public int BirthYear { get; set; }

        public Person(string name, string surname, string patronymic, int birthMonth, int birthYear)
        {
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            BirthMonth = birthMonth;
            BirthYear = birthYear;
        }

        public int GetAge(int currentYear)
        {
            return currentYear - BirthYear;
        }

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"\nІм'я: {Name}");
            Console.WriteLine($"Прізвище: {Surname}");
            Console.WriteLine($"По-батькові: {Patronymic}");
            Console.WriteLine($"Місяць народження: {BirthMonth}");
            Console.WriteLine($"Рік народження: {BirthYear}");
        }

        // Метод для визначення кількості зустрічань літери в прізвищі
        public int CountLetterInSurname(char letter)
        {
            return Surname.ToLower().Count(c => c == char.ToLower(letter));
        }
    }

    // Похідний клас "Студент"
    public class Student : Person
    {
        public int AdmissionYear { get; set; }
        public string Specialty { get; set; }

        public Student(string name, string surname, string patronymic, 
                      int birthMonth, int birthYear, int admissionYear, string specialty)
            : base(name, surname, patronymic, birthMonth, birthYear)
        {
            AdmissionYear = admissionYear;
            Specialty = specialty;
        }

        public int GetStudentAge(int currentYear)
        {
            return currentYear - AdmissionYear;
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Рік вступу до ВУЗу: {AdmissionYear}");
            Console.WriteLine($"Спеціальність: {Specialty}");
        }
    }

    // Патерн Builder для створення об'єктів
    public class PersonBuilder
    {
        private string _name;
        private string _surname;
        private string _patronymic;
        private int _birthMonth;
        private int _birthYear;

        public PersonBuilder SetName(string name)
        {
            _name = name;
            return this;
        }

        public PersonBuilder SetSurname(string surname)
        {
            _surname = surname;
            return this;
        }

        public PersonBuilder SetPatronymic(string patronymic)
        {
            _patronymic = patronymic;
            return this;
        }

        public PersonBuilder SetBirthMonth(int month)
        {
            _birthMonth = month;
            return this;
        }

        public PersonBuilder SetBirthYear(int year)
        {
            _birthYear = year;
            return this;
        }

        public Person Build()
        {
            return new Person(_name, _surname, _patronymic, _birthMonth, _birthYear);
        }
    }

    public class StudentBuilder
    {
        private string _name;
        private string _surname;
        private string _patronymic;
        private int _birthMonth;
        private int _birthYear;
        private int _admissionYear;
        private string _specialty;

        public StudentBuilder SetName(string name)
        {
            _name = name;
            return this;
        }

        public StudentBuilder SetSurname(string surname)
        {
            _surname = surname;
            return this;
        }

        public StudentBuilder SetPatronymic(string patronymic)
        {
            _patronymic = patronymic;
            return this;
        }

        public StudentBuilder SetBirthMonth(int month)
        {
            _birthMonth = month;
            return this;
        }

        public StudentBuilder SetBirthYear(int year)
        {
            _birthYear = year;
            return this;
        }

        public StudentBuilder SetAdmissionYear(int year)
        {
            _admissionYear = year;
            return this;
        }

        public StudentBuilder SetSpecialty(string specialty)
        {
            _specialty = specialty;
            return this;
        }

        public Student Build()
        {
            return new Student(_name, _surname, _patronymic, _birthMonth, 
                             _birthYear, _admissionYear, _specialty);
        }
    }

    // Патерн Repository для управління колекціями
    public class PersonRepository
    {
        private List<Person> _people = new List<Person>();

        public void Add(Person person)
        {
            _people.Add(person);
        }

        public List<Person> GetAll()
        {
            return _people;
        }

        public Person GetByName(string name)
        {
            return _people.FirstOrDefault(p => p.Name == name);
        }
    }

    public class StudentRepository
    {
        private List<Student> _students = new List<Student>();

        public void Add(Student student)
        {
            _students.Add(student);
        }

        public List<Student> GetAll()
        {
            return _students;
        }

        public Student GetByName(string name)
        {
            return _students.FirstOrDefault(s => s.Name == name);
        }
    }

    // Головна програма
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== СИСТЕМА ОБЛІКУ ЛЮДЕЙ ТА СТУДЕНТІВ ===\n");

            // Створення репозиторіїв
            var personRepo = new PersonRepository();
            var studentRepo = new StudentRepository();

            // Створення об'єктів класу "Людина" використовуючи Builder
            var person1 = new PersonBuilder()
                .SetName("Іван")
                .SetSurname("Петренко")
                .SetPatronymic("Олександрович")
                .SetBirthMonth(3)
                .SetBirthYear(1985)
                .Build();

            var person2 = new PersonBuilder()
                .SetName("Марія")
                .SetSurname("Коваленко")
                .SetPatronymic("Василівна")
                .SetBirthMonth(7)
                .SetBirthYear(1990)
                .Build();

            personRepo.Add(person1);
            personRepo.Add(person2);

            // Створення об'єктів класу "Студент"
            var student1 = new StudentBuilder()
                .SetName("Олег")
                .SetSurname("Шевченко")
                .SetPatronymic("Іванович")
                .SetBirthMonth(9)
                .SetBirthYear(2003)
                .SetAdmissionYear(2021)
                .SetSpecialty("Комп'ютерні науки")
                .Build();

            var student2 = new StudentBuilder()
                .SetName("Анна")
                .SetSurname("Мельник")
                .SetPatronymic("Петрівна")
                .SetBirthMonth(12)
                .SetBirthYear(2004)
                .SetAdmissionYear(2022)
                .SetSpecialty("Програмна інженерія")
                .Build();

            studentRepo.Add(student1);
            studentRepo.Add(student2);

            // Виведення інформації про людей
            Console.WriteLine(">>> СПИСОК ЛЮДЕЙ:");
            foreach (var person in personRepo.GetAll())
            {
                person.DisplayInfo();
                Console.WriteLine($"Вік (станом на 2025 рік): {person.GetAge(2025)} років");
            }

            // Виведення інформації про студентів
            Console.WriteLine("\n>>> СПИСОК СТУДЕНТІВ:");
            foreach (var student in studentRepo.GetAll())
            {
                student.DisplayInfo();
                Console.WriteLine($"Вік студента (роки навчання): {student.GetStudentAge(2025)} років");
            }

            // Демонстрація підрахунку літери в прізвищі
            Console.WriteLine("\n>>> ПІДРАХУНОК ЛІТЕР У ПРІЗВИЩАХ:");
            Console.Write("\nВведіть літеру для підрахунку в прізвищах: ");
            char letterToCount = Console.ReadKey().KeyChar;
            Console.WriteLine(); // Новий рядок після введення
            
            Console.WriteLine($"\nЛітера '{letterToCount}' в прізвищах людей:");
            foreach (var person in personRepo.GetAll())
            {
                int count = person.CountLetterInSurname(letterToCount);
                Console.WriteLine($"{person.Surname}: {count} разів");
            }

            Console.WriteLine($"\nЛітера '{letterToCount}' в прізвищах студентів:");
            foreach (var student in studentRepo.GetAll())
            {
                int count = student.CountLetterInSurname(letterToCount);
                Console.WriteLine($"{student.Surname}: {count} разів");
            }

            Console.WriteLine("\n=== ЗАВЕРШЕННЯ РОБОТИ ПРОГРАМИ ===");
            Console.ReadKey();
        }
    }
}
