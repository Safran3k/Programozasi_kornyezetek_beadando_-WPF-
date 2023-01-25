using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProgKorny_Beadando
{
    /// <summary>
    /// Interaction logic for RegisztracioForm.xaml
    /// </summary>
    public partial class RegisztracioForm : Window
    {
        public RegisztracioForm()
        {
            InitializeComponent();
        }

        private void btn_Regisztracio_Click(object sender, RoutedEventArgs e)
        {
            if (tbFelhasznalonev.Text == "" || tbJelszo.Password == "")
            {
                MessageBox.Show("A felhasználónév és a jelszó nem lehet üres.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (tbJelszo.Password != tbJelszoUjra.Password)
            {
                MessageBox.Show("A jelszavak nem egyeznek.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {

                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["filmekConnString"].ConnectionString);

                try
                {
                    if (connection.State == System.Data.ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    string parancs1 = "SELECT id FROM Felhasznalok WHERE FelhasznaloNev = @felhasznalonev";

                    SqlCommand sqlCom1 = new SqlCommand(parancs1, connection);
                    sqlCom1.CommandType = System.Data.CommandType.Text;
                    sqlCom1.Parameters.AddWithValue("@felhasznalonev", tbFelhasznalonev.Text);
                    int id = Convert.ToInt32(sqlCom1.ExecuteScalar());

                    if (id > 0)
                    {
                        MessageBox.Show("Ez a felhasználónév már foglalt.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);

                    }
                    else
                    {
                        string parancs2 = "INSERT INTO Felhasznalok(FelhasznaloNev, Jelszo, Admin) VALUES('" + tbFelhasznalonev.Text + "', '" + Titkositas(tbJelszo.Password.ToString()) + "', '" + 0 + "')";

                        SqlCommand sqlCom2 = new SqlCommand(parancs2, connection);
                        sqlCom2.CommandType = System.Data.CommandType.Text;
                        sqlCom2.ExecuteNonQuery();

                        MessageBox.Show("Sikeres regisztráció, bejelentkezhet.", "Sikeres regisztráció", MessageBoxButton.OK, MessageBoxImage.Information);

                        BejelentkezesForm ujAblak = new BejelentkezesForm();
                        ujAblak.Show();
                        Close();
                    }
                }
                catch (AdatBazisKivetel ex)
                {
                    MessageBox.Show(ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void btn_Vissza_Click(object sender, RoutedEventArgs e)
        {
            BejelentkezesForm ujAblak = new BejelentkezesForm();
            ujAblak.Show();
            Close();
        }

        public static string Titkositas(string jelszo)
        {
            var szovegBytes = Encoding.UTF8.GetBytes(jelszo);
            return Convert.ToBase64String(szovegBytes);
        }
    }
}
