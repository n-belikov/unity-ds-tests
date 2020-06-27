using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory.Repository
{
    [System.Serializable]
    public class InventoryRepository : IInventoryRepository
    {
        public List<InventoryItem> Items => _items;
        public int Count => _items.Count;

        [SerializeField] private List<InventoryItem> _items;

        public InventoryRepository()
        {
            _items = new List<InventoryItem>();
        }

        public InventoryItem Get(int index)
        {
            return _items[index];
        }

        public void AddItem(InventoryItem item)
        {
            _items.Add(item);
        }

        public bool Remove(InventoryItem item)
        {
            return _items.Remove(item);
        }

        public IEnumerable<InventoryItem> All()
        {
            return _items;
        }

        public InventoryItem FindByType<T>(Predicate<T> match, Predicate<InventoryItem> matchItem) where T : class
        {
            return _items.Find(x => (x.Item as T) != null && match(x.Item as T) && matchItem(x));
        }

        public InventoryItem FindByType<T>(Predicate<T> match) where T : class
        {
            return _items.Find(x => (x.Item as T) != null && match(x.Item as T));
        }

        public InventoryItem Find(Predicate<InventoryItem> match)
        {
            return _items.Find(match);
        }

        public IEnumerable<InventoryItem> GetEquipItemsByType(Type type)
        {
            return _items.Where(x => {
                if (!x.equip) {
                    return false;
                }

                return x.Item.GetType() == type;
            });
        }

        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
        }
    }

    public interface IInventoryRepository
    {
        List<InventoryItem> Items { get; }

        int Count { get; }

        InventoryItem Get(int index);

        void AddItem(InventoryItem item);

        bool Remove(InventoryItem item);

        IEnumerable<InventoryItem> All();

        InventoryItem Find(Predicate<InventoryItem> match);

        InventoryItem FindByType<T>(Predicate<T> match, Predicate<InventoryItem> matchItem) where T : class;

        InventoryItem FindByType<T>(Predicate<T> match) where T : class;

        IEnumerable<InventoryItem> GetEquipItemsByType(Type type);

        void RemoveAt(int index);
    }
}