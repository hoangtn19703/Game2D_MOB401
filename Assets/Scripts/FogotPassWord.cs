using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Text;

public class FogotPassWord : MonoBehaviour
{
    public TMP_InputField txtUser, txtOTP, txtNewPass, texReNewPass;
    public GameObject rePass, sendOTP, login;
    public TMP_Text txtError;
    public static OTPModel oTPModel;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SendOTP()
    {
        var user = txtUser.text;
        oTPModel = new OTPModel(user);
        StartCoroutine(SendOTPAPI(oTPModel));
        SendOTPAPI(oTPModel);
    }

    // goi api send otp
    IEnumerator SendOTPAPI(OTPModel oTPModel)
    {

        string jsonStringRequest = JsonConvert.SerializeObject(oTPModel);

        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/send-otp", "POST");
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
                // th�nh c�ng, load panel reset
                sendOTP.SetActive(false);
                rePass.SetActive(true);
            }
            else
            {
                // Th�ng b�o th?t b?i
                txtError.text = responseModel.notification;

            }

        }


    }

    public void ReSetPass()
    {
        //Debug.Log(oTPModel.username);





        var newpass = txtNewPass.text;
        string reNewPass = texReNewPass.text;

        if (newpass.Equals(reNewPass))
        {
            var otp = int.Parse(txtOTP.text);
            ReSetPassModel reSetPassModel = new ReSetPassModel(oTPModel.username, otp, reNewPass);
            StartCoroutine(ResetPassAPI(reSetPassModel));
            ResetPassAPI(reSetPassModel);
        }
        else
        {
            txtError.text = "Password không trùng nhau";
        }
    }

    //gọi api reset pass
    IEnumerator ResetPassAPI(ReSetPassModel reSetPassModel)
    {

        string jsonStringRequest = JsonConvert.SerializeObject(reSetPassModel);

        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/reset-password", "POST");
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
                rePass.SetActive(false);
                login.SetActive(true);
            }
            else
            {
                // Th�ng b�o th?t b?i
                txtError.text = responseModel.notification;

            }

        }


    }

}
