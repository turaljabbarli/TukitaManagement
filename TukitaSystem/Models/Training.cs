
namespace TukitaSystem;

public class Training
{
    private static List<Training> _extent = new List<Training>();
        
    
    private Manager? _manager;

    public DateTime TrainingStart { get; set; }
    public DateTime? TrainingEnd { get; set; }
    public string? Comments { get; set; }

    public Training(Manager manager, DateTime trainingStart)
    {
        
        if (manager != null && manager.RankType != RankType.Lead)
        {
            throw new InvalidOperationException("Only managers with 'Lead' rank can conduct training.");
        }

        TrainingStart = trainingStart;
        
        if (manager != null)
        {
            SetManager(manager);
        }

        _extent.Add(this);
    }

    public Manager? Manager
    {
        get => _manager;
    }

    public void SetManager(Manager? newManager)
    {
        if (_manager == newManager) return;
        
        if (newManager != null && newManager.RankType != RankType.Lead)
        {
            throw new InvalidOperationException("Only managers with 'Lead' rank can conduct training.");
        }
        
        var oldManager = _manager;
        _manager = newManager;

        if (oldManager != null)
        {
            oldManager.RemoveTraining(this);
        }
        
        if (_manager != null)
        {
            _manager.AddTraining(this);
        }
    }
}