using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Game : MonoBehaviour
{
    [SerializeField] private Wheel playerWheel;
    [SerializeField] private Wheel enemyWheel;

    [FormerlySerializedAs("_botControlWheel")] [SerializeField]
    private BotControlWheel botControlWheel;

    [SerializeField] private BaseCardsScriptableObject cardsDb;
    [SerializeField] private BaseCardScriptableObject emptyCard;
    private int _actions;
    private int turn = 0;

    // Start is called before the first frame update
    void Start()
    {
        var deck = new List<RunCard>();
        var heroCardDb = cardsDb.cards.FirstOrDefault(x => x.cardName.Contains("Kael Fireforge"));
        var heroCard = new RunCard(heroCardDb);
        deck.Add(heroCard);
        var unitCardDb = cardsDb.cards.FirstOrDefault(x => x.cardName.Contains("Fireborn Recruit"));
        for (int i = 0; i < 5; i++)
        {
            var playerUnit = new RunCard(unitCardDb);
            deck.Add(playerUnit);
        }

        var enemy3 = cardsDb.cards.FirstOrDefault(x => x.cardName.Contains("Slime"));
        // var enemy1 = cardsDb.cards.FirstOrDefault(x => x.cardName.Contains("Zombie"));
        // var enemy2 = cardsDb.cards.FirstOrDefault(x => x.cardName.Contains("Spider"));

        playerWheel.SetSize(5);
        playerWheel.SetCards(deck);

        enemyWheel.SetCards(new List<RunCard>() { new RunCard(enemy3), new RunCard(enemy3), new RunCard(enemy3) });
        enemyWheel.SetSize(3);

        playerWheel.InitializeWheel(true);
        enemyWheel.InitializeWheel(false);

        enemyWheel.LockWheel();
        playerWheel.UnlockWheel();

        playerWheel.Acted += OnPlayerActed;
        enemyWheel.Acted += OnEnemyActed;
    }

    private void OnEnemyActed(object sender, InPlayCard attacker)
    {
        _actions++;
        var defender = playerWheel.GetFrontCard();
        StartCoroutine(Attack(attacker, enemyWheel, defender, playerWheel));
    }

    private void OnPlayerActed(object sender, InPlayCard attacker)
    {
        _actions++;
        var defender = enemyWheel.GetFrontCard();
        StartCoroutine(Attack(attacker, playerWheel, defender, enemyWheel));
    }

    private IEnumerator Attack(InPlayCard attacker,Wheel attackerWheel, InPlayCard defender, Wheel defenderWheel )
    {
        attackerWheel.LockWheel();

        var defenderCard = defender.GetCard();
        var enemyCard = attacker.GetCard();
        defenderCard.Hp -= enemyCard.Attack;
        if (defenderCard.Hp <= 0)
        {
            defender.SetDead();
            if (defenderWheel.AllUnitsDead())
            {
                Debug.Log("LOST");
                yield break;
            }

            yield return StartCoroutine(defenderWheel.PutAliveUnitAtFront());
        }
        if (_actions == 3)
        {
            ChangeTurn();
            yield break;
        }

        attackerWheel.UnlockWheel();
    }

    private void ChangeTurn()
    {
        turn++;
        _actions = 0;
        
        if (turn % 2 == 0)
            StartPlayerTurn();
        else
            PlayBotTurn();
    }

    private void PlayBotTurn()
    {
        playerWheel.LockWheel();
        enemyWheel.UnlockWheel();
        StartCoroutine(BotAction());
    }

    private IEnumerator BotAction()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return StartCoroutine(botControlWheel.TurnTowardsDirection(Random.Range(0, 2) == 1));
            yield return new WaitForSeconds(Random.Range(1f, 1.5f));
        }
    }

    private void StartPlayerTurn()
    {
        enemyWheel.LockWheel();
        playerWheel.UnlockWheel();
    }
}