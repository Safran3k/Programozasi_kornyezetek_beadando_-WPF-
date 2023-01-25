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
    /// Interaction logic for FilmekForm.xaml
    /// </summary>
    public partial class FilmekForm : Window
    {
        static bool adminE = false;
        SqlConnection kapcsolat = new SqlConnection(ConfigurationManager.ConnectionStrings["filmekConnString"].ConnectionString);
        int id = 0;

        public FilmekForm(bool admin, string felhasznalo)
        {
            InitializeComponent();
            adminE = admin;
            lbFelhasznalo.Content = felhasznalo;

            if (!admin)
            {
                btnTorles.IsEnabled = false;
            }

            DataGridFeltoltese();
        }

        private void btnHozzaad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (kapcsolat.State == ConnectionState.Closed)
                {
                    kapcsolat.Open();
                }

                if (!string.IsNullOrEmpty(tbFilmCime.Text.Trim()) || !string.IsNullOrEmpty(tbMufaj.Text) || string.IsNullOrEmpty(tbRendezo.Text))
                {
                    string lekerdezes = "INSERT INTO Filmek(Cim, PremierDatuma, Mufaj, Rendezo) values('" + tbFilmCime.Text + "', '" + Convert.ToDateTime(dpPremierDatuma.Text).ToString("yyyy-MM-dd") + "', '" + tbMufaj.Text + "', '" + tbRendezo.Text + "');";

                    SqlCommand sqlCom = new SqlCommand(lekerdezes, kapcsolat);
                    sqlCom.CommandType = CommandType.Text;
                    sqlCom.ExecuteNonQuery();
                    TablaFrissites();
                    tbFilmCime.Text = "";
                    tbMufaj.Text = "";
                    tbRendezo.Text = "";
                }
                else
                {
                    MessageBox.Show("Adatok megadása kötelező.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                kapcsolat.Close();
            }
        }

        private void btnTorles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (kapcsolat.State == ConnectionState.Closed)
                {
                    kapcsolat.Open();
                }

                string lekerdezes = "DELETE FROM Filmek WHERE Id = '" + id + "'";

                if (MessageBox.Show("Biztos törli a kijelölt filmek?", "Törlés", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    SqlCommand sqlCom = new SqlCommand(lekerdezes, kapcsolat);
                    sqlCom.CommandType = CommandType.Text;
                    sqlCom.ExecuteNonQuery();
                    TablaFrissites();
                }
            }
            catch (AdatBazisKivetel)
            {
                MessageBox.Show("Sikertelen kapcsolat.", "Figyelem", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnModositas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (kapcsolat.State == ConnectionState.Closed)
                {
                    kapcsolat.Open();
                }

                if (!string.IsNullOrEmpty(tbFilmCime.Text.Trim()) || !string.IsNullOrEmpty(tbMufaj.Text) || string.IsNullOrEmpty(tbRendezo.Text))
                {
                    string lekerdezes = "UPDATE Filmek SET " +
                        "Cim = '" + tbFilmCime.Text + "', " +
                        "PremierDatuma = '" + Convert.ToDateTime(dpPremierDatuma.Text).ToString("yyyy-MM-dd") + "'," +
                        "Mufaj = '" + tbMufaj.Text + "'," +
                        "Rendezo = '" + tbRendezo.Text + "'" +
                        " WHERE Id = '" + id + "';";

                    SqlCommand sqlCom = new SqlCommand(lekerdezes, kapcsolat);
                    sqlCom.CommandType = CommandType.Text;
                    sqlCom.ExecuteNonQuery();
                    TablaFrissites();
                    tbFilmCime.Text = "";
                    tbMufaj.Text = "";
                    tbRendezo.Text = "";
                }
                else
                {
                    MessageBox.Show("Adatok megadása kötelező.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                kapcsolat.Close();
            }
        }

        private void DataGridFeltoltese()
        {
            kapcsolat = new SqlConnection(ConfigurationManager.ConnectionStrings["filmekConnString"].ConnectionString);

            try
            {
                if (kapcsolat.State == ConnectionState.Closed)
                {
                    kapcsolat.Open();
                }

                string lekerdezes = "SELECT * FROM Filmek";
                SqlCommand sqlCom = new SqlCommand(lekerdezes, kapcsolat);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCom);
                DataTable dt = new DataTable("Filmek");
                dataAdapter.Fill(dt);
                dgFilmek.ItemsSource = dt.DefaultView;
            }
            catch (AdatBazisKivetel ex)
            {
                MessageBox.Show(ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                kapcsolat.Close();
            }
        }

        private void TablaFrissites()
        {
            string lekerdezes = "SELECT * FROM Filmek";
            SqlCommand sqlCom = new SqlCommand(lekerdezes, kapcsolat);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCom);
            DataTable dt = new DataTable("Filmek");
            dataAdapter.Fill(dt);
            dgFilmek.ItemsSource = dt.DefaultView;
        }

        private void dgFilmek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            DataRowView sor = dg.SelectedItem as DataRowView;

            if (sor != null)
            {
                id = Convert.ToInt32(sor["Id"].ToString());
                tbFilmCime.Text = sor["Cim"].ToString();
                dpPremierDatuma.SelectedDate = Convert.ToDateTime(sor["PremierDatuma"].ToString());
                tbMufaj.Text = sor["Mufaj"].ToString();
                tbRendezo.Text = sor["Rendezo"].ToString();
            }
        }
    }
}
