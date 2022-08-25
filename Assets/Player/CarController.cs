using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

    public GameManager gm;

    public bool isGrounded;
    private float input;
    private float turnInput;
    public float fwdspeed;
    public float bwdspeed;
    public float turnspeed;
    public LayerMask groundLayer;
    public TrailRenderer[] trailRenderer;
    public ParticleSystem[] particle_dust;
    private bool is_drifting;
    private float lower_limit_speed=135f;
    private float upper_limit_speed=200f;
    private bool is_moving;
    private bool is_not_moving;
    public float mass=-0.9f;

    public AudioSource engine_noise;
    public AudioSource drifting_noise;

    private void Start()
    {
        transform.GetComponent<Rigidbody>().centerOfMass = new Vector3(0, mass, 0);
    }
    // Update is called once per frame
    void Update() {

        input = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");
        is_drifting = Input.GetKey(KeyCode.LeftShift);
        input *= input>0?fwdspeed:bwdspeed;
        if (isGrounded)
        {
            transform.Rotate(Vector3.up * turnInput * turnspeed * Time.deltaTime * Input.GetAxisRaw("Vertical"), Space.World);
            
        }

        fwdspeed = fwdspeed > upper_limit_speed ? upper_limit_speed : fwdspeed < lower_limit_speed ? lower_limit_speed : fwdspeed;
        turnspeed = turnspeed > upper_limit_speed ? upper_limit_speed : turnspeed < lower_limit_speed ? lower_limit_speed : turnspeed;

        is_moving = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) ;
        is_not_moving = Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow);
        foreach (ParticleSystem dust in particle_dust)
        {
            
            var main = dust.main;
            if (is_moving)
            {
                if (isGrounded)
                {
                    main.loop = true;
                    dust.Play();
                }

            }
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow)|| !isGrounded)
            {
                main.loop = false;
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
            drifting_noise.Play();
        }
        else
        {
            fwdspeed += 0.5f;
            turnspeed -= 0.5f;
            foreach (TrailRenderer trails in trailRenderer)
            {
                trails.emitting = false;
            }
            drifting_noise.Pause();
        }


        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position,Vector3.down, out hit, 0.25f,groundLayer);
        transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

        transform.GetComponent<Rigidbody>().drag = isGrounded ? 4f : 0.15f;


        if (is_moving)
        {
            engine_noise.Play();
        }else if (is_not_moving)
        {
            engine_noise.Pause();
        }
        
    }

    private void FixedUpdate()
    {
        if (isGrounded)
        {
            transform.GetComponent<Rigidbody>().AddForce(transform.forward * input, ForceMode.Acceleration);
        }
        else
        {
            transform.GetComponent<Rigidbody>().AddForce(transform.up * -9.81f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gm.PlayerIsDead();
            Destroy(gameObject);

        }
    }
}
