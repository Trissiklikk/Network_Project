using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleRagdoll : MonoBehaviour
{
    public BoxCollider mainCollider;
    public GameObject charactorRigs;
    public Animator charactorAnimation;
    public AudioSource audioSource;
    public AudioClip[] audioClipArray;
    
    


    void Start()
    {
        GetRagdollPart();
        RagdollModeOff();
    }

    
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            RagdollModeOn();
            audioSource.PlayOneShot(RandomClip());
        }
        if(collision.gameObject.tag == "Player")
        {
            RagdollModeOn();
            audioSource.Play();
        }
    }
    AudioClip RandomClip()
    {
        return audioClipArray[Random.Range(0, audioClipArray.Length)];
    }
    void GameOver()
    {
        Debug.Log("Mission Failed");
    }
    Collider[] ragdollColliders;
    Rigidbody[] limbsRigidbodies;
    void GetRagdollPart()
    {
        ragdollColliders = charactorRigs.GetComponentsInChildren<Collider>();
        limbsRigidbodies = charactorRigs.GetComponentsInChildren<Rigidbody>();
    }
    void RagdollModeOn()
    {
        charactorAnimation.enabled = false;
        foreach (Collider col in ragdollColliders)
        {
            col.enabled = true;

        }
        foreach (Rigidbody rigid in limbsRigidbodies)
        {
            rigid.isKinematic = false;

        }

        
        mainCollider.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }
    void RagdollModeOff()
    {
        foreach(Collider col in ragdollColliders)
        {
            col.enabled = false;

        }
        foreach(Rigidbody rigid in limbsRigidbodies)
        {
            rigid.isKinematic = true;

        }

        charactorAnimation.enabled = true;
        mainCollider.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
