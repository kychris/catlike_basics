using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;
using MPTK.NAudio.Midi;

public class TestMidi : MonoBehaviour
{
    public MidiStreamPlayer midiStreamPlayer;
    public long soundDuration;
    List<MPTKEvent> mptkEvents;

    // Start is called before the first frame update
    void Start()
    {
        mptkEvents = new List<MPTKEvent>();
        midiStreamPlayer = FindObjectOfType<MidiStreamPlayer>();
        midiStreamPlayer.MPTK_ChannelForcedPresetSet(0, 42);
    }

    [ContextMenu("Play")]
    void Play()
    {
        var mptkEvent = new MPTKEvent()
        {
            Channel = 0, // Between 0 and 15
            Duration = soundDuration, // Infinite
            Value = 60, // Between 0 and 127, with 60 plays a C4
            Velocity = 100, // Max 127
        };
        mptkEvents.Add(mptkEvent);

        mptkEvent = new MPTKEvent()
        {
            Channel = 0, // Between 0 and 15
            Duration = soundDuration, // Infinite
            Value = 64, // Between 0 and 127, with 60 plays a C4
            Velocity = 100, // Max 127
        };
        mptkEvents.Add(mptkEvent);

        mptkEvent = new MPTKEvent()
        {
            Channel = 0, // Between 0 and 15
            Duration = soundDuration, // Infinite
            Value = 67, // Between 0 and 127, with 60 plays a C4
            Velocity = 100, // Max 127
        };
        mptkEvents.Add(mptkEvent);

        foreach (var noteEvent in mptkEvents)
        {
            midiStreamPlayer.MPTK_PlayEvent(noteEvent);
        }
    }

    [ContextMenu("Stop")]
    void Stop()
    {
        foreach (var noteEvent in mptkEvents)
        {
            midiStreamPlayer.MPTK_StopEvent(noteEvent);
        }
    }

    [ContextMenu("Change")]
    void Change()
    {
        for (int i = 0; i < mptkEvents.Count; i++)
        {
            mptkEvents[i].RealTime = 30;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
