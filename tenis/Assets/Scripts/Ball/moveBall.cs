using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class moveBall : MonoBehaviour
{
    
    public Button ExitBtn;
    
    public int PlayerScore1 = 0;
    public int PlayerScore2 = 0;

    public Text Score1;
    public Text Score2;

    public Text Ping;

    public Text RoomKeyText;
    private Rigidbody2D rb;
    public static string Key;

    private void ScoreTable()
    {        
        Score1.text = PhotonNetwork.PlayerList[1].NickName +": "+ PlayerScore2.ToString();
        Score2.text = PhotonNetwork.PlayerList[0].NickName +": "+ PlayerScore1.ToString();        
    }
    private void Update()
    {
        Ping.text = PhotonNetwork.GetPing().ToString() + "Ms"; //Ping Text
    }

    private PhotonView pv;
    [PunRPC]
    public void ExitPlayer() //Oyuncu ciktiginda topun konumu sifirlanir.
    {
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;
    }


    [PunRPC]


    public void GoBall()
    {
        ScoreTable();              
        //Topun Sag ya da Sol sahaya gitmesi ve giderken bu degerler arasinda gitmesi.
        float rand = Random.Range(0, 2); //sag ya da sola gitme olayi.
        

        while(true)
        {
            float randx = Random.Range(-25, 25); //x ekseninde -25 ila 25 arasında itme gucu uyguluyoruz.
            float randy = Random.Range(-25, 25); // y ekseninde ....
            if (randx > 15 || randx < -15) // topu her defasında farklı hiz olsun istedik, x degerinin az olmasi top yavas hareket ediyor. 
            {
                if (rand < 1)
                {
                    rb.AddForce(new Vector2(randx, randy)); //itme gucu uygulanıyor.
                    break;
                }
                else
                {
                    rb.AddForce(new Vector2(randx, randy));
                    break;
                }
            }


            
        }

           
    }    

    [PunRPC]

    //Gol olunca uygulanan islemler.
    public void goal(int Player1 = 0, int Player2 = 0)
    {
        
        PlayerScore1 += Player1;
        PlayerScore2 += Player2;
        

        if (PlayerScore1 == 5 || PlayerScore2 == 5)
        {
            RestartGame();
        }
        else
        {
            ResetBall();
            GoBall();
        }
        
        
    }
    //Odadan cikis fonk.
    public void GoLobby()
    {
        PhotonNetwork.LeaveRoom();        
    }
    //Herhangi bir oyuncu kazaninca oyun yeniden basliyor.
    public void RestartGame()
    {
        if (PlayerScore1 == 5)
        {
            Score2.text = PhotonNetwork.PlayerList[0].NickName + ": " + PlayerScore1.ToString() + "\nWINNER";

        }
        else if (PlayerScore2 == 5)
        {            
            Score1.text = PhotonNetwork.PlayerList[1].NickName + ": " + PlayerScore2.ToString() +"\nWINNER";
        }
        PlayerScore1 = 0;
        PlayerScore2 = 0;
        ResetBall();
        Invoke("GoBall", 5.0f);      
        
    }
    
    //Nesneleri tanimladik.
    void Start()
    {
        Ping = GameObject.Find("Canvas/Ping").GetComponent<Text>();
        ExitBtn = GameObject.Find("Canvas/Exit").GetComponent<Button>();
        RoomKeyText = GameObject.Find("Canvas/RoomKeyText").GetComponent<Text>();
        RoomKeyText.text = "" + Key;
        Debug.Log(RoomKeyText.name);
        Score1 = GameObject.Find("Canvas/Score1").GetComponent<Text>();
        Score2 = GameObject.Find("Canvas/Score2").GetComponent<Text>();

        rb = GetComponent<Rigidbody2D>();
        
        
        pv = GetComponent<PhotonView>();
     }

    public void ResetBall()
    {
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;
    }
    //Sag ve Sol duvar'a top carpinca
      void OnCollisionEnter2D(Collision2D coll)
    {
        if (pv.IsMine)
        {
            if (coll.gameObject.name == "RightWall")
            {
                pv.RPC("goal", RpcTarget.AllBuffered, 0, 1);
                
            }
            else if (coll.gameObject.name == "LeftWall")
            {
                pv.RPC("goal", RpcTarget.AllBuffered, 1, 0);
            }

        }



        

    }
   
  }
