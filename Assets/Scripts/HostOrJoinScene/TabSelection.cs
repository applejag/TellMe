using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabSelection : MonoBehaviour
{
    private EventSystem system;

    private void Start() {
        system = EventSystem.current;
    }

    private void Update()
    {
        if (system.currentSelectedGameObject != null && Input.GetKeyDown(KeyCode.Tab))
        {
            var target = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

            if (target != null) {
                TMPro.TMP_InputField inputField = target.GetComponent<TMPro.TMP_InputField>();
                if (inputField != null) {
                    inputField.OnPointerClick(new PointerEventData(system));
                }

                system.SetSelectedGameObject(target.gameObject, new BaseEventData(system));
            }
        
        }
    }
}
