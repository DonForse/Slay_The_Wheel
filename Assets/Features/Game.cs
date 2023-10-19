using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.Battles;
using Features.Cards;
using Features.Maps;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private BaseCardsScriptableObject cardsDb;

        private List<RunCard> _deck;
        private int _currentLevel = 0;
        private Battle _battleGo;
        private Map _mapGo;

        // Start is called before the first frame update
        IEnumerator Start()
        {
            DontDestroyOnLoad(this.gameObject);
            AddInitialCards();

            yield return LoadBattleSceneCoroutine();
        }

        private void AddInitialCards()
        {
            _deck = new List<RunCard>();
            var heroCardDb = cardsDb.cards.FirstOrDefault(x => x.cardName.Contains("Kael Fireforge"));
            var heroCard = new RunCard(heroCardDb);
            _deck.Add(heroCard);
            var recruits = cardsDb.cards.FirstOrDefault(x => x.cardName.Contains("Fireborn Recruit"));
            var warrior = cardsDb.cards.FirstOrDefault(x => x.cardName.Contains("Fireborn Warrior"));
            var sorceress = cardsDb.cards.FirstOrDefault(x => x.cardName.Contains("Firestorm Sorceress"));

            for (var i = 0; i < 12; i++)
            {
                var playerUnit = new RunCard(recruits);
                _deck.Add(playerUnit);
            }

            for (var i = 0; i < 3; i++)
            {
                var playerUnit = new RunCard(warrior);
                _deck.Add(playerUnit);
            }

            for (var i = 0; i < 2; i++)
            {
                var playerUnit = new RunCard(sorceress);
                _deck.Add(playerUnit);
            }
        }
    
        private void ShuffleDeck() => _deck = _deck.OrderBy(d => Random.Range(0, 100f)).ToList();

        private void RemoveDeadCardsFromDeck() => _deck = _deck.Where(x => !x.IsDead).ToList();

        private int GetEnemyWheelSize()
        {
            switch (_currentLevel)
            {
                case 0:
                    return 3;
                case 1:
                    return 4;
                case 2:
                case 3:
                    return 3;
                default:
                    return 3;
            }
        }

        private List<RunCard> GetBattleEnemies()
        {
            var slime = cardsDb.cards.FirstOrDefault(x => x.cardName.Contains("Slime"));
            var zombie = cardsDb.cards.FirstOrDefault(x => x.cardName.Contains("Zombie"));
            var spider = cardsDb.cards.FirstOrDefault(x => x.cardName.Contains("Spider"));
            return _currentLevel switch
            {
                0 => new List<RunCard>() { new RunCard(slime), new RunCard(slime), new RunCard(slime) },
                1 => new List<RunCard>()
                {
                    new RunCard(slime), new RunCard(slime), new RunCard(slime), new RunCard(zombie),
                },
                2 => new List<RunCard>() { new RunCard(zombie), new RunCard(spider), new RunCard(slime) },
                _ => new List<RunCard>() { new RunCard(spider), new RunCard(spider), new RunCard(spider) }
            };
        }

        private void BattleComplete(object sender, bool playerWin)
        {
            
            _currentLevel++;
            Debug.Log($"<color=cyan>{_currentLevel}</color>");
            if (!playerWin)
                return;
            StartCoroutine(LoadMapSceneCoroutine());
        }

        private void OnMapStageComplete(object sender, BaseCardScriptableObject e)
        {
            var runCard = new RunCard(e);
            _deck.Add(runCard);
            StartCoroutine(LoadBattleSceneCoroutine());
        }

        private IEnumerator LoadBattleSceneCoroutine()
        {
            if (_mapGo != null)
                _mapGo.SelectedUpgradeCard -= OnMapStageComplete;
            
            ShuffleDeck();
            SceneManager.LoadScene("Battle");
            yield return new WaitForSeconds(.5f);
            _battleGo = GameObject.Find("Battle").GetComponent<Battle>();
            yield return _battleGo.Initialize(_deck,GetBattleEnemies(), 5, GetEnemyWheelSize());
            
            if (_battleGo != null)
                _battleGo.BattleFinished += BattleComplete;
        }

        private IEnumerator LoadMapSceneCoroutine()
        {
            if (_battleGo != null)
                _battleGo.BattleFinished -= BattleComplete;
            
            SceneManager.LoadScene("Map");
            yield return new WaitForSeconds(.5f);
            _mapGo = GameObject.Find("Map").GetComponent<Map>();
            _mapGo.Initialize(_currentLevel);
            RemoveDeadCardsFromDeck();
            
            if (_mapGo != null)
                _mapGo.SelectedUpgradeCard += OnMapStageComplete;
        }
    }
}