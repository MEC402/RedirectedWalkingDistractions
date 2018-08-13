using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantRowGenerator : MonoBehaviour {

    public GameObject plant;
    public int count;
    public float xSpacing;
    public float zVariance;
    public float rotationalVariance;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < count; ++i)
        {
            Quaternion rotation = Quaternion.Euler(0, Random.Range(-rotationalVariance, rotationalVariance), 0);
            Vector3 position = transform.position;
            position.x += i*xSpacing;
            position.z += Random.Range(-zVariance, zVariance);
            GameObject spawnedPlant = Instantiate(plant, position, rotation);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
