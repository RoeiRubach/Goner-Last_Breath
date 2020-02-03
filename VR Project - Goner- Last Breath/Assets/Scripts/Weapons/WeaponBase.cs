using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    protected const int _submachineMaxBullets = 32;
    protected int _currentBulletAmount = 0;
    [SerializeField] protected float _submachineFireRate = 10f, _damage;

    protected OVRInput.Button shootingButton = OVRInput.Button.PrimaryIndexTrigger;
    protected OVRGrabbable ovrGrabbable;
    protected Animator animator;

    [SerializeField] private GameObject _casingPrefab, _muzzleFlashPrefab;
    [SerializeField] private Transform _barrelLocation, _casingExitLocation;

    private Camera _mainCameraRef;
    private float _nextTimeToFire = 0f;

    //protected abstract void Reload();

    protected virtual void Start()
    {
        _mainCameraRef = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        ovrGrabbable = GetComponent<OVRGrabbable>();
        _currentBulletAmount = _submachineMaxBullets;
    }

    protected virtual void Update()
    {
        if (ovrGrabbable.isGrabbed && OVRInput.GetDown(shootingButton, ovrGrabbable.grabbedBy.GetController()))
        {
            if (Time.time >= _nextTimeToFire)
            {
                _nextTimeToFire = Time.time + 1f / _submachineFireRate;
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        if (_currentBulletAmount != 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(_mainCameraRef.transform.position, _mainCameraRef.transform.forward, out hit))
            {
                EnemyBase _currentEnemy = hit.transform.GetComponent<EnemyBase>();

                if (_currentEnemy)
                    _currentEnemy.TakeDamage(_damage);
            }

            GameObject _tempFlash;
            _tempFlash = Instantiate(_muzzleFlashPrefab, _barrelLocation.position, _barrelLocation.rotation);

            Destroy(_tempFlash, 0.5f);
            _currentBulletAmount--;
        }
    }

    private void CasingRelease()
    {
        GameObject _casing;
        _casing = Instantiate(_casingPrefab, _casingExitLocation.position,
                                _casingExitLocation.rotation) as GameObject;
        _casing.GetComponent<Rigidbody>().AddExplosionForce(550f, (_casingExitLocation.position - _casingExitLocation.right * 0.3f - _casingExitLocation.up * 0.6f), 1f);
        _casing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f),
                                                    Random.Range(10f, 1000f)), ForceMode.Impulse);
    }
}
