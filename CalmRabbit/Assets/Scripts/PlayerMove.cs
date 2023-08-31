using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{

    public float maxSpeed = 5.0f;
    private Rigidbody rb;
    private bool onLand = true;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = onLand ? maxSpeed : 0.3f * maxSpeed;


        //Get the Screen positions of the object
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        //Get the Screen position of the mouse
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        //Get the angle between the points
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

        float inputZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        rb.transform.Translate(0f, 0f, inputZ);

        transform.rotation = Quaternion.Euler(new Vector3(0f, -angle * 5f, 0f));



        if (Input.GetKey(KeyCode.W))
        {
            if (onLand)
            {
                anim.SetBool("IsMoving", true);
            }
            else
            {
                anim.SetBool("IsMoving", false);
            }
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onLand = true;
            Debug.Log("Ground");
        }
        else if (collision.gameObject.CompareTag("Water"))
        {
            onLand = false;
            Debug.Log("Water");
        }
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
