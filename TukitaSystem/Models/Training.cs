using System;
using System.Collections.Generic;

namespace TukitaSystem
{
    public class Training
    {
        private static List<Training> _extent = new List<Training>();

        // Переменные для менеджера-студента и менеджера-учителя
        private Manager? _managerStudent;
        private Manager? _managerTeacher;

        public DateTime TrainingStart { get; set; }
        public DateTime? TrainingEnd { get; set; }
        public string? Comments { get; set; }

        // Конструктор теперь может принимать и студента, и учителя (опционально)
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

        // --- Свойства и методы для Студента ---

        public Manager? ManagerStudent => _managerStudent;

        public void SetManagerStudent(Manager? newStudent)
        {
            if (_managerStudent == newStudent) return;

            var oldStudent = _managerStudent;
            _managerStudent = newStudent;

            // 1. Убираем этот тренинг у старого студента
            if (oldStudent != null)
            {
                // Если у старого студента этот тренинг записан как текущий, обнуляем его
                if (oldStudent.StudentTraining == this)
                {
                    oldStudent.SetStudentTraining(null);
                }
            }

            // 2. Назначаем этот тренинг новому студенту
            if (_managerStudent != null)
            {
                // Это автоматически удалит старый тренинг у нового студента, если он был
                _managerStudent.SetStudentTraining(this);
            }
        }

        // --- Свойства и методы для Учителя ---

        public Manager? ManagerTeacher => _managerTeacher;

        public void SetManagerTeacher(Manager? newTeacher)
        {
            if (_managerTeacher == newTeacher) return;

            // Проверка на ранг перед назначением (на всякий случай, хотя Manager тоже проверяет)
            if (newTeacher != null && newTeacher.RankType != RankType.Lead)
            {
                throw new InvalidOperationException("Cannot assign a non-Lead manager as a teacher.");
            }

            var oldTeacher = _managerTeacher;
            _managerTeacher = newTeacher;

            // 1. Убираем тренинг из списка старого учителя
            if (oldTeacher != null)
            {
                oldTeacher.RemoveLeadingTraining(this);
            }

            // 2. Добавляем тренинг в список нового учителя
            if (_managerTeacher != null)
            {
                _managerTeacher.AddLeadingTraining(this);
            }
        }
    }
}