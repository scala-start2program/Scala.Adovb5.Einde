using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Scala.Adovb5.Core.Entities;
using Scala.Adovb5.Core.Services;

namespace Scala.Adovb5.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KachelService kachelService = new KachelService();
        bool isNew;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VulCombos();
            LinksActief();
            VulListbox();
        }

        private void LstKachels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearControls();
            if (lstKachels.SelectedItem != null)
            {
                Kachel kachel = (Kachel)lstKachels.SelectedItem;
                txtMerk.Text = kachel.Merk;
                txtSerie.Text = kachel.Serie;
                txtPrijs.Text = kachel.Prijs.ToString("#,##0.00");
                cmbSoort.SelectedValue = kachel.SoortId;
            }
        }

        private void CmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearControls();
            VulListbox();
        }

        private void BtnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();
            cmbFilter.SelectedIndex = -1;
            VulListbox();
        }

        private void BtnNieuw_Click(object sender, RoutedEventArgs e)
        {
            isNew = true;
            RechtsActief();
            ClearControls();
            cmbSoort.Focus();
        }

        private void BtnWijzig_Click(object sender, RoutedEventArgs e)
        {
            if (lstKachels.SelectedItem != null)
            {
                isNew = false;
                RechtsActief();
                cmbSoort.Focus();
            }
        }

        private void BtnVerwijder_Click(object sender, RoutedEventArgs e)
        {
            if (lstKachels.SelectedItem != null)
            {
                if (MessageBox.Show("Ben je zeker?", "Toestel verwijderen", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Kachel kachel = (Kachel)lstKachels.SelectedItem;
                    if (!kachelService.KachelVerwijderen(kachel))
                    {
                        MessageBox.Show("DB ERROR", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    ClearControls();
                    VulListbox();
                }
            }
        }

        private void BtnBewaren_Click(object sender, RoutedEventArgs e)
        {
            string merk = txtMerk.Text.Trim();
            string serie = txtSerie.Text.Trim();
            if (merk.Length == 0)
            {
                MessageBox.Show("Merk invoeren", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                txtMerk.Focus();
                return;
            }
            if (serie.Length == 0)
            {
                MessageBox.Show("Serie invoeren", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                txtSerie.Focus();
                return;
            }
            decimal.TryParse(txtPrijs.Text, out decimal prijs);
            if (prijs < 0)
            {
                MessageBox.Show("Negatieve prijzen verboden", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                txtPrijs.Focus();
                return;
            }
            if (cmbSoort.SelectedIndex == -1)
            {
                MessageBox.Show("Soort opgeven", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                cmbSoort.Focus();
                return;
            }
            Soort soort = (Soort)cmbSoort.SelectedItem;

            Kachel kachel;
            if (isNew)
            {
                kachel = new Kachel(soort.Id, merk, serie, prijs);
                if (!kachelService.KachelToevoegen(kachel))
                {
                    MessageBox.Show("DB ERROR", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                kachel = (Kachel)lstKachels.SelectedItem;
                kachel.Merk = merk;
                kachel.Serie = serie;
                kachel.Prijs = prijs;
                kachel.SoortId = soort.Id;
                if (!kachelService.KachelWijzigen(kachel))
                {
                    MessageBox.Show("DB ERROR", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            VulListbox();
            LinksActief();
            lstKachels.SelectedValue = kachel.Id;
            LstKachels_SelectionChanged(null, null);

        }

        private void BtnAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();
            LinksActief();
            LstKachels_SelectionChanged(null, null);
        }


        private void LinksActief()
        {
            grpKachels.IsEnabled = true;
            grpDetails.IsEnabled = false;
            btnBewaren.Visibility = Visibility.Hidden;
            btnAnnuleren.Visibility = Visibility.Hidden;
        }
        private void RechtsActief()
        {
            grpKachels.IsEnabled = false;
            grpDetails.IsEnabled = true;
            btnBewaren.Visibility = Visibility.Visible;
            btnAnnuleren.Visibility = Visibility.Visible;
        }
        private void VulListbox()
        {
            if (cmbFilter.SelectedIndex == -1)
                lstKachels.ItemsSource = kachelService.GetKachels();
            else
            {
                Soort soort = (Soort)cmbFilter.SelectedItem;
                lstKachels.ItemsSource = kachelService.GetKachels(soort);
            }
            lstKachels.Items.Refresh();
        }
        private void ClearControls()
        {
            txtMerk.Text = "";
            txtSerie.Text = "";
            txtPrijs.Text = "";
            cmbSoort.SelectedIndex = -1;
        }
        private void VulCombos()
        {
            cmbFilter.ItemsSource = kachelService.GetSoorten();
            cmbSoort.ItemsSource = kachelService.GetSoorten();
        }

        private void BtnSoorten_Click(object sender, RoutedEventArgs e)
        {
            WinSoorten winSoorten = new WinSoorten();
            winSoorten.ShowDialog();
            if(winSoorten.refreshen == true)
            {
                string selectedFilterSoortID = null;
                if (cmbFilter.SelectedItem != null)
                {
                    selectedFilterSoortID = cmbFilter.SelectedValue.ToString();
                }
                VulCombos(); 
                if(selectedFilterSoortID != null)
                {
                    cmbFilter.SelectedValue = selectedFilterSoortID;
                }
            }
        }
    }
}
