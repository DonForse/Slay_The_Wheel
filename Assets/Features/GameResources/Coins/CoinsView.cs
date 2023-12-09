using TMPro;
using UnityEngine;

namespace Features.GameResources.Coins
{
   public class CoinsView : MonoBehaviour
   {
      [SerializeField] private TMP_Text amountText;
      private PlayerPrefsCoinsRepository _playerPrefsCoinsRepository;
      private void OnEnable()
      {
         _playerPrefsCoinsRepository ??= new PlayerPrefsCoinsRepository();
         _playerPrefsCoinsRepository.AmountChanged += OnAmountChanged;
         amountText.text = _playerPrefsCoinsRepository.Get().ToString();
      }

      private void OnDisable()
      {
         _playerPrefsCoinsRepository.AmountChanged -= OnAmountChanged;
      }

      private void OnAmountChanged(object sender, int coins)
      {
         amountText.text = coins.ToString();
      }
   }
}
