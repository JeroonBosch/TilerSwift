using UnityEngine;

public class AudioLibrary : MonoBehaviour
{

    public AudioClip[] audioLibrary;

    public AudioClip PlayFromLibrary(string clipName)
    {
        foreach (AudioClip clip in audioLibrary)
        {
            if (clip.name == clipName)
                return clip;
        }
        return null;
    }
}
