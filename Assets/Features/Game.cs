using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private BaseCardsScriptableObject cardsDb;

    private List<RunCard> _deck;
    private int _currentLevel = 0;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        DontDestroyOnLoad(this.gameObject);

        _deck = new List<RunCard>();
        var heroCardDb = cardsDb.cards.FirstOrDefault(x => x.cardName.Contains("Kael Fireforge"));
        var heroCard = new RunCard(heroCardDb);
        _deck.Add(heroCard);
        var unitCardDb = cardsDb.cards.FirstOrDefault(x => x.cardName.Contains("Fireborn Recruit"));
        var unitCardDb2 = cardsDb.cards.FirstOrDefault(x => x.cardName.Contains("Fireborn Warrior"));

        for (var i = 0; i < 3; i++)
        {
            var playerUnit = new RunCard(unitCardDb);
            _deck.Add(playerUnit);
        }

        for (var i = 0; i < 2; i++)
        {
            var playerUnit = new RunCard(unitCardDb2);
            _deck.Add(playerUnit);
        }

        yield return LoadBattleSceneCoroutine();
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

    private void ShuffleDeck() => _deck = _deck.OrderBy(d => Random.Range(0, 100f)).ToList();

    private void RemoveDeadCardsFromDeck() => _deck = _deck.Where(x => !x.IsDead).ToList();

    private IEnumerator LoadBattleSceneCoroutine()
    {
        ShuffleDeck();

        SceneManager.LoadScene("Battle");
        yield return new WaitForSeconds(.5f);
        var battleGo = GameObject.Find("Battle").GetComponent<Battle>();
        battleGo.Initialize(_deck);
        battleGo.OnComplete += BattleComplete;
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