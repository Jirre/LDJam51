using JvLib.UI.Visualizers;
using TMPro;
using UnityEngine;

public class ResourceVisualizer : UIVisualizer<int>
{
    [SerializeField] private TMP_Text _Visualizer;
    
    protected override void OnContextUpdate(int pContext)
    {
        if (_Visualizer != null) _Visualizer.SetText(pContext.ToString());
    }
}
