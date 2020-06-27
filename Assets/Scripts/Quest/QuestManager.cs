using System;
using CryoDI;
using Inputs;
using Quest.Data.Abstracts;
using UnityEngine;

namespace Quest
{
    public class QuestManager : MonoBehaviour, IQuestManager
    {
        [Dependency] private IInputHandler _inputHandler { get; set; }

        private bool _inQuest = false;

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            if (_inQuest) {
                InQuestUpdate(deltaTime);
            }
        }

        private void InQuestUpdate(float deltaTime)
        {
            
        }

        public void StartQuest(IQuest quest)
        {
        }
    }

    public interface IQuestManager
    {
        void StartQuest(IQuest quest);
    }
}