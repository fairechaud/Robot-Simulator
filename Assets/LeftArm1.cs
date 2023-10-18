// Copyright 2021 Juan Carlos Orozco Arena
// Uses SharpOSC library: https://github.com/ValdemarOrn/SharpOSC

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SharpOSC;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


public class LeftArm1 : MonoBehaviour
{
    // private UDPSender sender;
    public GameObject server; // you will need this if scriptB is in another GameObject
                     // if not, you can omit this
                     // you'll realize in the inspector a field GameObject will appear
                     // assign it just by dragging the game object there
    public UDPReceive2 script; // this will be the container of the script
    
    bool printVar = false;
    public float interpolationPeriod = 0.05f;
    Vector3 hide = new Vector3(0,10,10);

    Text Text1;
    Vector3 wrist1UL;
    Vector3 wristOrientation1L;
    Transform spWristUL;
    Transform spShoulderL;
    Transform spElbowL;
    Transform spWristL;
    Hand hand;
    float fingersAngle = 0.0F;

    Transform J1L;
    Transform J2L;
    Transform J3L;
    Transform J4L;
    Transform J5L;
    Transform J6L;
    Transform J7L;

    float dataIn1;
    float dataIn2;
    float dataIn3;
    float dataIn4;
    float dataIn5;
    float dataIn6;

    float j1c, j2c, j3c, j4c, j5c, j6c, j7c = 0;

    

    string message;
    // string[] values = new string[6] {"-90","25","-25","0","45","90"};
    string[] values = new string[6] {"0","0","0","0","0","0"};

    float ScaleAngle1(float angle){
        float ret = angle;
        if(ret > 180) {
            ret = ret - 360;
        }
        ret = ret*1.0f;
        return ret;
    }

    void MoveLeftArm() {
        var elbowWrist = Lib1.getElbowWrist(wrist1UL);
        var T1_7 = Lib1.getAxisAngles(elbowWrist, hand, wristOrientation1L, true);

        simulateArm(T1_7);
    }

    void OnGUI()
        {
            // GUI.contentColor = Color.white;
            // GUIStyle style = new GUIStyle(GUI, style);
            GUIStyle guiStyle = new GUIStyle();
            // GUI.fontSize = 80;
            // GUIStyle fontSize = new GUIStyle();
            guiStyle.fontSize = 50;
            guiStyle.normal.textColor = Color.white;
            // guiStyle.style = Color.white;
            GUI.Label(new Rect(100, 100, 200,  40), "Left arm:", guiStyle);
            GUI.Label(new Rect(125, 175, 200,  40), "J1: " + dataIn1 +"°", guiStyle);
            GUI.Label(new Rect(125, 250, 200,  40), "J2: " + dataIn2 +"°", guiStyle);
            GUI.Label(new Rect(125, 325, 200,  40), "J3: " + dataIn3 +"°", guiStyle);
            GUI.Label(new Rect(125, 400, 200,  40), "J4: " + dataIn4 +"°", guiStyle);
            GUI.Label(new Rect(125, 475, 200,  40), "J5: " + dataIn5 +"°", guiStyle);
            GUI.Label(new Rect(125, 550, 200,  40), "J6: " + dataIn6 +"°", guiStyle);
            
            // GUI.Label(new Rect(450, 275, 200, 50), "Theta 2: " + j2c, guiStyle);
        }
        

    void Start()
    {
        printVar = true;
        Text1=GameObject.Find("Text").GetComponent<Text>();

        J1L = (GameObject.Find("J1L")).transform;
        J2L = (GameObject.Find("J2L")).transform;
        J3L = (GameObject.Find("J3L")).transform;
        J4L = (GameObject.Find("ElbowL")).transform;
        J5L = (GameObject.Find("Wrist1L")).transform;
        J6L = (GameObject.Find("Wrist2L")).transform;
        J7L = (GameObject.Find("Wrist3L")).transform;
        
  
        script = server.GetComponent<UDPReceive2>();
        // Units in m and radians

        // Unity:
        // x axis between shulders. Positive to the right
        // y axis up-down. Positive up
        // z axis front-back. Positive front

        // This complies with right hand rule. On 3 axis and rotation.
        // Kinematics:
        // x axis front-back. Positive front (sticking out from chest)
        // y axis between shulders. Positive to the left shoulder
        // z axis up-down. Positive up

        ///arm_length_factor = 0.95F*robot_arm_length/arm_length;

        // Notes:
        // SR: Shoulder reference: Uses frame0 orientation (Y Axis parallel to the floor)
        // F0: Frame 0 reference. Chest axis.
        // F1: Frame 1 reference. Center on chest, rotated 25degrees on X0
        // Floor: _F0 displaced on z 1.000-0.0776476532296187
        // 
        // Use _SR to calculate elbow position (elbow out using horizontal plane from shoulder to wrist)
        //    

        // Shoulder to floor sitted down: 1.000
        // Chest axis reference to shoulder: # [x,y,z] -> [0,166.515929712244,77.6476532296187]
        // Floor reference to chest (not rotated) => [0,0,1000-77.6476532296187]
        ///sitting_chest_height = sitting_shoulder_height-0.0776476532296187F;

        ////var wrist1 = new Vector3(0.7000,0.0,0.0)
        //wrist1UL = new Vector3(-0.166515929712244F, 1.0F, 0.7000F);
        //wrist1UL = new Vector3(-0.166515929712244F, 1.0F, 0.6000F);
        wrist1UL = new Vector3(-0.2F, 0.5F, 0.3000F);
        
        ////var wrist1 = new Vector3(2800.0,0.0,0.0)
        //var wrist1L = new Vector3(2.800F, 0.166515929712244F, 1.0F);
        ////var wrist1 = new Vector3(300.0,100.0,0.0)
        //var wrist1L = new Vector3(0.300, 0.100F+0.166515929712244F, 1.0F)
        ////var wrist1 = new Vector3(300.0,0.0,-100.0)
        //var wrist1L = new Vector3(0.300F, 0.166515929712244F, 1.0F-0.100F)

        var elbowWrist = Lib1.getElbowWrist(wrist1UL);
        print(elbowWrist);

        wristOrientation1L = new Vector3(90,0,90); // x,y,z euler angles

        var MU_0L = Lib1.getMU_0L();
        var spSize = 0.05F;
        var spShoulderL = Lib1.NewSphere(Vector3.one*spSize, Lib1.SR2U(Vector3.zero), Color.red);
        spElbowL = Lib1.NewSphere(Vector3.one*spSize, Lib1.SR2U(elbowWrist.Elbow), Color.green);
        //spWristL = Lib1.NewSphere(Vector3.one*spSize, SR2U(elbowWrist.Wrist), Color.blue);
        spWristUL = Lib1.NewSphere(Vector3.one*spSize, wrist1UL, new Color(1.0F, 1.0F, 0.0F, 0.5F));

        var spX = Lib1.NewSphere(Vector3.one*spSize, new Vector3(0.1F,0.0F,0.0F), Color.red);
        var spY = Lib1.NewSphere(Vector3.one*spSize, new Vector3(0.0F,0.1F,0.0F), Color.green);
        var spZ = Lib1.NewSphere(Vector3.one*spSize, new Vector3(0.0F,0.0F,0.1F), Color.blue);

        hand = new Hand(Lib1.SR2U(elbowWrist.Wrist), new Quaternion(), true);

        // TODO JCOA: Update address and port to project server (Jetson Nano on robot).
        // Note JCOA: We can create a sender (client to the same port) in each part.
        //            So we can copy this code to the left arm
        // sender = new SharpOSC.UDPSender("127.0.0.1", 9001);
        // TODO: Uncomment for continuous messages
        InvokeRepeating("MoveLeftArm", 1.0f, interpolationPeriod);
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Remove condition to do continuous tracking.
        // For debuging purposes
        message = script.lastReceivedUDPPacket;
        Debug.Log(message);
        
        if(!String.Equals(script.lastReceivedUDPPacket,string.Empty))
        {
            values = message.Trim('$').Split(',');
            Debug.Log("yes");
        }


        if (Input.GetKeyDown("0")) {
            print(transform.position);
            print(transform.localEulerAngles);
            print(transform.rotation);
            print(transform.rotation.w); //[3]); // Both ways work OK
            //MoveRightArm();
            print("deltaTime "+Time.deltaTime);
            print("wrist1UL "+wrist1UL);
            MoveLeftArm();
        }
        var speed = 1.0F;
        if(Text1.text == "Left Arm") {
            if (Input.GetKey("s")) {
                wrist1UL = wrist1UL + new Vector3(0.0F,0.0F,-speed * Time.deltaTime);
                // Set minimum value for wrist z
                if(wrist1UL.z < 0.2F) { // 0.015F
                   wrist1UL.z = 0.2F; 
                }
            }
            else if (Input.GetKey("w")) {
                wrist1UL = wrist1UL + new Vector3(0.0F,0.0F,speed * Time.deltaTime);
            }
            else if (Input.GetKey("a")) {
                wrist1UL = wrist1UL + new Vector3(-speed * Time.deltaTime,0.0F,0.0F);
            }
            else if (Input.GetKey("d")) {
                wrist1UL = wrist1UL + new Vector3(speed * Time.deltaTime,0.0F,0.0F);
            }
            else if (Input.GetKey(KeyCode.UpArrow)) {
                wrist1UL = wrist1UL + new Vector3(0.0F,speed * Time.deltaTime,0.0F);
            }
            else if (Input.GetKey(KeyCode.DownArrow)) {
                wrist1UL = wrist1UL + new Vector3(0.0F,-speed * Time.deltaTime,0.0F);
            }
        }
        if(Text1.text == "Left Hand") {
            if (Input.GetKey("s")) {
                wristOrientation1L = wristOrientation1L + new Vector3(-100.0F*speed * Time.deltaTime,0,0);
            }
            else if (Input.GetKey("w")) {
                wristOrientation1L = wristOrientation1L + new Vector3(100.0F*speed * Time.deltaTime,0,0);
            }
            else if (Input.GetKey("a")) {
                wristOrientation1L = wristOrientation1L + new Vector3(0,0,-100.0F*speed * Time.deltaTime);
            }
            else if (Input.GetKey("d")) {
                wristOrientation1L = wristOrientation1L + new Vector3(0,0,100.0F*speed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.UpArrow)) {
                wristOrientation1L = wristOrientation1L + new Vector3(0,100.0F*speed * Time.deltaTime,0);
            }
            else if (Input.GetKey(KeyCode.DownArrow)) {
                wristOrientation1L = wristOrientation1L + new Vector3(0,-100.0F*speed * Time.deltaTime,0);
            }
            else if (Input.GetKey(KeyCode.LeftArrow)) {
                fingersAngle -= 100.0F*speed*Time.deltaTime;
                if(fingersAngle<0.0F){
                    fingersAngle = 0.0F;
                }
                hand.setFingersAngle(fingersAngle);
            }
            else if (Input.GetKey(KeyCode.RightArrow)) {
                fingersAngle += 100.0F*speed*Time.deltaTime;
                if(fingersAngle>110.0F){
                    fingersAngle = 110.0F;
                }
                hand.setFingersAngle(fingersAngle);
            }
        }
        var elbowWrist = Lib1.getElbowWrist(wrist1UL);
        spWristUL.localPosition = wrist1UL+hide;
        // spShoulderL.localPosition = spShoulderL.localPosition+hide;
        spElbowL.localPosition = Lib1.SR2U(elbowWrist.Elbow)+hide;
        //print("elbowWrist.Elbow "+ Lib1.SR2U(elbowWrist.Elbow));
        //spWristL.localPosition = Lib1.SR2U(elbowWrist.Wrist);
        //print("elbowWrist.Wrist "+ Lib1.SR2U(elbowWrist.Wrist));
    }

    void simulateArm(float[] angles) {
        // var j1c = angles[0]*180.0F/Mathf.PI;
        // var j2c = angles[1]*180.0F/Mathf.PI+(90.0F+25.0F);
        // var j3c = angles[2]*180.0F/Mathf.PI;
        // var j4c = angles[3]*180.0F/Mathf.PI;
        // var j5c = (angles[4]*180.0F/Mathf.PI) % 360.0F;
        // var j6c = (angles[5]*180.0F/Mathf.PI) % 360.0F;
        // var j7c = (angles[6]*180.0F/Mathf.PI) % 360.0F;
        
        dataIn1 =  float.Parse(values[0]);
        dataIn2 =  float.Parse(values[1]);
        dataIn3 =  float.Parse(values[2]);
        dataIn4 =  float.Parse(values[3]);
        dataIn5 =  float.Parse(values[4]);
        dataIn6 =  float.Parse(values[5]);

        j1c = - float.Parse(values[0]);
        j2c = - float.Parse(values[1]);
        j3c = - float.Parse(values[2]);
        j4c =   float.Parse(values[3]);
        j5c =   float.Parse(values[4]);
        j6c =   -(float.Parse(values[5]));
        j7c = 0;

        // Simulate arm:
        // J1L J1L rot -X
        // J2L J2L rot -Y
        // J3L J3L rot +X
        // J4L Elbow rot +Z
        // J5L Wrist1 rot +X
        // J6L Wrist2 rot +Y
        // J7L Wrist3 rot +Z

        // if(Input.GetKeyDown("j"))
        // {
        //     // float recordedJ1 = j1c;
        //     // float recordedJ2 = j2c;
        //     // Debug.Log("Recorded: "+recordedJ1+"."+recordedJ2);
        //     printVar = !printVar;
        // }
        // Debug.Log(message);
        // if(printVar)
        // {
        Debug.Log("Left angles j1: "+j1c+" j2: "+j2c+" j3: "+j3c+" j4: "+j4c+" j5: "+j5c+" j6: "+j6c);
        // }
        J1L.localEulerAngles = new Vector3(-j1c,0,0);
        J2L.localEulerAngles = new Vector3(0,-j2c,0);
        J3L.localEulerAngles = new Vector3(-j3c,0,0);
        J4L.localEulerAngles = new Vector3(0,0,j4c);
        J5L.localEulerAngles = new Vector3(-j5c,0,0);
        J6L.localEulerAngles = new Vector3(0,-j6c,0);
        J7L.localEulerAngles = new Vector3(0,0,j7c);
        
        // JCO: Enable or disable right arm commands
        //print("Left angles j1, j2, j3, j4, j5, j6, j7: "+j1c+" "+j2c+" "+j3c+" "+j4c+" "+j5c+" "+j6c+" "+j7c);

        //var command = 1; // Set angles
        //var message = new SharpOSC.OscMessage("/ArmL", command, j1c, j2c, j3c, j4c, j5c, j6c, j7c);
        //if(sender != null)
        //{
        //    sender.Send(message);
        //}
    }
}
