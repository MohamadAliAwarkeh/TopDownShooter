using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class RoomSpawner : MonoBehaviour
{

	//Public Variables
	public int openingDirection;
	//1 ---> Spawns room with a S door
	//2 ---> Spawns room with a N door
	//3 ---> Spawns room with a W door
	//4 ---> Spawns room with a E door

	//Private Variables
	private RoomTemplates templates;
	private int rand;
	private bool spawned = false;

	void Start()
	{
		templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
		Invoke("CreateRoom", 0.1f);
	}
	
	void CreateRoom () 
	{
		if (spawned == false)
		{
			if (openingDirection == 1)
			{
				//Needs to spawn a room with a SOUTH door
				rand = Random.Range(0, templates.southRooms.Length);
				Instantiate(templates.southRooms[rand], transform.position, templates.southRooms[rand].transform.rotation);
			} 
			else if (openingDirection == 2)
			{
				//Needs to spawn a room with a NORTH door
				rand = Random.Range(0, templates.northRooms.Length);
				Instantiate(templates.northRooms[rand], transform.position, templates.northRooms[rand].transform.rotation);
			} 
			else if (openingDirection == 3)
			{
				//Needs to spawn a room with a WEST door
				rand = Random.Range(0, templates.westRooms.Length);
				Instantiate(templates.westRooms[rand], transform.position, templates.westRooms[rand].transform.rotation);
			}
			else if (openingDirection == 4)
			{
				//Needs to spawn a room with a EAST door
				rand = Random.Range(0, templates.eastRooms.Length);
				Instantiate(templates.eastRooms[rand], transform.position, templates.eastRooms[rand].transform.rotation);
			}

			spawned = true;
		}	
	}

	 void OnTriggerEnter2D(Collider2D theCol)
	{
		if (theCol.CompareTag("SpawnPoint"))
		{
			if (theCol.GetComponent<RoomSpawner>().spawned == false && spawned == false)
			{
				//Instantiates wall if there is an opening with a door
				Instantiate(templates.closedRooms, transform.position, Quaternion.identity);
				Destroy(gameObject);
			}

			spawned = true;
		}
	}
}
