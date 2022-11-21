using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public GameObject MeteorObj;
    public GameObject EndGameUi;
    public Player PlayerClass;

    private float meteorCounter = 0;
    private int maxMeteorLength = 3;

    public delegate void OnGameEnd();
    public static OnGameEnd onGameEnd;

    public GameObject PauseGameUi;
    private static bool gameIsPaused;

    // Start is called before the first frame update
    void Start()
    {
        // InvokeRepeating("AddMeteorToScene", 1, 1);
        Meteor.onMeteorDestroyed += OnMeteorDestroyed;
        onGameEnd += OnEnd;
        StartCoroutine(AddMeteorsToScene());
    }

    void OnEnd()
    {
        EndGameUi.SetActive(true);
    }

    public void Restart()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 1;
        }
        StopAllCoroutines();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    void OnMeteorDestroyed(Meteor destroyedMeteor)
    {
        float destroyedIndex = destroyedMeteor.getIndex();
        if (destroyedIndex > 0.5)
        {
            // split in 2
            Vector3 difPos = new Vector3(1, 0, 0);
            Vector3 nextScale = destroyedMeteor.gameObject.transform.localScale / 2;
            SpawnHalfMeteor(nextScale, destroyedMeteor, difPos, -35f);
            SpawnHalfMeteor(nextScale, destroyedMeteor, -difPos, 35f);
        } else {
            // spawn health bonus
            meteorCounter -= 0.5f;
        }

        if (meteorCounter == 0)
        {
            maxMeteorLength += 1;
            StartCoroutine(AddMeteorsToScene());
        }
    }

    Vector3 GetMeteorRandomSpawnPosition()
    {
        while (true)
        {
            float[] xs = new float[2] { -15f, 15f };
            float[] zs = new float[2] { -9f, 9f };
            int extreme = Random.Range(0, 2);
            int i = Random.Range(0, 2);
            float x = extreme == 0 ? xs[i] : Random.Range(xs[0], xs[1]);
            float z = extreme == 1 ? zs[i] : Random.Range(zs[0], zs[1]);
            Vector3 pos = new Vector3(x, 1f, z);
            if(Physics.CheckSphere(pos, 1f))
            {
                return pos;
            }
        }
    }

    IEnumerator AddMeteorsToScene()
    {
        for(var j=0; j<maxMeteorLength && PlayerClass != null; j++)
        {
            yield return new WaitForSeconds(1);
            Vector3 pos = GetMeteorRandomSpawnPosition();
            Meteor meteor = SpawnMeteor(pos, Quaternion.identity, 1f);
            Vector3 relativePosRespectPlayer = PlayerClass != null ? PlayerClass.transform.position - meteor.transform.position : Vector3.forward;
            Quaternion rotation = Quaternion.LookRotation(relativePosRespectPlayer, Vector3.forward);
            rotation.z = 0;
            rotation.x = 0;
            meteor.transform.rotation = rotation;
            meteor.GetComponent<Rigidbody>().AddForce(meteor.transform.forward * 250, ForceMode.Acceleration);
            meteorCounter += 1;
        }
    }

    Meteor SpawnMeteor(Vector3 pos, Quaternion rot, float index)
    {
        Meteor meteor = Instantiate(MeteorObj, pos, rot).GetComponent<Meteor>();
        meteor.setIndex(index);
        return meteor;
    }

    Meteor SpawnHalfMeteor(Vector3 nextScale, Meteor Parent, Vector3 difPos, float yRot)
    {
        float destroyedIndex = Parent.getIndex();
        Meteor meteor = SpawnMeteor(
            Parent.gameObject.transform.position + difPos,
            Parent.gameObject.transform.rotation,
            destroyedIndex / 2
        );
        meteor.transform.localScale = nextScale;
        meteor.transform.position = new Vector3(meteor.transform.position.x, 0.5f, meteor.transform.position.z);
        meteor.transform.Rotate(0, yRot, 0);
        meteor.GetComponent<Rigidbody>().AddForce(meteor.transform.forward * 250, ForceMode.Acceleration);
        return meteor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;
            TogglePauseGame();
        }
    }

    void TogglePauseGame()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0f;
            PauseGameUi.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            PauseGameUi.SetActive(false);
        }
    }
}
