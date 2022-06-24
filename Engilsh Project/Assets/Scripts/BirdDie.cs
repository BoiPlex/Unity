using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdDie : MonoBehaviour
{
    public GameObject bird;
    public GameObject ship;
    public Material newSkyBox;
    public GameObject dieEffect;
    public MeshRenderer oceanMesh;
    public Material newOcean;

    public AudioSource oldMusicSource;
    public AudioSource newMusicSource;

    private CameraMovement camScript;
    private Water oceanScript;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        camScript = GetComponent<CameraMovement>();
        oceanScript = GameObject.FindObjectOfType<Water>().GetComponent<Water>();
        newMusicSource.mute = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDead)
        {
            isDead = true;
            Instantiate(dieEffect, bird.transform.position, bird.transform.rotation);
            bird.GetComponent<RandomFlyer>().enabled = false;

            StartCoroutine(Dying(3f)); // Wait 3 seconds
        }
    }

    private IEnumerator Dying(float time)
    {
        yield return new WaitForSeconds(time);
        camScript.target = ship.transform;
        camScript.distanceToTarget = 75;
        Destroy(bird);
        oceanScript.waveFrequency = 0.2f;
        oceanScript.waveHeight = 0.5f;

        oldMusicSource.mute = true;
        newMusicSource.mute = false;

        RenderSettings.skybox = newSkyBox;
        RenderSettings.ambientSkyColor = new Color(224/255f, 102/255f, 92/255f);
        oceanMesh.material = newOcean;
    }
}