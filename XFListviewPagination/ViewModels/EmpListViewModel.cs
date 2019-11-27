using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Newtonsoft.Json;
using Xamarin.Forms;
using XFListviewPagination.Models;

namespace XFListviewPagination.ViewModels
{
    public class EmpListViewModel : INotifyPropertyChanged
    {
        List<Employee> objEmployeeList;
        
        int currentPageIndex = 1;
        int initialItemIndex = 0;
        int totalNumberOfPage = 1;

        #region Properties

        public ObservableCollection<Employee> _employeeList;
        public ObservableCollection<Employee> EmployeeList
        {
            get { return _employeeList; }
            set
            {
                _employeeList = value;
                OnPropertyChanged("EmployeeList");
            }
        }

        public List<string> PickerItems { get; set; }

        public bool _isPrevBtnEnabled;
        public bool IsPrevBtnEnabled
        {
            get { return _isPrevBtnEnabled; }
            set
            {
                _isPrevBtnEnabled = value;
                OnPropertyChanged("IsPrevBtnEnabled");
            }
        }

        public bool _isNextBtnEnabled;
        public bool IsNextBtnEnabled
        {
            get { return _isNextBtnEnabled; }
            set
            {
                _isNextBtnEnabled = value;
                OnPropertyChanged("IsNextBtnEnabled");
            }
        }

        private string _selectedItemPerPage;
        public string SelectedItemPerPage
        {
            get
            {
                return _selectedItemPerPage;
            }
            set
            {
                _selectedItemPerPage = value;
                ItemsPerPage = Convert.ToInt32(_selectedItemPerPage);
            }
        }

        private int _itemsPerPage;
        public int ItemsPerPage
        {
            get
            {
                return _itemsPerPage;
            }
            set
            {
                _itemsPerPage = value;
                OnPropertyChanged("CityText");
            }
        }
        #endregion

        #region Command

        public ICommand NextButtonCommand { get; private set; }

        //By default if IsEnabled property of button is set false it won't reflect
        private ICommand _prevButtonCommand;
        public ICommand PrevButtonCommand
        {
            get { return _prevButtonCommand ?? (_prevButtonCommand = new Command(PrevButtonAction, (x) => IsPrevBtnEnabled)); }
        }
        #endregion

        public EmpListViewModel()
        {
            NextButtonCommand = new Command(NextButtonAction);
            PickerItems = new List<string>()
            {
                "3","4","5","6","7","8","9","10"
            };

            SelectedItemPerPage = "10";
            //ItemsPerPage = 10;
            GetJsonData();
        }

        void GetJsonData()
        {
            string jsonFileName = "MOCK_DATA.json";
            objEmployeeList = new List<Employee>();
            var assembly = typeof(EmpListViewModel).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{jsonFileName}");
            using (var reader = new StreamReader(stream))
            {
                var jsonString = reader.ReadToEnd();

                //Converting JSON Array Objects into generic list  
                objEmployeeList = JsonConvert.DeserializeObject<List<Employee>>(jsonString);
            }

            EmployeeList = new ObservableCollection<Employee>(objEmployeeList.ToList().GetRange(initialItemIndex, ItemsPerPage));

            totalNumberOfPage = GetPageCount(objEmployeeList.Count, ItemsPerPage);
        }


        private void NextButtonAction(object obj)
        {
            initialItemIndex += ItemsPerPage;
            currentPageIndex += 1;

            if (currentPageIndex == totalNumberOfPage)
            {
                UpdateListData(initialItemIndex);
                IsNextBtnEnabled = false; 
                return;
            }

            UpdateListData(initialItemIndex);
            IsNextBtnEnabled = true;
            IsPrevBtnEnabled = true;
        }

        private void PrevButtonAction(object obj)
        {
            initialItemIndex -= ItemsPerPage;
            currentPageIndex -= 1;

            if (currentPageIndex == 1)
            {
                UpdateListData(initialItemIndex);
                IsPrevBtnEnabled = false;
                return;
            }

            UpdateListData(initialItemIndex);
            IsNextBtnEnabled = true;
            IsPrevBtnEnabled = true;
        }
        private void UpdateListData(int startIndex)
        {
            if (EmployeeList?.Count > 0)
                EmployeeList.Clear();

            int total = objEmployeeList.Count;

            if (startIndex + ItemsPerPage <= total)
                EmployeeList = new ObservableCollection<Employee>(objEmployeeList.ToList().GetRange(startIndex, ItemsPerPage));
            else
                EmployeeList = new ObservableCollection<Employee>(objEmployeeList.ToList().GetRange(startIndex, total - startIndex));

        }

        private int GetPageCount(int count, int pageSize)
        {
            int result = 0;

            if (count > 0)
            {
                result = count / pageSize;
                if (result > 0 && (count > (pageSize * result)))
                {
                    result++;
                }
            }

            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property)
        {
            var changed = PropertyChanged;

            if (changed == null)
                return;

            changed(this, new PropertyChangedEventArgs(property));
        }
    }
}
