using Inventory.Item.Abstracts;
using UnityEngine;

namespace Inventory.Item
{
    public class Arrow : BaseArrow
    {
        [SerializeField] private GameObject _trail;
        
        public override void Shot()
        {
            base.Shot();
            _trail.SetActive(true);
        }
    }
}