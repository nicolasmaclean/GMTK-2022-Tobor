using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

	public Transform player;
	public Transform exit;

	private bool playerEntered = false;

	// Update is called once per frame
	void Update () {
		if (playerEntered)
		{
			Vector3 portalToPlayer = player.position - transform.position;
			float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

			if (dotProduct < 0f)
			{
				float rotationDiff = -Quaternion.Angle(transform.rotation, exit.rotation);
				rotationDiff += 180;
				player.Rotate(Vector3.up, rotationDiff);

				Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
				player.position = exit.position + positionOffset;

				playerEntered = false;
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player")
		{
			playerEntered = true;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Player")
		{
			playerEntered = false;
		}
	}
}
