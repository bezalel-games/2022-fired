using System;
using System.Collections;
using System.Collections.Generic;
using Avrahamy.Audio;
using Avrahamy.Utils;
using BitStrap;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    [SerializeField]
    private AudioEvent audioEvent;

    [SerializeField]
    private float timer = 10f;

    private IEnumerator _timer;

    private void Start()
    {
        StartCoroutine(CoroutineUtils.Repeat(Play, timer));
    }

    private IEnumerable PlayOnTime(float time)
    {
        yield return new WaitForSeconds(time);
        Play();
    }

    [Button]
    private void Play()
    {
        audioEvent.Play(transform);
    }

}
