﻿#pragma checksum "..\..\..\LibraryControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A201CE030064411BFCB9279A7928DF33"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Media_player_skin_V2._0;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Media_player_skin_V2._0 {
    
    
    /// <summary>
    /// LibraryControl
    /// </summary>
    public partial class LibraryControl : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 13 "..\..\..\LibraryControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView LibraryPath;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\LibraryControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private System.Windows.Controls.TreeViewItem PictureTree;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\LibraryControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private System.Windows.Controls.TreeViewItem VideoTree;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\LibraryControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private System.Windows.Controls.TreeViewItem MusicTree;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\LibraryControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddLibrary;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\LibraryControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button DeleteLibrary;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\LibraryControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button OK;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Media player skin V2.0;component/librarycontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\LibraryControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 6 "..\..\..\LibraryControl.xaml"
            ((Media_player_skin_V2._0.LibraryControl)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.LibraryPath = ((System.Windows.Controls.TreeView)(target));
            return;
            case 3:
            this.PictureTree = ((System.Windows.Controls.TreeViewItem)(target));
            return;
            case 4:
            this.VideoTree = ((System.Windows.Controls.TreeViewItem)(target));
            return;
            case 5:
            this.MusicTree = ((System.Windows.Controls.TreeViewItem)(target));
            return;
            case 6:
            this.AddLibrary = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\..\LibraryControl.xaml"
            this.AddLibrary.Click += new System.Windows.RoutedEventHandler(this.AddLibrary_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.DeleteLibrary = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\..\LibraryControl.xaml"
            this.DeleteLibrary.Click += new System.Windows.RoutedEventHandler(this.DeleteLibrary_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.OK = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\..\LibraryControl.xaml"
            this.OK.Click += new System.Windows.RoutedEventHandler(this.OK_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

