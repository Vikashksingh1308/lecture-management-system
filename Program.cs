using lecture_management_system;

/*
 * This file starts the lecture management app.
 * It creates the data manager and menu, and then runs the menu loop.
 */
LectureManager manager = new LectureManager();

Menu menu = new Menu(manager);
menu.Run();