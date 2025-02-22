using UnityEngine;
using Unity.Netcode;

public class DisplayDelegator : NetworkBehaviour
{
    private bool DisplayOptions = true;
    public bool LockDisplay = false;
    private Camera[] realAllCameras;
    public enum DisplayType
    {
        Unassigned,
        Host,
        Map,
        Radar,
        Console,
    }
    public DisplayType CurrentDisplay = new DisplayType();
    public enum HostMode
    {
        Off,
        PlaceAsteroids,
        MoveShip,
    }
    public HostMode CurrentMode = new HostMode();
    public GameObject Asteroid = null;

    public static DisplayDelegator LocalDelegator = null;

    private string newSpeed = "0.01";
    private string newTurn = "0.01";
    private string forceHeading = "0";

    void Update()
    {
        if (CurrentDisplay == DisplayType.Host)
        {

            if (CurrentMode == HostMode.PlaceAsteroids && Input.GetMouseButtonDown(1))
            {
                Vector3 mPosition = Input.mousePosition;
                mPosition.z = 10;
                SpawnAsteroidRpc(Camera.main.ScreenToWorldPoint(mPosition));
            }
            if (CurrentMode == HostMode.MoveShip && Input.GetMouseButtonDown(1))
            {
                Vector3 mPosition = Input.mousePosition;
                mPosition.z = 10;
                MoveShipRpc(Camera.main.ScreenToWorldPoint(mPosition));
            }
        }
    }

    public void Awake()
    {
        realAllCameras = Camera.allCameras;
        LocalDelegator = this;
        newSpeed = Move.InstanceShip.GetComponents<Move>()[0].speed.ToString();
        newTurn = Move.InstanceShip.GetComponents<Move>()[0].turnSpeed.ToString();
    }
    private void OnGUI()
    {
        if (this.IsOwner && (LockDisplay == false || CurrentDisplay == DisplayType.Host))
        {
            DisplayGUI();
        }
        if(CurrentDisplay == DisplayType.Host)
        {
            HostGUI();
        }
    }

    public void DisplayGUI()
    {

        GUILayout.BeginArea(new Rect(1, 1, 10, 10));
        if (GUILayout.Button("X"))
        {
            print("Button pressed");
            if (DisplayOptions)
            {
                DisplayOptions = false;
            }
            else
            {
                DisplayOptions = true;
            }
        }
        GUILayout.EndArea();

        if (DisplayOptions)
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));

            if (GUILayout.Button("Host Display"))
            {
                print("Setting display to host");
                SwitchCamera("HostCamera");
                CommandPromt.InstanceCommandPromt.DisplayConsole = false;
                CurrentDisplay = DisplayType.Host;
            }
            if (GUILayout.Button("Map Display"))
            {
                print("Setting display to map");
                SwitchCamera("MapCamera");
                CommandPromt.InstanceCommandPromt.DisplayConsole = false;
                CurrentDisplay = DisplayType.Map;
            }
            if (GUILayout.Button("Radar Display"))
            {
                print("Setting display to radar");
                SwitchCamera("RadarCamera");
                CommandPromt.InstanceCommandPromt.DisplayConsole = false;
                CurrentDisplay = DisplayType.Radar;
            }
            if (GUILayout.Button("Console Display"))
            {
                print("Setting display to console");
                SwitchCamera("ConsoleCamera");
                CommandPromt.InstanceCommandPromt.DisplayConsole = true;
                CurrentDisplay = DisplayType.Console;
            }
            if (!NetworkManager.IsHost && GUILayout.Button("Lock Display"))
            {
                LockDisplay = true;
            }

            GUILayout.EndArea();
        }
    }

    public void HostGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width - 300, 10, 300, 300));
        if (GUILayout.Button("Placing OFF"))
        {
            CurrentMode = HostMode.Off;
        }
        if (GUILayout.Button("Place Asteroids"))
        {
            CurrentMode = HostMode.PlaceAsteroids;
        }
        if (GUILayout.Button("Move Ship"))
        {
            CurrentMode = HostMode.MoveShip;
        }
        float newValue;

        //Sets the speed of the ship to the input
        GUILayout.Label("Set speed");
        if (float.TryParse(newSpeed, out newValue))
        {
            newSpeed = GUILayout.TextField(newSpeed);
            Move.InstanceShip.GetComponents<Move>()[0].setTurnRateRpc(newValue);
        }
        else
        {
            newSpeed = "0";
        }

        //Sets the rotations speed to the input
        GUILayout.Label("Set rotation speed");
        if(float.TryParse(newTurn,out newValue))
        {
            newTurn = GUILayout.TextField(newTurn);
            Move.InstanceShip.GetComponents<Move>()[0].setTurnRateRpc(newValue);
        }
        else
        {
            newTurn = "0";
        }

        //Sets the heading to the input
        //GUILayout.Label("force heading");
        //forceHeading = GUILayout.TextField(forceHeading);
        //if(float.Parse(forceHeading) > 0)
        //{
        //    Move.InstanceShip.GetComponents<Move>()[0].setTurnRpc(float.Parse(forceHeading));
        //}

        GUILayout.EndArea();
    }



    [Rpc(SendTo.Everyone)]
    public void SpawnAsteroidRpc(Vector3 SpawnPosition)
    {
        Instantiate(Asteroid, SpawnPosition, new Quaternion(0, 0, 0, 0));
    }

    [Rpc(SendTo.Everyone)]
    public void MoveShipRpc(Vector3 MovePosition)
    {
        Move.InstanceShip.transform.position = MovePosition;
    }


    //Enables the camera with the given name, and disables all others.
    private void SwitchCamera(string DesiredCam)
    {
        foreach (Camera curCam in realAllCameras)
        {
            if (string.Equals(curCam.gameObject.name, DesiredCam))
            {
                curCam.enabled = true;
            }
            else
            {
                curCam.enabled = false;
            }
        }
    }



}
