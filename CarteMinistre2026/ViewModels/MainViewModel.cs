using CarteMinistre2026.Helpers;
using CarteMinistre2026.Models;
using CarteMinistre2026.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CarteMinistre2026.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private ObservableCollection<Employee> _employees;
        private Employee _selectedEmployee;

        public MainViewModel()
        {
            Employees = new ObservableCollection<Employee>();

            ImportCommand = new RelayCommand(ExecuteImport);
            PreviewCommand = new RelayCommand(ExecutePreview, CanExecutePreview);
            PrintCommand = new RelayCommand(ExecutePrint, CanExecutePreview);
            DesignerCommand = new RelayCommand(ExecuteDesigner);
            HistoryCommand = new RelayCommand(ExecuteHistory);
            RefreshCommand = new RelayCommand(ExecuteRefresh);
            QuitCommand = new RelayCommand(ExecuteQuit);
        }

        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }

        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand ImportCommand { get; }
        public ICommand PreviewCommand { get; }
        public ICommand PrintCommand { get; }
        public ICommand DesignerCommand { get; }
        public ICommand HistoryCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand QuitCommand { get; }

        private void ExecuteImport(object parameter)
        {
            var importView = new ImportView();
            importView.Owner = Application.Current.MainWindow;
            importView.ShowDialog();
            ExecuteRefresh(null);
        }

        private bool CanExecutePreview(object parameter)
        {
            return SelectedEmployee != null;
        }

        private void ExecutePreview(object parameter)
        {
            if (SelectedEmployee == null) return;

            var previewView = new PreviewView();
            previewView.Owner = Application.Current.MainWindow;
            previewView.SetEmployee(SelectedEmployee);
            previewView.ShowDialog();
        }

        private void ExecutePrint(object parameter)
        {
            if (SelectedEmployee == null) return;

            var printView = new PrintView();
            printView.Owner = Application.Current.MainWindow;
            printView.ShowDialog();
        }

        private void ExecuteDesigner(object parameter)
        {
            var designerView = new DesignerView();
            designerView.Owner = Application.Current.MainWindow;
            designerView.ShowDialog();
        }

        private void ExecuteHistory(object parameter)
        {
            var historyView = new HistoryView();
            historyView.Owner = Application.Current.MainWindow;
            historyView.ShowDialog();
        }

        private void ExecuteRefresh(object parameter)
        {
            LoadEmployees();
        }

        private void ExecuteQuit(object parameter)
        {
            Application.Current.Shutdown();
        }

        private void LoadEmployees()
        {
            Employees.Clear();
            // TODO: Charger depuis la base de données
            // Données de test
            Employees.Add(new Employee
            {
                Id = 1,
                FirstName = "Fidèle",
                LastName = "MUBUNDU",
                PostName = "SEYA",
                JobTitle = "Pasteur",
                Ministry = "Évangéliste",
                BirthPlace = "Pindi",
                BirthDate = new DateTime(1980, 12, 12),
                OrdinationDate = new DateTime(2023, 4, 30),
                IssueDate = new DateTime(2024, 5, 23),
                ExpiryDate = new DateTime(2026, 8, 28)
            });
        }
    }
}