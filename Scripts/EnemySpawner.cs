using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    private bool _isActive = false;
    [SerializeField]
    private GameObject _enemy;
    public int numberOfEnemies; 
    [SerializeField]
    private float spawnRadius = 1.5f;
    [SerializeField]
    private float spawnInterval = 0.5f;

    private Transform navMeshPlane;
    private Transform planetsParent;


    private void Start()
    {
        GameObject planetsObj = GameObject.Find("Planets");
        if (planetsObj != null)
        {
            planetsParent = planetsObj.transform;
        }
        else
        {
            Debug.LogError("Planets object not found in scene!");
        }

        navMeshPlane = GameObject.Find("NavMeshSurface")?.transform;
        if (navMeshPlane == null)
        {
            Debug.LogError("NavMeshPlane object not found! Make sure it exists and is named correctly.");
        }


        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneIndex == 1)
        {
            numberOfEnemies = 10;
        }
        else if (sceneIndex >= 2 && sceneIndex <= 6)
        {
            numberOfEnemies = 10 + (sceneIndex - 1) * 2;
        }
        else if (sceneIndex == 7)
        {
            numberOfEnemies = 10;
        }


    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_isActive)
            {
                _isActive = true;
                StartCoroutine(SpawnEnemies());
                other.GetComponent<Player>().WayPointOff();
            }
        }
    }

    private IEnumerator SpawnEnemies()
    {
        int spawned = 0;
        int maxTries = 5;

        while (spawned < numberOfEnemies)
        {
            int tries = 0;
            bool foundPosition = false;
            Vector3 spawnPos = Vector3.zero;

            while (tries < maxTries && !foundPosition)
            {
                Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;

                Vector3 randomPos = new Vector3(
                    transform.position.x + randomCircle.x,
                    navMeshPlane != null ? navMeshPlane.position.y : transform.position.y,
                    transform.position.z + randomCircle.y
                );

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPos, out hit, 10.0f, NavMesh.AllAreas))
                {
                    spawnPos = hit.position;
                    foundPosition = true;
                }
                tries++;
            }

            if (foundPosition)
            {
                GameObject newEnemy = Instantiate(_enemy, spawnPos, Quaternion.identity);

                if (planetsParent != null)
                {
                    newEnemy.transform.SetParent(planetsParent);
                }

                spawned++;
                Debug.Log("Enemy spawned at: " + spawnPos);
            }
            else
            {
                Debug.LogWarning("Failed to find a valid spawn position after several tries.");
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
