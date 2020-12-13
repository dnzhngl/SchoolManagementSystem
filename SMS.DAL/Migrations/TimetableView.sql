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

--DISMISSED STUDENTS VIEW CREATION

--CREATE VIEW View_DismissedStudents AS
--SELECT * FROM Students
--WHERE Students.StudentStatus = 'Mezun' or StudentStatus = 'Sevk' and Students.StudentStatusEditDate IS NOT NULL



--STUDENTS SCHOOL REPORT VIEW CREATION

--CREATE VIEW View_StudentSchoolReport AS 	
--SELECT Students.Id, Students.SchoolNumber, Students.FirstName, Students.LastName, 
--Subjects.SubjectName,
--Subjects.WeeklyCourseHours,
--ROUND(AVG(ExamResults.ExamMark),0) AS AvgMark, 
--AVG(ExamResults.ExamMarkNumeral) as AvgMarkNumeral,
--(ROUND(AVG(ExamResults.ExamMark),0) * Subjects.WeeklyCourseHours) as GPA,
--(AVG(ExamResults.ExamMarkNumeral) * Subjects.WeeklyCourseHours) as GpaNumeral,
--COUNT(DISTINCT ExamResults.Id) AS [NumberOfExams],
--MainSubjects.MainSubjectName,
--Semesters.SemesterName, Semesters.AcademicYear
--FROM ExamResults
--INNER JOIN Exams ON Exams.Id = ExamResults.ExamId
--INNER JOIN Students ON Students.Id = ExamResults.StudentId
--INNER JOIN Subjects ON Subjects.Id = Exams.SubjectId
--INNER JOIN MainSubjects ON MainSubjects.Id = Subjects.MainSubjectId
--INNER JOIN Timetables On Timetables.SubjectId = Subjects.Id
--INNER JOIN Semesters ON Semesters.Id = Timetables.SemesterId
--where (year(exams.ExamDate) = year(Semesters.SemesterBeginning))
--GROUP BY Students.Id, Students.SchoolNumber, Students.FirstName, Students.LastName, 
--Subjects.SubjectName, Subjects.WeeklyCourseHours, 
--Semesters.SemesterName, Semesters.AcademicYear,
--MainSubjects.MainSubjectName


--FILESTREAM TABLE CREATION FOR STUDENTS IMAGES

--create table StudentImages(
--	Id UniqueIdentifier rowguidcol not null unique, 
--	ImageFile Varbinary(MAX) Filestream
--);