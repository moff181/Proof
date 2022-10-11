﻿using Proof.Core;
using Proof.DevEnv.Exporting;
using Proof.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using UserControl = System.Windows.Controls.UserControl;

namespace Proof.DevEnv.Components
{
    public partial class Toolbar : UserControl
    {
        private Scene? _scene;
        private AssemblyWrapper? _scriptAssembly;

        public Toolbar()
        {
            InitializeComponent();
        }

        public void Init(Scene? scene, AssemblyWrapper scriptAssembly)
        {
            _scene = scene;
            _scriptAssembly = scriptAssembly;
        }

        public IEnumerable<CommandBinding> GetCommandBindings()
        {
            return new List<CommandBinding>
            {
                GenerateCommandBinding(Key.S, ModifierKeys.Control, Save_Click),
                GenerateCommandBinding(Key.S, ModifierKeys.Control | ModifierKeys.Shift, SaveAs_Click),
            };
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if(_scene == null)
            {
                return;
            }

            _scene.Save(_scene.FilePath);
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            if(_scene == null)
            {
                return;
            }

            using var dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.DefaultExt = "scene";
            dialog.Filter = "Scene file (*.scene)|*.scene";
            if(dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string filePath = dialog.FileName;
            _scene.Save(filePath);
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            BuildProject();

            Process.Start("Proof.Runner.exe");
        }

        private void Build_Click(object sender, RoutedEventArgs e)
        {
            BuildProject();
        }

        private void BuildDependencies_Click(object sender, RoutedEventArgs e)
        {
            Exporter.OutputRequiredFiles(Directory.GetCurrentDirectory());
        }

        private void BuildProject()
        {
            var exporter = new Exporter(
                new Compiler()
                    .WithAdditionalReferences(
                        "Proof.dll",
                        "Proof.OpenGL.dll",
                        "GLFW.NET.dll",
                        "System.Numerics.dll",
                        "System.Numerics.Vectors.dll"),
                new EntryPointGenerator("Proof Game"));
            exporter.BuildGameDll(Directory.GetCurrentDirectory(), "Game.dll");

            if(_scriptAssembly == null)
            {
                return;
            }

            _scriptAssembly.Reload();
        }

        private static CommandBinding GenerateCommandBinding(Key key, ModifierKeys modifiers, Action<object, RoutedEventArgs> action)
        {
            var cmd = new RoutedCommand();
            cmd.InputGestures.Add(new KeyGesture(key, modifiers));
            return new CommandBinding(cmd, (x, y) => action(x, y));
        }
    }
}
