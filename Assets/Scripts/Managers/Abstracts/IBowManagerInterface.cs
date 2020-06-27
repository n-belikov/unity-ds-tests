using UnityEngine.Events;

namespace Managers.Abstracts
{
    public interface IBowManagerInterface
    {
        UnityEvent OnDrawEvent { get; }
        UnityEvent OnUnDrawEvent { get; }
        UnityEvent OnShootingEvent { get; }
    }
}