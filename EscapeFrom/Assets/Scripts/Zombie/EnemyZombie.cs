﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZombie : EnemyMelee
{
    public GameObject meleeAttackArea;


    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        attackCoolTime = 2f;
        attackCoolTimeCacl = attackCoolTime;

        attackRange = 2.5f;
        m_nav.stoppingDistance = 1f;
        StartCoroutine(ResetAttackArea());
    }

    IEnumerator ResetAttackArea()
    {
        while (true)
        {
            yield return null;
            if(!meleeAttackArea.activeInHierarchy && currentState == State.ATTACK)
            {
                yield return new WaitForSeconds(attackCoolTime);
                meleeAttackArea.SetActive(true);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
   
}
