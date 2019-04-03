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

        public Character GetEnum()
        {
            switch (name.ToLower())
            {
                case "actress":
                    return Character.ACTRESS;
                case "butler":
                    return Character.BUTLER;
                case "colonel":
                    return Character.COLONEL;
                case "countess":
                    return Character.COUNTESS;
                case "earl":
                    return Character.EARL;
                case "gangster":
                    return Character.GANGSTER;
                case "police":
                    return Character.POLICE;
            }
            return Character.POLICE; // Default
        }

    }
}

