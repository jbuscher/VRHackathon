using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager: MonoBehaviour 
{	
	//Information the Player may need to share (might want all info here)
	public class Player
	{
		public string name = "DefaultName";
		public string info = null;

		public Player(string newName)
		{
			myConstructor(newName, null);
		}
		public Player(string newName, string newInfo)
		{
            myConstructor(newName, newInfo);
		}
        public void myConstructor(string newName, string newInfo)
        {
            name = newName;
            info = newInfo;
            // Right now all people store is a name and a null info
        }

		public void changeName(string newName)
		{
			name = newName;
		}
	}


	string registeredHostName = "TwoBrothers"; // Master Server name
	string serverName = "DefaultServer"; // Server name

	float refreshRequestLength = 3.0f; // Length of server searching
	bool isRefreshing = false; // Is client currently refreshing server list
	HostData[] hostData; // Stores the found servers

	string pickedName = "DefaultName"; // Stores the name chosen at the main menu
	int pickedSlot = 0; // Stores the slot chosen
	string[] slotPossibilities = new string[5]{"Slot 1", "Slot 2", "Slot 3", "Slot 4", "Slot 5"};

	bool gameOn = false; // Is player currently in game
	bool startingServer = false;
	bool joiningServer = false;
	bool options = false;
	bool credits = false;

	public Texture2D background;

	// Player information
	public Player me; // I am a player
	Dictionary<string, Player> players = new Dictionary<string, Player>(); // The list of players

	public static NetworkManager Instance; // Create an instance of this object (test if it exists already)

	// Ensure manager does not already exist
	void Awake()
	{
		if(Instance)
			DestroyImmediate(gameObject);
		else
		{
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
	}

    // This is the LOADING script for loading a character
    private string loadSlot(int slot)
    {
        return null;
    }

	// Command to add a player. The server tells the client its IP address and to give it
	// their player information, then the server stores the information and gives the 
	// client the full server list.
	[RPC]
	public void AddMe(string ip, string name)
	{
		if (Network.isServer)
		{
            // A client just ensured they exists and sent you their name
			Debug.Log (ip + " has said hi!");
			if (!players.ContainsKey(ip)) // Are they already in the game list
            {
                Debug.Log("New Player: " + name);
				players.Add (ip, new Player(name)); // If not, add them to the list!
            }
			StartCoroutine("GivePlayerList", ip); // Now you need to tell players about the new guy!
		} else {
            // The server just said hello, respond back with your name
            Debug.Log ("Tell the server you exist!");
			GetComponent<NetworkView>().RPC ("AddMe", RPCMode.Server, Network.player.ipAddress, me.name);
		}
	}

	// Server sends out the player list
	[RPC]
	public IEnumerator GivePlayerList(string ip)
	{
		if (Network.isServer)
		{
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();

            // Find the new player in the network (using the ip)
            foreach (NetworkPlayer player in Network.connections)
            {
                if (player.ipAddress == ip)
                {
                    NetworkPlayer newPlayer = player;

                    // Once the player is located, send the new information out
                    foreach (KeyValuePair<string, Player> playerSub in players)
                    {
                        // If I am sending the new player data, send it to everyone
                        if (playerSub.Key == ip)
                            GetComponent<NetworkView>().RPC ("AddPlayerToList", RPCMode.OthersBuffered, playerSub.Key, playerSub.Value.name);
                        // Otherwise, just send each player data to the new player
                        else
                            GetComponent<NetworkView>().RPC ("AddPlayerToList", newPlayer, playerSub.Key, playerSub.Value.name);
                    }
                    break;
                }
            }
            Debug.Log("Sent Out Player List");
		}
	}

	// Client adds players to the list
	[RPC]
	public void AddPlayerToList(string ip, string name)
	{
		if (Network.isClient) 
            if (!players.ContainsKey(ip)) // Ensure we dont duplicate
			    players.Add (ip, new Player (name));
	}

    // Client removes player from the list
    [RPC]
    public void RemovePlayerFromList(string ip)
    {
        if (Network.isClient)
            players.Remove (ip);
    }

    // Starts a server on the unity master server
    private void StartServer () 
    {
        Network.InitializeServer(16, 25002, false);
        MasterServer.RegisterHost(registeredHostName, serverName, "Insert Short Decription Miah");
        
        me = new Player (pickedName, loadSlot(pickedSlot)); // Load yourself
        
        players.Add(Network.player.ipAddress, me); // Add yourself to the player list
    }
    
    // Refreshes the server list
    public IEnumerator RefreshHostList()
    {
        // Start Refresh
        isRefreshing = true;
        Debug.Log("Refreshing...");
        MasterServer.RequestHostList(registeredHostName); // Request the list
        
        // Duration of refresh
        float timeEnd = Time.time + refreshRequestLength;
        
        while (Time.time < timeEnd)
        {
            hostData = MasterServer.PollHostList(); // Store data recieved
            yield return new WaitForEndOfFrame();
        }
        
        // Done Refreshing
        isRefreshing = false;
        
        if (hostData== null || hostData.Length == 0)
            Debug.Log("No Active Servers");
        else
            Debug.Log("Servers found: " + hostData.Length);
    }
    
    
    // The next few functions are server messages that are sent during either server or client events
    void OnConnectedToServer()
    {
        Debug.Log("Connected to server");

        // Load your player
        me = new Player(pickedName, loadSlot(pickedSlot));

        // Tell the server that you exist
        AddMe("",""); 
    }
    
    void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        Debug.Log("Disconnected from server");
        Application.LoadLevel("Intro"); // Go back to the main menu
    }
    
    void OnFailedToConnect(NetworkConnectionError error)
    {
        Debug.Log("Failed to connect");
    }
    
    void OnServerInitialized()
    {
        Debug.Log("Successfully initialized server!");
        Debug.Log("Your ip is: " + Network.player.ipAddress);
    }
    
    void OnMasterServerEvent(MasterServerEvent masterServerEvent)
    {
        if (masterServerEvent == MasterServerEvent.RegistrationSucceeded)
            Debug.Log("Registration sucessful");
    }
    
    void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Player from " + player.ipAddress + " connected");
    }
    
    void OnPlayerDisconnected(NetworkPlayer player)
    {
        Debug.Log("Player disconnected from:" + player.ipAddress + ":" + player.port);
        Network.RemoveRPCs(player); // Remove the pending network functions that have been called
        Network.DestroyPlayerObjects(player); // Destroy all objects they created
        
        players.Remove(player.ipAddress); // Take them off the player list
        // If someone disconnects, tell the other players
        GetComponent<NetworkView>().RPC ("RemovePlayerFromList", RPCMode.OthersBuffered, player.ipAddress);
    }
    
    void OnFailedToConnectToMasterServer(NetworkConnectionError info)
    {
        Debug.Log("Failed to connect to master server");
    }
    
    void OnNetworkInstantiate(NetworkMessageInfo info)
    {
        Debug.Log("Instantiated Network");
    }
    
    void OnApplicationQuit()
    {
        if (Network.isServer)
        {
            Network.Disconnect(200);
            MasterServer.UnregisterHost();
        }
        
        if (Network.isClient)
            Network.Disconnect(200);
    }

	public void AutoJoinServer() {
		if (hostData != null && hostData.Length > 0) {
			Network.Connect(hostData[0]);
		} else {
			StartCoroutine("RefreshHostList");
		}
	}

	public void StartServerPlease() {
		StartServer();
	}

    // IT'S
    // THE
    // THE
	// THE GUI!!!!!! (If you're reading this, you need to make the menu prettier)
	public void OnGUI()
	{
//		if (Network.isServer || Network.isClient)
//		{
//			int index = 0;
//			if (players.Values != null)
//			{
//				foreach (Player aPlayer in players.Values)
//				{
//					GUI.Button(new Rect(Screen.width - 110, 10 + 40*index ,100f , 30f), aPlayer.name);
//					index++;
//				}
//			}
//		}
//
//		// Show what player is running as (Mainly for debug)
//		if (Network.isServer)
//				GUILayout.Label("Running as a server.");
//		else if (Network.isClient)
//				GUILayout.Label("Running as a client.");
//
//		if ((Network.isServer) && !gameOn)
//		{
//			if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2, 150f, 30f), "Start Game!"))
//			{
//				gameOn = true;
//				GetComponent<NetworkView>().RPC ("LoadLevel", RPCMode.AllBuffered, "Main", 1);
//			}
//		}
//
//		if ((Network.isServer) && !gameOn)
//		{
//			if (GUI.Button(new Rect(10, 10, 150f, 30f), "Exit"))
//			{
//				gameOn = false;
//				GetComponent<NetworkView>().RPC ("LoadLevel", RPCMode.AllBuffered, "Intro", 0);
//			}
//		}
//
//		// If in a room, dont show anything beyond this point
//		if ((Network.isClient || Network.isServer))
//			return;
//
//		//GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), background);
//
//		if (startingServer)
//		{
//			// Get Server Name and Player Name
//			pickedName = GUI.TextField(new Rect(Screen.width/2 - 75f, Screen.height / 2 - 30f, 150f, 20f), pickedName, 16);
//			serverName = GUI.TextField(new Rect(Screen.width/2 - 75f, Screen.height / 2, 150f, 20f), serverName, 20);
//			if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2 + 30, 150f, 30f), "Start Server") && pickedName.Length > 4 && serverName.Length > 6)
//			{
//				startingServer = false;
//				StartServer();
//			}
//			
//			// Cancel
//			if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2 + 70, 150f, 30f), "Cancel"))
//			{
//				startingServer = false;
//			}
//		} else if (joiningServer) {
//			if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2 + 30, 150f, 30f), "Refresh Server List"))
//				StartCoroutine("RefreshHostList");
//			
//			// Perhaps delete? idk
//			if (isRefreshing)
//				GUILayout.Label("Refreshing...");
//			
//			// Host list
//			if (hostData != null)
//			{
//				for (int i = 0; i < hostData.Length; i++)
//				{
//					if (GUI.Button(new Rect(30f + (Screen.width - 360) * Mathf.RoundToInt(i/20), 65f + (30f * i), 300f, 30f), hostData[i].gameName))
//					{
//						joiningServer = false;
//						Network.Connect(hostData[i]);
//					}
//				}
//			}
//			
//			// Cancel
//			if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2 + 70, 150f, 30f), "Cancel"))
//			{
//				joiningServer = false;
//			}
//		} else if (options) {
//			
//			// Cancel
//			if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2 + 70, 150f, 30f), "Cancel"))
//			{
//				options = false;
//			}
//		} else if (credits) {
//			
//			// Cancel
//			if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2 + 70, 150f, 30f), "Cancel"))
//			{
//				credits = false;
//			}
//		} else {
//
//			// Create a server
//			if (GUI.Button (new Rect (Screen.width / 2 - 75f, Screen.height / 2 - 15, 150f, 30f), "Start a Server"))
//				startingServer = true;
//
//			// Join a server
//			if (GUI.Button(new Rect(Screen.width/2 - 75f, Screen.height/2 + 25, 150f, 30f), "Join a Server"))
//				joiningServer = true;
//
//			// Options
//			if (GUI.Button(new Rect(Screen.width/2 - 75f, Screen.height/2 + 65, 150f, 30f), "Options"))
//				options = true;
//
//			// Credits
//			if (GUI.Button(new Rect(Screen.width/2 - 75f, Screen.height/2 + 105, 150f, 30f), "Credits"))
//				credits = true;
//		}
	}

	// Online Script I found that lets you change scenes within Networks

	[RPC]
	public void LoadLevel(string level, int levelPrefix)
	{
		print(level + " " + levelPrefix);
		StartCoroutine(loadLevel(level, levelPrefix));
	}
	
	private IEnumerator loadLevel(string level, int levelPrefix)
	{
		Application.LoadLevel(level);
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		
		// Allow receiving data again
		Network.isMessageQueueRunning = true;
		// Now the level has been loaded and we can start sending out data
		Network.SetSendingEnabled(0, true);
		
		// Notify our objects that the level and the network is ready
		foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
			go.SendMessage("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver);
	}
}
