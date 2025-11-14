using System;

namespace TukitaSystem
{
    public class Education
    {
        public string Degree { get; set; }
        public string Institution { get; set; }

        public Education(string degree, string institution)
        {
            if (string.IsNullOrWhiteSpace(degree))
                throw new ArgumentException("Degree cannot be empty.");
            if (string.IsNullOrWhiteSpace(institution))
                throw new ArgumentException("Institution cannot be empty.");

            Degree = degree;
            Institution = institution;
        }

        public override string ToString()
        {
            return $"{Degree} from {Institution}";
        }
    }
}