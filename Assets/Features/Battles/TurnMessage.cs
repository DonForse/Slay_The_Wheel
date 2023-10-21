using System.Collections;
using UnityEngine;

namespace Features.Battles
{
    public class TurnMessage : MonoBehaviour
    {
        [SerializeField] private Transform playerTextContainer;
        [SerializeField] private Transform enemyTextContainer;
        public IEnumerator Show(bool isPlayer)
        {
            if (isPlayer)
            {
                playerTextContainer.gameObject.SetActive(true);
                yield return new WaitForSeconds(1.5f);
                playerTextContainer.gameObject.SetActive(false);

            }
            else
            {
                enemyTextContainer.gameObject.SetActive(true);
                yield return new WaitForSeconds(1.5f);
                enemyTextContainer.gameObject.SetActive(false);
            }
        }
    }
}