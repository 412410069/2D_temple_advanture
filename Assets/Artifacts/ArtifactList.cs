using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactList : MonoBehaviour
{
    public Artifact[] artifactList;
    public GameObject artifactPrefab;

    public void InstantiateArtifact(Vector3 pos){
        Artifact artifact = GetArtifact();

        pos += new Vector3(.5f, .5f, 1);

        GameObject ArtifactGameObject = Instantiate(artifactPrefab, pos, Quaternion.identity);
        ArtifactGameObject.GetComponent<SpriteRenderer>().sprite = artifact.artifactSprite;
    }

    public Artifact GetArtifact(){
        int index = Random.Range(0, artifactList.GetLength(0));
        return artifactList[index];
    }
}
