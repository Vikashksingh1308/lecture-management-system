namespace lecture_management_system;

/*
 * This file defines the base Lecture type used by the lecture manager.
 * It stores common lecture details and provides methods for simple and detailed views.
 */
public class Lecture
{
    public static int Count = 0;
    public string Code { get; set; }
    public string Name { get; set; }
    public string LecturerName { get; set; }
    public int Hours { get; set; }

    // Creates a lecture and adds it to the shared lecture count.
    public Lecture(string code, string name, string lecturerName, int hours)
    {
        Code = code;
        Name = name;
        LecturerName = lecturerName;
        Hours = hours;
        Count++;
    }

    public virtual string GetTypeName() { return "Lecture"; }
    public virtual string GetExtraLabel() { return "Detail"; }
    public virtual string GetExtra() { return ""; }
    public string GetDetailedView()
    {
        return $"Code: {Code}\nName: {Name}\nLecturer: {LecturerName}\nHours: {Hours}\n" +
               $"Type: {GetTypeName()}\n{GetExtraLabel()}: {GetExtra()}";
    }

    // Builds a readable summary: marked virtual so a derived lecture type can customise it.
    public virtual string GetDetails()
    {
        return $"Code: {Code}, Name: {Name}, Lecturer: {LecturerName}, Hours: {Hours}";
    }
}