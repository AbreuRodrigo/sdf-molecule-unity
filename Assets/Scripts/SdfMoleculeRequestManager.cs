using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

public class SdfMoleculeRequestManager : MonoBehaviour
{
    private const string m_chemStructUrl = "https://cactus.nci.nih.gov/chemical/structure/{0}/file?format=sdf&get3d=true";
    
    [Header("Prefabs")]
    public GameObject m_carbonPrefab;
    public GameObject m_hydrogenPrefab;
    public GameObject m_bondPrefab;

    [Header("Config")]
    [SerializeField] private ChemicalElementColorConfig m_chemicalElementColorConfig;

    private ObjectSpawner m_objectSpawner;
    private Molecule3DGenerator m_molecule3DGenerator;

    private GameObject m_currentMolecule;

    private void Awake()
    {
        Assert.IsNotNull(value: m_carbonPrefab);
        Assert.IsNotNull(value: m_hydrogenPrefab);
        Assert.IsNotNull(value: m_bondPrefab);
        Assert.IsNotNull(value: m_chemicalElementColorConfig);

        m_objectSpawner = new ObjectSpawner(m_chemicalElementColorConfig, m_carbonPrefab, m_hydrogenPrefab, m_bondPrefab);
        m_molecule3DGenerator = new Molecule3DGenerator(m_objectSpawner);

        EventMessageBus.RegisterListener(OnChemicalStructureRequested);
    }

    private void OnDestroy()
    {
        EventMessageBus.Instance.Dispose();
    }

    private void OnChemicalStructureRequested(string chemicalStructure)
    {
        StartCoroutine(RequestSdfStructure(chemicalStructure));
    }    

    private IEnumerator RequestSdfStructure(string chemicalStructure)
    {
        if (m_currentMolecule != null)
        {
            Destroy(m_currentMolecule);
        }

        using (var webRequest = UnityWebRequest.Get(string.Format(m_chemStructUrl, chemicalStructure)))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error downloading SDF data: " + webRequest.error);
            }
            else
            {
                var sdfString = webRequest.downloadHandler.text;
                m_currentMolecule = m_molecule3DGenerator.Generate3DMolecule(sdfString);

                if (m_currentMolecule != null)
                {
                    m_currentMolecule.AddComponent<Spinner>();
                }
            }
        }
    }
}