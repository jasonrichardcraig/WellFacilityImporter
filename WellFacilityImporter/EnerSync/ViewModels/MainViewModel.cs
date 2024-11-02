using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Data.SqlClient;

namespace EnerSync.ViewModels
{
    public partial class MainViewModel : ObservableValidator
    {
        private bool _isBusy;
        private string _userName = string.Empty;
        private string _server = string.Empty;
        private string _database = string.Empty;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                }
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Server // Added Server property
        {
            get { return _server; }
            set
            {
                if (_server != value)
                {
                    _server = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Database // Added Database property
        {
            get { return _database; }
            set
            {
                if (_database != value)
                {
                    _database = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainViewModel()
        {
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(Properties.Settings.Default.ConnectionString);
            UserName = Environment.UserName;
            Server = sqlConnectionStringBuilder.DataSource;
            Database = sqlConnectionStringBuilder.InitialCatalog;
        }
    }
}
