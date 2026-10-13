namespace Arcade2048.Domain.Users.Dtos
{
    public class SubmitScoreResultDto
    {
        public bool IsNewBest { get; set; }
        public int BestScore { get; set; }
    }
}
