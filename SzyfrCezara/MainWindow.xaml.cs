using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SzyfrCezara
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static char[] alphabet = {
                'a', 'ą', 'b', 'c', 'ć', 'd', 'e', 'ę', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'ł', 'm',
                'n', 'ń', 'o', 'ó', 'p', 'q', 'r', 's', 'ś', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'ż', 'ź'
            };

        private static bool IsValidFormat(string input)
        {
            string pattern = @"(?i)^[aąbcćdeęfghijklłmnńoópqrsśtuvwxyzżź]+(\s[aąbcćdeęfghijklłmnńoópqrsśtuvwxyzżź]+)*$";
            return Regex.IsMatch(input, pattern);
        }

        public static string Encryption(string input, int shift, bool isRight)
        //true - encryption; false - decryption
        {
            if (shift < 0 || shift > 34) { throw new Exception("Shift is invalid."); }

            if (isRight == false)
            { Array.Reverse(alphabet); }

            input = input.ToLower().Replace(" ", "");

            char[] encryptedChars = new char[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                char currentChar = input[i];
                int charIndex = Array.IndexOf(alphabet, currentChar);
                int encryptedIndex = (charIndex + shift) % alphabet.Length;
                encryptedChars[i] = alphabet[encryptedIndex];
            }

            return new string(encryptedChars);
        }
        public void EncryptionSliderSetValue(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            EncryptionValue.Text = EncryptionSlider.Value.ToString();
        }

        public void DecryptionSliderSetValue(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            DecryptionValue.Text = DecryptionSlider.Value.ToString();
        }

        public void EncryptionButton_Click(object sender, EventArgs e)
        {
            int shift = Convert.ToInt32(EncryptionSlider.Value);

            if (IsValidFormat(encryptionInput.Text))
            {
                encryptionOutput.Text = Encryption(encryptionInput.Text, shift, true);
            }
            else
            {
                MessageBox.Show("Niewłaściwy format.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void DecryptionButton_Click(object sender, EventArgs e)
        {
            int shift = Convert.ToInt32(DecryptionSlider.Value);
          
            if (IsValidFormat(decryptionInput.Text))
            {
                decryptionOutput.Text = Encryption(decryptionInput.Text, shift, false);
                Array.Reverse(alphabet);
            }
            else
            {
                MessageBox.Show("Niewłaściwy format.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

