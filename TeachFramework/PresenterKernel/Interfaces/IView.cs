using System;
using System.Collections.Generic;

namespace TeachFramework.Interfaces
{
    public interface IView
    {
        string ViewName { set; }
        event EventHandler<EventArgs> SubmitButtonPressed;
        event EventHandler<EventArgs> AutoButtonPressed;
        event EventHandler<EventArgs> ResetButtonPressed;
        UiDescription GetUserData();
        void ShowCorrectData(IEnumerable<UiDescriptionItem> correctData);
        void ShowMessage(string message);
        bool IsValid();
        void ResetErrors();
    }
}
