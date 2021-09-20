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
                GameManager.instance.player.GetComponent<PlayerInput>().SwitchCurrentActionMap("Game");
                gamePaused = false;
            }
            else
            {
                GameManager.instance.player.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
                gamePaused = true;
            }
            
            pauseMenuUI.SetActive(gamePaused);
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
