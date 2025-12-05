using System;
using System.Collections.Generic;

namespace TukitaSystem
{
    public class Shift
    {
        private static List<Shift> _extent = new List<Shift>();
        private static readonly string FilePath = "shifts.json";

        private static int _workingHours = 8;
        private TimeSpan _startAt;
        private TimeSpan _endAt;
        private double? _hoursWorked;
        
        public ShiftType Type { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
        public List<Employee> Employees { get; private set; } = new();
        public double ScheduledDuration => (EndAt - StartAt).TotalHours;

        public Shift(ShiftType type, DateTime date, TimeSpan startAt, TimeSpan endAt, List<Employee> assignedEmployees)
        {
            if (assignedEmployees == null || assignedEmployees.Count == 0)
                throw new ArgumentException("A shift must have at least one assigned employee.");
            
            if (endAt <= startAt)
                throw new ArgumentException("End time must be after start time.");

            Type = type;
            Date = date;
            StartAt = startAt;
            EndAt = endAt;
            IsPresent = false;
            foreach (var e in assignedEmployees)
                AddEmployee(e);

            _extent.Add(this);
        }

        public TimeSpan StartAt
        {
            get => _startAt;
            set
            {
                if (value >= EndAt && EndAt != default)
                    throw new ArgumentException("Start time must be before end time.");
                _startAt = value;
            }
        }

        public TimeSpan EndAt
        {
            get => _endAt;
            set
            {
                if (value <= StartAt && StartAt != default)
                    throw new ArgumentException("End time must be after start time.");
                _endAt = value;
            }
        }

        public double? HoursWorked
        {
            get => _hoursWorked;
            set
            {
                if (value.HasValue && value.Value < 0)
                    throw new ArgumentException("Hours worked cannot be negative.");
                _hoursWorked = value;
            }
        }
        
        public void AddEmployee(Employee employee)
        {
            if (!Employees.Contains(employee))
            {
                Employees.Add(employee);
                employee.AddShift(this);
            }
        }
        
        public void RemoveEmployee(Employee employee)
        {
            if (Employees.Count <= 1)
                throw new InvalidOperationException("A shift must have at least one employee.");

            if (Employees.Remove(employee))
            {
                employee.RemoveShift(this);
            }
        }

        public static List<Shift> GetExtent()
        {
            return new List<Shift>(_extent);
        }
        
        public static void SaveExtent()
        {
            StorageService.Save(_extent, FilePath);
        }

        public static void LoadExtent()
        {
            _extent = StorageService.Load<Shift>(FilePath);
        }
    }
}