using System;
using UnityEngine;

namespace Features.GameResources.Coins
{
   public class PlayerPrefsCoinsRepository
   {
      public event EventHandler<int> AmountChanged;
      public void Increase(int amount)
      {
         var coins = Get();
         coins += amount;
         AmountChanged?.Invoke(this,coins);
         PlayerPrefs.SetInt(Key(),coins);
      }
      public void Decrease(int amount)
      {
         var coins = Get();
         coins -= amount;
         if (coins <= 0)
            coins = 0;
         
         AmountChanged?.Invoke(this,coins);
         PlayerPrefs.SetInt(Key(),coins);
         
      }

      public bool HasEnough(int amount) => Get() >= amount;

      public int Get() => PlayerPrefs.GetInt(Key(), 200);

      private static string Key() => RunRepository.GetCurrent() + "_COINS";
   }
}
