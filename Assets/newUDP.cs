using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

// using System;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class newUDP : MonoBehaviour
{
    public string text;

    public void Main()
    {
        byte[] dataReceived = new byte[1024];
        
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 10001);
        UdpClient newsock = new UdpClient(ipep);

        // Console.WriteLine("Waiting for a client...");

        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);

        //data = newsock.Receive(ref sender);

        // Console.WriteLine("Message received from {0}:", sender.ToString());
        // Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));

        // string welcome = "Welcome to my test server";
        // data = Encoding.ASCII.GetBytes(welcome);
        // newsock.Send(data, data.Length, sender);

        while(true)
        {
            dataReceived = newsock.Receive(ref sender);

            string data = Encoding.ASCII.GetString(dataReceived, 0, dataReceived.Length);
            text=data;
            Debug.Log(text);
            //newsock.Send(data, data.Length, sender);

            //  string thumb = data.Substring(data.IndexOf("Thumb:")).Replace("Thumb:", "");
            //  string index = data.Substring(data.IndexOf("Index:")).Replace("Index:", "");
            //  string middle = data.Substring(data.IndexOf("Middle:")).Replace("Middle:", "");
            //  string ring = data.Substring(data.IndexOf("Ring:")).Replace("Ring:", "");
            //  string pinky = data.Substring(data.IndexOf("Pinky:")).Replace("Pinky:", "");

            //  Console.WriteLine(Int32.Parse(thumb));
            // Console.WriteLine(Int32.Parse(index));
            // Console.WriteLine(Int32.Parse(middle));
            // Console.WriteLine(Int32.Parse(ring));
            // Console.WriteLine(Int32.Parse(pinky));
        }
   }
}
