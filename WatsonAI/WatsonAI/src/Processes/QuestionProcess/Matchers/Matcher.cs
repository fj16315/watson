using OpenNLP.Tools.Parser;

namespace WatsonAI
{
  interface IMatcher
  {
    bool MatchOn(Parse tree);

    string GenerateResponse();
  }
}
