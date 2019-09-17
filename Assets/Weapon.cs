using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    private AudioSource audiosource;
    private SteamVR_TrackedController controller;

    [SerializeField]
    private GameObject muzzleFlashPrefab;

    [SerializeField]
    private Transform muzzlePoint;

    // Use this for initialization
    void Start () {
        audiosource = GetComponent<AudioSource>();
        controller = GetComponentInParent<SteamVR_TrackedController>();
        controller.TriggerClicked += FireWeapon;

        if (controller)
            Debug.Log("Initialized!");

	}
	
    private void FireWeapon(object sender, ClickedEventArgs e)
    {
        audiosource.Play();

        var muzzleFlash = Instantiate(muzzleFlashPrefab, muzzlePoint.position, muzzlePoint.rotation);
        Destroy(muzzleFlash.gameObject, 0.5f);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
