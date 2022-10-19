using Proof.Entities.Components;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Proof.DevEnv.Components.SceneEditor.EntityComponents
{
    public partial class AudioComponentPanel : UserControl
    {
        private AudioComponent? _audioComponent;
        private ChangeHistory? _changeHistory;
        private Action<IComponent>? _onRemove;

        public AudioComponentPanel()
        {
            InitializeComponent();
        }

        public void Init(AudioComponent audioComponent, ChangeHistory changeHistory, Action<IComponent> onRemove)
        {
            _audioComponent = audioComponent;
            _changeHistory = changeHistory;
            _onRemove = onRemove;

            FilePathInput.Text = _audioComponent.Sound.Path;
        }

        private void FilePathInput_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if(_changeHistory == null || _audioComponent == null)
            {
                return;
            }

            if (_audioComponent.Sound.Path == FilePathInput.Text)
            {
                return;
            }

            _changeHistory.RegisterChange();
            _audioComponent.Sound.Path = FilePathInput.Text;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (_audioComponent == null || _onRemove == null)
            {
                return;
            }

            _onRemove(_audioComponent);
        }
    }
}
