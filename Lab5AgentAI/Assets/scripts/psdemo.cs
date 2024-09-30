using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psdemo : MonoBehaviour
{
    // Parameters for the Poisson Disc Sampling
    public float width = 10f;
    public float height = 10f;
    public float radius = 1f;
    public GameObject spherePrefab;
    private List<Vector2> samples;

    void Start()
    {
        PoissonDiscSampler sampler = new PoissonDiscSampler(width, height, radius);

        // Store all the generated samples in a list
        samples = new List<Vector2>();
        foreach (var sample in sampler.Samples())
        {
            samples.Add(sample);
            if (spherePrefab != null)
            {
                Vector3 spawnPosition = new Vector3(sample.x, 0f, sample.y);
                Instantiate(spherePrefab, spawnPosition, Quaternion.identity);
            }
        }
    }

    // Optional: Draw the samples using Gizmos
    private void OnDrawGizmos()
    {
        if (samples == null) return;

        Gizmos.color = Color.red;
        foreach (Vector2 sample in samples)
        {
            // Draw small spheres at each sample position
            Vector3 pos = new Vector3(sample.x, 0f, sample.y);
            Gizmos.DrawSphere(pos, 0.2f);
        }
    }
}
