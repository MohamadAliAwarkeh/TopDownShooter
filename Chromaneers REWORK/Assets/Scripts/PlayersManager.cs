using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

//Players Manager is a script that handles connect players from existing devices.
//I am using an InputManager.OnDeviceDetached so we can reference which controller is stored
//for each players and to know when one is detached.
//
//To detect a player joining, we will simply check the current active device (Which is the last
//device to provide input) for a relevant button press, check if it is or is not already assigned
//to a player, and then create a new player for it.
//
//There are a few limitations that could be easily worked around but for now I will stick with 
//this, but at a later date, I might work on further implementing it where if a player disconnects
//then a UI pop up will appear pausing the game stating a controller has been disconnected, and
//to resume the game, players must reconnect that controller.

public class PlayersManager : MonoBehaviour {

    //Public Variables
    public GameObject playerPrefab;
    const int maxPlayers = 3;


    //This is a list to show available spawnPoints for the new players to connect to
    List<Vector3> playerSpawnPoints = new List<Vector3>()
    {
        new Vector3(1, 1, 0),
        new Vector3(0, 1, 0),
        new Vector3(0, 1, 1),
    };

    List<PlayerController> players = new List<PlayerController>(maxPlayers);

	void Start () {
        InputManager.OnDeviceDetached += OnDeviceDetached;
	}
	
	void Update () {
        //Here I am checking if a button was pressed then if the player that pressed that button
        //already has a controller or not, if they do not already have a controller assigned to them
        //then I create a new player for them and spawn them in the next available spawnPoint
        var inputDevice = InputManager.ActiveDevice;

        if (JoinButtonWasPressedOnDevice(inputDevice))
        {
            if (ThereIsNoPlayerUsingDevice(inputDevice))
            {
                CreatePlayer(inputDevice);
            }
        }
	}

    bool JoinButtonWasPressedOnDevice(InputDevice inputDevice)
    {
        //Providing certain inputs, and if one is pressed then the action is performed
        return inputDevice.Action1.WasPressed || inputDevice.Action2.WasPressed || inputDevice.Action3.WasPressed || inputDevice.Action4.WasPressed;
    }

    PlayerController FindPlayerUsingDevice(InputDevice inputDevice)
    {
        //This is to check which player are connect and using which device, so an array
        //will run through all currently active players and check if they are using a device 
        //or not
        var playerCount = players.Count;
        for (var i = 0; i < playerCount; i++)
        {
            var player = players[i];
            if (player.Device == inputDevice)
            {
                return player;
            }
        }

        return null;
    }

    bool ThereIsNoPlayerUsingDevice(InputDevice inputDevice)
    {
        //If there is not player using a device, then we will return null
        return FindPlayerUsingDevice(inputDevice) == null;
    }

    void OnDeviceDetached(InputDevice inputDevice)
    {
        //Check if the device is detached, and if so to then remove the player from the scene
        var player = FindPlayerUsingDevice(inputDevice);
        if (player != null)
        {
            RemovePlayer(player);
        }
    }

    PlayerController CreatePlayer(InputDevice inputDevice)
    {
        //Here we are simply creating and instantiating the player into the scene
        if (players.Count < maxPlayers)
        {
            //Takes a position off of the list created.
            //The position will be added back if the player is removed
            var playerPosition = playerSpawnPoints[0];
            playerSpawnPoints.RemoveAt(0);

            var gameObject = (GameObject)Instantiate(playerPrefab, playerPosition, Quaternion.identity);
            var player = gameObject.GetComponent<PlayerController>();
            player.Device = inputDevice;
            players.Add(player);

            return player;
        }

        return null;
    }

    void RemovePlayer(PlayerController player)
    {
        playerSpawnPoints.Insert(0, player.transform.position);
        players.Remove(player);
        player.Device = null;
        Destroy(player.gameObject);
    }
}
