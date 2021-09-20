using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

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
                    pauseMenuUI.SetActive(true);
                    Time.timeScale = 0f;
                    gamePaused = true;
                }
            }
        }
        
        //this is a separate function so it can be used for the resume button
        public void ResumeGame()
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            gamePaused = false;
        }

        public void QuitFromPauseMenu()
        {
            Application.Quit();
            Debug.Log("Quit game from pause menu");
        }
    }
}
