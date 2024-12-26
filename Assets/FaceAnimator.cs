using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceAnimator : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public AudioSource audioSource;
    
    private readonly int blinkIndex = 13;
    private readonly int mouthOpenIndex = 39;
    private readonly float blinkInterval = 2.8f;
    private float timer;
    private float blinkWeight;
    private float mouthOpenWeight;

    void Start()
    {
        timer = 0f;
        blinkWeight = 0f;
        mouthOpenWeight = 0f;
    }

    void Update()
    {
        // timer
        timer += Time.deltaTime;
        if (timer > blinkInterval)
            timer = 0f;

        // blink
        float durationTime = (timer % blinkInterval) / blinkInterval;

        if (timer == 0)
            blinkWeight = 0;
        if (durationTime < 0.05f)
            blinkWeight = durationTime * 2000;
        else if (durationTime < 0.1f)
            blinkWeight = 100 - (durationTime - 0.05f) * 2000;
        else
            blinkWeight = 0;

        skinnedMeshRenderer.SetBlendShapeWeight(blinkIndex, blinkWeight);

        // mouth open
        float[] clipSampleData = new float[1024];
        audioSource.GetOutputData(clipSampleData, 0);
        float clipLoudness = 0f;
        foreach (var sample in clipSampleData)
            clipLoudness += Mathf.Abs(sample);
        clipLoudness /= 1024;

        mouthOpenWeight = Mathf.Lerp(mouthOpenWeight, (clipLoudness * 400), 0.8f);
        skinnedMeshRenderer.SetBlendShapeWeight(mouthOpenIndex, mouthOpenWeight);
    }
}