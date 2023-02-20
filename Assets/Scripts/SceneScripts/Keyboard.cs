using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Newtonsoft;
using Newtonsoft.Json;
using System;

public class Keyboard : MonoBehaviour
{
    public TMP_Text textField;
    public TextAsset midiConversionText;

    string[] components = new string[] { "", "", "" };

    private void Start()
    {
        LoadJson();
    }

    public void ChangeNote(ButtonType type, string c)
    {
        // Remove accidentals if seeing _
        if (c == "_")
        {
            components[2] = "";
        }
        else
        {
            components[(int)type] = c;
        }
        updateText();
    }

    void updateText()
    {
        textField.text = string.Join("", components);
    }

    void LoadJson()
    {
        string json = midiConversionText.text;
        Dictionary<string, int> data = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
        foreach (KeyValuePair<string, int> kvp in data)
        {
            Debug.Log(kvp.Key + " " + kvp.Value);
        }
    }
}

class MidiNum
{
    public string name;
}
