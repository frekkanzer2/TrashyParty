using System.Collections.Generic;

public interface IMultipleMatchesManager
{
    List<MatchVictoryDto> TeamMatchVictories { get; set; }
    void InitializeTeamMatchVictories(List<TeamDto> teams);
    void SetMatchesVictoryLimit(int limit);
    void AddMatchVictory(int winnerTeamId);
    int? GetTeamIdThatReachedVictoriesLimit();
    void RestartMatch();
    void OnEveryMatchEnded();
}
