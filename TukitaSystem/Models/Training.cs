using System;
using System.Collections.Generic;

namespace TukitaSystem
{
    public class Training
    {
        private static List<Training> _extent = new List<Training>();

        // CHANGED: Manager -> ManagerRole
        private ManagerRole? _managerStudent;
        private ManagerRole? _managerTeacher;

        public DateTime TrainingStart { get; set; }
        public DateTime? TrainingEnd { get; set; }
        public string? Comments { get; set; }

        // CHANGED: Constructor accepts ManagerRoles
        public Training(ManagerRole? student, ManagerRole? teacher, DateTime trainingStart)
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

        // CHANGED: Return type and parameter type
        public ManagerRole? ManagerStudent => _managerStudent;

        public void SetManagerStudent(ManagerRole? newStudent)
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

        // CHANGED: Return type and parameter type
        public ManagerRole? ManagerTeacher => _managerTeacher;

        public void SetManagerTeacher(ManagerRole? newTeacher)
        {
            if (_managerTeacher == newTeacher) return;

            // ManagerRole has the RankType property, so this logic remains valid
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