using System.Collections;
using Features.Cards;

public class BattleRelicsManager
{
    // public BattleRelicsManager()
    // {
    //     
    // }

    private static BattleRelicsManager _instance;
    public static BattleRelicsManager Instance => _instance ??= new BattleRelicsManager();

    public IEnumerator ApplyRelicEffect(BattleEventEnum startBattle)
    {
        yield break;
    }
}