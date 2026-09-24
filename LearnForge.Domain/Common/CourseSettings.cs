namespace LearnForge.Domain.Common;

public record CourseSettings(
    bool StreaksEnabled,
    bool HeartsEnabled,
    int MaxHearts,
    int XpPerExercise,
    bool WeeklyLeaderboardEnabled
)
{
    public static CourseSettings Default => new(true, true, 5, 10, true);
}