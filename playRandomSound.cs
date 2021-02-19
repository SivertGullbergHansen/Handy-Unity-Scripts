using UnityEngine;

// Script written by Sivert Gullberg Hansen
// Version 1.0 [19-Feb-21]
// Made for Unity Version 2020+ (Most likely backwards compatible)
// https://github.com/SivertGullbergHansen
// https://sivert.xyz
// Description: This script spawns gameobjects inside a set area-space and plays a random clip, taken from the array of clips inside the script component.
public class playRandomSound : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Vector3 size = new Vector3(1,1,1);
    public float spawnTime = 6f;
    public float destroyTime = 8f;
    private float timing;
    [Header("AudioSource Settings")]
    [Range(0,1)]
    public float SpatialBlend = 1f;
    [Range(0, 1)]
    public float Volume = 1f;
    [Range(0, 1000)]
    public float mindDistance = 10f;
    [Range(0, 1000)]
    public float maxDistance = 300f;
    [Header("AudioClips")]
    public AudioClip[] Clips;
    [Header("DEBUG")]
    public Color DEBUG_colour = new Color(1,0,1,235/255f);


    private int randomPositiveNegative()
    {
        int someValue = Random.Range(0, 2) * 2 - 1;
        return someValue;
    }
    private Vector3 findLocation()
    {
        float x = 0;
        float y = 0;
        float z = 0;
        x = transform.position.x + Random.Range(0, size.x) * randomPositiveNegative();
            y = transform.position.y + Random.Range(0, size.y) * randomPositiveNegative();
            z = transform.position.z + Random.Range(0, size.z) * randomPositiveNegative();

        Debug.Log("Location: " + new Vector3(x, y, z));
        return new Vector3(x, y, z);
    }

    private void InstantiateAudioClip(Vector3 position)
    {
        GameObject audioclip = new GameObject();
        audioclip.name = "Sound Source";
        audioclip.AddComponent<AudioSource>();
        audioclip.GetComponent<AudioSource>().volume = Volume;
        audioclip.GetComponent<AudioSource>().spatialBlend = SpatialBlend;
        audioclip.GetComponent<AudioSource>().minDistance = mindDistance;
        audioclip.GetComponent<AudioSource>().maxDistance = maxDistance;
        audioclip.transform.position = position;
        if (Clips.Length != 0)
        {
            audioclip.GetComponent<AudioSource>().PlayOneShot(Clips[Random.Range(0, Clips.Length)]);
        }
        else
        {
            Debug.LogError("No clip found. Clips-variable contains no AudioClips! Please populate the array in the script of the object.");
        }

        Destroy(audioclip,destroyTime);
    }

    private void Start()
    {
        timing = spawnTime;
    }

    private void Timer()
    {
        if (timing < spawnTime)
        {
            timing += Time.deltaTime;
        }
        else if (timing >= spawnTime)
        {
            InstantiateAudioClip(findLocation());
            timing = 0;
        }
    }

    private void Update() => Timer();

    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = DEBUG_colour;
        Gizmos.DrawCube(transform.position, size);
    }
    void OnDrawGizmos()
    {
        // Draws the Light bulb icon at position of the object.
        // Because we draw it inside OnDrawGizmos the icon is also pickable
        // in the scene view.

        Gizmos.DrawIcon(transform.position, "audio-wave-1.png", true);
    }
}
