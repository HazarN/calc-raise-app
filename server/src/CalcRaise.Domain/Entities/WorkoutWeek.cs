using CalcRaise.Domain.Common;

namespace CalcRaise.Domain.Entities;

/// <summary>Anchors a calendar week so "current week" / "previous week" are well-defined and orderable.</summary>
public class WorkoutWeek : BaseEntity
{
    public DateOnly WeekStartDate { get; set; }
    public int WeekNumber { get; set; }

    public ICollection<WorkoutSetLog> SetLogs { get; set; } = new List<WorkoutSetLog>();
}
