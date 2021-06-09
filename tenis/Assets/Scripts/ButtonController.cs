using System.Collections;
using System.Collections.Generic;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    InputField LoginRoomKey;
    InputField NickName;

    public static string name;
    


    public void QuickGame()
    {

        if (PhotonNetwork.InLobby)
        {
            
            name = NickName.text;
           
            if (name != "")
            {
                PhotonNetwork.JoinRandomRoom();  //herhangi kurulmuş random odaya gir.
                SceneManager.LoadScene("SampleScene"); //Oyun sahnesini yükle.
            }
            else
            {
                Debug.Log("İsim Girilmedi.");
            }
        }
        

    }

    public void CreateGame()
    {
   

        System.Random rastgele = new System.Random();
        string harfler = "ABCDEFGHIJKLMNOPRSTUVYZ";
        string uret = "";
        for (int i = 0; i < 6; i++)
        {
            uret += harfler[rastgele.Next(harfler.Length)];

        }
        if (PhotonNetwork.InLobby) //eger serverin içersindeyse
        {            
            name = NickName.text;
                        
            moveBall.Key = uret;
            if (name != "")
            {
                PhotonNetwork.CreateRoom(uret, new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true }
        , TypedLobby.Default); //Yeni oda olustur.
                SceneManager.LoadScene("SampleScene"); //Oyun sahnesini yükle.
            }
            else
            {
                Debug.Log("İsim Girilmedi.");
            }
        }
    }
   

    public void GoGame(string roomKey)
    {
        if (PhotonNetwork.InLobby)
        {
            name = NickName.text;
            roomKey = LoginRoomKey.text;
            if (roomKey != "" && name != "")
            {
                PhotonNetwork.JoinRoom(roomKey);
                
                SceneManager.LoadScene("SampleScene");
            }
            else
            {
                Debug.Log("İsim Gir.");
            }

        }
       
    }

    public void ExitGame()
    {
        Application.Quit(); //Uygulamadan cik.
    }
    // Start is called before the first frame update
    void Start()
    {
        NickName = GameObject.Find("NickName").GetComponent<InputField>(); //Nickname InputField olarak tanımladık.
        
        LoginRoomKey = GameObject.Find("LoginRoomKey").GetComponent<InputField>();
        PhotonNetwork.JoinLobby(); //Eger lobiden cikma durumu olursa yeniden baglanmasi icin bir kez daha burda kullandik.
        
    }

   
}
