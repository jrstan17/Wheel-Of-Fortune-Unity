using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTracks : MonoBehaviour {

    public AudioClip PuzzleReveal;
    public AudioClip LetterReveal;
    public AudioClip Buzzer;
    public AudioClip DoubleBuzzer;
    public AudioClip Drumroll;
    public AudioClip CymbalCrash;
    public AudioClip RoundWin;
    public AudioClip Bankrupt;
    public AudioClip Oh;
    public AudioClip Remains;
    public AudioClip CheerApplause;
    public AudioClip Cheering;
    public AudioClip FreePlay;
    public AudioClip Prize;
    public AudioClip PoliceQuest;
    public AudioClip Theme;
    public AudioClip CountdownMusic;
    public AudioClip Ah;
    public AudioClip Boo;
    public AudioClip CarHorn;
    public AudioClip WheelClick;
    public AudioClip ClapStart;
    public AudioClip ClapSustain;
    public AudioClip ClapFinish;

    public float PitchBend = 0.25f;

    List<AudioSource> Sources;

    void Start() {
        Sources = new List<AudioSource>();
        AddNewSource();
    }

    void AddNewSource() {
        AudioSource aSource = gameObject.AddComponent<AudioSource>();
        Sources.Add(aSource);
    }

    public void Play(string name) {
        AudioSource AudioSource = null;

        for (int i = 0; i < Sources.Count; i++) {
            if (!Sources[i].isPlaying) {
                AudioSource = Sources[i];
                break;
            }

            if (i == Sources.Count - 1) {
                AddNewSource();
                AudioSource = Sources[i + 1];
            }
        }

        AudioSource.loop = false;
        AudioSource.pitch = 1f;

        if (name.Equals("reveal")) {
            AudioSource.clip = PuzzleReveal;
        } else if (name.Equals("ding")) {
            AudioSource.clip = LetterReveal;
        } else if (name.Equals("wheel_click")) {
            AudioSource.clip = WheelClick;
            AudioSource.pitch += Random.Range(-PitchBend, PitchBend);
        } else if (name.Equals("buzzer")) {
            AudioSource.clip = Buzzer;
        } else if (name.Equals("double_buzzer")) {
            AudioSource.clip = DoubleBuzzer;
        } else if (name.Equals("round_win")) {
            AudioSource.clip = RoundWin;
        } else if (name.Equals("clap_start")) {
            AudioSource.clip = ClapStart;
        } else if (name.Equals("clap_sustain")) {
            AudioSource.clip = ClapSustain;
            AudioSource.loop = true;
        } else if (name.Equals("clap_finish")) {
            AudioSource.clip = ClapFinish;
        } else if (name.Equals("bankrupt")) {
            AudioSource.clip = Bankrupt;
        } else if (name.Equals("oh")) {
            AudioSource.clip = Oh;
        } else if (name.Equals("remains")) {
            AudioSource.clip = Remains;
        } else if (name.Equals("car_horn")) {
            AudioSource.clip = CarHorn;
        } else if (name.Equals("cheering")) {
            AudioSource.clip = Cheering;
        } else if (name.Equals("freeplay")) {
            AudioSource.clip = FreePlay;
        } else if (name.Equals("prize")) {
            AudioSource.clip = Prize;
        } else if (name.Equals("pq")) {
            AudioSource.clip = PoliceQuest;
        } else if (name.Equals("theme")) {
            AudioSource.clip = Theme;
            AudioSource.loop = true;
        } else if (name.Equals("ah")) {
            AudioSource.clip = Ah;
        } else if (name.Equals("drumroll")) {
            AudioSource.clip = Drumroll;
        } else if (name.Equals("cymbal_crash")) {
            AudioSource.clip = CymbalCrash;
        } else if (name.Equals("cheer_applause")) {
            AudioSource.clip = CheerApplause;
        } else if (name.Equals("countdown")) {
            AudioSource.clip = CountdownMusic;
            AudioSource.loop = true;
        } else if (name.Equals("boo")) {
            AudioSource.clip = Boo;
        } else {
            return;
        }

        AudioSource.Play();
    }

    public AudioSource GetSource(string objName) {
        foreach(AudioSource source in Sources) {
            if (source.clip.name.Equals(objName)) {
                return source;
            }
        }

        return null;
    }

    public void Stop() {
        foreach (AudioSource source in Sources) {
            source.Stop();
        }
    }
}
