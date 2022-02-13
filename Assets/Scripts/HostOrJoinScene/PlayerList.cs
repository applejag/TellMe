using UnityEngine;

public class PlayerList : MonoBehaviour {
    public GameObject group;
    public GameObject listEntryPrefab;

    public void Add(string name) {
        var entry = Instantiate(listEntryPrefab, transform.position, transform.rotation, group.transform);
        entry.GetComponent<TMPro.TMP_Text>().SetText(name);
    }
}
