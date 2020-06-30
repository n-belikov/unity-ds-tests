using System.Collections.Generic;
using Managers.Containers.Data;
using UnityEngine;

namespace Managers.Containers.Abstracts
{
    public enum SoundType
    {
        InventoryItemClick
    }
    
    public interface ISoundContainer
    {
        List<SoundItemData> Items { get; }

        List<SoundItemData> GetByType(SoundType type);
        
        SoundItemData GetRandomByType(SoundType type);

        void PlayItem(AudioSource source, SoundItemData item);

        void PlayItem(SoundItemData item);

        void PlayByType(SoundType type);

        void PlayByType(SoundType type, AudioSource source);
    }
}