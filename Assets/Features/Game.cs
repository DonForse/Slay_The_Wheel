using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.Battles;
using Features.Cards;
using Features.Maps;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Features
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private BaseCardsScriptableObject unitsDb;
        [SerializeField] private BaseCardsScriptableObject enemiesDb;
        [SerializeField] private BaseCardsScriptableObject heroesDb;

        private List<RunCard> _deck;
        private Battle _battleGo;
        private Maps.Map _mapGo;
        private RunCard _heroCard;

        // Start is called before the first frame update
        IEnumerator Start()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            DontDestroyOnLoad(this.gameObject);
            AddInitialCards();

            yield return LoadBattleSceneCoroutine();
        }

        private void AddInitialCards()
        {
            _deck = new List<RunCard>();
            var heroCardDb = heroesDb.cards.FirstOrDefault(x => x.cardName.Contains("Kael Fireforge"));
            _heroCard = new RunCard(heroCardDb);
            var recruits = unitsDb.cards.FirstOrDefault(x => x.cardName.Contains("Fireborn Recruit"));
            var warrior = unitsDb.cards.FirstOrDefault(x => x.cardName.Contains("Fireborn Warrior"));
            var sorceress = unitsDb.cards.FirstOrDefault(x => x.cardName.Contains("Firestorm Sorceress"));

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
            switch (PlayerPrefs.GetInt("CurrentLevel"))
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
            var slime = enemiesDb.cards.FirstOrDefault(x => x.cardName.Contains("Slime"));
            var zombie = enemiesDb.cards.FirstOrDefault(x => x.cardName.Contains("Zombie"));
            var spider = enemiesDb.cards.FirstOrDefault(x => x.cardName.Contains("Spider"));
            return PlayerPrefs.GetInt("CurrentLevel") switch
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
            if (!playerWin)
                return;
            StartCoroutine(LoadMapSceneCoroutine());
        }

        private void OnShopPackObtained(object sender, List<BaseCardScriptableObject> cards)
        {
            foreach (var card in cards)
            {
                var runCard = new RunCard(card);
                _deck.Add(runCard);
            }
        }

        private IEnumerator LoadBattleSceneCoroutine()
        {
            if (_mapGo != null)
            {
                _mapGo.SelectedPack -= OnShopPackObtained;
                _mapGo.SelectedRest -= OnSelectedRest;
                _mapGo.MinorEnemySelected -= OnMinorEnemySelected;
            }

            ShuffleDeck();
            SceneManager.LoadScene("Battle");
            yield return new WaitForSeconds(.5f);
            _battleGo = GameObject.Find("Battle").GetComponent<Battle>();
            yield return _battleGo.Initialize(_deck, GetBattleEnemies(), 5, GetEnemyWheelSize(), _heroCard);

            if (_battleGo != null)
                _battleGo.BattleFinished += BattleComplete;
        }

        private IEnumerator LoadMapSceneCoroutine()
        {
            if (_battleGo != null)
                _battleGo.BattleFinished -= BattleComplete;

            SceneManager.LoadScene("Map");
            yield return new WaitForSeconds(.5f);
            _mapGo = GameObject.Find("Map").GetComponent<Maps.Map>();
            RemoveDeadCardsFromDeck();

            if (_mapGo != null)
            {
                _mapGo.SelectedPack += OnShopPackObtained;
                _mapGo.SelectedRest += OnSelectedRest;
                _mapGo.MinorEnemySelected += OnMinorEnemySelected;
            }
        }

        private void OnSelectedRest(object sender, EventArgs e)
        {
            foreach (var card in _deck)
            {
                card.Hp = Math.Min(card.Hp + Mathf.FloorToInt(card.Hp * 0.2f), card.baseCard.hp);
            }
        }

        private void OnMinorEnemySelected(object sender, EventArgs e)
        {
            StartCoroutine(LoadBattleSceneCoroutine());
        }
    }
}