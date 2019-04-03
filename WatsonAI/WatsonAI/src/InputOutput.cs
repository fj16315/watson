using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonAI
{
  /// <summary>
  /// Stores the state of the input text processing.
  /// </summary>
  public struct InputOutput
  {
    public string input { get; }

    public string remainingInput { get; set; }
      
    public string output { get; set; }

    /// <summary>
    /// Constructs a new InputOutput object with full remaining input and empty output.
    /// </summary>
    /// <param name="input">The input string.</param>
    public InputOutput(string input)
    {
      this.input = input;
      this.remainingInput = input;
      this.output = "";
    }

    /// <summary>
    /// Calls the rule on itself, for nice syntax.
    /// </summary>
    /// <param name="rule">The rule to apply.</param>
    /// <returns>A InputOutput object with fields mutated.</returns>
    public InputOutput Process(IRule rule)
    {
      return rule.Process(this);
    }
  }
}
