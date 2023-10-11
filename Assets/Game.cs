using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Game : MonoBehaviour
{
    [SerializeField] private Wheel playerWheel;
    [SerializeField] private Wheel enemyWheel;
    [FormerlySerializedAs("_botControlWheel")] [SerializeField] private BotControlWheel botControlWheel;
    [SerializeField] private BaseCardsScriptableObject cardsDb;
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
        var enemyDb = cardsDb.cards.FirstOrDefault(x => x.cardName.Contains("Slime"));

        playerWheel.SetSize(5);
        playerWheel.SetCards(deck);
        
        enemyWheel.SetCards(new List<RunCard>() { new RunCard(enemyDb), new RunCard(enemyDb), new RunCard(enemyDb) });
        enemyWheel.SetSize(3);
        
        playerWheel.InitializeWheel(true);
        enemyWheel.InitializeWheel(false);

        enemyWheel.LockWheel();
        playerWheel.UnlockWheel();
        
        playerWheel.Acted += OnPlayerActed;
        enemyWheel.Acted += OnEnemyActed;

    }

    private void OnPlayerActed(object sender, InPlayCard inPlayPlayerCard)
    {
        playerWheel.LockWheel();
        _actions++;
        var enemyInPlayCard = enemyWheel.GetFrontCard();
        var enemyCard = enemyInPlayCard.GetCard();
        var playerCard = inPlayPlayerCard.GetCard();
        enemyCard.Hp -= playerCard.Attack;
        if (enemyCard.Hp < 0)
            Debug.Log("ENEMY DEADDDD");

        if (turn == 0 && _actions == 2)
        {
            ChangeTurn();
            return;
        }
        else if (_actions == 3)
        {
            ChangeTurn();
            return;
        }
        playerWheel.UnlockWheel();
    }
    
    private void OnEnemyActed(object sender, InPlayCard inPlayEnemyCard)
    {
        enemyWheel.LockWheel();
        _actions++;
        var inPlayPlayerCard = playerWheel.GetFrontCard();
        var playerCard = inPlayPlayerCard.GetCard();
        var enemyCard = inPlayEnemyCard.GetCard();
        playerCard.Hp -= enemyCard.Attack;
        
        if (playerCard.Hp < 0)
            Debug.Log("Player DEADDDD");

        if (_actions == 3)
        {
            ChangeTurn();
            return;
        }
        enemyWheel.UnlockWheel();
    }

    private void ChangeTurn()
    {
        turn++;
        _actions = 0;
        if (turn % 2 == 0)
        {
            StartPlayerTurn();
        }
        else
        {
            PlayBotTurn();
        }
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
