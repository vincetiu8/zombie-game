using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitching : MonoBehaviour {

    public int selectedWeapon = 0;
    public float mouseScrollY;

    private Input input;

    private void Awake() {
        input = new Input();

        input.Game.WeaponSwitching.performed += x => mouseScrollY = x.ReadValue<float>();
    }

    private void OnEnable() {
        input.Enable();    
    }

    private void OnDisable() {
        input.Disable();
    }

    private void Start() {
        SelectWeapon();
    }

    private void Update() {
        Keyboard kb = InputSystem.GetDevice<Keyboard>();
        Debug.Log(mouseScrollY);
        int previousSelectedWeapon = selectedWeapon;

        if (mouseScrollY > 0f) {
            Debug.Log("Scrolled Up");
            if (selectedWeapon >= transform.childCount - 1) {
                selectedWeapon = 0;
            }
            else {
                selectedWeapon++;
            }
        }

        if (mouseScrollY < 0f) {
            Debug.Log("Scrolled Down");
            if (selectedWeapon <= 0) {
                selectedWeapon = transform.childCount - 1;
            }
            else {
                selectedWeapon--;
            }
        }

        if (kb.digit1Key.wasPressedThisFrame) {
            selectedWeapon = 0;
        }

        if (kb.digit2Key.wasPressedThisFrame && transform.childCount >= 2) {
            selectedWeapon = 1;
        }

        if (kb.digit3Key.wasPressedThisFrame && transform.childCount >= 3) {
            selectedWeapon = 2;
        }

        if (previousSelectedWeapon != selectedWeapon) {
            SelectWeapon();
        }
    }

    private void SelectWeapon() {
        int i = 0;

        foreach (Transform weapon in transform) {
            if (i == selectedWeapon) {
                weapon.gameObject.SetActive(true);
            }
            else {
                weapon.gameObject.SetActive(false);
            }

            i++;
        }
    }
}
