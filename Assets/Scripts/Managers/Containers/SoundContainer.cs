using System.Collections.Generic;
using Managers.Containers.Abstracts;
using Managers.Containers.Data;
using UnityEngine;

namespace Managers.Containers
{
    public class SoundContainer : MonoBehaviour, ISoundContainer
    {
        public List<SoundItemData> Items => _data.Items;
        [SerializeField] private SoundContainerData _data;
        [SerializeField] private AudioSource _source;
        
        public List<SoundItemData> GetByType(SoundType type)
        {
            return _data.Items.FindAll(x => x.Type == type);
        }

        public SoundItemData GetRandomByType(SoundType type)
        {
            var items = GetByType(type);
            return items[Random.Range(0, items.Count)];
        }

        public void PlayItem(AudioSource source, SoundItemData item)
        {
            source.Stop();
            source.clip = item.Audio;
            source.loop = item.Loop;
            source.Play();
        }

        public void PlayItem(SoundItemData item)
        {
            PlayItem(_source, item);
        }

        public void PlayByType(SoundType type)
        {
            PlayByType(type, _source);
        }

        public void PlayByType(SoundType type, AudioSource source)
        {
            var item = GetRandomByType(type);
            PlayItem(source, item);
        }
    }
}