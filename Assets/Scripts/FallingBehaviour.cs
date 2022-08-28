using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBehaviour : MonoBehaviour
{
    protected bool falling = false;

    IEnumerator c_fall()
    {
        yield return new WaitForSeconds(4f);
        gameObject.SetActive(false);
    }

    public bool isFalling()
    {
        return falling;
    }

    public void fall()
    {
        falling = true;

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Animator>().Play("Dead");
        
        gameObject.layer = LayerMask.NameToLayer("Dying");
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        
        StartCoroutine(c_fall());
    }
}
