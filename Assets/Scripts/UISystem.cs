using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    [SerializeField] private Button m_generateMoleculeButton;
    [SerializeField] private TMP_InputField m_chemicalStructureInput;

    private void Awake()
    {
        Assert.IsNotNull(value: m_generateMoleculeButton);
        Assert.IsNotNull(value: m_chemicalStructureInput);

        m_generateMoleculeButton.onClick.AddListener(() => DispatchChemicalStructureRequestEvent());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            DispatchChemicalStructureRequestEvent();
        }
    }

    private void OnDestroy()
    {
        m_generateMoleculeButton.onClick.RemoveAllListeners();
    }

    private void DispatchChemicalStructureRequestEvent()
    {
        // Dispatching a ChemicalStructureRequestEvent whenever the generateMoleculeButton is pressed
        EventMessageBus.DispatchEventMessage(m_chemicalStructureInput.text);
    }
}
