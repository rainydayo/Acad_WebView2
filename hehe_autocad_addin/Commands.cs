#if ZWCAD

using ZwSoft.ZwCAD.Runtime;
using ZwSoft.ZwCAD.ApplicationServices;
using ZwSoft.Windows;

#else

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Windows;
#endif


using System;
using System.Linq;
using System.Windows.Input;
using System.Windows.Forms;

[assembly: ExtensionApplication(typeof(hehe_autocad_addin.Commands))]
[assembly: CommandClass(typeof(hehe_autocad_addin.Commands))]

namespace hehe_autocad_addin
{
    public class Commands : IExtensionApplication
    {
        public void Initialize()
        {
            AddRibbonPanel();
        }

        public void Terminate()
        {
            // Clean-up if needed
        }

        private void AddRibbonPanel()
        {
            var ribbonControl = ComponentManager.Ribbon;
            if (ribbonControl == null) return;


            string tabName = "Add-ins";

#if ZWCAD
            tabName = "APP+";
#endif

            // Find the built-in "Add-ins" tab by Title
            var tab = ribbonControl.Tabs.FirstOrDefault(t =>
                t.Title.Equals(tabName, StringComparison.InvariantCultureIgnoreCase)
            );
            if (tab == null)
            {
                // Create an Add-ins tab if not found
                tab = new RibbonTab
                {
                    Title = tabName,
                    Id = "MyAddInsTab"
                };
                ribbonControl.Tabs.Add(tab);
            }

            // Create a new panel under the tab
            var panelSource = new RibbonPanelSource
            {
                Title = "hehe",
                Id = "hehePanelSource"
            };
            var panel = new RibbonPanel
            {
                Source = panelSource,
                Id = "hehePanel"
            };
            tab.Panels.Add(panel);

            // Create a button on the panel
            var button = new RibbonButton
            {
                Id = "heheButton",
                Text = "Open WebView",
                ShowText = true,
                Orientation = System.Windows.Controls.Orientation.Vertical,
                Size = RibbonItemSize.Large,
                CommandHandler = new RibbonCommandHandler()
            };
            panelSource.Items.Add(button);
        }
    }

    // ICommand implementation to handle button clicks
    public class RibbonCommandHandler : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            // Show the WinForms modeless window and keep it on top
            var form = new Form1
            {
                TopMost = true
            };
            // Bring it to front once shown
            form.Shown += (s, e) => form.BringToFront();
            form.Show();
        }
    }
}
