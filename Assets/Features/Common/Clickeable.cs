using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Clickeable : MonoBehaviour
{
   private Button _button;
   public event EventHandler Clicked;

   private void OnEnable()
   {
      _button = GetComponent<Button>();
      _button.onClick.AddListener(OnClicked);
   }

   private void OnClicked()
   {
      Clicked?.Invoke(this, null);
   }
}
