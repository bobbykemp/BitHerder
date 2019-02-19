using System.Collections;
//using System;
using System.Collections.Generic;
using UnityEngine;

//https://en.wikipedia.org/wiki/Boids
//https://en.wikipedia.org/wiki/Flocking_(behavior)

public class Animal : MonoBehaviour {

    //the ninth layer represents the layer that the animals are on
    //this constant is used to prevent collisions between animal rigidbodies
    private const int ANIMAL_LAYER = 9;

    public float maxSpeed = 5f;
    public float fleeing_proximity = 8; //closest radius at which an animal will flee from user's cursor
    public float flee_distance = 5f;    //distance the animal will flee when provoked
    public CircleCollider2D personal_space;

    private GameObject omanager;
    private HerdManager manager;
    private Vector2 movement;
    private float timeLeft;
    private Rigidbody2D rb;
    private Vector2 mousepos;
    private Vector2 distance;
    private Animator anim;
    private SpriteRenderer rend;
    private RaycastHit2D hit;
    private Coroutine moverand;
    private List<GameObject> environment;
    bool moving_randomly;

    private void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();

        environment = new List<GameObject>();

        moving_randomly = false;

        //omanager = GameObject.FindGameObjectWithTag("Manager");
        //manager = (HerdManager)omanager.GetComponent(typeof(HerdManager));


        //try {
            
        //}

        //catch(Exception e) {
        //    if (e.InnerException is NullReferenceException){

        //    }
        //}

        //manager.RecalculateAnimalNumber();

        //moverand = StartCoroutine(MoveRandom());

        //Physics2D.IgnoreLayerCollision(ANIMAL_LAYER, ANIMAL_LAYER);
    }

    void Update() {

        //get the current position of the mouse on the screen in terms of world space
        mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        distance = mousepos - (Vector2)transform.position;

        //check if there is anyone nearby
        if (environment.Count > 0) {
            // do boid-like behavior
            foreach(GameObject o in environment) {
                Debug.DrawLine(o.transform.position, transform.position, Color.red);
            }
        }

        else {
            //move about randomly
            if (!moving_randomly) {
                moverand = StartCoroutine(MoveRandom());
            }
        }

        //define the vector coming out of the other side of the animal
        //this is the direction the animal will flee
        //flee_distance must ALWAYS be negative or else the animal will flee toward user's cursor
        Vector2 flee_direction = Vector2.MoveTowards(transform.position, mousepos, -flee_distance);

        //show the flee vector for debug purposes
        Debug.DrawLine(transform.position, flee_direction, Color.white);

        if (distance.magnitude < fleeing_proximity) {
            if (Input.GetMouseButtonDown(0)) {
                //stop moving randomly
                //StopCoroutine(moverand);
                StopAllCoroutines();

                //begin fleeing routine
                StartCoroutine(MoveAwayFromClick(flee_direction, flee_distance));
            }
        }
        
    }

    private void OnDestroy() {
        //manager.RecalculateAnimalNumber();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        switch (collision.gameObject.tag) {
            case "Animal":
                //print("Animal collision");
                //StopAllCoroutines();
                //moverand = StartCoroutine(MoveRandom());
                environment.Add(collision.gameObject);
                break;
            case "Landmine":
                //print("Landmine collision");
                Trap trap = (Trap)collision.gameObject.GetComponent(typeof(Trap));
                trap.Activate(this.gameObject);
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        switch (collision.gameObject.tag) {
            case "Animal":
                environment.Remove(collision.gameObject);
                break;
        }
    }

    IEnumerator GroupUp() {
        //Vector2 vector_average = manager.GetAverage();

        //smaller travel time means faster movement
        float traveltime = Random.Range(3f, 4f);
        float timestart = Time.time;
        Vector2 locstart = transform.position;

        //yield return StartCoroutine(Move(vector_average, traveltime));

        //prevents animal from moving immediatley from one location to the next
        float wait_time = Random.Range(2f, 4f);
        yield return new WaitForSeconds(wait_time);
    }

    //Represents aimless, illogical movement
    IEnumerator MoveRandom() {

        moving_randomly = true;

        while (true) {
            
            //includes the possible of a zero outcome
            //may need to alter this in future
            Vector2 randtrans = Random.insideUnitCircle * Random.Range(-5f, 5f);

            //define animal's new location to move to in reference to current position
            Vector2 newloc = new Vector2(transform.position.x + randtrans.x, transform.position.y + randtrans.y);

            //smaller travel time means faster movement
            float traveltime = Random.Range(1f, 4f);
            float timestart = Time.time;
            Vector2 locstart = transform.position;

            yield return StartCoroutine(Move(newloc, traveltime));

            //prevents animal from moving immediatley from one location to the next
            float wait_time = Random.Range(2f, 4f);
            yield return new WaitForSeconds(wait_time);

            //moving_randomly = false;

            //yield return StartCoroutine(GroupUp());
        }
    }

    //Represents provoked movement that always moves away from the user's finger/cursor
    IEnumerator MoveAwayFromClick(Vector2 direction, float distance) {

        float traveltime = 1.5f;
        float timestart = Time.time;
        Vector2 locstart = transform.position;

        yield return StartCoroutine(Move(direction, traveltime));

        //need to start the MoveRandom coroutine here, after we have finished our fleeing movement
        moverand = StartCoroutine(MoveRandom());
    }

    IEnumerator Move(Vector2 direction, float travel_time) {
        Vector2 start = transform.position;
        //travel_time = (start.magnitude - direction.magnitude) / 0.25f;
        float time_start = Time.time;

        Debug.DrawLine(start, direction, Color.yellow, 5f);

        hit = Physics2D.Raycast(transform.position, direction, flee_distance, ANIMAL_LAYER);

        //Prevents the mean ol' player from shoving sheep into a corner. Because nobody puts sheep in a corner.
        //It's hacky; sorry!
        if(hit.distance < .8f) {
            transform.Translate(-direction / 100);
        }

        //Start movement animation
        anim.SetBool("Moving", true);

        while (Time.time < time_start + travel_time && (hit.distance > .8f || hit.collider == null)) {
            transform.position = Vector2.Lerp(start, direction, (Time.time - time_start) / travel_time);
            hit = Physics2D.Raycast(transform.position, direction, flee_distance, ANIMAL_LAYER);
            //print("Collider hit: " + (hit.collider == null ? "Nothing" : hit.collider.name) + " Starting distance from collision: " + hit.distance + " Distance from collision (current frame): " + hit.distance);
            yield return null;
        }

        //Stop movement animation
        anim.SetBool("Moving", false);
    }

}
