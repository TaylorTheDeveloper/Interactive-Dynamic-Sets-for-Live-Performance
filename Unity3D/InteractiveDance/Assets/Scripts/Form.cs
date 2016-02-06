using UnityEngine;
using System.Collections;

public class Form {
    public int id;
    public GameObject root, handRight, handLeft;
    public Vector3 body, leftHand, rightHand;
    public float radius, velocity;
    public void SetGameObjects(GameObject rt, GameObject right, GameObject left)
    {
        root = rt;
        handRight = right;
        handLeft = left;
    }
    public void SetRoot(float x, float y, float z)
    {
        body = new Vector3(x, y, z);
    }
    public void SetPositions(float xlh, float ylh, float xrh, float yrh, float rad, float vel)
    {
        leftHand = new Vector2(xlh, ylh);
        rightHand = new Vector2(xrh, yrh);
        radius = rad;
        velocity = vel;
    }
}
