using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JoinAsClient : MonoBehaviour
{
    private GameSettings settings;
    public OverlayFader fader;
    bool joined = false;
    //int[] joinedPlayers = new int[6];

    private void Start()
    {
        //      for(int i = 0; i < 6; i++)
        //      {
        //          joinedPlayers[i] = 0;
        //      }

        if (Settings.gameSettings != null)
        {
            settings = Settings.gameSettings;
        }
        else
        {
            settings = ScriptableObject.CreateInstance<GameSettings>();
        }
    }
    //triggers when another object enters its area.
    private void OnTriggerEnter(Collider other)
    {
        //if object is player then load scene
        if (other.gameObject.tag == "Player" && joined == false)
        {
            if (fader == null)
            {
                StartCoroutine(Join());
            }
            else
            {
                joined = true;
                StartCoroutine(fader.FadeToBlackAndDo(Join()));
            }
        }
    }

    public IEnumerator Join()
    {
        if (NetworkManager.singleton == null)
        {
            yield break;
        }
        if(NetworkClient.active) {
            yield break;
        }
        string ipAddress = settings.IpAddress ?? "localhost";
        Debug.Log(settings);
        Debug.Log("Joining as client on " + ipAddress);
        NetworkManager.singleton.networkAddress = ipAddress;
        NetworkManager.singleton.StartClient(); //Join as client, only changes to online scene if server/host is on
        yield return new WaitForEndOfFrame();
    }
}