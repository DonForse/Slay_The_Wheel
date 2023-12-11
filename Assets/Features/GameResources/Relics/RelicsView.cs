using System.Collections.Generic;
using System.Linq;
using Features.Common;
using Features.Maps;
using UnityEngine;

namespace Features.GameResources.Relics
{
    public class RelicsView : MonoBehaviour
    {
        [SerializeField] private Transform container;
        [SerializeField] private RelicView relicViewPrefab;
        [SerializeField] private RelicsScriptableObject relicsScriptableObject;
        private PlayerPrefsRelicsRepository _playerPrefsRelicsRepository;
        private List<RelicType> _currentRelics = new ();
        private List<RelicView> _instantiatedRelics = new ();

        private void OnEnable()
        {
            _playerPrefsRelicsRepository = Provider.PlayerPrefsRelicsRepository();
            _playerPrefsRelicsRepository.Changed += OnRelicsChanged;
            UpdateRelics(_playerPrefsRelicsRepository.Get());
            _currentRelics = _playerPrefsRelicsRepository.Get();
        }

        private void OnDisable()
        {
            _playerPrefsRelicsRepository.Changed -= OnRelicsChanged;
        }

        private void UpdateRelics(List<RelicType> relicTypes)
        {
            var relicsToAdd = relicTypes.Where(r => !_currentRelics.Contains(r));
            var relicsToRemove = _currentRelics.Where(r => !relicTypes.Contains(r));
            foreach (var relic in relicsToAdd)
            {
                var relicScriptableObject = relicsScriptableObject.Find(relic);
                var go = Instantiate(relicViewPrefab, container);
                go.Set(relicScriptableObject);
                _instantiatedRelics.Add(go);
                _currentRelics.Add(relic);
            }

            foreach (var relic in relicsToRemove)
            {
                Destroy(_instantiatedRelics.First(x => x.RelicId == relic).gameObject);
            }

        }

        private void OnRelicsChanged(object sender, List<RelicType> e) => UpdateRelics(e);
    }
}