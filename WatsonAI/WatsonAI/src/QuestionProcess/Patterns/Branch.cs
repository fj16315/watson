using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  public class Branch : Pattern<Parse>
  {
    private readonly string branch;

    public Branch(string branch)
    {
      this.branch = branch;
    }

    public override Result<Parse> Match(Parse tree)
      => tree.Type == branch ? new Result<Parse>(tree) : new Result<Parse>();
  }
}
