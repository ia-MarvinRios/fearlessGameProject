using UnityEngine;

public class ObjectSounds : MonoBehaviour
{
    [SerializeField] AudioClip[] objectSounds;

    public void PlayOnObjectPos(int index)
    {
        if (objectSounds.Length > 0)
        {
            for (int i = 0; i < objectSounds.Length; i++)
            {
                if (i == index)
                {
                    AudioSource.PlayClipAtPoint(objectSounds[i], transform.position);
                }
            }
        }
    }
}
