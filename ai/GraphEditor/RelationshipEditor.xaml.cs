using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GameAI;

namespace GraphEditor
{
  /// <summary>
  /// Interaction logic for RelationshipEditor.xaml
  /// </summary>
  public partial class RelationshipEditor : Window
  {

    private Dictionary<SingleRelation, string> relationNames;

    public RelationshipEditor(Dictionary<SingleRelation,string> relationNames)
    {
      InitializeComponent();
      this.relationNames = relationNames;
    }
  }
}
