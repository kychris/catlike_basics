using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class NotesDictionary : MonoBehaviour {

    public static NotesDictionary Instance;
    private static Dictionary<string, int> noteToMidiDict;

    // Start is called before the first frame update
    public void Initialize() {
        string[] searchResuls = AssetDatabase.FindAssets("NotesToMidiNums");
        TextAsset midiConversionText = AssetDatabase.LoadAssetAtPath<TextAsset>(AssetDatabase.GUIDToAssetPath(searchResuls[0]));
        string json = midiConversionText.text;
        noteToMidiDict = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);

        Instance = this;
    }

    public int NoteToMidi(string note) {
        if (noteToMidiDict.ContainsKey(note)) 
            return noteToMidiDict[note];
        return 0;
    }

    public string MidiToNote(int midiNum) {
        foreach (var pair in noteToMidiDict) {
            if (pair.Value == midiNum) return pair.Key;
        }
        return "";
    }
}
