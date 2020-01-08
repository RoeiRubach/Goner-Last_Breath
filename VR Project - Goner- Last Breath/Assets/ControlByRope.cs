using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ControlByRope : MonoBehaviour
{
    [SerializeField]
    Transform ropeFirstCharJoint;

    [SerializeField]
    Transform ovrCamera;

    [SerializeField]
    float speed;

    [HideInInspector]
    public bool isLookingAtDummy;

    private void FixedUpdate()
    {
        if (ropeFirstCharJoint.position.y <= 2 && !isLookingAtDummy)
        {
            isLookingAtDummy = true;
            StartCoroutine(WaitAndSwitchScenes(1));
        }
        if (isLookingAtDummy)
        {
            ovrCamera.LookAt(transform);
            transform.position += new Vector3(0, 0, ropeFirstCharJoint.position.y * speed * Time.deltaTime);
        }
    }

    IEnumerator WaitAndSwitchScenes(int sceneIndex)
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(sceneIndex);
    }
}
