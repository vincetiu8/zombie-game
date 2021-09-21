using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Weapons;

namespace Menus_UI
{
    public class PauseMenu : MonoBehaviour
    {
        public bool gamePaused = false;

        [SerializeField] private GameObject pauseMenuUI;
        private PlayerInput playerInput;

        public void PauseMenuToggle()
        {
            if(playerInput == null)
            {
                playerInput = GameManager.instance.player.GetComponent<PlayerInput>();
            }

            gamePaused = !gamePaused;
            string actionMap = gamePaused ? "UI" : "Game";
            playerInput.SwitchCurrentActionMap(actionMap);
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
