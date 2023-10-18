// Copyright 2021 Juan Carlos Orozco Arena

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camera1 : MonoBehaviour
{
    Text Text1;    

    void Start()
    {
        Text1=GameObject.Find("Text").GetComponent<Text>();
    }

    void Update()
    {   
        var speed = 2.0F;
        if(Text1.text=="Camera") {
            if (Input.GetKey("s")) {
                transform.position = transform.position + new Vector3(0.0F,0.0F,-speed * Time.deltaTime);
                //print(transform.position);
            }
            if (Input.GetKey("w")) {
                transform.position = transform.position + new Vector3(0.0F,0.0F,speed * Time.deltaTime);
                //print(transform.position);
            }
            if (Input.GetKey("a")) {
                transform.position = transform.position + new Vector3(-speed * Time.deltaTime,0.0F,0.0F);
                //print(transform.position);
            }
            if (Input.GetKey("d")) {
                transform.position = transform.position + new Vector3(speed * Time.deltaTime,0.0F,0.0F);
                //print(transform.position);
            }
            if (Input.GetKey(KeyCode.UpArrow)) {
                transform.position = transform.position + new Vector3(0.0F,speed * Time.deltaTime,0.0F);
                //print(transform.position);
            }
            if (Input.GetKey(KeyCode.DownArrow)) {
                transform.position = transform.position + new Vector3(0.0F,-speed * Time.deltaTime,0.0F);
                //print(transform.position);
            }
            if (Input.GetKey(KeyCode.LeftArrow)) {
                //transform.RotateAround(target.transform.position, Vector3.up, 20 * Time.deltaTime);
                transform.RotateAround(Vector3.zero, Vector3.up, speed*36.0F* Time.deltaTime);
                //print(transform.rotation);
            }
            if (Input.GetKey(KeyCode.RightArrow)) {
                //transform.RotateAround(target.transform.position, Vector3.up, 20 * Time.deltaTime);
                transform.RotateAround(Vector3.zero, Vector3.up, -speed*36.0F * Time.deltaTime);
                //print(transform.rotation);
            }
        }
    }
}
