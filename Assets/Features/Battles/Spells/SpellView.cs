using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Battles.Spells
{
    public class SpellView : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text cooldownCount;
        [SerializeField] public int actionCost;
        [SerializeField] public int totalCooldown;

        public int currentCooldown = 0;

        private void OnEnable()
        {
            currentCooldown = 0;
            cooldownCount.text = $"{(currentCooldown<= 0 ? "": currentCooldown)}";
        }

        public void Activate()
        {
            currentCooldown = totalCooldown;
            button.interactable = false;
            cooldownCount.text = $"{(currentCooldown<= 0 ? "": currentCooldown)}";
        }

        public void ReduceCooldown()
        {
            currentCooldown--;
            cooldownCount.text = $"{(currentCooldown<= 0 ? "": currentCooldown)}";
        }

        public void SetActivateable(bool value)
        {
            button.interactable = value && currentCooldown <= 0;
        }
    }
}