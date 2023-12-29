using System.Collections;

public interface IGameAction
{
    IEnumerator Start();
    IEnumerator End();
    IEnumerator ApplyRelics();
    IEnumerator ApplyAbilities();
    IEnumerator ApplyEffects();
}