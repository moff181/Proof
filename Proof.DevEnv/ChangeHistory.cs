using System;

namespace Proof.DevEnv
{
    public class ChangeHistory
    {
        private readonly Action _onUnsavedChange;
        private readonly Action _onSave;
        private bool _unsavedChange;

        public ChangeHistory(Action onUnsavedChange, Action onSave)
        {
            _onUnsavedChange = onUnsavedChange;
            _onSave = onSave;
        }
        
        public void RegisterChange()
        {
            if(_unsavedChange)
            {
                return;
            }

            _unsavedChange = true;
            _onUnsavedChange();
        }

        public void RegisterSave()
        {
            _unsavedChange = false;
            _onSave();
        }
    }
}
