using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Transform _barrelLocation, _casingExitLocation, _magParentRef;
    [SerializeField] private AudioClip _shotHitEnemy, _shotHitConcrete, _shotHitMetal, _magDetach, _magAttach, _machinegunShoot;

    protected GameManager _gameManager;
    private AudioSource _audioSource;
    private Transform _leftHandRef;
    private GameObject _magazineCopyRef;
    private Camera _mainCameraRef;
    private float _nextTimeToFire = 0f;
    private bool _isAbleToSpawnNewMag;
    private Text _bulletCounter;

    protected virtual void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _mainCameraRef = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        _leftHandRef = _gameManager.GetLeftHandRef();
        _audioSource = GetComponent<AudioSource>();
        ovrGrabbable = GetComponent<OVRGrabbable>();
        _currentBulletAmount = _submachineMaxBullets;
        _bulletCounter = GetComponentInChildren<Text>();
        _bulletCounter.text = _currentBulletAmount.ToString();
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
                _audioSource.PlayOneShot(_magDetach);

                if (_currentBulletAmount != 0)
                {
                    _currentBulletAmount = 1;
                    _bulletCounter.text = _currentBulletAmount.ToString();
                }

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
            _audioSource.PlayOneShot(_machinegunShoot);

            OVRInput.SetControllerVibration(0.3f, 0.3f, ovrGrabbable.grabbedBy.GetController());
            Invoke("InvokeResetHaptic", 0.25f);

            RaycastOnTarget();

            GameObject _tempFlash;
            _tempFlash = Instantiate(_muzzleFlashPrefab, _barrelLocation.position, _barrelLocation.rotation);

            Destroy(_tempFlash, 0.5f);
            _currentBulletAmount--;
            _bulletCounter.text = _currentBulletAmount.ToString();

            DestroyBulletsIfNone();
        }
    }

    private void RaycastOnTarget()
    {
        RaycastHit hit;

        if (Physics.Raycast(_barrelLocation.transform.position, _barrelLocation.transform.forward, out hit, 100f))
        {
            print(hit.transform.name);
            if (hit.transform.CompareTag("Enemy"))
            {
                print("enemy been triggered");
                EnemyBase _currentEnemy = hit.transform.GetComponent<EnemyBase>();

                if (!_currentEnemy)
                    _currentEnemy = hit.transform.root.GetComponent<EnemyBase>();
                
                _currentEnemy.TakeDamage(_damage);
                Invoke("InvokeHitDetectionEnemy", 0.15f);
            }
            else if (hit.transform.CompareTag("Concrete"))
            {
                Invoke("InvokeHitDetectionConcrete", 0.15f);
            }
            else if (hit.transform.CompareTag("Metal"))
            {
                Invoke("InvokeHitDetectionMetal", 0.15f);
            }
        }
    }

    private void InvokeHitDetectionEnemy()
    {
        _audioSource.PlayOneShot(_shotHitEnemy);
    }

    private void InvokeHitDetectionConcrete()
    {
        _audioSource.PlayOneShot(_shotHitConcrete);
    }

    private void InvokeHitDetectionMetal()
    {
        _audioSource.PlayOneShot(_shotHitMetal);
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
        _magazineCopyRef.GetComponent<Rigidbody>().drag = 55f;
        _isAbleToSpawnNewMag = false;
    }

    public bool AttachMagazineToItsPlace()
    {
        _audioSource.PlayOneShot(_magAttach);

        _magazineCopyRef.GetComponent<OVRGrabbable>().grabbedBy.ForceRelease(_magazineCopyRef.GetComponent<OVRGrabbable>());
        _magazineCopyRef.transform.parent = _magParentRef;
        _magazineCopyRef.transform.localPosition = _magazineRef.transform.localPosition;
        _magazineCopyRef.transform.localRotation = _magazineRef.transform.localRotation;
        _currentBulletAmount = _submachineMaxBullets;
        _bulletCounter.text = _currentBulletAmount.ToString();
        _magazineCopyRef.GetComponent<BoxCollider>().enabled = false;
        _magazineCopyRef.GetComponent<Rigidbody>().isKinematic = true;
        _magazineCopyRef.GetComponent<Rigidbody>().drag = 0f;
        return true;
    }

    private void InvokeResetHaptic()
    {
        OVRInput.SetControllerVibration(0f, 0f, ovrGrabbable.grabbedBy.GetController());
    }
}
