using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityApp.Forms.DataModels
{
    internal class Course
    {
        private String Name;
        private List<Student> Students;

        public Course(string Name, List<Student> students)
        {
            this.Name = Name;
            this.Students = students ?? new List<Student>();
        }
        public void SetName(string name)
        {
            Name = name;
        }
        public string GetName()
        {
            return Name;
        }
        public void SetStudents(List<Student> students)
        {
            Students = students ?? new List<Student>();
        }
        public List<Student> GetStudents()
        {
            return Students;
        }
        public override string ToString()
        {
            return $"Course Name: {GetName()}, Students:\n\t{string.Join("\n\t", GetStudents())}";
        }
    }
}
