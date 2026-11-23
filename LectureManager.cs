namespace lecture_management_system;

/*
 * This file manages the list of lectures used by the app.
 * It loads saved lectures, lets users add, find, view, search, list and delete them.
 * Also keeps the lecture data in sync with disk.
 */
public class LectureManager
{
    private List<Lecture> lectures;
    private FileHandler fileHandler = new FileHandler();

    // Per-type counters: only ever increment, never reuse a deleted lecture's number.
    private int _nextProgrammingId;
    private int _nextDatabaseId;
    private int _nextNetworkingId;

    public LectureManager()
    {
        LectureData data = fileHandler.Load();
        lectures = data.ProgrammingLectures.Cast<Lecture>()
            .Concat(data.DatabaseLectures)
            .Concat(data.NetworkingLectures)
            .ToList();
        _nextProgrammingId = data.NextProgrammingId;
        _nextDatabaseId    = data.NextDatabaseId;
        _nextNetworkingId  = data.NextNetworkingId;
    }

    // Builds a LectureData snapshot (lectures + current counters) and writes it to disk.
    public void Save()
    {
        LectureData data = new LectureData
        {
            ProgrammingLectures = lectures.OfType<Programming>().ToList(),
            DatabaseLectures    = lectures.OfType<Database>().ToList(),
            NetworkingLectures  = lectures.OfType<Networking>().ToList(),
            NextProgrammingId   = _nextProgrammingId,
            NextDatabaseId      = _nextDatabaseId,
            NextNetworkingId    = _nextNetworkingId
        };
        if (!fileHandler.Save(data))
            Console.WriteLine("Warning: changes are in memory but could not be saved to disk.");
    }

    // Generates the next unique code for the given lecture type and advances its counter.
    private string AssignCode(Lecture lecture)
    {
        if (lecture is Programming) return $"PRG-{_nextProgrammingId++}";
        if (lecture is Database)    return $"DB-{_nextDatabaseId++}";
        if (lecture is Networking)  return $"NET-{_nextNetworkingId++}";
        return $"LEC-{_nextProgrammingId++}";
    }

    // Finds a lecture by code, ignoring letter case.
    public Lecture? FindByCode(string code)
    {
        return lectures.FirstOrDefault(l => l.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
    }

    // Assigns a system-generated code, adds the lecture, and returns the generated code.
    public string Add(Lecture lecture)
    {
        lecture.Code = AssignCode(lecture);
        lectures.Add(lecture);
        Save();
        return lecture.Code;
    }

    // Finds and displays one lecture or reports if it cannot be found.
    public bool View(string code)
    {
        Lecture? lecture = FindByCode(code);

        if (lecture == null)
        {
            Console.WriteLine("Lecture not found.");
            return false;
        }

        Console.WriteLine(lecture.GetDetailedView());
        return true;
    }

    // Prints all saved lectures in a simple table.
    public void ListAll()
    {
        if (lectures.Count == 0)
        {
            Console.WriteLine("No lectures stored yet.");
            return;
        }

        PrintTable(lectures);
        Console.WriteLine();
        Console.WriteLine($"Total lectures: {lectures.Count}");
    }

    // Searches all lecture properties without caring about letter case.
    public void Search(string term)
    {
        bool Matches(Lecture lecture) =>
            lecture.Code.Contains(term, StringComparison.OrdinalIgnoreCase) ||
            lecture.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
            lecture.LecturerName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
            lecture.GetExtra().Contains(term, StringComparison.OrdinalIgnoreCase);

        List<Lecture> matches = lectures.Where(Matches).ToList();

        if (matches.Count == 0)
        {
            Console.WriteLine($"No lectures match \"{term}\".");
            return;
        }

        PrintTable(matches);
        Console.WriteLine();
        Console.WriteLine($"{matches.Count} match(es) found.");
    }

    // Prints lectures as a table and adjusts each column to fit the longest value.
    private void PrintTable(List<Lecture> list)
    {
        string[] headers = { "Code", "Name", "Lecturer", "Hours", "Type", "Detail" };

        List<string[]> rows = list
            .Select(l => new[] { l.Code, l.Name, l.LecturerName, l.Hours.ToString(), l.GetTypeName(), l.GetExtra() })
            .ToList();

        int[] widths = Enumerable.Range(0, headers.Length)
            .Select(c => rows.Select(row => row[c].Length).Prepend(headers[c].Length).Max())
            .ToArray();

        // Local helper that aligns a row before it is printed.
        string Format(string[] cells)
        {
            string line = "";
            for (int c = 0; c < cells.Length; c++)
            {
                // The hours column lines up on the right; all the others use the left.
                line += (c == 3 ? cells[c].PadLeft(widths[c]) : cells[c].PadRight(widths[c])) + "  ";
            }
            return line.TrimEnd();
        }

        string headerLine = Format(headers);
        Console.WriteLine(headerLine);
        Console.WriteLine(new string('-', headerLine.Length));
        foreach (string[] row in rows)
        {
            Console.WriteLine(Format(row));
        }
    }

    // Removes a lecture by code and updates the saved data.
    public bool Delete(string code)
    {
        Lecture? lecture = FindByCode(code);

        if (lecture == null)
        {
            return false;
        }

        lectures.Remove(lecture);
        Lecture.Count--;
        Save();
        return true;
    }

}