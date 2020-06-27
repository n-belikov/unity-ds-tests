﻿using CCamera;
using CryoDI;
using Inputs;
using Inventory;
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

        protected override void SetupContainer(CryoContainer container)
        {
            Resolve = container;
            container.RegisterInstance<ICamera>(_camera);
            container.RegisterInstance<IGameManager>(this);
            container.RegisterInstance<IInputHandler>(_inputHandler);
            container.RegisterInstance<IPlayerStats>(_playerStatsManager);
            container.RegisterInstance<IInventoryManager>(_inventoryManager);
        }
    }

    public interface IGameManager
    {
    }
}