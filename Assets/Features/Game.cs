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
            if (_currentLevel == 0)
            {
                return 3;
            }

            return 0;
        }

        private List<RunCard> GetBattleEnemies()
        {
            var enemy3 = cardsDb.cards.FirstOrDefault(x => x.cardName.Contains("Slime"));
            var enemy1 = cardsDb.cards.FirstOrDefault(x => x.cardName.Contains("Zombie"));
            var enemy2 = cardsDb.cards.FirstOrDefault(x => x.cardName.Contains("Spider"));
            if (_currentLevel == 0)
            {
                return new List<RunCard>() { new RunCard(enemy1), new RunCard(enemy2), new RunCard(enemy3) };
            }
            return new List<RunCard>() { new RunCard(enemy1), new RunCard(enemy2), new RunCard(enemy3) };    

        }

        private void BattleComplete(object sender, bool playerWin)
        {
            _currentLevel++;
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
            ShuffleDeck();

            SceneManager.LoadScene("Battle");
            yield return new WaitForSeconds(.5f);
            var battleGo = GameObject.Find("Battle").GetComponent<Battle>();
            yield return battleGo.Initialize(_deck,GetBattleEnemies(), 5, GetEnemyWheelSize());
            battleGo.BattleFinished += BattleComplete;
        }

        private IEnumerator LoadMapSceneCoroutine()
        {
            SceneManager.LoadScene("Map");
            yield return new WaitForSeconds(.5f);
            var mapGo = GameObject.Find("Map").GetComponent<Map>();
            mapGo.Initialize(_currentLevel);
            RemoveDeadCardsFromDeck();
            mapGo.SelectedUpgradeCard += OnMapStageComplete;
        }
    }
}