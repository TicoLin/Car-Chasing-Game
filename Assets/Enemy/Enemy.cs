using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public bool isGrounded;
    public float fwdspeed;
    public float bwdspeed;
    public float turnspeed;
    public LayerMask groundLayer;
    public TrailRenderer[] trailRenderer;
    public ParticleSystem[] particle_dust;
    public float mass = -0.9f;

    private float rotation;

    private void Start()
    {
        transform.GetComponent<Rigidbody>().centerOfMass = new Vector3(0, mass, 0);
        player = GameObject.FindWithTag("Player");

    }
    // Update is called once per frame
    void Update()
    {

        /*input = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");
        is_drifting = Input.GetKey(KeyCode.LeftShift);
        input *= input > 0 ? fwdspeed : bwdspeed;
        if (true)
        {
            transform.Rotate(Vector3.up * turnInput * turnspeed * Time.deltaTime * Input.GetAxisRaw("Vertical"), Space.World);

        }

        fwdspeed = fwdspeed > upper_limit_speed ? upper_limit_speed : fwdspeed < lower_limit_speed ? lower_limit_speed : fwdspeed;
        turnspeed = turnspeed > upper_limit_speed ? upper_limit_speed : turnspeed < lower_limit_speed ? lower_limit_speed : turnspeed;

        is_moving = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow);
        foreach (ParticleSystem dust in particle_dust)
        {

            var main = dust.main;
            if (true)
            {
                if (isGrounded)
                {
                    main.loop = true;
                    dust.Play();
                }

            }

        }


        if (is_drifting && isGrounded)
        {
            fwdspeed -= 0.5f;
            turnspeed += 0.5f;
            foreach (TrailRenderer trails in trailRenderer)
            {
                trails.emitting = true;
            }
        }
        else
        {
            fwdspeed += 0.5f;
            turnspeed -= 0.5f;
            foreach (TrailRenderer trails in trailRenderer)
            {
                trails.emitting = false;
            }
        }*/


        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, 0.25f, groundLayer);
        transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

        transform.GetComponent<Rigidbody>().drag = isGrounded ? 4f : 0.1f;
        if (player != null)
        {
            Vector3 target_direction = transform.position - player.transform.position;
            target_direction.Normalize();

            rotation = Vector3.Cross(target_direction, transform.forward).y;
        }
        
    }

    private void FixedUpdate()
    {
        /*if (isGrounded)
        {
            transform.GetComponent<Rigidbody>().AddForce(transform.forward * input, ForceMode.Acceleration);
        }
        else
        {
            transform.GetComponent<Rigidbody>().AddForce(transform.up * -9.81f);
        }*/
        if (isGrounded)
        {
            transform.GetComponent<Rigidbody>().angularVelocity = turnspeed * rotation * new Vector3(0, 1, 0);
            transform.GetComponent<Rigidbody>().velocity = transform.forward * fwdspeed;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
