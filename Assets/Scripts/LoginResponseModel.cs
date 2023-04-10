using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginResponseModel
{
    public LoginResponseModel(int status, string notification, int score, string username, string positionX, string positionY, string positionZ)
    {
        this.status = status;
        this.notification = notification;
        this.score = score;
        this.username = username;
        this.positionX = positionX;
        this.positionY = positionY;
        this.positionZ = positionZ;
    }

    public int status { get; set; }
    public string notification { get; set; }
    public int score { get; set; }
    public string username { get; set; }
    public string positionX { get; set; }
    public string positionY { get; set; }
    public string positionZ { get; set; }
}
