using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plzDeleteScript : MonoBehaviour
{
    // Variables to always show
    [SerializeField] private string alwaysShow;
    
    // Variables tied to bool set 1
    [SerializeField] private bool showStuff;
    [Tooltip("yee")] [Range(1,5)]
    [SerializeField] private int number;
    [SerializeField] private string text;
    
    // Variables tied to bool set 2
    [SerializeField] private bool showMoreStuff;
    [SerializeField] private GameObject gameObject;
    [SerializeField] private MonoBehaviour monoBehaviour;
    [SerializeField] private Transform transform;
    [SerializeField] private Vector2 vector2;
    
    void Start()
    {
       Debug.Log("Text that always shows: " + alwaysShow);
       
       Debug.Log(showStuff ? "showStuff toggled : " + number + text : "ShowStuff not toggled");
       
       Debug.Log(showMoreStuff ? "showMoreStuff toggled: " + gameObject + monoBehaviour + transform + vector2 : "showMoreStuff not toggled");
    }

}
