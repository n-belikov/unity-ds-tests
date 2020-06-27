namespace Stats.Effects
{
    public interface IEffectInterface
    {
        int Percent { get; }

        int Calculate(IStatsRepositoryInterface statsRepository);

        int Timeout { get; }
    }
}