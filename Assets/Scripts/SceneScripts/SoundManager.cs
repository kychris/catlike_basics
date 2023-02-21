using MidiPlayerTK;
using MPTK.NAudio.Midi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public MidiStreamPlayer midiStreamPlayer;

    // Start is called before the first frame update
    void Start()
    {
        midiStreamPlayer = FindObjectOfType<MidiStreamPlayer>();
        PlayNote(60);
    }


    [ContextMenu("Play")]
    public void test() { PlayNote(60); }


    public void PlayNote(int num)
    {
        var mptkEvent = new MPTKEvent()
        {
            Channel = 0, // Between 0 and 15
            Duration = -1, // Infinite
            Value = num, // Between 0 and 127, with 60 plays a C4
            Velocity = 100, // Max 127
        };

        midiStreamPlayer.MPTK_PlayEvent(mptkEvent);
    }
}
