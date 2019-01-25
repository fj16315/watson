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
    private List<GraphNode> nodes = new List<GraphNode>();
    private Dictionary<int,List<Relationship>> relationships = new Dictionary<int, List<Relationship>>();

    private int maxId = 0;
    private Stack<int> freeIds = new Stack<int>();

    public MainWindow()
    {
      InitializeComponent();
      ComboBoxItem relationship0 = new ComboBoxItem
      {
        Content = "Contains"
      };
      ComboBoxItem relationship1 = new ComboBoxItem
      {
        Content = "Owns"
      };
      ComboBoxItem relationship2 = new ComboBoxItem
      {
        Content = "Wants"
      };
      comboBoxRelationships.Items.Add(relationship0);
      comboBoxRelationships.Items.Add(relationship1);
      comboBoxRelationships.Items.Add(relationship2);
    }

    public void ButtonClick_AddNewNode(object sender, RoutedEventArgs e)
    {
      GraphNode node = new GraphNode(this.GetNextFreeId(), nodeName.Text);
      this.nodes.Add(node);

      this.AddComboNode(node);
      this.AddSourceListNode(node);

      nodeName.Text = "";
    }

    private void AddComboNode(GraphNode node)
    {
      ComboBoxItem nodeComboItem = new ComboBoxItem
      {
        Content = node.Id.ToString() + " : " + node.Name
      };

      comboBoxNodes.Items.Add(nodeComboItem);
    }

    private void AddSourceListNode(GraphNode node)
    {
      ListBoxItem nodeListItem = new ListBoxItem
      {
        Content = node.Id.ToString() + " : " + node.Name
      };

      nodeList.Items.Add(nodeListItem);
    }

    public void ButtonClick_AddNewRelationshipMapping(object sender, RoutedEventArgs e)
    {
      int source = this.nodes.ElementAt(nodeList.SelectedIndex).Id;
      int destination = this.nodes.ElementAt(comboBoxNodes.SelectedIndex).Id;
      Relationship relationship = new Relationship(destination, 1 << comboBoxRelationships.SelectedIndex);

      if (!this.relationships.ContainsKey(source))
      {
        this.relationships.Add(source, new List<Relationship>());
      }
      this.relationships[source].Add(relationship);

      ListBoxItem relationshipListItem = new ListBoxItem
      {
        Content = this.RelationshipIdToName(relationship.RelationshipId) + " " + this.nodes.ElementAt(comboBoxNodes.SelectedIndex).Name
      };

      relationshipList.Items.Add(relationshipListItem);
    }

    private int GetNextFreeId()
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

    private string RelationshipIdToName(int id)
    {
      if (id == 1) return "Contains";
      if (id == 2) return "Owns";
      if (id == 4) return "Wants";
      return "";
    }

    public void ButtonClick_DeleteNode(object sender, RoutedEventArgs e)
    {
      GraphNode deletedNode = this.nodes.ElementAt(nodeList.SelectedIndex);
      freeIds.Push(deletedNode.Id);
      this.nodes.RemoveAt(nodeList.SelectedIndex);
      comboBoxNodes.Items.RemoveAt(nodeList.SelectedIndex);
      nodeList.Items.RemoveAt(nodeList.SelectedIndex);
    }

    public void ButtonClick_DeleteRelationshipMapping(object sender, RoutedEventArgs e)
    {
      Relationship deletedRelationship = this.relationships[nodeList.SelectedIndex].ElementAt(relationshipList.SelectedIndex);
      this.relationships[nodeList.SelectedIndex].RemoveAt(relationshipList.SelectedIndex);
      relationshipList.Items.RemoveAt(relationshipList.SelectedIndex);
    }

    public void SelectedNode(object sender, RoutedEventArgs e)
    {
      int source = this.nodes.ElementAt(nodeList.SelectedIndex).Id;
      relationshipList.Items.Clear();
      if (this.relationships.ContainsKey(source))
      {
        foreach (Relationship relationship in this.relationships[source])
        {
          ListBoxItem relationshipListItem = new ListBoxItem
          {
            Content = this.RelationshipIdToName(relationship.RelationshipId) + " " + this.nodes.ElementAt(comboBoxNodes.SelectedIndex).Name
          };
          relationshipList.Items.Add(relationshipListItem);
        }
      }
    }

    public void SaveGraph(object sender, RoutedEventArgs e)
    {
      Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
      {
        FileName = "UntitledGraph",
        DefaultExt = ".AIgraph",
        Filter = "AI Knowledge Graphs (.AIgraph)|*.AIgraph"
      };

      Nullable<bool> result = dlg.ShowDialog();

      if (result == true)
      {
        string filename = dlg.FileName;

        KnowledgeGraphBuilder builder = new KnowledgeGraphBuilder(this.nodes.Count);
        foreach (GraphNode n in this.nodes)
        {
          foreach (Relationship r in this.relationships[n.Id])
          {
            Entity source = new Entity(n.Id);
            Entity destination = new Entity(r.Destination);

            //TODO: Combine edges with same source and destination

            Relationships relationships = new Relationships((Relationships.Flags)r.RelationshipId);
            builder.AddEdge(source, destination, relationships);
          }
        }

        Dictionary<Entity, string> entitiyNames = new Dictionary<Entity, string>();
        foreach (GraphNode n in this.nodes)
        {
          entitiyNames.Add(new Entity(n.Id), n.Name);
        }
        builder.AddEntityNames(entitiyNames);

        KnowledgeGraph knowledgeGraph = builder.Build();
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, knowledgeGraph);
        stream.Close();
      }
    }


    public void OpenGraph(object sender, RoutedEventArgs e)
    {
      Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
      {
        DefaultExt = ".AIgraph",
        Filter = "AI Knowledge Graphs (.AIgraph)|*.AIgraph"
      };

      Nullable<bool> result = dlg.ShowDialog();

      if (result == true)
      {
        string filename = dlg.FileName;

        IFormatter formatter = new BinaryFormatter();  
        Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);  
        KnowledgeGraph knowledgeGraph = (KnowledgeGraph) formatter.Deserialize(stream);  
        stream.Close();

        this.nodes.Clear();
        foreach (Entity entity in knowledgeGraph.entity_names.Keys)
        {
          this.nodes.Add(new GraphNode(entity._n, knowledgeGraph.entity_names[entity]));
          //This mysteriously does not work
          //knowledgeGraph.RelationshipsFrom(entity).Select((ent) => !ent.IsNone());
        }

        this.RefreshLists();
      }
    }

    private void RefreshLists()
    {
      nodeList.Items.Clear();
      comboBoxNodes.Items.Clear();
      comboBoxRelationships.Items.Clear();
      relationshipList.Items.Clear();

      foreach (GraphNode node in this.nodes)
      {
        this.AddComboNode(node);
        this.AddSourceListNode(node);
      }
    }
  }

  public class GraphNode
  {
    public int Id { get; }

    public string Name { get; }

    public GraphNode(int id, string name)
    {
      this.Id = id;
      this.Name = name;
    }
  }

  public class Relationship
  {
    public int Destination { get; }

    public int RelationshipId { get; }

    public Relationship(int destination, int relationshipId)
    {
      this.Destination = destination;
      this.RelationshipId = relationshipId;
    }
  }
}
