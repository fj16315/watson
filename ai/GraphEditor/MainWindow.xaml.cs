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
    private List<Entity> nodes = new List<Entity>();
    private Dictionary<Entity, string> nodeNames = new Dictionary<Entity, string>();

    private Dictionary<Entity,List<RelationDestinationRow>> relations = new Dictionary<Entity, List<RelationDestinationRow>>();
    private Dictionary<SingleRelation, string> relationNames = new Dictionary<SingleRelation, string>();

    private FreeValueManager relationValueManager = new FreeValueManager();
    private FreeValueManager entityValueManager = new FreeValueManager();


    public MainWindow()
    {
      InitializeComponent();
    }

    public void ButtonClick_AddNewNode(object sender, RoutedEventArgs e)
    {
      this.AddNewNode(new Entity(comboBox_PossibleSourceNodes.SelectedIndex));
    }

    public void KeyUp_AddNewNode(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Return)
      {
        this.AddNewNode(new Entity(comboBox_PossibleSourceNodes.SelectedIndex));
      }
    }

    public void MenuItemClick_SaveGraph(object sender, RoutedEventArgs e)
    {
      var graphWriter = new GraphWriter(this.nodes, this.nodeNames, this.relations, this.relationNames);
      graphWriter.SaveGraph();
    }

    /// <summary>
    /// Adds a new entity to the knowledge graph.
    /// </summary>
    /// <param name="entity">The entity to be added.</param>
    private void AddNewNode(Entity entity)
    {
      if (!this.nodes.Contains(entity))
      {
        this.nodes.Add(entity);
        this.AddSourceListNode(entity);
        this.RefreshLists();
      }
    }

    private void AddComboSourceEntity(Entity entity)
    {
      ComboBoxItem sourceComboItem = new ComboBoxItem
      {
        Content = $"{(int)entity} : {this.nodeNames[entity]}"
      };

      comboBox_PossibleSourceNodes.Items.Add(sourceComboItem);
    }

    private void AddComboDestinationEntity(Entity entity)
    {
      ComboBoxItem destinationComboItem = new ComboBoxItem
      {
        Content = $"{(int)entity} : {this.nodeNames[entity]}"
      };

      comboBox_PossibleDestinationNodes.Items.Add(destinationComboItem);
    }

    private void AddComboRelation(SingleRelation relation)
    {
      ComboBoxItem relationComboItem = new ComboBoxItem
      {
        Content = $"{(int)relation} : {this.relationNames[relation]}"
      };

      comboBox_PossibleRelations.Items.Add(relationComboItem);
    }

    private void AddSourceListNode(Entity node)
    {
      ListBoxItem nodeListItem = new ListBoxItem
      {
        Content = $"{(int)node} : {this.nodeNames[node]}"
      };

      nodeList.Items.Add(nodeListItem);
    }

    private void AddRelationListRelation(SingleRelation relation)
    {
      ListBoxItem relationshipListItem = new ListBoxItem
      {
        Content = $"{this.relationNames[relation]} {this.nodeNames[this.nodes.ElementAt(comboBox_PossibleDestinationNodes.SelectedIndex)]}"
      };

      relationList.Items.Add(relationshipListItem);
    }

    public void ButtonClick_AddNewRelationshipMapping(object sender, RoutedEventArgs e)
    {
      Entity source = this.nodes.ElementAt(nodeList.SelectedIndex);
      Entity destination = this.nodes.ElementAt(comboBox_PossibleDestinationNodes.SelectedIndex);
      RelationDestinationRow relation = new RelationDestinationRow(destination, new SingleRelation(comboBox_PossibleRelations.SelectedIndex));

      if (!this.relations.ContainsKey(source))
      {
        this.relations.Add(source, new List<RelationDestinationRow>());
      }
      this.relations[source].Add(relation);

      this.AddRelationListRelation(relation.relation);
    }

    public void ButtonClick_DeleteNode(object sender, RoutedEventArgs e)
    {
      var deletedNode = this.nodes.ElementAt(nodeList.SelectedIndex);
      this.DeleteNode(deletedNode);
    }

    public void KeyUp_DeleteNode(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete || e.Key == Key.Back)
      {
        var deletedNode = this.nodes.ElementAt(nodeList.SelectedIndex);
        this.DeleteNode(deletedNode);
      }
    }

    private void DeleteNode(Entity deletedNode)
    {
      this.nodes.RemoveAt(nodeList.SelectedIndex);
      comboBox_PossibleDestinationNodes.Items.RemoveAt(nodeList.SelectedIndex);
      nodeList.Items.RemoveAt(nodeList.SelectedIndex);
      foreach (List<RelationDestinationRow> relations in this.relations.Values)
      {
        relations.RemoveAll(r => r.destination.Equals(deletedNode));
      }
    }

    public void ButtonClick_DeleteRelationshipMapping(object sender, RoutedEventArgs e)
    {
      var selectedNode = new Entity(nodeList.SelectedIndex);
      var deletedRelation = this.relations[selectedNode].ElementAt(relationList.SelectedIndex);
      this.DeleteRelationshipMapping(deletedRelation);
    }

    public void KeyUp_DeleteRelationshipMapping(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete || e.Key == Key.Back)
      {
        var selectedNode = new Entity(nodeList.SelectedIndex);
        var deletedRelation = this.relations[selectedNode].ElementAt(relationList.SelectedIndex);
        this.DeleteRelationshipMapping(deletedRelation);
      }
    }

    private void DeleteRelationshipMapping(RelationDestinationRow deletedRelation)
    {
      var selectedNode = new Entity(nodeList.SelectedIndex);
      this.relations[selectedNode].RemoveAt(relationList.SelectedIndex);
      relationList.Items.RemoveAt(relationList.SelectedIndex);
    }

    public void SelectedNode(object sender, RoutedEventArgs e)
    {
      Entity source = this.nodes.ElementAtOrDefault(nodeList.SelectedIndex);
      relationList.Items.Clear();
      if (this.relations.ContainsKey(source))
      {
        foreach (RelationDestinationRow relationship in this.relations[source])
        {
          ListBoxItem relationshipListItem = new ListBoxItem
          {
            Content = $"{this.relationNames[relationship.relation]} {this.nodeNames[this.nodes.ElementAt(comboBox_PossibleDestinationNodes.SelectedIndex)]}"
          };
          relationList.Items.Add(relationshipListItem);
        }
      }
    }

    public void RefreshLists()
    {
      nodeList.Items.Clear();
      comboBox_PossibleSourceNodes.Items.Clear();
      comboBox_PossibleDestinationNodes.Items.Clear();
      comboBox_PossibleRelations.Items.Clear();
      relationList.Items.Clear();

      foreach (Entity node in this.nodes)
      {
        this.AddSourceListNode(node);
      }
      foreach (Entity node in this.nodeNames.Keys)
      {
        this.AddComboSourceEntity(node);
      }
      foreach (Entity node in this.nodes)
      {
        this.AddComboDestinationEntity(node);
      }
      foreach (SingleRelation relation in this.relationNames.Keys)
      {
        this.AddComboRelation(relation);
      }
    }

    private void ButtonClick_RelationshipWindow(object sender, RoutedEventArgs e)
    {
      RelationEditor relationshipEditor = new RelationEditor(this, this.relationValueManager, this.relationNames);
      relationshipEditor.Show();
    }

    private void ButtonClick_EntityWindow(object sender, RoutedEventArgs e)
    {
      EntityEditor entityEditor = new EntityEditor(this, this.entityValueManager, this.nodeNames);
      entityEditor.Show();
    }
  }
  
  /// <summary>
  /// Utility class that stores single relations and a destination. 
  /// Represents one arrow in the knowledge graph and one line in the editor relation listbox.
  /// </summary>
  public class RelationDestinationRow
  {
    public Entity destination { get; }

    public SingleRelation relation { get; }

    public RelationDestinationRow(Entity destination, SingleRelation relation)
    {
      this.destination = destination;
      this.relation = relation;
    }
  }
}
