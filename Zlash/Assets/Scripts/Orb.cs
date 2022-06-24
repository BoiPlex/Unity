using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Orb : MonoBehaviour
{
    public float ballSpeed;

    private Rigidbody2D body;

    public GameObject orbEffect;

    public bool orbBounce = false;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        body.velocity = new Vector2(ballSpeed * transform.localScale.x, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        string[] PowerupTags = {"PowerupHealth", "PowerupJump", "PowerupMana", "PowerupOrb", "PowerupSpeed"};
        if (!PowerupTags.Contains(other.gameObject.tag))
        {
            Instantiate(orbEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    public void SetOrbBounce(bool isBounce)
    {
        orbBounce = isBounce;
    }
}
