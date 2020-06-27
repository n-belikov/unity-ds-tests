using System;
using System.Collections.Generic;
using App;
using CryoDI;
using Inventory;
using Inventory.Item.Data.Abstracts;
using Stats.Abstracts;
using UnityEngine;

namespace Stats.Player
{
    public class PlayerStatsManager : MonoBehaviour, IPlayerStats
    {
        [SerializeField] private int _startHealth = 100, _health = 0;
        [SerializeField] private PlayerStatsUI _playerStatsUi;
        public IInventoryManager InventoryManager { get; private set; }
        public int MaxHealth { get; private set; } = 0;

        [System.Serializable]
        public class Mod
        {
            public float Percent => percent / 100f;
            [SerializeField, Range(0, 100)] private int percent = 0;
        }

        public int Health
        {
            get => _health;
            protected set => _health = value;
        }

        private void Awake()
        {
            MaxHealth = Health = _startHealth;
            UpdateUi();
        }

        private void Start()
        {
            InventoryManager = GameManager.Resolve.Resolve<IInventoryManager>();
        }

        public void AddHealth(int health)
        {
            Health = Mathf.Clamp(Health + health, 0, MaxHealth);
            UpdateUi();
        }

        public int GetDamage(WeaponType type)
        {
            var item = InventoryManager.GetEquipWeapon(type);

            var weapon = item?.Item as IWeaponItemData;
            if (weapon == null) {
                return 0;
            }

            var damage = weapon.Attack;

            return damage;
        }

        private void UpdateUi()
        {
            _playerStatsUi.SetFillImage((int) Health, (int) MaxHealth);
        }
    }
}