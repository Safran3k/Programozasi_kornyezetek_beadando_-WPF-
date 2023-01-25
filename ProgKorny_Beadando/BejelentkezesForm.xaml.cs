using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
    /// Interaction logic for BejelentkezesForm.xaml
    /// </summary>
    public partial class BejelentkezesForm : Window
    {
        public BejelentkezesForm()
        {
            InitializeComponent();
        }

        private void btn_Kilepes_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Biztos kilép?", "Kilépés", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Environment.Exit(0);
            }
        }

        private void btn_Regisztracio_Click(object sender, RoutedEventArgs e)
        {
            RegisztracioForm ujAblak = new RegisztracioForm();
            ujAblak.Show();
            Close();
        }

        private void btn_Bejelentkezes_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection kapcsolat = new SqlConnection(ConfigurationManager.ConnectionStrings["filmekConnString"].ConnectionString);

            string adat = "SELECT * FROM Felhasznalok WHERE FelhasznaloNev = '" + tbFelhasznalonev.Text + "'";

            string parancs = "SELECT COUNT(1) FROM Felhasznalok WHERE FelhasznaloNev = @felhasznalonev AND Jelszo = @jelszo";

            try
            {
                if (kapcsolat.State == ConnectionState.Closed)
                {
                    kapcsolat.Open();
                }

                string eredmeny = string.Empty;
                bool admin = false;
                string felhasznalonev = string.Empty;

                SqlCommand sqlCom1 = new SqlCommand(adat, kapcsolat);
                using (var reader = sqlCom1.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            eredmeny = reader["Jelszo"].ToString().Trim();
                            admin = Convert.ToBoolean(reader["Admin"]);
                            felhasznalonev = reader["FelhasznaloNev"].ToString();
                        }

                    }
                }

                if (Titkositas(tbJelszo.Password.Trim()) == eredmeny && tbFelhasznalonev.Text != string.Empty && tbJelszo.Password != string.Empty)
                {

                    SqlCommand sqlCom2 = new SqlCommand(parancs, kapcsolat);
                    sqlCom2.CommandType = CommandType.Text;
                    sqlCom2.Parameters.AddWithValue("@felhasznalonev", tbFelhasznalonev.Text.Trim());
                    sqlCom2.Parameters.AddWithValue("@jelszo", Titkositas(tbJelszo.Password.Trim()));
                    int id = Convert.ToInt32(sqlCom2.ExecuteScalar());

                    if (id > 0)
                    {
                        FilmekForm ujAblak = new FilmekForm(admin, felhasznalonev);
                        ujAblak.Show();
                        kapcsolat.Close();
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("Helytelen felhasználónév/jelszó páros", "Sikertelen bejelentkezés", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (AdatBazisKivetel)
            {
                MessageBox.Show("Sikertelen kapcsolat", "Figyelem", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static string Titkositas(string jelszo)
        {
            var szovegBytes = Encoding.UTF8.GetBytes(jelszo);
            return Convert.ToBase64String(szovegBytes);
        }
    }
}
