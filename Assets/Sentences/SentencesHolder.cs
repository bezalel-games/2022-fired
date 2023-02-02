using System;
using System.Collections.Generic;
using Avrahamy.EditorGadgets;
using Avrahamy.Math;
using BitStrap;
using Gilad;
using UnityEngine;
using FloatRange = Avrahamy.Math.FloatRange;

namespace Sentences
{
    [CreateAssetMenu(fileName = "Sentences Holder", menuName = "Fired/Sentences Holder", order = 0)]
    public class SentencesHolder : ScriptableObject
    {
        [SerializeField]
        [MinMaxRange(0f, 1f)]
        public FloatRange percentageRange = new FloatRange(0f, 0.1f);

        [SerializeField]
        public List<Sentence> sentences;

        public Sentence GetRandomSentence()
        {
            return sentences.ChooseRandom();
        }
    }
}
