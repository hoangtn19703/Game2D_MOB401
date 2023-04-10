using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    private int count = 1;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(count-- > 0){
           float position = Random.Range(-1f, 3f);
            GameObject Mt = (GameObject) Instantiate(Resources.Load("Prefabs/BeetleRed"), 
                                                new Vector3(position, -3.18f, 0), 
                                                Quaternion.identity);
            Mt.GetComponent<BeetleRedScript>().SetStart(position - 5);
            Mt.GetComponent<BeetleRedScript>().SetEnd(position + 5); 
            Mt.GetComponent<BeetleRedScript>().SetPlayer(player); 
        }
        
    }
}
