using System;
using System.Collections;
using System.ComponentModel;
using MvvmValidation;

namespace MixPlanner.ViewModels
{
    public class ValidatableViewModelBase : CloseableViewModelBase, INotifyDataErrorInfo
    {
        private readonly NotifyDataErrorInfoAdapter notifyDataErrorInfoAdapter;
        protected readonly ValidationHelper Validator;

        public ValidatableViewModelBase()
        {
            Validator = new ValidationHelper();
            notifyDataErrorInfoAdapter = new NotifyDataErrorInfoAdapter(Validator);
            PropertyChanged += delegate { Validator.ValidateAll(); };
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return notifyDataErrorInfoAdapter.GetErrors(propertyName);
        }

        public bool HasErrors
        {
            get { return notifyDataErrorInfoAdapter.HasErrors; }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
        {
            add { notifyDataErrorInfoAdapter.ErrorsChanged += value; }
            remove { notifyDataErrorInfoAdapter.ErrorsChanged -= value; }
        }

        public void Validate()
        {
            Validator.ValidateAll();
        }
    }
}