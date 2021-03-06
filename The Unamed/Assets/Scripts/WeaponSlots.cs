using UnityEngine;
using zheavy.Combat;

public class WeaponSlots : MonoBehaviour
{
    [SerializeField] WeaponConfig[] weapon;

    public WeaponConfig SwitchWeapon(int index)
    {
        return weapon[index];
    }
}
