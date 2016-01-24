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

public class simpleSocket002 : MonoBehaviour {
	
	// Use this for initialization
	
	//private const int listenPort = 5005;
	public GameObject d0_center;
	public GameObject d1_center;
	public GameObject d2_center;
	public GameObject d3_center;
	public GameObject handl;
	public GameObject handr;
	
	public struct form {
		public int id;
		public GameObject root;
		public GameObject handRight;
		public GameObject handLeft;
		public float xPosRoot;
		public float yPosRoot;
		public float xPosLeftHand;
		public float yPosLeftHand;
		public float xPosRightHand;
		public float yPosRightHand;
		public float radius;
		public float velocity;
	} 
	
	
	form[] bodies;
	private int[] initial; // number of forms to track// maximum limb form
	
	Thread receiveThread;
	UdpClient client;
	public int port;
	public float yoffset = 0;
	public float xoffset = 0;
	public float zoffset = 0;
	public float div = 10.0f;
	public int bodycount = 1;
	//info
	
	public string lastReceivedUDPPacket = "";
	public string allReceivedUDPPackets = "";
	
	
	//Particle System Control
	public GameObject PS;
	public int maxQueueLength = 10;
	private Queue<float> overalvalues;//Average of this determines particle system speed (Overal for group)
	public float modifier;
	
	void Start () {
		init();
		
		bodies = new form[bodycount];
		Debug.Log("Staring Up");
		for(int i = 0; i < bodies.Length; i++){
			if (i == 0){
				bodies[i].root = d0_center;
				bodies[i].handRight = handr;
				bodies[i].handLeft = handl;
			}
			if (i == 1){
				bodies[i].root = d1_center;
			}	
			if (i == 2){
				bodies[i].root = d2_center;
			}
			if (i == 3){
				bodies[i].root = d3_center;
			}						
		}

		
		
		overalvalues = new Queue<float> ();
		PS = GameObject.Find ("FallingSandWall2");
		
	}
	
	void printForm(form f,string mess=""){
		Debug.Log (mess + "\n" +
		           "Center " + f.root.transform.position.x + " " + f.root.transform.position.y 
		           );		
	}
	
	void OnGUI(){
		Rect  rectObj=new Rect (40,10,200,400);
		
		GUIStyle  style  = new GUIStyle ();
		
		style .alignment  = TextAnchor.UpperLeft;
		
		GUI .Box (rectObj,"# UDPReceive\n127.0.0.1 "+port +" #\n"
		          
		          //+ "shell> nc -u 127.0.0.1 : "+port +" \n"
		          
		          + "\nLast Packet: \n"+ lastReceivedUDPPacket
		          
		          //+ "\n\nAll Messages: \n"+allReceivedUDPPackets
		          
		          ,style );
		
	}
	
	private void init(){
		print ("UPDSend.init()");
		port = 5005;//All ports start here				
		print ("Sending to 127.0.0.1 : " + port);
		receiveThread = new Thread (new ThreadStart(ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start ();
	}
	
	private void ReceiveData(){
		client = new UdpClient (port);
		while (true) {
			try{
				IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
				byte[] data = client.Receive(ref anyIP);				
				string text = Encoding.UTF8.GetString(data);
				lastReceivedUDPPacket=text;
				allReceivedUDPPackets=allReceivedUDPPackets+text;
				
				string[] words = text.Split(',');
				


				if (words[0] == "SET"){
					Debug.Log("Data Recieved "+text);
					// 0 = Message Type, 1=name,2=value
					if (words[1] == "COUNT"){
						bodycount = int.Parse(words[2]);
						//must re-initialize on unity end
						bodies = new form[bodycount];		
						for(int i = 0; i < bodies.Length; i++){
							if (i == 0){
								bodies[i].root = d0_center;
								bodies[i].handRight = handr;
								bodies[i].handLeft = handl;
							}
							if (i == 1){
								bodies[i].root = d1_center;
							}	
							if (i == 2){
								bodies[i].root = d2_center;
							}
							if (i == 3){
								bodies[i].root = d3_center;
							}						
						}

					}

				}
				
				if (words[0] == "DATA"){
					// 0 = Message Type; 1 = id; 2,3 = xy root; 4,5 = xy leftmost; 6,7 = xy rightmost; 8 = self.radius; 9 = self.velocity
					int id = int.Parse(words[1]);
					bodies[id].xPosRoot = (float.Parse(words[2]))/div;
					bodies[id].yPosRoot = (-float.Parse(words[3]) + zoffset)/(div*2); //set as z later
					
					bodies[id].xPosLeftHand = (float.Parse(words[4]))/div;
					bodies[id].yPosLeftHand = (-float.Parse(words[5]) + yoffset)/div;
					
					bodies[id].xPosRightHand = (float.Parse(words[6]))/div;
					bodies[id].yPosRightHand = (-float.Parse(words[7]) + yoffset)/div;

					bodies[id].radius = (float.Parse(words[8]));
					bodies[id].velocity = (float.Parse(words[9]));
					float val = (float.Parse(words[9]))/div;
					overalvalues.Enqueue(val);
				}
				
			}catch(Exception e){
				//print (e.ToString());
			}
		}
	}
	
	public string getLatestUDPPacket(){
		allReceivedUDPPackets = "";
		return lastReceivedUDPPacket;
	}
	
	// Update is called once per frame
	void Update () {
		if (bodies.Length > 0){
			for(int i = 0 ; i < bodies.Length; i++){
				bodies[i].root.transform.position = new Vector3(bodies[i].xPosRoot+xoffset,yoffset,bodies[i].yPosRoot+zoffset); //Change later
			}
		}
		
		//Particle Systems Code
		int p = 0;
		foreach (float v in overalvalues) {
			p+=1;
		}
		
		if (p > maxQueueLength) {
			while(p>maxQueueLength){
				overalvalues.Dequeue();
				p -= 1;
			}
		}
		
		float sum = 0.0f;
		
		foreach (float v in overalvalues) {
			sum+=v;
		}
		
		sum = sum / maxQueueLength;
		Debug.Log ("Sum" + sum*modifier);
		PS.GetComponent<ParticleSystem> ().startSpeed = sum;
		//PS.transform.scale.x = sum;
		//End Particle Systems Code
	}
	
	void OnApplicationQuit(){
		if (receiveThread != null) {
			receiveThread.Abort();
			Debug.Log(receiveThread.IsAlive); //must be false
		}
	}
}
