using System;
using Quest.Data.Abstracts;
using UnityEngine;

namespace Quest.Entities
{
    public class StartQuestHandler : MonoBehaviour, IStartQuestHandler
    {
        public IQuest Quest => _quest;
        [SerializeField] private QuestContainer _quest;

        private void OnTriggerEnter(Collider other)
        {
            // TODO: Start quest
        }
    }

    public interface IStartQuestHandler
    {
        IQuest Quest { get; }
    }
}