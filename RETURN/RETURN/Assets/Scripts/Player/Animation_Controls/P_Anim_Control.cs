using UnityEngine;
using System.Collections;

public class P_Anim_Control : MonoBehaviour {

    P_Controls p_Controls;
    P_Ground_Check g_Check;
    Animator anim;
    Material mat;
    Physics_Animation_Blend animPhysics;

    public SkinnedMeshRenderer Mrend;
    float movment;
    bool isGrounded;

	void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        anim.animatePhysics = true;
        p_Controls = transform.root.GetComponent<P_Controls>();
        mat = Mrend.material;
        g_Check = FindObjectOfType<P_Ground_Check>();
        animPhysics = FindObjectOfType<Physics_Animation_Blend>();
    }

	void Start ()
    {
        StartCoroutine(dickballs());
    }
	

    IEnumerator dickballs()
    {
        while(isGrounded)
        {
           // Debug.Log("Nigger");
            yield return null;
        }

        //Debug.Log("Nigger no more");

    }

	void FixedUpdate ()
    {
        anim.SetFloat("LocoMotion", Input.GetAxis("Vertical"));

        isGrounded =  P_Ground_Check.isGrounded;
       
        
        anim.SetBool("IsGrounded", isGrounded);
        if (isGrounded)
        {
            mat.SetColor("_Color", new Color(1, 0, 0, 0));
        }
        else
        {
            mat.SetColor("_Color", new Color(1, 1, 1, 1));
        }

        if (Input.GetButtonDown("Grab"))
        {
            //Grab();
            anim.SetBool("Mag", true);
            anim.SetLayerWeight(1, 1);
        }
        else if (Input.GetButtonUp("Grab"))
        {
            //Grab();
            anim.SetBool("Mag", false);
            anim.SetLayerWeight(1, 0);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            anim.SetTrigger("Jump");
        }
        else
        {

        }
    }
}
