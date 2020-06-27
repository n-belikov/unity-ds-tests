using System;
using System.Collections.Generic;
using Stats.Effects;

namespace Stats
{
    public class StatsRepository // : IStatsRepositoryInterface
    {
        
    }

    public interface IStatsRepositoryInterface
    {
        // Stats
        int Health { get; }

        // Attacks
        int PhysicalAttack { get; }
        
        // Protections
        int PhysicalProtection { get; }
        
        // TODO: ??? maybe delete
        // void AddHealthAffection(IEffectInterface effect);

        // IEnumerable<IEffectInterface> AllEffects();
    }
}