using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
//using static StylizedGrass.StylizedGrassGUI;



public enum ButtonType
{
    Note = 0,
    Accidental = 1,
    Octave = 2,
    Function = 3,
}

public class SelectButton : MonoBehaviour
{
    public ButtonType buttonType;
    Keyboard keyboard;
    TextMeshProUGUI buttonText;
    GameObject pressComponent;

    private void Start()
    {
        keyboard = GetComponentInParent<Keyboard>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        GetComponentInChildren<ButtonVR>().onRelease.AddListener(inputNote);

        NameToButtonText();
        pressComponent = transform.Find("Press").gameObject;
        SetColor();
    }

    public void NameToButtonText() { 
        //if (buttonType != ButtonType.Function)
            buttonText.text = gameObject.name;
    }

    void SetColor()
    {
        var material = pressComponent.GetComponent<Renderer>().material;
        switch (buttonType)
        {
            case ButtonType.Note:
                //material.color = Color.blue;
                break;
            case ButtonType.Accidental:
                material.color = new Color(0.5f, 1.0f, 0.5f);
                break;
            case ButtonType.Octave:
                material.color = new Color(0.5f, 0.5f, 1.0f);
                break;
            case ButtonType.Function:
                material.color = new Color(1.0f, 0.5f, 0.5f);
                break;
        }
    }

    void inputNote() 
    {
        // 0 - 2 input type button
        if ((int)buttonType <= 2)
        {
            keyboard.ChangeNote(buttonType, buttonText.text);
        }
        // function type button
        else
        {
            switch (buttonText.text)
            {
                case "+":
                    keyboard.AddNote();
                    break;
                case "-":
                    keyboard.DeleteNote();
                    break;
                case ".":
                    keyboard.ClearCurrentNote();
                    break;
                case "create":
                    keyboard.CreateNoteObject();
                    break;
            }
        }
    }
}
