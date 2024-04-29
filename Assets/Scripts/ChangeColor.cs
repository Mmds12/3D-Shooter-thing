using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ChangeColor : MonoBehaviour
{
    public Material material;

    public float hue = 8f;
    public float saturation = 60f;
    public float value = 100f;

    void Start()
    {
        Color newColor = Color.HSVToRGB(hue / 360f, saturation / 100f, value / 100f);

        material.color = newColor;
    }
}
