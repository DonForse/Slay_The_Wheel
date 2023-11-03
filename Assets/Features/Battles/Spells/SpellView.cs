using UnityEngine;
using UnityEngine.UI;

public class SpellView : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] public int actionCost;
    [SerializeField] public int totalCooldown;

    public int currentCooldown = 0;

    public void Activate()
    {
        currentCooldown = totalCooldown;
        button.interactable = false;
    }

    public void ReduceCooldown()
    {
        currentCooldown--;
    }

    public void SetActivateable(bool value)
    {
        button.interactable = value && currentCooldown <= 0;
    }
}