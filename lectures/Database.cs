namespace lecture_management_system;

/*
 * This file defines database lectures.
 * It stores the database system and the common lecture details.
 */
public class Database : Lecture
{
    // Database system covered by the lecture.
    public string DatabaseSystem { get; set; }

    // Creates a database lecture and passes the shared details to the base type.
    public Database(string code, string name, string lecturerName, int hours, string databaseSystem)
        : base(code, name, lecturerName, hours)
    {
        DatabaseSystem = databaseSystem;
    }

    // Returns the lecture type name used in table and detail views.
    public override string GetTypeName() { return "Database"; }

    // Names the extra information shown for this lecture type.
    public override string GetExtraLabel() { return "System"; }

    // Returns the database system for display.
    public override string GetExtra() { return DatabaseSystem; }

    // Builds a text summary that includes the database system.
    public override string GetDetails()
    {
        return $"Code: {Code}, Name: {Name}, Lecturer: {LecturerName}, Hours: {Hours}, Type: Database, System: {DatabaseSystem}";
    }
}