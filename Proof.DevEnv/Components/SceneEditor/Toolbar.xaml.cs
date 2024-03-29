﻿using Proof.Core;
using Proof.DevEnv.Exporting;
using Proof.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace Proof.DevEnv.Components
{
    public partial class Toolbar : UserControl
    {
        private Scene? _scene;
        private AssemblyWrapper? _scriptAssembly;
        private UIElement? _focusKill;
        private ChangeHistory? _changeHistory;

        public Toolbar()
        {
            InitializeComponent();
        }

        public void Init(Scene? scene, AssemblyWrapper scriptAssembly, UIElement focusKill, ChangeHistory? changeHistory)
        {
            _scene = scene;
            _scriptAssembly = scriptAssembly;
            _focusKill = focusKill;
            _changeHistory = changeHistory;
        }

        public IEnumerable<CommandBinding> GetCommandBindings()
        {
            return new List<CommandBinding>
            {
                GenerateCommandBinding(Key.S, ModifierKeys.Control, Save_Click),
                GenerateCommandBinding(Key.S, ModifierKeys.Control | ModifierKeys.Shift, SaveAs_Click),
                GenerateCommandBinding(Key.F5, ModifierKeys.None, Run_Click),
                GenerateCommandBinding(Key.B, ModifierKeys.Control, Build_Click),
            };
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if(_scene == null)
            {
                return;
            }

            KillFocus();
            _changeHistory?.RegisterSave();
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
            KillFocus();
            // Register save here? Potentially want to update the filepath the scene points at
            _scene.Save(filePath);
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            KillFocus();
            BuildProject();
            Process.Start(Path.Combine(Directory.GetCurrentDirectory(), "Proof.Runner.exe"));
        }

        private void Build_Click(object sender, RoutedEventArgs e)
        {
            KillFocus();
            BuildProject();
        }

        private void BuildDependencies_Click(object sender, RoutedEventArgs e)
        {
            KillFocus();
            Exporter.OutputRequiredFiles(Directory.GetCurrentDirectory());
        }

        private void BuildProject()
        {
            if(_scene == null)
            {
                return;
            }

            _scene.Save(_scene.FilePath);

            Exporter.OutputRequiredFiles(Directory.GetCurrentDirectory());

            var exporter = new Exporter(
                new Compiler()
                    .WithAdditionalReferences(
                        "Proof.dll",
                        "Proof.OpenGL.dll",
                        "GLFW.NET.dll",
                        "System.Numerics.dll",
                        "System.Numerics.Vectors.dll"),
                new EntryPointGenerator("Proof Game"));
            
            List<string> errorList = exporter.BuildGameDll(Directory.GetCurrentDirectory(), "Game.dll");

            if (errorList.Any())
            {
                string messageBoxText = errorList.Aggregate("", (current, next) => $"{current}\n{next}");
                string caption = "CompileResults";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            }

            if (_scriptAssembly == null)
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

        private void KillFocus()
        {
            if(_focusKill == null)
            {
                return;
            }

            _focusKill.Focus();
            Keyboard.ClearFocus();
        }
    }
}
