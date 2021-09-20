using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using KModkit;
using UnityEngine;

public class BinaryButtons : MonoBehaviour
{
    public KMAudio Audio;
    public KMBombInfo BombInfo;
    public KMBombModule BombModule;
    public KMSelectable[] Buttons;
    public KMSelectable Submit;
    public TextMesh[] Texts;

    int[] yourpresses = new int[5] { 0, 0, 0, 0, 0 };
    int[] correctpresses = new int[5] { 0, 0, 0, 0, 0 };

    static int moduleIdCounter = 1;
    int moduleId;
    bool ModuleSolved;

    void Awake()
    {
        moduleId = moduleIdCounter++;

        for (byte i = 0; i < Buttons.Length; i++)
        {
            KMSelectable aaaa = Buttons[i];
            aaaa.OnInteract += delegate
            {
                HandlePress(aaaa);
                return false;
            };
        }

        Submit.OnInteract += delegate ()
        {
            Submit.AddInteractionPunch();
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Submit.transform);
            if (!ModuleSolved)
            {
                if ((yourpresses[0] == correctpresses[0]) && (yourpresses[1] == correctpresses[1]) && (yourpresses[2] == correctpresses[2]) && (yourpresses[3] == correctpresses[3]) && (yourpresses[4] == correctpresses[4]))
                {
                    BombModule.HandlePass();
                    Debug.LogFormat("[Binary Buttons #{0}] You submitted {1}{2}{3}{4}{5}, and that's correct! Module solved!", moduleId, yourpresses[0], yourpresses[1], yourpresses[2], yourpresses[3], yourpresses[4]);
                    ModuleSolved = true;
                }
                else
                {
                    BombModule.HandleStrike();
                    Debug.LogFormat("[Binary Buttons #{0}] You submitted {1}{2}{3}{4}{5} when I expected {6}{7}{8}{9}{10}. Strike!", moduleId, yourpresses[0], yourpresses[1], yourpresses[2], yourpresses[3], yourpresses[4], correctpresses[0], correctpresses[1], correctpresses[2], correctpresses[3], correctpresses[4]);
                }
            }
            return false;
        };
    }

    void Start()
    {
        if ((BombInfo.IsIndicatorOn(Indicator.BOB)) && ((BombInfo.GetBatteryCount() > 2)) && BombInfo.IsPortPresent(Port.Parallel))
        {
            Debug.LogFormat("[Binary Buttons #{0}] First rule applies, submit 00000.", moduleId);
        }
        else if ((BombInfo.GetBatteryCount() == 0) && (BombInfo.GetPortCount() > 3))
        {
            correctpresses[1] = 1;
            correctpresses[2] = 1;
            correctpresses[4] = 1;
            Debug.LogFormat("[Binary Buttons #{0}] Second rule applies, submit 13 in binary.", moduleId);
        }
        else if ((BombInfo.GetOnIndicators().Count() > 0) && (BombInfo.IsPortPresent(Port.PS2)))
        {
            correctpresses[1] = 1;
            correctpresses[3] = 1;
            correctpresses[4] = 1;
            Debug.LogFormat("[Binary Buttons #{0}] Third rule applies, submit 26 in binary, reversed.", moduleId);
        }
        else if (BombInfo.GetBatteryCount() >= 2)
        {
            correctpresses[1] = 1;
            correctpresses[2] = 1;
            correctpresses[3] = 1;
            Debug.LogFormat("[Binary Buttons #{0}] Fourth rule applies, submit 7 in binary, with the digits reversed & the 1st and 4th digits swapped.", moduleId);
        }
        else
        {
            correctpresses[0] = 1;
            correctpresses[2] = 1;
            correctpresses[3] = 1;
            Debug.LogFormat("[Binary Buttons #{0}] No rules applied. Submit 11 in binary after the 3rd and 4th digits are swapped & all digits are reversed.", moduleId);
        }
    }

    void HandlePress(KMSelectable aaaa)
    {
        int aly = Array.IndexOf(Buttons, aaaa);
        Buttons[aly].AddInteractionPunch();
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Buttons[aly].transform);
        if (!ModuleSolved)
        {
            yourpresses[aly]++;
            if (yourpresses[aly] == 2)
                yourpresses[aly] = 0;
            Texts[aly].text = yourpresses[aly].ToString();
        }
    }
}
