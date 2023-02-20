using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Keyboard : MonoBehaviour
{
    public TMP_Text textField;


    string[] components = new string[] { "", "", "" };

    public void ChangeNote(ButtonType type, string c)
    {
        components[(int)type] = c;
        updateText();
    }

    void updateText()
    {
        textField.text = string.Join("", components);
    }
}
