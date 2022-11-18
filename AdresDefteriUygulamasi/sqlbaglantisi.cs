using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace AdresDefteriUygulamasi
{
    class sqlbaglantisi
    {
        public MySqlConnection baglan()    //global bağlantı açtık.
        {
            MySqlConnection baglan1 = new MySqlConnection("Server = localhost; Database = adresdefteriuygulamasi; Uid = root; Pwd = 12345678"); //bağlantı kodları
            MySqlConnection.ClearPool(baglan1); //bağlantıyı temizleme
            try    //try catch
            {
                baglan1.Open();
            }
            catch { }
            return (baglan1);
        }
    }
}
