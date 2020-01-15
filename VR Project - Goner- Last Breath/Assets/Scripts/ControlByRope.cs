using UnityEngine;
using System.Collections;

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
        if (ropeFirstCharJoint.position.y <= 2)
        {
            if (isLookingAtDummy)
            {
                //ovrCamera.LookAt(transform);
                SceneController.LoadScene(1, 1, 3f);
                transform.position += new Vector3(0, 0, ropeFirstCharJoint.position.y * speed * Time.deltaTime);
            }
        }
    }
}
