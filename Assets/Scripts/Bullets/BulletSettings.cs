using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullets/Settings")]
public class BulletSettings : ScriptableObject
{
    public string bulletName;
    public float damage;
    public float force;
    public ParticleSystem particle;
}
