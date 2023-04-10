using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Text;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rb2D;
    //animator
    private Animator animator;
    public bool Isrunning;
    public bool playerIsOnFloor;

    // hiệu ứng bụi khi chạy
    public ParticleSystem psDust;

    //menu 
    public GameObject menu;
    private bool isPlaying = true;

    // coin
    public TMP_Text txtCoin;
    private int countCoin = 0;
    public AudioSource soundCoin;

    //bullet
    private int bulletNumber;
    public TMP_Text txtBullet;

    private bool isRight;


    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        //load điểm
        if (LoginUser.responseModel.score >= 0)
        {
            countCoin = LoginUser.responseModel.score;
            txtCoin.text = countCoin + " x";
        }

        // load vị trí player 
        if (LoginUser.responseModel.positionX != "")
        {
            var posX = float.Parse(LoginUser.responseModel.positionX);
            var posY = float.Parse(LoginUser.responseModel.positionY);
            var posZ = float.Parse(LoginUser.responseModel.positionZ);
            transform.position = new Vector3(posX, posY, posZ);
        }

        bulletNumber = 0;
        txtBullet.text = bulletNumber + " ";


    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("PlayerIsOnFloor", playerIsOnFloor);
        animator.SetBool("Isrunning", Isrunning);
        Isrunning = false;
        playerIsOnFloor = true;

        Quaternion rotation = psDust.transform.localRotation;

        Vector2 scale = transform.localScale;

        //nhấn mũi tên phải
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rotation.y = 180;
            psDust.transform.localRotation = rotation;
            psDust.Play();
            scale.x = 1;
            isRight = true;
            // chạy phải Vector3(1,0,0)
            transform.Translate(Vector3.right * 5f * Time.deltaTime);
            Isrunning = true;



        }

        //nhấn mũi tên trái
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rotation.y = 0;
            psDust.transform.localRotation = rotation;
            psDust.Play();
            scale.x = -1;
            isRight = false;
            //chạy trái Vector3(-1,0,0)
            transform.Translate(Vector3.left * 5f * Time.deltaTime);
            Isrunning = true;
        }

        transform.localScale = scale;

        // nhấn Space và phím tên lên 
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))
        {
            //bay lên Vector3(0,1,0)
            transform.Translate(Vector3.up * 10f * Time.deltaTime);
            // rb2D.AddForce(new Vector2(0, 10));// này làm lúc rơi xuống mượt hơn
            playerIsOnFloor = false;
            Isrunning = false;

        }

        //pause
        if (Input.GetKeyDown(KeyCode.P))
        {
            pauseGame();

        }

        //bắn đạn 
        if(bulletNumber > 0){
            if (Input.GetKeyDown(KeyCode.R))
            {
                var x = transform.position.x + (isRight ? 0.5f : -0.5f);
                var y = transform.position.y;
                var z = transform.position.z;
                GameObject gameObject = (GameObject)Instantiate(Resources.Load("Prefabs/bullet"),
                    new Vector3(x, y, z),
                    Quaternion.identity
                );
                bulletNumber --;
                txtBullet.text = bulletNumber + " ";
                gameObject.GetComponent<BulletScript>().SetisRight(isRight);
            }

        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "coin")
        {
            soundCoin.Play();
            countCoin += 1;
            txtCoin.text = countCoin + "x";
            Destroy(other.gameObject);
        }

        if(other.gameObject.tag == "bullet1"){
            bulletNumber += 1;
            txtBullet.text = bulletNumber + " ";
            Destroy(other.gameObject);
        }
    }

    public void pauseGame()
    {
        if (isPlaying)
        {
            menu.SetActive(true);
            Time.timeScale = 0;
            isPlaying = false;
        }
        else
        {
            menu.SetActive(false);
            Time.timeScale = 1;
            isPlaying = true;
        }
    }

    public void nextScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Scene03");
    }

    public void saveScore()
    {
        var user = LoginUser.responseModel.username;
        ScoreModel score = new ScoreModel(user, countCoin);

        StartCoroutine(saveScoreAPI(score));
        saveScoreAPI(score);
    }

    //Api save score
    IEnumerator saveScoreAPI(ScoreModel score)
    {

        string jsonStringRequest = JsonConvert.SerializeObject(score);

        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/save-score", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonStringRequest);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            var jsonString = request.downloadHandler.text.ToString();
            ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(jsonString);
            if (responseModel.status == 1)
            {
                pauseGame();
            }
            else
            {
                // txtError.text = scoreResponseModel.notification;
            }

        }


    }

    public void SavePosition()
    {
        var user = LoginUser.responseModel.username;
        var x = transform.position.x;
        var y = transform.position.y;
        var z = transform.position.z;
        PositionModel positionModel = new PositionModel(user, x.ToString(), y.ToString(), z.ToString());


        StartCoroutine(savePositionAPI(positionModel));
        savePositionAPI(positionModel);
    }

    //API save position
    IEnumerator savePositionAPI(PositionModel positionModel)
    {
        Debug.Log("savePositionAPI");
        string jsonStringRequest = JsonConvert.SerializeObject(positionModel);

        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/save-position", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonStringRequest);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            var jsonString = request.downloadHandler.text.ToString();
            ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(jsonString);
            if (responseModel.status == 1)
            {
                //done
                Debug.Log("checkPosition - done ");
            }
            else
            {

                // gọi lại api save pos
                Debug.Log("checkPosition - " + responseModel.status);
            }

        }

    }
}