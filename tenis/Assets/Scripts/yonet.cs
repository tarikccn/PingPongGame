using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class yonet : MonoBehaviourPunCallbacks
{   
    static yonet admin = null;
    
    void Start()
    {
        if (admin == null)
        {
            admin = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        PhotonNetwork.ConnectUsingSettings();        
    }   
   
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Sunucuya Bağlanıldı.");

        //lobiye bağlanma 
        PhotonNetwork.JoinLobby(); 
        //Debug.Log("giriş sağlandı");

    }    
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("oyuna girildi.");

        GameObject raket2 = PhotonNetwork.Instantiate("racket2",  Vector2.zero , Quaternion.identity, 0, null);
        raket2.GetComponent<PhotonView>().Owner.NickName = ButtonController.name;     
    }
    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
        Debug.Log("Lobiden ayrıldı.");
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Debug.Log("Odadan ayrıldı.");
        SceneManager.LoadScene("Menu");
    }
    //HATA FONK.
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("Herhangi bir odaya giriş yapılamadı.");
        
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("Odaya girilemedi.");
        SceneManager.LoadScene("Menu");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("Odaya Oluşturlamadı.");
    }
    //Oyuncu odadan cikar ise
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log("Oyuncu Çıktı.");
        GameObject.FindWithTag("Player").GetComponent<PhotonView>().RPC("leavePlayer", RpcTarget.All, null);
    }        
}
