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
    private EditorModel model;

    public RelationEditor(MainWindow mainWindow, EditorModel model)
    {
      InitializeComponent();
      this.mainWindow = mainWindow;
      this.model = model;

      foreach (SingleRelation relation in model.relationNames.Keys)
      {
        this.AddPossibleRelation(relation);
      }
    }

    public void ButtonClick_AddRelation(object sender, RoutedEventArgs e)
    {
      int id = this.model.relationValueManager.NextFreeId();
      SingleRelation relation = new SingleRelation(id);
      string relationName = textBox_NewRelationName.Text;

      model.relationNames.Add(relation, relationName);

      this.AddPossibleRelation(relation);


      textBox_NewRelationName.Text = "";

      this.mainWindow.RefreshLists();
    }

    //TODO: Add enter key handler.

    private void AddPossibleRelation(SingleRelation relation)
    {
      ListBoxItem listBoxItem = new ListBoxItem
      {
        Content = $"{(int)relation} : {model.relationNames[relation]}"
      };

      listBox_RelationList.Items.Add(listBoxItem);
    }
  }
}
