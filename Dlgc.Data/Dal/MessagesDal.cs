using Dlgc.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Dlgc.Data.ErrXmlMap;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Dlgc.Data.DataEngine;
using Dlgc.Data.Core;
using System.Linq;
using SkyLogger;
using SkyLogger.SkyEnum;


namespace Dlgc.Data.Dal
{
    public class MessagesDal : IMessagesDal
    {
        private readonly MessageContext _ctx;
        private readonly ISysLogger _sysLoger;
        public MessagesDal(MessageContext ctx, ISysLogger sysLoger)
        {
            _ctx = ctx;
            _sysLoger = sysLoger;
        }
        private static readonly Random Getrandom = new Random();
        public async Task<Messages> GetMessageByIdAsync(long id,long sysHash)
        {
            try
            {
                //await _sysLoger.LogAsync(LogTipi.CoreGWSend, "GetMessageByIdAsync params ID{" + id + "},SysHash{" + sysHash + "}");
                var res = await _ctx.Messages.Where(x => x.ID == id && x.sysHash == sysHash.ToString()).FirstOrDefaultAsync();
                //await _sysLoger.LogAsync(LogTipi.CoreGWSend, "GetMessageByIdAsync res: ID{" + res.ID + "}, SysHash{" + res.sysHash + "}");
                return res;
            }
            catch (Exception ex)
            {
                await _sysLoger.LogAsync(LogTipi.CoreGWErr, "GetMessageByIdAsync:" + ex.Message);
                throw;
            }
        }
        public async Task<bool> LockMessageAsync(Messages mes)
        {

            //Mesaj bloklu değil ise
            if (!mes.sysLock)
            {
                mes.sysLock = true;
                //int x = ctx.Messages.Update(mes);
                int x = await _ctx.SaveChangesAsync();
                return x > 0 ? true : false;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> UpdateMessageModifierAsync(long ID,int modifier,long sysHash)
        {
            var mess = await GetMessageByIdAsync(ID, sysHash);
            mess.Modifier = modifier;
            int z = await _ctx.SaveChangesAsync();
            if (z > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> UpdateSuccessMessageAsync(long mesId,long sysHash)
        {
            var mess = await GetMessageByIdAsync(mesId, sysHash);
            mess.StatusDetailsID = 221;
            mess.StatusID = 2;
            mess.LastUpdate = DateTime.Now;
            mess.SentTime = DateTime.Now;
            mess.Modifier = 4;
            int z =await _ctx.SaveChangesAsync();
            if (z > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public  async Task<bool> UpdateErrorMessageAsync(ErrorMessage erMessage,long syshash)
        {
            var mess = await GetMessageByIdAsync(erMessage.ID,syshash);
            mess.StatusID = 3;
            mess.StatusDetailsID = 210;
            mess.CustomField = erMessage.code.MapErrorCode;

            if (string.IsNullOrEmpty(mess.Trace))
            {
                mess.Trace = mess.Trace + "-"+ DateTime.Now + " " + erMessage.Text;
            }
            else
            {
                mess.Trace =DateTime.Now + " " + erMessage.Text;
            }
                
            mess.LastUpdate = DateTime.Now;
            mess.Modifier = 4;
            int z = await _ctx.SaveChangesAsync();
            if (z > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }



        /// <summary>
        /// İlk gönderim öncesi mesajları update eder ve listeyi alır.
        /// Böylece mesajın yeniden alınması engellenir.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MesId>> GetHemenSmsAsync(long sysHash, int bodyFormat)
        {
            if (await GetHemenSmsFirstUpdateAsync(sysHash, bodyFormat))
            {
                //await _sysLoger.LogAsync(LogTipi.CoreGWSend, "GetHemenSmsAsync (GetHemenSmsFirstUpdateAsync : TRUE)");
                var list =  await GetMessageListByHashCodeAsync(sysHash, bodyFormat);
                //await _sysLoger.LogAsync(LogTipi.CoreGWSend, "GetHemenSmsAsync (GetMessageListByHashCodeAsync count :"+list.Count());
                return list;
            }
            else
            {
                return null;
            }
        }

        private async Task<bool> GetHemenSmsFirstUpdateAsync(long syshascode, int bodyFormat)
        {
            try
            {
                var x=  await _ctx.Database.ExecuteSqlCommandAsync("spGetHemenMesajListFirstUpdateCore @sysHash={0}, @bodyFormat={1}", syshascode, bodyFormat );
                return true;
            }
            catch (Exception ex)
            {
                await _sysLoger.LogAsync(LogTipi.CoreGWErr, "GetHemenSmsFirstUpdateAsync:" + ex.Message);
                return false;
                //throw;
            }
        }
        private async Task<IEnumerable<MesId>> GetMessageListByHashCodeAsync(long syshascode, int bodyFormat)
        {
            try
            {
                var list = await _ctx.Messages.Where(i=> i.StatusID == 1
                                  && !i.SentTime.HasValue
                                  && i.sysLock
                                  && i.Modifier == 0
                                  && (!i.ScheduledTime.HasValue && !i.EndTime.HasValue)
                                  && i.sysHash == syshascode.ToString()
                                  && i.ReadReceipt
                                  && i.BodyFormatID == bodyFormat).OrderByDescending(z => z.Priority).ThenBy(n => n.SentTime).Take(40)
                                  .Select(z => new MesId
                                  {
                                      ID = z.ID
                                  }).ToArrayAsync();
                //await _sysLoger.LogAsync(LogTipi.CoreGWSend, "GetMessageListByHashCodeAsync list Count{"+list.Count() + ": syshascode{" + syshascode + "}, bodyFormat{" + bodyFormat + "}");
                if (list != null)
                {
                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                await _sysLoger.LogAsync(LogTipi.CoreGWErr, "GetMessageListByHashCodeAsync:" + ex.Message);
                return null;
            }
         
        }

        //SqlParameter parameterS = new SqlParameter("@sysHash", syshascode);
        //SqlParameter parameterF = new SqlParameter("@bodyFormat", bodyFormat);
        ////var r = await _ctx.Messages.FromSql("spGetHemenMesajListByHashTEST @sysHash, @bodyFormat", parameterS, parameterF).AsNoTracking().ToArrayAsync();//.ToArrayAsync();
        //var r = await _ctx.Messages.FromSql("spGetHemenMesajListByHashCore @sysHash, @bodyFormat", parameterS, parameterF)
        //                                       .Select(z => new MesId
        //                                       {
        //                                           ID = z.ID
        //                                       }).ToArrayAsync();


        #region TEST
        private Messages FillSMSFromReader(SqlDataReader reader)
        {
            var val = new Messages { ID = Converter.ToInt32(reader["ID"]) };

            if (reader["ChannelID"] != DBNull.Value)
                val.ChannelID = Converter.ToInt32(reader["ChannelID"]);
            if (reader["ScheduledTime"] != DBNull.Value)
                val.ScheduledTime = Converter.ToDateTime(reader["ScheduledTime"]);
            if (reader["EndTime"] != DBNull.Value)
                val.EndTime = Converter.ToDateTime(reader["EndTime"]);
            if (reader["Body"] != DBNull.Value)
                val.Body = Converter.ToString(reader["Body"]);
            if (reader["CharsetID"] != DBNull.Value)
                val.CharsetID = Converter.ToInt32(reader["CharsetID"]);
            if (reader["Modifier"] != DBNull.Value)
                val.Modifier = Converter.ToInt32(reader["Modifier"]);
            if (reader["sysLock"] != DBNull.Value)
                val.sysLock = Converter.ToBool(reader["sysLock"]);
            if (reader["FromAddress"] != DBNull.Value)
                val.FromAddress = Converter.ToString(reader["FromAddress"]);
            if (reader["sysHash"] != DBNull.Value)
                val.sysHash = Converter.ToString(reader["sysHash"]);
            if (reader["ToAddress"] != DBNull.Value)
                val.ToAddress = Converter.ToString(reader["ToAddress"]);
            if (reader["GWID"] != DBNull.Value)
                val.GWID = Converter.ToInt64(reader["GWID"]);

            return val;
        }
        private async Task<bool> GetHemenSmsFirstUpdateAsyncDENEME(long syshascode, int bodyFormat)
        {
            DataCommandAsync _cmd = new DataCommandAsync("spGetHemenMesajListFirstUpdateTEST", CommandType.StoredProcedure, _ctx);

            //DbCommand cmd = _ctx.Database.GetDbConnection().CreateCommand();
            try
            {
                _cmd.AddParameter("@sysHash", syshascode);
                _cmd.AddParameter("@bodyFormat", bodyFormat);
                await _cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                _cmd.Close();
            }
            
            #region OLd 
            //13 DialogicDSISWS
            //var cmd = new Commands("spGetHemenMesajListFirstUpdate", CommandType.StoredProcedure, 13); //13 DialogicDSISWS
            //try
            //{
            //    cmd.AddParameter("@sysHash", syshascode);
            //    cmd.AddParameter("@bodyFormat", bodyFormat);
            //    var resultupd = cmd.ExecuteNonQuery();
            //    return true;
            //}
            //catch (Exception exception)
            //{
            //    //Logger.LogFile(LogTipi.SmsGateWayHata, "GetHemenSMS spGetHemenMesajListFirstUpdate Hata :" + exception.Message);
            //    return false;
            //}
            //finally
            //{
            //    cmd.Close();
            //}
            #endregion

        }
        private async Task<List<Messages>> GetMessageListByHashCodeAsyncDENEME(long syshascode, int bodyFormat)
        {
            var list = new List<Messages>();
            DataCommandAsync _cmd = new  DataCommandAsync("spGetHemenMesajListByHashTEST", CommandType.StoredProcedure, _ctx);
            try
            {
                _cmd.AddParameter("@sysHash", syshascode);
                _cmd.AddParameter("@bodyFormat", bodyFormat);
                var reader =await _cmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    list.Add(FillSMSFromReader(reader));
                }
                return list;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                _cmd.Close();
            }

            #region OLD CODE
            //int z = _ctx.Database.ExecuteSqlCommand("spGetHemenMesajListByHash @sysHash @bodyFormat", parameters: new[] { hashCode, bodyFormat }).ToL;
            //var list = new List<EDialogicSms>();
            //var cmd2 = new Commands("spGetHemenMesajListByHash", CommandType.StoredProcedure, 13);
            //try
            //{
            //    cmd2.AddParameter("@sysHash", hashCode);
            //    cmd2.AddParameter("@bodyFormat", bodyFormat);
            //    var reader = cmd2.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        list.Add(FillDialogicSMSFromReader(reader));
            //    }
            //    reader.Dispose();
            //    reader.Close();
            //}
            //catch (Exception exception)
            //{
            //    Logger.LogFile(LogTipi.SmsGateWayHata,
            //        "GetHemenSMS spGetHemenMesajListByHash Hata :" + exception.Message);
            //    return list;
            //}
            //finally
            //{
            //    cmd2.Close();
            //}
            //return list;
            #endregion
        }
        #endregion
    }
}
