using UnityEngine;
using System.Collections;

public class Form {
    public int id;
    public GameObject root, handRight, handLeft;
    public float xPosRoot, yPosRoot, xPosLeftHand, yPosLeftHand,
        xPosRightHand, yPosRightHand, radius, velocity;

    public void SetGameObjects(GameObject rt, GameObject right, GameObject left)
    {
        root = rt;
        handRight = right;
        handLeft = left;
    }
    public void SetPositions(float xrt, float yrt, float xlh, float ylh, float xrh, float yrh, float rad, float vel)
    {
        xPosRoot = xrt;
        yPosRoot = yrt;
        xPosLeftHand = xlh;
        yPosLeftHand = ylh;
        xPosRightHand = xrh;
        yPosRightHand = yrh;
        radius = rad;
        velocity = vel;
    }
}
