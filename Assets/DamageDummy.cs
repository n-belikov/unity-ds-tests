using System.Collections;
using System.Collections.Generic;
using Stats.Abstracts;
using UnityEngine;

public class DamageDummy : MonoBehaviour, IDamagable
{
    public void TakeDamage(int value, IDamagable from)
    {
        Debug.Log($"Damage: {value} from {from.gameObject.name}");
    }
}
