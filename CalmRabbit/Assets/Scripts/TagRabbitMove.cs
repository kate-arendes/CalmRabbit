using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagRabbitMove : MonoBehaviour
{

    public Transform target;
    public float range;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(target.position, transform.position);

        if(dist <= range && dist > 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed);
        }
    }
}
