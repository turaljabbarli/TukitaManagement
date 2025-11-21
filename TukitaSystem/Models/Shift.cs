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

        public Shift(ShiftType type, DateTime date, TimeSpan startAt, TimeSpan endAt, Employee assignedEmployee)
        {
            if (endAt <= startAt)
                throw new ArgumentException("End time must be after start time.");

            if (assignedEmployee == null)
                throw new ArgumentNullException(nameof(assignedEmployee), "Shift must be assigned to an employee.");

            Type = type;
            Date = date;
            StartAt = startAt;
            EndAt = endAt;
            AssignedEmployee = assignedEmployee;
            IsPresent = false;

            _extent.Add(this);
        }

        public ShiftType Type { get; set; }
        public DateTime Date { get; set; }
        public Employee AssignedEmployee { get; set; }
        public bool IsPresent { get; set; }

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

        public double ScheduledDuration => (EndAt - StartAt).TotalHours;

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