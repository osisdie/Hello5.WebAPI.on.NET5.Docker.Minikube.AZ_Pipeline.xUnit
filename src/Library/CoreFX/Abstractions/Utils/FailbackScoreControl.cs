using System.Threading;

namespace CoreFX.Abstractions.Utils
{
    public sealed class FailbackScoreControl
    {
        public const int InitScore = 0;
        public const int SuccessScore = -1;
        public const int FailScore = 2;
        public const int ExceedScore = 7;
        public static IFailbackScore CreateNew => new FailbackScore();
    }

    public interface IFailbackScore
    {
        int Score(bool? isSuccess);
        int Success();
        int Fail();
        int Reset();
        bool IsExceedLimit();
    }

    public class FailbackScore : FailbackScoreBase
    {

    }

    public class FailbackScoreBase : IFailbackScore
    {
        public int Score(bool? isSuccess)
        {
            return true == isSuccess ? Success() : Fail();
        }

        public int Success()
        {
            if (Interlocked.Add(ref Scores, SuccessScore) <= 0)
            {
                return Interlocked.Exchange(ref Scores, FailbackScoreControl.InitScore);
            }

            return Scores;
        }

        public int Fail() => Interlocked.Add(ref Scores, FailScore);
        public int Reset() => Interlocked.Exchange(ref Scores, FailbackScoreControl.InitScore);
        public bool IsExceedLimit() => Scores >= ExceedScore;

        public int SuccessScore { get; set; } = FailbackScoreControl.SuccessScore;
        public int FailScore { get; set; } = FailbackScoreControl.FailScore;
        public int ExceedScore { get; set; } = FailbackScoreControl.ExceedScore;

        public int Scores = FailbackScoreControl.InitScore;
    }
}
