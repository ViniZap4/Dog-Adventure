using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Moviment Variables")]
    [Space]
    private Vector3 direction;
    public float MovementSpeed = 3f;
    private bool isWalking;


    //input variables
    private float horizontal;
    private float vertical;

    [Header("Attack Variables")]
    [Space] private bool isAttacking;
    public Transform HitArea;
    private float hitRange = 0.5f;
    public Collider[] hitInfo;
    public LayerMask hitMask;
    public int amountDmg;


    //components
    private CharacterController controller;
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        MoveCharacter();
        UpdateAnimator();
    }

    //geting and managing inputs
    void Inputs()
    {
        //mobiment input
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if(Input.GetButtonDown("Fire1") && !isAttacking)
        {
            Attack();
        }

    }

    void MoveCharacter()
    {
        // set direction in axis x and z and normalized
        direction = new Vector3(horizontal, 0f, vertical).normalized;

        // if player is in moving 
        if (direction.magnitude > 0.1f)
        {

            //geting angle of direction and converting for deg
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // set transform rotation of player
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);

            isWalking = true;

        }
        else
        {
            isWalking = false;
        }

        //move Character setting direaction and speed
        controller.Move(direction * MovementSpeed * Time.deltaTime);

    }

    
    void UpdateAnimator()
    {
        anim.SetBool("isWalking", isWalking);

    }

    void Attack()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");

        hitInfo = Physics.OverlapSphere(HitArea.position, hitRange, hitMask);

        foreach (Collider itemCollided in hitInfo)
        {
            itemCollided.gameObject.SendMessage("GetHit", amountDmg, SendMessageOptions.DontRequireReceiver);
        }
    }
    void AttackIsDone()
    {
        isAttacking = false;
    }
}