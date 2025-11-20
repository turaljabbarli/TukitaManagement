using System;

namespace TukitaSystem
{
    public class Menu
    {
        private string _name;
        private TimeSpan _startTime;
        private TimeSpan _endTime;

        public Menu(string name, TimeSpan startTime, TimeSpan endTime)
        {
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
            IsAvailable = true;
        }

        public bool IsAvailable { get; set; }
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Menu name cannot be empty.");
                
                _name = value;
            }
        }

        public TimeSpan StartTime
        {
            get => _startTime;
            set
            {
                if (value < TimeSpan.Zero || value >= TimeSpan.FromDays(1))
                    throw new ArgumentException("Start time must be a valid time of day.");

                _startTime = value;
            }
        }

        public TimeSpan EndTime
        {
            get => _endTime;
            set
            {
                if (value < TimeSpan.Zero || value >= TimeSpan.FromDays(1))
                    throw new ArgumentException("End time must be a valid time of day.");

                if (StartTime != default && value <= StartTime)
                    throw new ArgumentException("End time must be later than start time.");

                _endTime = value;
            }
        }

        public string ServingInterval()
        {
            return $"{StartTime:hh\\:mm} - {EndTime:hh\\:mm}";
        }
    }
}