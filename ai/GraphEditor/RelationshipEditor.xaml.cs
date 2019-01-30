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
  public partial class RelationEditor : Window
  {
    private MainWindow mainWindow;
    private FreeValueManager freeValueManager;
    private Dictionary<SingleRelation, string> relationNames;

    public RelationEditor(MainWindow mainWindow, FreeValueManager freeValueManager, 
      Dictionary<SingleRelation,string> relationNames)
    {
      InitializeComponent();
      this.mainWindow = mainWindow;
      this.freeValueManager = freeValueManager;
      this.relationNames = relationNames;

      foreach (SingleRelation relation in relationNames.Keys)
      {
        this.AddPossibleRelation(relation);
      }
    }

    public void ButtonClick_AddRelation(object sender, RoutedEventArgs e)
    {
      int id = freeValueManager.NextFreeId();
      SingleRelation relation = new SingleRelation(id);
      string relationName = textBox_NewRelationName.Text;

      this.relationNames.Add(relation, relationName);

      this.AddPossibleRelation(relation);


      textBox_NewRelationName.Text = "";

      this.mainWindow.RefreshLists();
    }

    //TODO: Add enter key handler.

    private void AddPossibleRelation(SingleRelation relation)
    {
      ListBoxItem listBoxItem = new ListBoxItem
      {
        Content = $"{(int)relation} : {this.relationNames[relation]}"
      };

      listBox_RelationList.Items.Add(listBoxItem);
    }
  }
}
