using System.Collections;
using System.Collections.Generic;
using ULTanksZombies.Bullets;
using UnityEngine;
using UnityEngine.UI;

public enum GoingTo
{
    Upward, Downward, None
}

public class BossController : MonoBehaviour, IDamageable
{
    public GameObject tank;
    public GameObject bulletPrefab;
    public GameObject firepoints;
    public GameObject[] firePoints;
    public GameObject face;

    public GameObject turret;
    public float degreesPerSecond = 200;

    public Slider healthSlider;

    [Header("Shoot Type 1")]
    public float shootingDelay_t1 = 0.7f;
    private float currentShootingDelay_t1 = 0f;
    public float shootingSpeed_t1 = 10f;
    public int bulletAmount_t1 = 1;
    public float degreeChange_t1 = 10f;
    //public float bulletLifeTime = 8f;

    private float initDegree = 0f;

    [Header("Shoot Type 2")]
    public float shootingDelay_t2 = 0.7f;
    private float currentShootingDelay_t2 = 0f;
    public float shootingSpeed_t2 = 10f;

    //public float bulletLifeTime = 8f;

    [Header("STATS")]
    public float health = 100f;
    

    private bool typeOneActive = true;
    private float timeChangeShootType = 5f; //Tiempo para cambiar tipo de disparo

    GoingTo State;

    private void Start()
    {
        State = GoingTo.Downward;
    }

    private void Update()
    {
        SpinTurret();

        if (timeChangeShootType > 0f)
        {
            timeChangeShootType -= Time.deltaTime;
        }
        if(timeChangeShootType <= 0)
        {
            timeChangeShootType = 5f;
            typeOneActive = !typeOneActive;
        }

        if (typeOneActive)
        {
            if (currentShootingDelay_t1 > 0)
            {
                currentShootingDelay_t1 -= Time.deltaTime;
            }
            if (currentShootingDelay_t1 <= 0)
            {
                currentShootingDelay_t1 = shootingDelay_t1;
                StartCoroutine(Shoot());
            }
        }
        else
        {
            if (currentShootingDelay_t2 > 0)
            {
                currentShootingDelay_t2 -= Time.deltaTime;
            }
            if (currentShootingDelay_t2 <= 0)
            {
                currentShootingDelay_t2 = shootingDelay_t2;
                ShootTypeTwo();
            }
        }


        LookAtObject(tank);
        Floating();
        
        
    }

    void SpinTurret()
    {
        turret.transform.Rotate(new Vector3(0, degreesPerSecond, 0) * Time.deltaTime);
    }

    void Floating()
    {
        if (transform.position.y < -2f)
        {
            State = GoingTo.Upward;
        }
        else
        {
            if (State == GoingTo.Upward && transform.position.y >= -1.3f)
            {
                State = GoingTo.Downward;
            }
        }
        switch (State)
        {
            case (GoingTo.Downward):
                transform.Translate(Vector3.down * Time.deltaTime);
                break;
            case (GoingTo.Upward):
                transform.Translate(Vector3.up * Time.deltaTime);
                break;
            default:
                break;
        }
    }

    void LookAtObject(GameObject obj)
    {
        Vector3 targetPosition = new Vector3(obj.transform.position.x,
            face.transform.position.y,
            obj.transform.position.z);

        face.transform.LookAt(targetPosition);
    }

    IEnumerator Shoot()
    {
        for (int i = 0; i < firePoints.Length; i++)
        {
            GameObject newBullet = Instantiate(bulletPrefab, firePoints[i].transform.position, firePoints[i].transform.rotation);
            newBullet.GetComponent<BulletController>().Shoot(newBullet.GetComponent<BulletController>().settings.force);
            yield return new WaitForSeconds(shootingDelay_t1);
        }
        //foreach (Transform child in firepoints.transform)
        //{
        //    GameObject newBullet = Instantiate(bulletPrefab, child.position, child.rotation);
        //    newBullet.GetComponent<BulletController>().Shoot(newBullet.GetComponent<BulletController>().settings.force);
        //    StartCoroutine(Wait(0.1f));
        //    //Something(child.gameObject);
        //}
        //float newDegree = initDegree;
        //for (int i = 0; i < bulletAmount_t1; i++)
        //{
        //    GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        //    newBullet.transform.eulerAngles = new Vector3(0, newDegree, 0);
        //    newBullet.GetComponent<BulletController>().Shoot(30f);
        //    newDegree += 360 / bulletAmount_t1;
        //}
        //initDegree += degreeChange_t1;
    }
    
    
    void ShootTypeTwo()
    {
        for (int i = 0; i < firePoints.Length; i++)
        {
            GameObject newBullet = Instantiate(bulletPrefab, firePoints[i].transform.position, firePoints[i].transform.rotation);
            newBullet.GetComponent<BulletController>().Shoot(newBullet.GetComponent<BulletController>().settings.force);
        }
        //foreach (Transform child in firepoints.transform)
        //{
        //    GameObject newBullet = Instantiate(bulletPrefab, child.position, child.rotation);
        //    newBullet.GetComponent<BulletController>().Shoot(newBullet.GetComponent<BulletController>().settings.force);
        //    //Something(child.gameObject);
        //}
        //float newDegree = initDegree;
        //for (int i = 0; i < 8; i++)
        //{
        //    GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        //    newBullet.transform.eulerAngles = new Vector3(0, newDegree, 0);
        //    newBullet.GetComponent<BulletController>().Shoot(30f);
        //    newDegree += 360 / 8;
        //}
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        healthSlider.value = health / 100f;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
