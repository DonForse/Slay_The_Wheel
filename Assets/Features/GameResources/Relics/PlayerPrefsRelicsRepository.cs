using System;
using System.Collections.Generic;
using Features.Maps;
using Newtonsoft.Json;
using UnityEngine;

namespace Features.GameResources.Relics
{
    public class PlayerPrefsRelicsRepository
    {
        public event EventHandler<List<RelicType>> Changed;

        public void Add(RelicType relic)
        {
            var relics = Get();
            relics.Add(relic);
            Save(relics);
            Changed?.Invoke(this, relics);
        }

        public void Remove(RelicType relic)
        {
            var relics = Get();
            relics.Remove(relic);
            Save(relics);
            Changed?.Invoke(this, relics);
        }

        public List<RelicType> Get()
        {
            var relicsSerialized= PlayerPrefs.GetString(Key(), "");
            return JsonConvert.DeserializeObject<List<RelicType>>(relicsSerialized) ?? new List<RelicType>();
        }

        private void Save(List<RelicType> relics)
        {
            var relicsSerialized = JsonConvert.SerializeObject(relics);
            PlayerPrefs.SetString(Key(), relicsSerialized);
        }

        private static string Key() => RunRepository.GetCurrent() + "_RELICS";    }
}