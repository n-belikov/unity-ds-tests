﻿using UnityEngine;
using UnityEngine.Events;

namespace Managers.Abstracts
{
    public interface ISwordManagerInterface
    {
        UnityEvent OnStartSlashEvent { get; }
        UnityEvent OnEndSlashEvent { get; }
        void InvokeDamage(GameObject gameObject);
    }
}