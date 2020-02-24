using UnityEngine;

public class LightController : MonoBehaviour
{
    private Light[] _lights;
    [SerializeField] private float _durationDelay;

    // Start is called before the first frame update
    void Start()
    {
        _lights = GetComponentsInChildren<Light>();

        if (_lights[0].GetComponent<Light>().isActiveAndEnabled)
            Invoke("InvokeTurnOffLights", _durationDelay);
        else
            Invoke("InvokeTurnOnLights", _durationDelay);
    }

    private void InvokeTurnOnLights()
    {
        for (int i = 0; i < _lights.Length; i++)
        {
            _lights[i].GetComponent<Light>().enabled = true;
        }
        Invoke("InvokeTurnOffLights", _durationDelay);
    }

    private void InvokeTurnOffLights()
    {
        for (int i = 0; i < _lights.Length; i++)
        {
            _lights[i].GetComponent<Light>().enabled = false;
        }
        Invoke("InvokeTurnOnLights", _durationDelay);
    }
}
