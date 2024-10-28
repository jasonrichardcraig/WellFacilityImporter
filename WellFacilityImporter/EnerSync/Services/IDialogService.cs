using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnerSync.Services
{
    public interface IDialogService
    {
        bool ShowConfirmationDialog(string message, string title);
        void ShowWarningDialog(string message, string title);
        void ShowErrorDialog(string message, string title);
    }
}
