using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public enum Character : int { ACTRESS, BUTLER, COLONEL, COUNTESS, EARL, GANGSTER, POLICE };

    public class NPCController : MonoBehaviour
    {

        public string charName;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public NPC.Character GetEnum()
        {
            switch (charName)
            {
                case "Actress":
                    return Character.ACTRESS;
                case "Butler":
                    return Character.BUTLER;
                case "Colonel":
                    return Character.COLONEL;
                case "Countess":
                    return Character.COUNTESS;
                case "Earl":
                    return Character.EARL;
                case "Gangster":
                    return Character.GANGSTER;
                case "Police":
                    return Character.POLICE;
            }
            return Character.POLICE; // Default
        }

    }
}

