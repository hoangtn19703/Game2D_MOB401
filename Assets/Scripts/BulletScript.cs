using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BulletScript : MonoBehaviour
{
    private bool isRight;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate((isRight ? Vector3.right : Vector3.left) * Time.deltaTime * 3f);

        Vector2 scale = transform.localScale;
        if (isRight)
        {
            scale.x = 1;
            transform.Translate((isRight ? Vector3.right : Vector3.left) * Time.deltaTime * 3f);
        }
        else
        {
            scale.x = -1;
            transform.Translate((isRight ? Vector3.right : Vector3.left) * Time.deltaTime * 3f);
        }

        transform.localScale = scale;
    }


    public void SetisRight(bool isRight)
    {
        this.isRight = isRight;
    }
}
