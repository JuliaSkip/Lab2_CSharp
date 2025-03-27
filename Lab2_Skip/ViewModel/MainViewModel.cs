using KMA.ProgrammingInCSharp2025.Lab2_Skip.Model;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KMA.ProgrammingInCSharp2025.Lab2_Skip
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _birthday;
        private DateTime? _selectedDate;
        private bool _isProceedEnabled;
        private bool _isProcessing;

        private string _isAdultStr;
        private string _sunSign;
        private string _chineseSign;
        private string _isBirthdayStr;
        private bool _isBirthday;



        private Visibility _showData = Visibility.Hidden;
        private Visibility _isBirthdayVisible = Visibility.Hidden;



        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
                UpdateProceedState();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
                UpdateProceedState();
            }
        }
        public string Birthday
        {
            get => _birthday;
            set
            {
                _birthday = value;
                OnPropertyChanged(nameof(Birthday));
                UpdateProceedState();
            }
        }


        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
                UpdateProceedState();
            }
        }

        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value; 
                OnPropertyChanged(nameof(SelectedDate));
                UpdateProceedState();
            }
        }

        public bool IsProceedEnabled
        {
            get => _isProceedEnabled;
            set
            {
                _isProceedEnabled = value; 
                OnPropertyChanged(nameof(IsProceedEnabled));
            }
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                _isProcessing = value;
                OnPropertyChanged(nameof(IsProcessing));
                UpdateProceedState();
            }
        }

        public bool IsInputEnabled => !IsProcessing;


        public Visibility ShowData
        {
            get => _showData;
            set
            {
                _showData = value; 
                OnPropertyChanged(nameof(ShowData));
            }
        }
        public Visibility IsBirthdayVisible
        {
            get => _isBirthdayVisible;
            set
            {
                _isBirthdayVisible = value;
                OnPropertyChanged(nameof(IsBirthdayVisible));
            }
        }

        public string IsAdultStr
        {
            get => _isAdultStr;
            set
            {
                _isAdultStr = value; 
                OnPropertyChanged(nameof(IsAdultStr));
            }
        }
        public string SunSign
        {
            get => _sunSign;
            set
            {
                _sunSign = value; 
                OnPropertyChanged(nameof(SunSign));
            }
        }
        public string ChineseSign
        {
            get => _chineseSign;
            set
            {
                _chineseSign = value; 
                OnPropertyChanged(nameof(ChineseSign));
            }
        }
        public string IsBirthdayStr
        {
            get => _isBirthdayStr;
            set
            {
                _isBirthdayStr = value; 
                OnPropertyChanged(nameof(IsBirthdayStr));
            }
        }
        public bool IsBirthday
        {
            get => _isBirthday;
            set
            {
                _isBirthday = value;
                OnPropertyChanged(nameof(IsBirthday));
            }
        }

        public ICommand ProceedCommand { get; }

        public MainViewModel()
        {
            ProceedCommand = new RelayCommand(Proceed, () => IsProceedEnabled);
        }

        private void UpdateProceedState()
        {
            IsProceedEnabled = !IsProcessing &&
                               !string.IsNullOrWhiteSpace(FirstName) &&
                               !string.IsNullOrWhiteSpace(LastName) &&
                               !string.IsNullOrWhiteSpace(Email) &&
                               SelectedDate.HasValue;

            OnPropertyChanged(nameof(IsProceedEnabled));
            OnPropertyChanged(nameof(IsInputEnabled));

            ((RelayCommand)ProceedCommand).RaiseCanExecuteChanged();
        }


        private async void Proceed()
        {
            IsProcessing = true;
            IsBirthdayVisible = Visibility.Hidden;
            ShowData = Visibility.Hidden;

            await Task.Run(() =>
            {
                Task.Delay(1000).Wait();

                Person person = new Person(FirstName, LastName, Email, SelectedDate.Value);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        person.ValidateAge();
                        person.ValidateEmail(person.Email);

                        IsAdultStr = $"IsAdult: {person.IsAdult}";
                        IsBirthdayStr = $"IsBirthday: {person.IsBirthday}";
                        ChineseSign = $"ChineseSign: {person.ChineseSign}";
                        SunSign = $"SunSign: {person.SunSign}";
                        Birthday = $"Date of Birth: {person.DateOfBirth:yyyy-MM-dd}";
                        IsBirthday = person.IsBirthday;

                        if (IsBirthday)
                        {
                            IsBirthdayVisible = Visibility.Visible;
                        }
                        ShowData = Visibility.Visible;
                    }
                    catch (BirthdayInFutureException)
                    {
                        MessageBox.Show("Sorry, you are not born yet");
                    }
                    catch (DeadPersonException)
                    {
                        MessageBox.Show("Sorry, you are dead");
                    }
                    catch (InvalidEmailException)
                    {
                        MessageBox.Show("Please, enter a valid email address");
                    }
                });
            });

            Application.Current.Dispatcher.Invoke(() => { IsProcessing = false; });
        }




        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();
        public void Execute(object parameter) => _execute();

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public event EventHandler CanExecuteChanged;
    }
}
