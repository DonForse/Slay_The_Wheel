using System;
using System.Collections;
using Febucci.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.Battles
{
    public class TurnMessage : MonoBehaviour
    {
        [SerializeField] private Transform playerTextContainer;
        [SerializeField] private Transform enemyTextContainer;
        [SerializeField] private TypewriterByCharacter playerTypewriterByCharacter;
        [SerializeField] private TypewriterByCharacter enemyTypewriterByCharacter;
        private TMP_Text playerTmpText;
        private TMP_Text enemyTmpText;
        private bool _endDisplay;

        private void OnEnable()
        {
            playerTypewriterByCharacter.onTextShowed.AddListener(TextCompleted);
            enemyTypewriterByCharacter.onTextShowed.AddListener(TextCompleted);

            playerTmpText = playerTypewriterByCharacter.gameObject.GetComponent<TMP_Text>();
            enemyTmpText = enemyTypewriterByCharacter.gameObject.GetComponent<TMP_Text>();
        }
        private void OnDisable()
        {
            playerTypewriterByCharacter.onTextShowed.RemoveListener(TextCompleted);
            enemyTypewriterByCharacter.onTextShowed.RemoveListener(TextCompleted);
        }

        public IEnumerator Show(bool isPlayer)
        {
            _endDisplay = false;
            if (isPlayer)
            {
                playerTextContainer.gameObject.SetActive(true);
                playerTypewriterByCharacter.ShowText(playerTmpText.text);
                yield return new WaitUntil(()=>_endDisplay);
                yield return new WaitForSeconds(1.5f);
                playerTextContainer.gameObject.SetActive(false);


            }
            else
            {
                enemyTextContainer.gameObject.SetActive(true);
                enemyTypewriterByCharacter.ShowText(enemyTmpText.text);
                yield return new WaitUntil(()=>_endDisplay);
                yield return new WaitForSeconds(1.5f);
                enemyTextContainer.gameObject.SetActive(false);
            }
        }

        private void TextCompleted() => _endDisplay = true;
    }
    
}