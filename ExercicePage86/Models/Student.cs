namespace ExercicePage86.Models;

internal class Student
{
    public Student(string lastname, string firstname, DateTime birthDate, int? yearResult, int sectionId)
    {
        Lastname = lastname;
        Firstname = firstname;
        BirthDate = birthDate;
        YearResult = yearResult;
        SectionId = sectionId;
    }

    public string Lastname { get; set; }
    public string Firstname { get; set; }
    public DateTime BirthDate { get; set; }
    public int? YearResult { get; set; }
    public int SectionId { get; set; }
}
