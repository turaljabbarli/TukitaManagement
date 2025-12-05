
namespace TukitaSystem;

public class Training
{
    private static List<Training> _extent = new List<Training>();
        
    
    private Manager? _managerStudent;

    public DateTime TrainingStart { get; set; }
    public DateTime? TrainingEnd { get; set; }
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

    public Manager? Manager
    {
        get => _managerStudent;
    }

    public void SetManager(Manager? newManager)
    {
        if (_managerStudent == newManager) return;
        
        var oldManager = _managerStudent;
        _managerStudent = newManager;

        if (oldManager != null)
        {
            oldManager.RemoveTraining(this);
        }
        
        if (_managerStudent != null)
        {
            _managerStudent.AddTraining(this);
        }
    }
}