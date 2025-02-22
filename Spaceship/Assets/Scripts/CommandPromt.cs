using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;


public class CommandPromt : NetworkBehaviour
{
    public Move Ship = null;
    public static CommandPromt InstanceCommandPromt;
    public List<string> SecretCodes = new List<string>();
    public int FTLCharges = 0;
    public bool DisplayConsole = false;

    private string PreviousCommands = "";
    private string Command = "";

    public int SpaceHeight = 15;
    public int MaxHeight = 100;
    public Texture2D BackgroundTexture = null;

    public TMP_InputField CurrentInput = null;
    public TextMeshProUGUI PreviousText = null;

    public bool LegacyUI = false;

    public void Awake()
    {
        InstanceCommandPromt = this;
    }

    public void Update()
    {
        if (DisplayConsole)
        {
            CurrentInput.ActivateInputField();
        }
    }

    public void TakeCommand()
    {
        if(CurrentInput.text.Length > 0)
        {
            ProcessCommand(CurrentInput.text);
            PreviousText.text = PreviousText.text + "\n" + CurrentInput.text;
            CurrentInput.text = "";
        }
    }

    public void ProcessCommand(string CurrentCommand)
    {

        //Adds the current command into the string of previous commands for display.
        PreviousCommands = PreviousCommands + "\n" + CurrentCommand;

        //Checks to if the current command matches any known commands
        string FirstCommand;
        float Degrees = 0;
        int firstSpace = CurrentCommand.IndexOf(" ");

        if (firstSpace != -1)
        {
            FirstCommand = CurrentCommand.Substring(0, firstSpace);
        }
        else
        {
            FirstCommand = CurrentCommand;
        }

        switch (FirstCommand)
        {
            case "turn_port":
                if (firstSpace != -1)
                    float.TryParse(CurrentCommand.Substring(firstSpace + 1), out Degrees);
                Ship.turnRpc(-Mathf.Abs(Degrees));
                break;

            case "turn_starboard":
                if (firstSpace != -1)
                    float.TryParse(CurrentCommand.Substring(firstSpace + 1), out Degrees);
                Ship.turnRpc(Mathf.Abs(Degrees));
                break;

            case "check_heading":
                DisplayCommandRpc("Current Heading:" + Ship.currentHeading.ToString()
                    + "\nTarget Heading:" + Ship.targetHeading.ToString());
                break;

            case "input_code":
                int codeLocation = SecretCodes.BinarySearch(CurrentCommand.Substring(firstSpace + 1));
                if (codeLocation != -1)
                {
                    SecretCodes.RemoveAt(codeLocation);
                    DisplayCommandRpc("Code accepted");
                    FTLCharges++;
                }
                else
                {
                    DisplayCommandRpc("Code Rejected");
                }
                break;

            case "check_FTLcharges":
                DisplayCommandRpc("Current Ftl Charges = " + FTLCharges);
                break;

            case "engage_FTLjump":
                if (FTLCharges > 0)
                {
                    DisplayCommandRpc("Intiating FTL Jump:");
                    Ship.jumpRpc();
                    FTLCharges--;
                }
                else
                {
                    DisplayCommandRpc("FTL jump failed, insufficient charges.");
                }
                break;

            default:
                DisplayCommandRpc(FirstCommand + " is not recognized as an internal or external command, \noperable program or batch file.");
                break;
        }
    }
        
    public int TerminalHeight()
    {
        int CurrentHeight = 0;
        foreach(char CurrentChar in PreviousCommands)
        {
            if(CurrentChar == (char)10)
            {
                CurrentHeight = CurrentHeight + SpaceHeight;
            }
        }
        return CurrentHeight;
    }
    //public void ForceLineEnders(string Input)
    //{
    //    while(Input.Contains("\\n"))
    //    {
    //        Input.Replace("\\n", "" + (char)10);
    //    }
    //}

    [Rpc(SendTo.Everyone)]
    public void DisplayCommandRpc(string NewString)
    {
        if(LegacyUI)
            PreviousCommands = PreviousCommands + "\n" + NewString;
        else
            PreviousText.text = PreviousText.text + "\n" + NewString;
    }


    //The UI Code for the old version of the UI, here as a backup.
    void OnGUI()
    {
        if (DisplayConsole == true && LegacyUI)
        {
            if (LegacyUI)
            {
                GUIStyle MyStyle = new GUIStyle();
                MyStyle.normal.background = BackgroundTexture;
                MyStyle.normal.textColor = new Color(255, 255, 255);
                MyStyle.alignment = TextAnchor.LowerLeft;

                int terminalHeight = TerminalHeight();
                GUILayout.BeginArea(new Rect(30, Screen.height - 75 - terminalHeight, Screen.width - 60, 2 * terminalHeight));
                GUILayout.Label(PreviousCommands, MyStyle);
                GUILayout.EndArea();

                GUILayout.BeginArea(new Rect(30, Screen.height - 50, 300, 300));
                Command = GUILayout.TextArea(Command, MyStyle);
                GUILayout.EndArea();

                if (Command.Length > 0)
                {
                    CurrentInput.text = "";
                    if (Command.Contains((char)10))
                    {
                        //Removes Line ender
                        int LineEnderPostion = Command.LastIndexOf((char)10);
                        Command = Command.Remove(LineEnderPostion, 1);

                        ProcessCommand(Command);
                        Command = "";
                    }
                }
            }
        }
    }


}
