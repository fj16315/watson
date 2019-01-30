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
    private FreeValueManager freeValueManager;
    private Dictionary<Entity, string> entityNames;

    public EntityEditor(MainWindow mainWindow, FreeValueManager freeValueManager,
      Dictionary<Entity,string> entityNames)
    {
      InitializeComponent();
      this.mainWindow = mainWindow;
      this.freeValueManager = freeValueManager;
      this.entityNames = entityNames;

      foreach (Entity entity in entityNames.Keys)
      {
        this.AddPossibleEntity(entity);
      }
    }

    public void ButtonClick_AddEntity(object sender, RoutedEventArgs e)
    {
      int id = freeValueManager.NextFreeId();
      Entity entity = new Entity(id);
      string entityName = textBox_NewEntityName.Text;

      this.entityNames.Add(entity, entityName);

      this.AddPossibleEntity(entity);

      textBox_NewEntityName.Text = "";

      this.mainWindow.RefreshLists();
    }

    //TODO: Add enter key handler.

    private void AddPossibleEntity(Entity entity)
    {
      ListBoxItem listBoxItem = new ListBoxItem
      {
        Content = $"{(int)entity} : {this.entityNames[entity]}"
      };

      listBox_EntityList.Items.Add(listBoxItem);
    }
  }
}
