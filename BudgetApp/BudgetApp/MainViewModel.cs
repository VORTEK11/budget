using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;


namespace BudgetApp
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<BudgetRecord> _records;
        private ObservableCollection<string> _types;
        private DateTime _selectedDate;
        private BudgetRecord _selectedRecord;

        public MainViewModel()
        {
            LoadData(); // Загрузка данных из файла при запуске приложения
            SelectedDate = DateTime.Today;
        }

        public ObservableCollection<BudgetRecord> FilteredRecords { get; private set; }

        public ObservableCollection<string> Types
        {
            get => _types;
            set
            {
                _types = value;
                OnPropertyChanged();
            }
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                FilterRecordsByDate();
                OnPropertyChanged();
            }
        }

        public BudgetRecord SelectedRecord
        {
            get => _selectedRecord;
            set
            {
                _selectedRecord = value;
                OnPropertyChanged();
            }
        }

        public decimal TotalAmount { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void PreviousDate()
        {
            SelectedDate = SelectedDate.AddDays(-1);
        }

        public void NextDate()
        {
            SelectedDate = SelectedDate.AddDays(1);
        }

        public void AddRecord()
        {
            var record = new BudgetRecord();
            var editWindow = new EditRecordWindow(record);
            if (editWindow.ShowDialog() == true)
            {
                _records.Add(record);
                FilterRecordsByDate();
                UpdateTotalAmount();
                SaveData();
            }
        }

        public void UpdateRecord()
        {
            if (SelectedRecord == null)
            {
                MessageBox.Show("Пожалуйста, выберите запись для изменения.");
                return;
            }

            var editWindow = new EditRecordWindow(SelectedRecord);
            if (editWindow.ShowDialog() == true)
            {
                FilterRecordsByDate();
                UpdateTotalAmount();
                SaveData();
            }
        }

        public void DeleteRecord()
        {
            if (SelectedRecord == null)
            {
                MessageBox.Show("Пожалуйста, выберите запись для удаления.");
                return;
            }

            if (MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _records.Remove(SelectedRecord);
                FilterRecordsByDate();
                UpdateTotalAmount();
                SaveData();
            }
        }

        private void FilterRecordsByDate()
        {
            FilteredRecords = new ObservableCollection<BudgetRecord>();
            foreach (var record in _records)
            {
                if (record.Date.Date == SelectedDate.Date)
                {
                    FilteredRecords.Add(record);
                }
            }
        }

        private void UpdateTotalAmount()
        {
            TotalAmount = 0;
            foreach (var record in FilteredRecords)
            {
                if (record.IsIncome)
                {
                    TotalAmount += record.Amount;
                }
                else
                {
                    TotalAmount -= record.Amount;
                }
            }
            OnPropertyChanged(nameof(TotalAmount));
        }

        private void LoadData()
        {
            try
            {
                var json = File.ReadAllText("budget.json");
                _records = JsonSerializer.Deserialize<ObservableCollection<BudgetRecord>>(json);
            }
            catch
            {
                _records = new ObservableCollection<BudgetRecord>();
            }
            Types = GetDistinctTypes();
            FilterRecordsByDate();
            UpdateTotalAmount();
        }

        private void SaveData()
        {
            var json = JsonSerializer.Serialize(_records);
            File.WriteAllText("budget.json", json);
        }

        private ObservableCollection<string> GetDistinctTypes()
        {
            var types = new HashSet<string>();
            foreach (var record in _records)
            {
                types.Add(record.Type);
            }
            return new ObservableCollection<string>(types);
        }
    }
}
