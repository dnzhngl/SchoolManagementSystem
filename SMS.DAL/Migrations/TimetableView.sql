CREATE VIEW View_Timetables AS 
SELECT Timetables.Id, Classrooms.ClassroomName, Sections.SectionName, Subjects.SubjectName, 
Instructors.FirstName + ' ' + Instructors.LastName AS Instructor, Days.DayName, 
LessonTimes.LessonBeginTime + ' - ' + LessonTimes.LessonEndTime  AS LessonPeriod
FROM Timetables 
INNER JOIN Sections ON Sections.Id = Timetables.SectionId 
INNER JOIN Subjects ON Subjects.Id = Timetables.SubjectId 
INNER JOIN Instructors ON Instructors.Id = Timetables.InstructorId 
INNER JOIN Days ON Days.Id = Timetables.DayId 
INNER JOIN LessonTimes ON LessonTimes.Id = Timetables.LessonTimeId 
INNER JOIN Semesters ON Semesters.Id = Timetables.SemesterId
INNER JOIN Classrooms ON Classrooms.Id = Timetables.ClassroomId

--Sql view'i 2 turde olusturabilirsin = migration ile ya da sql command'i visual studio'da calistirarak. 


--CREATE VIEW View_DismissedStudents AS
--SELECT * FROM Students
--WHERE Students.StudentStatus = 'Mezun' or StudentStatus = 'Sevk' and Students.StudentStatusEditDate IS NOT NULL