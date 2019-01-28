using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://en.wikipedia.org/wiki/Boids
//https://en.wikipedia.org/wiki/Flocking_(behavior)

public class Animal : MonoBehaviour {

    private const int ANIMAL_LAYER = 9;

    public float maxSpeed = 5f;
    public float fleeing_proximity = 8; //closest radius at which an animal will flee from user's cursor
    public float flee_distance = 5f;    //distance the animal will flee when provoked

    private Vector2 movement;
    private float timeLeft;
    private Rigidbody2D rb;
    private Vector2 mousepos;
    private Vector2 distance;
    private Animator anim;
    private SpriteRenderer rend;
    private RaycastHit2D hit;
    private Coroutine moverand;

    private void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        float interval = Random.Range(1f, 4f);
        moverand = StartCoroutine(MoveRandom());
        anim.SetBool("Moving", true);
        Physics2D.IgnoreLayerCollision(ANIMAL_LAYER, ANIMAL_LAYER);
    }

    void Update() {

        //get the current position of the mouse on the screen in terms of world space
        mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        distance = mousepos - (Vector2)transform.position;

        //define the vector coming out of the other side of the animal
        //this is the direction the animal will flee
        //flee_distance must ALWAYS be negative or else the animal will flee toward user's cursor
        Vector2 flee_direction = Vector2.MoveTowards(transform.position, mousepos, -flee_distance);

        //print("Flee direction is: " + flee_direction);

        //show the flee vector for debug purposes
        Debug.DrawLine(transform.position, flee_direction, Color.white);

        if (distance.magnitude < fleeing_proximity) {
            if (Input.GetMouseButtonDown(0)) {
                //stop moving randomly
                //StopCoroutine(moverand);
                StopAllCoroutines();

                //begin fleeing routine
                StartCoroutine(MoveAway(flee_direction, flee_distance));
            }
        }
        
    }

    IEnumerator MoveRandom() {

        while (true) {
            
            //print("Entered move");

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
        }
    }

    IEnumerator MoveAway(Vector2 direction, float distance) {

        float traveltime = 1.5f;
        float timestart = Time.time;
        Vector2 locstart = transform.position;

        yield return StartCoroutine(Move(direction, traveltime));

        //need to start the MoveRandom coroutine here, after we have finished our fleeing movement
        moverand = StartCoroutine(MoveRandom());
    }

    IEnumerator Move(Vector2 direction, float travel_time) {
        Vector2 start = transform.position;
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
            print("Collider hit: " + (hit.collider == null ? "Nothing" : hit.collider.name) + " Starting distance from collision: " + hit.distance + " Distance from collision (current frame): " + hit.distance);
            yield return null;
        }

        //Stop movement animation
        anim.SetBool("Moving", false);
    }

}
