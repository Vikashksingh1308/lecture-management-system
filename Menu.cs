namespace lecture_management_system;

/*
 * This file provides the text-based menu users interact with.
 * It collects input, calls the lecture manager, and shows the result on screen.
 */
public class Menu
{
    // The lecture manager used for all data actions
    private LectureManager manager;

    // Connects this menu to the shared lecture manager
    public Menu(LectureManager manager)
    {
        this.manager = manager;
    }

    // Shows the menu repeatedly and sends each choice to the matching action
    public void Run()
    {
        bool running = true;

        while (running)
        {
            ShowMenu();
            string? choice = ReadInput();

            switch (choice)
            {
                case "1":
                    AddLecture();
                    Pause();
                    break;
                case "2":
                    ShowHeader("All Lectures");
                    manager.ListAll();
                    Pause();
                    break;
                case "3":
                    EditLecture();
                    Pause();
                    break;
                case "4":
                    DeleteLecture();
                    Pause();
                    break;
                case "5":
                    SearchLectures();
                    Pause();
                    break;
                case "6":
                    Console.WriteLine("Goodbye!");
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter a number from 1 to 6.");
                    Pause();
                    break;
            }
        }
    }

    // Clears the screen and displays the available menu choices
    private void ShowMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Lecture Management System ===");
        Console.WriteLine($"Lectures stored: {Lecture.Count}");
        Console.WriteLine();
        Console.WriteLine("1. Add a lecture");
        Console.WriteLine("2. List all lectures");
        Console.WriteLine("3. Edit a lecture");
        Console.WriteLine("4. Delete a lecture");
        Console.WriteLine("5. Search lectures");
        Console.WriteLine("6. Exit");
        Console.Write("Choose an option: ");
    }

    // Reads one line of input and closes the app if no more input is available
    private static string ReadInput()
    {
        string? line = Console.ReadLine();
        if (line == null)
        {
            Environment.Exit(0);
        }
        return line;
    }

    // Clears the screen and prints a title for the current action
    private void ShowHeader(string title)
    {
        Console.Clear();
        Console.WriteLine($"=== {title} ===");
        Console.WriteLine();
    }

    // Waits for Enter so the result stays visible before the menu resets
    private void Pause()
    {
        Console.WriteLine();
        Console.Write("Press Enter to return to the menu...");
        ReadInput();
    }

    // Collects a search term and asks the manager to find matching lectures
    private void SearchLectures()
    {
        ShowHeader("Search Lectures");
        string term = ReadRequired("Search by code, name, lecturer or language/system/protocol: ");
        Console.WriteLine();
        manager.Search(term);
    }

    // Collects lecture details and adds the chosen lecture type to the list
    private void AddLecture()
    {
        ShowHeader("Add Lecture");
        Console.WriteLine("Lecture types: 1. Programming  2. Database  3. Networking");
        Console.Write("Choose type: ");
        string? type = ReadInput();

        if (type != "1" && type != "2" && type != "3")
        {
            Console.WriteLine("Invalid type. Please choose 1, 2 or 3.");
            return;
        }

        string name = ReadRequired("Subject name: ");
        string lecturer = ReadRequired("Lecturer name: ");
        int hours = ReadHours("Lecture hours: ");

        Lecture lecture;

        if (type == "1")
            lecture = new Programming("", name, lecturer, hours, ReadRequired("Programming language: "));
        else if (type == "2")
            lecture = new Database("", name, lecturer, hours, ReadRequired("Database system: "));
        else
            lecture = new Networking("", name, lecturer, hours, ReadRequired("Protocol: "));

        string code = manager.Add(lecture);
        Console.WriteLine($"Lecture added with code: {code}");
    }

    // Keeps asking until the user enters a non-empty value
    private string ReadRequired(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = (ReadInput() ?? "").Trim();

            if (input != "")
            {
                return input;
            }
            Console.WriteLine("This field is required. Please try again.");
        }
    }

    // Requires a valid positive number of lecture hours
    private int ReadHours(string prompt)
    {
        return ReadOptionalHours(prompt.TrimEnd(' ', ':'), null);
    }

    // Lets the user update a lecture while keeping any fields they leave blank
    private void EditLecture()
    {
        ShowHeader("Edit Lecture");
        string code = ReadRequired("Enter subject code to edit: ");
        Lecture? lecture = manager.FindByCode(code);

        if (lecture == null)
        {
            Console.WriteLine("Lecture not found.");
            return;
        }

        Console.WriteLine("Current details:");
        Console.WriteLine(lecture.GetDetails());
        Console.WriteLine("Press Enter to keep the current value.");

        lecture.Name = ReadOptional("Subject name", lecture.Name);
        lecture.LecturerName = ReadOptional("Lecturer name", lecture.LecturerName);
        lecture.Hours = ReadOptionalHours("Lecture hours", lecture.Hours);

        // Each type has its own extra field to edit
        if (lecture is Programming programming)
        {
            programming.Language = ReadOptional("Programming language", programming.Language);
        }
        else if (lecture is Database database)
        {
            database.DatabaseSystem = ReadOptional("Database system", database.DatabaseSystem);
        }
        else if (lecture is Networking networking)
        {
            networking.Protocol = ReadOptional("Protocol", networking.Protocol);
        }

        manager.Save();

        Console.WriteLine("Lecture updated.");
    }

    // Uses the new text if entered, or keeps the current value on Enter
    private string ReadOptional(string label, string current)
    {
        Console.Write($"{label} [{current}]: ");
        string input = (ReadInput() ?? "").Trim();
        return input == "" ? current : input;
    }

    // Reads a positive whole number and can keep the current value on Enter
    private int ReadOptionalHours(string label, int? current)
    {
        while (true)
        {
            Console.Write(current == null ? $"{label}: " : $"{label} [{current}]: ");
            string input = ReadInput().Trim();

            if (input == "" && current != null)
            {
                return current.Value;
            }
            if (int.TryParse(input, out int hours) && hours > 0)
            {
                return hours;
            }
            Console.WriteLine("Hours must be a whole number greater than 0.");
        }
    }

    // Asks for a subject code and removes the lecture after confirmation
    private void DeleteLecture()
    {
        ShowHeader("Delete Lecture");
        string code = ReadRequired("Enter subject code to delete: ");
        Lecture? lecture = manager.FindByCode(code);

        if (lecture == null)
        {
            Console.WriteLine("Lecture not found.");
            return;
        }

        Console.WriteLine(lecture.GetDetails());
        Console.Write("Are you sure you want to delete this lecture? (y/n): ");
        string answer = (ReadInput() ?? "").Trim().ToLower();

        if (answer == "y")
        {
            manager.Delete(lecture.Code);
            Console.WriteLine("Lecture deleted.");
        }
        else
        {
            Console.WriteLine("Delete cancelled.");
        }
    }
}