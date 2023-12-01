using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.GameMenu
{
    public class GameMenu : MonoBehaviour
    {
        [UsedImplicitly]
        public void StartGame()
        {
            RunRepository.CreateNew();
            SceneManager.LoadScene("Start");
        }
        [UsedImplicitly]
        public void LoadGame()
        {
            RunRepository.LoadRun(0);
            SceneManager.LoadScene("Start");
        }
        [UsedImplicitly]
        public void Options()
        {
        }
        [UsedImplicitly]
        public void Exit()
        {
            Application.Quit();
        }
    }
}
