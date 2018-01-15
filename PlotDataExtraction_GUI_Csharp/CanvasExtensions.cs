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
using System.Data;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;

namespace PlotDataExtraction_GUI_Csharp
{
    public static class CanvasExtensions
    { //http://stackoverflow.com/questions/10835755/delete-all-images-added-to-canvas

        public static void Remove<T>(this UIElementCollection targetCollection)
        {
            // This will loop to the end of the children collection.
            int index = 0;

            // Loop over every element in the children collection.
            while (index < targetCollection.Count)
            {
                // Remove the item if it's of type T
                if (targetCollection[index] is T)
                    targetCollection.RemoveAt(index);
                else
                    index++;
            }
        }

    }
}
