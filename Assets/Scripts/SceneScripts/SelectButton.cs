using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.InputSystem;

public enum ButtonType
{
    Note,
    Octave,
    Accidental
}

public class SelectButton : MonoBehaviour
{
    public ButtonType buttonType;
    Keyboard keyboard;
    TextMeshProUGUI buttonText;

    

    private void Start()
    {
        keyboard = GetComponentInParent<Keyboard>();
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
        switch(buttonType)
        {
            case ButtonType.Note:
                keyboard.ChangeNotes(buttonText.text); 
                break;
            case ButtonType.Octave:
                keyboard.ChangeOctaves(buttonText.text);
                break;
            case ButtonType.Accidental:
                keyboard.ChangeAccidentals(buttonText.text);
                break;
        }
    }
}
