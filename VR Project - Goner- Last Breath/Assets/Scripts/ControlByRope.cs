using UnityEngine;

public class ControlByRope : MonoBehaviour
{
    [SerializeField]
    private Transform _ropeFirstCharJoint;

    [SerializeField]
    private AudioSource _monsterScream;
    private AudioSource _maleScream;

    [SerializeField]
    float speed;

    [HideInInspector]
    public bool isLookingAtDummy;

    private bool isMaleScreamPlayed, isRopeBreak;

    private void Start()
    {
        _maleScream = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (_ropeFirstCharJoint.position.y <= 2 && !isRopeBreak)
        {
            isRopeBreak = true;
            _monsterScream.Play();
        }

        if (isLookingAtDummy && isRopeBreak && !_monsterScream.isPlaying)
        {
            if (!isMaleScreamPlayed)
            {
                isMaleScreamPlayed = true;
                _maleScream.Play();
                SceneController.LoadScene(1, 3, 3f);
            }

            transform.position += new Vector3(0, 0, speed * Time.deltaTime);
        }
    }
}
