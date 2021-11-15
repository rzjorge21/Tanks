using System.Collections;
using System.Collections.Generic;
using ULTanksZombies.Bullets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ULTanksZombies.Tank
{
    [RequireComponent(typeof(Rigidbody))]
    public class TankController : MonoBehaviour, IDamageable
    {
        public float speed;
        public float rotationSpeed;
        public GameObject bulletPrefab;
        public float fireRate;

        private TankStateMachine fsm;
        private Transform firePoint;
        private Vector3 initialPosition;
        //private IdleState idleState;
        private MoveState moveState;

        public GameObject[] bullets;
        private int currentBulletIndex = 0;
        public Text bulletInfo;

        [Header("STATS")]
        public float health = 100f;

        [Header("UI")]
        public Slider healthSlider;
        public GameObject bossHealth;

        private void Start()
        {
            fsm = new TankStateMachine();
            moveState = new MoveState(this, fsm);

            fsm.Start(moveState);

            firePoint = transform.GetChild(0).GetChild(0).transform;
            initialPosition = transform.position;
        }

        private void Update()
        {
            fsm.CurrentState.OnHandleInput();
            fsm.CurrentState.OnLogicUpdate();
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ChangeBullet();
            }
        }

        private void FixedUpdate()
        {
            fsm.CurrentState.OnPhysicsUpdate();
        }

        public void ChangeBullet()
        {
            currentBulletIndex++;
            if (currentBulletIndex >= bullets.Length)
            {
                currentBulletIndex = 0;
            }
            bulletInfo.text = bullets[currentBulletIndex].GetComponent<BulletController>().settings.bulletName;
        }

        public void Fire()
        {
            GameObject ob = Instantiate(bullets[currentBulletIndex], firePoint.position, firePoint.rotation);
            ob.GetComponent<BulletController>().Shoot(bullets[currentBulletIndex].GetComponent<BulletController>().settings.force);
        }

        public void TakeDamage(float amount)
        {
            health -= amount;
            healthSlider.value = health / 100f;
            if (health <= 0)
            {
                health = 100f;
                healthSlider.value = health / 100f;
                bossHealth.SetActive(false);
                transform.position = initialPosition;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Boss"))
            {
                bossHealth.SetActive(true);
            }
        }
    }

}
