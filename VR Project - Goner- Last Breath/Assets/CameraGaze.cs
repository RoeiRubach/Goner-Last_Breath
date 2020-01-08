using UnityEngine;

public class CameraGaze : MonoBehaviour
{
    public LayerMask layerMask;

    [SerializeField]
    private ControlByRope controlByRope;

    private void Update()
    {
        Ray myRay = new Ray(transform.position, transform.TransformDirection(Vector3.forward));

        RaycastHit myHit;
        if (Physics.Raycast(myRay, out myHit, Mathf.Infinity, layerMask))
        {
            controlByRope.isLookingAtDummy = true;
        }
        else
        {
            controlByRope.isLookingAtDummy = false;
        }

    }
}
