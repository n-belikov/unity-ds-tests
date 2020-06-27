using System;
using UnityEngine;
using UnityEngine.Events;

namespace Utility
{
    public class BaseUnityEvent : UnityEvent  {}
    
    public class GameObjectEvent : UnityEvent<GameObject> {}
    
    public class IntEvent : UnityEvent<int> {}
    
    [Serializable]
    public class BoolEvent : UnityEvent<bool> {}
    
}