using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plzDeleteScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool showStuff;
    [SerializeField] private int number;
    [SerializeField] private string text;
    
    void Start()
    {
        /*string WhatIsMyName = "Hello World";
        string Hello = ExampleFunction(nameof(WhatIsMyName));
        Debug.Log(Hello);
        Debug.Log("hi");
        Debug.Log(nameof(WhatIsMyName));*/
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public string ExampleFunction(string variableName) {
        //Construct your log statement using c# 6.0 string interpolation
        return $"Error occurred in {variableName}";
    }
    
}
