using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackEvents : MonoBehaviour
{
    public EnemyWeapon weapon;

    public void StartAttack()
    {
        weapon.StartAttack();
    }

    public void EndAttack()
    {
        weapon.EndAttack();
    }
}