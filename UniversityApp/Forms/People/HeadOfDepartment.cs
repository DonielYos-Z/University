using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace UniversityApp.Forms.DataModels
{
    // Extension-class HeadOfDepartment, extends from Professor
    internal class HeadOfDepartment:Professor
    {
        // Attributes
        private List<StudyTrack> TracksManaging;
        private List<Person> Supervising;
        private List<Student> StudentsInDepartment;
        // Constructor
        private HeadOfDepartment(
            string id, string name, int age, string phoneNumber, string email, List<string> messageList, string filepath,
            int EmployeeNumber, List<Course> coursesTeaching, string specialization, double ratingFromStudents,
            List<StudyTrack> tracksManaging, List<Person> supervising, List<Student> studentsInDepartment)
            : base(id, name, age, phoneNumber, email, messageList, filepath,
                EmployeeNumber, coursesTeaching, specialization, ratingFromStudents)
        {
            SetTracksManaging(tracksManaging);
            SetSupervising(supervising);
            SetStudentsInDepartment(studentsInDepartment);
        }
        // Setters, Getters, and ToString()
        public void SetTracksManaging(List<StudyTrack> tracksManaging)
        {
            if (tracksManaging == null)
                throw new ArgumentNullException(nameof(tracksManaging), "TracksManaging list cannot be null.");
            TracksManaging = tracksManaging;
        }
        public List<StudyTrack> GetTracksManaging()
        {
            return TracksManaging;
        }
        public void SetSupervising(List<Person> supervising)
        {
            if (supervising == null)
                throw new ArgumentNullException(nameof(supervising), "Supervising list cannot be null.");
            foreach (Person p in supervising)
            {
                if (!(p is StudentTeacher || p is Professor) || p is HeadOfDepartment)
                {
                    throw new ArgumentException("Supervising can only be a Student Teacher or Professor.");
                }
            }
            Supervising = supervising;
        }
        public List<Person> GetSupervising()
        {
            return Supervising;
        }
        public void SetStudentsInDepartment(List<Student> studentsInDepartment)
        {
            if (studentsInDepartment == null)
                throw new ArgumentNullException(nameof(studentsInDepartment), "StudentsInDepartment list cannot be null.");
            StudentsInDepartment = studentsInDepartment;
        }
        public List<Student> GetStudentsInDepartment()
        {
            return StudentsInDepartment;
        }
        public override string ToString()
        {
            return base.ToString()+
                $"Tracks Managing:\n\t{ string.Join("\n\t", GetTracksManaging())},\n" +
                $"Supervising:\n\t{string.Join("\n\t", GetSupervising())},\n" +
                $"Students in Department:\n\t{string.Join("\n\t", GetStudentsInDepartment())}";
        }
        // No need to call the abstract method since a call is inherited from Professor
        // Reminder: That method returns a string of the courses being taught (not managed) by the professor and the students enrolled in each course

        /* --- Track Management ---
         * These methods display all of the tracks the Head of Department is managing (name only to prevent loops or annoyingly long text, 
         * as well as to preserve the usefulness of other methods), add a track to the list of tracks being managed by him,
         * remove a track from that list, and update an existing track, swapping it out with a different one
        */
        public string DisplayTracksManaging()
        {
            if (TracksManaging == null || TracksManaging.Count == 0)
                return "No tracks being managed.";
            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (var track in TracksManaging)
                sb.AppendLine("#"+count+": "+track.GetName());
            return sb.ToString();
        }
        public void AddTrack(StudyTrack track)
        {
            // Top add a track, it must not be null and it must not already exist in the list of tracks being managed
            if (track != null && !TracksManaging.Contains(track))
                TracksManaging.Add(track);
            else if (track == null)
                throw new ArgumentNullException(nameof(track), "Track cannot be null.");
            else
                throw new ArgumentException("Track already exists in the list of tracks being managed.");
        }
        public void RemoveTrack(StudyTrack track)
        {
            // To remove a track, it must not be null and it must exist in the list of tracks being managed
            if (track!=null && TracksManaging.Contains(track))
                TracksManaging.Remove(track);
            else if (track == null)
                throw new ArgumentNullException(nameof(track), "Track cannot be null.");
            else
                throw new ArgumentException("Track not found in the list of tracks being managed.");
        }
        public void UpdateTrack(StudyTrack oldTrack, StudyTrack newTrack)
        {
            // To update a track, both the old and new tracks must not be null, and the old track must exist in the list of tracks being managed
            if (oldTrack == null)
                throw new ArgumentNullException(nameof(oldTrack), "Old track cannot be null.");
            if (newTrack == null)
                throw new ArgumentNullException(nameof(newTrack), "New track cannot be null.");
            int idx = TracksManaging.IndexOf(oldTrack);
            if (idx >= 0)
                TracksManaging[idx] = newTrack;
            else
                throw new ArgumentException("Old track not found in the list of tracks being managed.");
        }
        /* --- Course Management ---
         * These methods display all of the courses in the tracks the Head of Department is managing (name only for the reasons metioned above),
         * add a course to a specified track, remove a course from a specified track,
         * and update a specific course in a specified track, swapping it out for a different course
        */
        public string DisplayCoursesManaging()
        {
            if (TracksManaging == null || TracksManaging.Count == 0)
                return "No tracks being managed.";
            StringBuilder sb = new StringBuilder();
            foreach (var track in TracksManaging)
            {
                sb.AppendLine($"Courses in {track.GetName()}: ");
                if (track.GetCourses() != null && track.GetCourses().Count > 0)
                    sb.AppendLine($"{string.Join(", ", track.GetCourses().Select(c => c.GetName()))}");
                else
                    sb.AppendLine("No courses in this track.");
            }
            return sb.ToString();
        }
        public void AddCourseToTrack(StudyTrack track, Course course)
        {
        // To add a course to a track, both the track and course must not be null, and the course must not already exist in the track's course list
            if (track != null && course != null && !track.GetCourses().Contains(course))
                track.GetCourses().Add(course);
            else if (track == null)
                throw new ArgumentNullException(nameof(track), "Track cannot be null.");
            else if (course == null)
                throw new ArgumentNullException(nameof(course), "Course cannot be null.");
            else
                throw new ArgumentException("This course already exists in the track's course list.");
        }
        public void RemoveCourseFromTrack(StudyTrack track, Course course)
        {
            // To remove a course from a track, both the track and course must not be null, and the course must exist in the track's course list
            if (track != null && course != null && track.GetCourses().Contains(course))
                track.GetCourses().Remove(course);
            else if (track == null)
                throw new ArgumentNullException(nameof(track), "Track cannot be null.");
            else if (course == null)
                throw new ArgumentNullException(nameof(course), "Course cannot be null.");
            else
                throw new ArgumentException("This course is not found in the track's course list.");
        }
        public void UpdateCourseInTrack(StudyTrack track, Course oldCourse, Course newCourse)
        {
            // To update a course in a track, both the old and new courses must not be null, the track must not be null,
            // and the old course must exist in the track's course list
            if (track == null)
                throw new ArgumentNullException(nameof(track), "Track cannot be null.");
            if (oldCourse == null)
                throw new ArgumentNullException(nameof(oldCourse), "Old course cannot be null.");
            if (newCourse == null)
                throw new ArgumentNullException(nameof(newCourse), "New course cannot be null.");
            if (TracksManaging.Contains(track))
            { 
                var courses = track.GetCourses();
                int idx = courses.IndexOf(oldCourse);
                if (idx >= 0)
                    courses[idx] = newCourse;
            }
            else
                throw new ArgumentException("Track not found in the list of tracks being managed.");
        }
        /* --- Teacher Management ---
         * These methods display all of the teachers (student teachers and professors) the Head of Department is supervising
         * (name and the courses they are teaching), add a teacher to the list of teachers they are supervising,
         * remove a teacher from the list, and update a specific teacher, swapping it out for a different teacher
        */
        public string displayTeachersSupervising()
        {
            if (Supervising == null || Supervising.Count == 0)
                return ("No teachers being supervised.");
            StringBuilder sb = new StringBuilder();
            foreach (var teacher in Supervising)
            {
                sb.AppendLine($"Teacher: {teacher.GetName()}");
                if (teacher is Professor prof)
                    sb.AppendLine($"Courses Teaching: {string.Join(", ", prof.GetCoursesTeaching().Select(c => c.GetName()))}");
                else if (teacher is StudentTeacher st)
                    sb.AppendLine($"Courses Teaching: {string.Join(", ", st.GetCoursesTeaching().Select(c => c.GetName()))}");
            }
            return (sb.ToString());
        }
        public void AddTeacher(Person teacher)
        {
            // To add a teacher, it must not be null, it must be a non-HoD Professor or StudentTeacher,
            // and it must not already exist in the list of teachers the HoD is supervising
            if ( teacher != null && (teacher is Professor || teacher is StudentTeacher) && !(teacher is HeadOfDepartment) && !Supervising.Contains(teacher))
                Supervising.Add(teacher);
            else if (teacher == null)
                throw new ArgumentNullException(nameof(teacher), "Teacher cannot be null.");
            else if (!(teacher is Professor || teacher is StudentTeacher) || teacher is HeadOfDepartment)
                throw new ArgumentException("Head of Department can only supervise student teachers and professors that aren't Head of Department.");
            else
                throw new ArgumentException("This teacher already exists in the list of supervising teachers.");
        }
        public void RemoveTeacher(Person teacher)
        {
            // To remove a teacher, it must not be null and it must exist in the list of teachers the HoD is supervising
            if (teacher != null && Supervising.Contains(teacher))
                Supervising.Remove(teacher);
            else if (teacher == null)
                throw new ArgumentNullException(nameof(teacher), "Teacher cannot be null.");
            else
                throw new ArgumentException("Teacher not found in the list of supervising teachers.");
        }
        public void UpdateTeacher(Person oldTeacher, Person newTeacher)
        {
            // To update a teacher, both the old and new teachers must not be null, the new teacher must be a non-HoD Professor or StudentTeacher,
            // and the old teacher must exist in the list of teachers the HoD is supervising
            if (oldTeacher == null)
                throw new ArgumentNullException(nameof(oldTeacher), "Old teacher cannot be null.");
            if (newTeacher == null)
                throw new ArgumentNullException(nameof(newTeacher), "New teacher cannot be null.");
            if (!(newTeacher is Professor || newTeacher is StudentTeacher) || newTeacher is HeadOfDepartment)
                throw new ArgumentException("New teacher must be a Professor or Student Teacher, and cannot be a Head of Department.");
            if (Supervising == null)
                throw new ArgumentNullException(nameof(Supervising), "Supervising list cannot be null.");
            if(!Supervising.Contains(oldTeacher))
                throw new ArgumentException("Old teacher not found in the list of supervising teachers.");
            else
            {
                int idx = Supervising.IndexOf(oldTeacher);
                if (idx >= 0)
                    Supervising[idx] = newTeacher;
            }
        }
        /* --- Student Management ---
         * These methods display all of the students in the Head of Department's department (name and the tracks they are learning),
         * add a student to that list of students in the HoF's department, remove a student from that list, 
         * and update a specific student, swapping it out for a different student
        */
        public string DisplayStudentsInDepartment()
        {
            if (StudentsInDepartment == null || StudentsInDepartment.Count == 0)
                return "No students in the department.";
            StringBuilder sb = new StringBuilder();
            foreach (var student in StudentsInDepartment)
            {
                sb.AppendLine($"Student: {student.GetName()}");
                sb.AppendLine($"Track: {student.GetStudyTrack().GetName()}");
            }
            return sb.ToString();
        }
        public void AddStudent(Student student)
        {
            // To add a student, it must not be null and it must not already exist in the list of students in the department
            if (student != null && !StudentsInDepartment.Contains(student))
                StudentsInDepartment.Add(student);
            else if (student == null)
                throw new ArgumentNullException(nameof(student), "Student cannot be null.");
            else
                throw new ArgumentException("This student already exists in the list of students in the department.");
        }
        public void RemoveStudent(Student student)
        {
            // To remove a student, it must not be null and it must exist in the list of students in the department
            if (student != null && StudentsInDepartment.Contains(student))
                StudentsInDepartment.Remove(student);
            else if (student == null)
                throw new ArgumentNullException(nameof(student), "Student cannot be null.");
            else
                throw new ArgumentException("Student not found in the list of students in the department.");
        }
        public void UpdateStudent(Student oldStudent, Student newStudent)
        {
            // To update a student, both the old and new students must not be null, and the old student must exist in the list of students in the department
            if (oldStudent == null)
                throw new ArgumentNullException(nameof(oldStudent), "Old student cannot be null.");
            if (newStudent == null)
                throw new ArgumentNullException(nameof(newStudent), "New student cannot be null.");
            if (StudentsInDepartment == null)
                throw new ArgumentNullException(nameof(StudentsInDepartment), "StudentsInDepartment list cannot be null.");
            if (!StudentsInDepartment.Contains(oldStudent))
                throw new ArgumentException("Old student not found in the list of students in the department.");
            else
            {
                int idx = StudentsInDepartment.IndexOf(oldStudent);
                if (idx >= 0)
                    StudentsInDepartment[idx] = newStudent;
            }
        }
        // --- Assignment Methods ---
        // The following methods assigns pre-existing students/teachers to pre-existing tracks/courses.
        public void AssignStudentToTrack(Student student, StudyTrack track)
        {
            // To assign a student to a track, both the student and track must not be null, and the student must not yet be in the track's student list.
            if (student != null && track != null && !track.GetStudents().Contains(student))
            {
                track.GetStudents().Add(student);
                // If the student is already enrolled in a different track, remove them from that track
                if (student.GetStudyTrack() != track)
                {
                    student.GetStudyTrack().GetStudents().Remove(student);
                    student.SetStudyTrack(track);
                }
            }
            else if (student != null && track != null && track.GetStudents().Contains(student))
                throw new ArgumentException("This student is already enrolled in the track.");
            else if (student == null)
                throw new ArgumentNullException(nameof(student), "Student cannot be null.");
            else if (track == null)
                throw new ArgumentNullException(nameof(track), "Track cannot be null.");
            else if (student.GetStudyTrack() != track)
                throw new ArgumentException("The student is not enrolled in this track.");
            else
                throw new ArgumentException("This student is already enrolled in the track.");
        }
        public void AssignStudentToCourse(Student student, Course course)
        {
            if (student != null && course != null && !course.GetStudents().Contains(student))
                course.GetStudents().Add(student);
        }
        public void AssignTeacherToCourse(Person teacher, Course course)
        {
            if ((teacher is Professor || teacher is StudentTeacher) && course != null)
            {
                // You may want to add logic to associate the teacher with the course
                // For example, if Professor/StudentTeacher has a CoursesTeaching list:
                if (teacher is Professor prof && !prof.GetCoursesTeaching().Contains(course))
                    prof.GetCoursesTeaching().Add(course);
                else if (teacher is StudentTeacher st && !st.GetCoursesTeaching().Contains(course))
                    st.GetCoursesTeaching().Add(course);
            }
        }
    }
}