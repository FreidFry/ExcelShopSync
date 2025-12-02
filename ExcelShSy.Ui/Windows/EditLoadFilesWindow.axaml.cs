using Avalonia.Controls;
using ExcelShSy.Core.Helpers;
using ExcelShSy.Core.Interfaces.ViewModels;

namespace ExcelShSy.Ui.Windows
{
    public partial class EditLoadFilesWindow : Window
    {

#if DESIGNER
        public EditLoadFilesWindow(IEditLoadFilesModel vm)
        {
            InitializeComponent();

            DataContext = vm;
        }
#endif


    }
}
