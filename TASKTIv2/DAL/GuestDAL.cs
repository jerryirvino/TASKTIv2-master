using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Dapper;
using TASKTIv2.Models;

namespace TASKTIv2.DAL
{
    public class GuestDAL : IDisposable
    {
        private GuestModel db = new GuestModel();


        private string GetConnstr()
        {
            return ConfigurationManager.ConnectionStrings["GuestModel"].ConnectionString;
        }
        public IQueryable<DataGuest> GetData()
        {
            var results = from a in db.DataGuests
                          orderby a.NamaGuest
                          select a;
            return results;
        }

        public DataGuest GetDataById(int IdGuest)
        {
            var result = (from a in db.DataGuests
                          where a.IdGuest == IdGuest
                          select a).SingleOrDefault();

            return result;
        }


        public IQueryable<DataGuest> GetDataByUsername(string NameGuest)
        {   
            var result = from a in db.DataGuests
                         where a.NamaGuest == NameGuest
                         select a;

            return result;
        }
        public void Add(DataGuest obj)
        {
            try
            {
                db.DataGuests.Add(obj);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Edit(DataGuest obj)
        {
            var model = GetDataById(obj.IdGuest);

            if (model != null)
            {
                model.NamaGuest = obj.NamaGuest;
                model.Alamat = obj.Alamat;
                model.Ucapan = obj.Ucapan;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                throw new Exception("Data tidak ditemukan !");
            }
        }

        public void Delete(int IdGuest)
        {
            var model = GetDataById(IdGuest);
            if (model != null)
            {
                db.DataGuests.Remove(model);
                try
                {                    
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public IQueryable<DataGuest> Search(string txtsearch)
        {
            var results = from m in db.DataGuests
                          where m.NamaGuest.ToLower().Contains(txtsearch.ToLower())
                          select m;
            return results;

        }

        //public IEnumerable<DataGuest> Search(string txtsearch)
        //{
        //    using (SqlConnection conn = new SqlConnection(GetConnstr()))
        //    {
        //        string strsql = @"select * from DataGuest where NamaGuest='" + txtsearch + "'";


        //        return conn.Query<DataGuest>(strsql);
        //    }
        //}

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
