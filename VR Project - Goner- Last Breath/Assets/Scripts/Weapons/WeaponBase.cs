using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    private const int _submachineMaxBullets = 32;

    protected int _currentBulletAmount = 0;
    [SerializeField] protected float _submachineFireRate = 10f, _damage;

    protected OVRInput.Button shootingButton = OVRInput.Button.PrimaryIndexTrigger;
    protected OVRInput.Button DetachMagButton = OVRInput.Button.One;
    protected OVRInput.Button SpawnMagButton = OVRInput.Button.Three;
    protected OVRGrabbable ovrGrabbable;
    protected Animator animator;

    [SerializeField] private GameObject _casingPrefab, _muzzleFlashPrefab, _magazineRef;
    [SerializeField] private Transform _barrelLocation, _casingExitLocation, _magParentRef, _leftHandRef;

    private GameObject _magazineCopyRef;
    private Camera _mainCameraRef;
    private float _nextTimeToFire = 0f;
    private bool _isAbleToSpawnNewMag;

    protected virtual void Start()
    {
        _mainCameraRef = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        ovrGrabbable = GetComponent<OVRGrabbable>();
        _currentBulletAmount = _submachineMaxBullets;
        _magazineCopyRef = Instantiate(_magazineRef, _magParentRef);
        _magazineCopyRef.transform.localScale = Vector3.one;
    }

    protected virtual void Update()
    {
        if (ovrGrabbable.isGrabbed && OVRInput.Get(shootingButton, ovrGrabbable.grabbedBy.GetController()))
        {
            if (Time.time >= _nextTimeToFire)
            {
                _nextTimeToFire = Time.time + 1f / _submachineFireRate;
                Shoot();
            }
        }

        if (ovrGrabbable.isGrabbed && OVRInput.GetDown(DetachMagButton, ovrGrabbable.grabbedBy.GetController()))
            DetachMagazine();

        if (_isAbleToSpawnNewMag && OVRInput.GetDown(SpawnMagButton))
            InstantiateNewMag();
    }

    protected virtual void DetachMagazine()
    {
        if (_magazineCopyRef)
        {
            if (_magazineCopyRef.transform.parent)
            {
                if (_currentBulletAmount != 0)
                    _currentBulletAmount = 1;

                _magazineCopyRef.transform.parent = null;
                _magazineCopyRef.GetComponent<BoxCollider>().enabled = true;
                _magazineCopyRef.GetComponent<Rigidbody>().isKinematic = false;

                _isAbleToSpawnNewMag = true;

                Destroy(_magazineCopyRef, 3f);
            }
        }
    }

    private void Shoot()
    {
        if (_currentBulletAmount != 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(_barrelLocation.transform.position, _barrelLocation.transform.forward, out hit, 100f))
            {
                print(hit.transform.name);
                EnemyBase _currentEnemy = hit.transform.GetComponent<EnemyBase>();

                if (_currentEnemy)
                    _currentEnemy.TakeDamage(_damage);
            }

            GameObject _tempFlash;
            _tempFlash = Instantiate(_muzzleFlashPrefab, _barrelLocation.position, _barrelLocation.rotation);

            Destroy(_tempFlash, 0.5f);
            _currentBulletAmount--;

            DestroyBulletsIfNone();
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

    private void DestroyBulletsIfNone()
    {
        if (_currentBulletAmount <= 0)
        {
            foreach (Transform children in _magazineCopyRef.GetComponentInChildren<Transform>())
            {
                if (children.GetComponent<SkinnedMeshRenderer>())
                    return;

                Destroy(children.gameObject);
            }
        }
    }

    private void InstantiateNewMag()
    {
        _magazineCopyRef = Instantiate(_magazineRef, _leftHandRef.position, _leftHandRef.rotation);
        _magazineCopyRef.AddComponent<AttachMagazine>();
        _magazineCopyRef.GetComponent<BoxCollider>().enabled = true;
        _magazineCopyRef.GetComponent<Rigidbody>().isKinematic = false;
        _magazineCopyRef.GetComponent<Rigidbody>().drag = 25f;
        _isAbleToSpawnNewMag = false;
    }

    public bool AttachMagazineToItsPlace()
    {
        _magazineCopyRef.transform.parent = _magParentRef;
        _magazineCopyRef.transform.localPosition = _magazineRef.transform.localPosition;
        _magazineCopyRef.transform.localRotation = _magazineRef.transform.localRotation;
        _currentBulletAmount = _submachineMaxBullets;
        _magazineCopyRef.GetComponent<BoxCollider>().enabled = false;
        _magazineCopyRef.GetComponent<Rigidbody>().isKinematic = true;
        _magazineCopyRef.GetComponent<Rigidbody>().drag = 0f;
        return true;
    }
}
