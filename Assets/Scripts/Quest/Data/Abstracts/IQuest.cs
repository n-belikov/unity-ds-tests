using System.Collections.Generic;
using UnityEngine;

namespace Quest.Data.Abstracts
{
    public class QuestContainer : ScriptableObject, IQuest
    {
        public List<IQuestItem> Items => _items;

        private List<IQuestItem> _items = new List<IQuestItem>();
    }
    
    public interface IQuest
    {
        List<IQuestItem> Items { get; }
    }
}