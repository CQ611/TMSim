using System;
using System.Collections.Generic;
using System.Text;

namespace TMSim.UI
{
    public partial class ViewModel : ObservableObject
    {
        private readonly int lastPageNumber = 18;
        private readonly string imageSourcePrefix = "/Images/HelpWindow_";
        private readonly string imageNumberLangCodeDelimiter = "_";
        private readonly string imageFileEnding = ".png";
        private readonly string headingTextPrefix = "TEXT_HelpWindow_Heading_";
        private readonly string pageTextPrefix = "TEXT_HelpWindow_";

        private bool _previousHelpPageAvailable = false;
        public bool PreviousHelpPageAvailable
        {
            get
            {
                return _previousHelpPageAvailable;
            }
            set
            {
                _previousHelpPageAvailable = value;
                OnPropertyChanged(nameof(PreviousHelpPageAvailable));
            }
        }

        private bool _nextHelpPageAvailable = true;
        public bool NextHelpPageAvailable
        {
            get
            {
                return _nextHelpPageAvailable;
            }
            set
            {
                _nextHelpPageAvailable = value;
                OnPropertyChanged(nameof(NextHelpPageAvailable));
            }
        }

        private string _headingText = String.Empty;
        public string HeadingText
        {
            get
            {
                return _headingText;
            }
            set
            {
                _headingText = Translator.GetString(headingTextPrefix + NecessaryLeadingZeros() + value);
                OnPropertyChanged(nameof(HeadingText));
            }
        }

        private int _currentPageNumber = 0;
        public int CurrentPageNumber
        {
            get
            {
                return _currentPageNumber;
            }
            set
            {
                if (value < 0)
                    _currentPageNumber = 0;
                else if (value > lastPageNumber)
                    _currentPageNumber = lastPageNumber;
                else
                _currentPageNumber = value;

                PreviousHelpPageAvailable = true;
                NextHelpPageAvailable = true;

                if (_currentPageNumber == 0)
                    PreviousHelpPageAvailable = false;
                else if (_currentPageNumber == lastPageNumber)
                    NextHelpPageAvailable = false;

                // Update text and image according to new page number
                HeadingText = _currentPageNumber.ToString();
                ImageSource = _currentPageNumber.ToString();
                PageText = _currentPageNumber.ToString();
                OnPropertyChanged(nameof(CurrentPageNumber));
            }
        }
        
        private string _currentImageLanguage = "de-DE";
        public string CurrentImageLanguage
        {
            get
            {
                return _currentImageLanguage;
            }
            set
            {
                _currentImageLanguage = value;
                // Force current image to reload in newly selected language
                ImageSource = CurrentPageNumber.ToString();
                OnPropertyChanged(nameof(CurrentImageLanguage));
            }
        }

        private string _imageSource = "/Images/HelpWindow_00_de-DE.png";
        public string ImageSource
        {
            get
            {
                return _imageSource;
            }
            set
            {
                // Images 0-9 have a leading zero so that images 10-19 dont appear before 2
                    _imageSource = imageSourcePrefix + NecessaryLeadingZeros() + 
                                    value + imageNumberLangCodeDelimiter + 
                                    CurrentImageLanguage + imageFileEnding;

                OnPropertyChanged(nameof(ImageSource));
            }
        }

        private string _pageText = String.Empty;
        public string PageText
        {
            get
            {
                return _pageText;
            }
            set
            {
                _pageText = Translator.GetString(
                             pageTextPrefix + NecessaryLeadingZeros() + value);
                OnPropertyChanged(nameof(PageText));
            }
        }

        public void TranslateHelpWindow()
        {
            HeadingText = _currentPageNumber.ToString();
            PageText = _currentPageNumber.ToString();
        }

        // Leading zeros in file names allow for better organization
        // (i.e. 10-19 don't appear before 2 anymore)
        public string NecessaryLeadingZeros()
        {
            string leadingZeros = String.Empty;
            int lastPageDigits = lastPageNumber.ToString().Length;
            int currentPageDigits = CurrentPageNumber.ToString().Length;

            for (int i = 0; i < lastPageDigits - currentPageDigits; i++)
                leadingZeros += "0";

            return leadingZeros;
        }
    }
}
