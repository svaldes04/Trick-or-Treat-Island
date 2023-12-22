using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public float spawnTime;
    private bool playing = true;
    public GameObject enemyPrefab;
    public int maxEnemies;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }
    
    IEnumerator Spawn()
    {
        while (playing) {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length <= maxEnemies)
            {
                Vector3 position = Vector3.zero;
                position.x = Random.Range(-20, 19);
                position.y = Random.Range(-19, 20);

                Instantiate(enemyPrefab, position, Quaternion.identity);
                yield return new WaitForSeconds(spawnTime);
            }
            else {
                yield return new WaitForSeconds(spawnTime);
            }
        }
    }
}
