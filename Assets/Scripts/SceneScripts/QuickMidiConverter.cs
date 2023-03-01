using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuickMidiConverter : MonoBehaviour
{
    public TextAsset midiConversionText;
    public List<string> notes;
    public List<int> midiNums;
    Dictionary<string, int> noteToMidiDict;

    private void Initialize()
    {
        string json = midiConversionText.text;
        noteToMidiDict = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
    }

    [ContextMenu("Notes to midi")]
    public void ConvertNotes()
    {
        Initialize();
        midiNums = new List<int>();
        foreach (var note in notes) 
        {
            midiNums.Add(noteToMidiDict[note]);
        }
    }
    [ContextMenu("Midi to note")]
    public void MidiToNote()
    {
        Initialize();
        notes = new List<string>();
        foreach (int num in midiNums)
        {
            notes.Add(ReverseLookup(num));
        }
    }

    private string ReverseLookup(int num)
    {
        foreach (var pair in noteToMidiDict)
        {
            if (pair.Value == num) return pair.Key;
        }

        return "";
    }
}
