using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class SelectButton : MonoBehaviour
{
    TextMeshProUGUI buttonText;
    private void Start()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        NameToButtonText();
        GetComponentInChildren<ButtonVR>().onRelease.AddListener(testFunc);
    }

    public void NameToButtonText()
    {
        buttonText.text = gameObject.name;
    }

    void testFunc()
    {
        Debug.Log("hello");
    }
}
