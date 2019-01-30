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

    private Dictionary<int,List<RelationshipDestinationRow>> Relation = new Dictionary<int, List<RelationshipDestinationRow>>();
    private Dictionary<SingleRelation, string> relationNames = new Dictionary<SingleRelation, string>();

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
      comboBoxRelation.Items.Add(relationship0);
      comboBoxRelation.Items.Add(relationship1);
      comboBoxRelation.Items.Add(relationship2);
    }

    public void ButtonClick_AddNewNode(object sender, RoutedEventArgs e)
    {
      this.AddNewNode();
    }

    public void KeyUp_AddNewNode(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Return)
      {
        this.AddNewNode();
      }
    }

    private void AddNewNode()
    {
      Entity node = new Entity(this.GetNextFreeId());
      this.nodes.Add(node);
      this.nodeNames.Add(node, nodeName.Text);

      this.AddComboNode(node);
      this.AddSourceListNode(node);

      nodeName.Text = "";
    }

    private void AddComboNode(Entity node)
    {
      ComboBoxItem nodeComboItem = new ComboBoxItem
      {
        Content = $"{(int)node}:{this.nodeNames[node]}"
      };

      comboBoxNodes.Items.Add(nodeComboItem);
    }

    private void AddSourceListNode(Entity node)
    {
      ListBoxItem nodeListItem = new ListBoxItem
      {
        Content = $"{(int)node}:{this.nodeNames[node]}"
      };

      nodeList.Items.Add(nodeListItem);
    }

    public void ButtonClick_AddNewRelationshipMapping(object sender, RoutedEventArgs e)
    {
      int source = (int)this.nodes.ElementAt(nodeList.SelectedIndex);
      int destination = (int)this.nodes.ElementAt(comboBoxNodes.SelectedIndex);
      RelationshipDestinationRow relationship = new RelationshipDestinationRow(destination, 1 << comboBoxRelation.SelectedIndex);

      if (!this.Relation.ContainsKey(source))
      {
        this.Relation.Add(source, new List<RelationshipDestinationRow>());
      }
      this.Relation[source].Add(relationship);

      ListBoxItem relationshipListItem = new ListBoxItem
      {
        Content = $"{this.RelationshipIdToName(relationship.RelationshipId)} {this.nodeNames[this.nodes.ElementAt(comboBoxNodes.SelectedIndex)]}"
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
      this.DeleteNode();
    }

    public void KeyUp_DeleteNode(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete || e.Key == Key.Back)
      {
        this.DeleteNode();
      }
    }

    private void DeleteNode()
    {
      Entity deletedNode = this.nodes.ElementAt(nodeList.SelectedIndex);
      this.nodeNames.Remove(deletedNode);
      freeIds.Push((int)deletedNode);
      this.nodes.RemoveAt(nodeList.SelectedIndex);
      comboBoxNodes.Items.RemoveAt(nodeList.SelectedIndex);
      nodeList.Items.RemoveAt(nodeList.SelectedIndex);
    }

    public void ButtonClick_DeleteRelationshipMapping(object sender, RoutedEventArgs e)
    {
      this.DeleteRelationshipMapping();
    }

    public void KeyUp_DeleteRelationshipMapping(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete || e.Key == Key.Back)
      {
        this.DeleteRelationshipMapping();
      }
    }

    private void DeleteRelationshipMapping()
    {
      RelationshipDestinationRow deletedRelationship = this.Relation[nodeList.SelectedIndex].ElementAt(relationshipList.SelectedIndex);
      this.Relation[nodeList.SelectedIndex].RemoveAt(relationshipList.SelectedIndex);
      relationshipList.Items.RemoveAt(relationshipList.SelectedIndex);
    }

    public void SelectedNode(object sender, RoutedEventArgs e)
    {
      int source = (int)this.nodes.ElementAtOrDefault(nodeList.SelectedIndex);
      relationshipList.Items.Clear();
      if (this.Relation.ContainsKey(source))
      {
        foreach (RelationshipDestinationRow relationship in this.Relation[source])
        {
          ListBoxItem relationshipListItem = new ListBoxItem
          {
            Content = this.RelationshipIdToName(relationship.RelationshipId) + " " + this.nodeNames[this.nodes.ElementAt(comboBoxNodes.SelectedIndex)]
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
        foreach (Entity source in this.nodes)
        {
          foreach (RelationshipDestinationRow r in this.Relation[(int)source])
          {
            Entity destination = new Entity(r.Destination);

            //TODO: Combine edges with same source and destination

            Relation Relation = new Relation(r.RelationshipId);
            builder.AddEdge(source, Relation, destination);
          }
        }

        builder.AddEntityNames(this.nodeNames);

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
          this.nodes.Add(entity);
          //TODO: THIS IS BROKEN
          //knowledgeGraph.RelationFrom(entity).Select((ent) => !ent.IsNone());
        }
        this.nodeNames = knowledgeGraph.entity_names;

        this.RefreshLists();
      }

    }

    private void RefreshLists()
    {
      nodeList.Items.Clear();
      comboBoxNodes.Items.Clear();
      comboBoxRelation.Items.Clear();
      relationshipList.Items.Clear();

      foreach (Entity node in this.nodes)
      {
        this.AddComboNode(node);
        this.AddSourceListNode(node);
      }
    }

    private void ButtonClick_RelationshipWindow(object sender, RoutedEventArgs e)
    {
      RelationshipEditor relationshipEditor = new RelationshipEditor(this.relationNames);
      relationshipEditor.Show();
    }
  }

  class RelationshipDestinationRow
  {
    public int Destination { get; }

    public int RelationshipId { get; }

    public RelationshipDestinationRow(int destination, int relationshipId)
    {
      this.Destination = destination;
      this.RelationshipId = relationshipId;
    }
  }
}
