using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyLogger.Models
{
    [Table("SystemLog")]
    public class ESystemLog
    {
        #region ID
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key]
        public decimal ID { get; set; }
        #endregion

        #region AppId
        public int AppId { get; set; }
        #endregion

        #region CreateDate
        private DateTime _CreateDate = new DateTime();
        public DateTime CreateDate
        {
            get { return _CreateDate; }
            set { _CreateDate = value; }
        }
        #endregion

        #region Log_Data
        private string _Log_Data = "";
        public string Log_Data
        {
            get { return _Log_Data; }
            set { _Log_Data = value; }
        }
        #endregion

        #region Proc
        public int Proc { get; set; }
        #endregion

        #region ProcDate
        private DateTime _ProcDate = new DateTime();
        public DateTime ProcDate
        {
            get { return _ProcDate; }
            set { _ProcDate = value; }
        }
        #endregion
    }
}
