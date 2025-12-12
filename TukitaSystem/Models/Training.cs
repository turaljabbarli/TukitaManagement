using System;
using System.Collections.Generic;

namespace TukitaSystem
{
    public class Training
    {
        private static List<Training> _extent = new List<Training>();

        private Manager? _managerStudent;
        private Manager? _managerTeacher;

        public DateTime TrainingStart { get; set; }
        public DateTime? TrainingEnd { get; set; }
        public string? Comments { get; set; }

        public Training(Manager? student, Manager? teacher, DateTime trainingStart)
        {
            TrainingStart = trainingStart;

            if (student != null)
            {
                SetManagerStudent(student);
            }

            if (teacher != null)
            {
                SetManagerTeacher(teacher);
            }

            _extent.Add(this);
        }


        public Manager? ManagerStudent => _managerStudent;

        public void SetManagerStudent(Manager? newStudent)
        {
            if (_managerStudent == newStudent) return;

            var oldStudent = _managerStudent;
            _managerStudent = newStudent;

            if (oldStudent != null)
            {
                if (oldStudent.StudentTraining == this)
                {
                    oldStudent.SetStudentTraining(null);
                }
            }

            if (_managerStudent != null)
            {
                _managerStudent.SetStudentTraining(this);
            }
        }


        public Manager? ManagerTeacher => _managerTeacher;

        public void SetManagerTeacher(Manager? newTeacher)
        {
            if (_managerTeacher == newTeacher) return;

            if (newTeacher != null && newTeacher.RankType != RankType.Lead)
            {
                throw new InvalidOperationException("Cannot assign a non-Lead manager as a teacher.");
            }

            var oldTeacher = _managerTeacher;
            _managerTeacher = newTeacher;

            if (oldTeacher != null)
            {
                oldTeacher.RemoveLeadingTraining(this);
            }

            if (_managerTeacher != null)
            {
                _managerTeacher.AddLeadingTraining(this);
            }
        }
    }
}