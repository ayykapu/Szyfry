using Microsoft.VisualBasic;
using System.Drawing;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
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

namespace PolybiusCypherWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetCurrentDate();
            dateEncryption();
        }
        public static char[] alphabet = {
                'a', 'ą', 'b', 'c', 'ć', 'd', 'e', 'ę', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'ł', 'm',
                'n', 'ń', 'o', 'ó', 'p', 'q', 'r', 's', 'ś', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'ż', 'ź'
            };
        public static char[] lettersEnc = new char[35];
        public static char[] lettersDec = new char[35];

        private void SetCurrentDate()
        {
            datePicker.SelectedDate = DateTime.Now;
        }
        public static int dateEncryption()
        {
            int currentDay = DateTime.Now.Day;
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            string[] lastDigit = { currentDay.ToString(), currentMonth.ToString(), currentYear.ToString() };

            for (int i = 0; i < lastDigit.Length; i++)
            {
                if (!string.IsNullOrEmpty(lastDigit[i]))
                {
                    lastDigit[i] = lastDigit[i][lastDigit[i].Length - 1].ToString();
                }
            }

            return Convert.ToInt32(lastDigit[0]) + Convert.ToInt32(lastDigit[1]) + Convert.ToInt32(lastDigit[2]);
        }
        public void DefaultEncryptionButton_Click(object sender, EventArgs e)
        {
            TextBox[] KeyInputArray = { KeyInput0, KeyInput1, KeyInput2, KeyInput3, KeyInput4, KeyInput5, KeyInput6, KeyInput7,
                                       KeyInput8, KeyInput9, KeyInput10, KeyInput11, KeyInput12, KeyInput13, KeyInput14, KeyInput15, KeyInput16,
                                       KeyInput17, KeyInput18, KeyInput19, KeyInput20, KeyInput21, KeyInput22, KeyInput23, KeyInput24, KeyInput25,
                                       KeyInput26, KeyInput27, KeyInput28, KeyInput29, KeyInput30, KeyInput31, KeyInput32, KeyInput33, KeyInput34 };


            for (int i = 0; i < KeyInputArray.Length; i++)
            {
                KeyInputArray[i].Text = alphabet[i].ToString();
            }
        }
        public void RandomButton_Click(object sender, EventArgs e)
        {
            int[] randomArray = new int[alphabet.Length];

            for (int i = 0; i < alphabet.Length; i++)
            {
                randomArray[i] = i;
            }

            Random random = new Random();
            for (int i = alphabet.Length - 1; i > 0; i--)
            {
                int j = random.Next(0, 35);

                int temp = randomArray[i];
                randomArray[i] = randomArray[j];
                randomArray[j] = temp;
            }

            TextBox[] KeyInputArray = { KeyInput0, KeyInput1, KeyInput2, KeyInput3, KeyInput4, KeyInput5, KeyInput6, KeyInput7,
                                       KeyInput8, KeyInput9, KeyInput10, KeyInput11, KeyInput12, KeyInput13, KeyInput14, KeyInput15, KeyInput16,
                                       KeyInput17, KeyInput18, KeyInput19, KeyInput20, KeyInput21, KeyInput22, KeyInput23, KeyInput24, KeyInput25,
                                       KeyInput26, KeyInput27, KeyInput28, KeyInput29, KeyInput30, KeyInput31, KeyInput32, KeyInput33, KeyInput34 };

            for (int i = 0; i < KeyInputArray.Length; i++)
            {
                KeyInputArray[i].Text = alphabet[randomArray[i]].ToString();
            }
        }
        private static bool IsEncValidFormat(string input)
        {
            string pattern = @"(?i)^[aąbcćdeęfghijklłmnńoópqrsśtuvwxyzżź]+(\s[aąbcćdeęfghijklłmnńoópqrsśtuvwxyzżź]+)*$";
            return Regex.IsMatch(input, pattern);
        }
        private static bool IsDecValidFormat(string input)
        {
            string pattern = @"^([1-9][0-9] )*[1-9][0-9]$";
            return Regex.IsMatch(input, pattern);
            // return true;
        }
        public void EncryptionButton_Click(object sender, EventArgs e)
        {
            if (Input.Text == "")
            {
                MessageBox.Show("Podaj wiadomość do zaszyfrowania.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                TextBox[] KeyInputs = { KeyInput0, KeyInput1, KeyInput2, KeyInput3, KeyInput4, KeyInput5, KeyInput6,
                                KeyInput7, KeyInput8, KeyInput9, KeyInput10, KeyInput11, KeyInput12, KeyInput13,
                                KeyInput14, KeyInput15, KeyInput16, KeyInput17, KeyInput18, KeyInput19, KeyInput20,
                                KeyInput21, KeyInput22, KeyInput23, KeyInput24, KeyInput25, KeyInput26, KeyInput27,
                                KeyInput28, KeyInput29, KeyInput30, KeyInput31, KeyInput32, KeyInput33, KeyInput34
                 };

                if (!(KeyInputs.All(tb => !string.IsNullOrEmpty(tb?.Text)) && KeyInputs.Select(tb => tb.Text).Distinct().Count() == KeyInputs.Length))
                {
                    MessageBox.Show("Podany klucz jest nieprawidłowy. Upewnij się, że każde pole zostało wypełnione. Upewnij się, że wartości się nie powtarzają.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                {
                    if (IsEncValidFormat(Input.Text))
                    {
                        for (int i = 0; i < KeyInputs.Length; i++)
                        {
                            lettersEnc[i] = Convert.ToChar(KeyInputs[i].Text);
                        }

                        encryptionOutput.Text = Encryption(Input.Text);
                    }
                    else
                    {
                        MessageBox.Show("Niewłaściwy format.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        public static string Encryption(string input)
        {
            input = input.ToLower().Replace(" ", "");
            int rowNumber = 0;
            int columnNumber = 0;
            string result = "";

            for (int i = 0; i < input.Length; i++)
            {
                if (Array.IndexOf(lettersEnc, input[i]) <= 6)
                {
                    rowNumber = 1;
                    columnNumber = Array.IndexOf(lettersEnc, input[i]) + 1;
                }
                else if (Array.IndexOf(lettersEnc, input[i]) <= 13)
                {
                    rowNumber = 2;
                    columnNumber = Array.IndexOf(lettersEnc, input[i]) + 1 - 7;
                }
                else if (Array.IndexOf(lettersEnc, input[i]) <= 20)
                {
                    rowNumber = 3;
                    columnNumber = Array.IndexOf(lettersEnc, input[i]) + 1 - 14;


                }
                else if (Array.IndexOf(lettersEnc, input[i]) <= 27)
                {
                    rowNumber = 4;
                    columnNumber = Array.IndexOf(lettersEnc, input[i]) + 1 - 21;
                }
                else
                {
                    rowNumber = 5;
                    columnNumber = Array.IndexOf(lettersEnc, input[i]) + 1 - 28;
                }
                result = result + (Convert.ToInt32(rowNumber.ToString() + columnNumber.ToString()) + dateEncryption()).ToString() + " ";
            }

            result = result.Substring(0, result.Length - 1);
            return result;

        }
        public void DefaultDecryptionButton_Click(object sender, EventArgs e)
        {
            TextBox[] DecKeyInputArray = { DecKeyInput0, DecKeyInput1, DecKeyInput2, DecKeyInput3, DecKeyInput4, DecKeyInput5, DecKeyInput6,
                                DecKeyInput7, DecKeyInput8, DecKeyInput9, DecKeyInput10, DecKeyInput11, DecKeyInput12, DecKeyInput13,
                                DecKeyInput14, DecKeyInput15, DecKeyInput16, DecKeyInput17, DecKeyInput18, DecKeyInput19, DecKeyInput20,
                                DecKeyInput21, DecKeyInput22, DecKeyInput23, DecKeyInput24, DecKeyInput25, DecKeyInput26, DecKeyInput27,
                                DecKeyInput28, DecKeyInput29, DecKeyInput30, DecKeyInput31, DecKeyInput32, DecKeyInput33, DecKeyInput34 };

            for (int i = 0; i < DecKeyInputArray.Length; i++)
            {
                DecKeyInputArray[i].Text = alphabet[i].ToString();
            }
        }
        public void DecryptionButton_Click(object sender, EventArgs e)
        {

            TextBox[] DecKeyInputArray = { DecKeyInput0, DecKeyInput1, DecKeyInput2, DecKeyInput3, DecKeyInput4, DecKeyInput5, DecKeyInput6,
                                DecKeyInput7, DecKeyInput8, DecKeyInput9, DecKeyInput10, DecKeyInput11, DecKeyInput12, DecKeyInput13,
                                DecKeyInput14, DecKeyInput15, DecKeyInput16, DecKeyInput17, DecKeyInput18, DecKeyInput19, DecKeyInput20,
                                DecKeyInput21, DecKeyInput22, DecKeyInput23, DecKeyInput24, DecKeyInput25, DecKeyInput26, DecKeyInput27,
                                DecKeyInput28, DecKeyInput29, DecKeyInput30, DecKeyInput31, DecKeyInput32, DecKeyInput33, DecKeyInput34
                    };

            if (!(DecKeyInputArray.All(tb => !string.IsNullOrEmpty(tb?.Text)) && DecKeyInputArray.Select(tb => tb.Text).Distinct().Count() == DecKeyInputArray.Length))
            {
                MessageBox.Show("Podany klucz jest nieprawidłowy. Upewnij się, że każde pole zostało wypełnione. Upewnij się, że wartości się nie powtarzają.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            {
                if (!DecDatePicker.SelectedDate.HasValue)
                {
                    MessageBox.Show("Podaj datę zaszyfrowania wiadomości.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (IsDecValidFormat(DecInput.Text))
                    {
                        for (int i = 0; i < lettersDec.Length; i++)
                        {
                            lettersDec[i] = Convert.ToChar(DecKeyInputArray[i].Text);
                        }

                        DateTime selectedDate = DecDatePicker.SelectedDate.Value;
                        int currentDay = selectedDate.Day;
                        int currentMonth = selectedDate.Month;
                        int currentYear = selectedDate.Year;

                        string[] lastDigit = { currentDay.ToString(), currentMonth.ToString(), currentYear.ToString() };

                        for (int i = 0; i < lastDigit.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(lastDigit[i]))
                            {
                                lastDigit[i] = lastDigit[i][lastDigit[i].Length - 1].ToString();
                            }
                        }

                        int valueToSubstract = Convert.ToInt32(lastDigit[0]) + Convert.ToInt32(lastDigit[1]) + Convert.ToInt32(lastDigit[2]);

                        string[] inputArray = DecInput.Text.Split(' ');
                        int[] parseArray = new int[inputArray.Length];

                        for (int i = 0; i < inputArray.Length; i++)
                        {
                            parseArray[i] = Convert.ToInt32(inputArray[i]) - valueToSubstract;
                        }

                        decryptionOutput.Text = Decryption(parseArray);
                    }
                    else
                    {
                        MessageBox.Show("Niewłaściwy format.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        public static string Decryption(int[] input)
        {
            string result = "";
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == 11 || input[i] == 12 || input[i] == 13 || input[i] == 14 || input[i] == 15 || input[i] == 16 || input[i] == 17 || input[i] == 21 || input[i] == 22 ||
                        input[i] == 23 || input[i] == 24 || input[i] == 25 || input[i] == 26 || input[i] == 27 || input[i] == 31 || input[i] == 32 || input[i] == 33 || input[i] == 34 ||
                        input[i] == 35 || input[i] == 36 || input[i] == 37 || input[i] == 41 || input[i] == 42 || input[i] == 43 || input[i] == 44 || input[i] == 45 || input[i] == 46 ||
                        input[i] == 47 || input[i] == 51 || input[i] == 52 || input[i] == 53 || input[i] == 54 || input[i] == 55 || input[i] == 56 || input[i] == 57)
                {
                    switch (input[i].ToString().Substring(0, 1))
                    {
                        case "1":
                            result = result + lettersDec[input[i] - 11];
                            break;
                        case "2":
                            result = result + lettersDec[input[i] - 14];
                            break;
                        case "3":
                            result = result + lettersDec[input[i] - 17];
                            break;
                        case "4":
                            result = result + lettersDec[input[i] - 20];
                            break;
                        case "5":
                            result = result + lettersDec[input[i] - 23];
                            break;
                    }
                }
                else
                {
                    Random random = new Random();
                    int rng = random.Next(0, 35);
                    result = result + lettersDec[rng];
                }
            }
            return result;
        }

    }
}