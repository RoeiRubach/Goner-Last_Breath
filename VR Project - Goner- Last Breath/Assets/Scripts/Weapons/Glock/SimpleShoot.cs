﻿using UnityEngine;

public class SimpleShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;
    public Transform barrelLocation;
    public Transform casingExitLocation;

    [SerializeField] private float _shotPower;
    [SerializeField] private AudioClip _glockShoot;
    private OVRGrabbable ovrGrabbable;

    private AudioSource _audioSource;

    public float _bulletDamage { get; private set; }

    void Start()
    {
        ovrGrabbable = GetComponent<OVRGrabbable>();
        _audioSource = GetComponent<AudioSource>();

        _bulletDamage = 1f;

        if (barrelLocation == null)
            barrelLocation = transform;
    }

    public void TriggerShoot()
    {
        OVRInput.SetControllerVibration(0.5f, 0.6f, ovrGrabbable.grabbedBy.GetController());
        Invoke("InvokeResetHaptic", 0.6f);

        GetComponent<Animator>().SetTrigger("Fire");
    }

    void Shoot()
    {
        //  GameObject bullet;
        //  bullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        // bullet.GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);

        GameObject tempFlash;
        Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation).GetComponent<Rigidbody>().AddForce(barrelLocation.forward * _shotPower);
        tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

        _audioSource.PlayOneShot(_glockShoot);

        Destroy(tempFlash, 0.5f);
        //  Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation).GetComponent<Rigidbody>().AddForce(casingExitLocation.right * 100f);

    }

    void CasingRelease()
    {
        GameObject casing;
        casing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        casing.GetComponent<Rigidbody>().AddExplosionForce(550f, (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        casing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(10f, 1000f)), ForceMode.Impulse);
    }

    private void InvokeResetHaptic()
    {
        OVRInput.SetControllerVibration(0f, 0f, ovrGrabbable.grabbedBy.GetController());
    }
}
