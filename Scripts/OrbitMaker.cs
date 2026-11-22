using System.Collections.Generic;
using UnityEngine;

public class OrbitMaker : MonoBehaviour
{
    [SerializeField]
    public Transform centerObject;
    private float orbitSpeed;
    private float orbitRadius;
    private float initialAngle;

    private Dictionary<string, float> orbitSpeeds = new Dictionary<string, float>()
    {
        { "Mercury", (4.7f/2)/4 },
        { "Venus", (3.5f/2)/2/4 },
        { "Earth", (3.0f / 2)/4 },
        { "Mars", (2.4f / 2)/4 },
        { "Jupiter", (1.3f / 2)/2 },
        { "Saturn", (0.9f / 2)/2 },
        { "Uranus", (0.6f / 2)/2 },
        { "Neptune", (0.5f / 2)/2 },
        { "Pluto", (0.3f / 2)/2 },
        {"Moon",(40 / 2)/2 }
    };

    private Dictionary<string, float> orbitRadii = new Dictionary<string, float>()
    {
        { "Mercury", 617.9f },
        { "Venus", 668.2f },
        { "Earth", 709.6f },
        { "Mars", 787.9f },
        { "Jupiter", 1338.5f },
        { "Saturn", 1993.5f },
        { "Uranus", 3432.5f },
        { "Neptune", 5055.1f },
        { "Pluto", 6466.4f },
        {"Moon", 1.30136f }
    };
    //void Start()
    //{
    //    if (orbitSpeeds.ContainsKey(gameObject.name))
    //    {
    //        orbitSpeed = orbitSpeeds[gameObject.name];
    //        orbitRadius = orbitRadii[gameObject.name];
    //        //if (gameObject.name != "Moon")
    //        //{
    //        //    initialAngle = Random.Range(0f, 360f);
    //        //}


    //        float radians = initialAngle * Mathf.Deg2Rad;

    //        float x = centerObject.position.x + orbitRadius * Mathf.Cos(radians);
    //        float z = centerObject.position.z + orbitRadius * Mathf.Sin(radians);

    //        transform.position = new Vector3(x, transform.position.y, z);
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Planet name not found in dictionary: " + gameObject.name);
    //    }
    //}

    //void Update()
    //{
    //    if (centerObject != null)
    //    {
    //        transform.RotateAround(centerObject.position, Vector3.up, orbitSpeed/4 * Time.deltaTime);
    //    }
    //}
}
