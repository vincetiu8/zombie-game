using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

namespace Menus_UI
{
    public class PauseMenu : MonoBehaviour
    {
        public bool gamePaused = false;

        [SerializeField] private GameObject pauseMenuUI;

        public void PauseMenuToggle()
        {
            if(gamePaused)
            {
                pauseMenuUI.SetActive(false);
                GameManager.instance.player.GetComponent<PlayerInput>().SwitchCurrentActionMap("Game");
                gamePaused = false;
            }
            
            pauseMenuUI.SetActive(true);
            GameManager.instance.player.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
            gamePaused = true;
        }

        private void PauseAction(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                PauseMenuToggle();
            }
        }

        public void QuitFromPauseMenu()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void QuitGameFromPauseMenu()
        {
            Application.Quit();
            Debug.Log("Quit game from pause menu");
        }
    }
}
