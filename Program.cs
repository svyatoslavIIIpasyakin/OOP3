using DataAccess;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataAccess
{
    public class StudentsRepository
    {
        private string filePath;

        public StudentsRepository(string filePath)
        {
            this.filePath = filePath;
        }

        public void SaveStudents(List<Student> students)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var student in students)
                {
                    writer.WriteLine($"{student.FirstName},{student.LastName},{student.Age},{student.AverageGrade}");
                }
            }
        }

        public List<Student> ReadStudents()
        {
            List<Student> students = new List<Student>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    students.Add(new Student(parts[0], parts[1], int.Parse(parts[2]), double.Parse(parts[3])));
                }
            }
            return students;
        }
    }
}

public class Student
{
    public Student() { } // Добавлен пустой конструктор для сериализации и десериализации

    public Student(string firstName, string lastName, int age, double averageGrade)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        AverageGrade = averageGrade;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public double AverageGrade { get; set; }
}

public class University
{
    private List<Student> students = new List<Student>();

    public List<Student> Students => students; // Добавлено свойство для доступа к списку студентов

    public void AddStudent(Student student)
    {
        if (student == null)
            throw new ArgumentNullException("Student cannot be null");
        students.Add(student);
    }

    public void RemoveStudent(Student student)
    {
        students.Remove(student);
    }

    public Student FindStudent(string firstName, string lastName)
    {
        return students.Find(s => s.FirstName == firstName && s.LastName == lastName);
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Пример использования
        University university = new University();
        university.AddStudent(new Student("John", "Brown", 20, 85.5));
        university.AddStudent(new Student("Jane", "Willson", 22, 78.2));

        StudentsRepository repository = new StudentsRepository("students.txt");
        repository.SaveStudents(university.Students);

        List<Student> loadedStudents = repository.ReadStudents();
        foreach (var student in loadedStudents)
        {
            Console.WriteLine($"{student.FirstName} {student.LastName}, Age: {student.Age}, Average Grade: {student.AverageGrade}");
        }
    }
}
