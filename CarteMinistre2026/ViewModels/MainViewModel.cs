using CarteMinistre2026.Helpers;
using CarteMinistre2026.Models;
using CarteMinistre2026.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CarteMinistre2026.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private object _currentView;
        private ObservableCollection<Employee> _employees;
        private Employee _selectedEmployee;

        public MainViewModel()
        {
            Employees = new ObservableCollection<Employee>();

            // Commandes
            ImportCommand = new RelayCommand(ExecuteImport);
            PreviewCommand = new RelayCommand(ExecutePreview, CanExecutePreview);
            PrintCommand = new RelayCommand(ExecutePrint, CanExecutePreview);
            DesignerCommand = new RelayCommand(ExecuteDesigner);
            HistoryCommand = new RelayCommand(ExecuteHistory);
            RefreshCommand = new RelayCommand(ExecuteRefresh);
            QuitCommand = new RelayCommand(ExecuteQuit);
            ShowImportCommand = new RelayCommand(ExecuteShowImport);
            ShowEmployeeListCommand = new RelayCommand(ExecuteShowEmployeeList);

            // Vue par défaut
            CurrentView = CreateEmployeeListView();
        }

        // ===== PROPRIÉTÉS =====

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
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

        // ===== COMMANDES =====

        public ICommand ImportCommand { get; }
        public ICommand PreviewCommand { get; }
        public ICommand PrintCommand { get; }
        public ICommand DesignerCommand { get; }
        public ICommand HistoryCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand QuitCommand { get; }
        public ICommand ShowImportCommand { get; }
        public ICommand ShowEmployeeListCommand { get; }

        // ===== MÉTHODES D'EXÉCUTION =====

        private void ExecuteShowImport(object parameter)
        {
            var importView = new ImportView();
            CurrentView = importView;
        }

        private void ExecuteShowEmployeeList(object parameter)
        {
            CurrentView = CreateEmployeeListView();
        }

        private void ExecuteImport(object parameter)
        {
            var importView = new ImportView();
            CurrentView = importView;
        }

        private bool CanExecutePreview(object parameter)
        {
            return SelectedEmployee != null;
        }

        private void ExecutePreview(object parameter)
        {
            if (SelectedEmployee == null) return;

            var preview = new PreviewView();
            preview.SetEmployee(SelectedEmployee);
            CurrentView = preview;
        }

        private void ExecutePrint(object parameter)
        {
            if (SelectedEmployee == null) return;
            // À implémenter plus tard
        }

        private void ExecuteDesigner(object parameter)
        {
            var designerView = new DesignerView();
            CurrentView = designerView;
        }

        private void ExecuteHistory(object parameter)
        {
            var historyView = new HistoryView();
            CurrentView = historyView;
        }

        private void ExecuteRefresh(object parameter)
        {
            LoadEmployees();
        }

        private void ExecuteQuit(object parameter)
        {
            Application.Current.Shutdown();
        }

        // ===== MÉTHODES PRIVÉES =====

        private UserControl CreateEmployeeListView()
        {
            var view = new EmployeeListView();
            view.DataContext = this;
            return view;
        }

        private void LoadEmployees()
        {
            Employees.Clear();
            try
            {
                using (var db = new Data.AppDbContext())
                {
                    var employeesFromDb = db.Employees.ToList();
                    foreach (var emp in employeesFromDb)
                    {
                        Employees.Add(emp);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur chargement : {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}