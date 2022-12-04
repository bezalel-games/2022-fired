using Avrahamy;
using BitStrap;
using UnityEngine;
using Logger = Nemesh.Logger;

[RequireComponent(typeof(Animator))]
public class Showcase : MonoBehaviour
{
    [AnimatorField("animator")] // TODO: just get the animator like openAI
    [SerializeField]
    private BoolAnimationParameter boolParam;

    [SerializeField]
    [AnimatorField("animator")]
    private FloatAnimationParameter floatParam;
    
    [SerializeField]
    private PassiveTimer timer;
    
    [SerializeField]
    private Animator animator;
    
    private void OnValidate()
    {
        string message = $"<color=green>Float: {floatParam.name}</color>, <color=blue>Bool: {boolParam.name}</color>";
        Logger.Log(message, Logger.DontFormat);
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        Logger.Log("Normal Log", this);
        Logger.Log("Colored Log", Color.green, this);
        Logger.LogAssertion("Assertion", this);
        Logger.LogWarning("Warning", this);
        Logger.LogException("Exception", this);
        timer.Start();
    }

    private void Update()
    {
        if (timer.IsActive && timer.IsSet)
            return;
        timer.Clear();
        timer.Start();
        var color = Random.ColorHSV();
        Logger.Log("Timer reached", color);
    }

}
