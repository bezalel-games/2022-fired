using System;
using System.Collections.Generic;
using Avrahamy.EditorGadgets;
using Avrahamy.Math;
using Gilad;
using UnityEngine;

namespace Sentences
{
    [CreateAssetMenu(fileName = "Sentences List", menuName = "Fired/Sentences List", order = 0)]
    public class Sentences : ScriptableObject
    {
        [Tooltip("Takes the first bin with correct percentage, if overlapping")]
        [SerializeField]
        public List<SentencesHolder> sentenceBins;

        [SerializeField]
        public Sentence defaultSentence = "Finished";

        public string GetRandomSentence(float percentage)
        {
            if (sentenceBins.Count == 0)
            {
                return defaultSentence.sentence;
            }

            foreach (var sentencesHolder in sentenceBins)
            {
                if (sentencesHolder.sentences.Count > 0 && sentencesHolder.percentageRange.IsInRange(percentage))
                {
                    
                }
            }

            return "";
        }
        
        public string GetRandomSentence()
        {
            return GetRandomSentence(Flammable.BurningRatio());
        }


    }

    [Serializable]
    public class SentencesHolder
    {
        [SerializeField]
        [MinMaxRange(0, 1)]
        public FloatRange percentageRange = new FloatRange(0, 0.1f);

        [SerializeField]
        public List<Sentence> sentences;

        public Sentence GetRandomSentence()
        {
            return sentences.ChooseRandom();
        }
    }

    [Serializable]
    public class Sentence
    {
        [SerializeField]
        [TextArea]
        public string sentence = "You DED lol";

        public Sentence(string sentence)
        {
            this.sentence = sentence;
        }

        public Sentence()
        {
            
        }

        public static implicit operator Sentence(string sentence)
        {
            return new(sentence);
        }
    }
}
