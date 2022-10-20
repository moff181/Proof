using Proof.Entities.Components;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Proof.DevEnv.Components.EntityComponents
{
    public partial class TextureComponentPanel : UserControl
    {
        private readonly TextureComponent _textureComponent;
        private readonly ChangeHistory _changeHistory;
        private readonly Action<IComponent> _onRemove;

        public TextureComponentPanel(
            TextureComponent textureComponent,
            ChangeHistory changeHistory,
            Action<IComponent> onRemove)
        {
            InitializeComponent();

            _textureComponent = textureComponent;
            _changeHistory = changeHistory;
            _onRemove = onRemove;
        }

        private void TexturePath_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if(TexturePath.Text == _textureComponent.Texture.FilePath)
            {
                return;
            }

            _changeHistory.RegisterChange();
            _textureComponent.Texture.FilePath = TexturePath.Text;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            _onRemove(_textureComponent);
        }
    }
}
