using CCamera;
using CryoDI;
using Inputs;
using Inventory;
using Managers.Containers;
using Managers.Containers.Abstracts;
using Quest;
using Stats.Abstracts;
using Stats.Player;
using UnityEngine;

namespace App
{
    public class GameManager : UnityStarter, IGameManager
    {
        public static CryoContainer Resolve { get; private set; }

        [SerializeField] private CameraController _camera;
        [SerializeField] private PlayerStatsManager _playerStatsManager;
        [SerializeField] private InputHandler _inputHandler;
        [SerializeField] private InventoryManager _inventoryManager;
        [SerializeField] private QuestManager _questManager;
        [SerializeField] private EffectContainer _effectContainer;
        [SerializeField] private SoundContainer _soundContainer;

        protected override void SetupContainer(CryoContainer container)
        {
            Resolve = container;
            container.RegisterInstance<ICamera>(_camera);
            container.RegisterInstance<IGameManager>(this);
            container.RegisterInstance<IInputHandler>(_inputHandler);
            container.RegisterInstance<IPlayerStats>(_playerStatsManager);
            container.RegisterInstance<IInventoryManager>(_inventoryManager);
            container.RegisterInstance<IQuestManager>(_questManager);
            container.RegisterInstance<IEffectContainer>(_effectContainer);
            container.RegisterInstance<ISoundContainer>(_soundContainer);
        }
    }

    public interface IGameManager
    {
    }
}