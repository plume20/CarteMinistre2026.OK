using CarteMinistre2026.Data;
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
        // ===== CHAMPS PRIVÉS =====
        private object _currentView;
        private ObservableCollection<Employee> _employees;
        private Employee _selectedEmployee;

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
            }
        }

        // ===== COMMANDES =====
        public ICommand ShowImportCommand { get; }
        public ICommand ShowEmployeeListCommand { get; }
        public ICommand PreviewCommand { get; }
        public ICommand PrintCommand { get; }
        public ICommand DesignerCommand { get; }
        public ICommand HistoryCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand QuitCommand { get; }

        // ===== CONSTRUCTEUR =====
        public MainViewModel()
        {
            Employees = new ObservableCollection<Employee>();

            // Initialiser les commandes
            ShowImportCommand = new RelayCommand(ExecuteShowImport);
            ShowEmployeeListCommand = new RelayCommand(ExecuteShowEmployeeList);
            PreviewCommand = new RelayCommand(ExecutePreview);
            PrintCommand = new RelayCommand(ExecutePrint);
            DesignerCommand = new RelayCommand(ExecuteDesigner);
            HistoryCommand = new RelayCommand(ExecuteHistory);
            RefreshCommand = new RelayCommand(ExecuteRefresh);
            QuitCommand = new RelayCommand(ExecuteQuit);

            // Vue par défaut : liste des employés
            CurrentView = CreateEmployeeListView();
        }

        // ===== MÉTHODES D'EXÉCUTION =====

        private void ExecuteShowImport(object parameter)
        {
            var importView = new ImportView();
            CurrentView = importView;
        }

        private void ExecuteShowEmployeeList(object parameter)
        {
            CurrentView = CreateEmployeeListView();
            LoadEmployees();
        }

        private void ExecutePreview(object parameter)
        {
            if (SelectedEmployee == null)
            {
                MessageBox.Show("Veuillez sélectionner un employé.", "Information",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var preview = new PreviewView();
            preview.SetEmployee(SelectedEmployee);
            CurrentView = preview;
        }

        private void ExecutePrint(object parameter)
        {
            if (SelectedEmployee == null)
            {
                MessageBox.Show("Veuillez sélectionner un employé.", "Information",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            // TODO: Implémenter l'impression
            MessageBox.Show("Impression à implémenter", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
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
                using (var db = new AppDbContext())
                {
                    var employeesFromDb = db.Employees.OrderBy(e => e.LastName).ToList();
                    foreach (var emp in employeesFromDb)
                    {
                        Employees.Add(emp);
                    }
                }
            }
            catch (Exception ex)
            {
                // Silencieux au démarrage si la base n'existe pas encore
            }
        }
    }
}