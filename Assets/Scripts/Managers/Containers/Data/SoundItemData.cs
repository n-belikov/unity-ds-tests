using UnityEngine;
using System.Collections.Generic;
using Managers.Containers.Abstracts;

namespace Managers.Containers.Data
{
    [CreateAssetMenu(fileName = "Item", menuName = "Sound/Item")]
    public class SoundItemData : ScriptableObject
    {
        public SoundType Type => _type;
        public AudioClip Audio => _audio;
        public bool Loop => _loop;

        [SerializeField] private bool _loop = false;
        [SerializeField] private AudioClip _audio;
        [SerializeField] private SoundType _type;
    }

    [CreateAssetMenu(fileName = "Container", menuName = "Sound/Container")]
    public class SoundContainerData : ScriptableObject
    {
        public List<SoundItemData> Items = new List<SoundItemData>();
    }
}