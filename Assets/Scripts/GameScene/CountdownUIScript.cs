using System.Collections;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class CountdownUIScript : NetworkBehaviour
{
    public CanvasGroup countdownGroup;
    public TMP_Text countdownText;
    public Animator countdownAnim;
    public string countdownAnimTrigger = "Next";
    public GameStateScript gameState;

    private void Reset()
    {
        countdownGroup = GetComponentInChildren<CanvasGroup>();
        countdownAnim = GetComponentInChildren<Animator>();
        countdownText = GetComponentInChildren<TMP_Text>();
        gameState = FindObjectOfType<GameStateScript>();
    }

    private string[] GetRandomCoundownTexts()
    {
        var textAsset = Resources.Load<TextAsset>("CountdownTexts");
        var csv = CsvReader.ReadCsv(textAsset.text);
        var groups = csv.Rows
            .Where(row => !string.IsNullOrWhiteSpace(row.Cells[0].Value))
            .GroupBy(row => row.Cells[0].Value)
            .Where(group => group.Count() > 0)
            .Select(group => (
                id: group.Key,
                texts: group.Select(row => row.Cells[1].Value).ToArray(),
                weight: int.TryParse(group.First().Cells[2].Value, out var weight) ? weight : 1
            ))
            .ToArray();
        var totalWeight = groups.Sum(g => g.weight);
        var randomVal = Random.Range(0, totalWeight + 1);
        var texts = groups[Random.Range(0, groups.Length)].texts;
        var sum = 0;
        foreach (var g in groups)
        {
            sum += g.weight;
            if (sum >= randomVal)
            {
                texts = g.texts;
                break;
            }
        }
        return texts;
    }

    public void StartCountdownServer()
    {
        if (!IsHost)
        {
            Debug.LogWarning($"{nameof(StartCountdownServer)} was not called from the host.", this);
            return;
        }
        var texts = GetRandomCoundownTexts();
        StartCountdownClientRpc(string.Join('\n', texts));
    }

    [ClientRpc]
    private void StartCountdownClientRpc(string multilineTexts)
    {
        StartCoroutine(CountdownCoroutine(multilineTexts));
    }

    private IEnumerator CountdownCoroutine(string multilineTexts)
    {
        countdownGroup.alpha = 1;
        var texts = multilineTexts.Trim().Split('\n');
        foreach (var text in texts)
        {
            countdownText.text = text.Trim();
            countdownAnim.SetTrigger(countdownAnimTrigger);
            yield return new WaitForSeconds(1);
        }
        if (gameState.IsOwner)
        {
            gameState.StartGame();
        }
    }
}
