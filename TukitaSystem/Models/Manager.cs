
namespace TukitaSystem
{
    public class Manager : Employee
    {
        private int _yearsOfExperience;
        
        private List<Training> _trainings = new List<Training>();

        public Manager(string name, string surname, string passportNumber, DateTime birthDate, decimal baseSalary, DateTime employmentDate, RankType rankType, int yearsOfExperience)
            : base(name, surname, passportNumber, birthDate, baseSalary, employmentDate)
        {
            RankType = rankType;
            YearsOfExperience = yearsOfExperience;
        }
        public RankType RankType { get; set; }

        public int YearsOfExperience
        {
            get => _yearsOfExperience;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Years of experience cannot be negative.");
                _yearsOfExperience = value;
            }
        }
        
        
        public IReadOnlyCollection<Training> Trainings => _trainings.AsReadOnly();

        public void AddTraining(Training training)
        {
            if (training == null) throw new ArgumentNullException(nameof(training));

            
            if (this.RankType != RankType.Lead)
            {
                throw new InvalidOperationException("This manager does not have the 'Lead' rank and cannot conduct training.");
            }

            if (!_trainings.Contains(training))
            {
                _trainings.Add(training);
                
                
                if (training.Manager != this)
                {
                    training.SetManager(this);
                }
            }
        }

        public void RemoveTraining(Training training)
        {
            if (training == null) return;

            if (_trainings.Contains(training))
            {
                _trainings.Remove(training);

                
                if (training.Manager == this)
                {
                    training.SetManager(null);
                }
            }
        }
    }
}