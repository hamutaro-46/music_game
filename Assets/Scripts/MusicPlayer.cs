using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    public float CurrentTime { get { return audioSource.time; } }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void MusicPlay()
    {
        audioSource.time = 0f;
        audioSource.Play();
    }

    // Update is called once per frame
    public void MusicStop()
    {
        audioSource.Stop();
    }
}