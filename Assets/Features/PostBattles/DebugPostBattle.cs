using System.Linq;
using Features.Cards;
using Features.Cards.Heroes;
using UnityEngine;

namespace Features.PostBattles
{
    public class DebugPostBattle : MonoBehaviour
    {
        [SerializeField] private PostBattle postBattle;
        [SerializeField] private BaseCardScriptableObject heroRunCardScriptableObject;
        [SerializeField] private LevelUpsScriptableObject levelUpsScriptableObject;
        [SerializeField]private bool levelUp;
        [SerializeField]private int exp;
        // Start is called before the first frame update
        void OnEnable()
        {
            postBattle.Initialize(new HeroRunCardScriptableObject(heroRunCardScriptableObject),
                levelUp ? levelUpsScriptableObject.LevelUpInformations.First().ExpToLevel : exp);
        }
    }
}
