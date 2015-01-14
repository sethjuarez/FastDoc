using FastDoc.Core;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastDoc.Visualizer
{
    public class NodeVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            if (windowService == null)
                throw new ArgumentNullException("windowService");
            if (objectProvider == null)
                throw new ArgumentNullException("objectProvider");
            if (!(objectProvider.GetObject() is Node))
                throw new ArgumentException("Object type is not StructureNode");

            var data = objectProvider.GetObject() as Node;

            using(TreeForm f = new TreeForm())
            {
                f.Text = data.Name;
                f.LoadNode(data);
                windowService.ShowDialog(f);
            }
        }


        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(NodeVisualizer));
            visualizerHost.ShowVisualizer();
        }
    }
}
