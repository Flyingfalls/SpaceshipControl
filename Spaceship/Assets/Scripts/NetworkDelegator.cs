using UnityEngine;
using Unity.Netcode;

public class NetworkDelegator : MonoBehaviour
{
    private static NetworkManager MyNetworkManager;
    private string IPAddress = "172.30.141.125";
    private string Port = "7777";

    private void Awake()
    {
        MyNetworkManager = GetComponent<NetworkManager>();
    }
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if(!MyNetworkManager.IsClient && !MyNetworkManager.IsServer)
        {
            StartButtons();

            GUILayout.Label("IP Address");
            IPAddress = GUILayout.TextField(IPAddress);
            GUILayout.Label("Port");
            Port = GUILayout.TextField(Port);
            gameObject.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>().SetConnectionData(IPAddress, ushort.Parse(Port));
        }
        GUILayout.EndArea();
    }

    static void StartButtons()
    {
        if (GUILayout.Button("Host"))
        {
            MyNetworkManager.StartHost();
        }
        if (GUILayout.Button("Client"))
        {
            MyNetworkManager.StartClient();
        }

    }


}
