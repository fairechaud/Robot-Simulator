// Copyright 2021 Juan Carlos Orozco Arena

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Text1 : MonoBehaviour
{
    Text myText;

    // Start is called before the first frame update
    void Start()
    {
        myText=GameObject.Find("Text").GetComponent<Text>(); 
                  // here the variable myText reference to the game Object MainText
        myText.text="Camera";        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1")) {
            myText.text="Camera";
        } else if (Input.GetKeyDown("2")) {
            myText.text="Left Arm";
        } else if (Input.GetKeyDown("3")) {
            myText.text="Left Hand";
        } else if (Input.GetKeyDown("4")) {
            myText.text="Right Arm";
        } else if (Input.GetKeyDown("5")) {
            myText.text="Right Hand";
        }
    }
}
