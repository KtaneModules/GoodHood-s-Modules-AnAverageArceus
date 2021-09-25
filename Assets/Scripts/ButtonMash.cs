using System.Linq;
using KModkit;
using UnityEngine;

public class ButtonMash : MonoBehaviour
{
    public KMAudio Audio;
    public KMBombInfo BombInfo;
    public KMBombModule BombModule;
    public KMSelectable Mash;
    public KMSelectable Submit;
    public TextMesh Counter;

    int Mashes = 0;
    int CorrectMashes;

    static int moduleIdCounter = 1;
    int moduleId;
    bool ModuleSolved;

    void Awake()
    {
        moduleId = moduleIdCounter++;

        Mash.OnInteract += delegate ()
        {
            Mash.AddInteractionPunch();
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Mash.transform);
            if (!ModuleSolved)
            {
                if (Mashes == 99)
                {
                    BombModule.HandleStrike();
                    Debug.LogFormat("[Standard Button Masher #{0}] You passed 99 while you were mashing, strike!", moduleId);
                    Mashes = 0;
                }
                else
                    Mashes++;
                Counter.text = Mashes.ToString();
            }
            return false;
        };

        Submit.OnInteract += delegate ()
        {
            Submit.AddInteractionPunch();
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonPress, Submit.transform);
            if (Mashes == CorrectMashes)
            {
                BombModule.HandlePass();
                ModuleSolved = true;
                Debug.LogFormat("[Standard Button Masher #{0}] You submitted {1}, which was correct! Module solved!", moduleId, Mashes);
            }
            else
            {
                BombModule.HandleStrike();
                Debug.LogFormat("[Standard Button Masher #{0}] You submitted {1} but I expected {2}! Strike!", moduleId, Mashes, CorrectMashes);
                Mashes = 0;
                Counter.text = "0";
            }
            return false;
        };
    }

    void Start()
    {
        if ((BombInfo.IsIndicatorOn(Indicator.BOB)) && (BombInfo.GetPortCount() > 5) && (BombInfo.GetBatteryCount() > 4))
        {
            CorrectMashes = 0;
            Debug.LogFormat("[Standard Button Masher #{0}] First rule applies, no need to mash, just submit 0.", moduleId);
        }
        else if ((BombInfo.GetBatteryCount() > 2) && BombInfo.IsPortPresent(Port.Serial) && BombInfo.IsPortPresent(Port.DVI))
        {
            CorrectMashes = 83;
            Debug.LogFormat("[Standard Button Masher #{0}] Second rule applies, submit after 83 pushes.", moduleId);
        }
        else if ((BombInfo.GetModuleNames().Contains("Forget Me Not")) || BombInfo.GetModuleNames().Contains("Wires"))
        {
            CorrectMashes = 73;
            Debug.LogFormat("[Standard Button Masher #{0}] Third rule applies, submit after 73 pushes.", moduleId);
        }
        else if ((BombInfo.IsIndicatorPresent(Indicator.MSA) || BombInfo.IsIndicatorPresent(Indicator.NSA)) && BombInfo.GetPortCount() > 2)
        {
            CorrectMashes = 43;
            Debug.LogFormat("[Standard Button Masher #{0}] Fourth rule applies, submit after 43 pushes.", moduleId);
        }
        else if ((!BombInfo.IsDuplicatePortPresent()) && BombInfo.GetOnIndicators().Count() == 0)
        {
            CorrectMashes = 64;
            Debug.LogFormat("[Standard Button Masher #{0}] Fifth rule applies, submit after 64 pushes.", moduleId);
        }
        else
        {
            CorrectMashes = 98;
            Debug.LogFormat("[Standard Button Masher #{0}] No rules apply, submit after 98 pushes.", moduleId);
        }
    }
}
