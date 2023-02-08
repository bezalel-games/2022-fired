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
    [CreateAssetMenu(fileName = "Sentences List", menuName = "Fired/Sentences List", order = 0)]
    public class Sentences : ScriptableObject
    {
        [Tooltip("Takes the first bin with correct percentage, if overlapping")]
        [SerializeField]
        [InlineScriptableObject]
        public List<SentencesHolder> sentenceBins;

        [SerializeField]
        public Sentence defaultSentence;

        public Sentence GetRandomSentence(float percentage)
        {
            foreach (var sentencesHolder in sentenceBins)
            {
                if (sentencesHolder.sentences.Count > 0 && sentencesHolder.percentageRange.IsInRange(percentage))
                {
                    return sentencesHolder.GetRandomSentence();
                }
            }

            return defaultSentence;
        }
        
        public Sentence GetRandomSentence()
        {
            return GetRandomSentence(Flammable.BurningRatio());
        }


    }

    [Serializable]
    public class Sentence
    {
        [SerializeField]
        [TextArea]
        public string sentence = "You DED lol";

        [SerializeField]
        public Sprite sentenceAsImage;

        // public Sentence(string sentence)
        // {
        //     this.sentence = sentence;
        // }
        //
        // public Sentence()
        // {
        //     
        // }
        //
        // public static implicit operator Sentence(string sentence)
        // {
        //     return new(sentence);
        // }
    }
}
