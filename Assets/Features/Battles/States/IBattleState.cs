using System.Collections;

namespace Features.Battles.States
{
    public interface IBattleState
    {
        IEnumerator EnterState();
        IEnumerator ExitState();
        IEnumerator UpdateState();
    }
}