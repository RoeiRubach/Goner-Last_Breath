using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    [SerializeField] private AudioClip _windEffect2, _horrorAmbience, _electricity1, _electricity2;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        InvokeRepeating("InvokeWindEffect2", 4f, 13f);
        InvokeRepeating("InvokeElectricity1", 10f, 31f);
        InvokeRepeating("InvokeElectricity2", 6f, 19f);
        InvokeRepeating("InvokeHorrorAmbience", 2f, 70f);
    }

    private void InvokeHorrorAmbience()
    {
        _audioSource.PlayOneShot(_horrorAmbience);
    }

    private void InvokeElectricity1()
    {
        _audioSource.PlayOneShot(_electricity1);
    }

    private void InvokeElectricity2()
    {
        _audioSource.PlayOneShot(_electricity2);
    }

    private void InvokeWindEffect2()
    {
        _audioSource.PlayOneShot(_windEffect2);
    }
}
