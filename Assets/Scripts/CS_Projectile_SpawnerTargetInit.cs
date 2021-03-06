﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Projectile_SpawnerTargetInit : MonoBehaviour
{

    public CS_Projectile_Movement projectile;
    public float projectileSpeed;

    public Transform spawnLocation;
    public Transform rotateObject;
    public float spawnRate = 0.0f;
    private float timer;

    private Transform target;
    private Transform parent;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        parent = GameObject.FindWithTag("Playing").transform;
        timer = Random.Range(0.0f, spawnRate);
    }

    void Update()
    {
            float angle = CS_Utils.PointToDegree(target.position - rotateObject.position);
            if (rotateObject)
            {
                rotateObject.rotation = Quaternion.Euler(0.0f, 0.0f, angle + 90);
            }
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                CS_Projectile_Movement i = Instantiate(projectile, spawnLocation.position, spawnLocation.rotation, parent);
                i.angle = angle;
                i.speed = projectileSpeed;
                timer = spawnRate;
            }
    }
}
