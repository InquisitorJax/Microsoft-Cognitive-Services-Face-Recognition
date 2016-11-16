using Prism.Mvvm;
using System;

namespace Xamarin.Forms.MCS.FaceRecog.Model
{
    public class FaceData : BindableBase
    {
        private double _age;
        private string _gender;

        private string _glasses;

        public double Age
        {
            get { return _age; }
            set { SetProperty(ref _age, value); }
        }

        public string Gender
        {
            get { return _gender; }
            set { SetProperty(ref _gender, value); }
        }

        public string Glasses
        {
            get { return _glasses; }
            set { SetProperty(ref _glasses, value); }
        }

        public override string ToString()
        {
            string retDescription = null;

            retDescription += "Gender: " + _gender + Environment.NewLine;
            retDescription += "Age: " + _age + Environment.NewLine;
            retDescription += "Glasses: " + _glasses + Environment.NewLine;

            return retDescription;
        }
    }
}