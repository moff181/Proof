using System;

namespace Proof.DevEnv
{
    public class ChangeHistory
    {
        private readonly Action _onUnsavedChange;
        private bool _unsavedChange;

        public ChangeHistory(Action onUnsavedChange)
        {
            _onUnsavedChange = onUnsavedChange;
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
        }
    }
}
