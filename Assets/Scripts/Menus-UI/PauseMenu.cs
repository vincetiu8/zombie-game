using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;

namespace Menus
{
    public class PauseMenu : MonoBehaviour
    {
        public bool gamePaused = false;

        [SerializeField] private GameObject pauseMenuUI;

        public void PauseAction(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                if(gamePaused)
                {
                    ResumeGame();
                }
                else
                {   
                    GameManager.instance.player.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
                    pauseMenuUI.SetActive(true);
                    gamePaused = true;
                }
            }
        }
        
        //this is a separate function so it can be used for the resume button
        public void ResumeGame()
        {
            GameManager.instance.player.GetComponent<PlayerInput>().SwitchCurrentActionMap("Game");
            pauseMenuUI.SetActive(false);
            gamePaused = false;
        }

        public void QuitFromPauseMenu()
        {
            Application.Quit();
            Debug.Log("Quit game from pause menu");
        }
    }
}
