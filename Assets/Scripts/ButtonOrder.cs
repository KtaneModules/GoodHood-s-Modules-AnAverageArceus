using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using KModkit;
using UnityEngine;

public class ButtonOrder : MonoBehaviour
{
    public KMAudio Audio;
    public KMBombInfo BombInfo;
    public KMBombModule BombModule;
    public KMSelectable[] Buttons;

    int[] yourpresses = new int[2] { 0, 0 };
    int[] correctpresses = new int[2] { 0, 0 };

    static int moduleIdCounter = 1;
    int moduleId;
    bool ModuleSolved;

    void Awake()
    {
        moduleId = moduleIdCounter++;

        Buttons[0].OnInteract += delegate ()
        {
            Buttons[0].AddInteractionPunch();
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Buttons[0].transform);
            if (!ModuleSolved)
            {
                if (yourpresses[0] != 0)
                {
                    yourpresses[1] = 1;
                    if ((correctpresses[0] == yourpresses[0]) && (yourpresses[1] == correctpresses[1]))
                    {
                        BombModule.HandlePass();
                        ModuleSolved = true;
                        Debug.LogFormat("[Button Order #{0}] Buttons pressed correctly. Module solved!", moduleId);
                    }
                    else
                    {
                        BombModule.HandleStrike();
                        Debug.LogFormat("[Button Order #{0}] Incorrect order recieved, strike!", moduleId);
                        yourpresses[0] = 0;
                        yourpresses[1] = 0;
                    }
                }
                else
                    yourpresses[0] = 1;
            }
            return false;
        };

        Buttons[1].OnInteract += delegate ()
        {
            Buttons[1].AddInteractionPunch();
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Buttons[1].transform);
            if (!ModuleSolved)
            {
                if (yourpresses[0] != 0)
                {
                    yourpresses[1] = 2;
                    if ((correctpresses[0] == yourpresses[0]) && (yourpresses[1] == correctpresses[1]))
                    {
                        BombModule.HandlePass();
                        ModuleSolved = true;
                        Debug.LogFormat("[Button Order #{0}] Buttons pressed correctly. Module solved!", moduleId);
                    }
                    else
                    {
                        BombModule.HandleStrike();
                        Debug.LogFormat("[Button Order #{0}] Incorrect order recieved, strike!", moduleId);
                        yourpresses[0] = 0;
                        yourpresses[1] = 0;
                    }
                }
                else
                    yourpresses[0] = 2;
            }
            return false;
        };
    }

    void Start()
    {
        if (BombInfo.GetBatteryCount() >= 2)
        {
            correctpresses[0] = 1;
            correctpresses[1] = 2;
            Debug.LogFormat("[Button Order #{0}] At least 2 batteries are present. Press the buttons in order.", moduleId);
        }
        else
        {
            correctpresses[0] = 2;
            correctpresses[1] = 1;
            Debug.LogFormat("[Button Order #{0}] Less than 2 batteries are present. Press the buttons in reverse order.", moduleId);
        }
    }
}
