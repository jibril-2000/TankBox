using UnityEngine;
using System.Collections;

public class AC : MonoBehaviour
{
    public AudioClip[] audioClip;
    private AudioSource audioSource;

    public void SE(int Num)
    {
        Debug.Log("a");
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioClip[Num]);
    }

}