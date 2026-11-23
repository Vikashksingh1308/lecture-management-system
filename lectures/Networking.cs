namespace lecture_management_system;

/*
 * This file defines networking lectures.
 * It adds the main network protocol as extra information for each lecture.
 */
public class Networking : Lecture
{
    // Network protocol covered by the lecture.
    public string Protocol { get; set; }

    // Creates a networking lecture and passes the shared details to the base type.
    public Networking(string code, string name, string lecturerName, int hours, string protocol)
        : base(code, name, lecturerName, hours)
    {
        Protocol = protocol;
    }

    // Returns the lecture type name used in table and detail views.
    public override string GetTypeName() { return "Networking"; }

    // Names the extra information shown for this lecture type.
    public override string GetExtraLabel() { return "Protocol"; }

    // Returns the network protocol for display.
    public override string GetExtra() { return Protocol; }

    // Builds a text summary that includes the network protocol.
    public override string GetDetails()
    {
        return $"Code: {Code}, Name: {Name}, Lecturer: {LecturerName}, Hours: {Hours}, Type: Networking, Protocol: {Protocol}";
    }
}