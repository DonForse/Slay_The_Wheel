using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.Battles;
using Features.Cards;
using Features.Maps;
using Features.PostBattles;
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
        private List<Relic> _relics = new();
        private Battle _battleGo;
        private Map _mapGo;
        private PostBattle _postBattle;
        private HeroRunCard _heroCard;

        private int minorBattlesAmount = 0;
        private int majorBattlesAmount = 0;
        private int bossBattlesAmount = 0;

        private int expFromBattle = 0;

        private static void ClearRunData()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        IEnumerator Start()
        {
            ClearRunData();

            DontDestroyOnLoad(this.gameObject);
            AddInitialCards();

            yield return LoadMapSceneCoroutine();
        }

        private void AddInitialCards()
        {
            _deck = new List<RunCard>();
            var heroCardDb = heroesDb.cards.FirstOrDefault(x => x.cardName.Contains("Kael Fireforge"));
            _heroCard = new HeroRunCard(heroCardDb);
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

        private (List<RunCard>cards, int size) GetBattleEnemies(int difficulty)
        {
            if (difficulty == 0)
            {
                var slime = enemiesDb.cards.FirstOrDefault(x => x.cardName.Contains("Slime"));
                var kobold = enemiesDb.cards.FirstOrDefault(x => x.cardName.Contains("Kobold Archer"));
                var bigSpider = enemiesDb.cards.FirstOrDefault(x => x.cardName.Equals("Big Spider"));
                var zombie = enemiesDb.cards.FirstOrDefault(x => x.cardName.Contains("Zombie"));
                var spider = enemiesDb.cards.FirstOrDefault(x => x.cardName.Contains("Spider"));
                return minorBattlesAmount switch
                {
                    1 => (new List<RunCard>() { new RunCard(slime), new RunCard(slime), new RunCard(slime) }, 3),
                    2 => (new List<RunCard>() { new RunCard(slime), new RunCard(slime), new RunCard(slime), new RunCard(zombie)}, 4),
                    3 => (new List<RunCard>() { new RunCard(zombie), new RunCard(kobold), new RunCard(zombie) }, 3),
                    _ => (new List<RunCard>() { new RunCard(spider), new RunCard(kobold), new RunCard(spider) }, 3)
                };
            }

            if (difficulty == 1)
            {
                var slime = enemiesDb.cards.FirstOrDefault(x => x.cardName.Contains("Slime"));
                var kobold = enemiesDb.cards.FirstOrDefault(x => x.cardName.Contains("Kobold Archer"));
                var zombie = enemiesDb.cards.FirstOrDefault(x => x.cardName.Contains("Zombie"));
                var spider = enemiesDb.cards.FirstOrDefault(x => x.cardName.Equals("Spider"));
                var bigSpider = enemiesDb.cards.FirstOrDefault(x => x.cardName.Equals("Big Spider"));
                return majorBattlesAmount switch
                {
                    1 => (new List<RunCard>() { new RunCard(spider), new RunCard(spider), new RunCard(spider), new RunCard(spider), new RunCard(spider), new RunCard(spider), new RunCard(bigSpider)}, 5) ,
                    _ => (new List<RunCard>() { new RunCard(zombie), new RunCard(zombie), new RunCard(zombie), new RunCard(zombie),
                        new RunCard(zombie), new RunCard(zombie), new RunCard(zombie), new RunCard(zombie), new RunCard(zombie), new RunCard(zombie) }, 5)
                };
            }

            var dinosaur = enemiesDb.cards.FirstOrDefault(x => x.cardName.Contains("Cookie Dinosaur"));
            return bossBattlesAmount switch
            {
                _=> (new List<RunCard>() { new RunCard(dinosaur)},1)  
            };
        }

        private void BattleComplete(object sender, bool playerWin)
        {
            if (!playerWin)
                return;
            StartCoroutine(LoadPostBattleCoroutine());
            expFromBattle = 25;
        }

        private void OnPostBattleCompleted(object sender, EventArgs e)
        {
            StartCoroutine(LoadMapSceneCoroutine());
        }

        private IEnumerator LoadBattleSceneCoroutine(List<RunCard> battleEnemies, int enemyWheelSize)
        {
            if (_mapGo != null)
            {
                _mapGo.SelectedPack -= OnShopPackObtained;
                _mapGo.SelectedRest -= OnSelectedRest;
                _mapGo.SelectedRelic -= OnSelectedRelic;
                _mapGo.MinorEnemySelected -= OnMinorEnemySelected;
                _mapGo.MajorEnemySelected -= OnMajorEnemySelected;
                _mapGo.BossEnemySelected -= OnBossEnemySelected;
            }

            ShuffleDeck();
            SceneManager.LoadScene("Battle");
            yield return new WaitForSeconds(.5f);
            _battleGo = GameObject.Find("Battle").GetComponent<Battle>();
            yield return _battleGo.Initialize(_deck,battleEnemies, 5, enemyWheelSize, _heroCard);

            if (_battleGo != null)
                _battleGo.BattleFinished += BattleComplete;
        }

        private IEnumerator LoadPostBattleCoroutine()
        {            
            RemoveDeadCardsFromDeck();

            if (_battleGo != null)
                _battleGo.BattleFinished -= BattleComplete;

            SceneManager.LoadScene("PostBattles");
            yield return new WaitForSeconds(.5f);
            _postBattle = GameObject.Find("PostBattle").GetComponent<PostBattle>();

            _postBattle.Completed += OnPostBattleCompleted;
            if (expFromBattle > 0)
                _postBattle.Initialize(_heroCard,expFromBattle);
            expFromBattle = 0;
        }

        private IEnumerator LoadMapSceneCoroutine()
        {
            SceneManager.LoadScene("Map");
            yield return new WaitForSeconds(.5f);
            _mapGo = GameObject.Find("Map").GetComponent<Maps.Map>();

            if (_mapGo != null)
            {
                _mapGo.SelectedPack += OnShopPackObtained;
                _mapGo.SelectedRest += OnSelectedRest;
                _mapGo.SelectedRelic += OnSelectedRelic;
                _mapGo.MinorEnemySelected += OnMinorEnemySelected;
                _mapGo.BossEnemySelected += OnBossEnemySelected;
                _mapGo.MajorEnemySelected += OnMajorEnemySelected;
            }
        }

        private void OnShopPackObtained(object sender, List<BaseCardScriptableObject> cards)
        {
            foreach (var card in cards)
            {
                var runCard = new RunCard(card);
                _deck.Add(runCard);
            }
        }

        private void OnSelectedRest(object sender, EventArgs e)
        {
            foreach (var card in _deck)
            {
                card.Hp = Math.Min(card.Hp + Mathf.FloorToInt(card.Hp * 0.2f), card.baseCard.hp);
            }
        }

        private void OnSelectedRelic(object sender, Relic e)
        {
            _relics.Add(e);
            if (e.RelicBase.id == RelicType.HealthySnack)
            {
                foreach (var card in _deck)
                {
                    card.Hp += 5;
                }

                return;
            }

            if (e.RelicBase.id == RelicType.FireChauldron)
            {
                foreach (var card in _deck)
                {
                    var abs = card.Abilities.ToList();
                    abs.Add(Ability.Burn);
                    card.Abilities = abs.ToArray();
                }

                return;
            }

            if (e.RelicBase.id == RelicType.GordoBachicha||e.RelicBase.id == RelicType.PerroSalchicha)
            {
                foreach (var card in _deck)
                {
                    card.Attack += 1;
                }
                return;
            }

        }

        private void OnMinorEnemySelected(object sender, EventArgs e)
        {
            minorBattlesAmount++;
            var data = GetBattleEnemies(0);
            StartCoroutine(LoadBattleSceneCoroutine(data.cards,data.size));
        }

        private void OnMajorEnemySelected(object sender, EventArgs e)
        {
            majorBattlesAmount++;
            var data = GetBattleEnemies(1);
            StartCoroutine(LoadBattleSceneCoroutine(data.cards,data.size));
        }

        private void OnBossEnemySelected(object sender, EventArgs e)
        {
            bossBattlesAmount++;
            var data = GetBattleEnemies(2);
            StartCoroutine(LoadBattleSceneCoroutine(data.cards,data.size));
        }
    }
}