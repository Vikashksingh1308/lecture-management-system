namespace lecture_management_system;

/*
 * This file defines programming lectures.
 * It adds the programming language as extra info for each lecture
 */
public class Programming : Lecture
{
    // Programming language covered by the lecture.
    public string Language { get; set; }

    // Creates a programming lecture and passes the shared details to the base type.
    public Programming(string code, string name, string lecturerName, int hours, string language)
        : base(code, name, lecturerName, hours)
    {
        Language = language;
    }

    // Returns the lecture type name used in table and detail views.
    public override string GetTypeName() { return "Programming"; }

    // Names the extra information shown for this lecture type.
    public override string GetExtraLabel() { return "Language"; }

    // Returns the programming language for display.
    public override string GetExtra() { return Language; }

    // Builds a text summary that includes the programming language.
    public override string GetDetails()
    {
        return base.GetDetails() + $", Type: Programming, Language: {Language}";
    }
}