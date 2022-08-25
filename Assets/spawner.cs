using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    private int wave = 0;
    public float spawnInterval;
    public GameManager gm;
    [SerializeField]
    private GameObject enemyCar;
    // Start is called before the first frame update

    private void Start()
    {
        StartCoroutine(SpawnEnemy(spawnInterval, enemyCar));
    }
    // Update is called once per frame
    void Update()
    {
        
        spawnInterval -= 0.1f * wave;
        if (spawnInterval < 1)
        {
            spawnInterval = 1f;
        }
    }

    private IEnumerator SpawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-40f,150f),0.4f, Random.Range(-80f, -135f)),Quaternion.identity);
        wave += 1;
        if (!gm.GetPlayerIsDead())
        {
            StartCoroutine(SpawnEnemy(interval, enemy));
        }
        
    }
}
