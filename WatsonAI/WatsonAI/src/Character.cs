﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonAI
{
  /// <summary>
  /// Basic overview of character containing information for game mechanics.
  /// </summary>
  public class Character
  {
    public string Name { get; }
    
    public bool Murderer { get; }

    public Knowledge Knowledge { get; set; }

    /// <summary>
    /// Constructor for a character.
    /// </summary>
    /// <param name="name">Thier name.</param>
    /// <param name="murderer">Whether they are the murderer or not.</param>
    public Character(string name, bool murderer)
    {
      this.Name = name;
      this.Murderer = murderer;
      this.Knowledge = new Knowledge();
    }
  }
}
