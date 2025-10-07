using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PersonStudentSystem
{
    // ===== Person.cs =====
    /// <summary>
    /// Базовий клас "Людина"
    /// </summary>
    public class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public int BirthMonth { get; set; }
        public int BirthYear { get; set; }

        /// <summary>
        /// Конструктор класу Person з валідацією даних
        /// </summary>
        public Person(string name, string surname, string patronymic, int birthMonth, int birthYear)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Ім'я не може бути порожнім", nameof(name));
            if (string.IsNullOrWhiteSpace(surname))
                throw new ArgumentException("Прізвище не може бути порожнім", nameof(surname));
            if (string.IsNullOrWhiteSpace(patronymic))
                throw new ArgumentException("По-батькові не може бути порожнім", nameof(patronymic));
            if (birthMonth < 1 || birthMonth > 12)
                throw new ArgumentOutOfRangeException(nameof(birthMonth), "Місяць має бути від 1 до 12");
            if (birthYear < 1900 || birthYear > DateTime.Now.Year)
                throw new ArgumentOutOfRangeException(nameof(birthYear), "Рік народження некоректний");

            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            BirthMonth = birthMonth;
            BirthYear = birthYear;
        }

        /// <summary>
        /// Обчислює повний вік особи з урахуванням місяця народження
        /// </summary>
        public int GetAge(DateTime currentDate)
        {
            int age = currentDate.Year - BirthYear;
            
            // Якщо місяць народження ще не настав у поточному році, віднімаємо 1 рік
            if (currentDate.Month < BirthMonth)
                age--;
            
            return age;
        }

        /// <summary>
        /// Виводить інформацію про людину
        /// </summary>
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"\nІм'я: {Name}");
            Console.WriteLine($"Прізвище: {Surname}");
            Console.WriteLine($"По-батькові: {Patronymic}");
            Console.WriteLine($"Місяць народження: {BirthMonth}");
            Console.WriteLine($"Рік народження: {BirthYear}");
        }

        /// <summary>
        /// Підраховує кількість входжень літери в прізвищі (без урахування регістру)
        /// </summary>
        public int CountLetterInSurname(char letter)
        {
            if (string.IsNullOrEmpty(Surname))
                return 0;

            char lowerLetter = char.ToLowerInvariant(letter);
            return Surname.Count(c => char.ToLowerInvariant(c) == lowerLetter);
        }

        /// <summary>
        /// Деструктор (фіналайзер) для демонстрації
        /// </summary>
        ~Person()
        {
            // Фіналайзер викликається збирачем сміття
            // У реальних додатках використовується рідко
            // Тут для демонстрації концепції
        }
    }

    // ===== Student.cs =====
    /// <summary>
    /// Похідний клас "Студент"
    /// </summary>
    public class Student : Person
    {
        public int AdmissionYear { get; set; }
        public string Specialty { get; set; }

        /// <summary>
        /// Конструктор класу Student з валідацією
        /// </summary>
        public Student(string name, string surname, string patronymic, 
                      int birthMonth, int birthYear, int admissionYear, string specialty)
            : base(name, surname, patronymic, birthMonth, birthYear)
        {
            if (admissionYear < birthYear)
                throw new ArgumentException("Рік вступу не може бути раніше року народження");
            if (admissionYear > DateTime.Now.Year)
                throw new ArgumentOutOfRangeException(nameof(admissionYear), "Рік вступу не може бути в майбутньому");
            if (string.IsNullOrWhiteSpace(specialty))
                throw new ArgumentException("Спеціальність не може бути порожньою", nameof(specialty));

            AdmissionYear = admissionYear;
            Specialty = specialty;
        }

        /// <summary>
        /// Обчислює кількість років навчання з моменту вступу
        /// </summary>
        public int GetYearsSinceAdmission(DateTime currentDate)
        {
            return currentDate.Year - AdmissionYear;
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Рік вступу до ВУЗу: {AdmissionYear}");
            Console.WriteLine($"Спеціальність: {Specialty}");
        }

        /// <summary>
        /// Деструктор студента
        /// </summary>
        ~Student()
        {
            // Деструктор похідного класу
        }
    }

    // ===== PersonBuilder.cs =====
    /// <summary>
    /// Патерн Builder для створення об'єктів Person
    /// </summary>
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
            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException(nameof(month), "Місяць має бути від 1 до 12");
            _birthMonth = month;
            return this;
        }

        public PersonBuilder SetBirthYear(int year)
        {
            if (year < 1900 || year > DateTime.Now.Year)
                throw new ArgumentOutOfRangeException(nameof(year), "Рік народження некоректний");
            _birthYear = year;
            return this;
        }

        public Person Build()
        {
            return new Person(_name, _surname, _patronymic, _birthMonth, _birthYear);
        }
    }

    // ===== StudentBuilder.cs =====
    /// <summary>
    /// Патерн Builder для створення об'єктів Student
    /// </summary>
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
            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException(nameof(month), "Місяць має бути від 1 до 12");
            _birthMonth = month;
            return this;
        }

        public StudentBuilder SetBirthYear(int year)
        {
            if (year < 1900 || year > DateTime.Now.Year)
                throw new ArgumentOutOfRangeException(nameof(year), "Рік народження некоректний");
            _birthYear = year;
            return this;
        }

        public StudentBuilder SetAdmissionYear(int year)
        {
            if (year > DateTime.Now.Year)
                throw new ArgumentOutOfRangeException(nameof(year), "Рік вступу не може бути в майбутньому");
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

    // ===== IRepository.cs =====
    /// <summary>
    /// Інтерфейс для репозиторію
    /// </summary>
    public interface IRepository<T>
    {
        void Add(T item);
        IReadOnlyList<T> GetAll();
    }

    // ===== PersonRepository.cs =====
    /// <summary>
    /// Патерн Repository для управління колекцією людей
    /// </summary>
    public class PersonRepository : IRepository<Person>
    {
        private readonly List<Person> _people = new List<Person>();

        public void Add(Person person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));
            _people.Add(person);
        }

        public IReadOnlyList<Person> GetAll()
        {
            return _people.AsReadOnly();
        }

        /// <summary>
        /// Знаходить всіх людей за іменем
        /// </summary>
        public IEnumerable<Person> FindAllByName(string name)
        {
            return _people.Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Знаходить людину за прізвищем
        /// </summary>
        public IEnumerable<Person> FindAllBySurname(string surname)
        {
            return _people.Where(p => p.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase));
        }
    }

    // ===== StudentRepository.cs =====
    /// <summary>
    /// Патерн Repository для управління колекцією студентів
    /// </summary>
    public class StudentRepository : IRepository<Student>
    {
        private readonly List<Student> _students = new List<Student>();

        public void Add(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));
            _students.Add(student);
        }

        public IReadOnlyList<Student> GetAll()
        {
            return _students.AsReadOnly();
        }

        /// <summary>
        /// Знаходить всіх студентів за іменем
        /// </summary>
        public IEnumerable<Student> FindAllByName(string name)
        {
            return _students.Where(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Знаходить студентів за спеціальністю
        /// </summary>
        public IEnumerable<Student> FindAllBySpecialty(string specialty)
        {
            return _students.Where(s => s.Specialty.Equals(specialty, StringComparison.OrdinalIgnoreCase));
        }
    }

    // ===== Program.cs =====
    /// <summary>
    /// Головний клас програми
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== СИСТЕМА ОБЛІКУ ЛЮДЕЙ ТА СТУДЕНТІВ ===\n");

            try
            {
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

                DateTime currentDate = DateTime.Today;

                // Виведення інформації про людей
                Console.WriteLine(">>> СПИСОК ЛЮДЕЙ:");
                foreach (var person in personRepo.GetAll())
                {
                    person.DisplayInfo();
                    Console.WriteLine($"Повний вік (станом на {currentDate:dd.MM.yyyy}): {person.GetAge(currentDate)} років");
                }

                // Виведення інформації про студентів
                Console.WriteLine("\n>>> СПИСОК СТУДЕНТІВ:");
                foreach (var student in studentRepo.GetAll())
                {
                    student.DisplayInfo();
                    Console.WriteLine($"Повний вік: {student.GetAge(currentDate)} років");
                    Console.WriteLine($"Років з моменту вступу: {student.GetYearsSinceAdmission(currentDate)} років");
                }

                // Демонстрація підрахунку літери в прізвищі
                Console.WriteLine("\n>>> ПІДРАХУНОК ЛІТЕР У ПРІЗВИЩАХ:");
                Console.Write("\nВведіть літеру для підрахунку в прізвищах: ");
                char letterToCount = Console.ReadKey().KeyChar;
                Console.WriteLine();

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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nПомилка: {ex.Message}");
            }

#if DEBUG
            Console.WriteLine("\nНатисніть будь-яку клавішу для виходу...");
            Console.ReadKey();
#endif
        }
    }
}
