using UnityEngine;

namespace Stats.Abstracts
{
    // ReSharper disable once IdentifierTypo
    public interface IDamagable
    {
        // ReSharper disable once InconsistentNaming
        GameObject gameObject { get; }
        void TakeDamage(int value, IDamagable from);
    }
}