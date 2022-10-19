using Proof.Entities.Components;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Proof.DevEnv.Components.EntityComponents
{
    public partial class AudioComponentPanel : UserControl
    {
        private readonly AudioComponent _audioComponent;
        private readonly ChangeHistory _changeHistory;
        private readonly Action<IComponent> _onRemove;

        public AudioComponentPanel(AudioComponent audioComponent, ChangeHistory changeHistory, Action<IComponent> onRemove)
        {
            InitializeComponent();

            _audioComponent = audioComponent;
            _changeHistory = changeHistory;
            _onRemove = onRemove;

            FilePathInput.Text = _audioComponent.Sound.Path;
        }

        private void FilePathInput_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (_audioComponent.Sound.Path == FilePathInput.Text)
            {
                return;
            }

            _changeHistory.RegisterChange();
            _audioComponent.Sound.Path = FilePathInput.Text;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            _onRemove(_audioComponent);
        }
    }
}
