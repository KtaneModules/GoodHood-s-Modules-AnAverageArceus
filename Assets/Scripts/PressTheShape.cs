using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using KModkit;
using UnityEngine;

public class PressTheShape : MonoBehaviour
{
    public KMAudio Audio;
    public KMBombInfo BombInfo;
    public KMBombModule BombModule;
    public KMSelectable[] Shapes;

    int CorrectShape;

    static int ModuleIdCounter = 1;
    int ModuleId;
    bool ModuleSolved;

    void Awake()
    {
        ModuleId = ModuleIdCounter++;

        Shapes[0].OnInteract += delegate ()
        {
            Shapes[0].AddInteractionPunch();
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Shapes[0].transform);
            if (!ModuleSolved)
            {
                if (CorrectShape == 0)
                {
                    BombModule.HandlePass();
                    ModuleSolved = true;
                    Debug.LogFormat("[Press The Shape #{0}] You pressed the triangle. Module solved.", ModuleId);
                }
                else
                {
                    BombModule.HandleStrike();
                    Debug.LogFormat("[Press The Shape #{0}] You pressed the triangle but that's not the right shape to press. Strike!", ModuleId);
                }
            }
            return false;
        };

        Shapes[1].OnInteract += delegate ()
        {
            Shapes[1].AddInteractionPunch();
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Shapes[1].transform);
            if (!ModuleSolved)
            {
                if (CorrectShape == 1)
                {
                    BombModule.HandlePass();
                    ModuleSolved = true;
                    Debug.LogFormat("[Press The Shape #{0}] You pressed the square. Module solved.", ModuleId);
                }
                else
                {
                    BombModule.HandleStrike();
                    Debug.LogFormat("[Press The Shape #{0}] You pressed the square but that's not the right shape to press. Strike!", ModuleId);
                }
            }
            return false;
        };

        Shapes[2].OnInteract += delegate ()
        {
            Shapes[2].AddInteractionPunch();
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Shapes[2].transform);
            if (!ModuleSolved)
            {
                if (CorrectShape == 2)
                {
                    BombModule.HandlePass();
                    ModuleSolved = true;
                    Debug.LogFormat("[Press The Shape #{0}] You pressed the circle. Module solved.", ModuleId);
                }
                else
                {
                    BombModule.HandleStrike();
                    Debug.LogFormat("[Press The Shape #{0}] You pressed the circle but that's not the right shape to press. Strike!", ModuleId);
                }
            }
            return false;
        };
    }

    void Start()
    {
        if ((BombInfo.GetBatteryCount() > 2) && (BombInfo.IsPortPresent(Port.Serial)))
        {
            CorrectShape = 1;
            Debug.LogFormat("[Press The Shape #{0}] Rule 1 applies. Press the square.", ModuleId);
        }
        else if (BombInfo.IsIndicatorOn(Indicator.TRN) || BombInfo.IsIndicatorOn(Indicator.BOB) || BombInfo.IsIndicatorOff(Indicator.MSA) || BombInfo.IsIndicatorOff(Indicator.NSA))
        {
            CorrectShape = 2;
            Debug.LogFormat("[Press The Shape #{0}] Rule 2 applies. Press the circle.", ModuleId);
        }
        else if ((BombInfo.GetBatteryCount() < 4) && !BombInfo.IsIndicatorPresent(Indicator.BOB))
        {
            CorrectShape = 0;
            Debug.LogFormat("[Press The Shape #{0}] Rule 3 applies. Press the triangle.", ModuleId);
        }
        else if ((BombInfo.GetOnIndicators().Count() == 0) && (BombInfo.IsPortPresent(Port.PS2) || BombInfo.IsPortPresent(Port.RJ45)))
        {
            CorrectShape = 2;
            Debug.LogFormat("[Press The Shape #{0}] Rule 4 applies. Press the circle.", ModuleId);
        }
        else
        {
            CorrectShape = 0;
            Debug.LogFormat("[Press The Shape #{0}] No rules apply. Press the triangle.", ModuleId);
        }
    }
}
