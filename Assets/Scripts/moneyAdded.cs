using System;
using TMPro;
using UnityEngine;

public class moneyAdded : MonoBehaviour
{
    private TextMeshProUGUI fadeAwayText;
    private RectTransform rectTransform;

    public float fadeTime;
    public float alphaValue;
    public float fadeAwayPerSecond;
    public float rotation;

    public float movingSpeed = 0.1f;

    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();

        fadeAwayText = GetComponent<TextMeshProUGUI>();
        fadeAwayPerSecond = 1 / fadeTime;
        alphaValue = fadeAwayText.color.a;

        float temp2 = UnityEngine.Random.Range(-rotation, rotation);
        rectTransform.localRotation = Quaternion.Euler(0, 0, temp2);
    }


    void Update()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();

        float temp = movingSpeed * Time.deltaTime;
        rectTransform.localPosition += new Vector3(temp, temp, 0f);

        if (fadeTime > 0)
        {
            alphaValue -= fadeAwayPerSecond * Time.deltaTime;
            fadeAwayText.color = new Color(fadeAwayText.color.r, fadeAwayText.color.g, fadeAwayText.color.b, alphaValue);
            fadeTime -= Time.deltaTime;
        }
    }

}
