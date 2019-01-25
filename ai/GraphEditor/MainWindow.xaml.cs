using GameAI;
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
        Content = "None"
      };
      ComboBoxItem relationship1 = new ComboBoxItem
      {
        Content = "Contains"
      };
      ComboBoxItem relationship2 = new ComboBoxItem
      {
        Content = "Owns"
      };
      ComboBoxItem relationship3 = new ComboBoxItem
      {
        Content = "Wants"
      };
      comboBoxRelationships.Items.Add(relationship0);
      comboBoxRelationships.Items.Add(relationship1);
      comboBoxRelationships.Items.Add(relationship2);
      comboBoxRelationships.Items.Add(relationship3);
    }

    public void ButtonClick_AddNewNode(object sender, RoutedEventArgs e)
    {
      GraphNode node = new GraphNode(this.GetNextFreeId(), nodeName.Text);
      this.nodes.Add(node);

      ListBoxItem nodeListItem = new ListBoxItem
      {
        Content = node.Id.ToString() + " : " + node.Name
      };

      ComboBoxItem nodeComboItem = new ComboBoxItem
      {
        Content = node.Id.ToString() + " : " + node.Name
      };

      nodeList.Items.Add(nodeListItem);
      comboBoxNodes.Items.Add(nodeComboItem);

      nodeName.Text = "";
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
      if (id == 1) return "None";
      if (id == 2) return "Contains";
      if (id == 4) return "Owns";
      if (id == 8) return "Wants";
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
        string[] lines = { "First line", "Second line", "Third line" };
        System.IO.File.WriteAllLines(filename, lines);
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
