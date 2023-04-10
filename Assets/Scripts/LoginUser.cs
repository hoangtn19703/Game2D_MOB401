using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Text;


public class LoginUser : MonoBehaviour
{
    public TMP_InputField edtUser, edtPass;
    public TMP_InputField edtUserSignUp, edtPassSignUp;
    public TMP_Text txtError;
    public Selectable first;
    private EventSystem ev;
    public Button btnLogin;
    public Button btnSignup;
    public static LoginResponseModel responseModel;
    public GameObject signup, login;

    // Start is called before the first frame update
    void Start()
    {
        ev = EventSystem.current;
        first.Select();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Return)){
            btnLogin.onClick.Invoke();
        }

        if(Input.GetKey(KeyCode.Return)){
            btnSignup.onClick.Invoke();
        }
        
        if(Input.GetKeyDown(KeyCode.Tab)){
            Selectable next = ev
                .currentSelectedGameObject
                .GetComponent<Selectable>()
                .FindSelectableOnDown();
            if(next != null) {
                next.Select();
            }
        }

        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Tab)){
            Selectable next = ev
                .currentSelectedGameObject
                .GetComponent<Selectable>()
                .FindSelectableOnUp();
            if(next != null) {
                next.Select();
            }
        }
    }

    public void CheckLogin(){
        StartCoroutine(Login());
        Login();
    }

    IEnumerator Login()
    {
        var user = edtUser.text;
        var pass = edtPass.text;
        UserModel userModel = new UserModel(user, pass);

        if(user == "" || pass ==""){
            txtError.text = "Nhập đầy đủ thông tin";
        }
        string jsonStringRequest = JsonConvert.SerializeObject(userModel);

        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/login", "POST");
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
            responseModel = JsonConvert.DeserializeObject<LoginResponseModel>(jsonString);
            if(responseModel.status ==1){
                SceneManager.LoadScene("Scene1");
            }
            else {
                txtError.text = responseModel.notification;
            }
            
        }
    }

    public void CheckSignup(){
        StartCoroutine(Signup());
        Signup();
    }

    IEnumerator Signup()
    {
        var user = edtUserSignUp.text;
        var pass = edtPassSignUp.text;
        UserModel userModel = new UserModel(user, pass);

        if(user == "" || pass ==""){
            txtError.text = "Nhập đầy đủ thông tin";
        }
        string jsonStringRequest = JsonConvert.SerializeObject(userModel);

        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/register", "POST");
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
            responseModel = JsonConvert.DeserializeObject<LoginResponseModel>(jsonString);
            if(responseModel.status ==1){
                signup.SetActive(false);
                login.SetActive(true);
            }
            else {
                txtError.text = responseModel.notification;
            }
            
        }
    }

}
