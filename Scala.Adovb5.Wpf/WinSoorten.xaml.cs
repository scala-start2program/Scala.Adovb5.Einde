using Scala.Adovb5.Core.Entities;
using Scala.Adovb5.Core.Services;
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
using System.Windows.Shapes;
using Scala.Adovb5.Core.Entities;
using Scala.Adovb5.Core.Services;

namespace Scala.Adovb5.Wpf
{
    /// <summary>
    /// Interaction logic for WinSoorten.xaml
    /// </summary>
    public partial class WinSoorten : Window
    {
        public bool refreshen = false;
        KachelService kachelService = new KachelService();
        bool isNew;
        public WinSoorten()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LinksActief();
            VulListbox();
        }

        private void LstSoorten_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearControls();
            if (lstSoorten.SelectedItem != null)
            {
                Soort soort = (Soort)lstSoorten.SelectedItem;
                txtSoortnaam.Text = soort.Soortnaam;
            }
        }

        private void BtnNieuw_Click(object sender, RoutedEventArgs e)
        {
            isNew = true;
            RechtsActief();
            ClearControls();
        }

        private void BtnWijzig_Click(object sender, RoutedEventArgs e)
        {
            if (lstSoorten.SelectedItem != null)
            {
                isNew = false;
                RechtsActief();
            }
        }

        private void BtnVerwijder_Click(object sender, RoutedEventArgs e)
        {
            if (lstSoorten.SelectedItem != null)
            {
                if (MessageBox.Show("Ben je zeker?", "Toestel verwijderen", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Soort soort = (Soort)lstSoorten.SelectedItem;
                    bool geslaagd = true;
                    try
                    {
                        geslaagd = kachelService.SoortVerwijderen(soort);
                    }
                    catch(Exception fout)
                    {
                        MessageBox.Show(fout.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (!geslaagd)
                    {
                        MessageBox.Show("DB ERROR", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    ClearControls();
                    VulListbox();
                    refreshen = true;
                }
            }
        }

        private void BtnBewaren_Click(object sender, RoutedEventArgs e)
        {
            string soortnaam = txtSoortnaam.Text.Trim();
            if (soortnaam.Length == 0)
            {
                MessageBox.Show("soortnaam invoeren", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                txtSoortnaam.Focus();
                return;
            }
            Soort soort;
            if (isNew)
            {
                soort = new Soort(soortnaam);
                bool geslaagd = true;
                try
                {
                    kachelService.SoortToevoegen(soort);
                }
                catch(Exception fout)
                {
                    MessageBox.Show(fout.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!geslaagd)
                {
                    MessageBox.Show("DB ERROR", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                soort = (Soort)lstSoorten.SelectedItem;
                soort.Soortnaam = soortnaam;
                bool geslaagd = true;
                try
                {
                    geslaagd = kachelService.SoortWijzigen(soort);
                }
                catch(Exception fout)
                {
                    MessageBox.Show(fout.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;

                }
                if (!geslaagd)
                {
                    MessageBox.Show("DB ERROR", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            VulListbox();
            LinksActief();
            lstSoorten.SelectedValue = soort.Id;
            LstSoorten_SelectionChanged(null, null);
            refreshen = true;

        }

        private void BtnAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();
            LinksActief();
            LstSoorten_SelectionChanged(null, null);
        }

        private void LinksActief()
        {
            grpSoorten.IsEnabled = true;
            grpDetails.IsEnabled = false;
            btnBewaren.Visibility = Visibility.Hidden;
            btnAnnuleren.Visibility = Visibility.Hidden;
        }
        private void RechtsActief()
        {
            grpSoorten.IsEnabled = false;
            grpDetails.IsEnabled = true;
            btnBewaren.Visibility = Visibility.Visible;
            btnAnnuleren.Visibility = Visibility.Visible;
        }
        private void VulListbox()
        {
            lstSoorten.ItemsSource = kachelService.GetSoorten();
            lstSoorten.Items.Refresh();
        }
        private void ClearControls()
        {
            txtSoortnaam.Text = "";
        }
    }
}
