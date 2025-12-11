using System;
using System.Collections.Generic;

namespace TukitaSystem
{
    public class Training
    {
        private static List<Training> _extent = new List<Training>();
        private static readonly string FilePath = "trainings.json";

        private Manager? _managerStudent; // This variable name from your snippet suggests the 'student', but Manager.cs implies logic for the 'Trainer'. Kept as is to match your request.
        private DateTime _trainingStart;
        private DateTime? _trainingEnd;

        public string? Comments { get; set; }

        public Training(Manager studentManager, DateTime trainingStart)
        {
            TrainingStart = trainingStart;
            
            if (studentManager != null)
            {
                SetManager(studentManager);
            }

            _extent.Add(this);
        }

        public DateTime TrainingStart
        {
            get => _trainingStart;
            set
            {
                if (_trainingEnd.HasValue && value > _trainingEnd.Value)
                    throw new ArgumentException("Training start cannot be after training end.");
                _trainingStart = value;
            }
        }

        public DateTime? TrainingEnd
        {
            get => _trainingEnd;
            set
            {
                if (value.HasValue && value.Value < _trainingStart)
                    throw new ArgumentException("Training end cannot be before training start.");
                _trainingEnd = value;
            }
        }

        public Manager? Manager
        {
            get => _managerStudent;
        }

        // --- STRICT RECURSION GUARD PATTERN (Reference Side) ---

        public void SetManager(Manager? newManager)
        {
            // 1. THE CRITICAL CHECK (Recursion Guard)
            if (_managerStudent == newManager)
            {
                return; // Stop loop
            }

            // 2. Swap Logic
            var oldManager = _managerStudent;
            _managerStudent = newManager;

            // 3. Trigger Reverse Connections
            
            // Remove from the old manager list
            if (oldManager != null)
            {
                oldManager.RemoveTraining(this);
            }
            
            // Add to the new manager list
            if (_managerStudent != null)
            {
                _managerStudent.AddTraining(this);
            }
        }

        public static List<Training> GetExtent() => new List<Training>(_extent);
        public static void SaveExtent() => StorageService.Save(_extent, FilePath);
        public static void LoadExtent() => _extent = StorageService.Load<Training>(FilePath);
    }
}