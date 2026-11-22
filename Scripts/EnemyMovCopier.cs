using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMovCopier : MonoBehaviour
{
    public GameObject enemyShadow;
    private Vector3 enemyPosFixer, playerPos, spread, firePointPos;
    private GameObject player;
    private Transform playerTrans;
    private Transform planetsParent;
    public Transform firePoint;
    public ParticleSystem particleEffect, explosionEffect;
    private bool isAttacking;
    public Image healthBar;
    private float maxHealth, currentHealth;
    private Player playerScript;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.transform.GetComponent<Player>();
        GameObject planetsObj = GameObject.Find("Planets");
        if (planetsObj != null)
        {
            planetsParent = planetsObj.transform;
        }
        else
        {
            Debug.LogError("Planets object not found in scene!");
        }
        spread = new Vector3(0.006f, 0.006f, 0.006f);
        maxHealth = 3f;
        currentHealth = maxHealth;
    }

    void Update()
    {
        playerTrans = player.transform;
        playerPos = playerTrans.position;
        enemyPosFixer = enemyShadow.transform.position;
        transform.position = new Vector3(enemyPosFixer.x, transform.position.y, enemyPosFixer.z);
        transform.LookAt(playerTrans);

        float yDifference = playerPos.y - transform.position.y;

        if (Mathf.Abs(yDifference) > 0.01f)
        {
            float direction = Mathf.Sign(yDifference);
            transform.position += new Vector3(0, direction * 0.4f * Time.deltaTime, 0);
        }
        float distance = Vector3.Distance(transform.position, playerPos);

        
        if (distance >= 3f && !isAttacking)
        {
            StartCoroutine(Shoot());
            
        }
    }

    public void Explode()
    {
        ParticleSystem explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(explosion.gameObject, explosion.main.duration);
        Destroy(transform.parent.gameObject);
    }

    public IEnumerator Shoot()
    {
        isAttacking = true;
        shoot();
        yield return new WaitForSeconds(2f);
        isAttacking = false;
        
    }

    void shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, GetDirection(), out hit, 100f))
        {
            if (hit.transform.tag == "Player")
            {
                hit.transform.GetComponent<Player>().Damage();
            }
        }
        else
        {
            Debug.DrawRay(firePoint.position, GetDirection(), Color.red, 100f);
        }
            
        particleEffect.Play();
    }


    private Vector3 GetDirection()
    {
        firePointPos = firePoint.forward;
        firePointPos += new Vector3(Random.Range(-spread.x, +spread.x), Random.Range(-spread.y, +spread.y), Random.Range(-spread.z, +spread.z));
        return firePointPos;
    }


    public void TakeDamage()
    {
        currentHealth = currentHealth - 1 ;

        healthBar.fillAmount = currentHealth / maxHealth;
        if (currentHealth <= 0)
        {
            Explode();
            playerScript.EnemyKilled();
        }

    }
}
