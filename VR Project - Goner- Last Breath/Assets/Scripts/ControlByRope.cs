using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ControlByRope : MonoBehaviour
{
    [SerializeField]
    Transform ropeFirstCharJoint;

    [SerializeField]
    AudioSource monsterScream, maleScream;

    [SerializeField]
    float speed;

    [HideInInspector]
    public bool isLookingAtDummy;

    private bool isRopeBreak, once;

    private void FixedUpdate()
    {
        if (ropeFirstCharJoint.position.y <= 2 && !isRopeBreak)
        {
            isRopeBreak = true;
            monsterScream.Play();
        }

        if (isLookingAtDummy && isRopeBreak)
        {
            if (!once)
            {
                once = true;
                maleScream.Play();
            }
            StartCoroutine(WaitAndSwitchScenes(1));
            transform.position += new Vector3(0, 0, speed * Time.deltaTime);
        }
    }

    IEnumerator WaitAndSwitchScenes(int sceneIndex)
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(sceneIndex);
    }
}
