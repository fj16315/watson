using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphEditor
{
  /// <summary>
  /// Keeps track of free values so the next id can be assigned.
  /// </summary>
  public class FreeValueManager
  {
    private int maxId = 0;
    private Stack<int> freeIds = new Stack<int>();

    /// <summary>
    /// Gets the next available ID.
    /// </summary>
    /// <returns>The next available ID.</returns>
    public int NextFreeId()
    {
      if (this.freeIds.Any())
      {
        return this.freeIds.Pop();

      }
      else
      {
        int id = this.maxId;
        this.maxId++;
        return id;
      }
    }

    /// <summary>
    /// Called when an ID becomes free.
    /// </summary>
    /// <param name="id">The ID that has been freed.</param>
    public void FreeValue(int id)
    {
      freeIds.Push(id);
    }
  }
}
