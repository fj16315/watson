namespace WatsonAI
{
  /// <summary>
  /// Interface for processing the InputOutput class.
  /// </summary>
  public interface IRule
  {
    InputOutput Process(InputOutput io);
  }
}
