
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracking : MonoBehaviour
{
    public GameObject player; 
    public GameObject enemy;
  
    public float orbitSpeed = 1.0f; 
    private float rotateSpeed = 66f;

     public int numberOfEnemies = 1;
    public float spawnRangeMin;  
    public float spawnRangeMax;
    private List<Transform> enemies = new List<Transform>();

    void Start() 
    {
        EnemySpawn();   
    } 

    void Update()
    {
        foreach (Transform enemy in enemies)
        {
            OrbitToPlayer(enemy);  
        }
    }

  private void OrbitToPlayer(Transform Enemy)
{ 
   // Calculate direction from the enemy to the player
    Vector2 directionToPlayer =  (Vector2)Enemy.position - (Vector2)player.transform.position;
    float distanceToPlayer = directionToPlayer.magnitude;
float adjustedOrbitSpeed = orbitSpeed * distanceToPlayer;
directionToPlayer.Normalize();

    // Rotate around the player
    Enemy.RotateAround(player.transform.position, Vector3.forward, adjustedOrbitSpeed * Time.deltaTime);

    // Calculate the direction to face the player
    Vector2 directionToFace = (Vector2)player.transform.position - (Vector2)Enemy.position;
    float angleToPlayer = Mathf.Atan2(directionToFace.y, directionToFace.x) * Mathf.Rad2Deg;

    // Smoothly rotate towards the player
    Quaternion targetRotation = Quaternion.Euler(0, 0, angleToPlayer + 180f); // Adjust if needed
    Enemy.rotation = Quaternion.Slerp(Enemy.rotation, targetRotation, rotateSpeed * Time.deltaTime); 
}

 private void EnemySpawn()
  {
    enemies.Clear();
    for (int i = 0; i < numberOfEnemies; i++)
        {
            
        Vector2 randomDirection = Random.insideUnitCircle.normalized; // Ensure the direction is normalized
        float randomDistance = Random.Range(spawnRangeMin, spawnRangeMax); // Distance between min and max

        Vector2 spawnPosition = (Vector2)player.transform.position + randomDirection * randomDistance;
        GameObject newEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
        enemies.Add(newEnemy.transform);
        }
  } 
}
