using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleRedScript : MonoBehaviour
{
    private new Rigidbody2D rb2D;
    public float start, end;
    private bool isRight;
    public GameObject player;

    private Vector3 originalPosition; // vị trí ban đầu
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float positionMonster = transform.position.x;
        // quái đi theo player
        if(player != null){
            var positionPlayer = player.transform.position.x;
            if(positionPlayer > start && positionPlayer < end){
                if(positionPlayer < positionMonster){
                    isRight = false;
                }
                
                if(positionPlayer > positionMonster){
                    isRight = true;
                }
            }  
        }
        

        if(positionMonster < start){
            isRight = true;

        }
        if(positionMonster > end){
            isRight = false;
        }

        Vector2 scale = transform.localScale;
        if(isRight){
            scale.x = -1;
            transform.Translate(Vector3.right * 2f * Time.deltaTime);
        }
        else {
            scale.x = 1;
            transform.Translate(Vector3.left * 2f * Time.deltaTime);
        }

        transform.localScale = scale;

    }

    public void OnCollisionEnter2D (Collision2D collision){
        // player nhảy lên giết quái 
        if(collision.gameObject.CompareTag("player"))
        {
            var direction = collision.GetContact(0).normal;
            //chạm dưới chân player
            if(Mathf.Round(direction.y) == -1){
                //chuyển thành hình chết
                // GetComponent<SpriteRenderer>().sprite = newSprite;
                // tắc animation
                // GetComponent<Animator>().enabled = false;
                //tắc chuyển động
                // isAlive = false;
                // bật trigger, đi xuyên nền
                GetComponent<BoxCollider2D>().isTrigger = true;
                originalPosition = transform.position;
                //xóa khỏi game
                Destroy(gameObject, 2);

            }
        }

        // player b?n ??n gi?t qu�i 
        if (collision.gameObject.CompareTag("bullet"))
        {
            var direction = collision.GetContact(0).normal;
            //ch?m d??i ch�n player
            if (Mathf.Round(direction.x) == -1 || Mathf.Round(direction.x) == 1)
            {
                // b?t trigger, ?i xuy�n n?n
                GetComponent<BoxCollider2D>().isTrigger = true;
                originalPosition = transform.position;
                //x�a kh?i game
                Destroy(collision.gameObject, 2);
            }
        }

        // 2 con quái đụng đầu  nhau thì quay chỗ khác 
        if (collision.gameObject.CompareTag("monster")){
            var direction = collision.GetContact(0).normal;
            //chạm bênh trái 
            if(Mathf.Round(direction.x) == 1){
                // isRight = isRight ? false : true; 
                /*
                    if(isRight == true){
                        isRight = false;
                    
                    }
                    else {
                        isRight = true;
                    }
                 */
                 isRight = !isRight;
            }
        }
    }

    public void SetStart(float start){
        this.start = start;
    }

    public void SetEnd(float end){
        this.end = end;
    }

    public void SetPlayer (GameObject player){
        this.player = player;
    }
}
