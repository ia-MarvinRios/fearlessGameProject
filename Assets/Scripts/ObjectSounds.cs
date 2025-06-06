using UnityEngine;
using UnityEngine.Audio;

public class ObjectSounds : MonoBehaviour
{
    [SerializeField] AudioClip[] objectSounds;
    [SerializeField] AudioMixerGroup mixerGroup;
    AudioSource sc;

    private void Start()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        sc = gm.gameObject.AddComponent<AudioSource>();
    }

    public void PlayOnObjectPos(int index)
    {
        if (objectSounds.Length > 0)
        {
            for (int i = 0; i < objectSounds.Length; i++)
            {
                if (i == index)
                {
                    PlaySoundAtPoint(objectSounds[i], transform.position);
                }
            }
        }
    }

    public void PlayGlobal(int index)
    {
        if (objectSounds.Length > 0 && !sc.isPlaying)
        {
            sc.clip = objectSounds[index];
            sc.outputAudioMixerGroup = mixerGroup;
            sc.Play();
        }
    }

    public void StopGlobal()
    {
        sc.Stop();
    }

    public void PlaySoundAtPoint(AudioClip clip, Vector3 position)
    {
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = position;

        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.outputAudioMixerGroup = mixerGroup;
        aSource.Play();

        Destroy(tempGO, clip.length);
    }
}
