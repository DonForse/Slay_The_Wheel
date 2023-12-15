using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.Battles;
using Features.Cards;
using Features.Common;
using Features.GameResources.Relics;
using Features.Maps;
using Features.PostBattles;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Features
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private DeckConfigurationScriptableObject deck;
        [SerializeField] private BaseCardsScriptableObject enemiesDb;
        private List<RunCardScriptableObject> _deck;
        private Battle _battleGo;
        private Map _mapGo;
        private PostBattle _postBattle;
        private HeroRunCardScriptableObject _heroCardScriptableObject;

        private int minorBattlesAmount = 0;
        private int majorBattlesAmount = 0;
        private int bossBattlesAmount = 0;

        private int expFromBattle = 0;
        private PlayerPrefsRelicsRepository _playerPrefsRelicsRepository;

        IEnumerator Start()
        {
            _playerPrefsRelicsRepository = Provider.PlayerPrefsRelicsRepository();
            DontDestroyOnLoad(this.gameObject);
            AddInitialCards();

            yield return LoadMapSceneCoroutine();
        }

        private void AddInitialCards()
        {
            _deck = new List<RunCardScriptableObject>();
            var heroCardDb = deck.hero;
            _heroCardScriptableObject = new HeroRunCardScriptableObject(heroCardDb);
            foreach (var cardConfiguration in deck.cards)
            {
                for (int i = 0; i < cardConfiguration.Amount; i++)
                    _deck.Add(new RunCardScriptableObject(cardConfiguration.card));
            }
        }

        private void ShuffleDeck() => _deck = _deck.OrderBy(d => Random.Range(0, 100f)).ToList();

        private void RemoveDeadCardsFromDeck() => _deck = _deck.Where(x => x.hp > 0).ToList();

        private (List<RunCardScriptableObject>cards, int size) GetBattleEnemies(int difficulty)
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
                    1 => (new List<RunCardScriptableObject>() { new RunCardScriptableObject(slime), new RunCardScriptableObject(slime), new RunCardScriptableObject(slime) }, 3),
                    2 => (new List<RunCardScriptableObject>() { new RunCardScriptableObject(slime), new RunCardScriptableObject(slime), new RunCardScriptableObject(slime), new RunCardScriptableObject(zombie)}, 4),
                    3 => (new List<RunCardScriptableObject>() { new RunCardScriptableObject(zombie), new RunCardScriptableObject(kobold), new RunCardScriptableObject(zombie) }, 3),
                    _ => (new List<RunCardScriptableObject>() { new RunCardScriptableObject(spider), new RunCardScriptableObject(kobold), new RunCardScriptableObject(spider) }, 3)
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
                    1 => (new List<RunCardScriptableObject>() { new RunCardScriptableObject(spider), new RunCardScriptableObject(spider), new RunCardScriptableObject(spider), new RunCardScriptableObject(spider), new RunCardScriptableObject(spider), new RunCardScriptableObject(spider), new RunCardScriptableObject(bigSpider)}, 5) ,
                    _ => (new List<RunCardScriptableObject>() { new RunCardScriptableObject(zombie), new RunCardScriptableObject(zombie), new RunCardScriptableObject(zombie), new RunCardScriptableObject(zombie),
                        new RunCardScriptableObject(zombie), new RunCardScriptableObject(zombie), new RunCardScriptableObject(zombie), new RunCardScriptableObject(zombie), new RunCardScriptableObject(zombie), new RunCardScriptableObject(zombie) }, 5)
                };
            }

            var dinosaur = enemiesDb.cards.FirstOrDefault(x => x.cardName.Contains("Cookie Dinosaur"));
            return bossBattlesAmount switch
            {
                _=> (new List<RunCardScriptableObject>() { new RunCardScriptableObject(dinosaur)},1)  
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

        private IEnumerator LoadBattleSceneCoroutine(List<RunCardScriptableObject> battleEnemies, int enemyWheelSize)
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
            yield return _battleGo.Initialize(_deck,battleEnemies, 5, enemyWheelSize, _heroCardScriptableObject);

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
                _postBattle.Initialize(_heroCardScriptableObject,expFromBattle);
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
                var runCard = new RunCardScriptableObject(card);
                _deck.Add(runCard);
            }
        }

        private void OnSelectedRest(object sender, EventArgs e)
        {
            foreach (var card in _deck)
            {
                card.hp = Math.Min(card.hp + Mathf.FloorToInt(card.hp * 0.2f), card.baseCard.hp);
            }
        }

        private void OnSelectedRelic(object sender, Relic e)
        {
            _playerPrefsRelicsRepository.Add(e.RelicBase.id);
            if (e.RelicBase.id == RelicType.HealthySnack)
            {
                foreach (var card in _deck)
                {
                    card.hp += 5;
                }

                return;
            }

            if (e.RelicBase.id == RelicType.FireChauldron)
            {
                foreach (var card in _deck)
                {
                    var abs = card.onDealDamageAbilities.ToList();
                    var ability = abs.FirstOrDefault(x=>x.Type == AbilityEnum.Burn);
                    abs.Remove(ability);
                    if (ability == null)
                        ability = new Ability(){Type =  AbilityEnum.Burn,AbilityData = new []{new AbilityData()
                        {
                            Amount = 0, Target = TargetEnum.Enemy
                        }}};
                    else
                    {
                        if (ability.AbilityData.All(x => x.Target != TargetEnum.Enemy))
                        {
                            ability.AbilityData.AddRange(new []{new AbilityData()
                            {
                                Amount = 0, Target = TargetEnum.Enemy
                            }});
                        }
                    }

                    var abilityBurnSingle = ability.AbilityData.First(x => x.Target == TargetEnum.Enemy);
                    abilityBurnSingle.Amount++;
                    abs.Add(ability);
                    card.onDealDamageAbilities = abs.ToList();
                }

                return;
            }

            if (e.RelicBase.id == RelicType.GordoBachicha||e.RelicBase.id == RelicType.PerroSalchicha)
            {
                foreach (var card in _deck)
                {
                    card.attack += 1;
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