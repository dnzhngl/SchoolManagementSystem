CREATE VIEW View_Timetables AS 
SELECT Timetables.Id, Timetables.ClassroomName, Sections.SectionName, Subjects.SubjectName, 
Instructors.FirstName + ' ' + Instructors.LastName AS Instructor, Days.DayName, 
LessonTimes.LessonBeginTime, LessonTimes.LessonEndTime 
FROM Timetables 
INNER JOIN Sections ON Sections.Id = Timetables.SectionId 
INNER JOIN Subjects ON Subjects.Id = Timetables.SubjectId 
INNER JOIN Instructors ON Instructors.Id = Timetables.InstructorId 
INNER JOIN Days ON Days.Id = Timetables.DayId 
INNER JOIN LessonTimes ON LessonTimes.Id = Timetables.LessonTimeId 
INNER JOIN Semesters ON Semesters.Id = Timetables.SemesterId


