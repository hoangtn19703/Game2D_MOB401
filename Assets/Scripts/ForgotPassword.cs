using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class ForgotPassword : MonoBehaviour
{
    public TMP_InputField txtUser;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //send OTP
    public void SendOTP()
    {
        var user = txtUser.text;
        OTPModel oTPModel = new OTPModel(user);
        StartCoroutine(SendOTPAPI(oTPModel));
        SendOTPAPI(oTPModel);
    }
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
            ReponseModel reponseModel = JsonConvert.DeserializeObject<ReponseModel>(jsonString);
            if (reponseModel.status == 1)
            {
                //SceneManager.LoadScene("Scene1");
            }
            else
            {
                //
            }
        }
        request.Dispose();
    }
}
