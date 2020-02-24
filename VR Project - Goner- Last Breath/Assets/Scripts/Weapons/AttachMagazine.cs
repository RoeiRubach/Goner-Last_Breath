using UnityEngine;

public class AttachMagazine : MonoBehaviour
{
    [SerializeField] private WeaponBase _weaponBase;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("MagazinePlacement"))
        {
            if (other.GetComponentInParent<WeaponBase>().AttachMagazineToItsPlace())
            {
                Destroy(GetComponent<AttachMagazine>());
            }
        }
    }
}
