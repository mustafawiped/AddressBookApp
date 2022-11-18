using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace AdresDefteriUygulamasi
{
    public partial class AdresDefteri : Form
    {
        public AdresDefteri()
        {
            InitializeComponent();
        }
        sqlbaglantisi baglanti = new sqlbaglantisi();         //sınıfı tanımladık.
        MySqlCommand komut = new MySqlCommand();              //mysql komutları kullanacağımız için mysqlcommand tanımladık
        DataTable tablo = new DataTable();                    //veri tablosu tanımladık.

        private void AdresDefteri_Load(object sender, EventArgs e)
        {
            kayityenile();   //kayıt yenileme methodunu çağırdık
        }

        private void KayitEklebtn_Click(object sender, EventArgs e)
        {
            if (KayitAditxt.Text == "" || KayitAdrestxt.Text == "" || KayitSoyadtxt.Text == "" || KayitTeltxt.Text == "")
            {
                MessageBox.Show("lütfen boş yer bırakmayın."); //hata mesajı verdi
            }
            else
            {
                try
                {
                    string sorgu = "INSERT INTO kayitlar(telno, adi, soyadi, adres) VALUES(@telno, @adi, @soyadi, @adres)";  //kayıt ekleme için sorgu cümlemizi yazdık.
                    komut = new MySqlCommand(sorgu, baglanti.baglan());   //sorgu komutunu kullandık ve bağlantı açtık.
                    komut.Parameters.AddWithValue("@telno", KayitTeltxt.Text.ToString());       //parametrelere değer atadık.
                    komut.Parameters.AddWithValue("@adi", KayitAditxt.Text);                    //parametrelere değer atadık.
                    komut.Parameters.AddWithValue("@soyadi", KayitSoyadtxt.Text);               //parametrelere değer atadık.
                    komut.Parameters.AddWithValue("@adres", KayitAdrestxt.Text.ToString());     //parametrelere değer atadık.
                    komut.Connection = baglanti.baglan();            //tekrar bağlantı açtık
                    komut.ExecuteNonQuery();                         //parametre kodlarını çalıştırdık.
                    komut.Connection.Close();                        //açtığımız bağlantıyı kapattık
                    MessageBox.Show("Başarıyla Yeni Kayıt Eklendi!");//onay mesajı.
                    kayityenile();                                   //eklediğimiz yeni kişiyi datagrid de göstermek için kayıtların hepsini yeniledik.
                }
                catch
                {
                    MessageBox.Show("Kayıt eklenemedi, Lütfen tekrar deneyin.");
                }
            }
        }
        private void kayityenile()
        {
            dtgwListe.RowHeadersVisible = false;          //data gridin row headers visible özelliğini kapattık.
            tablo.Columns.Clear();                           //tablomuzun kolonlarını temizledik
            tablo.Clear();                                   //tablonun verilerini temizledik
            MySqlDataAdapter veri = new MySqlDataAdapter("SELECT * FROM kayitlar", baglanti.baglan()); //veri tabanındaki tüm kayıtları seçtirdik ve bağlantı açtık
            veri.Fill(tablo);                             //seçilen tüm verileri tablo ya yazdırdık.
            dtgwListe.DataSource = tablo;                 //tablodaki tüm verileri data grid e yazdırdık.
            dtgwListe.Columns["adi"].HeaderText = "İsim"; //data griddeki verilerin başlıklarını ayarladık
            dtgwListe.Columns["soyadi"].HeaderText = "Soyisim"; //data griddeki verilerin başlıklarını ayarladık
            dtgwListe.Columns["telno"].HeaderText = "Telefon Numarası"; //data griddeki verilerin başlıklarını ayarladık
            dtgwListe.Columns["adres"].HeaderText = "Adres"; //data griddeki verilerin başlıklarını ayarladık
        }

        private void KayitSilbtn_Click(object sender, EventArgs e)
        {
            try
            {
                komut.Connection = baglanti.baglan();   //bağlantı açtık.
                komut.CommandText = "DELETE FROM kayitlar WHERE telno = '" + dtgwListe.CurrentRow.Cells["telno"].Value.ToString() + "'";  //kayıt silme komutlarını girdik
                komut.ExecuteNonQuery();    //girdiğimiz komudu çalıştırdık
                komut.Connection.Close();   //bağlantıyı kapattık
                MessageBox.Show("Kayıt Başarıyla Silindi."); //onay mesajı verdik
                kayityenile();               //silinen kaydı görmemek için kayıtları yeniledik.
            }
            catch
            {
                MessageBox.Show("Kayıt silinemedi, lütfen tekrar deneyiniz.");
            }
        }

        private void KayitAramatxt_TextChanged(object sender, EventArgs e)
        {
            DataTable tablo2 = new DataTable();         //2. bir veri tablosu tanımladık
            tablo2 = tablo.Copy();                 //mevcut verilerimizi 2. tabloya kopyaladık
            DataView goster = tablo2.DefaultView;  //veri gösterme komutuna gösterdik 
            goster.RowFilter = "adi LIKE '%" + KayitAramatxt.Text + "%' OR soyadi LIKE '%"+KayitAramatxt.Text+"%'"; //arama işlemi
            dtgwListe.DataSource = goster;     //data gridde arananı gösterdik (:
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dtgwListe_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            KayitTeltxt.Text = dtgwListe.Rows[e.RowIndex].Cells[0].Value.ToString();
            KayitAditxt.Text = dtgwListe.Rows[e.RowIndex].Cells[1].Value.ToString();        //data gride 2 kez tıklandığında seçmiş olduğumuz kişinin verilerini textbox 'lara yazdırdık.
            KayitSoyadtxt.Text = dtgwListe.Rows[e.RowIndex].Cells[2].Value.ToString();
            KayitAdrestxt.Text = dtgwListe.Rows[e.RowIndex].Cells[3].Value.ToString();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try {   
                string sorgu = "UPDATE kayitlar SET telno = @telno, adi = @adi, soyadi = @soyadi, adres = @adres WHERE id = @id";
            komut = new MySqlCommand(sorgu, baglanti.baglan());  //güncelleme komutlarını yazdık ve bağlantı açtık
            komut.Parameters.AddWithValue("@id", dtgwListe.CurrentRow.Cells["id"].Value.ToString());
            komut.Parameters.AddWithValue("@telno", KayitTeltxt.Text);    
            komut.Parameters.AddWithValue("@adi", KayitAditxt.Text);            //parametrelere yeni girilen değerleri ekledik
            komut.Parameters.AddWithValue("@soyadi", KayitSoyadtxt.Text);
            komut.Parameters.AddWithValue("@adres", KayitAdrestxt.Text);
            komut.Connection = baglanti.baglan();   //tekrar bağlantı açtık
            komut.ExecuteNonQuery();                //yazdığımız komutları çalıştırdık
            komut.Connection.Close();               //bağlantıyı kapattık
            kayityenile();                          //kayıtları yeniledik
            MessageBox.Show("Başarıyla Kayıt Güncellendi!"); //onay mesajı
        }
            catch
            {
                MessageBox.Show("Hata! lütfen tekrar dene.");
            }
        }
    }
}
