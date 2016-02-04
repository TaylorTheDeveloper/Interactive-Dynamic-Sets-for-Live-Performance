﻿//SocketClient Version 0.2
//Takes data via UDP and displays it acordingly.

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class FormController : MonoBehaviour {

    //object info
    public GameObject d0_center, d1_center, d2_center, d3_center,
        handl, handr;
    public float yoffset, xoffset, zoffset, div = 10.0f;
    private Form[] bodies;
    private int bodycount = 1;

    //connection info
    private Thread receiveThread;
    private UdpClient client;
    public int port = 5005;

    //debug info
    private string lastReceivedUDPPacket;
    private List<string> allReceivedUDPPackets;


    // Update is called once per frame
    void Update()
    {
        UpdateFormData();
        if (bodies.Length > 0)
        {
            foreach (var form in bodies)
            {
                form.root.transform.position = form.body; //Change later
            }
        }
    }


    void Start() {
        Debug.Log(string.Format(@"Sending to 127.0.0.1 : {0}", port));
        client = new UdpClient(port);
        bodies = new Form[bodycount];
        allReceivedUDPPackets = new List<string>();
        for (var i = 0; i < bodies.Length; i++)
        {
            if (i == 0)
            {
                bodies[i].SetGameObjects(d0_center, handr, handl);
            }
            if (i == 1)
            {
                bodies[i].root = d1_center;
            }
            if (i == 2)
            {
                bodies[i].root = d2_center;
            }
            if (i == 3)
            {
                bodies[i].root = d3_center;
            }
        }

    }

    void PrintForm(Form f, string mess = "") {
        Debug.Log(string.Format(@"{0}\n Center {1} {2}", mess, f.root.transform.position.x, f.root.transform.position.y));
    }

    void OnGUI() {
        var rectObj = new Rect(40, 10, 200, 400);
        var style = new GUIStyle() {
            alignment = TextAnchor.UpperLeft
        };
        GUI.Box(rectObj, string.Format(@"# UDPReceive\n127.0.0.1 {0} #\nLast Packet: \n {1}\n", port, lastReceivedUDPPacket), style);
    }

    private void UpdateFormData()
    {
        //rewrite to serialize and deserialize packets.
        try
        {
            var anyIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            var text = Encoding.UTF8.GetString(client.Receive(ref anyIP));
            lastReceivedUDPPacket = text;
            allReceivedUDPPackets.Add(text);
            var words = text.Split(',');
            if (words[0] == "SET")
            {
                Debug.Log(string.Format(@"Data Received {0}", text));
                // 0 = Message Type, 1=name, 2=value
                if (words[1] == "COUNT")
                {
                    SetNewForms(words);
                }
            }
            else if (words[0] == "DATA")
            {
                UpdateForms(int.Parse(words[1]), words);
            }

        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    private void SetNewForms(string[] msg)
    {
        //must re-initialize on unity end
        bodycount = int.Parse(msg[2]);
        if (bodycount != bodies.Length)
        {        
            bodies = new Form[bodycount];
            for (int i = 0; i < bodies.Length; i++)
            {
                if (i == 0)
                {
                    bodies[i].SetGameObjects(d0_center, handr, handl);
                }
                if (i == 1)
                {
                    bodies[i].root = d1_center;
                }
                if (i == 2)
                {
                    bodies[i].root = d2_center;
                }
                if (i == 3)
                {
                    bodies[i].root = d3_center;
                }
            }
        }
    }
    private void UpdateForms(int id, string[] msg)
    {
        // 0 = Message Type; 1 = id; 2,3 = xy root; 4,5 = xy leftmost; 6,7 = xy rightmost; 8 = self.radius; 9 = self.velocity
        bodies[id].SetPositions(
            float.Parse(msg[2]) / div,                      //xPosRoot
            (-float.Parse(msg[3]) + zoffset) / (div * 2),   //yPosRoot
            float.Parse(msg[4]) / div,                      //xPosLeftHand
            (-float.Parse(msg[5]) + yoffset) / div,         //yPosLeftHand
            float.Parse(msg[6]) / div,                      //xPosRightHand
            (-float.Parse(msg[7]) + yoffset) / div,         //yPosRightHand
            float.Parse(msg[8]),                            //radius
            float.Parse(msg[9])                             //velocity
            );
    }


}
