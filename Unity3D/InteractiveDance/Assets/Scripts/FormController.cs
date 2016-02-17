//SocketClient Version 0.2
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
    public Vector3 offset; //-27.2, 6.2, 30
    public float handOffset;
    public float div = 10.0f;
    public Form[] bodies;
    private int bodycount;

    public GameObject dancePrefab;
    //connection info
    private Thread receiveThread;
    private UdpClient client;
    public int port = 5005;

    //debug info
    private string lastReceivedUDPPacket;
    private List<string> allReceivedUDPPackets;
    private IPEndPoint anyIP;
    private float currentNoUpdateTime = 0;
    public float maxTimeWithoutUpdate = 3;

    void Start()
    {
        Debug.Log(string.Format(@"Sending to 127.0.0.1 : {0}", port));
        client = new UdpClient(port);
        anyIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
        allReceivedUDPPackets = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFormData();


    }

    //void PrintForm(Form f, string mess = "") {
    //    Debug.Log(string.Format(@"{0}\n Center {1} {2}", mess, f.root.transform.position.x, f.root.transform.position.y));
    //}

    //void OnGUI() {
    //    var rectObj = new Rect(40, 10, 200, 400);
    //    var style = new GUIStyle() {
    //        alignment = TextAnchor.UpperLeft
    //    };
    //    GUI.Box(rectObj, string.Format(@"# UDPReceive 127.0.0.1 {0} #Last Packet:  {1}", port, lastReceivedUDPPacket), style);
    //}

    private void UpdateFormData()
    {
        //rewrite to serialize and deserialize packets.
        try
        {
            if (client.Available > 0)
            {
                currentNoUpdateTime = 0;
                var msg = Encoding.UTF8.GetString(client.Receive(ref anyIP));
                lastReceivedUDPPacket = msg;
                allReceivedUDPPackets.Add(msg);
                var words = msg.Split(',');
                switch (words[0])
                {
                    case "SET":
                        bodycount = int.Parse(words[2]);
                        SetNewForms();
                        break;
                    case "DATA":
                        UpdateForms(int.Parse(words[1]), words);
                        break;
                    default:
                        Debug.Log("UNKNOWN UDP MESSAGE");
                        break;
                }
                foreach (var form in bodies)
                {
                    form.UpdatePositions();
                }
            }
            else if (currentNoUpdateTime < maxTimeWithoutUpdate)
            {
                currentNoUpdateTime += Time.deltaTime;
            }
            else
            {
                bodycount = 0;
                for (var i = 0; i < transform.childCount; i++)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }
            }

        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    private void SetNewForms()
    {
        Debug.Log(bodycount);
        for (var i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        bodies = new Form[bodycount];
        for (var i = 0; i < bodies.Length; i++)
        {
            Debug.Log("instantiating");
            bodies[i] = new Form();
            bodies[i].Root = (GameObject) Instantiate(dancePrefab, Vector3.zero, Quaternion.identity);
            bodies[i].RightHand = bodies[i].Root.transform.GetChild(0).gameObject;
            bodies[i].LeftHand = bodies[i].Root.transform.GetChild(1).gameObject;
            bodies[i].Root.transform.parent = transform;
            EffectGenerator.SetCurrentEffect(bodies[i].Root);
        }
    }
    private void UpdateForms(int id, IList<string> msg)
    {
        if (id + 1 > bodycount)
        {
            bodycount = id + 1;
            SetNewForms();
        }
        // 0 = Message Type; 1 = id; 2,3 = xy root; 4,5 = xy leftmost; 6,7 = xy rightmost; 8 = self.radius; 9 = self.velocity
        bodies[id].RootVector = new Vector3(float.Parse(msg[2])/div, 0, (-float.Parse(msg[3]))/(div * 2)) + offset;
        bodies[id].LeftHandVector = new Vector3(float.Parse(msg[4]) / div + handOffset, 0, (-float.Parse(msg[5])) / (div * 2)) + offset - bodies[id].RootVector;
        bodies[id].RightHandVector = new Vector3(float.Parse(msg[6]) / div - handOffset, 0, (-float.Parse(msg[7])) / (div * 2)) + offset - bodies[id].RootVector;
        bodies[id].Radius = float.Parse(msg[8]);
        bodies[id].Velocity = float.Parse(msg[9]);
    }


}
