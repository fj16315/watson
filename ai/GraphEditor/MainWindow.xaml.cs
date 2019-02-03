using GameAI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphEditor
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private EditorModel model; 

    private List<Entity> entities;
    private Dictionary<Entity, List<RelationDestinationRow>> relations;

    private RelationEditor relationshipEditor;
    private EntityEditor entityEditor;

    public MainWindow()
    {
      InitializeComponent();

      this.model = new EditorModel();
      this.entities = new List<Entity>();
      this.relations = new Dictionary<Entity, List<RelationDestinationRow>>();
      this.relationshipEditor = new RelationEditor(this, this.model);
      this.entityEditor = new EntityEditor(this, this.model);
    }

    //---------------------------------------------------------------------------------
    // UI Handlers
    //---------------------------------------------------------------------------------

    public void ButtonClick_AddNewEntity(object sender, RoutedEventArgs e)
      => this.AddNewEntity(comboBox_PossibleSourceEntities.SelectedIndex);

    public void KeyUp_AddNewEntity(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Return)
      {
        this.AddNewEntity(comboBox_PossibleSourceEntities.SelectedIndex);
      }
    }

    public void MenuItemClick_SaveGraph(object sender, RoutedEventArgs e)
      => model.SaveGraph();

    public void ButtonClick_AddNewRelationshipMapping(object sender, RoutedEventArgs e)
    {
      Entity source = this.entities[entityList.SelectedIndex];
      Entity destination = this.entities[comboBox_PossibleDestinationEntity.SelectedIndex];
      SingleRelation relation =  new SingleRelation(comboBox_PossibleRelations.SelectedIndex);

      model.AddNewRelationshipMapping(source, relation, destination);

      this.AddRelationListRelation(relation);
    }

    public void ButtonClick_DeleteEntity(object sender, RoutedEventArgs e)
    {
      var deletedEntity = this.entities[entityList.SelectedIndex];
      model.DeleteEntity(deletedEntity);
    }

    public void KeyUp_DeleteEntity(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete || e.Key == Key.Back)
      {
        var deletedEntity = this.entities[entityList.SelectedIndex];
        model.DeleteEntity(deletedEntity);
      }
    }

    public void ButtonClick_DeleteRelationshipMapping(object sender, RoutedEventArgs e)
    {
      var selectedEntity = this.entities[entityList.SelectedIndex];
      var deletedRelation = this.relations[selectedEntity][relationList.SelectedIndex];
      this.DeleteRelationshipMapping(deletedRelation);
    }

    public void KeyUp_DeleteRelationshipMapping(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete || e.Key == Key.Back)
      {
        var selectedEntity = this.entities[entityList.SelectedIndex];
        var deletedRelation = this.relations[selectedEntity][relationList.SelectedIndex];
        this.DeleteRelationshipMapping(deletedRelation);
      }
    }

    public void SelectedEntity(object sender, RoutedEventArgs e)
    {
      Entity source = entities[entityList.SelectedIndex];
      relationList.Items.Clear();
      if (this.relations.ContainsKey(source))
      {
        foreach (RelationDestinationRow relationship in this.relations[source])
        {
          this.AddRelationListRelation(relationship.relation);
        }
      }
    }

    public void ButtonClick_RelationshipWindow(object sender, RoutedEventArgs e)
    {
      relationshipEditor.Show();
    }

    public void ButtonClick_EntityWindow(object sender, RoutedEventArgs e)
    {
      entityEditor.Show();
    }

    //---------------------------------------------------------------------------------
    // UI Element Constructors
    //---------------------------------------------------------------------------------

    private void AddComboSourceEntity(Entity entity)
    {
      ComboBoxItem sourceComboItem = new ComboBoxItem
      {
        Content = $"{(int)entity} : {model.entityNames[entity]}"
      };

      comboBox_PossibleSourceEntities.Items.Add(sourceComboItem);
    }

    private void AddComboDestinationEntity(Entity entity)
    {
      ComboBoxItem destinationComboItem = new ComboBoxItem
      {
        Content = $"{(int)entity} : {model.entityNames[entity]}"
      };

      comboBox_PossibleDestinationEntity.Items.Add(destinationComboItem);
    }

    private void AddComboRelation(SingleRelation relation)
    {
      ComboBoxItem relationComboItem = new ComboBoxItem
      {
        Content = $"{(int)relation} : {model.relationNames[relation]}"
      };

      comboBox_PossibleRelations.Items.Add(relationComboItem);
    }

    private void AddSourceListEntity(Entity entity)
    {
      ListBoxItem entityListItem = new ListBoxItem
      {
        Content = $"{(int)entity} : {model.entityNames[entity]}"
      };

      entityList.Items.Add(entityListItem);
    }

    private void AddRelationListRelation(SingleRelation relation)
    {
      Entity destination = this.entities[comboBox_PossibleDestinationEntity.SelectedIndex];
      ListBoxItem relationshipListItem = new ListBoxItem
      {
        Content = $"{model.relationNames[relation]} {model.entityNames[destination]}"
      };

      relationList.Items.Add(relationshipListItem);
    }
    
    //---------------------------------------------------------------------------------
    // Controller Functionality 
    //---------------------------------------------------------------------------------

    /// <summary>
    /// Adds a new entity to the knowledge graph.
    /// </summary>
    /// <param name="entity">The entity to be added.</param>
    private void AddNewEntity(int id)
    {
      Entity entity = new Entity(id);
      if (!model.ContainsEntity(entity))
      {
        model.AddNewEntity(entity);
        this.entities.Add(entity);
        this.AddSourceListEntity(entity);
        this.RefreshLists();
      }
    }

    /// <summary>
    /// Deletes an entity.
    /// </summary>
    /// <param name="deletedEntity">The entity to be deleted.</param>
    private void DeleteEntity(Entity deletedEntity)
    {
      model.DeleteEntity(deletedEntity);
      this.entities.RemoveAt(entityList.SelectedIndex);
      comboBox_PossibleDestinationEntity.Items.RemoveAt(entityList.SelectedIndex);
      entityList.Items.RemoveAt(entityList.SelectedIndex);
      foreach (List<RelationDestinationRow> relations in this.relations.Values)
      {
        relations.RemoveAll(r => r.destination.Equals(deletedEntity));
      }
    }

    /// <summary>
    /// Deletes a relationship mapping.
    /// </summary>
    /// <param name="deletedRelation">The destination entity and relation to be deleted.</param>
    private void DeleteRelationshipMapping(RelationDestinationRow deletedRelation)
    {
      var selectedEntity = this.entities[entityList.SelectedIndex];
      model.DeleteRelationshipMapping(selectedEntity, deletedRelation);
      this.relations[selectedEntity].RemoveAt(relationList.SelectedIndex);
      relationList.Items.RemoveAt(relationList.SelectedIndex);
    }

    /// <summary>
    /// Refreshes all the UI lists.
    /// </summary>
    public void RefreshLists()
    {
      entityList.Items.Clear();
      comboBox_PossibleSourceEntities.Items.Clear();
      comboBox_PossibleDestinationEntity.Items.Clear();
      comboBox_PossibleRelations.Items.Clear();
      relationList.Items.Clear();

      foreach (Entity entity in this.entities)
      {
        this.AddSourceListEntity(entity);
      }
      foreach (Entity entity in this.entities)
      {
        this.AddComboDestinationEntity(entity);
      }
      foreach (Entity entity in model.entityNames.Keys)
      {
        this.AddComboSourceEntity(entity);
      }
      foreach (SingleRelation relation in model.relationNames.Keys)
      {
        this.AddComboRelation(relation);
      }
    }

  }
  
  /// <summary>
  /// Utility class that stores single relations and a destination. 
  /// Represents one arrow in the knowledge graph and one line in the editor relation listbox.
  /// </summary>
  public struct RelationDestinationRow
  {
    public Entity destination { get; }

    public SingleRelation relation { get; }

    public RelationDestinationRow(Entity destination, SingleRelation relation)
    {
      this.destination = destination;
      this.relation = relation;
    }

    /// <summary>
    /// Equals method generated by visual studio.
    /// </summary>
    /// <param name="obj">Object to be compared.</param>
    /// <returns>True if obj is equal to the relation destination row.</returns>
    public override bool Equals(object obj)
    {
      if (!(obj is RelationDestinationRow))
      {
        return false;
      }

      var row = (RelationDestinationRow)obj;
      return EqualityComparer<Entity>.Default.Equals(destination, row.destination) &&
             EqualityComparer<SingleRelation>.Default.Equals(relation, row.relation);
    }

    /// <summary>
    /// HashCode method generated by visual studio.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
      var hashCode = 521575480;
      hashCode = hashCode * -1521134295 + EqualityComparer<Entity>.Default.GetHashCode(destination);
      hashCode = hashCode * -1521134295 + EqualityComparer<SingleRelation>.Default.GetHashCode(relation);
      return hashCode;
    }
  }
}
