using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;// for saving 
using System.Threading.Tasks;// for multithreading

public class SaveManager : MonoBehaviour
{
    SaveStateBase[] allTransformSaveStates;
    // Start is called before the first frame update
    void Start()
    {
        //allTransformSaveStates = FindObjectsOfType<SaveStateBase>();
    }

    public void Setup()
    {
        allTransformSaveStates = FindObjectsOfType<SaveStateBase>(true);
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void SaveFunction()
    {
        DeleteAllFiles();// delete first so we don't get any leftover 
        StartCoroutine("Save");
    }
    private IEnumerator Save()
    {
        foreach (SaveStateBase state in allTransformSaveStates)
        {
            if (state.ShouldSave())
            {// only save the state if need to save on disk space 
                yield return new WaitForEndOfFrame();

                string resultingJson = state.SaveState();//get JSON from the state 

                //write the file from the app data folder
                WriteFileAsync(Application.persistentDataPath + "/" + state.GetUId() + ".save", resultingJson);

                yield return new WaitForEndOfFrame();
            }
        }
    }

    public async Task WriteFileAsync(string path, string json)
    {
        using (StreamWriter outputFile = new StreamWriter(path))
        {//using the stream writer to write to a path 
            await outputFile.WriteAsync(json);
        }
    }

    public void Load()
    {
        foreach (SaveStateBase state in allTransformSaveStates)
        {
            if (state.ShouldLoad())
            {
                string expectedFileLocation = (Application.persistentDataPath + "/" + state.GetUId() + ".save");

                if (File.Exists(expectedFileLocation))
                {
                    string json = File.ReadAllText(expectedFileLocation);
                    state.LoadState(json);
                }
                else if (state.gameObject.name.Contains("Asteroid")&& state.gameObject.activeInHierarchy)
                {// for asteroids that will need to be deactivated
                    ObjectPool.SharedInstance.ReturnToPool(state.gameObject);
                }
            }
        }
    }

    public void DeleteAllFiles()
    {

        var allFiles = Directory.GetFiles(Application.persistentDataPath);

        for (int i = 0; i < allFiles.Length; i++)
        {
            File.Delete(allFiles[i]);
        }
    }
}
