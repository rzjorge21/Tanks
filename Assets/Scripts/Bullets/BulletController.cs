using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ULTanksZombies.Bullets
{
    [RequireComponent(typeof(Rigidbody))]
    public class BulletController : MonoBehaviour
    {
        //public float force;
        public BulletSettings settings;

        private float lifetime = 1.5f;

        private void Awake()
        {
            Destroy(gameObject, lifetime);
        }

        public void Shoot(float force)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * force, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision collision)
        {
            IDamageable damageObj = collision.gameObject.GetComponent<IDamageable>();
            if (damageObj != null)
            {
                damageObj.TakeDamage(settings.damage);
                Instantiate(settings.particle, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            //if (collision.gameObject.CompareTag("Bullet"))
            //{
            //    Physics.IgnoreCollision(collision.collider, GetComponent<SphereCollider>());
            //}
            if (!collision.gameObject.CompareTag("Bullet"))
            {
                Destroy(gameObject);
            }
            //if (!collision.gameObject.CompareTag("Boss") && !collision.gameObject.CompareTag("Bullet"))
            //{
            //    Destroy(gameObject);
            //}
        }

    }
}
