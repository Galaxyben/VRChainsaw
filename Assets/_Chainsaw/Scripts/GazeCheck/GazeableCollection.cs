using UnityEngine;

public class GazeableCollection : MonoBehaviour
{
    [SerializeField] private Gazeable[] gazeables;

    public Gazeable GetGazeable(Chainsaw.ChainsawParts part)
    {
        return gazeables[(int)part];
    }
}
