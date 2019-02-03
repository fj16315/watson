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
  /// Interaction logic for EntityEditor.xaml
  /// </summary>
  public partial class EntityEditor : Window
  {
    private MainWindow mainWindow;
    private EditorModel model;

    public EntityEditor(MainWindow mainWindow, EditorModel model)
    {
      InitializeComponent();
      this.mainWindow = mainWindow;
      this.model = model;

      foreach (Entity entity in model.entityNames.Keys)
      {
        this.AddPossibleEntity(entity);
      }
    }

    public void ButtonClick_AddEntity(object sender, RoutedEventArgs e)
    {
      int id = model.entityValueManager.NextFreeId();
      Entity entity = new Entity(id);
      string entityName = textBox_NewEntityName.Text;

      model.entityNames.Add(entity, entityName);

      this.AddPossibleEntity(entity);

      textBox_NewEntityName.Text = "";

      this.mainWindow.RefreshLists();
    }

    //TODO: Add enter key handler.

    private void AddPossibleEntity(Entity entity)
    {
      ListBoxItem listBoxItem = new ListBoxItem
      {
        Content = $"{(int)entity} : {model.entityNames[entity]}"
      };

      listBox_EntityList.Items.Add(listBoxItem);
    }
  }
}
