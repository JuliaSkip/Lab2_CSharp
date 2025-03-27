using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices.Swift;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace KMA.ProgrammingInCSharp2025.Lab2_Skip.Model
{
    class Person
    {
        private string _name;
        private string _surname;
        private string _email;
        private DateTime _dateOfBirth;
        private bool _isAdult;
        private string _sunSign;
        private string _chineseSign;
        private bool _isBirthday;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
            }
        }

        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                _dateOfBirth = value;
            }
        }

        public bool IsAdult
        {
            get => _isAdult;
            private set
            {
                _isAdult = value;
            }
        }
        public string SunSign
        {
            get => _sunSign;
            private set
            {
                _sunSign = value;
            }
        }
        public string ChineseSign
        {
            get => _chineseSign;
            private set
            {
                _chineseSign = value;
            }
        }
        public bool IsBirthday
        {
            get => _isBirthday;
            private set
            {
                _isBirthday = value;
            }
        }


        public Person(string name, string surname, string email, DateTime birthday)
        {
            Name = name;
            Surname = surname;
            Email = email;
            DateOfBirth = birthday;


            _isAdult = CountAge() >= 18;
            _sunSign = CalculateWestern();
            _chineseSign = CalculateChinese();
            _isBirthday = CheckBirthDay();

        }

        public Person(string name, string surname, string email)
            : this(name, surname, email, DateTime.Today) { }

        public Person(string name, string surname, DateTime birthday) :
            this(name, surname, "", birthday) { }


        private int CountAge(){

            DateTime today = DateTime.Now;
            int age = today.Year - DateOfBirth.Year;

            if (DateTime.Now < DateOfBirth.AddYears(age))
            {
                age--;
            }

            return age;
        }

        private string CalculateChinese()
        {
            string zodiac = "";

            switch (DateOfBirth.Year % 12)
            {
                case 4: zodiac = "Rat"; break;
                case 5: zodiac = "Ox"; break;
                case 6: zodiac = "Tiger"; break;
                case 7: zodiac = "Rabbit"; break;
                case 8: zodiac = "Dragon"; break;
                case 9: zodiac = "Snake"; break;
                case 10: zodiac = "Horse"; break;
                case 11: zodiac = "Goat"; break;
                case 0: zodiac = "Monkey"; break;
                case 1: zodiac = "Rooster"; break;
                case 2: zodiac = "Dog"; break;
                case 3: zodiac = "Pig"; break;
            }

            return zodiac;
        }

        private string CalculateWestern()
        {
            string zodiac = "";
            int day = DateOfBirth.Day;
            int month = DateOfBirth.Month;

            if ((month == 3 && day >= 21) || (month == 4 && day <= 20))
                zodiac = "Aries";
            else if ((month == 4 && day >= 21) || (month == 5 && day <= 20))
                zodiac = "Taurus";
            else if ((month == 5 && day >= 21) || (month == 6 && day <= 20))
                zodiac = "Gemini";
            else if ((month == 6 && day >= 21) || (month == 7 && day <= 22))
                zodiac = "Cancer";
            else if ((month == 7 && day >= 23) || (month == 8 && day <= 22))
                zodiac = "Leo";
            else if ((month == 8 && day >= 23) || (month == 9 && day <= 22))
                zodiac = "Virgo";
            else if ((month == 9 && day >= 23) || (month == 10 && day <= 22))
                zodiac = "Libra";
            else if ((month == 10 && day >= 23) || (month == 11 && day <= 21))
                zodiac = "Scorpio";
            else if ((month == 11 && day >= 22) || (month == 12 && day <= 21))
                zodiac = "Sagittarius";
            else if ((month == 12 && day >= 22) || (month == 1 && day <= 19))
                zodiac = "Capricorn";
            else if ((month == 1 && day >= 20) || (month == 2 && day <= 18))
                zodiac = "Aquarius";
            else if ((month == 2 && day >= 19) || (month == 3 && day <= 20))
                zodiac = "Pisces";

            return zodiac;
        }

        private bool CheckBirthDay()
        {
            DateTime today = DateTime.Now;

            if (DateOfBirth.Day == today.Day && DateOfBirth.Month == today.Month)
            {
                return true;
            }

            return false;
        }

        public void ValidateEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            if (!Regex.IsMatch(email, pattern))
            {
                throw new InvalidEmailException(email);
            }
        }

        public void ValidateAge()
        {
            int age = CountAge();

            if (age > 135)
            {
                throw new DeadPersonException(age);
            }
            if (age < 0)
            {
                throw new BirthdayInFutureException(age);
            }
        }

    }
    [Serializable]
    class InvalidEmailException : Exception
    {
        public InvalidEmailException() { }

        public InvalidEmailException(string email)
            : base(String.Format("Invalid Email: {0}", email))
        {

        }
    }
    [Serializable]
    class BirthdayInFutureException : Exception
    {
        public BirthdayInFutureException() { }

        public BirthdayInFutureException(int age)
        : base(String.Format("Invalid age: {0}. You must be born first.", age))
        {

        }
    }
    [Serializable]
    class DeadPersonException : Exception
    {
        public DeadPersonException() { }

        public DeadPersonException(int age)
        : base(String.Format("Invalid age: {0}. You cannot be older than 135.", age))
        {

        }
    }
}
