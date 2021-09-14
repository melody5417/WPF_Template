using GalaSoft.MvvmLight;

namespace Demo.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {

        private string m_title;

        public string Title
        {
            get => m_title;
            set => Set(nameof(Title), ref m_title, value);
        }


        private string m_inputText;

        public string InputText
        {
            get => m_inputText;
            set => Set(nameof(InputText), ref m_inputText, value);
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Title = "Test MVVM and HandyControl";
        }
    }
}