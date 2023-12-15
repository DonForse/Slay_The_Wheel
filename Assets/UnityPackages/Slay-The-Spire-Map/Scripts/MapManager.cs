using System.Linq;
using Features;
using Newtonsoft.Json;
using UnityEngine;

namespace UnityPackages.Slay_The_Spire_Map.Scripts
{
    public class MapManager : MonoBehaviour
    {
        public MapConfig config;
        public MapView view;

        public Map CurrentMap { get; private set; }

        private void Start()
        {
            if (PlayerPrefs.HasKey(GetKey()))
            {
                var mapJson =  PlayerPrefs.GetString(GetKey());
                var map = JsonConvert.DeserializeObject<Map>(mapJson);
                // using this instead of .Contains()
                if (HasReachedBossNode(map))
                {
                    GenerateNewMap();
                    StartCoroutine(view.PanMapEndToStart(4f));
                }
                else
                {
                    CurrentMap = map;
                    view.ShowMap(map);
                }
            }
            else
            {
                GenerateNewMap();
                StartCoroutine(view.PanMapEndToStart(4f));
            }
        }

        private static bool HasReachedBossNode(Map map) => map.path.Any(p => p.Equals(map.GetBossNode().point));

        public void GenerateNewMap()
        {
            var map = MapGenerator.GetMap(config);
            CurrentMap = map;
            view.ShowMap(map);
        }

        public void SaveMap()
        {
            if (CurrentMap == null) return;

            var json = JsonConvert.SerializeObject(CurrentMap, Formatting.Indented,
                new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            PlayerPrefs.SetString(GetKey(), json);
            PlayerPrefs.Save();
        }

        private void OnApplicationQuit()
        {
            SaveMap();
        }

        private static string GetKey() => RunRepository.GetCurrent() + "_Map";
    }
}
