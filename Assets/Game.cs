using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Game : MonoBehaviour
{
    [FormerlySerializedAs("playerWheel")] [SerializeField] private WheelController playerWheelController;
    [FormerlySerializedAs("enemyWheel")] [SerializeField] private WheelController enemyWheelController;

    [FormerlySerializedAs("_botControlWheel")] [SerializeField]
    private BotControlWheel botControlWheel;

    [SerializeField] private BaseCardsScriptableObject cardsDb;
    [SerializeField] private BaseCardScriptableObject emptyCard;
    private int _actions;
    private int turn = 0;
    private bool _acting;

    // Start is called before the first frame update
    void Start()
    {
        var deck = new List<RunCard>();
        var heroCardDb = cardsDb.cards.FirstOrDefault(x => x.cardName.Contains("Kael Fireforge"));
        var heroCard = new RunCard(heroCardDb);
        deck.Add(heroCard);
        var unitCardDb = cardsDb.cards.FirstOrDefault(x => x.cardName.Contains("Fireborn Recruit"));
        var unitCardDb2 = cardsDb.cards.FirstOrDefault(x => x.cardName.Contains("Fireborn Warrior"));

        for (int i = 0; i < 3; i++)
        {
            var playerUnit = new RunCard(unitCardDb);
            deck.Add(playerUnit);
        }
        for (int i = 0; i < 2; i++)
        {
            var playerUnit = new RunCard(unitCardDb2);
            deck.Add(playerUnit);
        }

        var enemy3 = cardsDb.cards.FirstOrDefault(x => x.cardName.Contains("Slime"));
        var enemy1 = cardsDb.cards.FirstOrDefault(x => x.cardName.Contains("Zombie"));
        var enemy2 = cardsDb.cards.FirstOrDefault(x => x.cardName.Contains("Spider"));

        playerWheelController.SetSize(5);
        playerWheelController.SetCards(deck);

        enemyWheelController.SetCards(new List<RunCard>() { new RunCard(enemy1), new RunCard(enemy2), new RunCard(enemy3) });
        enemyWheelController.SetSize(3);

        playerWheelController.InitializeWheel(true);
        enemyWheelController.InitializeWheel(false);

        enemyWheelController.LockWheel();
        playerWheelController.UnlockWheel();

        playerWheelController.Acted += OnPlayerActed;
        enemyWheelController.Acted += OnEnemyActed;
        playerWheelController.WheelTurn += OnPlayerWheelMoved;
        enemyWheelController.WheelTurn += OnEnemyWheelMoved;
    }

    private void OnEnemyWheelMoved(object sender, InPlayCard e)
    {
        StartCoroutine(SpinWheel(enemyWheelController));
    }

    private void OnPlayerWheelMoved(object sender, InPlayCard e)
    {
        StartCoroutine(SpinWheel(playerWheelController));
    }

    private void OnEnemyActed(object sender, InPlayCard attacker)
    {
        _actions++;
        StartCoroutine(Act(attacker, enemyWheelController, playerWheelController));
    }

    private void OnPlayerActed(object sender, InPlayCard attacker)
    {
        _actions++;
        StartCoroutine(Act(attacker, playerWheelController, enemyWheelController));
    }

    private IEnumerator Act(InPlayCard attacker,WheelController attackerWheelController, WheelController defenderWheelController )
    {
        _acting = true;
        attackerWheelController.LockWheel();
        var attackerCard = attacker.GetCard();

        yield return ApplyWheelMovementEffect(attackerWheelController);
        
        if (attacker.IsDead) //own unit dead on movement
        {
            if (_actions == 3)
            {
                ChangeTurn();
                _acting = false;
                yield break;
            }
            _acting = false;
            yield break;
        }

        ApplyFrontCardEffect(attackerCard, defenderWheelController);
        yield return ApplyFrontCardAttack(attackerCard, defenderWheelController);

        yield return ApplyAfterHitEffect(attackerCard, defenderWheelController);
        if (defenderWheelController.AllUnitsDead())
        {
            Debug.Log("LOST");
            _acting = false;
            yield break;
        }
        if (_actions == 3)
        {
            ChangeTurn();
            _acting = false;
            yield break;
        }

        attackerWheelController.UnlockWheel();
        _acting = false;
    }

    private IEnumerator SpinWheel(WheelController wheelController)
    {
        yield return ApplyWheelMovementEffect(wheelController);
    }

    private IEnumerator ApplyAfterHitEffect(RunCard attackerCard, WheelController defenderWheelController)
    {
        foreach (var ability in attackerCard.Abilities)
        {
            if (ability == Ability.RotateRight)    
                yield return defenderWheelController.RotateRight();
            if (ability == Ability.RotateLeft)
                yield return defenderWheelController.RotateLeft();
        }
        // yield return defenderWheelController.PutAliveUnitAtFront(true);
    }

    private IEnumerator ApplyFrontCardAttack(RunCard attackerCard, WheelController defenderWheelController)
    {
        if (attackerCard.AttackType == AttackType.Front)
        {
            var defender = defenderWheelController.GetFrontCard();
            yield return ApplyDamage(attackerCard.Attack, defender, defenderWheelController);
        }
        else if (attackerCard.AttackType == AttackType.All)
        {
            foreach (var defender in defenderWheelController.Cards)
                yield return ApplyDamage(attackerCard.Attack, defender, defenderWheelController);
        }
        else if (attackerCard.AttackType == AttackType.FrontAndSides)
        {
            var defenders = defenderWheelController.GetFrontNeighborsCards(0,2).ToList();
            foreach (var defender in defenders)
                yield return ApplyDamage(attackerCard.Attack, defender, defenderWheelController);
        }
    }

    private IEnumerator ApplyDamage(int damage, InPlayCard defender, WheelController defenderWheelController, Ability? source = null)
    {
        var defenderCard = defender.GetCard();
        if (defender.IsDead) yield break;
        
        defender.PlayGetHitAnimation(damage, source);
        defenderCard.Hp -= damage;
        if (defenderCard.Hp > 0) yield break;
        
        defender.SetDead();
        yield return defenderWheelController.PutAliveUnitAtFront(true);
    }

    private static void ApplyFrontCardEffect(RunCard attackerCard, WheelController defenderWheelController)
    {
        foreach (var ability in attackerCard.Abilities)
        {
            if (ability == Ability.Burn)
            {
                defenderWheelController.GetFrontCard().GetCard().Effects.Add(Ability.Burn);
            }
        }
    }

    private IEnumerator ApplyWheelMovementEffect(WheelController wheelController)
    {
        foreach (var card in wheelController.Cards)
        {
            var cardActiveEffects = card.GetCard().Effects;
            var burns = cardActiveEffects.Count(a => a == Ability.Burn);
            if (burns > 0)
            {
                yield return ApplyDamage(burns, card, wheelController, Ability.Burn);
                card.GetCard().Effects.Remove(Ability.Burn);
            }
        }
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
        playerWheelController.LockWheel();
        enemyWheelController.UnlockWheel();
        StartCoroutine(BotAction());
    }

    private IEnumerator BotAction()
    {
        for (int i = 0; i < 3; i++)
        {
            while (_actions < i || _acting)
                yield return new WaitForSeconds(0.1f);
            yield return botControlWheel.TurnTowardsDirection(Random.Range(0, 2) == 1);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void StartPlayerTurn()
    {
        enemyWheelController.LockWheel();
        playerWheelController.UnlockWheel();
    }
}