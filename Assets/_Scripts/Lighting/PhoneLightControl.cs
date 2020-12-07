using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneLightControl : MonoBehaviour
{

    public bool lightOn;
    private AudioSource phoneChime;
    public AudioClip phoneChimeSound;
    public GameObject phoneScreen;

    private void Start()
    {
        WindowManager.OnPhoneRing += SetPhoneBool;
        lightOn = false;
        phoneChime = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (lightOn == true)
            RingPhone();
    }

    void RingPhone ()
    {
        phoneScreen.SetActive(true);
        phoneChime.PlayOneShot(phoneChimeSound);
        Invoke("PhoneOff", 4.0f);
        lightOn = false;
    }

    void PhoneOff ()
    {
        phoneScreen.SetActive(false);
    }

    void SetPhoneBool(WindowManager window)
    {
        lightOn = true;
    }

    private void OnDestroy()
    {
        WindowManager.OnPhoneRing -= SetPhoneBool;
    }
}
