using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private float speed = 0.02f;
    public float boostRecharge, boostDischarge, reduceSpeed = 2f;
    private float boostMultiplier = 0.2f, currentBoost, maxBoost, target;
    private Vector2 turn;
    public GameObject _planets, _camera;
    public Vector3 _playerPos;
    public Transform planetsRoot, enemyStation;
    private float resetDistance = 1500f, maxhealth, currentHealth, maxShield, currentShield, enemiesKilled;
    public LayerMask layerMask;
    private Vector3 spread = new Vector3(0.0006f, 0.0006f, 0.0006f);
    public ParticleSystem leftTurret, rightTurret, rightThrust, leftThrust, superSpeed;
    public Image wayPoint, healhBar, shieldBar, boostBar;
    public TextMeshProUGUI meter, enemyCounter;
    private bool isBoosting, needWayPoint, isGivenHealth;
    ParticleSystemRenderer rightPar, leftPar;
    public GameObject OutOfZonePanel;
    public bool isEarth;
    public AudioSource combatSound, ambianceSound, laserSound, spaceShipSound, enemyDeadSound;
    private int earthEnemies = 30;
    int numberOfEnemy;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        needWayPoint = true;
        target = 1f;
        maxhealth = 3;
        maxShield = 3;
        maxBoost = 10f;
        currentBoost = maxBoost;
        currentHealth = maxhealth;
        currentShield = maxShield;
        rightPar = rightThrust.GetComponent<ParticleSystemRenderer>();
        leftPar = leftThrust.GetComponent<ParticleSystemRenderer>();
        ambianceSound.Play();
    }

    private void Update()
    {   
        if (Time.timeScale == 0)
        {
            return;
        }

        if (currentShield == 0 && currentHealth == 0)
        {
            StartCoroutine(DeathScreen());
        }

        if ((!isGivenHealth && currentHealth < 3 ) && enemiesKilled >=6)
        {
            currentHealth = maxhealth;
            FillHealthShieldBar("health");
        }
        else if((!isGivenHealth && currentShield < 3) && enemiesKilled >= 6)
        {
            currentShield = maxShield;
            FillHealthShieldBar("shield");
        }

        numberOfEnemy = enemyStation.gameObject.GetComponent<EnemySpawner>().numberOfEnemies;
        Debug.Log("Enemies per Station: " + numberOfEnemy);
        if (isEarth)
        {
            enemyCounter.text =  earthEnemies - enemiesKilled  + " Enemies Left";
        }
        else
        {
            enemyCounter.text = numberOfEnemy - enemiesKilled + " Enemies Left";
        }

        if (needWayPoint)
        {
            ShowWayPoint();
        }
        else
        {
            wayPoint.gameObject.SetActive(false);
            CombatSound();
        }

        turn.x += Input.GetAxis("Mouse X");
        turn.y += Input.GetAxis("Mouse Y");

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, Mathf.Infinity))
            {
                Debug.DrawRay(_camera.transform.position, _camera.transform.forward * 1000f, Color.red);

                if (hit.transform.tag == "Enemy")
                {
                    hit.transform.GetComponent<EnemyMovCopier>().TakeDamage();
                }
                rightTurret.Play();
                leftTurret.Play();
                laserSound.Play();
            }
            else
            {
                Debug.DrawRay(_camera.transform.position, _camera.transform.forward * 1000f, Color.green);
                rightTurret.Play();
                leftTurret.Play();
            }
        }


    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isBoosting = true;
        }
        else
        {
            isBoosting = false;
        }
        if (Input.GetKey(KeyCode.W))
        {
            target = boostBar.fillAmount;
            if (isBoosting && target > 0.006f)
            {
                transform.position += transform.forward * boostMultiplier;
                rightPar.lengthScale = Mathf.Lerp(rightPar.lengthScale, -10, Time.deltaTime * 2);
                leftPar.lengthScale = Mathf.Lerp(leftPar.lengthScale, -10, Time.deltaTime * 2);
                boostBar.fillAmount -= boostDischarge;
                superSpeed.Play();
                Debug.Log("Boost");
                SpeceShipSound();
            }
            else if(!isBoosting)
            {
                transform.position += (transform.forward * speed);
                rightPar.lengthScale = Mathf.Lerp(rightPar.lengthScale, -3, Time.deltaTime * 2);
                leftPar.lengthScale = Mathf.Lerp(leftPar.lengthScale, -3, Time.deltaTime * 2);
                superSpeed.Stop();
                boostBar.fillAmount += boostRecharge;
                SpeceShipSound();
            }
            else
            {
                superSpeed.Stop();
            }
        }
        else 
        {

            rightPar.lengthScale = Mathf.Lerp(rightPar.lengthScale, 0, Time.deltaTime * 7);
            leftPar.lengthScale = Mathf.Lerp(leftPar.lengthScale, 0, Time.deltaTime * 7);
            boostBar.fillAmount += boostRecharge;


            superSpeed.Stop();
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += transform.forward * -speed;
        }
        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);


    }

    void LateUpdate()
    {
        if (transform.position.magnitude > resetDistance)
        {
            Vector3 offset = transform.position;
            planetsRoot.position -= offset;

            transform.position = Vector3.zero;
            Rigidbody rb = transform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.position = Vector3.zero;
            }
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = _camera.transform.forward;
        direction += new Vector3(Random.Range(-spread.x, spread.x), Random.Range(-spread.y, spread.y), Random.Range(-spread.z, spread.z));

        direction.Normalize();
        return direction;
    }

    private void ShowWayPoint()
    {
        float minX = wayPoint.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = wayPoint.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(enemyStation.position);

        if (screenPos.z < 0)
        {
            screenPos *= -1;
        }

        Vector2 pos = screenPos;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        wayPoint.transform.position = pos;

        meter.text = ((int)Vector3.Distance(enemyStation.position , transform.position)*10).ToString() + "m";

    }

    public void WayPointOff()
    {
        needWayPoint = false;
    }

    public void Damage()
    {
        if (currentShield > 0)
        {
            UIBarController("shield");
        }
        else if( currentShield <= 0)
        {
            UIBarController("health");
        }
    }

    private void UIBarController(string uiName)
    {
        if (uiName == "health")
        {
            currentHealth = currentHealth - 1;
            healhBar.fillAmount = currentHealth / maxhealth;
        }
        else if (uiName == "shield")
        {
            currentShield = currentShield - 1;
            shieldBar.fillAmount = currentShield / maxShield;
        }
        else if (uiName == "boost")
        {
            boostBar.fillAmount = currentBoost / maxBoost;
        }
    }

    private void FillHealthShieldBar(string uiName)
    {
        if (uiName == "health")
        {
            healhBar.fillAmount = currentHealth / maxhealth;
        }
        else if (uiName == "shield")
        {
            shieldBar.fillAmount = currentShield / maxShield;
        }
    }

    public void EnemyKilled()
    {
        enemyDeadSound.Play();
        enemiesKilled += 1f;
        if (!isEarth && enemiesKilled == enemyStation.gameObject.GetComponent<EnemySpawner>().numberOfEnemies)
        {
            StartCoroutine(LevelPassed());
        }
        else if (isEarth && enemiesKilled == 30)
        {
            StartCoroutine(WinLevel());
        }
    }

    private IEnumerator LevelPassed()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("Nextlevel");
    }

    private IEnumerator WinLevel()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("WinScreen");
    }

    private void SpeceShipSound()
    {
        if (!spaceShipSound.isPlaying)
        {
            spaceShipSound.Play();
        }
    }

    private void CombatSound()
    {
        if (!combatSound.isPlaying)
        {
            if (ambianceSound.isPlaying)
            {
                ambianceSound.Stop();
            }
            combatSound.Play();
        }
    }


    private IEnumerator DeathScreen()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("DeathScreen");
    }
}
