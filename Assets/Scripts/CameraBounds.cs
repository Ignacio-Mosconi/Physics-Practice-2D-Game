using System.Collections.Generic;
using UnityEngine;

public enum Border
{
    Top, Bottom, Left, Right
}

[RequireComponent(typeof(Camera))]
public class CameraBounds : MonoBehaviour
{
    Camera viewCamera;
    Dictionary<Border, float> boundaries = new Dictionary<Border, float>();

    void Awake()
    {
        viewCamera = GetComponent<Camera>();

        float topBorder = viewCamera.ViewportToWorldPoint(new Vector3(0f, 1f, 0f)).y;
        float bottomBorder = viewCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).y;
        float leftBorder = viewCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x;
        float rightBorder = viewCamera.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x;

        boundaries.Add(Border.Top, topBorder);
        boundaries.Add(Border.Bottom, bottomBorder);
        boundaries.Add(Border.Left, leftBorder);
        boundaries.Add(Border.Right, rightBorder);
    }

    public float GetBorder(Border border)
    {
        return boundaries[border];
    }
}