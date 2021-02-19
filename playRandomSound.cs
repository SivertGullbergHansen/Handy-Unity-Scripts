using UnityEngine;
using UnityEngine.Audio;

// Script written by Sivert Gullberg Hansen
// Version 1.1 [19-Feb-21]
// Made for Unity Version 2020+ (Most likely backwards compatible)
// https://github.com/SivertGullbergHansen
// https://sivert.xyz
// Description: This script spawns gameobjects during runtime inside a set area-space and plays a random audioclip taken from the array of audioclips inside the script component. Tutorial here: https://youtu.be/Yxr0_egPvcI
public class playRandomSound : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Vector3 size = new Vector3(1, 1, 1);
    public float spawnTime = 6f;
    public float destroyTime = 8f;
    private float timing;
    [Header("AudioSource Settings")]
    public AudioMixerGroup outputAudioMixerGroup;
    public bool bypassEffects;
    public bool bypassListenerEffects;
    public bool bypassReverbZones;
    [Range(0, 256)]
    public int priority = 128;
    [Range(0, 1)]
    public float volume = 1f;
    [Range(-3, 3)]
    public float pitch = 1;
    [Range(-1, 1)]
    public float panStereo = 0;
    [Range(0, 1)]
    public float spatialBlend = 0;
    [Range(0, 1.1f)]
    public float reverbZoneMix = 1;
    [Range(0, 5)]
    public float dopplerLevel = 1;
    [Range(0, 360)]
    public float spread = 0;
    public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;
    [Range(0, 1000)]
    public float mindDistance = 1;
    [Range(0, 1000)]
    public float maxDistance = 500;
    [Header("AudioClips")]
    public AudioClip[] clips;
    [Header("DEBUG")]
    public Color DEBUG_colour = new Color(67.5f / 255, 92.5f / 255, 83.5f / 255, 235 / 255f);


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
        x = transform.position.x + Random.Range(0, size.x / 2) * randomPositiveNegative();
        y = transform.position.y + Random.Range(0, size.y / 2) * randomPositiveNegative();
        z = transform.position.z + Random.Range(0, size.z / 2) * randomPositiveNegative();

        Debug.Log("Location: " + new Vector3(x, y, z));
        return new Vector3(x, y, z);
    }

    private void InstantiateAudioClip(Vector3 position)
    {
        GameObject audioclip = new GameObject();
        audioclip.name = "Sound Source";
        audioclip.AddComponent<AudioSource>();
        AudioSource source = audioclip.GetComponent<AudioSource>();
        source.outputAudioMixerGroup = outputAudioMixerGroup;
        source.bypassEffects = bypassEffects;
        source.bypassListenerEffects = bypassListenerEffects;
        source.bypassReverbZones = bypassReverbZones;
        source.priority = priority;
        source.volume = volume;
        source.pitch = pitch;
        source.panStereo = panStereo;
        source.spatialBlend = spatialBlend;
        source.reverbZoneMix = reverbZoneMix;
        source.dopplerLevel = dopplerLevel;
        source.spread = spread;
        source.rolloffMode = rolloffMode;
        source.minDistance = mindDistance;
        source.maxDistance = maxDistance;
        audioclip.transform.position = position;
        if (clips.Length != 0)
        {
            audioclip.GetComponent<AudioSource>().PlayOneShot(clips[Random.Range(0, clips.Length)]);
        }
        else
        {
            Debug.LogError("No clip found. Clips-variable contains no AudioClips! Please populate the array in the script of the object.");
        }

        Destroy(audioclip, destroyTime);
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
        // Gizmo is not provided, but you can use whatever .png image you have :) Place it inside Assets/Gizmos/ and name the file: "audio-wave-1.png" (or edit the line below and replace "audio-wave-1.png" with "your-file-name.png"
        Gizmos.DrawIcon(transform.position, "audio-wave-1.png", true);
    }
}
