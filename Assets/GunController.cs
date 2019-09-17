using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZEffects;

public class GunController : MonoBehaviour {

    public GameObject controllerRight;

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;

    private SteamVR_TrackedController controller;

    public EffectTracer tracerEffect;
    public Transform muzzleTransform;

    //Sounds
    private AudioSource bulletSound;
    private AudioSource noBulletsLeft;
    private AudioSource reloadingSound;
    public AudioSource[] sounds;

    //Reload Controller Script
    private ReloadController reloadZoneScript;

    private int startingBullets = 10;
    private bool canReload = false;

    void Start () {

        //Get reload zone script
        reloadZoneScript = GameObject.FindWithTag("ReloadZone").GetComponent<ReloadController>();

        //Initialize gun sounds - audio source components in gun
        sounds = GetComponents<AudioSource>();
        bulletSound = sounds[0];
        noBulletsLeft = sounds[1];
        reloadingSound = sounds[2];

        //Get controller and trackedcontroller components
        controller = controllerRight.GetComponent<SteamVR_TrackedController>();
        controller.TriggerClicked += TriggerPressed;
        trackedObj = controllerRight.GetComponent<SteamVR_TrackedObject>();
	}

    private void TriggerPressed(object sender, ClickedEventArgs e)
    {
        if (canReload)
            Reload();
        else
            ShootWeapon();
    }

    private void ShootWeapon()
    {

        if(startingBullets > 0)
        {
            startingBullets--;

            RaycastHit hit = new RaycastHit();
            Ray ray = new Ray(muzzleTransform.position, muzzleTransform.forward);

            device = SteamVR_Controller.Input((int)trackedObj.index);
            device.TriggerHapticPulse(10000);
            tracerEffect.ShowTracerEffect(muzzleTransform.position, muzzleTransform.forward, 250f);

            bulletSound.Play();

            if (Physics.Raycast(ray, out hit, 5000f))
            {
                if (hit.collider.attachedRigidbody)
                {
                    Debug.Log("We've hit" + hit.collider.gameObject.name);
                }
            }
        } else
        {
            noBulletsLeft.Play();
        }

    }

    private void Reload()
    {
        startingBullets = 10;
        reloadingSound.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
        if(collision.collider.tag == "ReloadZone")
        {
            canReload = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "ReloadZone")
        {
            canReload = false;
        }
    }

    void Update () {
        canReload = reloadZoneScript.canReload;
	}
}
